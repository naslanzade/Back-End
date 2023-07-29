
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class PodcastService : IPodcastService
    {

        private readonly AppDbContext _context;

        public PodcastService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Podcast.CountAsync();
        }

        public async Task<List<Podcast>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Podcast.Include(m => m.Images).
                                       Include(m => m.Author).
                                       Include(m => m.Category).
                                       Include(m => m.Records).
                                       Skip((page * take) - take).
                                       Take(take).ToListAsync();
        }

        public async Task<Podcast> GetPodcastDetailAsync(int? id)
        {
            return await _context.Podcast.Include(m => m.Images).
                                        Include(m => m.Author).
                                        Include(m=>m.Records).
                                        Include(m => m.Category).
                                        FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
