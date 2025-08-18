using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.DistrictStreet
{
    public class DistrictStreetBaseDto
    {
        [Required]
        public string DistrictId { get; set; } = string.Empty;
        [Required]
        public string StreetId { get; set; } = string.Empty;
    }
}
