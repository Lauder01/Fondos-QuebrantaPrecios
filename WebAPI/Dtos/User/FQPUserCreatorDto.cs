using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.User
{
    public class FQPUserCreatorDto : FQPUserBaseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        // No necesita Id, IsActive se puede omitir o inicializar en el servicio
    }
}
