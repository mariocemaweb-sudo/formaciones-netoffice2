using System.ComponentModel.DataAnnotations;

namespace FormacionesApp.Models
{
    public class AccesoLog
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        public DateTime FechaAcceso { get; set; } = DateTime.Now;

        public string? DireccionIP { get; set; }

        public bool ExitosoAcceso { get; set; } = true;

        public string? Navegador { get; set; }
    }
}
