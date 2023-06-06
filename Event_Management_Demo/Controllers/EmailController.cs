using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_Demo.Controllers
{
    
    public class EmailController : Controller
    {
        private readonly IAccountRepository _acc;
        public EmailController(IAccountRepository acc)
        {
            _acc = acc;
        }
        [HttpGet]
        public IActionResult ConfirmEmail(string email, string token)
        {
            var userToken = _acc.GetRegistrationToken(email, token);
            if (userToken.Result == null)
            {
                return View("Error");
            }
            else
            {
                _acc.ApproveRegStatus(email);
                return View("ConfirmEmail");
            }
        }
    }
}
