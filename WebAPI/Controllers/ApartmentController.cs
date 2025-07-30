using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using ClassLibraryProject.Enums;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetApartments()
        {
            // Mock: create a sample list of apartments
            var district = new District
            {
                Name = "Centro",
                ZipCode = "31001",
                City = "Pamplona",
                Country = "Spain"
            };
            var building = new Building
            {
                Name = "Edificio Central",
                BuildingDistrict = district,
                Doorway = "12A"
            };
            var floor = new Floor
            {
                FloorNumber = 2,
                BuildingFloor = building
            };
            var street = new Street
            {
                Name = "Avenida de Navarra",
                AddressStreetType = StreetTypeEnum.Avenida
            };
            var apartment = new Apartment
            {
                ApartmentFloor = floor,
                ApartmentBuilding = building,
                Door = "B"
            };
            var apartmentAddress = new Address(building, apartment, street, true);
            apartment.ApartmentAddress = apartmentAddress;

            var apartments = new List<object>
            {
                new
                {
                    apartment.Id,
                    apartment.Door,
                    Floor = floor.FloorNumber,
                    Building = new { building.Id, building.Name, building.Doorway },
                    District = new { district.Name, district.ZipCode, district.City, district.Country },
                    Address = apartment.ApartmentAddress.ApartmentAddressToString()
                }
            };

            return Ok(apartments);
        }
    }
}
