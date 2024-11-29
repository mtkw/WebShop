using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShop.Models;
using WebShop.Models.Views;
using WebShop.Repository.IRepository;
using static NuGet.Packaging.PackagingConstants;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            OrderVM = new OrderVM
            {
                Orders = _unitOfWork.OrderHeader.GetAll(
            u => u.ApplicationUserId == userId
             
        ).ToList()
            };

            return View("~/Areas/Customer/Views/Orders/Index.cshtml", OrderVM);
        }

        public IActionResult Details(int orderId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            var orderDetails = _unitOfWork.OrderDetail.GetAll(
                        u => u.OrderHeaderId == orderId)
                        .Include(od => od.Product) 
                        .ToList();

            if (orderDetails == null || !orderDetails.Any())
            {
                Console.WriteLine("No order product not found for OrderId: " + orderId);
            }

            OrderVM = new OrderVM
            {
                
                OrderDetails = orderDetails

            };
            return View("~/Areas/Customer/Views/Orders/Details.cshtml",OrderVM);
        }
    }
}
