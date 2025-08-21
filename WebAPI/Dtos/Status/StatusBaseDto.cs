using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Status
{
    public class StatusBaseDto
    {
        [Required]
        [Range(0,48)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
    }
}
