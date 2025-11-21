using Microsoft.AspNetCore.Mvc;

namespace Backend_MVC_TASK_1.Areas.Admin.Controllers
{
    public class SlideController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
