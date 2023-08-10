using OneSoundApp.Areas.Admin.ViewModels.PlaylistSongs;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IPlaylistSongService
    {
        Task<List<PlaylistSong>> GetAllAsync();
        Task<PlaylistSong> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<PlaylistSong>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(PlaylistSongCreateVM song);
        Task EditAsync(PlaylistSongEditVM song);
        Task DeleteAsync(int id);
    }
}
