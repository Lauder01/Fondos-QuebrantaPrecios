using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorBaseDto : IValidatableObject
    {
        [Required]
        [Range(-12, 100)]
        public int FloorNumber { get; set; }
        [Required]
        public bool HasLift { get; set; }
        [Required]
        public string BuildingId { get; set; } = string.Empty;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
