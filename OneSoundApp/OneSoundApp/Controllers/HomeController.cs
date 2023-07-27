using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Home;


namespace OneSoundApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;
        private readonly IEventService _eventService;
        private readonly ISingerService _singerService;


        public HomeController(ISliderService sliderService,
                              IAlbumService albumService,
                              ISongService songService,
                              IEventService eventService,
                              ISingerService singerService)
        {
            _sliderService = sliderService;
            _albumService = albumService;
            _songService = songService;
            _eventService = eventService;
            _singerService = singerService;

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

    
    }
}