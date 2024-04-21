using Escape.DAL;
using Escape.Models;
using Escape.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Escape.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("manage")]
    public class OnlineLessonController : Controller
    {
        private readonly EscapeDbContext _context;
        private readonly IEmailSender _emailSender;

        public OnlineLessonController(EscapeDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(OnlineLesson oLesson)
        {
           List<AppUser> users = _context.AppUsers.ToList();

             string messageBody = $@"
             <p><strong>{oLesson.Title}</strong></p>
             <p><strong>Link:</strong> {oLesson.Link}</p>
             <p><strong>Məlumat:</strong> {oLesson.Description}</p>";

            foreach (AppUser user in users)
            {
                _emailSender.Send(user.Email, "Escape Academy Online Dərs", messageBody);
            }
            return Redirect("dashboard");
        }
    }
}
