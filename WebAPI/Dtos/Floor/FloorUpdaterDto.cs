using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorUpdaterDto : FloorBaseDto
    {
        [Required(ErrorMessage = "ERR008: El campo Identificador es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR009: El campo Identificador debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
