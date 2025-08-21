using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingCompany
{
    public class BuildingCompanyUpdaterDto : BuildingCompanyBaseDto
    {
        [Required(ErrorMessage = "ERR007: El campo Id es obligatorio")]
        [Range(0, 36, ErrorMessage = "ERR008: El Id debe estar entre 0 y 36")]
        public string Id { get; set; } = string.Empty;
    }
}
