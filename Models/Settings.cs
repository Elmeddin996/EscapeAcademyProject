using System.ComponentModel.DataAnnotations;

namespace Escape.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string Address { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
