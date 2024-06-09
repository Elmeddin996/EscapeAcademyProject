using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Escape.DAL;
using Escape.Helpers;
using Escape.Models;
using Escape.ViewModels;
using System.Data;
using Escape.Web.Models;

namespace Escape.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]

    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly EscapeDbContext _context;

        public IWebHostEnvironment _env { get; }

        public SliderController(EscapeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Sliders.OrderBy(x=>x.Order).AsQueryable();

            
            return View(PaginatedList<Slider>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            ViewBag.NextOrder = _context.Sliders.Any() ? _context.Sliders.Max(x => x.Order) + 1 : 1;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            ViewBag.NextOrder = slider.Order;
            if (!ModelState.IsValid) return View();

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }

            foreach (var item in _context.Sliders.Where(x => x.Order >= slider.Order))
                item.Order++;

            slider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null) return View("Error");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider existSlider = _context.Sliders.Find(slider.Id);

            if (existSlider == null) return View("Error");

            
            string oldFileName = null;
            if (slider.ImageFile != null)
            {
                oldFileName = existSlider.Image;
                existSlider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }

            existSlider.Order = slider.Order;
            existSlider.Title = slider.Title;
            existSlider.BtnUrl = slider.BtnUrl;
            existSlider.ButtonText = slider.ButtonText;
            existSlider.Description = slider.Description;

            _context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/sliders", oldFileName);

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null) return StatusCode(404);

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "uploads/sliders", slider.Image);

            return StatusCode(200);
        }
    }
}
