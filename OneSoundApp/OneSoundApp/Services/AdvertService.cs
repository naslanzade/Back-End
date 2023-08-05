using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdvertService(AppDbContext context,
                             IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(List<IFormFile> images)
        {

            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

                Advert advert = new()
                {
                    Image = fileName,

                };

                await _context.Adverts.AddAsync(advert);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Advert advert = await GetByIdAsync(id);

            _context.Adverts.Remove(advert);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "assets/images", advert.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(Advert image, IFormFile newImage)
        {
            string oldPath = Path.Combine(_env.WebRootPath, "assets/images", image.Image);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

            image.Image = fileName;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Advert>> GetAll()
        {
            return await _context.Adverts.ToListAsync();
        }

        public async Task<Advert> GetByIdAsync(int? id)
        {
            return await _context.Adverts.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Adverts.CountAsync();
        }

        public async Task<List<Advert>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Adverts.Skip((page * take) - take).
                                          Take(take).
                                          ToListAsync();
        }
    }
}
