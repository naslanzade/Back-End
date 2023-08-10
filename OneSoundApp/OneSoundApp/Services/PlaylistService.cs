using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Playlist;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class PlaylistService : IPlaylistService
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlaylistService(AppDbContext context,
                               IWebHostEnvironment env)
        {
            
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(PlaylistCreateVM model)
        {
            List<PlaylistImage> images = new();

            foreach (var item in model.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");
                images.Add(new PlaylistImage { Image = fileName });
            }

         
            images.FirstOrDefault().IsMain = true;

            Playlist playlist = new()
            {
                Name = model.Name,
                Description = model.Description,  
                CategoryId=model.CategoryId,

                Images = images
            };

            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var playlist = await _context.Playlists.Include(m => m.Images).Include(m=>m.Songs).FirstOrDefaultAsync(m => m.Id == id);

            _context.Playlists.Remove(playlist);

            await _context.SaveChangesAsync();

            foreach (var item in playlist.Images)
            {
                string path = Path.Combine(_env.WebRootPath, "assets/images", item.Image);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public async Task EditAsync(int playlistId, PlaylistEditVM model)
        {
            List<PlaylistImage> images = new();

            Playlist playlist = await GetByIdAsnyc(playlistId);

            if (model.newImages != null)
            {
                foreach (var item in model.newImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");
                    images.Add(new PlaylistImage { Image = fileName, PlaylistId = playlistId });

                }

                await _context.PlaylistImages.AddRangeAsync(images);

            }

            playlist.Name = model.Name;
            playlist.Description = model.Description;
            playlist.CategoryId= model.CategoryId;
           


            await _context.SaveChangesAsync();
        }

        public async Task<List<Playlist>> GetAllAsync()
        {
            return await _context.Playlists.ToListAsync();
        }

        public async Task<Playlist> GetByIdAsnyc(int? id)
        {
            return await _context.Playlists.FindAsync(id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Playlists.CountAsync();
        }

        public async Task<PlaylistImage> GetImageById(int? id)
        {
            return await _context.PlaylistImages.FindAsync((int)id);
        }

        public async Task<List<Playlist>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Playlists.Include(m=>m.Images).
                                            Include(m=>m.Songs).
                                            Skip((page * take) - take).
                                            Take(take).ToListAsync();
        }

        public async Task<Playlist> GetPodcastDetailAsync(int? id)
        {
            return await _context.Playlists.Include(m => m.Images).
                                           Include(m => m.Songs).
                                           Include(m=>m.Category).
                                           FirstOrDefaultAsync(m=>m.Id==id);
        }

        public async Task<Playlist> GetProductByImageId(int? id)
        {
            return await _context.Playlists.Include(p => p.Images)
                                         .FirstOrDefaultAsync(p => p.Images
                                         .Any(p => p.Id == id));
        }

        public async Task<Playlist> GetWithIncludesAsync(int? id)
        {
            return await _context.Playlists.Where(m => m.Id == id).Include(m => m.Images)
                                                                .Include(m => m.Songs)
                                                                .Include(m => m.Category)
                                                                .FirstOrDefaultAsync();
        }
    }
}
