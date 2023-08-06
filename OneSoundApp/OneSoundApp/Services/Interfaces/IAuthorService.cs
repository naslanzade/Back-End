using OneSoundApp.Areas.Admin.ViewModels.Author;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<Author>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(AuthorCreateVM author);
        Task EditAsync(AuthorEditVM author);
        Task DeleteAsync(int id);
    }
}
