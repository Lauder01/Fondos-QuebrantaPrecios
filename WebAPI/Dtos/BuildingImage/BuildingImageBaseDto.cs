using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingImage
{
    public class BuildingImageBaseDto
    {
        [Required(ErrorMessage = "ERR001: El campo Identificador de Edificio es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR002: El campo Identificador de Edificio debe tener exactamente 36 caracteres")]
        public required string BuildingId { get; set; }

        [Range(0,255, ErrorMessage = "ERR003: El campo Nombre de Archivo debe estar entre 0 y 255 caracteres")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "ERR004: El campo Ruta de Archivo es obligatorio")]
        [StringLength(1024, ErrorMessage = "ERR005: El campo Ruta de Archivo no puede superar los 1024 caracteres")]
        public required string FilePath { get; set; }

        [Range(0,255, ErrorMessage = "ERR006: El campo Texto Alternativo debe estar entre 0 y 255 caracteres")]
        public string AltText { get; set; } = string.Empty;

        [Required(ErrorMessage = "ERR007: El campo Es Imagen de Portada es obligatorio")]
        public bool IsCoverImage { get; set; }
    }
}