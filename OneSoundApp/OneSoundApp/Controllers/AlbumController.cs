using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Albums;
using OneSoundApp.ViewModels.Blog;

namespace OneSoundApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly IAlbumService _albumService;
       


        public AlbumController(IAdvertService advertService,
                              IAlbumService albumService)
        {

            _advertService = advertService;
            _albumService = albumService;
            
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Advert> adverts = await _advertService.GetAll();
            List<Album> paginateAlbums = await _albumService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Album> paginatedDatas = new(paginateAlbums, page, pageCount);


            AlbumVM model = new()
            {
                Adverts= adverts,
                PaginatedDatas=paginatedDatas
                
                
            };
            return View(model);
           
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _albumService.GetCountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }

     
    }
}
