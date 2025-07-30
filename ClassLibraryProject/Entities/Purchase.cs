namespace ClassLibraryProject.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Building PurchaseBuilding { get; set; } = new Building();
        public Company PurchaseCompany { get; set; } = new Company();
        public string? Description { get; set; } = string.Empty;
        public decimal? Amount { get; set; } = 0.0m;
        public DateTime? PurchaseDate { get; set; } = DateTime.UtcNow;

        public Purchase() { }
    }
}
