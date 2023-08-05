using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Areas.Admin.ViewModels.Advert;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;


        public AdvertController(IAdvertService advertService,
                                    IWebHostEnvironment env,
                                    AppDbContext context)
        {
            _advertService = advertService;
            _env = env;
            _context = context;
        }



        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Advert> datas = await _advertService.GetPaginatedDatas(page, take);
            List<AdvertVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<AdvertVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }

        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Advert dbAdvert = await _advertService.GetByIdAsync((int)id);
                if (dbAdvert is null) return NotFound();
                ViewBag.page = page;

                AdvertDetailVM model = new()
                {
                    Id = dbAdvert.Id,
                   Image= dbAdvert.Image,
                   CreatedDate=dbAdvert.CreatedDate.ToString("MMMM dd, yyyy"),
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> Create(AdvertCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            foreach (var item in request.Images)
            {

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Please select only image file");
                    return View();
                }

                if (item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Image", "Image size must be max 200KB");
                    return View();
                }
            }

            await _advertService.CreateAsync(request.Images);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Delete(int id)
        {

            await _advertService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Advert dbAdvert = await _advertService.GetByIdAsync((int)id);

            if (dbAdvert is null) return NotFound();

            return View(new AdvertEditVM
            {
                Image = dbAdvert.Image,
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Edit(int? id, AdvertEditVM request)
        {
            if (id is null) return BadRequest();

            Advert dbAdvert = await _advertService.GetByIdAsync((int)id);

            if (dbAdvert is null) return NotFound();


            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = dbAdvert.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200KB");
                request.Image = dbAdvert.Image;
                return View(request);
            }

            if (request.NewImage is null) return RedirectToAction(nameof(Index));


            await _advertService.EditAsync(dbAdvert, request.NewImage);


            return RedirectToAction(nameof(Index));
        }

        private List<AdvertVM> GetMappedDatas(List<Advert> products)
        {
            List<AdvertVM> mappedDatas = new();
            foreach (var product in products)
            {
                AdvertVM productList = new()
                {
                    Id = product.Id,
                    Image= product.Image,
                };
                mappedDatas.Add(productList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _advertService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
