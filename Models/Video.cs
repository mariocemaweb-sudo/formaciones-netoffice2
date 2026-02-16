using System.ComponentModel.DataAnnotations;

namespace FormacionesApp.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Required]
        public int FormacionId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required]
        public string RutaArchivo { get; set; } = string.Empty;

        public string? NombreArchivo { get; set; }

        public long TamanoBytes { get; set; }

        public int Orden { get; set; } = 0;

        public DateTime FechaSubida { get; set; } = DateTime.Now;

        // Relación
        public Formacion Formacion { get; set; } = null!;
    }
}
