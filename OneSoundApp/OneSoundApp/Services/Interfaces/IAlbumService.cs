using OneSoundApp.Areas.Admin.ViewModels.Album;
using OneSoundApp.Areas.Admin.ViewModels.Playlist;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetLatestAlbumAsync();
        Task<IEnumerable<Album>> GetTopAlbumAsync();
        Task<int> GetCountAsync();
        Task<List<Album>> GetPaginatedDatas(int page, int take);
        Task<Album> GetAlbumDetailAsync(int? id);
        Task<Album> GetByIdAsnyc(int? id);
        Task<Album> GetByIdWithImageAsnyc(int? id);
        Task<List<Album>> GetAll();


        Task CreateAsync(AlbumCreateVM model);
        Task EditAsync(int albumtId, AlbumEditVM model);
        Task DeleteAsync(int id);      
        Task<Album> GetWithIncludesAsync(int? id);
        Task<AlbumImage> GetImageById(int? id);
        Task<Album> GetAlbumByImageId(int? id);
    }
}
