namespace ClassLibraryProject.Entities
{
	public class Apartment
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public Floor ApartmentFloor { get; set; } = new Floor();
        public Address ApartmentAddress { get; set; } = new Address();
		public double? Surface { get; set; } = 0.0;
		public string? RoomNumber { get; set; } = string.Empty;
		public string? BathroomNumber { get; set; } = string.Empty;
        public Building ApartmentBuilding { get; set; } = new Building();

        public Apartment(){}
	}
}
