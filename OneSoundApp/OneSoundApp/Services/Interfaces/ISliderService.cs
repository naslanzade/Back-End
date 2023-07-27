using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISliderService
    {
        Task<List<Slider>> GetAll();
    }
}
