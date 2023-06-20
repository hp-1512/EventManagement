using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Event_Management_Demo.Models;
using EventServices.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Event_Management_Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _acc;
        private readonly IEmailHelper _emailHelper;
        public AccountController(IAccountRepository acc, IEmailHelper emailHelper)
        {
            _acc = acc;
            _emailHelper = emailHelper;
        }
        #region Unique Username and Email Validation
        public bool IfUserNameExist(string UserName)
        {
            var ifUsernameExist = _acc.GetUserName(UserName);
            if(ifUsernameExist.Result == null)
            {
                return true;
            }
            return false;
        }
        public bool IfEmailExist(string Email)
        {
            var ifUsernameExist = _acc.GetEmail(Email);
            if (ifUsernameExist.Result == null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Registration
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(User userModel)
        {
            try
            {
                userModel.Password = _acc.GetHashPassword(userModel.Password);
                userModel.IsRegSuccess = 0;
                userModel.IsActive = 0;
                //saving data of user
                var registerUser = await _acc.CreateUser(userModel);

                //generating unique token
                var token = Guid.NewGuid().ToString();

                RegistrationStatus appUser = new RegistrationStatus
                {
                    UserId = registerUser,
                    Email = userModel.Email,
                    Token = token,
                };
                //storing token for verifying purpose
                var tokenStore = _acc.StoreToken(appUser);

                //Mail Sending Operarion
                var subject = "Confirm Your Email";
                var decodedverificationLink = Url.Action("ConfirmEmail", "Email", new { email = userModel.Email, token }, Request.Scheme);
                var verificationLink = Url.ActionLink(System.Net.WebUtility.UrlEncode(decodedverificationLink));
                var message = $"Please click on the following link to verify your email:<a href = '{decodedverificationLink}'>{verificationLink}</a>";
                bool emailResponse = _emailHelper.SendEmail(userModel.Email, subject, message);

                if (emailResponse)
                {
                    return View("~/Views/Email/EmailSent.cshtml");
                }
                else
                {
                    return View("~/Views/Email/Error.cshtml");
                }
                //End of Mail Sending Operarion 
            }
            catch
            {
                return RedirectToAction("Registration");
            }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if(userLogin.UserName != null && userLogin.Password!= null)
            {
                var ifActive = await _acc.IfActiveUser(userLogin.UserName, userLogin.Password);
                if (ifActive == null)
                {
                    TempData["error"] = "Invalid Credentials!";
                    return View(userLogin);
                }
                else
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, ifActive.Email) },
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, ifActive.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Email, ifActive.Email));
                    //identity.AddClaim(new Claim(ClaimTypes.Role, status.Role));
                    var principal = new ClaimsPrincipal(identity);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                    };


                    await HttpContext.SignInAsync(principal, authProperties);
                    HttpContext.Session.SetString("EmailId", ifActive.Email);
                    return RedirectToAction("Dashboard", "Home");
                }

            }
            TempData["error"] = "Invalid Credebtials!";
            return View(userLogin);

        }
        #endregion

        #region Logout
        //---------------------for logout--------------------------//
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookie in storedCookies)
            {
                Response.Cookies.Delete(cookie);
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        #endregion

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