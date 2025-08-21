using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.BuildingCompany
{
    public class BuildingCompanyBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Nombre es obligatorio")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "ERR002: El nombre debe tener entre 2 y 255 caracteres")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "ERR003: El campo CIF es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "ERR004: El CIF debe tener 9 caracteres")]
        public string Cif { get; set; } = string.Empty;
        [StringLength(1024, ErrorMessage = "ERR005: La web no puede superar los 1024 caracteres")]
        [Url(ErrorMessage = "ERR006: La web debe ser una URL válida")]
        public string? Website { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Puedes agregar validaciones personalizadas aquí
            yield break;
        }
    }
}
