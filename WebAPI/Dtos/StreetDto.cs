using ClassLibraryProject.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class StreetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
    public class CreateStreetDto
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public StreetTypeEnum StreetType { get; set; }
    }
}
