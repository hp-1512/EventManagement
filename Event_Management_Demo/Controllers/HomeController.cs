using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using EventServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
            var status = _eventsPage.Participate(userId, eventId);
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
        public IActionResult CreateEvent(EventCreation eventObj, List<IFormFile> eventImagesList, string[] preloaded)
        {
            
            if (ModelState.IsValid && eventImagesList.Count()>0)
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
            if(eventImagesList.Count() == 0)
            {
            TempData["error"] = "Please Select at least one image.";
            }
            return View(eventObj);
        }

        [HttpGet]
        public IActionResult CreatedEvents()
        {
            var events =  CreatedEventsList();
            return View(events);
        }

        public EventsDetailList CreatedEventsList()
        {
            var user = LoggedUser();

            var eventsList = _eventsPage.CreatedEventsList(user.UserId);
            EventsDetailList events = new()
            {
                ListOfEvents = eventsList,
            };
            return events;
        }


        [HttpGet]
        public IActionResult UpdateEvent(long eventId)
        {
            var eventToBeUpdated = _eventsPage.GetEventForUpdate(eventId);
            return View(eventToBeUpdated);
        }
        [HttpPost]
        public IActionResult UpdateEvent(EventUpdation obj, List<IFormFile> eventImagesList, string[] preloaded)
        {
            var oldDataOfEvent = _eventsPage.GetEventForUpdate(obj.EventId);
            var eventMailingData = _eventsPage.UpdateEvent(obj, eventImagesList, preloaded);
            //string input = obj.StartTime.ToString();
            //TimeSpan timeSpan;
            //var x = TimeSpan.TryParseExact(input, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out timeSpan);
            //var z = timeSpan.ToString(@"hh\:mm\:ss tt");
            //TimeSpan input = new TimeSpan(obj.StartTime.Hours, obj.StartTime.Minutes, obj.StartTime.Seconds);
            //var y = (input).ToString(@"hh:mm:ss tt");
            if (oldDataOfEvent.EventTitle != obj.EventTitle || oldDataOfEvent.StartDate != obj.StartDate || oldDataOfEvent.EndDate != obj.EndDate || oldDataOfEvent.StartTime!=obj.StartTime || oldDataOfEvent.EndTime != obj.EndTime || oldDataOfEvent.Vanue != obj.Vanue)
            {
                foreach(var participatedUser in eventMailingData)
                {
                    var subject = "Update Of the Event"; 
                    var message = $"There was an update for Event - <b>{obj.EventTitle}</b> starting from <b>{obj.StartDate.ToShortDateString()}</b> {Convert.ToDateTime(obj.StartTime).ToString("hh:mm:ss tt")} to <b>{obj.EndDate.ToShortDateString()} {Convert.ToDateTime(obj.EndTime).ToString("hh:mm:ss tt")}</b>.";
                    bool emailResponse = _emailHelper.SendEmail(participatedUser.Email, subject, message);
                    if (emailResponse)
                    {
                        return RedirectToAction("CreatedEvents");
                    }
                    else
                    {
                        return View("~/Views/Email/Error.cshtml");
                    }
                }
            }
            return RedirectToAction("CreatedEvents");
        }
        [HttpPost]
        public IActionResult DeleteEvent(long eventId, string resasonToDelete)
        {
            var isDeleted = _eventsPage.DeleteEvent(eventId, resasonToDelete);
            if (isDeleted.Any())
            {
                foreach (var participatedUser in isDeleted)
                {
                    var subject = $"Cancellation of Event - {participatedUser.EventTitle}";
                    var message = $"I hope this email finds you well. I am writing to inform you that, unfortunately, we have made the difficult decision to cancel the <b>{participatedUser.EventTitle}</b> due to { resasonToDelete}. we apologize for any inconvenience caused and thank you for your understanding and continued support.<br>" ;
                    bool emailResponse = _emailHelper.SendEmail(participatedUser.Email, subject, message);
                    if (emailResponse)
                    {
                        return Json(true);
                    }
                }
            }
            return Json(false);
        }

        [HttpGet]
        public IActionResult ParticipatedEvents()
        {
            var events = ParticipatedEventsList();
            
            return View(events);
        }


        public EventsDetailList ParticipatedEventsList()
        {
            var user = LoggedUser();
            var userId = user.UserId;
            var eventsList = _eventsPage.ParticipatedEventsList(userId);
            EventsDetailList events = new()
            {
                ListOfEvents = eventsList,
            };
            return events;
        }
    }
}
