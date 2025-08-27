using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingCompany
{
    public class BuildingCompanyUpdaterDto : BuildingCompanyBaseDto
    {
        [Required(ErrorMessage = "ERR009: El campo Identificador es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El campo Identificador debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
