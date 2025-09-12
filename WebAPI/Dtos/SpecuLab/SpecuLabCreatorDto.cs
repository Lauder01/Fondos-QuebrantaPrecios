using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.SpecuLab
{
    /// <summary>
    /// Este dto se encarga de mandar request a SpecuLab. Esta relacionado con el método Post a SpecuLab.
    /// </summary>  
    public class SpecuLabCreatorDto
    {
        [Required(ErrorMessage = "ERR013: El campo Código de Edificio es obligatorio")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "ERR014: El campo Código de Edificio debe tener entre 1 y 30 caracteres")]
        public required string BuildingCode { get; set; }

        [Required(ErrorMessage = "ERR015: El campo Descripción es obligatorio")]
        private string _description = string.Empty;
        public string Description {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ? "Edificio sin descripción" : value.Trim();
        }

        [Required(ErrorMessage = "ERR016: El campo Precio del Edificio es obligatorio")]
        [Range(0.0, 9999999999.99, ErrorMessage = "ERR017: El campo Precio del Edificio debe estar entre 0 y 9999999999.99")]
        public required string BuildingAmount { get; set; }

        /// <summary>
        /// Campo requerido por la API externa, no se almacena en BD local.
        /// </summary>
        [Required]
        public decimal MaintenanceAmount { get; set; } = 0;
    }
}
