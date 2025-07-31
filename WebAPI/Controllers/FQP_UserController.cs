using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FQP.Entities;
using FQP.Enums;

namespace WebAPI.Controllers
{
    public class FQP_UserController : Controller
    {
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
    }
}
