using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class BlogService : IBlogService
    {

        private readonly AppDbContext _context;

        public BlogService(AppDbContext context)
        {
            _context= context;
        }
        public async Task<List<Blog>> GetAll()
        {
            return await _context.Blogs.Include(m=>m.Author).
                                        Include(m=>m.Category).
                                        ToListAsync();
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
