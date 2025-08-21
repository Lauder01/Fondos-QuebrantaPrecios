using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.District
{
    public class DistrictUpdaterDto : DistrictBaseDto
    {
        [Required(ErrorMessage = "ERR008: El campo Id es obligatorio")]
        public Guid Id { get; set; }
    }
}
