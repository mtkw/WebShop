using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            u => u.ApplicationUserId == userId,
            includProperties: "OrderDetails,OrderDetails.Product" 
        ).ToList()
            };

            return View("~/Areas/Customer/Views/Orders/Index.cshtml", OrderVM);
        }
    }
}
