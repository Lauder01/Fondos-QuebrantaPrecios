using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Dtos;
using ClassLibraryProject.Entities;
using System.Linq;

namespace WebAPI.Controllers
{
    public class FQP_UserController : Controller
    {
        private readonly AppDbContext _context;
        public FQP_UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FQP_UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FQP_UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FQP_UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FQP_UserController/Create
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

        // GET: FQP_UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FQP_UserController/Edit/5
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

        // GET: FQP_UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FQP_UserController/Delete/5
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
        public ActionResult<IEnumerable<FQPUserDto>> GetAll()
        {
            var users = _context.Users.Select(u => new FQPUserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive
            }).ToList();
            return Ok(users);
        }

        [HttpPost]
        public ActionResult<FQPUserDto> Create(CreateFQPUserDto dto)
        {
            var user = new FQP_User
            {
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = dto.Password, // Aquí deberías hashear la contraseña
                IsActive = true
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            var result = new FQPUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive
            };
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, result);
        }
    }
}
