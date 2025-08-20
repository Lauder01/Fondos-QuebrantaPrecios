using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressBaseDto : IValidatableObject
    {
        [Required]
        [Range(0, 36)]
        public string BuildingId { get; set; } = string.Empty;
        [Range(0, 36)]
        public string? ApartmentId { get; set; }
        [Range(0, 36)]
        public bool? IsApartment { get; set; }
        [Required]
        [Range(0,50)]
        public string ZipcodeId { get; set; } = string.Empty;
        [Required]
        [Range(0,36)]
        public string StreetId { get; set; } = string.Empty;

        //este método de momento está vacío, no sirve de nada
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
