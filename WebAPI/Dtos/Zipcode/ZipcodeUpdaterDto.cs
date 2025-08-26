using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Zipcode
{
    public class ZipcodeUpdaterDto : ZipcodeBaseDto
    {
        [Required(ErrorMessage = "ERR005: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR006: El campo Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
