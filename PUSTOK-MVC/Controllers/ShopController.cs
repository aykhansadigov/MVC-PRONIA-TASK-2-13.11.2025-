using Microsoft.AspNetCore.Mvc;

namespace LabTask_MVC.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Whistlist()
        {
            return View();
        }
    }
}
