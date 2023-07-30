using OneSoundApp.Models;
using OneSoundApp.Responses;
using OneSoundApp.ViewModels.Cart;

namespace OneSoundApp.Services.Interfaces
{
    public interface ICartService
    {
        List<CartVM> GetAll();
        void AddProduct(List<CartVM> basket, Album product);
        Task<BasketDeleteResponse> DeleteProduct(int? id);
        int GetCount();
    }
}
