using ASPDotNetApp.Daos;
using ASPDotNetApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetApp.Controllers
{
    public class PriceListsController : Controller
    {
        private readonly IPriceListDao _priceListDao;
        private readonly IArticleDao _articleDao;

        public PriceListsController(IPriceListDao priceListDao, IArticleDao articleDao)
        {
            _priceListDao = priceListDao;
            _articleDao = articleDao;
        }               

        // GET: PriceListController/5
        public async Task<IActionResult> Index(int id)
        {
            var priceLists = await _priceListDao.GetAll(id);
            var article = await _articleDao.Get(id);
            var dataTuple = new Tuple<Article, IEnumerable<PriceList>>(article, priceLists);
            return View(dataTuple);
        }

        // GET: PriceListController/Create
        public IActionResult Create(int articleId)
        {
            ViewBag.ArticleId = articleId;
            return View();
        }

        // POST: PriceListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PriceList priceList)
        {
            if (ModelState.IsValid)
            {
                var isCreateSuccessful = await _priceListDao.Add(priceList);
                if (isCreateSuccessful)
                {
                    TempData["success"] = "Price list created successfully";
                    return RedirectToAction("Index", new { id = priceList.ArticleId });
                }
                else
                {
                    TempData["error"] = "Error creating Price list";
                }
            }
            ViewBag.ArticleId = priceList.ArticleId;
            return View(priceList);
        }

        // GET: PriceListController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var priceListFromDb = await _priceListDao.Get(id);
            if (priceListFromDb == null)
            {
                return NotFound();
            }
            ViewBag.ArticleId = priceListFromDb.ArticleId;
            return View(priceListFromDb);
        }

        // POST: PriceListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PriceList priceList)
        {
            if (ModelState.IsValid)
            {
                var isUpdateSuccessful = await _priceListDao.Update(priceList);
                if (isUpdateSuccessful)
                {
                    TempData["success"] = "Price list updated successfully";
                }
                else
                {
                    TempData["error"] = "Error updating Price list!";
                }
                return RedirectToAction("Index", new { id = priceList.ArticleId });
            }
            ViewBag.ArticleId = priceList.ArticleId;
            return View(priceList);
        }

        // GET: PriceListController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var priceListFromDb = await _priceListDao.Get(id);
            if (priceListFromDb == null)
            {
                return NotFound();
            }
            ViewBag.ArticleId = priceListFromDb.ArticleId;
            return View(priceListFromDb);
        }

        // POST: PriceListController/DeletePost/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var priceListFromDb = await _priceListDao.Get(id);
            var articleId = priceListFromDb?.ArticleId;
            var isDeleteSuccessful = await _priceListDao.Delete(id);
            if (isDeleteSuccessful)
            {
                TempData["success"] = "Price list deleted successfully";
            }
            else
            {
                TempData["error"] = "Error deleting price list!";
            }
            return RedirectToAction("Index", new { id = articleId });
        }

        // GET: PriceListController/Prices
        public async Task<IActionResult> SearchByDate(DateTime searchDate)
        {
            var priceLists = await _priceListDao.SearchValidPrices(searchDate);
            ViewBag.SearchDate = searchDate;
            return View(priceLists);
        }
    }
}
