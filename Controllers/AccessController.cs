using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MobileStore.Models;
using MobileStore.Data;
using Microsoft.EntityFrameworkCore;

namespace MobileStore.Controllers
{
    public class AccessController : Controller
    {
        private readonly MobileStoreContext _context;

        public AccessController(MobileStoreContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(string username, string password)
        {
            var user1 = _context.GetUserByUsernameAndPassword(username.ToLower(), password.ToLower());
            if (user1 != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,username),
                    new Claim(ClaimTypes.Role,"role")
                };

                var user = _context.Users
                   .Include(u => u.Roles) // Include the Roles navigation property
                   .FirstOrDefault(u => u.UserId == user1.UserId);

                if (user != null)
                {
                    var rolesForUser = user.Roles.ToList();
                    // Now you have a list of Role entities associated with the user.
                    foreach (var role in rolesForUser)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                    }
                }

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = Request.Form["KeepLoggedIn"].Contains("on")
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["Message"] = "User not found";
            return View();
        }
    }
}
