using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISubscribeService
    {
        Task<List<Subscribe>> GetAll();
        Task<Subscribe> GetSubscribeById(int? id);
        Task<int> GetCountAsync();
        Task<List<Subscribe>> GetPaginatedDatas(int page, int take);

        Task DeleteAsync(int id);
    }
}
