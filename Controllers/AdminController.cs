using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormacionesApp.Data;
using FormacionesApp.Models;

namespace FormacionesApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalFormaciones = await _context.Formaciones.CountAsync();
            ViewBag.TotalVideos = await _context.Videos.CountAsync();
            ViewBag.TotalArchivos = await _context.Archivos.CountAsync();
            ViewBag.TotalUsuarios = await _context.Usuarios.CountAsync();
            ViewBag.TotalEmpresas = await _context.Usuarios
                .Where(u => !string.IsNullOrEmpty(u.Empresa))
                .Select(u => u.Empresa)
                .Distinct()
                .CountAsync();

            var formaciones = await _context.Formaciones
                .Include(f => f.Videos)
                .Include(f => f.Archivos)
                .OrderByDescending(f => f.FechaCreacion)
                .ToListAsync();

            return View(formaciones);
        }

        // GESTIÓN DE FORMACIONES
        [HttpGet]
        public IActionResult CrearFormacion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearFormacion(Formacion formacion)
        {
            if (ModelState.IsValid)
            {
                _context.Formaciones.Add(formacion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Formación creada exitosamente";
                return RedirectToAction("EditarFormacion", new { id = formacion.Id });
            }
            return View(formacion);
        }

        [HttpGet]
        public async Task<IActionResult> EditarFormacion(int id)
        {
            var formacion = await _context.Formaciones
                .Include(f => f.Videos.OrderBy(v => v.Orden))
                .Include(f => f.Archivos)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (formacion == null)
            {
                return NotFound();
            }

            return View(formacion);
        }

        [HttpPost]
        public async Task<IActionResult> EditarFormacion(Formacion formacion)
        {
            if (ModelState.IsValid)
            {
                _context.Update(formacion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Formación actualizada exitosamente";
                return RedirectToAction("EditarFormacion", new { id = formacion.Id });
            }
            return View(formacion);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarFormacion(int id)
        {
            var formacion = await _context.Formaciones
                .Include(f => f.Videos)
                .Include(f => f.Archivos)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (formacion == null)
            {
                return NotFound();
            }

            // Eliminar archivos físicos
            foreach (var video in formacion.Videos)
            {
                EliminarArchivoFisico(video.RutaArchivo);
            }

            foreach (var archivo in formacion.Archivos)
            {
                EliminarArchivoFisico(archivo.RutaArchivo);
            }

            _context.Formaciones.Remove(formacion);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Formación eliminada exitosamente";
            return RedirectToAction("Index");
        }

        // GESTIÓN DE VIDEOS
        [HttpPost]
        public async Task<IActionResult> SubirVideo(int formacionId, IFormFile videoFile, string titulo, string? descripcion)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                TempData["Error"] = "Debe seleccionar un archivo de video";
                return RedirectToAction("EditarFormacion", new { id = formacionId });
            }

            var allowedExtensions = _configuration.GetSection("AllowedVideoExtensions").Get<string[]>();
            var extension = Path.GetExtension(videoFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] = $"Extensión no permitida. Solo se permiten: {string.Join(", ", allowedExtensions)}";
                return RedirectToAction("EditarFormacion", new { id = formacionId });
            }

            var maxFileSize = _configuration.GetValue<long>("MaxFileSize");
            if (videoFile.Length > maxFileSize)
            {
                TempData["Error"] = $"El archivo excede el tamaño máximo permitido ({maxFileSize / 1024 / 1024} MB)";
                return RedirectToAction("EditarFormacion", new { id = formacionId });
            }

            try
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "videos");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }

                var video = new Video
                {
                    FormacionId = formacionId,
                    Titulo = titulo,
                    Descripcion = descripcion,
                    RutaArchivo = $"/uploads/videos/{uniqueFileName}",
                    NombreArchivo = videoFile.FileName,
                    TamanoBytes = videoFile.Length,
                    Orden = await _context.Videos.Where(v => v.FormacionId == formacionId).CountAsync()
                };

                _context.Videos.Add(video);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Video subido exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al subir el video: {ex.Message}";
            }

            return RedirectToAction("EditarFormacion", new { id = formacionId });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            EliminarArchivoFisico(video.RutaArchivo);

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Video eliminado exitosamente";
            return RedirectToAction("EditarFormacion", new { id = video.FormacionId });
        }

        // GESTIÓN DE ARCHIVOS
        [HttpPost]
        public async Task<IActionResult> SubirArchivo(int formacionId, IFormFile archivoFile, string titulo, string? descripcion)
        {
            if (archivoFile == null || archivoFile.Length == 0)
            {
                TempData["Error"] = "Debe seleccionar un archivo";
                return RedirectToAction("EditarFormacion", new { id = formacionId });
            }

            var allowedExtensions = _configuration.GetSection("AllowedFileExtensions").Get<string[]>();
            var extension = Path.GetExtension(archivoFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] = $"Extensión no permitida. Solo se permiten: {string.Join(", ", allowedExtensions)}";
                return RedirectToAction("EditarFormacion", new { id = formacionId });
            }

            var maxFileSize = _configuration.GetValue<long>("MaxFileSize");
            if (archivoFile.Length > maxFileSize)
            {
                TempData["Error"] = $"El archivo excede el tamaño máximo permitido ({maxFileSize / 1024 / 1024} MB)";
                return RedirectToAction("EditarFormacion", new { id = formacionId });
            }

            try
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "archivos");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await archivoFile.CopyToAsync(stream);
                }

                var archivo = new Archivo
                {
                    FormacionId = formacionId,
                    Titulo = titulo,
                    Descripcion = descripcion,
                    RutaArchivo = $"/uploads/archivos/{uniqueFileName}",
                    NombreArchivo = archivoFile.FileName,
                    TamanoBytes = archivoFile.Length,
                    TipoArchivo = extension
                };

                _context.Archivos.Add(archivo);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Archivo subido exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al subir el archivo: {ex.Message}";
            }

            return RedirectToAction("EditarFormacion", new { id = formacionId });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarArchivo(int id)
        {
            var archivo = await _context.Archivos.FindAsync(id);

            if (archivo == null)
            {
                return NotFound();
            }

            EliminarArchivoFisico(archivo.RutaArchivo);

            _context.Archivos.Remove(archivo);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Archivo eliminado exitosamente";
            return RedirectToAction("EditarFormacion", new { id = archivo.FormacionId });
        }

        // GESTIÓN DE USUARIOS
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _context.Usuarios
                .OrderByDescending(u => u.FechaCreacion)
                .ToListAsync();
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> CrearUsuario()
        {
            // Obtener lista de empresas existentes
            ViewBag.Empresas = await _context.Usuarios
                .Where(u => !string.IsNullOrEmpty(u.Empresa))
                .Select(u => u.Empresa)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                {
                    ModelState.AddModelError("Email", "Ya existe un usuario con este email");
                    return View(usuario);
                }

                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Usuario creado exitosamente";
                return RedirectToAction("Usuarios");
            }
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            // No permitir eliminar el admin principal
            if (usuario.Email == "admin@formaciones.com")
            {
                TempData["Error"] = "No se puede eliminar el usuario administrador principal";
                return RedirectToAction("Usuarios");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Usuario eliminado exitosamente";
            return RedirectToAction("Usuarios");
        }

        // LOGS DE ACCESO
        public async Task<IActionResult> LogsAcceso()
        {
            var logs = await _context.AccesosLog
                .OrderByDescending(l => l.FechaAcceso)
                .Take(100)
                .ToListAsync();
            return View(logs);
        }

        // ESTADÍSTICAS POR EMPRESA
        public async Task<IActionResult> EstadisticasEmpresas()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // MÉTODO AUXILIAR
        private void EliminarArchivoFisico(string rutaRelativa)
        {
            try
            {
                var rutaCompleta = Path.Combine(_webHostEnvironment.WebRootPath, rutaRelativa.TrimStart('/'));
                if (System.IO.File.Exists(rutaCompleta))
                {
                    System.IO.File.Delete(rutaCompleta);
                }
            }
            catch
            {
                // Silenciosamente fallar si no se puede eliminar
            }
        }
    }
}
