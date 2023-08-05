using OneSoundApp.Areas.Admin.ViewModels.Category;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAll();
        Task<Category> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<Category>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(List<IFormFile> images, CategoryCreateVM newInfo);
        Task EditAsync(CategoryEditVM request, IFormFile newImage);
        Task DeleteAsync(int id);
    }
}
