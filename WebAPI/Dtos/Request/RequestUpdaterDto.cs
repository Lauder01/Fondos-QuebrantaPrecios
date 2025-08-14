using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Request
{
    public class RequestUpdaterDto : RequestBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
