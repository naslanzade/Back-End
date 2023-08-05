using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Slider;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext context,
                            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(List<IFormFile> images, SliderCreateVM newInfo)
        {
            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");


                Slider slider = new()
                {
                    Image = fileName,
                    Title = newInfo.Title,
                    Header = newInfo.Header,
                };

                await _context.Sliders.AddAsync(slider);

            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Slider slider = await GetByIdAsync(id);

            _context.Sliders.Remove(slider);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "assets/images", slider.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(SliderEditVM request, IFormFile newImage)
        {
            string oldPath = Path.Combine(_env.WebRootPath, "assets/images", newImage.FileName);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

            request.Image = fileName;

            Slider slider = new()
            {
                Id = request.Id,
                Title = request.Title,
                Header = request.Header,
                Image = request.Image
            };

            _context.Update(slider);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Slider>> GetAll()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<Slider> GetByIdAsync(int? id)
        {
            return await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Sliders.CountAsync();
        }

        public async Task<List<Slider>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Sliders.Skip((page * take) - take).
                                          Take(take).
                                          ToListAsync();
        }
    }
}
