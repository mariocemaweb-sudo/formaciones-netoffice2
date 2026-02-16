using Microsoft.EntityFrameworkCore;
using FormacionesApp.Models;

namespace FormacionesApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Formacion> Formaciones { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<AccesoLog> AccesosLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones
            modelBuilder.Entity<Formacion>()
                .HasMany(f => f.Videos)
                .WithOne(v => v.Formacion)
                .HasForeignKey(v => v.FormacionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Formacion>()
                .HasMany(f => f.Archivos)
                .WithOne(a => a.Formacion)
                .HasForeignKey(a => a.FormacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AccesoLog>()
                .HasIndex(a => a.FechaAcceso);
        }
    }
}
