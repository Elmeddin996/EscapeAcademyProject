using System.ComponentModel.DataAnnotations;

namespace Escape.ViewModels
{
    public class ContactPageMessageViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(40)]
        public string Fullname { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
