using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class SingerService : ISingerService
    {
        private readonly AppDbContext _context;

        public SingerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Singer>> GetTopSingerAsync()
        {
            return await _context.Singers.Include(m=>m.Album).
                                          Include(m=>m.Song).
                                          Take(5).
                                          OrderByDescending(m=>m.Id).
                                          ToListAsync();
        }
    }
}
