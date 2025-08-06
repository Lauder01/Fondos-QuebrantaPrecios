namespace ClassLibraryProject.Entities
{
    public class BuildingCompany
    {
        // Local
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Cif { get; set; } = string.Empty;
        public string? Website { get; set; } = string.Empty;

        // Remote
        public Address? CompanyAddress { get; } = new Address();
        public List<Building> CompnayBuildings { get; } = new List<Building>();

        public BuildingCompany() { }
    }
}
