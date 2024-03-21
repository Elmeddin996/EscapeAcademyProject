using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TeacherController : Controller
    {
        private readonly EscapeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TeacherController(EscapeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Teachers.AsQueryable();

            if (search != null)
                query = query.Where(x => x.Name.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Teacher>.Create(query, page, 6));
        }

        public IActionResult Create() 
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Teacher teacher)
        {
           
            teacher.Image = FileManager.Save(_env.WebRootPath, "uploads/teachers", teacher.ImageFile);
            


            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Teacher teacher = _context.Teachers.Find(id);

            if (teacher == null) return StatusCode(404);

            return View(teacher);
        }

        [HttpPost]
        public IActionResult Edit(Teacher teacher)
        {
            Teacher existTeacher = _context.Teachers.Find(teacher.Id);

            if (existTeacher == null) return StatusCode(404);


            string oldImage = null;
            if (teacher.ImageFile != null)
            {
                oldImage = existTeacher.Image;

                if (teacher.Image == null)
                {
                    teacher.Image = FileManager.Save(_env.WebRootPath, "uploads/teachers", teacher.ImageFile);
                    existTeacher.Image = teacher.Image;
                }
                else
                    teacher.Image = FileManager.Save(_env.WebRootPath, "uploads/teachers", teacher.ImageFile);
            }
            

            existTeacher.Name = teacher.Name;
            existTeacher.Surename = teacher.Surename;

            _context.SaveChanges();

            if (oldImage != null) FileManager.Delete(_env.WebRootPath, "uploads/teachers", oldImage);


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Teacher teacher = _context.Teachers.Find(id);

            if (teacher == null) return StatusCode(404);

            _context.Teachers.Remove(teacher);
            _context.SaveChanges();

            return StatusCode(200);
        }
    }
}
