using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseBaseDto
    {
        [Required(ErrorMessage = "ERR001: El campo BuildingId es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El BuildingId debe tener exactamente 36 caracteres")]
        public required string BuildingId { get; set; }

        [Required(ErrorMessage = "ERR002: El campo BuildingCompanyId es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El BuildingCompanyId debe tener exactamente 36 caracteres")]
        public required string BuildingCompanyId { get; set; }

        [Required(ErrorMessage = "ERR003: El campo RequestId es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El RequestId debe tener exactamente 36 caracteres")]
        public required string RequestId { get; set; } 

        [Required(ErrorMessage = "ERR004: El campo Date es obligatorio")]
        public required DateTime Date { get; set; }

        [Required(ErrorMessage = "ERR005: El campo Amount es obligatorio")]
        [Range(0.0, 9999999999.99)]
        public required decimal Amount { get; set; }
    }
}
