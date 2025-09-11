using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.SpecuLab
{
    /// <summary>
    /// DTO para operaciones JSON Patch estándar
    /// </summary>
    public class PatchOperationDto
    {
        [Required(ErrorMessage = "El campo 'path' es obligatorio")]
        public required string Path { get; set; }

        [Required(ErrorMessage = "El campo 'op' es obligatorio")]
        public required string Op { get; set; }

        [Required(ErrorMessage = "El campo 'value' es obligatorio")]
        public required string Value { get; set; }
    }
}
