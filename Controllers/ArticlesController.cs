using ASPDotNetApp.Daos;
using ASPDotNetApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleDao _articleDao;

        public ArticlesController(IArticleDao articleDao)
        {
            _articleDao = articleDao;
        }

        // GET: ArticlesController
        public async Task<IActionResult> Index()
        {
            var articlesList = await _articleDao.GetAll();
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
                try
                {
                    var isCreateSuccessful = await _articleDao.Add(article);
                    if (isCreateSuccessful)
                    {
                        TempData["success"] = $"Article {article.Name} created successfully";
                    }
                    else
                    {
                        TempData["error"] = $"Error creating Article {article.Name}!";
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Database error: {ex.Message}";
                }
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: ArticlesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var articleFromDb = await _articleDao.Get(id);
            if (articleFromDb == null)
            {
                return NotFound();
            }
            return View(articleFromDb);
        }

        // POST: ArticlesController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isUpdateSuccessful = await _articleDao.Update(article);
                    if (isUpdateSuccessful)
                    {
                        TempData["succes"] = $"Article {article.Name} updated successfully";
                    }
                    else
                    {
                        TempData["error"] = $"Error updating Article {article.Name}!";
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Database error: {ex.Message}";
                }                
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: ArticlesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var articleFromDb = await _articleDao.Get(id);
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
            var isDeleteSuccessful = await _articleDao.Delete(id);
            if (isDeleteSuccessful)
            {
                TempData["success"] = "Article deleted successfully";
            }
            else
            {
                TempData["error"] = "Error deleting article!";
            }
            return RedirectToAction("Index");
        }
    }
}
