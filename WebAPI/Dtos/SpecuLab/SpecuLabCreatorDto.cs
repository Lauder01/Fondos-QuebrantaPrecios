using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.SpecuLab
{
    public class SpecuLabCreatorDto
    {
        [Required(ErrorMessage = "ERR001: El campo Código de Edificio es obligatorio")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "ERR002: El campo Código de Edificio debe tener entre 1 y 30 caracteres")]
        public required string BuildingCode { get; set; }

        [Required(ErrorMessage = "ERR003: El campo Nombre de Edificio es obligatorio")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "ERR004: El campo Nombre de Edificio debe tener entre 0 y 255 caracteres")]
        public required string BuildingName { get; set; }

        [Required(ErrorMessage = "ERR005: El campo Dirección es obligatorio")]
        public required string ConstructedAddress { get; set; }

        [Required(ErrorMessage = "ERR006: El campo Distrito es obligatorio")]
        public required string DistrictName { get; set; }

        [Required(ErrorMessage = "ERR007: El campo Número de pisos es obligatorio")]
        [Range(0, 1000, ErrorMessage = "ERR008: El campo Número de plantas debe estar entre 0 y 1000")]
        public required int FloorCount { get; set; }

        [Required(ErrorMessage = "ERR009: El campo Año de Construcción es obligatorio")]
        [Range(1800, 2100, ErrorMessage = "ERR010: El campo Año de Construcción debe estar entre 1800 y 2100")]
        public int YearBuilt { get; set; }
        // public string ApartmentNumber { get; set; } // Para uso futuro
    }
}