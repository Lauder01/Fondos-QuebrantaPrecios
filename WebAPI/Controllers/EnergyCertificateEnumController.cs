using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FQP.Entities;
using FQP.Enums;

namespace WebAPI.Controllers
{
    public class EnergyCertificateEnumController : Controller
    {
        // GET: EnergyCertificateEnumController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EnergyCertificateEnumController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EnergyCertificateEnumController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnergyCertificateEnumController/Create
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

        // GET: EnergyCertificateEnumController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EnergyCertificateEnumController/Edit/5
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

        // GET: EnergyCertificateEnumController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EnergyCertificateEnumController/Delete/5
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
