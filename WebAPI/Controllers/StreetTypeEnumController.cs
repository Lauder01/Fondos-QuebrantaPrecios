using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FQP.Entities;
using FQP.Enums;

namespace WebAPI.Controllers
{
    public class StreetTypeEnumController : Controller
    {
        // GET: StreetTypeEnumController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StreetTypeEnumController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StreetTypeEnumController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StreetTypeEnumController/Create
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

        // GET: StreetTypeEnumController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StreetTypeEnumController/Edit/5
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

        // GET: StreetTypeEnumController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StreetTypeEnumController/Delete/5
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
