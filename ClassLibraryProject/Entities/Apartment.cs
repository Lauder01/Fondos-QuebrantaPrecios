namespace ClassLibraryProject.Entities
{
	public class Apartment
	{
		// Local
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid? FloorId { get; set; }
		public Floor? ApartmentFloor { get; set; } = null;
		public string Code { get; set; } = string.Empty;
		public string Door { get; set; } = string.Empty;
		public Guid? BuildingId { get; set; }
		public Building? ApartmentBuilding { get; set; } = null;
		public Guid? ApartmentAddressId { get; set; } // FK explícita
		public Address? ApartmentAddress { get; set; } = null;

		public Apartment(){}

		public string BuildApartmentCode()
		{
			var floorCode = ApartmentFloor?.Code ?? "00";
			var doorCode = Door ?? "0";
			return $"{floorCode}-{doorCode}";
        }
	}
}
