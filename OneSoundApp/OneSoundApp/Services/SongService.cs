using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Song;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class SongService : ISongService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SongService(AppDbContext context, 
                           IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(SongCreateVM model, List<IFormFile> images)
        {
            foreach (var item in images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");


                Song song = new()
                {
                    Image = fileName,
                    SongName = model.SongName,
                    AlbumId = model.AlbumId,
                    SingerId = model.SingerId,
                    
                };

                await _context.Songs.AddAsync(song);

            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Song song = await GetByIdAsnyc((int)id);

            _context.Songs.Remove(song);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "assets/images", song.Image);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task EditAsync(int songId, SongEditVM model, IFormFile newImage)
        {
            var song = await GetByIdAsnyc(songId);

            string oldPath = Path.Combine(_env.WebRootPath, "assets/images", newImage.FileName);

            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");

            model.Image = fileName;

            song.SongName = model.SongName;
            song.AlbumId = model.AlbumId;
            song.SingerId = model.SingerId;           
            song.Image = fileName;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Song>> GetAll()
        {
            return await _context.Songs.Include(m => m.Album).
                                        Include(m => m.Singer).
                                        ToListAsync();
        }

        public async Task<Song> GetByIdAsnyc(int? id)
        {
            return await _context.Songs.Include(m => m.Album).
                                         Include(m => m.Singer).
                                         FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Songs.CountAsync();
        }

        public async Task<IEnumerable<Song>> GetLatestSongAsync()
        {
            return await _context.Songs.Include(m => m.Singer).
                                        Include(m=>m.Album).
                                        Take(4).
                                        OrderByDescending(m => m.CreatedDate).
                                        ToListAsync();
        }

        public async Task<List<Song>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Songs.Include(m => m.Singer).
                                        Include(m => m.Album).
                                        Skip((page * take) - take).
                                        Take(take).ToListAsync();
        }

        public async Task<Song> GetSongDetailAsync(int? id)
        {
            return await _context.Songs.Include(m => m.Singer).
                                       Include(m => m.Album).
                                       FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
