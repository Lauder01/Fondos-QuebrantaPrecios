using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using System.Linq;

namespace WebAPI.Controllers
{
    public class FloorController : Controller
    {
        private readonly AppDbContext _context;
        public FloorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FloorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FloorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FloorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FloorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FloorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FloorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FloorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FloorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<FloorDto>> GetAll()
        {
            var floors = _context.Floors.Select(f => new FloorDto
            {
                Id = f.Id,
                FloorNumber = f.FloorNumber,
                HasLift = f.HasLift,
                BuildingId = f.BuildingFloor != null ? f.BuildingFloor.Id : Guid.Empty
            }).ToList();
            return Ok(floors);
        }

        [HttpPost]
        public ActionResult<FloorDto> Create(CreateFloorDto dto)
        {
            var building = _context.Buildings.Find(dto.BuildingId);
            if (building == null) return BadRequest("Building not found");
            var floor = new Floor
            {
                FloorNumber = dto.FloorNumber,
                HasLift = dto.HasLift,
                BuildingFloor = building
            };
            _context.Floors.Add(floor);
            _context.SaveChanges();
            var result = new FloorDto
            {
                Id = floor.Id,
                FloorNumber = floor.FloorNumber,
                HasLift = floor.HasLift,
                BuildingId = building.Id
            };
            return CreatedAtAction(nameof(GetAll), new { id = floor.Id }, result);
        }
    }
}
