using Backend_MVC_TASK_1.DAL;
using Backend_MVC_TASK_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_MVC_TASK_1.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        public SlideController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides= await _context.Slides/*.AsNoTracking()*/.ToListAsync();

            return View(slides);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {
            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError(nameof(Slide.Photo), "File type is incorrect");
                return View();
            }
            if (slide.Photo.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError(nameof(Slide.Photo), "File size is incorrect");
                return View();
            }
            if (!ModelState.IsValid) 
            { 
                return View();
            }

            bool result= await _context.Slides.AnyAsync(s=>s.Order == slide.Order);
            if (result)
            {
                ModelState.AddModelError(nameof(Slide.Order), $"{slide.Order} order already exist");
                return View();
            }


            string path = "C:\\Users\\ASUS\\OneDrive\\Desktop\\MVC-TASKS\\Backend-MVC-TASK\\wwwroot\\assets\\images\\website-images\\" + slide.Photo.FileName;
                FileStream stream= new FileStream(path, FileMode.Create);
           await slide.Photo.CopyToAsync(stream);
            slide.Image = slide.Photo.FileName;




            slide.CreatedAt = DateTime.Now;
            _context.Add(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(id is null || id< 1)
            {
                return BadRequest();
            }
            return View();
            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
                if (existed is null)
            {
                return NotFound();
            }
                
               return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Slide slide)
        {
            if (!ModelState.IsValid)
            {
                return View(slide);
            }
            bool result = await _context.Slides.AnyAsync(s => s.Order == slide.Order && s.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(Slide.Order), $"{slide.Order} order already exist");
                return View(slide);
            }

            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            existed.Title = slide.Title;
            existed.SubTitle = slide.SubTitle;
            existed.Order = slide.Order;
            existed.Description = slide.Description;
            existed.Image = slide.Image;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
