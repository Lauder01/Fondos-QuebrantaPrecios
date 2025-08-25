using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressUpdaterDto : AddressBaseDto
    {
        [Required(ErrorMessage = "ERR009: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public string Id { get; set; } = string.Empty;
    }
}
