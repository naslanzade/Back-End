using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Event;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class EventService : IEventService
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EventService(AppDbContext context, 
                            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(List<IFormFile> images, EventCreateVM newInfo)
        {
            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");


                Event model = new()
                {

                    EventName = newInfo.EventName,
                    Location = newInfo.Location,
                    Image=fileName

                };

                await _context.Events.AddAsync(model);

            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Event model = await GetByIdAsync(id);

            _context.Events.Remove(model);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "assets/images", model.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(EventEditVM request, IFormFile newImage)
        {
            string oldPath = Path.Combine(_env.WebRootPath, "assets/images", newImage.FileName);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

            request.Image = fileName;

            Event model = new()
            {
                Id = request.Id,
                EventName = request.EventName,
                Image = request.Image,
                Location = request.Location,
            };

            _context.Update(model);

            await _context.SaveChangesAsync();
        }

        public async Task<Event> GetByIdAsync(int? id)
        {
            return await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Events.CountAsync();
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            return await _context.Events.Take(6).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetLatestEventsAsync()
        {
            return await _context.Events.Take(4).OrderByDescending(m=>m.CreatedDate).ToListAsync();
        }

        public async Task<List<Event>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Events.Skip((page * take) - take).
                                          Take(take).
                                          ToListAsync();
        }
    }
}
