using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneSoundApp.Data;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Events;
using OneSoundApp.ViewModels.Subscribe;

namespace OneSoundApp.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEventService _eventService;
        private readonly ISubscribeService _subscribeService;


        public EventController(IEventService eventService, 
                              ISubscribeService subscribeService,
                              AppDbContext context)
        {
            _eventService = eventService;
            _subscribeService = subscribeService;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Event> events = await _eventService.GetEventsAsync();

            EventVM model = new()
            {
                Events = events
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
