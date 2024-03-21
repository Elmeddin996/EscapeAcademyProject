using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Escape.DAL;
using Escape.Models;
using Escape.ViewModels;
using System.Data;
using Escape.Web.Models;
using Escape.Helpers;

namespace Escape.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("manage")]
    public class CourseController : Controller
    {
        private readonly EscapeDbContext _context;

        public IWebHostEnvironment _env;

        public CourseController(EscapeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Courses
                .Include(x=>x.Teacher).Include(x => x.Categorie).AsQueryable();

            if (search != null)
                query = query.Where(x => x.Name.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Course>.Create(query, page, 6));
        }

        public IActionResult Create()
        {
            ViewBag.Teachers = _context.Teachers.ToList();
            ViewBag.Categories = _context.Categories.ToList();



            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Course course)
        {

            if (!_context.Teachers.Any(x => x.Id == course.TeacherId))
            {
                ModelState.AddModelError("TeacherId", "TeacherId is not correct");
                return View();
            }

            if (!_context.Categories.Any(x => x.Id == course.CategorieId))
            {
                ModelState.AddModelError("CategorieId", "CategorieId is not correct");
                return View();
            }

            course.Image = FileManager.Save(_env.WebRootPath, "uploads/courses", course.ImageFile);
            

            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Teachers = _context.Teachers.ToList();
            ViewBag.Categories = _context.Categories.ToList();

            Course course = _context.Courses.FirstOrDefault(x => x.Id == id);

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {

            Course existCourse = _context.Courses.FirstOrDefault(x => x.Id == course.Id);

            if (existCourse == null) return View("Error");

            if (course.TeacherId != existCourse.TeacherId && !_context.Teachers.Any(x => x.Id == course.TeacherId))
            {
                ModelState.AddModelError("TeacherId", "TeacherId is not correct");
                return View();
            }

            if (course.CategorieId != existCourse.CategorieId && !_context.Categories.Any(x => x.Id == course.CategorieId))
            {
                ModelState.AddModelError("CategorieId", "CategorieId is not correct");
                return View();
            }


            string oldImage = null;
            if (course.ImageFile != null)   
            {
                oldImage = course.Image;
                
                if (course.Image == null)
                {
                    course.Image = FileManager.Save(_env.WebRootPath, "uploads/courses", course.ImageFile);
                    existCourse.Image = course.Image;
                }
                else
                    course.Image = FileManager.Save(_env.WebRootPath, "uploads/courses", course.ImageFile);
            }

            existCourse.Name = course.Name;
            existCourse.Description = course.Description;
            existCourse.IsPopular = course.IsPopular;
            existCourse.CourseTime = course.CourseTime;
            existCourse.StudentCount = course.StudentCount;
            existCourse.TeacherId = course.TeacherId;
            existCourse.CategorieId = course.CategorieId;

            _context.SaveChanges();


            if (oldImage != null) FileManager.Delete(_env.WebRootPath, "uploads/courses", oldImage);

           
            return RedirectToAction("index");
        }


        public IActionResult Delete (int id)
        {
            Course course =_context.Courses.Find(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }


}
