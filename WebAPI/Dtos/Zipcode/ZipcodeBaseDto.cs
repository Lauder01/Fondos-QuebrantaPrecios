using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Zipcode
{
    public class ZipcodeBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo Código es obligatorio")]
        [Range(0,50, ErrorMessage = "ERR002: El campo Código debe estar entre 0 y 50")]
        public required string Code { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Puedes añadir validaciones personalizadas aquí si lo necesitas
            yield break;
        }
    }
}
