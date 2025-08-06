using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using System.Linq;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ApartmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApartmentDto>> GetAll()
        {
            var apartments = _context.Apartments.Select(a => new ApartmentDto
            {
                Id = a.Id,
                Code = a.Code,
                Door = a.Door,
                FloorId = a.ApartmentFloor != null ? a.ApartmentFloor.Id : Guid.Empty,
                BuildingId = a.ApartmentBuilding != null ? a.ApartmentBuilding.Id : Guid.Empty
            }).ToList();
            return Ok(apartments);
        }

        [HttpPost]
        public ActionResult<ApartmentDto> Create(CreateApartmentDto dto)
        {
            var floor = _context.Floors.Find(dto.FloorId);
            var building = _context.Buildings.Find(dto.BuildingId);
            if (floor == null || building == null) return BadRequest("Floor or Building not found");
            var apartment = new Apartment
            {
                Code = dto.Code,
                Door = dto.Door,
                ApartmentFloor = floor,
                ApartmentBuilding = building
            };
            _context.Apartments.Add(apartment);
            _context.SaveChanges();
            var result = new ApartmentDto
            {
                Id = apartment.Id,
                Code = apartment.Code,
                Door = apartment.Door,
                FloorId = floor.Id,
                BuildingId = building.Id
            };
            return CreatedAtAction(nameof(GetAll), new { id = apartment.Id }, result);
        }
    }
}
