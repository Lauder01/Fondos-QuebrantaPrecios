using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Código es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "ERR002: El código debe tener entre 1 y 50 caracteres")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR003: El campo Door es obligatorio")]
        [StringLength(24, MinimumLength = 1, ErrorMessage = "ERR004: La puerta debe tener entre 1 y 24 caracteres")]
        public string Door { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR005: El campo FloorId es obligatorio")]
        [Range(0,36, ErrorMessage = "ERR006: El FloorId debe estar entre 0 y 36")]
        public string FloorId { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR007: El campo BuildingId es obligatorio")]
        [Range(0,36, ErrorMessage = "ERR008: El BuildingId debe estar entre 0 y 36")]
        public string BuildingId { get; set; } = string.Empty;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Door) && Code == Door)
            {
                yield return new ValidationResult("El código y la puerta no pueden ser iguales.", new[] { nameof(Code), nameof(Door) });
            }
        }
    }
}
