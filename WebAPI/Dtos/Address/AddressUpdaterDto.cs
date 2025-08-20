using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressUpdaterDto : AddressBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string Id { get; set; } = string.Empty;
    }
}
