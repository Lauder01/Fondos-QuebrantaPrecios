using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Address
{
    public class AddressBaseDto : IValidatableObject
    {
        [Required]
        public string BuildingId { get; set; } = string.Empty;
        public string? ApartmentId { get; set; }
        public bool? IsApartment { get; set; }
        [Required]
        public string ZipcodeId { get; set; } = string.Empty;
        [Required]
        public string StreetId { get; set; } = string.Empty;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
