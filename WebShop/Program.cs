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

            //Konfiguracja p³atnoœci Stripe
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


            //builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>(); // Domyœlna konfiguracja tylko do rejesracji i logowania u¿ytkowników bez podzia³u na role

            // Konfiguracja rozszerzona o mo¿liwoœæ wporwadzenia ról dla poszczególnych u¿ytkowników np: Admin i User
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //Konfiguracja œcie¿ek do takich podstron jak Login/Logout i AccessDenied. Bez tego nie przejdzie na te podstrony. Domyœlna œcie¿ka jest b³êdna
            //Bardzo wa¿ne ta konfiguracja musi byæ umieszczona dopiero po linijce konfiguruj¹cej Idenity w tym wypadku po linijce 28
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            builder.Services.AddRazorPages(); // Konfiguracja Razor pages bez tego ¿adna strona stworzona dla Identity nie bêdzie dzia³aæ 
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
            app.MapRazorPages(); // Druga czêœæ konfiguracji Razor Pages
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
