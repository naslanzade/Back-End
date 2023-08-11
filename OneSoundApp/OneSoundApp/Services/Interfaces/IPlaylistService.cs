using OneSoundApp.Areas.Admin.ViewModels.Playlist;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<Playlist>> GetAllAsync();
        Task<int> GetCountAsync();
        Task<List<Playlist>> GetPaginatedDatas(int page, int take);
        Task<Playlist> GetPodcastDetailAsync(int? id);       
        Task CreateAsync(PlaylistCreateVM model);
        Task EditAsync(int playlistId, PlaylistEditVM model);
        Task DeleteAsync(int id);
        Task<Playlist> GetByIdAsnyc(int? id);
        Task<Playlist> GetWithIncludesAsync(int? id);
        Task<PlaylistImage> GetImageById(int? id);
        Task<Playlist> GetPlaylistByImageId(int? id);
    }
}
