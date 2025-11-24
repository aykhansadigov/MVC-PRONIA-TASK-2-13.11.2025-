using Backend_MVC_TASK_1.DAL;
using Microsoft.EntityFrameworkCore;

namespace Backend_MVC_TASK_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           ;
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

            });

            var app = builder.Build();

            app.MapControllerRoute(
               "area",
               "{area:exists}/{controller=home}/{action=index}/{id?}"
               );

            app.UseStaticFiles();

            app.MapControllerRoute(
                "default",
                "{controller=home}/{action=index}/{id?}"
                );
            app.Run();
        }
    }
}
