using Escape.DAL;
using Escape.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Escape.Controllers
{
    public class AboutController : Controller
    {
        private readonly EscapeDbContext _context;

        public AboutController(EscapeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           List<Teacher> ExpertTeachers = _context.Teachers.ToList();

            return View(ExpertTeachers);
        }
    }
}
