using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetLatestAlbumAsync();
        Task<IEnumerable<Album>> GetTopAlbumAsync();
        Task<int> GetCountAsync();
        Task<List<Album>> GetPaginatedDatas(int page, int take);
    }
}
