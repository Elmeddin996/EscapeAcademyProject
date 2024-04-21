using Escape.DAL;
using Escape.Helpers;
using Escape.Models;
using Escape.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Escape.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("manage")]
    public class SettingsController : Controller
    {
        private readonly EscapeDbContext _context;

        public SettingsController(EscapeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Settings settings = _context.Settings.FirstOrDefault();
            return View(settings);
        }

        public IActionResult Edit(int id)
        {
            Settings settings = _context.Settings.Find(id);
            if (settings == null) return StatusCode(404);

            return View(settings);
        }

        [HttpPost]
        public IActionResult Edit(Settings settings)
        {
            Settings existSettings = _context.Settings.Find(settings.Id);
            if (existSettings == null) return StatusCode(404);

            existSettings.Address= settings.Address;
            existSettings.Email= settings.Email;
            existSettings.Phone= settings.Phone;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
