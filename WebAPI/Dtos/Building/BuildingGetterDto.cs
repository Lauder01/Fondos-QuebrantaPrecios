using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingGetterDto : BuildingBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string Id { get; set; }
        
        public string? Address { get; set; }
        [Range(0, 255)] 
        public string? BuildingCompanyName { get; set; }
    }
}
