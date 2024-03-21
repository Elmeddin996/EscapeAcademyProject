using Microsoft.AspNetCore.Identity;

namespace Escape.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public bool IsAdmin { get; set; }
        public string Phone { get; set; }
    }
}
