using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseBaseDto
    {
        [Required(ErrorMessage = "ERR001: El campo Identificador de Edificio es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR002: El campo Identificador de Edificio debe tener exactamente 36 caracteres")]
        public required string BuildingId { get; set; }

        [Required(ErrorMessage = "ERR003: El campo Identificador de Empresa Constructora es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR004: El campo Identificador de Empresa Constructora debe tener exactamente 36 caracteres")]
        public required string BuildingCompanyId { get; set; }

        [Required(ErrorMessage = "ERR005: El campo Identificador de Pedido es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR006: El campo Identificador de Pedido debe tener exactamente 36 caracteres")]
        public required string RequestId { get; set; } 

        [Required(ErrorMessage = "ERR007: El campo Fecha es obligatorio")]
        public required DateTime Date { get; set; }

        [Required(ErrorMessage = "ERR008: El campo Cantidad es obligatorio")]
        [Range(0.0, 9999999999.99)]
        public required decimal Amount { get; set; }
    }
}
