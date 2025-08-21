using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Zipcode
{
    public class ZipcodeGetterDto : ZipcodeBaseDto
    {
        [Required]
        [Range(0,36)]
        public string Id { get; set; }
    }
}
