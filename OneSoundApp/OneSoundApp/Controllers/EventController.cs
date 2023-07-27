using Microsoft.AspNetCore.Mvc;
using OneSoundApp.Models;
using OneSoundApp.Services.Interfaces;
using OneSoundApp.ViewModels.Events;

namespace OneSoundApp.Controllers
{
    public class EventController : Controller
    {
       
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
           
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


    }
}
