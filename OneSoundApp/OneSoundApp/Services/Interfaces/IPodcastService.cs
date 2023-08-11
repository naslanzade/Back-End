using OneSoundApp.Areas.Admin.ViewModels.Podcast;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IPodcastService
    {
        Task<int> GetCountAsync();
        Task<List<Podcast>> GetPaginatedDatas(int page, int take);
        Task<Podcast> GetPodcastDetailAsync(int? id);
        Task<List<Podcast>> GetAll();
        Task CreateAsync(PodcastCreateVM model);
        Task EditAsync(int podcastId, PodcastEditVM model);
        Task DeleteAsync(int id);
        Task<Podcast> GetByIdAsnyc(int? id);
        Task<Podcast> GetWithIncludesAsync(int? id);
        Task<PodcastImage> GetImageById(int? id);
        Task<Podcast> GetPodcastByImageId(int? id);

    }
}
