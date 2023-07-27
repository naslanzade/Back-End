using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly AppDbContext _context;

        public AdvertService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Advert>> GetAll()
        {
            return await _context.Adverts.ToListAsync();
        }
    }
}
