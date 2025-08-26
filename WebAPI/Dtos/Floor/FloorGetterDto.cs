using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorGetterDto : FloorBaseDto
    {
        [Required(ErrorMessage = "ERR006: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR007: El campo Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
