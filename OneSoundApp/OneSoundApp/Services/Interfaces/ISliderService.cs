using OneSoundApp.Areas.Admin.ViewModels.Slider;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISliderService
    {
        Task<List<Slider>> GetAll();
        Task<Slider> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<Slider>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(List<IFormFile> images, SliderCreateVM newInfo);
        Task EditAsync(SliderEditVM request, IFormFile newImage);
        Task DeleteAsync(int id);
    }
}
