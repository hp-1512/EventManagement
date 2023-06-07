using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_Demo.Controllers
{
    public class MethodsController : Controller
    {
        private readonly IDashboardRepository _dash;
        public MethodsController(IDashboardRepository dashboardRepository)
        {
            _dash = dashboardRepository;
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
    }
}
