using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressGetterDto : AddressBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string Id { get; set; } = string.Empty;
        public string? Zipcode { get; set; }
        public string? Street { get; set; }
    }
}
