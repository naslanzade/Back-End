using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;

namespace OneSoundApp.Areas.Admin.Controllers
{

    [Area("Admin")]
    
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
             
            
           
        
    }
}
