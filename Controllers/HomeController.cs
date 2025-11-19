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
          

            HomeVM homeVM = new HomeVM()
            {
                Slides = _context.Slides
                .OrderBy(s => s.Order)
                .Take(2)
                .ToList(),

                Products = _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimaryImage != null))
                .ToList()

            };


           

            return View(homeVM);
        }
    }
}
