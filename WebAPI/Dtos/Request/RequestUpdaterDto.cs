using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Request
{
    public class RequestUpdaterDto : RequestBaseDto
    {
        [Required]
        [Range(0,36)]
        public string Id { get; set; } = string.Empty;
    }
}
