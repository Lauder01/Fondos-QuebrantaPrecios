using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressGetterDto : AddressBaseDto
    {
        [Required(ErrorMessage = "ERR009: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public string Id { get; set; } = string.Empty;
        [Range(0,50, ErrorMessage = "ERR011: El Zipcode debe estar entre 0 y 50")]
        public string? Zipcode { get; set; }
        [Range(0,255, ErrorMessage = "ERR012: La calle debe estar entre 0 y 255")]
        public string? Street { get; set; }
    }
}
