using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Albums;


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



        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Album album= await _albumService.GetAlbumDetailAsync(id);

            if (album == null) return NotFound();

            AlbumDetailVM model = new()
            {

                Id=album.Id,
                AlbumName=album.AlbumName,
                Price=album.Price,
                Songs=album.Song.ToList(),
                Images=album.Images.ToList(),
                SingerName=album.Singer.Name,
                

            };

            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _albumService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }

     
    }
}
