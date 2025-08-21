using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Request
{
    public class RequestBaseDto : IValidatableObject
    {
        [Required]
        [Range(0,36)]
        public string BuildingId { get; set; } = string.Empty;
        [Required]
        [Range(0,36)]
        public string StatusId { get; set; } = string.Empty;
        [Required]
        [Range(typeof(decimal), "0.00", "9999999999.99")]
        public decimal Price { get; set; }
        [Required]
        [Range(typeof(decimal), "0.00", "9999999999.99")]
        public float MaintenancePrice { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
