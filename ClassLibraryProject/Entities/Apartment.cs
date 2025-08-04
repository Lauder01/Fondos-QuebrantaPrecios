namespace FQP.Entities
{
	public class Apartment
	{
		// Local
		public Guid Id { get; set; } = Guid.NewGuid();
		public Floor? ApartmentFloor { get; set; } = null;
		public string Code { get; set; } = string.Empty;
		public string Door { get; set; } = string.Empty;

		// Remote
		public Building? ApartmentBuilding { get; set; } = null;
        public Address? ApartmentAddress { get; set; } = null;

        public Apartment(){}
	}
}
