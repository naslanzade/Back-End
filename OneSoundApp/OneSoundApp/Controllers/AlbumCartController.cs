using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Cart;

namespace OneSoundApp.Controllers
{
    public class AlbumCartController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IHttpContextAccessor _accessor;
        private readonly ICartService _cartService;
        private readonly ILayoutService _layoutService;
      

        public AlbumCartController(IHttpContextAccessor accessor,
                              ICartService cartService,
                              IAlbumService albumService,
                              ILayoutService layoutService)
        {


            _accessor = accessor;
            _cartService = cartService;
            _albumService = albumService;
            _layoutService = layoutService;
        }
        public async Task<IActionResult> Index()
        {
            List<CartAlbumDetailVM> basketList = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                List<CartVM> basketDatas = JsonConvert.DeserializeObject<List<CartVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
                foreach (var item in basketDatas)
                {
                    var dbAlbum = await _albumService.GetByIdWithImageAsnyc(item.AlbumId);


                    if (dbAlbum != null)
                    {
                        CartAlbumDetailVM basketDetail = new()
                        {
                            Id = dbAlbum.Id,
                            Name = dbAlbum.AlbumName,
                            Image = dbAlbum.Images.Where(m => m.IsMain).FirstOrDefault().Image,
                            Count = item.Count,
                            Price = dbAlbum.Price,
                            TotalPrice = dbAlbum.Price * item.Count,

                        };

                        basketList.Add(basketDetail);

                    }

                }

            }
            return View(basketList);
        }


        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id)
        {

            if (id is null) return BadRequest();

            Album product = await _albumService.GetAlbumDetailAsync(id);

            if (product is null) NotFound();

            List<CartVM> basket = _cartService.GetAll();

            _cartService.AddProduct(basket, product);

            return Ok(basket.Sum(m => m.Count));
        }


        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            return Ok(await _cartService.DeleteProduct(id));
        }


     
    }
}
