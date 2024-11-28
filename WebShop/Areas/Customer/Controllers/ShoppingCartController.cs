using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Models;
using WebShop.Models.Views;
using WebShop.Repository.IRepository;
using WebShop.Utility;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            /*            ShoppingCartVM = new()
                        {
                            Products = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includProperties: "Product"),
                            TotalCountOfProducts = CalculateCountOfProducts(ShoppingCartVM.Products.ToList()),
                            OrderHeader = new()
                        };*/
            ShoppingCartVM = new ShoppingCartVM();
            ShoppingCartVM.Products = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == userId, includProperties: "Product");
            ShoppingCartVM.TotalCountOfProducts = CalculateCountOfProducts(ShoppingCartVM.Products.ToList());
            ShoppingCartVM.OrderHeader = new OrderHeader();

            foreach (var cart in ShoppingCartVM.Products)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Product.Price);
            }


            return View(ShoppingCartVM);
        }

        public IActionResult IncrementProductInCart(int cartId)
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(u=>u.Id == cartId);
            var productFromCart = _unitOfWork.Product.Get(u=>u.Id == cartFromDB.ProductId);

            if(productFromCart.Quantity != 0)
            {
                cartFromDB.Count++;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
                productFromCart.Quantity -= 1;
                _unitOfWork.Product.Update(productFromCart);
                TempData["success"] = "Add Item To Cart";
            }
            else
            {
                TempData["error"] = "Product sold out";
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveProductFromCart(int cartId)
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            var productFromCart = _unitOfWork.Product.Get(u => u.Id == cartFromDB.ProductId);

            productFromCart.Quantity += cartFromDB.Count;
            _unitOfWork.ShoppingCart.Remove(cartFromDB);
            _unitOfWork.Product.Update(productFromCart);
            TempData["success"] = "Item Removed From Cart";
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecrementProductInCart(int cartId)
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            var productFromCart = _unitOfWork.Product.Get(u => u.Id == cartFromDB.ProductId);

            if (cartFromDB.Count == 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDB);
                productFromCart.Quantity += 1;
                _unitOfWork.Product.Update(productFromCart);
                TempData["success"] = "Item Removed From Cart";
            }
            else
            {
                cartFromDB.Count--;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
                productFromCart.Quantity += 1;
                _unitOfWork.Product.Update(productFromCart);
                TempData["success"] = "One item has been removed from the cart";
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private int CalculateCountOfProducts(List<ShoppingCart> cart) 
        { 
            var count = 0;
            foreach (var cartItem in cart) 
            {
                count += cartItem.Count;
            }
            return count;
        }

        private double CalculateCartTotalPrice(List<ShoppingCart> cart) 
        {
            double totalPrice = 0;
            foreach (var cartItem in cart) 
            {
                totalPrice += (cartItem.Count * cartItem.Product.Price);
            }
            return totalPrice;
        }
        
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


/*            ShoppingCartVM = new() 
            {
                Products = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includProperties: "Product"),
                TotalCountOfProducts = CalculateCountOfProducts(ShoppingCartVM.Products.ToList()),
                OrderHeader = new()
            };*/

            ShoppingCartVM = new ShoppingCartVM();
            ShoppingCartVM.Products = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includProperties: "Product");
            ShoppingCartVM.TotalCountOfProducts = CalculateCountOfProducts(ShoppingCartVM.Products.ToList());
            ShoppingCartVM.OrderHeader = new OrderHeader();

            ShoppingCartVM.OrderHeader.OrderTotal = CalculateCartTotalPrice(ShoppingCartVM.Products.ToList());
            ShoppingCartVM.OrderHeader.User = _unitOfWork.ApplicationUser.Get(x=>x.Id == userId);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.User.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.User.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.User.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.User.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.User.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.User.PostalCode;

            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.Products = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate= System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ShoppingCartVM.OrderHeader.OrderTotal = CalculateCartTotalPrice(ShoppingCartVM.Products.ToList());

            ShoppingCartVM.OrderHeader.User = _unitOfWork.ApplicationUser.Get(x => x.Id == userId);

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.Products)
            {
                /*                OrderDetail orderDetail = new()
                                {
                                   ProductId = cart.ProductId, 
                                   OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                                   Price = cart.Product.Price,
                                   Count = cart.Count
                                };*/
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.ProductId = cart.ProductId;
                orderDetail.OrderHeaderId = ShoppingCartVM.OrderHeader.Id;
                orderDetail.Price = cart.Product.Price;
                orderDetail.Count = cart.Count;
                _unitOfWork.OrderDetail.Add(orderDetail);
                /*                _unitOfWork.Save();*/
            }

            foreach (var product in ShoppingCartVM.Products)
            {
                _unitOfWork.ShoppingCart.Remove(product);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(OrderConfirmation), new {id=ShoppingCartVM.OrderHeader.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

    }
}
