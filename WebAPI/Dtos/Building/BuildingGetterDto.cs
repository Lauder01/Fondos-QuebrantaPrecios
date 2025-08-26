using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingGetterDto : BuildingBaseDto
    {
        [Required(ErrorMessage = "ERR014: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public string Id { get; set; } = string.Empty;
    }
}
