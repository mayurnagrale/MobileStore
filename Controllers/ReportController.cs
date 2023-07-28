using Microsoft.AspNetCore.Mvc;

namespace MobileStore.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
