using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OneSoundApp.Areas.Admin.ViewModels.Blog;
using OneSoundApp.Areas.Admin.ViewModels.Singer;
using OneSoundApp.Areas.Admin.ViewModels.Song;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SongController : Controller
    {
        private readonly ISongService _songService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IAlbumService _albumService;
        private readonly ISingerService _singerService;


        public SongController(ISongService songService,
                              AppDbContext context,
                              IWebHostEnvironment env,
                              IAlbumService albumService,
                              ISingerService singerService)
        {

            _songService = songService;
            _context = context;
            _env = env;
            _albumService = albumService;
            _singerService = singerService;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Song> datas = await _songService.GetPaginatedDatas(page, take);
            List<SongVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<SongVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Song dbSong = await _songService.GetByIdAsnyc(id);
                if (dbSong is null) return NotFound();
                ViewBag.page = page;

                SongDetailVM model = new()
                {
                    Id = dbSong.Id,
                    Image = dbSong.Image,
                    SongName = dbSong.SongName,                    
                    SingerName = dbSong.Singer.Name,
                    AlbumName = dbSong.Album.AlbumName,
                    CreatedDate = dbSong.CreatedDate.ToString("MMMM dd, yyyy"),
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
            await GetAlbumsAndSingers();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongCreateVM request)
        {
            await GetAlbumsAndSingers();

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

            await _songService.CreateAsync(request, request.Images);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await GetAlbumsAndSingers();

            if (id is null) return BadRequest();

            Song song = await _songService.GetByIdAsnyc(id);

            if (song is null) return NotFound();

            SongEditVM response = new()
            {
                SongName = song.SongName,
                Image = song.Image,
                AlbumId = (int)song.AlbumId,
                SingerId = song.SingerId,
               
            };

            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SongEditVM request)
        {
            if (id is null) return BadRequest();

            Song existSong = await _songService.GetByIdAsnyc(id);

            if (existSong is null) return NotFound();

            if (existSong.SongName.Trim() == request.SongName)
            {
                return RedirectToAction(nameof(Index));
            }
           
            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = existSong.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200KB");
                request.Image = existSong.Image;
                return View(request);
            }

            await _songService.EditAsync((int)id, request, request.NewImage);


            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _songService.GetByIdAsnyc(id);

            await _songService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));

        }


        private async Task<SelectList> GetAlbums()
        {
            List<Album> albums = await _albumService.GetAll();
            return new SelectList(albums, "Id", "AlbumName");
        }

        private async Task<SelectList> GetSingers()
        {
            List<Singer> singers = await _singerService.GetAll();
            return new SelectList(singers, "Id", "Name");
        }

        private async Task GetAlbumsAndSingers()
        {
            ViewBag.albums = await GetAlbums();
            ViewBag.singers = await GetSingers();

        }

        private List<SongVM> GetMappedDatas(List<Song> songs)
        {
            List<SongVM> mappedDatas = new();
            foreach (var blog in songs)
            {
                SongVM songList = new()
                {
                    Id = blog.Id,
                    SongName = blog.SongName,
                    Image = blog.Image,

                };
                mappedDatas.Add(songList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _songService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
