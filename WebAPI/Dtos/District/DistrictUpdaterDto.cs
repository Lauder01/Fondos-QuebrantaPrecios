using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.District
{
    public class DistrictUpdaterDto : DistrictBaseDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
