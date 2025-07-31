namespace FQP.Entities
{
	public class Apartment
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public string Door { get; set; } = string.Empty;
		public Floor? ApartmentFloor { get; set; } = null;
		public Address? ApartmentAddress { get; set; } = null;
		public double? Surface { get; set; } = 0.0;
		public string? RoomNumber { get; set; } = string.Empty;
		public string? BathroomNumber { get; set; } = string.Empty;
		public Building? ApartmentBuilding { get; set; } = null;

		public Apartment(){}
	}
}
