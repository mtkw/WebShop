using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models.Views;
using WebShop.Repository.IRepository;

namespace WebShop.Areas.Admin.Controllers
{
    [Controller]
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public IActionResult Index()
        {

            var orders = _unitOfWork.OrderHeader.GetAll(includProperties: "User").ToList();


            var orderVM = new OrderVM
            {
                Orders = orders
            };


            return View("~/Areas/Admin/Views/Orders/Orders.cshtml", orderVM);
        }

        public IActionResult Details(int orderId)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includProperties: "User");
            if (orderHeader == null)
            {
                return NotFound();
            }

            var orderDetails = _unitOfWork.OrderDetail.GetAll(
                u => u.OrderHeaderId == orderId,
                includProperties: "Product"
            ).ToList();

            var orderVM = new OrderVM
            {
                OrderHeader = orderHeader,
                OrderDetails = orderDetails
            };

            return View("~/Areas/Admin/Views/Orders/OrderDetails.cshtml", orderVM);
        }
    }
}
   