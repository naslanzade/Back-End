using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneSoundApp.Areas.Admin.ViewModels.Album;
using OneSoundApp.Areas.Admin.ViewModels.Playlist;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISingerService _singerService;


        public AlbumController(IAlbumService albumService,
                                 ICategoryService categoryService,
                                 AppDbContext context,
                                 IWebHostEnvironment env,
                                 ISingerService singerService)
        {
            _categoryService = categoryService;
            _albumService = albumService;
            _context = context;
            _env = env;
            _singerService = singerService;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Album> datas = await _albumService.GetPaginatedDatas(page, take);
            List<AlbumVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<AlbumVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Album dbAlbum = await _albumService.GetWithIncludesAsync(id);
                if (dbAlbum is null) return NotFound();
                ViewBag.page = page;

                AlbumDetailVM model = new()
                {
                    Id = dbAlbum.Id,
                    Images = dbAlbum.Images,
                    Description = dbAlbum.Description,
                    Name = dbAlbum.AlbumName,
                    Song = dbAlbum.Song,
                    SingerName=dbAlbum.Singer.Name,
                    Price = dbAlbum.Price,
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

            await GetCategoryAndSinger();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumCreateVM request)
        {
            await GetCategoryAndSinger();

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

                if (item.CheckFileSize(500))
                {
                    ModelState.AddModelError("image", "Image size must be max 500KB");
                    return View();
                }
            }

            await _albumService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await GetCategoryAndSinger();

            if (id is null) return BadRequest();

            Album album = await _albumService.GetWithIncludesAsync(id);

            if (album is null) return NotFound();

            AlbumEditVM response = new()
            {
                Name = album.AlbumName,
                Description = album.Description,
                CategoryId = (int)album.CategoryId,
                Images = album.Images.ToList(),
                SingerId= album.SingerId,
                Price = album.Price,

            };

            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlbumEditVM request)
        {
            await GetCategoryAndSinger();

            Album album = await _albumService.GetWithIncludesAsync(id);

            if (request.newImages != null)
            {

                foreach (var item in request.newImages)
                {
                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImage", "Please select only image file");
                        request.Images = album.Images.ToList();
                        return View(request);
                    }

                    if (item.CheckFileSize(500))
                    {
                        ModelState.AddModelError("NewImage", "Image size must be max 500KB");
                        request.Images = album.Images.ToList();
                        return View(request);
                    }
                }
            }

            await _albumService.EditAsync(id, request);

            return RedirectToAction(nameof(Index));


        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            await _albumService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int? id)
        {
            try
            {
                if (id is null) return BadRequest();
                AlbumImage image = await _albumService.GetImageById((int)id);
                if (image is null) return NotFound();
                Album dbAlbum = await _albumService.GetAlbumByImageId((int)id);
                if (dbAlbum is null) return NotFound();
                RemoveImageResponse response = new();
                response.Result = false;

                if (dbAlbum.Images.Count > 1)
                {
                    string path = FileExtentions.GetFilePath(_env.WebRootPath, "assets/images", image.Image);
                    FileExtentions.DeleteFile(path);
                    _context.AlbumImages.Remove(image);
                    await _context.SaveChangesAsync();
                }
                dbAlbum.Images.FirstOrDefault().IsMain = true;
                response.Id = dbAlbum.Images.FirstOrDefault().Id;
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
                AlbumImage image = await _albumService.GetImageById((int)id);
                if (image is null) return NotFound();
                Album dbAlbum = await _albumService.GetAlbumByImageId((int)id);
                if (dbAlbum is null) return NotFound();

                if (!image.IsMain)
                {
                    image.IsMain = true;
                    await _context.SaveChangesAsync();
                }
                var images = dbAlbum.Images.Where(m => m.Id != id).ToList();

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

        private async Task<SelectList> GetSinger()
        {
            List<Singer> singers = await _singerService.GetAll();
            return new SelectList(singers, "Id", "Name");
        }

        private async Task GetCategoryAndSinger()
        {
            ViewBag.categories = await GetCategory();
            ViewBag.singers = await GetSinger();


        }

        private List<AlbumVM> GetMappedDatas(List<Album> albums)
        {
            List<AlbumVM> mappedDatas = new();
            foreach (var album in albums)
            {
                AlbumVM albumtList = new()
                {
                    Id = album.Id,
                    Name = album.AlbumName,
                    Images = album.Images.ToList(),
                    Price = album.Price,
                    

                };
                mappedDatas.Add(albumtList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _albumService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
