using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressGetterDto : AddressBaseDto
    {
        [Required(ErrorMessage = "ERR016: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR017: El Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
