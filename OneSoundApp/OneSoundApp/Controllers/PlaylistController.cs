using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Albums;
using OneSoundApp.ViewModels.Playlist;

namespace OneSoundApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly IPlaylistService _playlistService;



        public PlaylistController(IAdvertService advertService,
                              IPlaylistService playlistService)
        {

            _advertService = advertService;
            _playlistService = playlistService;

        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Advert> adverts = await _advertService.GetAll();
            List<Playlist> paginatePlaylist = await _playlistService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Playlist> paginatedDatas = new(paginatePlaylist, page, pageCount);


            PlaylistVM model = new()
            {
                Adverts = adverts,
                PaginatedDatas = paginatedDatas


            };
            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _playlistService.GetCountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }
    }
}
