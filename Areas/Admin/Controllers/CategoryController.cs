using Backend_MVC_TASK_1.DAL;
using Backend_MVC_TASK_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_MVC_TASK_1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories=await _context.Categories.Include(c=>c.Products).ToListAsync();
            return View(categories);
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Category category)
        //{
        //    category.CreatedAt = DateTime.Now;
        //    return Json(category);
        //}

    }
}
