using Backend_MVC_TASK_1.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_MVC_TASK_1.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option):base (option)
        {  }
        public DbSet<Slide> Slides { get; set; }
    }
}
