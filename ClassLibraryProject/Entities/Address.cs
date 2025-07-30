namespace ClassLibraryProject.Entities
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Building? AddressBuilding { get; set; } = null;
        public Apartment? AddressApartment { get; set; } = null;
        public Street? AddressStreet { get; set; } = null;
        public bool IsApartment { get; set; } = false;
        public string AddressCountry { get; set; } = "Spain";
        public string AddressCity { get; set; } = "Pamplona";
        public string AddressZipCode { get; set; } = "ZZZ";
        public string AddressDoorway { get; set; } = "0";
        public int AddressFloor { get; set; } = -666;
        public string AddressDoor { get; set; } = "0";

        public Address() { }

        public Address(Building building, Apartment apartment, Street street, bool isApartment)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            AddressBuilding = building;
            AddressApartment = apartment;
            AddressStreet = street;
            IsApartment = isApartment;
            AddressCountry = AddressBuilding?.BuildingDistrict?.Country ?? "Spain";
            AddressCity = AddressBuilding?.BuildingDistrict?.City ?? "Pamplona";
            AddressZipCode = AddressBuilding?.BuildingDistrict?.ZipCode ?? "ZZZ";
            AddressDoorway = AddressBuilding?.Doorway ?? "0";
            AddressFloor = AddressApartment?.ApartmentFloor?.FloorNumber ?? -666;
            AddressDoor = AddressApartment?.Door ?? "Z";
        }

        public string BuildingAddressToString()
        {
            return $"{AddressStreet?.ToString()} {AddressDoorway}, {AddressZipCode} {AddressCity}, {AddressCountry}";
        }
        public string ApartmentAddressToString()
        {
            return $"{AddressStreet?.ToString()} {AddressDoorway}, {AddressFloor}º{AddressDoor}, {AddressZipCode} {AddressCity}, {AddressCountry}";
        }
    }
}
