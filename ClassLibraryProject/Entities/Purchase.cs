using System;

namespace ClassLibraryProject.Entities
{
    public class Purchase
    {
        public string Id { get; set; }
        public string BuildingId { get; set; }
        public string BuildingCompanyId { get; set; }
        public string RequestId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public virtual Building Building { get; set; }
        public virtual BuildingCompany BuildingCompany { get; set; }
        public virtual Request Request { get; set; }
        public Purchase() { }
    }
}
