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
        public async Task<IActionResult> Index()
        {



            HomeVM homeVM = new HomeVM()
            {
                Slides = await _context
                .Slides
                .OrderBy(s => s.Order)
                .Take(2)
                .ToListAsync(),

                Products = await _context
                .Products
                .OrderBy(p => p.CreatedAt)
                .Take(20)
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null))
                .ToListAsync()

            };
            return View(homeVM);
        }
    }
}   
