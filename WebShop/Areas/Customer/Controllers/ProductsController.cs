using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebShop.Models;
using WebShop.Models.Views;
using WebShop.Repository.IRepository;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int categoryId)
        {
            IQueryable<Product> products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category").Where(x=>x.ProductCategoryId==categoryId);
            IQueryable<ProductCategory> categories = _unitOfWork.ProductCategory.GetAll();

            var customVM = new ProductsByCategoryVM()
            {
                Products = products,
                ProductCategories = categories,
                SelectedCategory = categoryId

            };
            return View(customVM);
        }

        public IActionResult Details(int id) 
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id,includProperties: "Supplier,Category");

            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string serachString, int categoryOption)
        {
            IQueryable<Product> products;
            if (categoryOption == 0)
            {
                products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category").Where(x => x.Name.ToUpper().Contains(serachString.ToUpper()));
            }
            else
            {
                 products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category").Where(x => x.Name.ToUpper().Contains(serachString.ToUpper()) && x.ProductCategoryId == categoryOption);
            }

            IQueryable<ProductCategory> categories = _unitOfWork.ProductCategory.GetAll();

            var customVM = new ProductsByCategoryVM()
            {
                Products = products,
                ProductCategories = categories,
                SelectedCategory = categoryOption

            };
            return View(customVM);
        }
    }
}
