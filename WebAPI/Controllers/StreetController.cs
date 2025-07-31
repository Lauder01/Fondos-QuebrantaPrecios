using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FQP.Entities;
using FQP.Enums;

namespace WebAPI.Controllers
{
    public class StreetController : Controller
    {
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
    }
}
