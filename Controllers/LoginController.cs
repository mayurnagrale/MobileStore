using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MobileStore.Models; 

namespace MobileStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly MobileStoreContext _context;

        public LoginController(MobileStoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(string username, string password,bool KeepLoggedIn)
        {
            // Perform authentication logic here
            var user1 = _context.GetUserByUsernameAndPassword(username.ToLower(), password.ToLower());
            var x = Request.Form["KeepLoggedIn"].Contains("on");
            if (user1 != null)
            {
                // Successful login
                ViewBag.Message = "Logged in successfully";
                TempData["SuccessMessage"] = "Login successful! Welcome to the Mobile Phone Store.";

                
                var user = _context.Users
                   .Include(u => u.Roles) // Include the Roles navigation property
                   .FirstOrDefault(u => u.Id == user1.Id);

                if (user != null)
                {
                    var rolesForUser = user.Roles.ToList();
                    // Now you have a list of Role entities associated with the user.
                }
                // Store user information in session, cookie, or use ASP.NET Core Identity
                // Redirect to the main page or any other authorized area

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,username),
                    new Claim(ClaimTypes.Role,"role")
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = x
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

                return RedirectToAction("Index", "Home","Message");
            }

            // Failed login
            // You can add an error message to the ViewBag or ModelState to show in the view
            ModelState.AddModelError("", "Invalid username or password.");
            return View("Index");
        }
        public IActionResult Index()
        {
            //ClaimsPrincipal claimUser = HttpContext.User;

            //if (claimUser.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            return View();
        }
    }
}
