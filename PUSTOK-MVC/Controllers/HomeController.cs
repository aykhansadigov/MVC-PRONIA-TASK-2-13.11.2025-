using Microsoft.AspNetCore.Mvc;

namespace LabTask_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
