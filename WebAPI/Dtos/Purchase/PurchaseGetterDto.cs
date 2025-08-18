using System;

namespace WebAPI.Dtos.Purchase
{
    public class PurchaseGetterDto : PurchaseBaseDto
    {
        public string Id { get; set; } = string.Empty;
        public string? BuildingName { get; set; }
        public string? BuildingCompanyName { get; set; }
        public string? RequestDescription { get; set; }
        // No se duplican los campos del base
    }
}
