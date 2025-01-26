using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Models.Models;
using WebShop.DataAccess.Repository.IRepository;
using WebShop.Utility;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Controller]
    [Authorize(Roles = "admin")]
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
            OrderViewModel orderViewModel = new OrderViewModel();
            List<OrderHeader> orders = new List<OrderHeader>();
            orders = _unitOfWork.OrderHeader.GetAll(includProperties: "User").ToList();
            orderViewModel.orderHeader = orders;

            return View(orderViewModel);
        }

        public IActionResult Details(int id, string userId)
        {
            OrderViewModel orderDetailsViewModel = new()
            {
                orderHeader = _unitOfWork.OrderHeader.GetAll(u => (u.ApplicationUserId == userId && u.Id == id)).ToList(),
                orderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == id, includProperties: "Product").ToList()

            };

            if (orderDetailsViewModel.orderDetail == null || orderDetailsViewModel.orderHeader == null)
            {
                return NotFound();
            }

            return View(orderDetailsViewModel);
        }

        public IActionResult Cancel(int id, string userId) 
        {
            var orderToCanceled = _unitOfWork.OrderHeader.Get(u => (u.ApplicationUserId == userId && u.Id == id));

            if (orderToCanceled == null)
            {
                TempData["error"] = "Problem with order cancellation";
                return RedirectToAction(nameof(Details));
            }
            if (orderToCanceled.OrderStatus == SD.StatusShipped)
            {
                TempData["error"] = "You cannot cancel the order because the order has already been shipped.";
                return RedirectToAction(nameof(Details), new { id = id, userId = userId });
            }
            if (orderToCanceled.OrderStatus == SD.StatusCancelled)
            {
                TempData["error"] = "Order already canceled";
                return RedirectToAction(nameof(Details), new { id = id, userId = userId });
            }

            orderToCanceled.OrderStatus = SD.StatusCancelledByAdmin;
            _unitOfWork.OrderHeader.Update(orderToCanceled);
            _unitOfWork.Save();

            TempData["success"] = "Order Canceled";
            return RedirectToAction(nameof(Details), new { id = id, userId = userId });
        }

        public IActionResult CancelFromOrderList(int id, string userId) {
            var orderToCanceled = _unitOfWork.OrderHeader.Get(u => (u.ApplicationUserId == userId && u.Id == id));

            if (orderToCanceled == null)
            {

                return Json(new { success = false, message = "Problem with order cancellation" });
            }
            if (orderToCanceled.OrderStatus == SD.StatusShipped)
            {

                return Json(new { success = false, message = "You cannot cancel the order because the order has already been shipped." });
            }
            if (orderToCanceled.OrderStatus == SD.StatusCancelled)
            {
                return Json(new { success = false, message = "Order already canceled" });
            }

            orderToCanceled.OrderStatus = SD.StatusCancelledByAdmin;
            _unitOfWork.OrderHeader.Update(orderToCanceled);
            _unitOfWork.Save();


            return Json(new { success = true, message = "Order Canceled" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> orderList = _unitOfWork.OrderHeader.GetAll(includProperties: "User").ToList();
            return Json(new { data = orderList });
        }
    }
}
