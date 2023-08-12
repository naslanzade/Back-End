using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Areas.Admin.ViewModels.Account;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;


namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager,
                                             RoleManager<IdentityRole> roleManager,
                                             SignInManager<AppUser> signInManager,
                                             IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminLoginVM model, string viewName = null, string controllerName = null)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            AppUser user = await _userManager.FindByEmailAsync(model.EmailOrUsername);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.EmailOrUsername);
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong");
                return View(model);
            }
            ViewBag.UserId = await _userManager.FindByNameAsync(model.EmailOrUsername);
            viewName = "Index";
            controllerName = "Dashboard";
            return RedirectToAction("Index", "Dashboard", new { viewName = "Index", controllerName = "Dashboard" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("AdminLogin", "Account");
        }

 
    }
}
