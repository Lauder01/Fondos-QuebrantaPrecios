using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingCompany
{
    public class BuildingCompanyUpdaterDto : BuildingCompanyBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
