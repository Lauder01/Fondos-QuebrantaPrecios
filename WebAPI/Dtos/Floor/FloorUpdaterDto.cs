using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorUpdaterDto : FloorBaseDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
