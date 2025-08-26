using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Street
{
    public class StreetGetterDto : StreetBaseDto
    {
        [Required(ErrorMessage = "ERR006: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR007: El campo Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
