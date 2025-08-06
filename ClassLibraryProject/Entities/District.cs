namespace ClassLibraryProject.Entities
{
	public class District
	{
		// Remote
		public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
		public string ZipCode { get; set; } = string.Empty;
		public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
		public int BuildingCount { get; set; } = 0;
        
		// Local
		public List<Building> Buildings { get; } = new List<Building>();

        public District() { }
	}
}
