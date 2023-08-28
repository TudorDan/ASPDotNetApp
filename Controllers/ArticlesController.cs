using ASPDotNetApp.Daos;
using ASPDotNetApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleDao _iarticleDao;

        public ArticlesController(IArticleDao articleDao)
        {
            _iarticleDao = articleDao;
        }

        // GET: ArticlesController
        public async Task<IActionResult> Index()
        {
            var articlesList = await _iarticleDao.GetAll();
            return View(articlesList);
        }

        // GET: ArticlesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArticlesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                var isCreateSuccessful = await _iarticleDao.Add(article);
                if (isCreateSuccessful)
                {
                    TempData["success"] = $"Article {article.Name} created successfully";
                }
                else
                {
                    TempData["error"] = $"Error creating Article {article.Name}!";
                }
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: ArticlesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var articleFromDb = await _iarticleDao.Get(id);
            if (articleFromDb == null)
            {
                return NotFound();
            }
            return View(articleFromDb);
        }

        // POST: ArticlesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                var isUpdateSuccessful = await _iarticleDao.Update(article);
                if (isUpdateSuccessful)
                {
                    TempData["succes"] = $"Article {article.Name} updated successfully";
                }
                else
                {
                    TempData["error"] = $"Error updating Article {article.Name}!";
                }
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: ArticlesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var articleFromDb = await _iarticleDao.Get(id);
            if (articleFromDb == null)
            {
                return NotFound();
            }
            return View(articleFromDb);
        }

        // POST: ArticlesController/DeletePost/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var isDeleteSuccessful = await _iarticleDao.Delete(id);
            if (isDeleteSuccessful)
            {
                TempData["success"] = $"Article deleted successfully";
            }
            else
            {
                TempData["error"] = $"Article deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}
