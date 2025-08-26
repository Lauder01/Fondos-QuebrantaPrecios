using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Apartment
{
    public class ApartmentGetterDto : ApartmentBaseDto
    {
        [Required(ErrorMessage = "ERR007: El campo identificador es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR008: El campo identificador debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
