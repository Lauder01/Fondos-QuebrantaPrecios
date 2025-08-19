using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Zipcode
{
    public class ZipcodeUpdaterDto : ZipcodeBaseDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
