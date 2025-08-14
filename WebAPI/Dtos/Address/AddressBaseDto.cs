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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
