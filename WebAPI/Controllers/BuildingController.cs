using Microsoft.AspNetCore.Mvc;
using FQP.Enums;
using FQP.Entities;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBuildings()
        {
            var district = new District
            {
                Name = "Centro",
                ZipCode = "31001",
                City = "Pamplona",
                Country = "Spain"
            };
            var street = new Street
            {
                Name = "Avenida de Navarra",
                AddressStreetType = StreetTypeEnum.Avenida
            };
            var building = new Building
            {
                Name = "Edificio Central",
                BuildingDistrict = district,
                Doorway = "12A"
            };
            var buildingAddress = new Address(building, null, street, false);
            building.BuildingAdress = buildingAddress;

            var buildings = new List<object>
            {
                new
                {
                    building.Id,
                    building.Name,
                    building.Doorway,
                    District = new { district.Name, district.ZipCode, district.City, district.Country },
                    Address = building.BuildingAdress.BuildingAddressToString()
                }
            };

            return Ok(buildings);
        }
    }
}
