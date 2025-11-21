using Microsoft.AspNetCore.Mvc;

namespace Backend_MVC_TASK_1.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
