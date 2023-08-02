using OneSoundApp.Models;
using OneSoundApp.Responses;
using OneSoundApp.ViewModels.Wishlist;

namespace OneSoundApp.Services.Interfaces
{
    public interface IWishlistService
    {
        List<WishlistVM> GetAllAlbums();
        void AddAlbum(List<WishlistVM> wishlist, Album product);
        Task<WishlistDeleteResponse> DeleteAlbum(int? id);
        int GetAlbumsCount();
        Task<Wishlist> GetByUserIdAsync(string userId);
        Task<List<WishlistAlbum>> GetAllByWishlistIdAsync(int? wishlistId);
    }
}
