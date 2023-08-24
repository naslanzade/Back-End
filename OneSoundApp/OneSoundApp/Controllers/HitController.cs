using Microsoft.AspNetCore.Mvc;

namespace OneSoundApp.Controllers
{
    public class HitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
