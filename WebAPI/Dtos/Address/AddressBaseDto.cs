using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo BuildingId es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR002: El BuildingId debe ser un GUID válido de 36 caracteres")]
        public required string BuildingId { get; set; }

        [StringLength(36, ErrorMessage = "ERR003: El ApartmentId debe ser un GUID válido de máximo 36 caracteres")]
        public string? ApartmentId { get; set; } = null; // Cambiado a null por defecto

        [Required(ErrorMessage = "ERR004: El campo ZipcodeId es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR005: El ZipcodeId debe ser un GUID válido de 36 caracteres")]
        public required string ZipcodeId { get; set; }

        [Required(ErrorMessage = "ERROR006: El campo ZipcodeId es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR007: La direcci�n construida debe tener entre 1 y 255 caracteres")]
        public required string ConstructedAddress { get; set; }

        [Required(ErrorMessage = "ERROR008: El campo IsApartment es obligatorio")]
        public required bool IsApartment { get; set; }

        [Required(ErrorMessage = "ERROR010: El campo country es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR011: El country debe tener entre 1 y 255 caracteres")]
        public required string Country { get; set; } 

        [Required(ErrorMessage = "ERROR12: El campo state es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR013: El state debe tener entre 1 y 255 caracteres")]
        public required string City { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
