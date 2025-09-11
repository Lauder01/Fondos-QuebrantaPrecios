using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    /// <summary>
    /// DTO para la subida de imágenes
    /// </summary>
    public class ImageUploadDto
    {
        /// <summary>
        /// ID del edificio al que pertenece la imagen
        /// </summary>
        [Required]
        public string BuildingId { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del archivo (opcional, se usará el nombre original si no se especifica)
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Texto alternativo para accesibilidad (opcional)
        /// </summary>
        public string? AltText { get; set; }

        /// <summary>
        /// Archivo de imagen a subir
        /// </summary>
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
