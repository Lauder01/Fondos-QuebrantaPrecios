using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Request
{
    public class RequestUpdaterDto : RequestBaseDto
    {
        [Required(ErrorMessage = "ERR009: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El campo Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; } 
    }
}
