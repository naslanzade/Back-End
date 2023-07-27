using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class SongService : ISongService
    {
        private readonly AppDbContext _context;

        public SongService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Song>> GetLatestSongAsync()
        {
            return await _context.Songs.Include(m => m.Singer).
                                        Include(m=>m.Album).
                                        Take(4).
                                        OrderByDescending(m => m.CreatedDate).
                                        ToListAsync();
        }
    }
}
