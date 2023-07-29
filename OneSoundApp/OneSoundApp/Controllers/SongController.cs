using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Songs;

namespace OneSoundApp.Controllers
{
    public class SongController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly ISongService _songService;



        public SongController(IAdvertService advertService,
                              ISongService songService)
        {

            _advertService = advertService;
            _songService = songService;

        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Advert> adverts = await _advertService.GetAll();
            List<Song> paginateSongs = await _songService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Song> paginatedDatas = new(paginateSongs, page, pageCount);



            SongVM model = new()
            {

                Adverts = adverts,
                PaginatedDatas = paginatedDatas
            };


            return View(model);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var songCount = await _songService.GetCountAsync();

            return (int)Math.Ceiling((decimal)songCount / take);
        }
    }
}
