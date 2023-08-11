using OneSoundApp.Areas.Admin.ViewModels.Author;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser> GetByIdAsync(string id);
        Task<int> GetCountAsync();
        Task<List<AppUser>> GetPaginatedDatas(int page, int take);     
        Task DeleteAsync(string id);
    }
}
