using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo código es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "ERR002: El campo código debe tener entre 1 y 50 caracteres")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "ERR003: El campo puerta es obligatorio")]
        [StringLength(24, MinimumLength = 1, ErrorMessage = "ERR004: El campo puerta debe tener entre 1 y 24 caracteres")]
        public required string Door { get; set; }

        [Required(ErrorMessage = "ERR005: El campo identificador de piso es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR006: El campo identificador de piso debe tener exactamente 36 caracteres")]
        public required string FloorId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Door) && Code == Door)
            {
                yield return new ValidationResult("El código y la puerta no pueden ser iguales.", new[] { nameof(Code), nameof(Door) });
            }
        }
    }
}
