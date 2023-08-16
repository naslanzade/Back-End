using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneSoundApp.Areas.Admin.ViewModels.Podcast;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;


namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PodcastController : Controller
    {

        private readonly IPodcastService _podcastService;
        private readonly IAuthorService _authorService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public PodcastController(IPodcastService podcastService,
                                 IAuthorService authorService,
                                 AppDbContext context,
                                 IWebHostEnvironment env,
                                 ICategoryService categoryService)
        {
            _authorService = authorService;
            _podcastService = podcastService;
            _context = context;
            _env = env;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Podcast> datas = await _podcastService.GetPaginatedDatas(page, take);
            List<PodcastVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<PodcastVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Podcast dbPodcast = await _podcastService.GetWithIncludesAsync(id);
                if (dbPodcast is null) return NotFound();
                ViewBag.page = page;

                PodcastDetailVM model = new()
                {
                    Id = dbPodcast.Id,
                    Images = dbPodcast.Images,
                    Description=dbPodcast.Description,
                    Name = dbPodcast.Name,
                    AuthorName=dbPodcast.Author.Name,
                    Records=dbPodcast.Records,
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
            await GetAuthorAndCategory();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PodcastCreateVM request)
        {
            await GetAuthorAndCategory();

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

            await _podcastService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await GetAuthorAndCategory();

            if (id is null) return BadRequest();

            Podcast podcast = await _podcastService.GetWithIncludesAsync(id);

            if (podcast is null) return NotFound();

            PodcastEditVM response = new()
            {
                Name = podcast.Name,
                Description = podcast.Description,                
                AuthorId = (int)podcast.AuthorId,
                Images = podcast.Images.ToList(),

            };

            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PodcastEditVM request)
        {
            await GetAuthorAndCategory();

            Podcast product = await _podcastService.GetWithIncludesAsync(id);


            if (request.newImages != null)
            {

                foreach (var item in request.newImages)
                {
                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImage", "Please select only image file");
                        request.Images = product.Images.ToList();
                        return View(request);
                    }

                    if (item.CheckFileSize(500))
                    {
                        ModelState.AddModelError("NewImage", "Image size must be max 500KB");
                        request.Images = product.Images.ToList();
                        return View(request);
                    }
                }
            }

            await _podcastService.EditAsync(id, request);

            return RedirectToAction(nameof(Index));


        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            await _podcastService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int? id)
        {
            try
            {
                if (id is null) return BadRequest();
                PodcastImage image = await _podcastService.GetImageById((int)id);
                if (image is null) return NotFound();
                Podcast dbPodcast = await _podcastService.GetPodcastByImageId((int)id);
                if (dbPodcast is null) return NotFound();
                RemoveImageResponse response = new();
                response.Result = false;

                if (dbPodcast.Images.Count > 1)
                {
                    string path = FileExtentions.GetFilePath(_env.WebRootPath, "assets/images", image.Image);
                    FileExtentions.DeleteFile(path);
                    _context.PodcastImage.Remove(image);
                    await _context.SaveChangesAsync();
                }
                dbPodcast.Images.FirstOrDefault().IsMain = true;
                response.Id = dbPodcast.Images.FirstOrDefault().Id;
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
                PodcastImage image = await _podcastService.GetImageById((int)id);
                if (image is null) return NotFound();
                Podcast dbPodcast = await _podcastService.GetPodcastByImageId((int)id);
                if (dbPodcast is null) return NotFound();

                if (!image.IsMain)
                {
                    image.IsMain = true;
                    await _context.SaveChangesAsync();
                }
                var images = dbPodcast.Images.Where(m => m.Id != id).ToList();

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
        private async Task<SelectList> GetAuthor()
        {
            List<Author> authors = await _authorService.GetAllAsync();
            return new SelectList(authors, "Id", "Name");
        }

        private async Task GetAuthorAndCategory()
        {
            ViewBag.authors = await GetAuthor();
            ViewBag.categories = await GetCategory();


        }


        private List<PodcastVM> GetMappedDatas(List<Podcast> podcasts)
        {
            List<PodcastVM> mappedDatas = new();
            foreach (var podcast in podcasts)
            {
                PodcastVM podcastList = new()
                {
                    Id = podcast.Id,
                    Name = podcast.Name,
                    Images = podcast.Images.ToList(),

                };
                mappedDatas.Add(podcastList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var podcastCount = await _podcastService.GetCountAsync();

            return (int)Math.Ceiling((decimal)podcastCount / take);
        }
    }
}
