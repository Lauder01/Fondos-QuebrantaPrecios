using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorGetterDto : FloorBaseDto
    {
        [Required]
        [Range(2,50)]   

        public string Code { get; set; } = string.Empty;
    }
}
