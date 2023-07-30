using Newtonsoft.Json;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Responses;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Cart;

namespace OneSoundApp.Services
{
    public class CartService : ICartService
    {

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IAlbumService _albumService;

        public CartService(AppDbContext context,
                             IHttpContextAccessor accessor,
                             IAlbumService albumService)
        {
            _context = context;
            _accessor = accessor;
            _albumService = albumService;
        }
        public void AddProduct(List<CartVM> basket, Album product)
        {
            CartVM existProduct = basket.FirstOrDefault(m => m.Id == product.Id);

            if (existProduct is null)
            {
                basket.Add(new CartVM
                {
                    Id = product.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
        }

        public async Task<BasketDeleteResponse> DeleteProduct(int? id)
        {
            List<CartVM> basketDatas = JsonConvert.DeserializeObject<List<CartVM>>(_accessor.HttpContext.Request.Cookies["basket"]);

            var data = basketDatas.FirstOrDefault(m => m.Id == id);

            basketDatas.Remove(data);

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

            decimal total = 0;
            foreach (var basketData in basketDatas)
            {

                Album dbAlbum = await _albumService.GetByIdAsnyc(basketData.Id);
                total += (dbAlbum.Price * basketData.Count);

            }
            int count = basketDatas.Sum(m => m.Count);


            return new BasketDeleteResponse { Count = count, Total = total };
        }

        public List<CartVM> GetAll()
        {
            List<CartVM> basket;

            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<CartVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<CartVM>();
            }

            return basket;
        }

        public int GetCount()
        {
            List<CartVM> basket;

            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<CartVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<CartVM>();
            }

            return basket.Sum(m => m.Count);
        }
    }
}
