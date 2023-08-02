
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Responses;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Wishlist;

namespace OneSoundApp.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IAlbumService _albumService;



        public WishlistService(AppDbContext context,
                              IHttpContextAccessor accessor,
                              IAlbumService albumService)
        {
            
            _context = context;
            _accessor = accessor;
            _albumService = albumService;
        }



        public void AddAlbum(List<WishlistVM> wishlist, Album product)
        {
            WishlistVM existAlbum = wishlist.FirstOrDefault(m => m.Id == product.Id);

            if (existAlbum is null)
            {
                wishlist.Add(new WishlistVM
                {
                    Id = product.Id,
                    Count = 1
                });
            }
            else
            {
                existAlbum.Count++;
            }

            _accessor.HttpContext.Response.Cookies.Append("wishlistAlbum", JsonConvert.SerializeObject(wishlist));
        }

        public async Task<WishlistDeleteResponse> DeleteAlbum(int? id)
        {
            List<WishlistVM> wishlistDatas = JsonConvert.DeserializeObject<List<WishlistVM>>(_accessor.HttpContext.Request.Cookies["wishlistAlbum"]);

            var data = wishlistDatas.FirstOrDefault(m => m.Id == id);

            wishlistDatas.Remove(data);

            _accessor.HttpContext.Response.Cookies.Append("wishlistAlbum", JsonConvert.SerializeObject(wishlistDatas));

            decimal total = 0;
            foreach (var wishlistdata in wishlistDatas)
            {

                Album dbAlbum = await _albumService.GetByIdAsnyc(wishlistdata.Id);
                total += (dbAlbum.Price * wishlistdata.Count);

            }
            int count = wishlistDatas.Sum(m => m.Count);


            return new WishlistDeleteResponse { Count = count, Total = total };
        }

        public List<WishlistVM> GetAllAlbums()
        {
            List<WishlistVM> wishlist;

            if (_accessor.HttpContext.Request.Cookies["wishlistAlbum"] != null)
            {
                wishlist = JsonConvert.DeserializeObject<List<WishlistVM>>(_accessor.HttpContext.Request.Cookies["wishlistAlbum"]);
            }
            else
            {
                wishlist = new List<WishlistVM>();
            }

            return wishlist;
        }

        public int GetAlbumsCount()
        {
            List<WishlistVM> wishlist;

            if (_accessor.HttpContext.Request.Cookies["wishlistAlbum"] != null)
            {
                wishlist = JsonConvert.DeserializeObject<List<WishlistVM>>(_accessor.HttpContext.Request.Cookies["wishlistAlbum"]);
            }
            else
            {
                wishlist = new List<WishlistVM>();
            }

            return wishlist.Sum(m => m.Count);
        }

        public async Task<Wishlist> GetByUserIdAsync(string userId)
        {
            return await _context.Wishlist.Include(c => c.WishlistAlbums).FirstOrDefaultAsync(c => c.AppUserId == userId);
        }

        public async Task<List<WishlistAlbum>> GetAllByWishlistIdAsync(int? wishlistId)
        {
            return await _context.WishlistAlbum.Where(c => c.WishlistId == wishlistId).ToListAsync();
        }
    }
}
