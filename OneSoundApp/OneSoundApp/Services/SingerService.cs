using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Singer;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class SingerService : ISingerService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SingerService(AppDbContext context, 
                             IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(List<IFormFile> images, SingerCreateVM newInfo)
        {
            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");


                Singer singer = new()
                {
                    Image = fileName,
                    Name = newInfo.Name,

                };

                await _context.Singers.AddAsync(singer);

            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Singer singer = await GetByIdAsync(id);

            _context.Singers.Remove(singer);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "assets/images", singer.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(SingerEditVM request, IFormFile newImage)
        {
            string oldPath = Path.Combine(_env.WebRootPath, "assets/images", newImage.FileName);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

            request.Image = fileName;

            Singer singer = new()
            {
                Id = request.Id,
                Name = request.Name,
                Image = request.Image
            };

            _context.Update(singer);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Singer>> GetAll()
        {
            return await _context.Singers.ToListAsync();
        }

        public async  Task<Singer> GetByIdAsync(int? id)
        {
            return await _context.Singers.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Singers.CountAsync();
        }

        public async Task<List<Singer>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Singers.Skip((page * take) - take).
                                          Take(take).
                                          ToListAsync();
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
