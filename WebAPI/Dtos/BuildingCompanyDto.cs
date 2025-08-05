using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class BuildingCompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cif { get; set; } = string.Empty;
        public string? Website { get; set; }
    }

    public class CreateBuildingCompanyDto
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "CIF debe tener 9 caracteres")]
        public string Cif { get; set; } = string.Empty;

        [StringLength(1024)]
        [Url]
        public string? Website { get; set; }
    }
}
