using Escape.Models;
using Escape.Web.Models;

namespace Escape.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List <Teacher> ExpertTeachers { get; set; }
        public List<Categorie> Categories { get; set; }
        public List<Course> PopularCourses { get; set; }
        public List<Student>Students { get; set; }
    }
}
