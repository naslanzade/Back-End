using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class EventService : IEventService
    {

        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            return await _context.Events.Take(6).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetLatestEventsAsync()
        {
            return await _context.Events.Take(4).OrderByDescending(m=>m.CreatedDate).ToListAsync();
        }
    }
}
