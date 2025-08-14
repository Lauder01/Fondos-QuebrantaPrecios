using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorUpdaterDto : FloorBaseDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
