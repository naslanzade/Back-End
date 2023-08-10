using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneSoundApp.Areas.Admin.ViewModels.Playlist;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public PlaylistController(IPlaylistService playlistService,
                                 ICategoryService categoryService,
                                 AppDbContext context,
                                 IWebHostEnvironment env)
        {
            _categoryService = categoryService;
            _playlistService = playlistService;
            _context = context;
            _env = env;

        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Playlist> datas = await _playlistService.GetPaginatedDatas(page, take);
            List<PlaylistVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<PlaylistVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Playlist dbPlaylist = await _playlistService.GetWithIncludesAsync(id);
                if (dbPlaylist is null) return NotFound();
                ViewBag.page = page;

                PlaylistDetailVM model = new()
                {
                    Id = dbPlaylist.Id,
                    Images = dbPlaylist.Images,
                    Description = dbPlaylist.Description,
                    Name = dbPlaylist.Name,
                    Songs = dbPlaylist.Songs,
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
          
            await GetCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaylistCreateVM request)
        {
            await GetCategories();

            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach (var item in request.Images)
            {

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("image", "Please select only image file");
                    return View();
                }

                if (item.CheckFileSize(200))
                {
                    ModelState.AddModelError("image", "Image size must be max 200KB");
                    return View();
                }
            }

            await _playlistService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await GetCategories();

            if (id is null) return BadRequest();

            Playlist playlist = await _playlistService.GetWithIncludesAsync(id);

            if (playlist is null) return NotFound();

            PlaylistEditVM response = new()
            {
                Name = playlist.Name,
                Description = playlist.Description,
                CategoryId = (int)playlist.CategoryId,
                Images = playlist.Images.ToList(),

            };

            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlaylistEditVM request)
        {
            await GetCategories();

            Playlist playlist = await _playlistService.GetWithIncludesAsync(id);


            if (request.newImages != null)
            {

                foreach (var item in request.newImages)
                {
                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImage", "Please select only image file");
                        request.Images = playlist.Images.ToList();
                        return View(request);
                    }

                    if (item.CheckFileSize(200))
                    {
                        ModelState.AddModelError("NewImage", "Image size must be max 200KB");
                        request.Images = playlist.Images.ToList();
                        return View(request);
                    }
                }
            }

            await _playlistService.EditAsync(id, request);

            return RedirectToAction(nameof(Index));


        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            await _playlistService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int? id)
        {
            try
            {
                if (id is null) return BadRequest();
                PlaylistImage image = await _playlistService.GetImageById((int)id);
                if (image is null) return NotFound();
                Playlist dbPlaylist = await _playlistService.GetProductByImageId((int)id);
                if (dbPlaylist is null) return NotFound();
                RemoveImageResponse response = new();
                response.Result = false;

                if (dbPlaylist.Images.Count > 1)
                {
                    string path = FileExtentions.GetFilePath(_env.WebRootPath, "assets/images", image.Image);
                    FileExtentions.DeleteFile(path);
                    _context.PlaylistImages.Remove(image);
                    await _context.SaveChangesAsync();
                }
                dbPlaylist.Images.FirstOrDefault().IsMain = true;
                response.Id = dbPlaylist.Images.FirstOrDefault().Id;
                await _context.SaveChangesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetMainImage(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                PlaylistImage image = await _playlistService.GetImageById((int)id);
                if (image is null) return NotFound();
                Playlist dbPlaylist = await _playlistService.GetProductByImageId((int)id);
                if (dbPlaylist is null) return NotFound();

                if (!image.IsMain)
                {
                    image.IsMain = true;
                    await _context.SaveChangesAsync();
                }
                var images = dbPlaylist.Images.Where(m => m.Id != id).ToList();

                foreach (var item in images)
                {
                    if (item.IsMain)
                    {
                        item.IsMain = false;
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(image.IsMain);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }

        private async Task<SelectList> GetCategory()
        {
            List<Category> category = await _categoryService.GetAll();
            return new SelectList(category, "Id", "Name");
        }

        private async Task GetCategories()
        {
            ViewBag.categories = await GetCategory();


        }


        private List<PlaylistVM> GetMappedDatas(List<Playlist> playlists)
        {
            List<PlaylistVM> mappedDatas = new();
            foreach (var playlist in playlists)
            {
                PlaylistVM playlistList = new()
                {
                    Id = playlist.Id,
                    Name = playlist.Name,
                    Images = playlist.Images.ToList(),

                };
                mappedDatas.Add(playlistList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var playlistCount = await _playlistService.GetCountAsync();

            return (int)Math.Ceiling((decimal)playlistCount / take);
        }
    }
}
