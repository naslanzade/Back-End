using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Layout;

namespace OneSoundApp.ViewComponents
{
    public class FooterViewComponent :ViewComponent
    {
        private readonly ILayoutService _layoutService;

        public FooterViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM model = new FooterVM()
            {
                Settings = _layoutService.GetSettingDatas(),
              

            };
            return await Task.FromResult(View(model));
        }
    }
}
