using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Floor
{
    public class FloorUpdaterDto : FloorBaseDto
    {
        [Required(ErrorMessage = "ERR008: El campo Code es obligatorio")]
        [Range(2,50, ErrorMessage = "ERR009: El código debe estar entre 2 y 50")]
        public string Code { get; set; } = string.Empty;
    }
}
