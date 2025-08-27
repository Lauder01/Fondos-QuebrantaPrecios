using WebAPI.Dtos.Building;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Building
{
    public class BuildingWithAddressCreatorDto : BuildingCreatorDto
    {
        // Campos adicionales para la creación automática del Address
        
        [StringLength(5, MinimumLength = 5, ErrorMessage = "El código postal debe tener exactamente 5 caracteres")]
        public string? ZipcodeCode { get; set; }
        
        [StringLength(255, MinimumLength = 1, ErrorMessage = "La dirección construida debe tener entre 1 y 255 caracteres")]
        public string? ConstructedAddress { get; set; }
        
        [StringLength(255, MinimumLength = 1, ErrorMessage = "El país debe tener entre 1 y 255 caracteres")]
        public string? Country { get; set; }
        
        [StringLength(255, MinimumLength = 1, ErrorMessage = "La ciudad debe tener entre 1 y 255 caracteres")]
        public string? City { get; set; }
    }
}
