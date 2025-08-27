using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Request
{
    public class RequestBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Identificador de Edificio es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR002: El campo Identificador de Edificio debe tener exactamente 36 caracteres")]
        public required string BuildingId { get; set; }

        [Required(ErrorMessage = "ERR003: El campo Identificador de Estado es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR004: El campo Identificador de Estado debe tener exactamente 36 caracteres")]
        public required string StatusId { get; set; }

        [Required(ErrorMessage = "ERR005: El campo Precio es obligatorio")]
        [Range(0.0, 9999999999.99)]
        public required decimal Price { get; set; }

        [Required(ErrorMessage = "ERR006: El campo Precio de Mantenimiento es obligatorio")]
        [Range(0.0, 9999999999.99)]
        public required float MaintenancePrice { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
