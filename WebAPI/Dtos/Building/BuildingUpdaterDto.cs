using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingUpdaterDto : BuildingBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
