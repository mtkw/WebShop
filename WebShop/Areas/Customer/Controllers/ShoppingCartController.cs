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
                TotalPrice = CalculateCartTotalPrice(cartFromDB)
            };


            return View(shoppingCartVM);
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
