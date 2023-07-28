using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileStore.Data;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    public class SalesPersonController : Controller
    {
        private readonly MobileStoreContext _context;

        public SalesPersonController(MobileStoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<SaleRecord> salesRecords = _context.SaleRecords.ToList();
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
            return View();
        }

        [HttpPost]
        [ActionName("ModifyRecords1")]

        public IActionResult ModifyRecords1(int SaleId)
        {
            return View("ModifyRecord");
        }

    }
}
