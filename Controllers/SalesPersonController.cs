using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.Data;
using MobileStore.Models;
using System.Security.Claims;

namespace MobileStore.Controllers
{
    public class SalesPersonController : Controller
    {
        private readonly MobileStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalesPersonController(MobileStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor=httpContextAccessor;
        }
        public IActionResult Index()
        {
            List<SaleRecord> salesRecords = _context.SaleRecords.ToList();
            foreach (var saleRecord in salesRecords)
            {
                var user = _context.Users
                       .Include(u => u.SaleRecords) // Include the Roles navigation property
                       .FirstOrDefault(u => u.UserId == saleRecord.SaleId);
                if (user != null)
                {
                    saleRecord.User = user;
                }
            }
            return View(salesRecords);
        }

        [HttpGet]
        [ActionName("AddRecord")]

        public IActionResult AddRecord()
        {
            return View();
        }

        [HttpPost]
        [ActionName("AddRecords1")]

        public IActionResult AddRecords1()
        {
            ClaimsIdentity userClaimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string user;
            User user1 = null;
            if (userClaimsIdentity != null)
            {
                // Iterate through claims to find the username claim
                foreach (var claim in userClaimsIdentity.Claims)
                {
                    if (claim.Type.Contains("nameidentifier"))
                    {
                        user = claim.Value;
                        user1 = _context.Users.FirstOrDefault(u => u.Username == user);
                        // Use the username as needed...
                        break;
                    }
                }
            }

            

            SaleRecord record = new SaleRecord()
            {
                MobileModel = Request.Form["mobileModel"],
                MobileBrand = Request.Form["mobileBrand"],
                QuantitySold = int.Parse(Request.Form["quantitySold"]),
                SalesPrice = float.Parse(Request.Form["quantitySold"]),
                Discount = float.Parse(Request.Form["discount"]),
                SaleDate = DateTime.Now,
                User= user1
            };
            

            try
            {
                _context.SaleRecords.Add(record);
                _context.SaveChanges();
                return RedirectToAction("Index","SalesPerson");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View("AddRecord");
        }
        public IActionResult DeleteRecord(int SaleId)
        {
            var record = _context.SaleRecords.Find(SaleId);
            if (record != null)
            {
                _context.SaleRecords.Remove(record);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "SalesPerson");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ModifyRecord(int SaleId)
        {
            SaleRecord record = _context.SaleRecords.Find(SaleId);
            //record.User = _context.Users.Find(record.UserId);
            return View(record);
        }

        [HttpPost]
        [ActionName("ModifyRecords1")]

        public IActionResult ModifyRecords1(SaleRecord modifiedRecord)
        {
            //User user = _context.Users.Find(modifiedRecord.UserId);
            //modifiedRecord.User = user;
            try
            {
                _context.SaleRecords.Update(modifiedRecord);
                _context.SaveChanges();
                return RedirectToAction("Index", "SalesPerson");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
