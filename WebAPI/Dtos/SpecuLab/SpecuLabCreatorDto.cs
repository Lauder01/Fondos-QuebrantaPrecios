using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.SpecuLab
{
    /// <summary>
    /// Este dto se encarga de mandar request a SpecuLab.
    /// </summary>  
    public class SpecuLabCreatorDto
    {
        [Required(ErrorMessage = "ERR013: El campo Código de Edificio es obligatorio")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "ERR014: El campo Código de Edificio debe tener entre 1 y 30 caracteres")]
        public required string BuildingCode { get; set; }

        [Required(ErrorMessage = "ERR015: El campo Nombre de Edificio es obligatorio")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "ERR016: El campo Nombre de Edificio debe tener entre 0 y 255 caracteres")]
        public required string BuildingName { get; set; } 

        [Required(ErrorMessage = "ERR017: El campo Dirección de Construcción es obligatorio")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "ERR018: El campo Dirección de Construcción debe tener entre 0 y 255 caracteres")]
        public required string ConstructedAddress { get; set; }

        [Required(ErrorMessage = "ERR019: El campo Nombre de Distrito es obligatorio")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "ERR020: El campo Nombre de Distrito debe tener entre 1 y 255 caracteres")]
        public required string DistrictName { get; set; }

        [Required(ErrorMessage = "ERR021: El campo Número de Plantas es obligatorio")]
        [Range(0, 1000, ErrorMessage = "ERR022: El campo Número de Plantas debe estar entre 0 y 1000")]
        public required int FloorCount { get; set; }

        [Required(ErrorMessage = "ERR023: El campo Año de Construcción es obligatorio")]
        [Range(1800, 2100, ErrorMessage = "ERR024: El campo Año de Construcción debe estar entre 1800 y 2100")]
        public required int YearBuilt { get; set; }

        [Required(ErrorMessage = "ERR025: El campo Número de Apartamentos es obligatorio")]
        [Range(0, 1000, ErrorMessage = "ERR026: El campo Número de Apartamentos debe estar entre 0 y 1000")]
        public required int ApartmentCount { get; set; } // Para uso futuro
    }
}
