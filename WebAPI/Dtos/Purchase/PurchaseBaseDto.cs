using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseBaseDto
    {
        [Required]
        [Range(0, 36)]
        public string BuildingId { get; set; } = string.Empty;
        [Required]
        [Range(0, 36)]
        public string BuildingCompanyId { get; set; } = string.Empty;
        [Required]
        [Range(0, 36)]
        public string RequestId { get; set; } = string.Empty;
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Range(0.0, 9999999999.99)]
        public decimal Amount { get; set; }
    }
}
