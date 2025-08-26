using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Street
{
    public class StreetUpdaterDto : StreetBaseDto
    {
        [Required(ErrorMessage = "ERR008: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR009: El Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
