using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly AppDbContext _context;

        public AlbumService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Albums.CountAsync();
        }

        public async Task<IEnumerable<Album>> GetLatestAlbumAsync()
        {
           return await _context.Albums.Include(m=>m.Images).
                                        Include(m=>m.Singer).
                                        Take(6).
                                        OrderByDescending(m=>m.CreatedDate).
                                        ToListAsync();
        }

        public async Task<List<Album>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Albums.Include(m => m.Images).
                                        Include(m => m.Singer).
                                        Include(m=>m.Song).
                                        Skip((page * take) - take).
                                        Take(take).ToListAsync();
        }

        public async Task<Album> GetAlbumDetailAsync(int? id)
        {
            return await _context.Albums.Include(m=>m.Images).
                                         Include(m=>m.Song).
                                         Include(m=>m.Singer).
                                         Include(m=>m.Category).
                                         FirstOrDefaultAsync(m=>m.Id==id);
        }

        public async Task<IEnumerable<Album>> GetTopAlbumAsync()
        {
            return await _context.Albums.Include(m => m.Images).
                                       Include(m => m.Singer).
                                       Take(4).
                                       OrderByDescending(m => m.CreatedDate).
                                       ToListAsync();
        }

        public async Task<Album> GetByIdAsnyc(int? id)
        {
            return await _context.Albums.FindAsync(id);
        }
        public async Task<Album> GetByIdWithImageAsnyc(int? id)
        {
            return await _context.Albums.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Album>> GetAll()
        {
            return await _context.Albums.ToListAsync();
        }
    }
}
