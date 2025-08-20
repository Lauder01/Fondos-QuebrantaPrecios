using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressGetterDto : AddressBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string Id { get; set; } = string.Empty;
        [Range(0,50)]
        public string? Zipcode { get; set; }
        [Range(0,255)]
        public string? Street { get; set; }
    }
}
