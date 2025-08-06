namespace ClassLibraryProject.Entities
{
    public class Purchase
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Building PurchaseBuilding { get; set; } = new Building();
        public BuildingCompany PurchaseCompany { get; set; } = new BuildingCompany();
        public Request PurchaseRequest { get; set; } = new Request();
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public decimal? Amount { get; set; } = 0.0m;
        

        public Purchase() { }
    }
}
