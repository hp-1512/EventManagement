using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_Demo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEventsPageRepository _EventsPage;
        public HomeController(IEventsPageRepository EventsPage)
        {
            _EventsPage = EventsPage;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult EventsPage()
        {
            var eventsList = _EventsPage.EventsDataList();
            EventsDetailList events = new()
            {
                ListOfEvents = eventsList,
            };
            return View(events);
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateEvent(EventCreation eventObj ,List<IFormFile> eventImagesList, string[] preloaded)
        {
            if (ModelState.IsValid)
            {
                _EventsPage.CreateEventDb(eventObj, eventImagesList, preloaded);
            }
            return View(eventObj);
        }
    }
}
