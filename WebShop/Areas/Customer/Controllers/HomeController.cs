using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShop.Models;
using WebShop.Models.Views;
using WebShop.Repository.IRepository;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IQueryable<Product> products;
            products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category");
            IQueryable<ProductCategory> categories;
            categories = _unitOfWork.ProductCategory.GetAll();
            IQueryable<Supplier> suppliers;
            suppliers = _unitOfWork.Supplier.GetAll();

            var customVM = new ProductSupplierCategoryVM
            {
                Categories = categories,
                Products = products,
                Suppliers = suppliers
            };

            return View(customVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
