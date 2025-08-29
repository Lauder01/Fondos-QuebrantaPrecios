using System.ComponentModel.DataAnnotations;
/// <summary>
/// Este dto está para que speculab acceda a la request de un edificio en concreto.
/// </summary> 

namespace WebAPI.Dtos.SpecuLab
{
    public class SpecuLabGetterDto
    {
        [Required(ErrorMessage = "ERR001: El campo Código de Edificio es obligatorio")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "ERR002: El campo Código de Edificio debe tener entre 1 y 30 caracteres")]
        public required string BuildingCode { get; set; }

    }
}