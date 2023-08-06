using OneSoundApp.Areas.Admin.ViewModels.Category;
using OneSoundApp.Areas.Admin.ViewModels.Event;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync();
        Task<IEnumerable<Event>> GetLatestEventsAsync();
        Task<Event> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<Event>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(List<IFormFile> images, EventCreateVM newInfo);
        Task EditAsync(EventEditVM request, IFormFile newImage);
        Task DeleteAsync(int id);


    }
}
