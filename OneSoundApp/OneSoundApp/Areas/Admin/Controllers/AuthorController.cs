using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Author;
using OneSoundApp.Areas.Admin.ViewModels.Category;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;
using System.Data;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly AppDbContext _context;

        public AuthorController(IAuthorService authorService,
                                AppDbContext context)
        {
            _authorService = authorService;
            _context = context;
        }



        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Author> datas = await _authorService.GetPaginatedDatas(page, take);
            List<AuthorVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<AuthorVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Author dbAuthor = await _authorService.GetByIdAsync((int)id);
                if (dbAuthor is null) return NotFound();
                ViewBag.page = page;

                AuthorDetailVM model = new()
                {
                    Id = dbAuthor.Id,                    
                    Name = dbAuthor.Name,
                    CreatedDate = dbAuthor.CreatedDate.ToString("MMMM dd, yyyy"),
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
        public async Task<IActionResult> Create(AuthorCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _authorService.CreateAsync(request);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Author dbAuthor = await _authorService.GetByIdAsync((int)id);

            if (dbAuthor is null) return NotFound();

            return View(new AuthorEditVM
            {
                Name = dbAuthor.Name,

            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Edit(int? id, AuthorEditVM request)
        {
            if (id is null) return BadRequest();

            Author existAuthor = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existAuthor is null) return NotFound();

            if (existAuthor.Name.Trim() == request.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            await _authorService.EditAsync(request);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        private List<AuthorVM> GetMappedDatas(List<Author> authors)
        {
            List<AuthorVM> mappedDatas = new();
            foreach (var author in authors)
            {
                AuthorVM authorList = new()
                {
                    Id = author.Id,                    
                    Name = author.Name,
                };
                mappedDatas.Add(authorList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _authorService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
