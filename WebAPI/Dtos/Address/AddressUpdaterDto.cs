using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressUpdaterDto : AddressBaseDto
    {
        [Required(ErrorMessage = "ERR009: El campo Id es obligatorio")]
        [Range(0, 36, ErrorMessage = "ERR010: El Id debe estar entre 0 y 36")]
        public string Id { get; set; } = string.Empty;
    }
}
