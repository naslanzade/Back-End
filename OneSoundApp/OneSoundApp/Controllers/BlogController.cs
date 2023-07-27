using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Blog;

namespace OneSoundApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
       

        public BlogController(IAdvertService advertService,
                              IBlogService blogService,
                             ICategoryService categoryService)
        {
            
            _advertService = advertService;
            _blogService = blogService;
            _categoryService = categoryService;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 3)
        {
            List<Advert> adverts = await _advertService.GetAll();
            List<Blog> blogs = await _blogService.GetAll();
            List<Category> categories = await _categoryService.GetAll();
            List<Blog> paginateBlogs = await _blogService.GetPaginatedDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Blog> paginatedDatas = new(paginateBlogs, page, pageCount);

            BlogVM model = new()
            {
                Adverts= adverts,
                Blogs= blogs,
                Categories= categories,
                PaginatedDatas=paginatedDatas
            };
            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _blogService.GetCountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }
    }
}
