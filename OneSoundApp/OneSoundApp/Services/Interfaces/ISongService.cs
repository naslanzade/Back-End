using OneSoundApp.Areas.Admin.ViewModels.Blog;
using OneSoundApp.Areas.Admin.ViewModels.Song;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetLatestSongAsync();
        Task<int> GetCountAsync();
        Task<List<Song>> GetPaginatedDatas(int page, int take);
        Task<Song> GetSongDetailAsync(int? id);
        Task CreateAsync(SongCreateVM model, List<IFormFile> images);
        Task EditAsync(int songId, SongEditVM model, IFormFile newImage);
        Task DeleteAsync(int id);
        Task<Song> GetByIdAsnyc(int? id);
        Task<List<Song>> GetAll();

    }
}
