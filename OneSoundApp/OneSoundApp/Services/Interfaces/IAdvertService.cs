using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IAdvertService
    {
        Task<List<Advert>> GetAll();

        Task<Advert> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<Advert>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(List<IFormFile> images);
        Task DeleteAsync(int id);
        Task EditAsync(Advert image, IFormFile newImage);
    }

}
