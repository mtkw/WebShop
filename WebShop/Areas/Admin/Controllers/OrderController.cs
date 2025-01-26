using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Models.Models;
using WebShop.Repository.IRepository;

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

        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> orderList = _unitOfWork.OrderHeader.GetAll(includProperties: "User").ToList();
            return Json(new { data = orderList });
        }
    }
}
