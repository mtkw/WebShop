using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int? categoryId)
        {
            ProductCategory category = _unitOfWork.ProductCategory.Get(x=>x.Id==(categoryId.HasValue ? categoryId.Value : 1));
            IQueryable<Product> products;
            if (category.Name == "All Products")
            {
                products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category");
            }
            else
            {
                products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category").Where(x => x.ProductCategoryId == categoryId);
            }
            
            IQueryable<ProductCategory> categories = _unitOfWork.ProductCategory.GetAll();

            var customVM = new ProductsByCategoryVM()
            {
                Products = products,
                ProductCategories = categories,
                SelectedCategory = categoryId.HasValue ? categoryId.Value : 1

            };
            return View(customVM);
        }

        [Authorize] //Zebezpieczenie że ta metoda zostanie wykonana tylko dla zalogowanych użytkowników bez znaczenia typu użytkownika
        public IActionResult AddToCart(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCart cartFromDB = _unitOfWork.ShoppingCart.Get(u => u.UserId == userId && u.ProductId == id);

            if (cartFromDB != null)
            {
                //shopping cart exist
                cartFromDB.Count += 1;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
            }
            else
            {
                //add cart record
                ShoppingCart cart = new()
                {
                    Id = _unitOfWork.ShoppingCart.GetAll().Count() + 1,
                    Product = _unitOfWork.Product.Get(u => u.Id == id, includProperties: "Supplier,Category"),
                    Count = 1,
                    ProductId = id,
                    UserId = userId,

                };
                _unitOfWork.ShoppingCart.Add(cart);
            }
            TempData["success"] = "Cart updated successfully";
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
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
        [Authorize(Roles = "admin")]
        public IActionResult DeleteFromMainPage(int id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                TempData["error"] = "Error. The product has not been removed";
                return RedirectToAction("Index");
            }

            var oldImagePath = Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath, "img"), productToBeDeleted.ImgPath);

            //Błędna ścieżka oldImagePath do poprawy
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            TempData["success"] = "Product deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
