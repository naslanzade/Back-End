using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Podcast;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Services
{
    public class PodcastService : IPodcastService
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PodcastService(AppDbContext context, 
                              IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(PodcastCreateVM model)
        {
            List<PodcastImage> images = new();

            foreach (var item in model.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");
                images.Add(new PodcastImage { Image = fileName });
            }

            images.FirstOrDefault().IsMain = true;

            Podcast podcast = new()
            {
                Name = model.Name,
                Description = model.Description,               
                AuthorId = model.AuthorId,
                CategoryId = model.CategoryId,

                Images = images
            };

            await _context.Podcast.AddAsync(podcast);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var podcast = await _context.Podcast.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);

            _context.Podcast.Remove(podcast);

            await _context.SaveChangesAsync();

            foreach (var item in podcast.Images)
            {
                string path = Path.Combine(_env.WebRootPath, "assets/images", item.Image);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public async Task EditAsync(int podcastId, PodcastEditVM model)
        {
            List<PodcastImage> images = new();

            Podcast podcast = await GetByIdAsnyc(podcastId);

            if (model.newImages != null)
            {
                foreach (var item in model.newImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    await item.SaveFileAsync(fileName, _env.WebRootPath, "assets/images");
                    images.Add(new PodcastImage { Image = fileName, PodcastId = podcastId });

                }

                await _context.PodcastImage.AddRangeAsync(images);

            }

            podcast.Name=model.Name;
            podcast.Description=model.Description;
            podcast.AuthorId= model.AuthorId;
            podcast.CategoryId= model.CategoryId;


            await _context.SaveChangesAsync();
        }

        public async Task<List<Podcast>> GetAll()
        {
            return await _context.Podcast.ToListAsync();
        }

        public async Task<Podcast> GetByIdAsnyc(int? id)
        {
            return await _context.Podcast.FindAsync(id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Podcast.CountAsync();
        }            

        public async Task<List<Podcast>> GetPaginatedDatas(int page, int take)
        {
            return await _context.Podcast.Include(m => m.Images).
                                       Include(m => m.Author).
                                       Include(m => m.Category).
                                       Include(m => m.Records).
                                       Skip((page * take) - take).
                                       Take(take).ToListAsync();
        }

        public async Task<Podcast> GetPodcastDetailAsync(int? id)
        {
            return await _context.Podcast.Include(m => m.Images).
                                        Include(m => m.Author).
                                        Include(m=>m.Records).
                                        Include(m => m.Category).
                                        FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Podcast> GetPodcastByImageId(int? id)
        {
            return await _context.Podcast.Include(p => p.Images)
                                          .FirstOrDefaultAsync(p => p.Images
                                          .Any(p => p.Id == id));
        }

        public async Task<PodcastImage> GetImageById(int? id)
        {
            return await _context.PodcastImage.FindAsync((int)id);
        }

        public async Task<Podcast> GetWithIncludesAsync(int? id)
        {
            return await _context.Podcast.Where(m => m.Id == id).Include(m => m.Images)
                                                                .Include(m => m.Author)
                                                                .Include(m=>m.Category)
                                                                .Include(m=>m.Records)
                                                                .FirstOrDefaultAsync();
        }
    }
}
