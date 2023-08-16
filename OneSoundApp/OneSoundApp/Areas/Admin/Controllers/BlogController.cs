using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneSoundApp.Areas.Admin.ViewModels.Blog;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;


namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IAuthorService _authorService;
        private readonly ICategoryService _categoryService;


        public BlogController(IBlogService blogService,
                              AppDbContext context,
                              IWebHostEnvironment env,
                              IAuthorService authorService,
                              ICategoryService categoryService)
        {

            _blogService = blogService;
            _context = context;
            _env = env;
            _authorService = authorService;
            _categoryService = categoryService;
        }



        public async Task<IActionResult> Index(int page=1,int take=5)
        {
            List<Blog> datas = await _blogService.GetPaginatedDatas(page, take);
            List<BlogVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<BlogVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Blog dbBlog = await _blogService.GetByIdAsnyc(id);
                if (dbBlog is null) return NotFound();
                ViewBag.page = page;

                BlogDetailVM model = new()
                {
                    Id = dbBlog.Id,
                    Image = dbBlog.Image,
                    Title = dbBlog.Title,
                    Description = dbBlog.Description,
                    CategoryName=dbBlog.Category.Name,
                    AuthorName=dbBlog.Author.Name,
                    CreatedDate = dbBlog.CreatedDate.ToString("MMMM dd, yyyy"),
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
        public async Task<IActionResult> Create(BlogCreateVM request)
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

            await _blogService.CreateAsync(request, request.Images);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]       
        public async Task<IActionResult> Edit(int? id)
        {
            await GetAuthorAndCategory();

            if (id is null) return BadRequest();

            Blog blog = await _blogService.GetByIdAsnyc(id);

            if (blog is null) return NotFound();

            BlogEditVM response = new()
            {
                Title = blog.Title,
                Image = blog.Image,
                AuthorId = blog.AuthorId,
                CategoryId = blog.CategoryId,
                Description = blog.Description,
            };

            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Edit(int? id, BlogEditVM request)
        {
            if (id is null) return BadRequest();

            Blog existBlog = await _blogService.GetByIdAsnyc(id);

            if (existBlog is null) return NotFound();

            if (existBlog.Title.Trim() == request.Title)
            {
                return RedirectToAction(nameof(Index));
            }
            if (existBlog.Description.Trim() == request.Description)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = existBlog.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(500))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 500KB");
                request.Image = existBlog.Image;
                return View(request);
            }

            await _blogService.EditAsync((int)id, request, request.NewImage);


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogService.GetByIdAsnyc(id);

            await _blogService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));

        }

        private async Task<SelectList> GetAuthors()
        {
            List<Author> authors = await _authorService.GetAllAsync();
            return new SelectList(authors, "Id", "Name");
        }

        private async Task<SelectList> GetCategories()
        {
            List<Category> categories = await _categoryService.GetAll();
            return new SelectList(categories, "Id", "Name");
        }

        private async Task GetAuthorAndCategory()
        {
            ViewBag.authors = await GetAuthors();
            ViewBag.categories=await GetCategories();

        }


        private List<BlogVM> GetMappedDatas(List<Blog> blogs)
        {
            List<BlogVM> mappedDatas = new();
            foreach (var blog in blogs)
            {
                BlogVM blogList = new()
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Image=blog.Image,

                };
                mappedDatas.Add(blogList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _blogService.GetCountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }
    }
}
