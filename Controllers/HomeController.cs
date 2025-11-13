using Backend_MVC_TASK_1.DAL;
using Backend_MVC_TASK_1.Models;
using Backend_MVC_TASK_1.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

            HomeVM homeVM = new HomeVM()
            {
                Slides = slides

            };


            //List<Slide> slides = new List<Slide>()
            //{
            //    new Slide
            //    {
            //        Title = "Başlıq 1",
            //        SubTitle = "Köməkçi Başlıq 1",
            //        Description = "Güllərdən Qalmadı",
            //        CreatedAt = DateTime.Now,
            //        Image ="1-2-524x617.png",
            //        IsDeleted = false,
            //        Order = 3

            //    },

            //    new Slide
            //    {
            //        Title = "Başlıq 2",
            //        SubTitle = "Köməkçi Başlıq 2",
            //        Description = "Xırdalana Manatdan",
            //        CreatedAt = DateTime.Now,
            //        Image ="1-1-524x617.png",
            //        IsDeleted = false,
            //        Order = 2
            //    },

            //    new Slide
            //    {
            //        Title = "Başlıq 3",
            //        SubTitle = "Köməkçi Başlıq 3",
            //        Description = "Ən Gözəl Endirimlər",
            //        CreatedAt = DateTime.Now,
            //        Image ="1-1-270x300.webp",
            //        IsDeleted = false,
            //        Order = 1,
            //    }
            //};

            //_context.Slides.AddRange(slides);
            //_context.SaveChanges();


            return View(homeVM);
        }
    }
}
