using Event_Management.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_Demo.Controllers
{
    public class EmailController : Controller
    {
        public EmailController()
        {

        }

        //public async Task<IActionResult> ConfirmEmail(string token, string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //        return View("Error");

        //    var result = await _userManager.ConfirmEmailAsync(user, token);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}
    }
}
