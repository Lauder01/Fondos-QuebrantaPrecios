using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingGetterDto : BuildingBaseDto
    {
        [Required(ErrorMessage = "ERR014: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el edificio tiene imágenes
        /// </summary>
        public bool HasImages { get; set; } = false;

        /// <summary>
        /// ID de la imagen de portada si existe
        /// </summary>
        public string? CoverImageId { get; set; }

        /// <summary>
        /// URL de la imagen de portada si existe
        /// </summary>
        public string? CoverImageUrl { get; set; }
    }
}
