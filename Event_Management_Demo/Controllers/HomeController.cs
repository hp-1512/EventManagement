using Microsoft.AspNetCore.Mvc;

namespace Event_Management_Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
