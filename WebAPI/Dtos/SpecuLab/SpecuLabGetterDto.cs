using System.ComponentModel.DataAnnotations;
/// <summary>
/// Este dto está para que speculab acceda a la request de un edificio en concreto.
/// </summary> 

namespace WebAPI.Dtos.SpecuLab
{
    public class SpecuLabGetterDto
    {
        [Required(ErrorMessage = "ERR001: El campo Nombre de Edificio es obligatorio")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "ERR002: El campo Nombre de Edificio debe tener entre 0 y 255 caracteres")]
        public required string BuildingName { get; set; }

        [Required(ErrorMessage = "ERR003: El campo Dirección de Construcción es obligatorio")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "ERR004: El campo Dirección de Construcción debe tener entre 0 y 255 caracteres")]
        public required string ConstructedAddress { get; set; }

        [Required(ErrorMessage = "ERR005: El campo Nombre de Distrito es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR006: El campo Nombre de Distrito debe tener entre 1 y 255 caracteres")]
        public required string DistrictName { get; set; }

        [Required(ErrorMessage = "ERR007: El campo Número de Plantas es obligatorio")]
        [Range(0, 1000, ErrorMessage = "ERR008: El campo Número de Plantas debe estar entre 0 y 1000")]
        public required int FloorCount { get; set; }

        [Required(ErrorMessage = "ERR009: El campo Año de Construcción es obligatorio")]
        [Range(1800, 2100, ErrorMessage = "ERR010: El campo Año de Construcción debe estar entre 1800 y 2100")]
        public required int YearBuilt { get; set; }

        [Required(ErrorMessage = "ERR011: El campo Número de Apartamentos es obligatorio")]
        [Range(0, 1000, ErrorMessage = "ERR012: El campo Número de Apartamentos debe estar entre 0 y 1000")]
        public required int ApartmentCount { get; set; }
    }
}