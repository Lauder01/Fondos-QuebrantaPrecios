using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressUpdaterDto : AddressBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
