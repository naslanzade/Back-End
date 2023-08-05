using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Category;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryService(AppDbContext context,
                               IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(List<IFormFile> images, CategoryCreateVM newInfo)
        {
            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");


                Category category = new()
                {
                    Image = fileName,
                    Name = newInfo.Name,

                };

                await _context.Categories.AddAsync(category);

            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category category = await GetByIdAsync(id);

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "assets/images", category.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(CategoryEditVM request, IFormFile newImage)
        {
            string oldPath = Path.Combine(_env.WebRootPath, "assets/images", newImage.FileName);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

            request.Image = fileName;

            Category category = new()
            {
                Id = request.Id,
                Name = request.Name,
                Image = request.Image
            };

            _context.Update(category);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int? id)
        {
            return await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<List<Category>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Categories.Skip((page * take) - take).
                                           Take(take).
                                           ToListAsync();
        }
    }
}
