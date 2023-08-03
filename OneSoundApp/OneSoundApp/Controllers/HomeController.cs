using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Home;
using OneSoundApp.ViewModels.Subscribe;

namespace OneSoundApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;
        private readonly IEventService _eventService;
        private readonly ISingerService _singerService;
        private readonly AppDbContext _context;


        public HomeController(ISliderService sliderService,
                              IAlbumService albumService,
                              ISongService songService,
                              IEventService eventService,
                              ISingerService singerService,
                              AppDbContext context)
        {
            _sliderService = sliderService;
            _albumService = albumService;
            _songService = songService;
            _eventService = eventService;
            _singerService = singerService;
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();
            IEnumerable<Album> albums = await _albumService.GetLatestAlbumAsync();
            IEnumerable<Song> songs = await _songService.GetLatestSongAsync();
            IEnumerable<Event> events = await _eventService.GetLatestEventsAsync();
            IEnumerable<Album> topAlbums = await _albumService.GetTopAlbumAsync();
            IEnumerable<Song> topSongs = await _songService.GetLatestSongAsync();
            IEnumerable<Singer> singers = await _singerService.GetTopSingerAsync();




            HomeVM model = new()
            {
                Sliders = sliders,
                Albums = albums,
                Songs = songs,
                Events = events,
                TopAlbums = topAlbums,
                TopSongs = topSongs,
                Singers = singers

            };


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostSubscribe(SubscribeVM model)
        {
            try
            {
                if (!ModelState.IsValid) return RedirectToAction("Index", model);
                var existSubscribe = await _context.Subscribes.FirstOrDefaultAsync(m => m.Email == model.Email);
                if (existSubscribe != null)
                {
                    ModelState.AddModelError("Email", "Email already exist");
                    return RedirectToAction("Index");
                }
                Subscribe subscribe = new()
                {
                    Email = model.Email,
                };
                await _context.Subscribes.AddAsync(subscribe);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }

        }


    }
}