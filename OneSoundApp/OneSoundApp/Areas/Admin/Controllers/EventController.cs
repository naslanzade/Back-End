using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Areas.Admin.ViewModels.Event;
using OneSoundApp.Data;
using OneSoundApp.Helpers;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;

namespace OneSoundApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;



        public EventController(IEventService eventService,
                                IWebHostEnvironment env,
                                 AppDbContext context)
        {
            _eventService = eventService;
            _env = env;
            _context = context;
        }
        public async Task<IActionResult> Index(int page=1, int take=5)
        {
            List<Event> datas = await _eventService.GetPaginatedDatas(page, take);
            List<EventVM> mappedDatas = GetMappedDatas(datas);
            int pageCount = await GetPageCountAsync(take);
            ViewBag.take = take;
            Paginate<EventVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return View(paginatedDatas);
        }


        public async Task<IActionResult> Detail(int? id, int page)
        {
            try
            {
                if (id is null) return BadRequest();
                Event dbEvents = await _eventService.GetByIdAsync((int)id);
                if (dbEvents is null) return NotFound();
                ViewBag.page = page;

                EventDetailVM model = new()
                {
                    Id = dbEvents.Id,
                    Image = dbEvents.Image,
                    EventName = dbEvents.EventName,
                    Location = dbEvents.Location,
                    CreatedDate = dbEvents.CreatedDate.ToString("MMMM dd, yyyy"),
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
        public async Task<IActionResult> Create(EventCreateVM request)
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

            await _eventService.CreateAsync(request.Images, request);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Event dbEvent = await _eventService.GetByIdAsync((int)id);

            if (dbEvent is null) return NotFound();

            return View(new EventEditVM
            {
                Image = dbEvent.Image,
                EventName = dbEvent.EventName,
                Location=dbEvent.Location,

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, EventEditVM request)
        {
            if (id is null) return BadRequest();

            Event existEvent = await _context.Events.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (existEvent is null) return NotFound();

            if (existEvent.EventName.Trim() == request.EventName)
            {
                return RedirectToAction(nameof(Index));
            }
            if (existEvent.Location.Trim() == request.Location)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file");
                request.Image = existEvent.Image;
                return View(request);
            }

            if (request.NewImage.CheckFileSize(500))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 500KB");
                request.Image = existEvent.Image;
                return View(request);
            }

            await _eventService.EditAsync(request, request.NewImage);


            return RedirectToAction(nameof(Index));
        }


        private List<EventVM> GetMappedDatas(List<Event> events)
        {
            List<EventVM> mappedDatas = new();
            foreach (var item in events)
            {
                EventVM eventList = new()
                {
                    Id = item.Id,
                    Image = item.Image,
                    EventName = item.EventName,
                };
                mappedDatas.Add(eventList);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var albumCount = await _eventService.GetCountAsync();

            return (int)Math.Ceiling((decimal)albumCount / take);
        }
    }
}
