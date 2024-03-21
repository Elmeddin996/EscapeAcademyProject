using Escape.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escape.Web.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategorieId { get; set; }
        public int TeacherId { get; set; }
        public bool IsPopular { get; set; }
        public double CourseTime { get; set; }
        public int StudentCount { get; set; }
        public string Image { get; set; }

        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public Teacher Teacher { get; set; }
        public Categorie Categorie { get; set; }
    }
}
