using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAll();
    }
}
