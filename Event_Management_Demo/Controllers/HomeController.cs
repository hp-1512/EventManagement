using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_Demo.Controllers
{
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
        
        public IActionResult CreateEvent()
        {
            return View();
        }
    }
}
