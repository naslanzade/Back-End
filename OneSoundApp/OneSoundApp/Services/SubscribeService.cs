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
        public async Task<List<Subscribe>> GetAll()
        {
            return await _context.Subscribes.ToListAsync();
        }

        public async Task<Subscribe> GetSubscribeById(int? id)
        {
            return await _context.Subscribes.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
