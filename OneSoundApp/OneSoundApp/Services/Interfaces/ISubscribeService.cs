using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISubscribeService
    {
        Task<List<Subscribe>> GetAll();
        Task<Subscribe> GetSubscribeById(int? id);
    }
}
