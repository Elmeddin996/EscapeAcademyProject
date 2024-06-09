using Escape.DAL;
using Escape.Helpers;
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
    public class StudentController : Controller
    {
        private readonly EscapeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public StudentController(EscapeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Students.AsQueryable();

            if (search != null)
                query = query.Where(x => x.FullName.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Student>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Student student)
        {

            student.Image = FileManager.Save(_env.WebRootPath, "uploads/students", student.ImageFile);



            _context.Students.Add(student);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Student student = _context.Students.Find(id);

            if (student == null) return StatusCode(404);

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            Student existStudent = _context.Students.Find(student.Id);

            if (existStudent == null) return StatusCode(404);


            string oldImage = null;
            if (student.ImageFile != null)
            {
                oldImage = existStudent.Image;

                if (student.Image == null)
                {
                    student.Image = FileManager.Save(_env.WebRootPath, "uploads/students", student.ImageFile);
                    existStudent.Image = student.Image;
                }
                else
                    student.Image = FileManager.Save(_env.WebRootPath, "uploads/students", student.ImageFile);
            }


            existStudent.FullName = student.FullName;
            existStudent.Profession = student.Profession;
            existStudent.Comment = student.Comment;

            _context.SaveChanges();

            if (oldImage != null) FileManager.Delete(_env.WebRootPath, "uploads/students", oldImage);


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Student student = _context.Students.Find(id);

            if (student == null) return StatusCode(404);

            _context.Students.Remove(student);
            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath, "uploads/students", student.Image);

            return StatusCode(200);
        }
    }
}
