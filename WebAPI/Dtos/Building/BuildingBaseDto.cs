using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingBaseDto : IValidatableObject
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [StringLength(1024)]
        public string? Description { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Code { get; set; } = string.Empty;
        [Required]
        [StringLength(6, MinimumLength = 1)]
        public string Doorway { get; set; } = string.Empty;
        [Range(0, 100)]
        public int? FloorCount { get; set; }
        [Range(1800, 2100)]
        public int? YearBuilt { get; set; }
        [Range(typeof(decimal), "0.00", "9999999999.99")]
        public double? Price { get; set; }
        [Range(0, 36)]
        public string? DistrictId { get; set; }
        [Range(0, 36)]
        public string? StreetId { get; set; }
        [Range(0, 36)]
        public string? BuildingCompanyId { get; set; } 
        public bool HasElevator { get; set; }
        /// <summary>
        /// Valida que el nombre y el código no sean iguales.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Code) && Name == Code)
            {
                yield return new ValidationResult("El nombre y el código no pueden ser iguales.", new[] { nameof(Name), nameof(Code) });
            }
        }
    }
}
