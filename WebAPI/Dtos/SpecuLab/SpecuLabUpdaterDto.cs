using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.SpecuLab
{
    /// <summary>
    /// DTO para actualizar datos de edificio desde SpecuLab. Está relacionado con el método Patch de SpecuLab a nosotros.
    /// </summary>
    public class SpecuLabUpdaterDto
    {
        [Required(ErrorMessage = "ERR018: El campo Nombre de Estatus es obligatorio")]
        [StringLength(48, MinimumLength = 1, ErrorMessage = "ERR019: El campo Nombre de Estatus debe tener entre 1 y 48 caracteres")]
        public required string StatusName { get; set; }
    }
}
