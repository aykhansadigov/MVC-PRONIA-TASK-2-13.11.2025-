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
            List<Category> categories = await _context.Categories.Include(c => c.Products).ToListAsync();
            return View(categories);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result =await _context.Categories.AnyAsync(c=>c.CategoryName == category.CategoryName);
            if (result)
            {
                ModelState.AddModelError(nameof(category.CategoryName), $"{category.CategoryName}category already exist");
                return View(category);
            }
            category.CreatedAt = DateTime.Now;
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(existed == null)
            {
                return NotFound();
            }
            return View(existed);
        }
       
    }
}
