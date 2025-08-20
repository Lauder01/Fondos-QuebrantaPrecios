using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentBaseDto : IValidatableObject
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Code { get; set; } = string.Empty;
        [Required]
        [StringLength(24, MinimumLength = 1)]
        public string Door { get; set; } = string.Empty;
        [Required]
        [Range(0,36)]
        public string FloorId { get; set; } = string.Empty;
        [Required]
        [Range(0,36)]
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
