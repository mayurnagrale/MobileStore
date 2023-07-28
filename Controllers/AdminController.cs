using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileStore.Data;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly MobileStoreContext _context;
        List<User> users = new List<User>();
        public AdminController(MobileStoreContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            List<User> User1 = new List<User>();
            users = _context.Users.ToList();
            foreach (var item in users)
            {
                var roles = _context.Roles.Find(item.Id);
                var newuser = new User
                {
                    Id = item.Id,
                    Username = item.Username,
                    Password = item.Password,
                };
                newuser.Roles.Add(roles);
                User1.Add(newuser);

            }
             return View(User1);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string username, string password)
        {
            var roles = _context.Roles.Where(x => x.RoleId != 1).ToList();
            ViewBag.Roles = roles;
            var x = Request.Form["selectRole"];
            var newUser = new User
            {
                Username = username,
                Password = password   
            };
            var role = _context.Roles.Find(Convert.ToInt32(x));
            newUser.Roles.Add(role);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }
          
        public IActionResult DeleteUser(int Id)
        {
            var user = _context.Users.Find(Id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult ModifyUser1(int Id)
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];


            var user = _context.GetUserByUsernameAndPassword(username, password);

            if (user != null)
            {
                user.Username = username;
                user.Password = password;

                var roles = user.Roles.ToList();
                user.Roles = roles;
                _context.SaveChanges();
            }
            return View("ModifyUser");
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult ModifyUser(int Id)
        {
            User user = _context.Users.Find(Id);
            ViewBag.Username = user.Username;
            ViewBag.Password = user.Password;
            ViewBag.SelectedRole = user.Roles.FirstOrDefault();
            ViewBag.Roles = user.Roles;
            return View(user);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddUser()
        {
            @ViewBag.Roles = _context.Roles;
            return View();
        }
    }
}
