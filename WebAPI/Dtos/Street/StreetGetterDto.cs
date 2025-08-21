using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Street
{
    public class StreetGetterDto : StreetBaseDto
    {
        [Required]
        [Range(0,36)]
        public string Id { get; set; } = string.Empty;
    }
}
