namespace ClassLibraryProject.Entities
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public District AddressDistrict { get; set; } = new District();
        public Street AddressStreet { get; set; } = new Street();
        public string Doorway { get; set; } = string.Empty;
        public bool IsApartment { get; set; } = false;
        public Floor? AddressFloor { get; set; } = new Floor();
        public string? Door { get; set; } = string.Empty;

        public Address() { }

        public string BuildingAddressToString()
        {
            return $"{AddressStreet.ToString()} {Doorway}, {AddressDistrict.ZipCode} {AddressDistrict.City}, {AddressDistrict.Country}";
        }
        public string ApartmentAddressToString()
        {
            var floorNumber = AddressFloor?.FloorNumber.ToString() ?? string.Empty;
            return $"{AddressStreet.ToString()} {Doorway}, {floorNumber}º {Door}, {AddressDistrict.ZipCode} {AddressDistrict.City}, {AddressDistrict.Country}";
        }
    }
}
