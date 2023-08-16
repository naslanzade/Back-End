using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Category;
using OneSoundApp.Areas.Admin.ViewModels.Singer;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SingerController : Controller
    {
        private readonly ISingerService _singerService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;



        public SingerController(ISingerService singerService,
                                IWebHostEnvironment env,
                                 AppDbContext context)
        {
            _singerService = singerService;
            _env = env;
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1,int take=5)
        {
            List<Singer> datas = await _singerService.GetPaginatedDatas(page, take);
            List<SingerVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<SingerVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Singer dbSinger = await _singerService.GetByIdAsync((int)id);
                if (dbSinger is null) return NotFound();
                ViewBag.page = page;

                SingerDetailVM model = new()
                {
                    Id = dbSinger.Id,
                    Image = dbSinger.Image,
                    Name = dbSinger.Name,
                    CreatedDate = dbSinger.CreatedDate.ToString("MMMM dd, yyyy"),
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
        public async Task<IActionResult> Create(SingerCreateVM request)
        {
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

            await _singerService.CreateAsync(request.Images, request);

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _singerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Singer dbSinger = await _singerService.GetByIdAsync((int)id);

            if (dbSinger is null) return NotFound();

            return View(new SingerEditVM
            {
                Image = dbSinger.Image,
                Name = dbSinger.Name,

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SingerEditVM request)
        {
            if (id is null) return BadRequest();

            Singer existSinger = await _context.Singers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existSinger is null) return NotFound();

            if (existSinger.Name.Trim() == request.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = existSinger.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(500))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 500KB");
                request.Image = existSinger.Image;
                return View(request);
            }

            await _singerService.EditAsync(request, request.NewImage);


            return RedirectToAction(nameof(Index));
        }



        private List<SingerVM> GetMappedDatas(List<Singer> singers)
        {
            List<SingerVM> mappedDatas = new();
            foreach (var singer in singers)
            {
                SingerVM singerList = new()
                {
                    Id = singer.Id,                   
                    Name = singer.Name,



                };
                mappedDatas.Add(singerList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _singerService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
