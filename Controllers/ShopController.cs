using Microsoft.AspNetCore.Mvc;

namespace Backend_MVC_TASK_1.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

    }
}
