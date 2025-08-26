using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressUpdaterDto : AddressBaseDto
    {
        [Required(ErrorMessage = "ERR014: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR015: El Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; } 
    }
}
