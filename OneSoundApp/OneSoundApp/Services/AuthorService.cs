using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Author;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class AuthorService : IAuthorService
    {

        private readonly AppDbContext _context;


        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(AuthorCreateVM author)
        {
            Author newAuthor = new()
            {
                Name = author.Name,

            };
            await _context.Authors.AddAsync(newAuthor);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Author author = await GetByIdAsync(id);

            _context.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(AuthorEditVM author)
        {
            Author newAuthor = new()
            {
                Id = author.Id,
                Name = author.Name,
            };

            _context.Update(newAuthor);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int? id)
        {
            return await _context.Authors.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Authors.CountAsync();
        }

        public async Task<List<Author>> GetPaginatedDatas(int page, int take)
        {

            return await _context.Authors.Skip((page * take) - take).
                                           Take(take).
                                           ToListAsync();
        }
    }
}
