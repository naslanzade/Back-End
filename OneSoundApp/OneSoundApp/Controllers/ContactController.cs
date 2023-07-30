using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Contact;

namespace OneSoundApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILayoutService _layoutService;

        public ContactController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        public async Task<IActionResult> Index()
        {

            //ContactVM model = new()
            //{
            //    Settings = _layoutService.GetSettingDatas()
            //};
            return View();
        }
    }
}
