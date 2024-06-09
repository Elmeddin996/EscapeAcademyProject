using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Escape.DAL;
using Escape.Models;
using Escape.ViewModels;
using System.Data;
using Escape.Web.Models;
using Escape.Helpers;

namespace Escape.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("manage")]
    public class CategorieController : Controller
    {
        private readonly EscapeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategorieController(EscapeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Categories.AsQueryable();

            if (search != null)
                query = query.Where(x => x.CategoryName.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Categorie>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Categorie categorie)
        {

            categorie.Image = FileManager.Save(_env.WebRootPath, "uploads/categories", categorie.ImageFile);

            _context.Categories.Add(categorie);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Categorie categorie = _context.Categories.Find(id);

            if (categorie == null) return View("Error");

            return View(categorie);
        }

        [HttpPost]
        public IActionResult Edit(Categorie categorie)
        {

            Categorie existCategorie = _context.Categories.Find(categorie.Id);

            if (existCategorie == null) return View("Error");

            if (categorie.CategoryName != existCategorie.CategoryName && _context.Categories.Any(x => x.CategoryName == categorie.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "CategoryName is already taken");
                return View();
            }

            string oldImage = null;
            if (categorie.ImageFile != null)
            {
                oldImage = existCategorie.Image;

                if (categorie.Image == null)
                {
                    categorie.Image = FileManager.Save(_env.WebRootPath, "uploads/categories", categorie.ImageFile);
                    existCategorie.Image = categorie.Image;
                }
                else
                    categorie.Image = FileManager.Save(_env.WebRootPath, "uploads/categories", categorie.ImageFile);
            }
            else
            {
                categorie.Image = existCategorie.Image;
            }

            existCategorie.CategoryName = categorie.CategoryName;

            _context.SaveChanges();

              if (oldImage != null) FileManager.Delete(_env.WebRootPath, "uploads/categories", oldImage);


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Categorie categorie = _context.Categories.Find(id);

            if (categorie == null) return StatusCode(404);

            _context.Categories.Remove(categorie);
            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "uploads/categories", categorie.Image);

            return StatusCode(200);
        }
    }
}
