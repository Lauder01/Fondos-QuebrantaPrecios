using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.User
{
    public class FQPUserUpdaterDto : FQPUserBaseDto
    {
        [Required]
        public Guid Id { get; set; }
        // Hereda los demás campos del base
    }
}
