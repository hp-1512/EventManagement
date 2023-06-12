using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using EventServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_Demo.Controllers
{
    [Authorize]
    public class HomeController : MethodsController
    {
        private readonly IEventsPageRepository _eventsPage;
        private readonly IEmailHelper _emailHelper;
        public HomeController(IEventsPageRepository EventsPage, IEmailHelper emailHelper, IDashboardRepository dashboardRepository) : base(dashboardRepository)
        {
            _eventsPage = EventsPage;
            _emailHelper = emailHelper;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult EventsPage()
        {
            var user = LoggedUser();
            var userId = user.UserId;
            var eventsList = _eventsPage.EventsDataList(userId);
            EventsDetailList events = new()
            {
                ListOfEvents = eventsList,
            };
            return View(events);
        }

        public IActionResult Participate(long eventId)
        {
            var user = LoggedUser();
            var userId = user.UserId;
            var status = _eventsPage.Participate(userId,eventId);
            if (status != null)
            {
                var subject = "Invitation For Event";
                var message = $"You've Succesfully Participated and heartly Invited for the Event <b><u>{status.EventTitle}</u></b>" +
                    $" starting from <b>{status.StartDate.ToShortDateString()}</b> to <b>{status.EndDate.ToShortDateString()}</b>.<br>" +
                    $"Looking Forward to see you!!!";
                bool emailResponse = _emailHelper.SendEmail(user.Email, subject, message);
                if (emailResponse)
                {
                    return RedirectToAction("EventsPage");
                }
            }
            return View("~/Views/Email/Error.cshtml");
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
                _eventsPage.CreateEventDb(eventObj, eventImagesList, preloaded);
                var user = LoggedUser();
                var subject = "Event Created Successfully";
                var message = $"You've Succesfully Created an Event titled <b>{eventObj.EventTitle}</b> starting from <b>{eventObj.StartDate.ToShortDateString()}</b> to <b>{eventObj.EndDate.ToShortDateString()}</b>.";
                bool emailResponse = _emailHelper.SendEmail(user.Email, subject, message);
                if (emailResponse)
                {
                    return RedirectToAction("EventsPage");
                }
                else
                {
                    return View("~/Views/Email/Error.cshtml");
                }
            }
            return View(eventObj);
        }
        
        [HttpGet]
        public IActionResult CreatedEvents()
        {
            var user = LoggedUser();
            
            var eventsList = _eventsPage.CreatedEventsList(user.UserId);
            EventsDetailList events = new()
            {
                ListOfEvents = eventsList,
            };
            return View(events);
        }

        public bool DeleteEvent(long eventId,string resasonToDelete)
        {
            //var isDeleted = _eventsPage.DeleteEvent(eventId, resasonToDelete);
            return false;
        }

        [HttpGet]
        public IActionResult ParticipatedEvents()
        {
            var user = LoggedUser();
            var userId = user.UserId;
            var eventsList = _eventsPage.ParticipatedEventsList(userId);
            EventsDetailList events = new()
            {
                ListOfEvents = eventsList,
            };
            return View(events);
        }
    }
}
