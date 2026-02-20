using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using FormacionesApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar base de datos - Soporta SQLite (desarrollo) y PostgreSQL (Render production)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

// Determinar si usar PostgreSQL o SQLite
bool usePostgres = !string.IsNullOrEmpty(databaseUrl) || (builder.Environment.IsProduction() && !string.IsNullOrEmpty(connectionString) && connectionString.Contains("postgres"));

if (usePostgres && !string.IsNullOrEmpty(databaseUrl))
{
    // Usar PostgreSQL en producción (Render.com) - DATABASE_URL de Render
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(databaseUrl, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 5);
        }));
}
else if (usePostgres && !string.IsNullOrEmpty(connectionString))
{
    // Usar PostgreSQL con string de conexión configurado
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 5);
        }));
}
else
{
    // Usar SQLite en desarrollo local (por defecto)
    var sqliteConnection = string.IsNullOrEmpty(connectionString) || connectionString.Contains("SQLite") || connectionString.Contains("sqlite") 
        ? connectionString 
        : "Data Source=formaciones.db";
    
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(sqliteConnection));
}

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Configurar límites de tamaño para carga de archivos
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = 524288000; // 500 MB
});

// Configurar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar URLs para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
builder.WebHost.UseUrls($"http://+:{port}");

var app = builder.Build();

// Crear base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        
        // Crear usuario admin por defecto si no existe
        if (!context.Usuarios.Any(u => u.Email == "admin@formaciones.com"))
        {
            context.Usuarios.Add(new FormacionesApp.Models.Usuario
            {
                Email = "admin@formaciones.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Nombre = "Administrador",
                Empresa = "Administración",
                Rol = "Admin",
                FechaCreacion = DateTime.Now,
                Activo = true
            });
            context.SaveChanges();
        }
        
        // Crear usuario normal de prueba si no existe
        if (!context.Usuarios.Any(u => u.Email == "usuario@formaciones.com"))
        {
            context.Usuarios.Add(new FormacionesApp.Models.Usuario
            {
                Email = "usuario@formaciones.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Usuario123!"),
                Nombre = "Usuario de Prueba",
                Empresa = "Netoffice",
                Rol = "Usuario",
                FechaCreacion = DateTime.Now,
                Activo = true
            });
            context.SaveChanges();
        }
        
        // Crear formación de ejemplo si no existe
        if (!context.Formaciones.Any(f => f.Titulo == "Introducción a NetOffice"))
        {
            var formacion = new FormacionesApp.Models.Formacion
            {
                Titulo = "Introducción a NetOffice",
                Descripcion = "Módulo de formación que cubre los conceptos básicos de utilización del software NetOffice para la gestión de transporte.",
                Categoria = "Conceptos Básicos",
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            
            context.Formaciones.Add(formacion);
            context.SaveChanges();
            
            // Crear videos de ejemplo sin archivos reales (solo referencias)
            var video1 = new FormacionesApp.Models.Video
            {
                Titulo = "Video 1: Introducción al Sistema",
                Descripcion = "Visión general de la plataforma NetOffice",
                FormacionId = formacion.Id,
                RutaArchivo = "/uploads/videos/sample-video-1.mp4",
                NombreArchivo = "sample-video-1.mp4",
                TamanoBytes = 1024000,
                FechaSubida = DateTime.Now,
                Orden = 1
            };
            
            var video2 = new FormacionesApp.Models.Video
            {
                Titulo = "Video 2: Configuración Inicial",
                Descripcion = "Pasos para configurar tu primera formación",
                FormacionId = formacion.Id,
                RutaArchivo = "/uploads/videos/sample-video-2.mp4",
                NombreArchivo = "sample-video-2.mp4",
                TamanoBytes = 1536000,
                FechaSubida = DateTime.Now,
                Orden = 2
            };
            
            context.Videos.AddRange(video1, video2);
            
            // Crear archivos de ejemplo sin archivos reales (solo referencias)
            var archivo1 = new FormacionesApp.Models.Archivo
            {
                Titulo = "Guía Rápida de Inicio",
                Descripcion = "Manual de inicio rápido para nuevos usuarios",
                FormacionId = formacion.Id,
                RutaArchivo = "/uploads/archivos/guia-rapida.txt",
                NombreArchivo = "guia-rapida.txt",
                TipoArchivo = ".txt",
                TamanoBytes = 3072,
                FechaSubida = DateTime.Now
            };
            
            var archivo2 = new FormacionesApp.Models.Archivo
            {
                Titulo = "Documentación Completa",
                Descripcion = "Documentación técnica completa del sistema",
                FormacionId = formacion.Id,
                RutaArchivo = "/uploads/archivos/documentacion.txt",
                NombreArchivo = "documentacion.txt",
                TipoArchivo = ".txt",
                TamanoBytes = 5120,
                FechaSubida = DateTime.Now
            };
            
            context.Archivos.AddRange(archivo1, archivo2);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al crear la base de datos.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS redirection solo en desarrollo local, not en Railway (maneja reverse proxy)
if (app.Environment.IsDevelopment() || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT")))
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
