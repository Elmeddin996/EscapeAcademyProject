using Escape.DAL;
using Escape.Models;

namespace Escape.Services
{
    public class LayoutService
    {
        private readonly EscapeDbContext _contex;

        public LayoutService(EscapeDbContext contex)
        {
            _contex = contex;
        }

        public List<Settings> GetSettings()
        {
            return _contex.Settings.ToList();
        }
    }
}
