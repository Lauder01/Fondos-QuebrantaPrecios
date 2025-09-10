using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo C�digo es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "ERR002: El campo C�digo debe tener entre 1 y 50 caracteres")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "ERR003: El campo Puerta es obligatorio")]
        [StringLength(24, MinimumLength = 1, ErrorMessage = "ERR004: El campo Puerta debe tener entre 1 y 24 caracteres")]
        public required string Door { get; set; }

        [Required(ErrorMessage = "ERR005: El campo Identificador de Piso es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR006: El campo Identificador de Piso debe tener exactamente 36 caracteres")]
        public required string FloorId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "ERR007: La superficie debe ser mayor que 0")]
        public decimal Area { get; set; }

        [Range(1, 50, ErrorMessage = "ERR008: El número de habitaciones debe estar entre 1 y 50")]
        public int NumRooms { get; set; }

        [Range(1, 20, ErrorMessage = "ERR009: El número de baños debe estar entre 1 y 20")]
        public int NumBathrooms { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Door) && Code == Door)
            {
                yield return new ValidationResult("El c�digo y la puerta no pueden ser iguales.", new[] { nameof(Code), nameof(Door) });
            }
        }
    }
}
