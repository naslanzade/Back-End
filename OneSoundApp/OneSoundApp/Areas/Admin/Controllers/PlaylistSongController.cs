using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.PlaylistSong;
using OneSoundApp.Areas.Admin.ViewModels.Record;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class PlaylistSongController : Controller
    {
        private readonly IPlaylistSongService _playlistSongService;
        private readonly IPlaylistService _playlistService;
        private readonly AppDbContext _context;


        public PlaylistSongController(IPlaylistSongService playlistSongService,
                                     IPlaylistService playlistService,
                                     AppDbContext context)
        {
            _context = context;
            _playlistSongService = playlistSongService;
            _playlistService = playlistService;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<PlaylistSong> datas = await _playlistSongService.GetPaginatedDatas(page, take);
            List<PlaylistSongVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<PlaylistSongVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                PlaylistSong dbSong = await _playlistSongService.GetByIdAsync((int)id);
                if (dbSong is null) return NotFound();
                ViewBag.page = page;

                PlaylistSongDetailVM model = new()
                {
                    Id = dbSong.Id,
                    Name = dbSong.Name,
                    PlaylistName = dbSong.Playlist.Name,
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
            await GetPlaylists();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaylistSongCreateVM request)
        {
            await GetPlaylists();
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _playlistSongService.CreateAsync(request);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await GetPlaylists();
            if (id is null) return BadRequest();

            PlaylistSong dbSong = await _playlistSongService.GetByIdAsync((int)id);

            if (dbSong is null) return NotFound();

            return View(new PlaylistSongEditVM
            {
                Name = dbSong.Name,
                PlaylistId = dbSong.Playlist.Id,

            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, PlaylistSongEditVM request)
        {
            if (id is null) return BadRequest();

            PlaylistSong existSong = await _context.PlaylistSongs.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existSong is null) return NotFound();

            if (existSong.Name.Trim() == request.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            await _playlistSongService.EditAsync(request);

            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _playlistSongService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }



        private async Task<SelectList> GetPlaylist()
        {
            List<Playlist> playlists = await _playlistService.GetAllAsync();
            return new SelectList(playlists, "Id", "Name");
        }

        private async Task GetPlaylists()
        {
            ViewBag.playlists = await GetPlaylist();

        }


        private List<PlaylistSongVM> GetMappedDatas(List<PlaylistSong> songs)
        {
            List<PlaylistSongVM> mappedDatas = new();
            foreach (var song in songs)
            {
                PlaylistSongVM songList = new()
                {
                    Id = song.Id,
                    Name = song.Name,
                };
                mappedDatas.Add(songList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var songCount = await _playlistSongService.GetCountAsync();

            return (int)Math.Ceiling((decimal)songCount / take);
        }
    }
}
