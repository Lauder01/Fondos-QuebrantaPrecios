namespace FQPClassLibraryProject
{
	public class District
	{
		// Propiedades
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
		public int BuildingNumber { get; set; } = 0;

		// Constructor
		public District() { }
	}
}
