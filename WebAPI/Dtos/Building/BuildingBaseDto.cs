using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Nombre es obligatorio")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "ERR002: El nombre debe tener entre 2 y 255 caracteres")]
        public string Name { get; set; } = string.Empty;
        [StringLength(1024, ErrorMessage = "ERR003: La descripción no puede superar los 1024 caracteres")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "ERR004: El campo Código es obligatorio")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "ERR005: El código debe tener entre 2 y 30 caracteres")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR006: El campo Doorway es obligatorio")]
        [StringLength(6, MinimumLength = 1, ErrorMessage = "ERR007: El doorway debe tener entre 1 y 6 caracteres")]
        public string Doorway { get; set; } = string.Empty;
        [Range(0, 100, ErrorMessage = "ERR008: El número de plantas debe estar entre 0 y 100")]
        public int? FloorCount { get; set; }
        [Range(1800, 2100, ErrorMessage = "ERR009: El año de construcción debe estar entre 1800 y 2100")]
        public int? YearBuilt { get; set; }
        [Range(typeof(decimal), "0.00", "9999999999.99", ErrorMessage = "ERR010: El precio debe estar entre 0.00 y 9999999999.99")]
        public double? Price { get; set; }
        [Range(0, 36, ErrorMessage = "ERR011: El DistrictId debe estar entre 0 y 36")]
        public string? DistrictId { get; set; }
        [Range(0, 36, ErrorMessage = "ERR012: El StreetId debe estar entre 0 y 36")]
        public string? StreetId { get; set; }
        [Range(0, 36, ErrorMessage = "ERR013: El BuildingCompanyId debe estar entre 0 y 36")]
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
