using Escape.DAL;
using Escape.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escape.Controllers
{
    public class CoursesController : Controller
    {
        private readonly EscapeDbContext _context;

        public CoursesController(EscapeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Course> courses = _context.Courses.Include(x=>x.Teacher).ToList();

            return View(courses);
        }
    }
}
