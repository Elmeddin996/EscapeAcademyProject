using Escape.Models;
using Escape.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Escape.DAL
{
    public class EscapeDbContext:IdentityDbContext
    {
        public EscapeDbContext(DbContextOptions<EscapeDbContext> options) : base(options) { }


        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }    
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<OnlineLesson> OnlineLessons { get; set;}
        public DbSet<Student> Students { get; set; }    

    }
}
