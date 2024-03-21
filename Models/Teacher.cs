using Escape.Web.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escape.Web.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Surename { get; set; }
        public int Age { get; set; }
        public string About { get; set; }
        public string Subject { get; set; }
        public string Image { get; set; }
        public bool IsExpert { get; set; }

        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
