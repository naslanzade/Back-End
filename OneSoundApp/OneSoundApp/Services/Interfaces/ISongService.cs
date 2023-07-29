using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetLatestSongAsync();
        Task<int> GetCountAsync();
        Task<List<Song>> GetPaginatedDatas(int page, int take);

    }
}
