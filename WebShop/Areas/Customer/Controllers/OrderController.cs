using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            OrderViewModel orderViewModel = new OrderViewModel();
            List<OrderHeader> orders = new List<OrderHeader>();
            orders = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId == userId).ToList();
            orderViewModel.OrdersHeader = orders;
            return View(orderViewModel);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<OrderHeader> orderList = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId == userId).ToList();
            return Json(new { data = orderList });
        }
    }
}
