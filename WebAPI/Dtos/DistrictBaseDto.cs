using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class DistrictBaseDto
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string ZipCode { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Country { get; set; }
        [StringLength(255)]
        public string? City { get; set; }
    }
}
