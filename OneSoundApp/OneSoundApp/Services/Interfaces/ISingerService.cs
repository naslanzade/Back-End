using OneSoundApp.Areas.Admin.ViewModels.Category;
using OneSoundApp.Areas.Admin.ViewModels.Singer;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISingerService
    {
        Task<IEnumerable<Singer>> GetTopSingerAsync();
        Task<List<Singer>> GetAll();
        Task<Singer> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<Singer>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(List<IFormFile> images, SingerCreateVM newInfo);
        Task EditAsync(SingerEditVM request, IFormFile newImage);
        Task DeleteAsync(int id);
    }
}
