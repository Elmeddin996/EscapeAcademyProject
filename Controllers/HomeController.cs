using Escape.DAL;
using Escape.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escape.Controllers
{
    public class HomeController : Controller
    {
        private readonly EscapeDbContext _context;
        public HomeController(EscapeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel
            {
                Sliders = _context.Sliders.ToList(),
                PopularCourses = _context.Courses.Where(x => x.IsPopular).Take(3).ToList(),
                ExpertTeachers = _context.Teachers.Where(x => x.IsExpert).Take(4).ToList(),
                Students = _context.Students.Take(4).ToList(),
                Categories=_context.Categories.Include(x => x.Courses).ToList()
            };

            return View(vm);
        }
    }
}
