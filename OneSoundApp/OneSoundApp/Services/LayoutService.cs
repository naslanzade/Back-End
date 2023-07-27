using OneSoundApp.Data;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class LayoutService : ILayoutService
    {

        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public Dictionary<string, string> GetSettingDatas() => _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);

    }
}
