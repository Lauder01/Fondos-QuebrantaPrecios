using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingCompany
{
    public class BuildingCompanyGetterDto : BuildingCompanyBaseDto
    {
        [Required(ErrorMessage = "ERR007: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR008: El campo Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
