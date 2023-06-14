using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    }
}
