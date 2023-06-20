using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using IronPdf.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_Management_Demo.Controllers
{
    public class MethodsController : Controller
    {
        private readonly IDashboardRepository _dash;
        private readonly IEventsPageRepository _eventsPage;
        public MethodsController(IDashboardRepository dashboardRepository, IEventsPageRepository eventsPage)
        {
            _dash = dashboardRepository;
            _eventsPage = eventsPage;
        }
        
        [HttpGet]
        public IActionResult EventsDataPieChart()
        {
            var eventsData =  _dash.EventsDataForPieChart();
            return Json(eventsData);
        }
        [HttpGet]
        public IActionResult UsersDataPieChart()
        {
            var usersData = _dash.UsersDataForPieChart();
            return Json(usersData);
        }
        [HttpGet]
        public IActionResult EventsDataBarChart()
        {
            var annualEvents = _dash.AnnualEventsData();
            return Json(annualEvents);
        }
        [HttpGet]
        public User LoggedUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(ClaimTypes.Email)?.Value;
            var user = _dash.GetThisUser(email);
            return user;
        }
        public bool IfValidDesc(string eventDesc)
        {
            var ifValid = eventDesc.Replace(" ", "");
            if (ifValid.Length >= 20)
            {
                return true;
            }
            return false;
        }
        public IActionResult PdfDownload(long eventId)
        {
            var renderer = new ChromePdfRenderer();
            renderer.RenderingOptions.PrintHtmlBackgrounds = true;
            renderer.RenderingOptions.PaperOrientation = IronPdf.Rendering.PdfPaperOrientation.Landscape;
            renderer.RenderingOptions.MarginTop = 0; // millimeters
            renderer.RenderingOptions.MarginBottom = 0; // millimeters
            renderer.RenderingOptions.MarginLeft = 0; // millimeters
            renderer.RenderingOptions.MarginRight = 0; // millimeters
            renderer.RenderingOptions.CssMediaType = PdfCssMediaType.Print;

            var url = Url.Action("EventInvitation", "Methods", new { eventId }, Request.Scheme);
            var pdf = renderer.RenderUrlAsPdf(url);

            // You can return the PDF as a file result
            return File(pdf.BinaryData, "application/pdf", "pixel-perfect.pdf");

        }

        public IActionResult EventInvitation(long eventId)
        {
            var eventToBeShared = _eventsPage.GetEventForUpdate(eventId);
            
            return View(eventToBeShared);
        }

        [HttpGet]
        public IActionResult NotifiactionData()
        {
            var noti = _dash.Notificatioons();
            return Json(noti);
        }
    }
}
