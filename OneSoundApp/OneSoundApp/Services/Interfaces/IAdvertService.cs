using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IAdvertService
    {
        Task<List<Advert>> GetAll();
    }
}
