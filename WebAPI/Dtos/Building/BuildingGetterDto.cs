using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingGetterDto : BuildingBaseDto
    {
        [Required(ErrorMessage = "ERR014: El campo Id es obligatorio")]
        [Range(0, 36, ErrorMessage = "ERR015: El Id debe estar entre 0 y 36")]
        public string Id { get; set; }
        
        public string? Address { get; set; }
        [Range(0, 255, ErrorMessage = "ERR016: El nombre de la empresa debe estar entre 0 y 255")]
        public string? BuildingCompanyName { get; set; }
    }
}
