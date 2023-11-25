using Coffee.Models.Entities;
using Coffee.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coffee.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private NewsRepository _newsRepository;
        private DataRepository _dataRepository;

        public AdminController(NewsRepository newsRepository
            , DataRepository dataRepository)
        {
            _newsRepository = newsRepository;
            _dataRepository = dataRepository;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            bool isAdmin = User.IsInRole("Administrator");

            return View();
        }

        public async Task<ActionResult> Users()
        {
            var list = await _dataRepository.GetUsersAsync();

            return View(list);
        }

        [Route("/admin/users/block/{id}")]
        public async Task<ActionResult> BlockUsers(string id)
        {
            await _dataRepository.BlockUsersAsync(id);

            return Redirect("/Admin/Users");
        }

        [Route("/admin/users/unblock/{id}")]
        public async Task<ActionResult> UnBlockUsers(string id)
        {
            await _dataRepository.UnBlockUsersAsync(id);

            return Redirect("/Admin/Users");
        }

        public async Task<ActionResult> News()
        {
            var listNews = await _newsRepository.GetNewsAsync();

            return View(listNews);
        }

        [Route("/admin/news/createNews")]
        [HttpGet]
        public async Task<ActionResult> CreateNews()
        {
            return View();
        }

        [Route("/admin/news/createNews")]
        [HttpPost]
        public async Task<ActionResult> Create(News news)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                news.AuthorId = userId;

                news.Date = DateTime.SpecifyKind(news.Date, DateTimeKind.Utc);

                var result = await _newsRepository.CreateNewsAsync(news);
            }

            return Redirect("/Admin/News");
        }

        [Route("/admin/news/edit/{id}")]
        [HttpGet]
        public async Task<ActionResult> EditNews(int id)
        {
            var news = await _newsRepository.GetOneNewsAsync(id);

            return View(news);
        }

        [Route("/admin/news/edit/{id}")]
        [HttpPost]
        public async Task<ActionResult> Edit(News news)
        {
            news.Date = DateTime.SpecifyKind(news.Date, DateTimeKind.Utc);

            var result = await _newsRepository.UpdateNewsAsync(news);

            return Redirect("/Admin/News");
        }

        [Route("/admin/news/delete/{id}")]
        [HttpGet]
        public async Task<ActionResult> DeleteNews(int id)
        {
            await _newsRepository.DeleteNewsAsync(id);

            return Redirect("/Admin/News");
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
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

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
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

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
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
