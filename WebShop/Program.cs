using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Repository;
using WebShop.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using WebShop.Middleware;
using WebShop.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Add DatabaseConfiguration
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnecion")));

            //builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>(); // Domy�lna konfiguracja tylko do rejesracji i logowania u�ytkownik�w bez podzia�u na role

            // Konfiguracja rozszerzona o mo�liwo�� wporwadzenia r�l dla poszczeg�lnych u�ytkownik�w np: Admin i User
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); 

            builder.Services.AddRazorPages(); // Konfiguracja Razor pages bez tego �adna strona stworzona dla Identity nie b�dzie dzia�a� 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<ProductCategoryMiddleware>(); // konfigfuracja Klasy Middleware
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages(); // Druga cz�� konfiguracji Razor Pages
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
