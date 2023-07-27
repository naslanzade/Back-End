using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Layout;

namespace OneSoundApp.ViewComponents
{
    public class HeaderViewComponent :ViewComponent
    {
        private readonly ILayoutService _layoutService;

        public HeaderViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM model = new()
            {
                Settings = _layoutService.GetSettingDatas(),
               
            };
            return await Task.FromResult(View(model));
        }
    }
}
