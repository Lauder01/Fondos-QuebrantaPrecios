using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Street
{
    public class StreetBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Nombre es obligatorio")]
        [Range(0,255, ErrorMessage = "ERR002: El nombre debe estar entre 0 y 255")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR003: El campo Código es obligatorio")]
        [Range(0,24, ErrorMessage = "ERR004: El código debe estar entre 0 y 24")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR005: El campo StreetType es obligatorio")]
        public int StreetType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Code) && Name == Code)
            {
                yield return new ValidationResult("El nombre y el código no pueden ser iguales.", new[] { nameof(Name), nameof(Code) });
            }
        }
    }
}
