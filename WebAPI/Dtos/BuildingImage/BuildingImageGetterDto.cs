using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingImage
{
    public class BuildingImageGetterDto : BuildingImageBaseDto
    {
        [Required(ErrorMessage = "ERR008: El campo Identificador de Imagen de Edificio es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR009: El campo Identificador de Imagen de Edificio debe tener exactamente 36 caracteres")]
        public required string BuildingImageId { get; set; }
    }
}