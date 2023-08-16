using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Category;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;



        public CategoryController(ICategoryService categoryService,
                                IWebHostEnvironment env,
                                 AppDbContext context)
        {
            _categoryService = categoryService;
            _env = env;
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Category> datas = await _categoryService.GetPaginatedDatas(page, take);
            List<CategoryVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<CategoryVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Category dbCategory = await _categoryService.GetByIdAsync((int)id);
                if (dbCategory is null) return NotFound();
                ViewBag.page = page;

                CategoryDetailVM model = new()
                {
                    Id = dbCategory.Id,
                    Image = dbCategory.Image,
                    Name = dbCategory.Name,                    
                    CreatedDate = dbCategory.CreatedDate.ToString("MMMM dd, yyyy"),
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
        public async Task<IActionResult> Create(CategoryCreateVM request)
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

            await _categoryService.CreateAsync(request.Images, request);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Category dbCategory = await _categoryService.GetByIdAsync((int)id);

            if (dbCategory is null) return NotFound();

            return View(new CategoryEditVM
            {
                Image = dbCategory.Image,
                Name = dbCategory.Name,
               
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id is null) return BadRequest();

            Category existCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existCategory is null) return NotFound();

            if (existCategory.Name.Trim() == request.Name)
            {
                return RedirectToAction(nameof(Index));
            }
          
            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = existCategory.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(500))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 500KB");
                request.Image = existCategory.Image;
                return View(request);
            }

            await _categoryService.EditAsync(request, request.NewImage);


            return RedirectToAction(nameof(Index));
        }


        private List<CategoryVM> GetMappedDatas(List<Category> categories)
        {
            List<CategoryVM> mappedDatas = new();
            foreach (var category in categories)
            {
                CategoryVM categoryList = new()
                {
                    Id = category.Id,
                    Image = category.Image,
                    Name = category.Name,
                   


                };
                mappedDatas.Add(categoryList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _categoryService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
