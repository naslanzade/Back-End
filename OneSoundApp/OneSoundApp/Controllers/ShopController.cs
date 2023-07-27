using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Albums;
using OneSoundApp.ViewModels.Playlist;
using OneSoundApp.ViewModels.Shop;

namespace OneSoundApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly ICategoryService _categoryService;
        private readonly IAlbumService _albumService;
        private readonly IPlaylistService _playlistService;


        public ShopController(IAdvertService advertService,
                              ICategoryService categoryService,
                              IAlbumService albumService,
                              IPlaylistService playlistService)
        {
            
            _advertService = advertService;
            _categoryService = categoryService;
            _albumService = albumService;
            _playlistService = playlistService;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Advert> adverts = await _advertService.GetAll();
            List<Category> categories = await _categoryService.GetAll();
            //
            List<Album> paginateAlbums = await _albumService.GetPaginatedDatas(page, take);
            List<AlbumVM> mappedDatas = GetMappedDatas(paginateAlbums);
            int pageCount = await GetPageCountAsync(take);
            Paginate<AlbumVM> paginatedDatas = new(mappedDatas, page, pageCount);
            //
            List<Playlist> paginatePlaylist = await _playlistService.GetPaginatedDatas(page, take);
            List<PlaylistVM> mappData = GetMappedData(paginatePlaylist);
            int count = await GetPageCountPlaylistAsync(take);
            Paginate<PlaylistVM> paginateData = new(mappData, page, count);




            ShopVM model = new()
            {
               Categories= categories,
               Adverts= adverts,
               PaginatedDatas=paginatedDatas,
               PaginatedData=paginateData            
            };



            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _albumService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }

        private async Task<int> GetPageCountPlaylistAsync(int take)
        {
            var playlistCount = await _playlistService.GetCountAsync();

            return (int)Math.Ceiling((decimal)playlistCount / take);
        }

        private List<AlbumVM> GetMappedDatas(List<Album> albums)
        {
            List<AlbumVM> mappedDatas = new();
            foreach (var item in albums)
            {
                AlbumVM List = new()
                {
                    Id = item.Id,
                    AlbumName = item.AlbumName,
                    SingerName=item.Singer.Name,
                    Songs=item.Song.ToList(),
                    Images=item.Images.ToList(),
                    CategoryId=item.Category.Id,
                    Description=item.Description,
                    Price=item.Price,
                    SongCount = item.SongCount,

                };
                mappedDatas.Add(List);
            }
            return mappedDatas;
        }

        private List<PlaylistVM> GetMappedData(List<Playlist> playlists)
        {
            List<PlaylistVM> mappedDatas = new();
            foreach (var item in playlists)
            {
                PlaylistVM List = new()
                {
                    Id = item.Id,
                    PlaylistName = item.Name,
                    Name = item.Name,                    
                    Images = item.Images.ToList(),
                    Songs=item.Songs.ToList(),
                    CategoryId = item.Category.Id,
                    Description= item.Description,
                    Price = item.Price,
                    SongCount=item.SongCount,

                };
                mappedDatas.Add(List);
            }
            return mappedDatas;
        }


    }
}
