using System;
using ClassLibraryProject.Entities;
using ClassLibraryProject.Enums;

namespace ConsoleProject
{
    public class ExampleUsage
    {
        public static void Main()
        {
            // 1. Create a district with all data
            var district = new District
            {
                Name = "Centro",
                ZipCode = "31001",
                City = "Pamplona",
                Country = "Spain"
            };

            // 2. Create a building in that district
            var building = new Building
            {
                Name = "Edificio Central",
                BuildingDistrict = district,
                Doorway = "12A"
            };

            // 3. Create a floor in that building
            var floor = new Floor
            {
                FloorNumber = 2,
                BuildingFloor = building
            };

            // 4. Create a street
            var street = new Street
            {
                Name = "Avenida de Navarra",
                AddressStreetType = StreetTypeEnum.Avenida
            };

            // 5. Create an apartment in that floor
            var apartment = new Apartment
            {
                ApartmentFloor = floor,
                ApartmentBuilding = building,
                Door = "B"
            };

            // 6. Create the address for the building
            var buildingAddress = new Address(building, null, street, false);
            building.BuildingAdress = buildingAddress;

            // 7. Create the address for the apartment
            var apartmentAddress = new Address(building, apartment, street, true);
            apartment.ApartmentAddress = apartmentAddress;

            // 8. Show the addresses
            Console.WriteLine("District:");
            Console.WriteLine($"Name: {district.Name}, ZipCode: {district.ZipCode}, City: {district.City}, Country: {district.Country}");
            Console.WriteLine();

            Console.WriteLine("Building address:");
            Console.WriteLine(building.BuildingAdress.BuildingAddressToString());
            Console.WriteLine();

            Console.WriteLine("Apartment address:");
            Console.WriteLine(apartment.ApartmentAddress.ApartmentAddressToString());
        }
    }
}