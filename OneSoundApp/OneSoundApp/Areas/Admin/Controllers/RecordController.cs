using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Records;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecordController : Controller
    {

        private readonly IRecordService _recordService;
        private readonly IPodcastService _podcastService;
        private readonly AppDbContext _context;


        public RecordController(IRecordService recordService,
                                IPodcastService podcastService,
                                AppDbContext context)
        {
            _context = context;
            _recordService = recordService;
            _podcastService = podcastService;
        }



        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Record> datas = await _recordService.GetPaginatedDatas(page, take);
            List<RecordVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<RecordVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Record dbRecord = await _recordService.GetByIdAsync((int)id);
                if (dbRecord is null) return NotFound();
                ViewBag.page = page;

                RecordDetailVM model = new()
                {
                    Id = dbRecord.Id,
                    Name = dbRecord.Name,
                    PodcastName=dbRecord.Podcast.Name,
                    CreatedDate = dbRecord.CreatedDate.ToString("MMMM dd, yyyy"),
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
            await GetPodcasts();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecordCreateVM request)
        {
            await GetPodcasts();
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _recordService.CreateAsync(request);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await GetPodcasts();
            if (id is null) return BadRequest();

            Record dbRecord = await _recordService.GetByIdAsync((int)id);

            if (dbRecord is null) return NotFound();

            return View(new RecordEditVM
            {
                Name = dbRecord.Name,
                PodcastId=dbRecord.Podcast.Id,

            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, RecordEditVM request)
        {
            if (id is null) return BadRequest();

            Record existRecord = await _context.Record.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existRecord is null) return NotFound();

            if (existRecord.Name.Trim() == request.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            await _recordService.EditAsync(request);

            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _recordService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> GetPodcast()
        {
            List<Podcast> podcasts = await _podcastService.GetAll();
            return new SelectList(podcasts, "Id", "Name");
        }

        private async Task GetPodcasts()
        {            
            ViewBag.podcast = await GetPodcast();

        }


        private List<RecordVM> GetMappedDatas(List<Record> records)
        {
            List<RecordVM> mappedDatas = new();
            foreach (var record in records)
            {
                RecordVM recordList = new()
                {
                    Id = record.Id,
                    Name = record.Name,
                };
                mappedDatas.Add(recordList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var recordCount = await _recordService.GetCountAsync();

            return (int)Math.Ceiling((decimal)recordCount / take);
        }
    }
}
