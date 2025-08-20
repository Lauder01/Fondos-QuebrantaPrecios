using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseGetterDto : PurchaseBaseDto
    {
        [Required]
        [Range(0, 36)]  
        public string Id { get; set; } = string.Empty;
        [Range(0, 255)]
        public string? BuildingName { get; set; }
        [Range(0, 255)]
        public string? BuildingCompanyName { get; set; }
        public string? RequestDescription { get; set; }
        // No se duplican los campos del base
    }
}
