using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Request
{
    public class RequestBaseDto : IValidatableObject
    {
        [Required]
        public string BuildingId { get; set; } = string.Empty;
        [Required]
        public string StatusId { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, double.MaxValue)]
        public float MaintenancePrice { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
