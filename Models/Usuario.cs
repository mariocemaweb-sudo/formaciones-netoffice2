using System.ComponentModel.DataAnnotations;

namespace FormacionesApp.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La empresa es obligatoria")]
        [StringLength(100)]
        public string Empresa { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = "Usuario"; // Admin o Usuario

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? UltimoAcceso { get; set; }

        public bool Activo { get; set; } = true;
    }
}
