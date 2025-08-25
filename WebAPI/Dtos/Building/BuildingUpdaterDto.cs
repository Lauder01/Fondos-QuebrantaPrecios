using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingUpdaterDto : BuildingBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string Id { get; set; } = string.Empty;
    }
}
