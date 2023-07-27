using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetLatestSongAsync();

    }
}
