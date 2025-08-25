using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Status
{
    public class StatusBaseDto : IValidatableObject
    {
        [Required]
        [StringLength(36, MinimumLength = 1)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1024)]
        public string? Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && Name == Id)
            {
                yield return new ValidationResult("El nombre y el identificador no pueden ser iguales.", new[] { nameof(Name), nameof(Id) });
            }
        }
    }
}
