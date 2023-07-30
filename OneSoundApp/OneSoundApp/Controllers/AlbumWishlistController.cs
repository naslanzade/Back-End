using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Wishlist;

namespace OneSoundApp.Controllers
{
    public class AlbumWishlistController : Controller
    {

        private readonly IWishlistService _wishlistService;
        private readonly IAlbumService _albumService;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILayoutService _layoutService;

        public AlbumWishlistController(IWishlistService wishlistService,
                                       IAlbumService albumService,
                                       IHttpContextAccessor accessor,
                                       ILayoutService layoutService)
        {
            _accessor = accessor;
            _albumService = albumService;
            _wishlistService = wishlistService;
            _layoutService = layoutService;
        }

        public async Task<IActionResult> Index()
        {
            List<WishlistAlbumDetailVM> wishList = new();

            if (_accessor.HttpContext.Request.Cookies["wishlistAlbum"] != null)
            {
                List<WishlistVM> wishlistDatas = JsonConvert.DeserializeObject<List<WishlistVM>>(_accessor.HttpContext.Request.Cookies["wishlistAlbum"]);
                foreach (var item in wishlistDatas)
                {
                    var dbAlbum = await _albumService.GetByIdWithImageAsnyc(item.Id);


                    if (dbAlbum != null)
                    {
                        WishlistAlbumDetailVM wishlistDetail = new()
                        {
                            Id = dbAlbum.Id,
                            Name = dbAlbum.AlbumName,
                            Image = dbAlbum.Images.Where(m => m.IsMain).FirstOrDefault().Image,
                            Price = dbAlbum.Price,                          

                        };

                        wishList.Add(wishlistDetail);

                    }

                }

            }
            return View(wishList);
        }


        [HttpPost]
        public async Task<IActionResult> AddWishlist(int? id)
        {
            if (id is null) return BadRequest();

            Album album = await _albumService.GetByIdAsnyc(id);

            if (album is null) NotFound();

            List<WishlistVM> wishlist = _wishlistService.GetAllAlbums();

            _wishlistService.AddAlbum(wishlist, album);

            return Ok(wishlist.Sum(m => m.Count));
        }


        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            return Ok(await _wishlistService.DeleteAlbum(id));
        }
    }
}
