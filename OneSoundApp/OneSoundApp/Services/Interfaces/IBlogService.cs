using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAll();
        Task<int> GetCountAsync();
        Task<List<Blog>> GetPaginatedDatas(int page, int take);
    }
}
