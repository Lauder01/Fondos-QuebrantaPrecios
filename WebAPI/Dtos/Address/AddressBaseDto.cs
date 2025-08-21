using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo BuildingId es obligatorio")]
        [Range(0, 36, ErrorMessage = "ERR002: El BuildingId debe estar entre 0 y 36")]
        public string BuildingId { get; set; } = string.Empty;
        [Range(0, 36, ErrorMessage = "ERR003: El ApartmentId debe estar entre 0 y 36")]
        public string? ApartmentId { get; set; }
        [Range(0, 36, ErrorMessage = "ERR004: El IsApartment debe estar entre 0 y 36")]
        public bool? IsApartment { get; set; }
        [Required(ErrorMessage = "ERR005: El campo ZipcodeId es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "ERR006: El ZipcodeId debe tener entre 1 y 50 caracteres")]
        public string ZipcodeId { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR007: El campo StreetId es obligatorio")]
        [Range(0,36, ErrorMessage = "ERR008: El StreetId debe estar entre 0 y 36")]
        public string StreetId { get; set; } = string.Empty;

        //este método de momento está vacío, no sirve de nada
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
