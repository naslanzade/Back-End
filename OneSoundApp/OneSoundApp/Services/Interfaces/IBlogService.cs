using OneSoundApp.Areas.Admin.ViewModels.Blog;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAll();
        Task<int> GetCountAsync();
        Task<List<Blog>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(BlogCreateVM model, List<IFormFile> images);
        Task EditAsync(int blogId, BlogEditVM model, IFormFile newImage);
        Task DeleteAsync(int id);
        Task<Blog> GetByIdAsnyc(int? id);
    }
}
