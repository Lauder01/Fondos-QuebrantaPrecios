using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.District
{
    public class DistrictBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Name es obligatorio")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "ERR002: El campo Name debe tener entre 2 y 255 caracteres")]
        public required string Name { get; set; } 

        [Required(ErrorMessage = "ERR003: El campo Code es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "ERR004: El campo Code debe tener entre 2 y 50 caracteres")]
        public required string Code { get; set; } 

        [StringLength(255, ErrorMessage = "ERR005: El campo Country no puede superar los 255 caracteres")]
        public string? Country { get; set; }

        [StringLength(255, ErrorMessage = "ERR006: El campo City no puede superar los 255 caracteres")]
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
