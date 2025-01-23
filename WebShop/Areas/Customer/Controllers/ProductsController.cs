using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using System.Security.Claims;
using WebShop.Models;
using WebShop.Models.Views;
using WebShop.Repository.IRepository;
using WebShop.Utility;
using Product = WebShop.Models.Product;

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
            IEnumerable<Product> products;
            if (category.Name == "All Products")
            {
                products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category");
            }
            else
            {
                products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category").Where(x => x.ProductCategoryId == categoryId);
            }
            
            IEnumerable<ProductCategory> categories = _unitOfWork.ProductCategory.GetAll();

            var customVM = new ProductsByCategoryVM()
            {
                Products = products,
                ProductCategories = categories,
                SelectedCategory = categoryId.HasValue ? categoryId.Value : 1

            };
            return View(customVM);
        }
        [Authorize]
        public IActionResult AddToCart(int productId, int? orderId, bool? productDetailsPage)
        {
            //Newe Version
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCart cartFromDB = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId);
            Product productFromDB = _unitOfWork.Product.Get(u => u.Id == productId);

            if (cartFromDB != null)
            {
                if (productFromDB.Quantity == 0)
                {
                    TempData["errorr"] = "Product Sold Out. We will inform you when the product is available again.";
                }
                else
                {
                    //shopping cart exist
                    List<CartItem> cartItems = _unitOfWork.CartItem.GetAll(x=>x.ShoppingCartId == cartFromDB.Id).ToList();
                    var productFromCart = cartItems.Find(x => x.ProductId == productId);

                    if (productFromCart != null)
                    {
                        productFromCart.TotalQuantity += 1;
                        productFromCart.TotalAmmount += (decimal)productFromDB.Price;
                        productFromDB.Quantity -= 1;
                        cartFromDB.Ammount += (decimal)productFromDB.Price;
                        cartFromDB.CartCountItems += 1;
                        _unitOfWork.CartItem.Update(productFromCart);
                        _unitOfWork.ShoppingCart.Update(cartFromDB);
                        _unitOfWork.Product.Update(productFromDB);
                        TempData["success"] = "Cart updated successfully";
                        _unitOfWork.Save(); 
                    }
                    else
                    {
                        CartItem cartItem = new CartItem();
                        cartItem.ProductId = productId;
                        cartItem.Product = productFromDB;
                        productFromDB.Quantity -= 1;
                        cartItem.ShoppingCart = cartFromDB;
                        cartItem.ShoppingCartId = cartFromDB.Id;
                        cartItem.TotalQuantity = 1;
                        cartItem.TotalAmmount = (decimal)productFromDB.Price;
                        _unitOfWork.CartItem.Add(cartItem);

                        
                        cartFromDB.Ammount += (decimal)productFromDB.Price;
                        cartFromDB.CartCountItems += 1;
                        cartFromDB.CartItems.Add(cartItem);

                        _unitOfWork.ShoppingCart.Update(cartFromDB);
                        _unitOfWork.Product.Update(productFromDB);
                        TempData["success"] = "Cart updated successfully";
                        _unitOfWork.Save();

                    }
                }
            }
            else
            {
                if (productFromDB.Quantity == 0)
                {
                    TempData["errorr"] = "Product Sold Out. We will inform you when the product is available again.";
                }
                else
                {
                    ShoppingCart cart = new ShoppingCart();
                    cart.CartItems = new List<CartItem>();
                    cart.CartCountItems = 0;
                    cart.ApplicationUserId = userId;
                    cart.ApplicationUser = _unitOfWork.ApplicationUser.Get(x=>x.Id == userId);
                    _unitOfWork.ShoppingCart.Add(cart);
                    _unitOfWork.Save();


                    CartItem cartItem = new CartItem();
                    cartItem.Product = productFromDB;
                    cartItem.ProductId = productId;
                    cartItem.TotalQuantity = 1;
                    cartItem.TotalAmmount = (decimal)productFromDB.Price * cartItem.TotalQuantity;
                    productFromDB.Quantity -= 1;
                    cartItem.ShoppingCartId = cart.Id;
                    cartItem.ShoppingCart = cart;
                    _unitOfWork.CartItem.Add(cartItem);

                    cart.CartCountItems += 1;
                    cart.Ammount += ((decimal)cartItem.Product.Price * cartItem.TotalQuantity);
                    _unitOfWork.ShoppingCart.Update(cart);

                    TempData["success"] = "Cart updated successfully";
                    _unitOfWork.Save();
                }
            }
            if(orderId != null)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            if(productDetailsPage == true)
            {
                return RedirectToAction("Details", "Products", new { id = productId });
            }

            return RedirectToAction(nameof(Index), new { categoryId = productFromDB.ProductCategoryId });
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

        public IActionResult Summary() 
        { 
            
            return View(); 
        }
    }
}
