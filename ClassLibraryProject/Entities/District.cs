namespace ClassLibraryProject.Entities
{
	public class District
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Name { get; set; } = string.Empty;
		public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
		public List<Building> Buildings { get; set; } = new List<Building>();

        public District() { }
	}
}
