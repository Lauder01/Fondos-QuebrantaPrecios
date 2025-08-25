using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingGetterDto : BuildingBaseDto
    {
        [Required(ErrorMessage = "ERR014: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public string Id { get; set; } = string.Empty;
        
        public string? Address { get; set; }
        [Range(0, 255, ErrorMessage = "ERR016: El nombre de la empresa debe estar entre 0 y 255")]
        public string? BuildingCompanyName { get; set; }
    }
}
