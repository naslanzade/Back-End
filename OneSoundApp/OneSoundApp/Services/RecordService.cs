using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Records;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class RecordService : IRecordService
    {

        private readonly AppDbContext _context;

        public RecordService(AppDbContext context)
        {
            _context = context;
        }



        public async Task CreateAsync(RecordCreateVM record)
        {
            Record newRecord = new()
            {
                Name = record.Name,
                PodcastId = record.PodcastId,

            };
            await _context.Record.AddAsync(newRecord);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Record record = await GetByIdAsync(id);

            _context.Remove(record);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(RecordEditVM record)
        {
            Record newRecord = new()
            {
                Id = record.Id,
                Name = record.Name,
                PodcastId= record.PodcastId,
            };

            _context.Update(newRecord);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Record>> GetAllAsync()
        {
            return await _context.Record.Include(m => m.Podcast).
                                         ToListAsync();
        }

        public async Task<Record> GetByIdAsync(int? id)
        {
            return await _context.Record.Include(m => m.Podcast).
                                         FirstOrDefaultAsync(m=>m.Id==id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Record.CountAsync();
        }

        public async Task<List<Record>> GetPaginatedDatas(int page, int take)
        {

            return await _context.Record.Include(m=>m.Podcast).
                                           Skip((page * take) - take).
                                           Take(take).
                                           ToListAsync();
        }
    }
}
