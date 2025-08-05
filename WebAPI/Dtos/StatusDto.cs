using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class StatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
    public class CreateStatusDto
    {
        [Required]
        [StringLength(48, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Description { get; set; }
    }
}
