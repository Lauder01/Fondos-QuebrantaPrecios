using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingImage
{
    public class BuildingImageUpdaterDto : BuildingImageBaseDto
    {
        [Required(ErrorMessage = "ERR010: El campo Identificador de Imagen de Edificio es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR011: El campo Identificador de Imagen de Edificio debe tener exactamente 36 caracteres")]
        public required string BuildingImageId { get; set; }
    }
}