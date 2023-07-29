using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IPodcastService
    {
        Task<int> GetCountAsync();
        Task<List<Podcast>> GetPaginatedDatas(int page, int take);
        Task<Podcast> GetPodcastDetailAsync(int? id);
    }
}
