using Backend_MVC_TASK_1.DAL;
using Backend_MVC_TASK_1.Models;
using Backend_MVC_TASK_1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_MVC_TASK_1.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        [Route("shop")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }

            Product? product = _context.Products
                .Include(p=>p.ProductImages.OrderByDescending(pi=>pi.IsPrimaryImage))
                .Include(p=>p.Category)
                .FirstOrDefault(p => p.Id == id);
            if(product is null)
            {
                return NotFound();
            }

            List<Product> relatedProducts = _context.Products
                .Where(p=>p.CategoryId==product.CategoryId && p.Id!=id)
                .Include(p=>p.ProductImages.Where(pi=>pi.IsPrimaryImage!= null))
                .ToList();

            DetailsVM detailsVM = new()
            {
                Product = product,
                RelatedProducts = relatedProducts,
            };

            return View(detailsVM);
        }

    }
}
