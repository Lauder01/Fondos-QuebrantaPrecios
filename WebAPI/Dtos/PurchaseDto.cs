using System;

namespace WebAPI.Dtos
{
    public class PurchaseDto
    {
        public string Id { get; set; } = string.Empty;
        public string BuildingId { get; set; } = string.Empty;
        public string BuildingCompanyId { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        // Puedes agregar más propiedades según lo que quieras exponer
    }
}
