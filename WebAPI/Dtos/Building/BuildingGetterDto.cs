using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingGetterDto : BuildingBaseDto
    {
        
        public string? Address { get; set; }
        [Range(0, 255)] 
        public string? BuildingCompanyName { get; set; }
    }
}
