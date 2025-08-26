using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo FloorNumber es obligatorio")]
        [Range(-12, 100, ErrorMessage = "ERR002: El campo FloorNumber debe estar entre -12 y 100")]
        public required int FloorNumber { get; set; }
        
        [Required(ErrorMessage = "ERR003: El campo HasLift es obligatorio")]
        public required bool HasLift { get; set; }
        
        [Required(ErrorMessage = "ERR004: El campo BuildingId es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR005: El campo BuildingId debe tener exactamente 36 caracteres")]
        public required string BuildingId { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
