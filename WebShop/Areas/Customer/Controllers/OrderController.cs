using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Models.Models;
using WebShop.Repository.IRepository;
using WebShop.Utility;

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
            orders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId).ToList();
            orderViewModel.orderHeader = orders;
            return View(orderViewModel);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<OrderHeader> orderList = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId).ToList();
            return Json(new { data = orderList });
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

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
        [HttpPatch]
        public IActionResult Cancel(int id) //Temat do przemyślenia po usunięciu zamówienia powinny wracać produkty do magazynu, Przemyśleć temat opcji ponownego złożenia zamówienia
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orderToCanceled = _unitOfWork.OrderHeader.Get(u => (u.ApplicationUserId == userId && u.Id == id));

            if (orderToCanceled == null)
            {
                
                return Json(new { success = false, message = "Problem with order cancellation" });
            }
            if (!string.IsNullOrEmpty(orderToCanceled.TrackingNumber))
            {
                
                return Json(new { success = false, message = "You cannot cancel the order because the order has already been shipped." });
            }
            if(orderToCanceled.OrderStatus == SD.StatusCancelled)
            {
                return Json(new { success = false, message = "Order already canceled" });
            }

            orderToCanceled.OrderStatus = SD.StatusCancelled;
            _unitOfWork.OrderHeader.Update(orderToCanceled);
            _unitOfWork.Save();

            
            return Json(new { success = true, message = "Order Canceled" });
        }

        public IActionResult CancelFromDetailsPage(int id)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orderToCanceled = _unitOfWork.OrderHeader.Get(u => (u.ApplicationUserId == userId && u.Id == id));

            if (orderToCanceled == null)
            {
                TempData["error"] = "Problem with order cancellation";
                return RedirectToAction(nameof(Details));
            }
            if (!string.IsNullOrEmpty(orderToCanceled.TrackingNumber))
            {
                TempData["error"] = "You cannot cancel the order because the order has already been shipped.";
                return RedirectToAction(nameof(Details), new { id = id});
            }
            if (orderToCanceled.OrderStatus == SD.StatusCancelled)
            {
                TempData["error"] = "Order already canceled";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            orderToCanceled.OrderStatus = SD.StatusCancelled;
            _unitOfWork.OrderHeader.Update(orderToCanceled);
            _unitOfWork.Save();

            TempData["success"] = "Order Canceled";
            return RedirectToAction(nameof(Details), new {id = id });
        }
    }
}
