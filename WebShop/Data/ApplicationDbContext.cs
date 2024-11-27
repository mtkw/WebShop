using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;


namespace WebShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        protected readonly IConfiguration Configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-MVI6AM12;Database=WebShop;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { Id = 1, Name = "Tablets" },
                new ProductCategory { Id = 2, Name = "Smartphone" },
                new ProductCategory { Id = 3, Name = "Laptops" },
                new ProductCategory { Id = 4, Name = "Others" },
                new ProductCategory { Id = 5, Name = "Headphones"},
                new ProductCategory { Id = 6, Name = "Pointing devices"},
                new ProductCategory { Id = 7, Name = "Printers and Scanner"},
                new ProductCategory { Id = 8, Name = "Routers and Modems"},
                new ProductCategory { Id = 9, Name = "Desktop Computers" },
                new ProductCategory { Id = 10, Name = "Consoles" },
                new ProductCategory { Id = 11, Name = "SmartWatches" }, 
                new ProductCategory { Id = 12, Name = "All Products" }
                
                
                
    );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Amazon" },
                new Supplier { Id = 2, Name = "Lenovo" },
                new Supplier { Id = 3, Name = "Asus" },
                new Supplier { Id = 4, Name = "Apple" },
                new Supplier { Id = 5, Name = "Samsung" },
                new Supplier { Id = 6, Name = "Xiaomi" }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Amazon Fire", Price = 49.9, Currency = "USD", Description = "Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.", ImgPath = "Amazon Fire", ProductCategoryId = 1, SupplierId = 1 },
                new Product { Id = 2, Name = "Lenovo IdeaPad Miix 700", Price = 479.9, Currency = "USD", Description = "Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.", ImgPath = "Lenovo IdeaPad Miix 700", ProductCategoryId = 3, SupplierId = 2 },
              
                new Product { Id = 3, Name = "Amazon Fire HD 8", Price = 89.9, Currency = "USD", Description = "Amazon's latest Fire HD 8 tablet is a great value for media consumption.", ImgPath = "Amazon Fire HD 8", ProductCategoryId = 1, SupplierId = 1 },
                new Product { Id = 4, Name = "iPhone 12", Price = 799.9, Currency = "USD", Description = "The latest iPhone with improved performance, better camera, and 5G capabilities.", ImgPath = "iPhone 12", ProductCategoryId = 2, SupplierId = 4 },
                new Product { Id = 5, Name = "Lenovo Legion", Price = 500, Currency = "USD", Description = "The lates Lenovo Phone perfect for gaming purposes.", ImgPath = "Lenovo Legion", ProductCategoryId = 2, SupplierId = 2 },
                new Product { Id = 6, Name = "Lenovo K8", Price = 600, Currency = "USD", Description = "The lates Lenovo Phone perfect for gaming purposes.", ImgPath = "Lenovo K8", ProductCategoryId = 2, SupplierId = 2 }, 
                new Product { Id = 7, Name = "Lenovo M10", Price = 179.9, Currency = "USD", Description = "Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.", ImgPath = "Lenovo M10", ProductCategoryId = 1, SupplierId = 2 }
                );
        }
    }
}
