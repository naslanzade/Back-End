
using OneSoundApp.Areas.Admin.ViewModels.Records;
using OneSoundApp.Models;

namespace OneSoundApp.Services.Interfaces
{
    public interface IRecordService
    {
        Task<List<Record>> GetAllAsync();
        Task<Record> GetByIdAsync(int? id);
        Task<int> GetCountAsync();
        Task<List<Record>> GetPaginatedDatas(int page, int take);
        Task CreateAsync(RecordCreateVM record);
        Task EditAsync(RecordEditVM record);
        Task DeleteAsync(int id);
    }
}
