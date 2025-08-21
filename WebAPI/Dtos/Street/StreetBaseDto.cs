using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Street
{
    public class StreetBaseDto : IValidatableObject
    {
        [Required]
        [Range(0,255)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0,24)]
        public string Code { get; set; } = string.Empty;
        [Required]
        public int StreetType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Code) && Name == Code)
            {
                yield return new ValidationResult("El nombre y el código no pueden ser iguales.", new[] { nameof(Name), nameof(Code) });
            }
        }
    }
}
