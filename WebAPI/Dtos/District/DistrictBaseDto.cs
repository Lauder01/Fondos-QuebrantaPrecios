using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.District
{
    public class DistrictBaseDto : IValidatableObject
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Code { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Country { get; set; }
        [StringLength(255)]
        public string? City { get; set; }
        public List<string> Zipcodes { get; set; } = new List<string>();
        public List<string> Streets { get; set; } = new List<string>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Code) && Name == Code)
            {
                yield return new ValidationResult("El nombre y el código no pueden ser iguales.", new[] { nameof(Name), nameof(Code) });
            }
        }
    }
}
