using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Security.Claims;
using WebShop.Models.Models;
using WebShop.Models.Models.Views;
using WebShop.Repository.IRepository;
using WebShop.Utility;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

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

            ShoppingCartVM = new ShoppingCartVM();
            ShoppingCartVM.Products = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId, includProperties: "CartItems");
            if (ShoppingCartVM.Products is not null) 
            {
                foreach (var cartItem in ShoppingCartVM.Products.CartItems)
                {
                    cartItem.Product = _unitOfWork.Product.Get(p => p.Id == cartItem.ProductId);
                }
                ShoppingCartVM.TotalCountOfProducts = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId).CartCountItems;
                ShoppingCartVM.OrderHeader = new OrderHeader();
            }
            
            return View(ShoppingCartVM);
        }

        public IActionResult IncrementProductInCart(int productId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shopingCartFromDB = _unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId, includProperties: "CartItems");
            var cartItemFromShoppingCart = shopingCartFromDB.CartItems.First(x => x.ProductId == productId);
            var productFromDB = _unitOfWork.Product.Get(x => x.Id == productId);

            if (productFromDB.Quantity != 0)
            {
                cartItemFromShoppingCart.TotalQuantity += 1;
                cartItemFromShoppingCart.TotalAmmount += (decimal)productFromDB.Price;

                productFromDB.Quantity -= 1;

                shopingCartFromDB.CartCountItems += 1;
                shopingCartFromDB.Ammount += (decimal)productFromDB.Price;

                _unitOfWork.CartItem.Update(cartItemFromShoppingCart);
                _unitOfWork.Product.Update(productFromDB);
                _unitOfWork.ShoppingCart.Update(shopingCartFromDB);

                TempData["success"] = "Add Item To Cart";
            }
            else
            {
                TempData["error"] = "Product sold out";
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveProductFromCart(int productId)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shopingCartFromDB = _unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId, includProperties: "CartItems");
            var cartItemFromShoppingCart = shopingCartFromDB.CartItems.First(x => x.ProductId == productId);
            var productFromDB = _unitOfWork.Product.Get(x => x.Id == productId);

            productFromDB.Quantity += cartItemFromShoppingCart.TotalQuantity;
            shopingCartFromDB.CartCountItems -= cartItemFromShoppingCart.TotalQuantity;
            shopingCartFromDB.Ammount -= cartItemFromShoppingCart.TotalAmmount;
            _unitOfWork.Product.Update(productFromDB);
            _unitOfWork.ShoppingCart.Update(shopingCartFromDB);
            _unitOfWork.CartItem.Remove(cartItemFromShoppingCart);

            TempData["success"] = "Item Removed From Cart";
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecrementProductInCart(int productId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shopingCartFromDB = _unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId, includProperties: "CartItems");
            var cartItemFromShoppingCart = shopingCartFromDB.CartItems.First(x => x.ProductId == productId);
            var productFromDB = _unitOfWork.Product.Get(x => x.Id == productId);

            if (cartItemFromShoppingCart.TotalQuantity == 1) 
            {
                _unitOfWork.CartItem.Remove(cartItemFromShoppingCart);
                shopingCartFromDB.CartCountItems -= 1;
                shopingCartFromDB.Ammount -= (decimal)productFromDB.Price;
                productFromDB.Quantity += 1;
                _unitOfWork.Product.Update(productFromDB);
                TempData["success"] = "Item Removed From Cart";
            }
            else
            {
                cartItemFromShoppingCart.TotalQuantity -= 1;
                cartItemFromShoppingCart.TotalAmmount -= (decimal)productFromDB.Price;
                _unitOfWork.CartItem.Update(cartItemFromShoppingCart);

                shopingCartFromDB.CartCountItems -= 1;
                shopingCartFromDB.Ammount -= (decimal)productFromDB.Price;
                _unitOfWork.ShoppingCart.Update(shopingCartFromDB);

                productFromDB.Quantity += 1;
                _unitOfWork.Product.Update(productFromDB);
                TempData["success"] = "One item has been removed from the cart";
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM();
            ShoppingCartVM.Products = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId, includProperties: "CartItems");
            foreach (var cartItem in ShoppingCartVM.Products.CartItems)
            {
                cartItem.Product = _unitOfWork.Product.Get(p => p.Id == cartItem.ProductId);
            }
            ShoppingCartVM.OrderHeader = new OrderHeader();

            ShoppingCartVM.TotalCountOfProducts = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId).CartCountItems;
            ShoppingCartVM.OrderHeader.OrderTotal = (double)OrderTotal(ShoppingCartVM.Products.CartItems);
            ShoppingCartVM.OrderHeader.User = _unitOfWork.ApplicationUser.Get(x=>x.Id == userId);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.User.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.User.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.User.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.User.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.User.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.User.PostalCode;

            return View(ShoppingCartVM);
        }

        private decimal OrderTotal(ICollection<CartItem> cartItems)
        {
            decimal orderTotal = 0;
            foreach(var item in cartItems)
            {
                orderTotal += item.TotalAmmount;
            }
            return orderTotal; 
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.Products = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId, includProperties: "CartItems");
            foreach (var cartItem in ShoppingCartVM.Products.CartItems)
            {
                cartItem.Product = _unitOfWork.Product.Get(p => p.Id == cartItem.ProductId);
            }

            ShoppingCartVM.OrderHeader.OrderDate= System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ShoppingCartVM.OrderHeader.OrderTotal = (double)OrderTotal(ShoppingCartVM.Products.CartItems);

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(x => x.Id == userId);

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.Products.CartItems)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.ProductId = cart.ProductId;
                orderDetail.OrderHeaderId = ShoppingCartVM.OrderHeader.Id;
                orderDetail.Price = cart.Product.Price;
                orderDetail.Count = cart.TotalQuantity;
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (!applicationUser.IsBlocked)
            {
                //it is customr Active Account
                //stripe logic Configure from documentation
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/shoppingcart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + "customer/shoppingcart/index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in ShoppingCartVM.Products.CartItems)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Product.Price * 100), // $20.50 => 2050
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                 Name = item.Product.Name
                            }
                        },
                         Quantity = item.TotalQuantity
                    };
                    options.LineItems.Add(sessionLineItem);
                }


                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(OrderConfirmation), new {id=ShoppingCartVM.OrderHeader.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {

            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u=>u.Id == id, includProperties:"User");
            if(orderHeader.PaymentStatus!= SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            var shoppingCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId, includProperties: "CartItems");
            foreach (var product in shoppingCart)
            {
                _unitOfWork.ShoppingCart.Remove(product);
            }
            _unitOfWork.Save();
            return View(id);
        }

    }
}
