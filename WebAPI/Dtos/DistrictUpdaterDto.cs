using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class DistrictUpdaterDto : DistrictBaseDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
