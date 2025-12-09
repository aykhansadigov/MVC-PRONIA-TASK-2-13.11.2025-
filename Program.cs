using Backend_MVC_TASK_1.DAL;
using Backend_MVC_TASK_1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Backend_MVC_TASK_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });


            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;

                opt.User.RequireUniqueEmail = false;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

          

            var app = builder.Build();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                "default",
                "{area:exists}/{controller=home}/{action=index}/{id?}"

                );
            app.MapControllerRoute(
                "default",
                "{controller=home}/{action=index}/{id?}"

                );
            app.UseStaticFiles();
            app.Run();
        }
    }
    }

