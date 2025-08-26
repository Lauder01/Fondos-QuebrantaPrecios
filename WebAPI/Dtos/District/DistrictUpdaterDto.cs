using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.District
{
    public class DistrictUpdaterDto : DistrictBaseDto
    {
        [Required(ErrorMessage = "ERR009: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El campo Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
