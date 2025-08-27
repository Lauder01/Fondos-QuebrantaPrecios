using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Zipcode
{
    public class ZipcodeGetterDto : ZipcodeBaseDto
    {
        [Required(ErrorMessage = "ERR003: El campo Identificador es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR004: El campo Identificador debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
