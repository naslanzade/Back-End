using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<Playlist>> GetAllAsync();
        Task<int> GetCountAsync();
        Task<List<Playlist>> GetPaginatedDatas(int page, int take);
        Task<Playlist> GetPodcastDetailAsync(int? id);
    }
}
