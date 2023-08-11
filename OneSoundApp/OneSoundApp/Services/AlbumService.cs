using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Album;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AlbumService(AppDbContext context, 
                           IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Albums.CountAsync();
        }

        public async Task<IEnumerable<Album>> GetLatestAlbumAsync()
        {
           return await _context.Albums.Include(m=>m.Images).
                                        Include(m=>m.Singer).
                                        Take(6).
                                        OrderByDescending(m=>m.CreatedDate).
                                        ToListAsync();
        }

        public async Task<List<Album>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Albums.Include(m => m.Images).
                                        Include(m => m.Singer).
                                        Include(m=>m.Song).
                                        Skip((page * take) - take).
                                        Take(take).ToListAsync();
        }

        public async Task<Album> GetAlbumDetailAsync(int? id)
        {
            return await _context.Albums.Include(m=>m.Images).
                                         Include(m=>m.Song).
                                         Include(m=>m.Singer).
                                         Include(m=>m.Category).
                                         FirstOrDefaultAsync(m=>m.Id==id);
        }

        public async Task<IEnumerable<Album>> GetTopAlbumAsync()
        {
            return await _context.Albums.Include(m => m.Images).
                                       Include(m => m.Singer).
                                       Take(4).
                                       OrderByDescending(m => m.CreatedDate).
                                       ToListAsync();
        }

        public async Task<Album> GetByIdAsnyc(int? id)
        {
            return await _context.Albums.FindAsync(id);
        }
        public async Task<Album> GetByIdWithImageAsnyc(int? id)
        {
            return await _context.Albums.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Album>> GetAll()
        {
            return await _context.Albums.ToListAsync();
        }

        public async Task CreateAsync(AlbumCreateVM model)
        {
            List<AlbumImage> images = new();

            foreach (var item in model.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");
                images.Add(new AlbumImage { Image = fileName });
            }


            images.FirstOrDefault().IsMain = true;

            Album album = new()
            {
                AlbumName = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                SingerId = model.SingerId,
                Price = model.Price,

                Images = images
            };

            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int albumtId, AlbumEditVM model)
        {
            List<AlbumImage> images = new();

            Album album = await GetByIdAsnyc(albumtId);

            if (model.newImages != null)
            {
                foreach (var item in model.newImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");
                    images.Add(new AlbumImage { Image = fileName, AlbumId = albumtId });

                }

                await _context.AlbumImages.AddRangeAsync(images);

            }

            album.AlbumName = model.Name;
            album.Description = model.Description;
            album.CategoryId = model.CategoryId;
            album.SingerId = model.SingerId;
            album.Price = model.Price;



            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var album = await _context.Albums.Include(m => m.Images).
                                              Include(m=>m.Singer).
                                              Include(m=>m.Song).
                                              Include(m=>m.Category).
                                              FirstOrDefaultAsync(m => m.Id == id);

            _context.Albums.Remove(album);

            await _context.SaveChangesAsync();

            foreach (var item in album.Images)
            {
                string path = Path.Combine(_env.WebRootPath, "assets/images", item.Image);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public async Task<Album> GetWithIncludesAsync(int? id)
        {
            return await _context.Albums.Where(m => m.Id == id).Include(m => m.Images)
                                                                 .Include(m => m.Song)
                                                                 .Include(m=>m.Singer)
                                                                 .Include(m => m.Category)
                                                                 .FirstOrDefaultAsync();
        }

        public async Task<AlbumImage> GetImageById(int? id)
        {
            return await _context.AlbumImages.FindAsync((int)id);
        }

        public async Task<Album> GetAlbumByImageId(int? id)
        {
            return await _context.Albums.Include(p => p.Images)
                                        .FirstOrDefaultAsync(p => p.Images
                                        .Any(p => p.Id == id));
        }
    }
}
