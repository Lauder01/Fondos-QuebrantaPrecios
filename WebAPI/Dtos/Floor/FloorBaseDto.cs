using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorBaseDto : IValidatableObject
    {
        [Required(ErrorMessage = "ERR001: El campo FloorNumber es obligatorio")]
        [Range(-12, 100, ErrorMessage = "ERR002: El número de planta debe estar entre -12 y 100")]
        public int FloorNumber { get; set; }
        
        [Required(ErrorMessage = "ERR003: El campo HasLift es obligatorio")]
        public bool HasLift { get; set; }
        
        [Required(ErrorMessage = "ERR004: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public string Id { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "ERR006: El campo BuildingId es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El BuildingId debe tener exactamente 36 caracteres")]
        public string BuildingId { get; set; } = string.Empty;
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
