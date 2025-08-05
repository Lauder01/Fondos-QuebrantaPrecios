using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FQP.Entities;
using FQP.Enums;
using WebAPI.Data;
using WebAPI.Dtos;
using System.Linq;

namespace WebAPI.Controllers
{
    public class StreetController : Controller
    {
        private readonly AppDbContext _context;
        public StreetController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StreetController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StreetController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StreetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StreetController/Create
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

        // GET: StreetController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StreetController/Edit/5
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

        // GET: StreetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StreetController/Delete/5
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
        public ActionResult<IEnumerable<StreetDto>> GetAll()
        {
            var streets = _context.Streets.Select(s => new StreetDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                AddressStreetType = s.AddressStreetType
            }).ToList();
            return Ok(streets);
        }

        [HttpPost]
        public ActionResult<StreetDto> Create(CreateStreetDto dto)
        {
            var street = new Street
            {
                Name = dto.Name,
                AddressStreetType = dto.AddressStreetType,
                Code = string.Empty // O usa lógica para generar el código
            };
            _context.Streets.Add(street);
            _context.SaveChanges();
            var result = new StreetDto
            {
                Id = street.Id,
                Name = street.Name,
                Code = street.Code,
                AddressStreetType = street.AddressStreetType
            };
            return CreatedAtAction(nameof(GetAll), new { id = street.Id }, result);
        }
    }
}
