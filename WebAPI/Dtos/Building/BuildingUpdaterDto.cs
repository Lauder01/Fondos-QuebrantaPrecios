using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingUpdaterDto : BuildingBaseDto
    {
        [Required]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public string Id { get; set; } = string.Empty;
    }
}
