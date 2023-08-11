using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Areas.Admin.ViewModels.User;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;
        public UserController(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<AppUser> datas = await _userService.GetPaginatedDatas(page, take);
            List<UserVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<UserVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        private List<UserVM> GetMappedDatas(List<AppUser> users)
        {
            List<UserVM> mappedDatas = new();
            foreach (var user in users)
            {
                UserVM userList = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email=user.Email,
                };
                mappedDatas.Add(userList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var userCount = await _userService.GetCountAsync();

            return (int)Math.Ceiling((decimal)userCount / take);
        }
    }
}
