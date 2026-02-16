using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormacionesApp.Data;

namespace FormacionesApp.Controllers
{
    [Authorize]
    public class FormacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var formaciones = await _context.Formaciones
                .Include(f => f.Videos)
                .Include(f => f.Archivos)
                .Where(f => f.Activo)
                .OrderByDescending(f => f.FechaCreacion)
                .ToListAsync();

            return View(formaciones);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var formacion = await _context.Formaciones
                .Include(f => f.Videos.OrderBy(v => v.Orden))
                .Include(f => f.Archivos)
                .FirstOrDefaultAsync(f => f.Id == id && f.Activo);

            if (formacion == null)
            {
                return NotFound();
            }

            return View(formacion);
        }

        public async Task<IActionResult> DescargarArchivo(int id)
        {
            var archivo = await _context.Archivos.FindAsync(id);

            if (archivo == null)
            {
                return NotFound();
            }

            var rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", archivo.RutaArchivo.TrimStart('/'));

            if (!System.IO.File.Exists(rutaCompleta))
            {
                return NotFound();
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(rutaCompleta);
            return File(bytes, "application/octet-stream", archivo.NombreArchivo);
        }
    }
}
