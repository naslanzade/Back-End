using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<int> GetCountAsync()
        {
            return await _context.Songs.CountAsync();
        }

        public async Task<IEnumerable<Song>> GetLatestSongAsync()
        {
            return await _context.Songs.Include(m => m.Singer).
                                        Include(m=>m.Album).
                                        Take(4).
                                        OrderByDescending(m => m.CreatedDate).
                                        ToListAsync();
        }

        public async Task<List<Song>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Songs.Include(m => m.Singer).
                                        Include(m => m.Album).
                                        Skip((page * take) - take).
                                        Take(take).ToListAsync();
        }

        public async Task<Song> GetPodcastDetailAsync(int? id)
        {
            return await _context.Songs.Include(m => m.Singer).
                                       Include(m => m.Album).
                                       FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
