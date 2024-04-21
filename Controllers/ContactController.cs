using Escape.DAL;
using Escape.Models;
using Escape.Services;
using Escape.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Escape.Controllers
{
    public class ContactController : Controller
    {
        private readonly EscapeDbContext _context;
        private readonly IEmailSender _emailSender;

        public ContactController(EscapeDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            Settings settings = _context.Settings.FirstOrDefault();
            return View(settings);
        }

        [HttpPost]
        public IActionResult Index(ContactPageMessageViewModel vm)
        {
            Settings settings = _context.Settings.FirstOrDefault();

            string messageBody = $@"
             <p><strong>Ad Soyad:</strong> {vm.Fullname}</p>
             <p><strong>Telefon:</strong> {vm.Phone}</p>
             <p><strong>E-Mail:</strong> {vm.Email}</p>
             <p><strong>Mesaj:</strong> {vm.Message}</p>";

            _emailSender.Send(settings.Email, "Saytadan Bir Başa Mesaj", messageBody);
            return View(settings);
        }
    }
}
