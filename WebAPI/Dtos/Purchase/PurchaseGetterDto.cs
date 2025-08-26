using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseGetterDto : PurchaseBaseDto
    {
        [Required(ErrorMessage = "ERR009: El campo Id es obligatorio")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "ERR010: El Id debe tener exactamente 36 caracteres")]
        public required string Id { get; set; }
        
    }
}
