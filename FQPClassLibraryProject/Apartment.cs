namespace FQPClassLibraryProject
{
	public class Apartment
	{
		public string Id {  get; set; }	= string.Empty;
		public Address ApartmentAddress { get; set; } = new Address();
		public double Surface { get; set; } = 0.0;
		public string RoomNumber { get; set; } = string.Empty;
		public string Building { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty; // en un futuro será un enum

		public Apartment(){}
	}
}
