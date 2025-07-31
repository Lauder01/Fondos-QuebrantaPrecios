namespace FQP.Entities
{
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Name { get; set; } = string.Empty;
        public Address? CompanyAddress { get; set; } = new Address();
        public string? Cif { get; set; } = string.Empty;
        public string? Website { get; set; } = string.Empty;
        public List<Building> CompnayBuildings { get; set; } = new List<Building>();

        public Company() { }
    }
}
