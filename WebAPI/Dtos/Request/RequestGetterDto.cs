using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Request
{
    public class RequestGetterDto : RequestBaseDto
    {
        [Required(ErrorMessage = "ERR007: El campo Identificador es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR008: El campo Identificador debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
    }
}
