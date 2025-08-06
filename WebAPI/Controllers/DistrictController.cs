using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Enums;
using ClassLibraryProject.Entities;
using WebAPI.Data;
using WebAPI.Dtos;
using System.Linq;

namespace WebAPI.Controllers
{
    public class DistrictController : Controller
    {
        private readonly AppDbContext _context;
        public DistrictController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DistrictController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DistrictController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DistrictController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DistrictController/Create
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

        // GET: DistrictController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DistrictController/Edit/5
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

        // GET: DistrictController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DistrictController/Delete/5
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
        public ActionResult<IEnumerable<DistrictDto>> GetAll()
        {
            var districts = _context.Districts.Select(d => new DistrictDto
            {
                Id = d.Id,
                Name = d.Name,
                ZipCode = d.ZipCode,
                Country = d.Country,
                City = d.City
            }).ToList();
            return Ok(districts);
        }

        [HttpPost]
        public ActionResult<DistrictDto> Create(CreateDistrictDto dto)
        {
            var district = new District
            {
                Name = dto.Name,
                ZipCode = dto.ZipCode,
                Country = dto.Country,
                City = dto.City
            };
            _context.Districts.Add(district);
            _context.SaveChanges();
            var result = new DistrictDto
            {
                Id = district.Id,
                Name = district.Name,
                ZipCode = district.ZipCode,
                Country = district.Country,
                City = district.City
            };
            return CreatedAtAction(nameof(GetAll), new { id = district.Id }, result);
        }
    }
}
