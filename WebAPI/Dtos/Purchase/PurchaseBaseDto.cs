using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseBaseDto
    {
        [Required]
        public string BuildingId { get; set; } = string.Empty;
        [Required]
        public string BuildingCompanyId { get; set; } = string.Empty;
        [Required]
        public string RequestId { get; set; } = string.Empty;
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
