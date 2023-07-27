using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISingerService
    {
        Task<IEnumerable<Singer>> GetTopSingerAsync();
    }
}
