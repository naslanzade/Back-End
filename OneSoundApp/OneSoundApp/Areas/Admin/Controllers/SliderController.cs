
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Advert;
using OneSoundApp.Areas.Admin.ViewModels.Slider;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;



        public SliderController(ISliderService sliderService,
                                IWebHostEnvironment env,
                                 AppDbContext context)
        {
            _sliderService = sliderService;
            _env = env;
            _context = context;
        }


        public async Task<IActionResult> Index(int page = 1, int take=5)
        {
            List<Slider> datas = await _sliderService.GetPaginatedDatas(page, take);
            List<SliderVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<SliderVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }

        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Slider dbSlider = await _sliderService.GetByIdAsync((int)id);
                if (dbSlider is null) return NotFound();
                ViewBag.page = page;

                SliderDetailVM model = new()
                {
                    Id = dbSlider.Id,
                    Image = dbSlider.Image,
                    Header=dbSlider.Header,
                    Title=dbSlider.Title,
                    CreatedDate = dbSlider.CreatedDate.ToString("MMMM dd, yyyy"),
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
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
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

            await _sliderService.CreateAsync(request.Images, request);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Slider dbSlider = await _sliderService.GetByIdAsync((int)id);

            if (dbSlider is null) return NotFound();

            return View(new SliderEditVM
            {
                Image = dbSlider.Image,
                Title = dbSlider.Title,
                Header = dbSlider.Header,

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id is null) return BadRequest();

            Slider existSlider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existSlider is null) return NotFound();

            if (existSlider.Title.Trim() == request.Title)
            {
                return RedirectToAction(nameof(Index));
            }
            if (existSlider.Header.Trim() == request.Header)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = existSlider.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200KB");
                request.Image = existSlider.Image;
                return View(request);
            }

            await _sliderService.EditAsync(request, request.NewImage);


            return RedirectToAction(nameof(Index));
        }

        private List<SliderVM> GetMappedDatas(List<Slider> sliders)
        {
            List<SliderVM> mappedDatas = new();
            foreach (var slider in sliders)
            {
                SliderVM sliderList = new()
                {
                    Id = slider.Id,
                    Image=slider.Image,
                    Header=slider.Header,
                    Title=slider.Title,
                    
                    
                };
                mappedDatas.Add(sliderList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _sliderService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
