using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.PlaylistSong;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class PlaylistSongService : IPlaylistSongService
    {

        private readonly AppDbContext _context;

        public PlaylistSongService(AppDbContext context)
        {
            _context = context;
        }



        public async Task CreateAsync(PlaylistSongCreateVM song)
        {
            PlaylistSong newSong = new()
            {
                Name = song.Name,
                PlaylistId = song.PlaylistId,

            };
            await _context.PlaylistSongs.AddAsync(newSong);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            PlaylistSong song = await GetByIdAsync(id);

            _context.Remove(song);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(PlaylistSongEditVM song)
        {
            PlaylistSong newSong = new()
            {
                Id = song.Id,
                Name = song.Name,
                PlaylistId = song.PlaylistId,
            };

            _context.Update(newSong);

            await _context.SaveChangesAsync();
        }

        public async Task<List<PlaylistSong>> GetAllAsync()
        {
            return await _context.PlaylistSongs.Include(m => m.Playlist).
                                                ToListAsync();
        }

        public async Task<PlaylistSong> GetByIdAsync(int? id)
        {
            return await _context.PlaylistSongs.Include(m => m.Playlist).
                                                FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.PlaylistSongs.CountAsync();
        }

        public async Task<List<PlaylistSong>> GetPaginatedDatas(int page, int take)
        {
            return await _context.PlaylistSongs.Include(m => m.Playlist).
                                                Skip((page * take) - take).
                                                Take(take).
                                                ToListAsync();
        }
    }
}
