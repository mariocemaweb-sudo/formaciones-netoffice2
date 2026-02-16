using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FormacionesApp.Data;
using FormacionesApp.Models;

namespace FormacionesApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Activo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
            {
                // Registrar intento fallido
                await RegistrarAcceso(model.Email, false);
                ModelState.AddModelError("", "Email o contraseña incorrectos");
                return View(model);
            }

            // Actualizar último acceso
            usuario.UltimoAcceso = DateTime.Now;
            await _context.SaveChangesAsync();

            // Registrar acceso exitoso
            await RegistrarAcceso(model.Email, true, usuario.Id);

            // Crear claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RecordarMe,
                ExpiresUtc = model.RecordarMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (usuario.Rol == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Formaciones");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task RegistrarAcceso(string email, bool exitoso, int? usuarioId = null)
        {
            var log = new AccesoLog
            {
                UsuarioId = usuarioId ?? 0,
                Email = email,
                FechaAcceso = DateTime.Now,
                DireccionIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                ExitosoAcceso = exitoso,
                Navegador = HttpContext.Request.Headers["User-Agent"].ToString()
            };

            _context.AccesosLog.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
