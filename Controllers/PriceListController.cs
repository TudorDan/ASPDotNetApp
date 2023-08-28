using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetApp.Controllers
{
    public class PriceListController : Controller
    {
        // GET: PriceListController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PriceListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PriceListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PriceListController/Create
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

        // GET: PriceListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PriceListController/Edit/5
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

        // GET: PriceListController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PriceListController/Delete/5
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
