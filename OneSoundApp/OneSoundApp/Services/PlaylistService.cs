using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class PlaylistService : IPlaylistService
    {

        private readonly AppDbContext _context;

        public PlaylistService(AppDbContext context)
        {
            
            _context = context;
        }
        public async Task<List<Playlist>> GetAllAsync()
        {
            return await _context.Playlists.ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Playlists.CountAsync();
        }

        public async Task<List<Playlist>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Playlists.Include(m=>m.Images).
                                            Include(m=>m.Songs).
                                            Skip((page * take) - take).
                                            Take(take).ToListAsync();
        }
    }
}
