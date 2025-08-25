using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingBaseDto : IValidatableObject
    {
        [StringLength(255, MinimumLength = 2, ErrorMessage = "ERR002: El nombre debe tener entre 2 y 255 caracteres")]
        public string? Name { get; set; }
        [StringLength(1024, ErrorMessage = "ERR003: La descripción no puede superar los 1024 caracteres")]
        public string? Description { get; set; }
        // Código se genera automáticamente en el backend - no se valida desde el frontend
        [Required(ErrorMessage = "ERR006: El campo Doorway es obligatorio")]
        [StringLength(6, MinimumLength = 1, ErrorMessage = "ERR007: El doorway debe tener entre 1 y 6 caracteres")]
        public string Doorway { get; set; } = string.Empty;
        [Range(0, 100, ErrorMessage = "ERR008: El número de plantas debe estar entre 0 y 100")]
        public int? FloorCount { get; set; }
        [Range(1800, 2100, ErrorMessage = "ERR009: El año de construcción debe estar entre 1800 y 2100")]
        public int? YearBuilt { get; set; }
        [Range(0.0, 9999999999.99, ErrorMessage = "ERR010: El precio debe estar entre 0 y 9999999999.99")]
        public decimal? Price { get; set; }
        [StringLength(36, MinimumLength = 1, ErrorMessage = "ERR011: El DistrictId debe tener entre 1 y 36 caracteres")]
        public string? DistrictId { get; set; }
        [StringLength(36, MinimumLength = 1, ErrorMessage = "ERR012: El StreetId debe tener entre 1 y 36 caracteres")]
        public string? StreetId { get; set; }
        [StringLength(36, MinimumLength = 1, ErrorMessage = "ERR013: El BuildingCompanyId debe tener entre 1 y 36 caracteres")]
        public string? BuildingCompanyId { get; set; }
        [StringLength(10, ErrorMessage = "ERR015: El certificado energético no puede superar los 10 caracteres")]
        public string? EnergyCertificate { get; set; }
        public bool HasElevator { get; set; }
        
        /// <summary>
        /// Validación personalizada para el DTO de Building.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validaciones adicionales si son necesarias en el futuro
            yield break;
        }
    }
}
