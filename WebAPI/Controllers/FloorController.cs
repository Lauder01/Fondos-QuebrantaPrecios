using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FQP.Entities;
using FQP.Enums;

namespace WebAPI.Controllers
{
    public class FloorController : Controller
    {
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
    }
}
