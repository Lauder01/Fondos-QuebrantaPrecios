using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using ClassLibraryProject.Enums;
using WebAPI.Data;
using WebAPI.Dtos;
using System.Linq;

namespace WebAPI.Controllers
{
    public class StatusController : Controller
    {
        private readonly AppDbContext _context;
        public StatusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StatusController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StatusController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatusController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatusController/Create
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

        // GET: StatusController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatusController/Edit/5
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

        // GET: StatusController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatusController/Delete/5
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
        public ActionResult<IEnumerable<StatusDto>> GetAll()
        {
            var statuses = _context.Statuses.Select(s => new StatusDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();
            return Ok(statuses);
        }

        [HttpPost]
        public ActionResult<StatusDto> Create(CreateStatusDto dto)
        {
            var status = new Status
            {
                Name = dto.Name,
                Description = dto.Description
            };
            _context.Statuses.Add(status);
            _context.SaveChanges();
            var result = new StatusDto
            {
                Id = status.Id,
                Name = status.Name,
                Description = status.Description
            };
            return CreatedAtAction(nameof(GetAll), new { id = status.Id }, result);
        }
    }
}
