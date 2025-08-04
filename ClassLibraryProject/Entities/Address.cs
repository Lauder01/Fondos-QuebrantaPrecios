using System;

namespace FQP.Entities
{
    public class Address
    {
        // Remote
        public Guid Id { get; set; } = Guid.NewGuid();
        public Building? AddressBuilding { get; set; } = null;
        public Apartment? AddressApartment { get; set; } = null;
        public bool IsApartment { get; set; } = false;
        public string AddressCountry { get; set; } = "Spain";
        public string AddressCity { get; set; } = "Pamplona";

        // Local
        public string AddressZipCode { get; } = "ZZZ";
        public Street AddressStreet { get; private set; } = default!;
        public string AddressDoorway { get; } = "0";
        public int AddressFloor { get; } = -666;
        public string AddressDoor { get; } = "0";

        public Address()
        { }

        public Address(Building building, Apartment apartment, bool isApartment)
        {
            Id = Guid.NewGuid();
            AddressBuilding = building;
            AddressApartment = apartment;
            IsApartment = isApartment;
            AddressCountry = AddressBuilding?.BuildingDistrict?.Country ?? "Spain";
            AddressCity = AddressBuilding?.BuildingDistrict?.City ?? "Pamplona";
            AddressZipCode = AddressBuilding?.BuildingDistrict?.ZipCode ?? "ZZZ";
            AddressStreet = AddressBuilding?.BuildingStreet ?? default!;
            AddressDoorway = AddressBuilding?.Doorway ?? "0";
            AddressFloor = AddressApartment?.ApartmentFloor?.FloorNumber ?? -666;
            AddressDoor = AddressApartment?.Door ?? "Z";
        }

        public string BuildingAddressToString()
        {
            return $"{AddressStreet?.Name} {AddressDoorway}, {AddressZipCode} {AddressCity}, {AddressCountry}";
        }
        public string ApartmentAddressToString()
        {
            return $"{AddressStreet?.Name} {AddressDoorway}, {AddressFloor}º{AddressDoor}, {AddressZipCode} {AddressCity}, {AddressCountry}";
        }
    }
}
