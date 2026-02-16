using System.ComponentModel.DataAnnotations;

namespace FormacionesApp.Models
{
    public class Formacion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        public string? Categoria { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

        // Relaciones
        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public ICollection<Archivo> Archivos { get; set; } = new List<Archivo>();
    }
}
