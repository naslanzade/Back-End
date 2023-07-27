using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync();
        Task<IEnumerable<Event>> GetLatestEventsAsync();


    }
}
