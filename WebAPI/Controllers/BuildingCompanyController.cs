using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FQP.Entities;
using FQP.Enums;

namespace WebAPI.Controllers
{
    public class BuildingCompanyController : Controller
    {
        // GET: BuildingCompanyController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BuildingCompanyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BuildingCompanyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BuildingCompanyController/Create
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

        // GET: BuildingCompanyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BuildingCompanyController/Edit/5
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

        // GET: BuildingCompanyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BuildingCompanyController/Delete/5
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
