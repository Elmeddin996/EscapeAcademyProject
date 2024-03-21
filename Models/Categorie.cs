using Escape.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escape.Web.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string Image { get; set; }

        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public List<Course> Courses { get; set; }
    }
}
