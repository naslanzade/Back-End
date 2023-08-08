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
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public PodcastController(IPodcastService podcastService,
                                 IAuthorService authorService,
                                 AppDbContext context,
                                 IWebHostEnvironment env)
        {
            _authorService = authorService;
            _podcastService = podcastService;
            _context = context;
            _env = env;

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
            await GetAuthors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PodcastCreateVM request)
        {
            await GetAuthors();

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

            await _podcastService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await GetAuthors();

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
            await GetAuthors();

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

                    if (item.CheckFileSize(200))
                    {
                        ModelState.AddModelError("NewImage", "Image size must be max 200KB");
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
        public async Task<IActionResult> DeleteProductImage(int Id)
        {
            await _podcastService.DeleteImageByIdAsync(Id);
            return Ok();

        }


        private async Task<SelectList> GetAuthor()
        {
            List<Author> authors = await _authorService.GetAllAsync();
            return new SelectList(authors, "Id", "Name");
        }

        private async Task GetAuthors()
        {
            ViewBag.authors = await GetAuthor();
           

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
