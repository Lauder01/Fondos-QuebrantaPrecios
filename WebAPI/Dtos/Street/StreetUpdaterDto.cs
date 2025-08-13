using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Street
{
    public class StreetUpdaterDto : StreetBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
