using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Street
{
    public class StreetBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Nombre es obligatorio")]
        [Range(0,255, ErrorMessage = "ERR002: El campo Nombre debe estar entre 0 y 255")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "ERR003: El campo Código es obligatorio")]
        [Range(0,24, ErrorMessage = "ERR004: El campo Código debe estar entre 0 y 24")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "ERR005: El campo Tipo de Calle es obligatorio")]
        public required int StreetType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Code) && Name == Code)
            {
                yield return new ValidationResult("El nombre y el código no pueden ser iguales.", new[] { nameof(Name), nameof(Code) });
            }
        }
    }
}
