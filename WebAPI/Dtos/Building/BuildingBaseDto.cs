using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingBaseDto : IValidatableObject
    {
        [StringLength(255, MinimumLength = 0, ErrorMessage = "ERR001: El nombre debe tener entre 0 y 255 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1024, ErrorMessage = "ERR002: La descripción no puede superar los 1024 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "ERR003: El campo Doorway es obligatorio")]
        [StringLength(6, MinimumLength = 1, ErrorMessage = "ERR004: El doorway debe tener entre 1 y 6 caracteres")]
        public required string Doorway { get; set; }

        [Range(0, 1000, ErrorMessage = "ERR005: El número de plantas debe estar entre 0 y 1000")]
        public required int FloorCount { get; set; }

        [Range(1800, 2100, ErrorMessage = "ERR006: El año de construcción debe estar entre 1800 y 2100")]
        public int YearBuilt { get; set; } = 1970;

        [Range(0.0, 9999999999.99, ErrorMessage = "ERR007: El precio debe estar entre 0 y 9999999999.99")]
        public decimal Price { get; set; } = 0.0m;

        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR008: El DistrictId debe tener exactamente 36 caracteres")]
        public string DistrictId { get; set; } = string.Empty;

        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR009: El StreetId debe tener exactamente 36 caracteres")]
        public string StreetId { get; set; } = string.Empty;

        [Required(ErrorMessage = "ERROR")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR011: El BuildingCompanyId debe tener exactamente 36 caracteres")]
        public string BuildingCompanyId { get; set; } = string.Empty;

        //[StringLength(36, MinimumLength = 0, ErrorMessage = "ERR013: El StatusId debe tener exactamente 36 caracteres")]
        public string StatusId { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "ERR012: El certificado energético no puede superar los 10 caracteres")]
        public string EnergyCertificate { get; set; } = string.Empty;

        public bool HasElevator { get; set; }
        
        // Campos opcionales para la creación automática del Address
        [StringLength(36, MinimumLength = 36, ErrorMessage = "El ZipcodeId debe tener exactamente 36 caracteres")]
        public string? ZipcodeId { get; set; }
        
        [StringLength(255, ErrorMessage = "La dirección construida no puede superar los 255 caracteres")]
        public string? ConstructedAddress { get; set; }
        
        [StringLength(255, ErrorMessage = "El país no puede superar los 255 caracteres")]
        public string? Country { get; set; }
        
        [StringLength(255, ErrorMessage = "La ciudad no puede superar los 255 caracteres")]
        public string? City { get; set; }
        
        [Range(0, 20, ErrorMessage = "El número de apartamentos por piso debe estar entre 0 y 20")]
        public int ApartmentsPerFloor { get; set; } = 1;
        
        [Range(0, 20000, ErrorMessage = "El número total de apartamentos debe estar entre 0 y 20000")]
        public int ApartmentCount { get; set; } = 0;
        
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
