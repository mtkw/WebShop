using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Repository;
using WebShop.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using WebShop.Middleware;
using WebShop.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

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
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnecion")).
                EnableSensitiveDataLogging();
            }); 

            //Konfiguracja p�atno�ci Stripe
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


            //builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>(); // Domy�lna konfiguracja tylko do rejesracji i logowania u�ytkownik�w bez podzia�u na role

            // Konfiguracja rozszerzona o mo�liwo�� wporwadzenia r�l dla poszczeg�lnych u�ytkownik�w np: Admin i User
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //Konfiguracja �cie�ek do takich podstron jak Login/Logout i AccessDenied. Bez tego nie przejdzie na te podstrony. Domy�lna �cie�ka jest b��dna
            //Bardzo wa�ne ta konfiguracja musi by� umieszczona dopiero po linijce konfiguruj�cej Idenity w tym wypadku po linijce 28
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

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
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ProductCategoryMiddleware>(); // konfigfuracja Klasy Middleware // AUTHENTICATION AND AUTHORIZATION BEFORE MIDDLEWARE CLASS
            app.MapRazorPages(); // Druga cz�� konfiguracji Razor Pages
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
