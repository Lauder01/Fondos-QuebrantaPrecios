using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Dtos;
using FQP.Entities;
using System.Linq;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BuildingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BuildingDto>> GetAll()
        {
            var buildings = _context.Buildings.Select(b => new BuildingDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Code = b.Code,
                Doorway = b.Doorway,
                FloorCount = b.FloorCount,
                YearBuilt = b.YearBuilt,
                Price = b.Price,
                DistrictName = b.BuildingDistrict != null ? b.BuildingDistrict.Name : null,
                StreetName = b.BuildingStreet != null ? b.BuildingStreet.Name : null
            }).ToList();
            return Ok(buildings);
        }

        [HttpPost]
        public ActionResult<BuildingDto> Create(CreateBuildingDto dto)
        {
            var district = _context.Districts.Find(dto.DistrictId);
            var street = _context.Streets.Find(dto.StreetId);
            if (district == null || street == null) return BadRequest("District or Street not found");
            var building = new Building
            {
                Name = dto.Name,
                Description = dto.Description,
                Doorway = dto.Doorway,
                FloorCount = dto.FloorCount,
                YearBuilt = dto.YearBuilt,
                Price = dto.Price,
                BuildingDistrict = district,
                BuildingStreet = street
            };
            _context.Buildings.Add(building);
            _context.SaveChanges();
            var result = new BuildingDto
            {
                Id = building.Id,
                Name = building.Name,
                Description = building.Description,
                Code = building.Code,
                Doorway = building.Doorway,
                FloorCount = building.FloorCount,
                YearBuilt = building.YearBuilt,
                Price = building.Price,
                DistrictName = district.Name,
                StreetName = street.Name
            };
            return CreatedAtAction(nameof(GetAll), new { id = building.Id }, result);
        }
    }
}
