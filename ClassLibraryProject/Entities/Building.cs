using ClassLibraryProject.Enums;

namespace ClassLibraryProject.Entities
{
    public class Building
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? Name { get; set; } = "Empty";
        public District BuildingDistrict { get; set; } = new District();
        public Address BuildingAdress { get; set; } = new Address();
        public Company? BuildingCompany { get; set; } = new Company();
        public double? Price { get; set; } = 0.0;
        public Status Status { get; set; } = new Status();
        public EnergyCertificateEnum? BuildingEnergyCertificate { get; set; } = EnergyCertificateEnum.U;
        public List<Floor> FloorCount { get; set; } = new List<Floor>();
        public int? YearBuilt { get; set; } = 1;
        public string? Description { get; set; } = "Empty";
        public bool HasLift { get; set; } = false;
        public bool IsCompanyBuilding { get; set; } = false;

        public Building() { }
    }
}