using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingImage
{
    public class BuildingImageUpdaterDto : BuildingImageBaseDto
    {
        [Required]
        [StringLength(36, MinimumLength = 36)]
        public string BuildingImageId { get; set; }
    }
}