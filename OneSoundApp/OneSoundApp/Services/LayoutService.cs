using OneSoundApp.Data;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Layout;

namespace OneSoundApp.Services
{
    public class LayoutService : ILayoutService
    {

        private readonly AppDbContext _context;
        private readonly IWishlistService _wishlistService;

        public LayoutService(AppDbContext context, 
                             IWishlistService wishlistService)
        {
            _context = context;
            _wishlistService = wishlistService;
        }

    

        public Dictionary<string, string> GetSettingDatas() => _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);

    }
}
