using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Blog;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class BlogService : IBlogService
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogService(AppDbContext context, 
                           IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(BlogCreateVM model, List<IFormFile> images)
        {
            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");


                Blog blog = new()
                {
                    Image = fileName,
                    Title = model.Title,
                    AuthorId = model.AuthorId,
                    CategoryId= model.CategoryId,
                    Description = model.Description,
                };

                await _context.Blogs.AddAsync(blog);

            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Blog blog = await GetByIdAsnyc(id);

            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "assets/images", blog.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(int blogId, BlogEditVM model, IFormFile newImage)
        {
            var blog = await GetByIdAsnyc(blogId);

            string oldPath = Path.Combine(_env.WebRootPath, "assets/images", newImage.FileName);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

            model.Image = fileName;

            blog.Title = model.Title;
            blog.AuthorId = model.AuthorId;
            blog.CategoryId = model.CategoryId;
            blog.Description = model.Description;
            blog.Image = fileName;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Blog>> GetAll()
        {
            return await _context.Blogs.Include(m=>m.Author).
                                        Include(m=>m.Category).
                                        ToListAsync();
        }

        public async Task<Blog> GetByIdAsnyc(int? id)
        {
            return await _context.Blogs.Include(m => m.Author).
                                        Include(m => m.Category).
                                        FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Blogs.CountAsync();
        }

        public async Task<List<Blog>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Blogs.Include(m => m.Author).
                                        Include(m=>m.Category).
                                        Skip((page * take) - take).
                                        Take(take).ToListAsync();
        }
    }
}
