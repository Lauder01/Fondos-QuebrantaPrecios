using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Status
{
    public class StatusBaseDto
    {
        [Required]
        [StringLength(48, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Description { get; set; }
    }
}
