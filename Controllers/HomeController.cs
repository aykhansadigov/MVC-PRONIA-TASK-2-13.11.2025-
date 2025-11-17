using Backend_MVC_TASK_1.DAL;
using Backend_MVC_TASK_1.Models;
using Backend_MVC_TASK_1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_MVC_TASK_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Slide> slides = _context.Slides.OrderBy(s => s.Order).Take(2).ToList();
            List<Product>products =_context.Products.Include(p=>p.ProductImages).ToList();

            HomeVM homeVM = new HomeVM()
            {
                Slides = slides,
                Products =products,

            };


           

            return View(homeVM);
        }
    }
}
