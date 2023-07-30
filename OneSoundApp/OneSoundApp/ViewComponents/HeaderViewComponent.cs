using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Layout;

namespace OneSoundApp.ViewComponents
{
    public class HeaderViewComponent :ViewComponent
    {
        private readonly ILayoutService _layoutService;
        private readonly IWishlistService _wishlistService;
        private readonly ICartService _cartService;

        public HeaderViewComponent(ILayoutService layoutService, 
                                  IWishlistService wishlistService,
                                  ICartService cartService)
        {
            _layoutService = layoutService;
            _wishlistService = wishlistService;
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM model = new()
            {
                Settings = _layoutService.GetSettingDatas(),                
                WishlistCount = _wishlistService.GetAlbumsCount(),
                BasketCount=_cartService.GetCount(),
            };
            return await Task.FromResult(View(model));
        }
    }
}
