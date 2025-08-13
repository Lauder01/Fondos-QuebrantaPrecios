using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingCompany
{
    public class BuildingCompanyBaseDto : IValidatableObject
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Puedes agregar validaciones personalizadas aquí
            yield break;
        }
    }
}
