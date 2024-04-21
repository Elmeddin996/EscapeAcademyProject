using Escape.DAL;
using Escape.Models;
using Escape.ViewModels;
using Escape.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Escape.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("manage")]
    public class UserController : Controller
    {
        private readonly EscapeDbContext _context;

        public UserController(EscapeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.AppUsers.AsQueryable();

            if (search != null)
                query = query.Where(x => x.Name.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<AppUser>.Create(query, page, 6));
        }

    }
}
