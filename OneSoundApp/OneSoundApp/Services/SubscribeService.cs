using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class SubscribeService : ISubscribeService
    {

        private readonly AppDbContext _context;

        public SubscribeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            Subscribe subscribe = await GetSubscribeById(id);

            _context.Remove(subscribe);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Subscribe>> GetAll()
        {
            return await _context.Subscribes.ToListAsync();
        }

        public  async Task<int> GetCountAsync()
        {
            return await _context.Subscribes.CountAsync();
        }

        public async Task<List<Subscribe>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Subscribes.Skip((page * take) - take).
                                           Take(take).
                                           ToListAsync();
        }

        public async Task<Subscribe> GetSubscribeById(int? id)
        {
            return await _context.Subscribes.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
