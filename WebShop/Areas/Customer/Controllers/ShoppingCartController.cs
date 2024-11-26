using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Models;
using WebShop.Models.Views;
using WebShop.Repository.IRepository;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<ShoppingCart> cartFromDB = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includProperties: "Product").ToList();

            var shoppingCartVM = new ShoppingCartVM()
            {
                Products = cartFromDB,
                TotalCountOfProducts = CalculateCountOfProducts(cartFromDB),
            };
            shoppingCartVM.OrderHeader.OrderTotal = CalculateCartTotalPrice(cartFromDB); // Do przemyślenia bo coś mi tu nie działa


            return View(shoppingCartVM);
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

    }
}
