using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Event_Management_Demo.Models;
using EventServices.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Event_Management_Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _acc;
        //private readonly UserManager<RegistrationConfirmation> _userManager;
        private readonly IEmailHelper _emailHelper;
        public AccountController(IAccountRepository acc, IEmailHelper emailHelper)
        {
            _acc = acc;
            //_userManager = userManager;
            _emailHelper = emailHelper;
        }
        public bool IfUserNameExist(string UserName)
        {
            var ifUsernameExist = _acc.GetUserName(UserName);
            if(ifUsernameExist.Result == null)
            {
                return true;
            }
            return false;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(User userModel)
        {
            try
            {
                userModel.Password = _acc.GetHashPassword(userModel.Password);
                userModel.IsRegSuccess = 0;
                userModel.IsActive = 0;
                var registerUser = await _acc.CreateUser(userModel);
                var token = Guid.NewGuid().ToString();

                RegistrationStatus appUser = new RegistrationStatus
                {
                    UserId = registerUser,
                    Email = userModel.Email,
                    Token = token,
                };

                var tokenStore = _acc.StoreToken(appUser);

                var resetLink = Url.Action("ConfirmEmail", "Email", new { email = userModel.Email, token }, Request.Scheme);

                bool emailResponse = _emailHelper.SendEmail(userModel.Email, resetLink);

                if (emailResponse)
                {
                    return RedirectToAction("Registration");
                }
                else
                {
                    // log email failed 
                }
            }
            catch
            {
                return RedirectToAction("Registration");
            }
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}