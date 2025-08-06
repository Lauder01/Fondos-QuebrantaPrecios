using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using ClassLibraryProject.Enums;

namespace WebAPI.Controllers
{
    public class AdressController : Controller
    {
        // GET: AdressController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdressController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdressController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdressController/Create
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

        // GET: AdressController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdressController/Edit/5
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

        // GET: AdressController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdressController/Delete/5
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
