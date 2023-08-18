using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Areas.Admin.ViewModels.Subscribe;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubscribeController : Controller
    {

        private readonly ISubscribeService _subscribeService;
        private readonly AppDbContext _context;

        public SubscribeController(ISubscribeService subscribeService,
                                   AppDbContext context)
        {
            _context = context;
            _subscribeService = subscribeService;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Subscribe> datas = await _subscribeService.GetPaginatedDatas(page, take);
            List<SubscribeVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<SubscribeVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _subscribeService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }



        private List<SubscribeVM> GetMappedDatas(List<Subscribe> subscribes)
        {
            List<SubscribeVM> mappedDatas = new();
            foreach (var item in subscribes)
            {
                SubscribeVM subscribeList = new()
                {
                    Id = item.Id,                   
                    Email = item.Email,
                };
                mappedDatas.Add(subscribeList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var userCount = await _subscribeService.GetCountAsync();

            return (int)Math.Ceiling((decimal)userCount / take);
        }
    }
}
