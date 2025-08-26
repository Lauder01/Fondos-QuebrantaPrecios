using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo BuildingId es obligatorio")]
        [Range(0, 36, ErrorMessage = "ERR002: El BuildingId debe estar entre 0 y 36")]
        public required string BuildingId { get; set; }

        [Range(0, 36, ErrorMessage = "ERR003: El ApartmentId debe estar entre 0 y 36")]
        public string ApartmentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "ERR004: El campo ZipcodeId es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "ERR005: El ZipcodeId debe tener entre 1 y 50 caracteres")]
        public required string ZipcodeId { get; set; }

        [Required(ErrorMessage = "ERROR006: El campo ZipcodeId es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR007: La dirección construida debe tener entre 1 y 255 caracteres")]
        public required string ConstructedAddress { get; set; }

        [Range(0, 36, ErrorMessage = "ERR008: El IsApartment debe estar entre 0 y 36")]
        public bool? IsApartment { get; set; }

        [Required(ErrorMessage = "ERROR09: El campo country es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR010: El country debe tener entre 1 y 255 caracteres")]
        public required string Country { get; set; }

        [Required(ErrorMessage = "ERROR11: El campo state es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR012: El state debe tener entre 1 y 255 caracteres")]
        public required string City { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
