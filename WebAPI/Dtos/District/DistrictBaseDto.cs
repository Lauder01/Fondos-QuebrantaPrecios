using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.District
{
    public class DistrictBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Nombre es obligatorio")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "ERR002: El nombre debe tener entre 2 y 255 caracteres")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR003: El campo Código es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "ERR004: El código debe tener entre 2 y 50 caracteres")]
        public string Code { get; set; } = string.Empty;
        [StringLength(255, ErrorMessage = "ERR005: El país no puede superar los 255 caracteres")]
        public string? Country { get; set; }
        [StringLength(255, ErrorMessage = "ERR006: La ciudad no puede superar los 255 caracteres")]
        public string? City { get; set; }
        public List<string> Zipcodes { get; set; } = new List<string>();
        public List<string> Streets { get; set; } = new List<string>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Code) && Name == Code)
            {
                yield return new ValidationResult("ERR007: El nombre y el código no pueden ser iguales.", new[] { nameof(Name), nameof(Code) });
            }
        }
    }
}
