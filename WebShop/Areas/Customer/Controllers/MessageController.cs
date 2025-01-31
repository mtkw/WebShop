using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MessageController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MessageController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]   
        public IActionResult GetAllMessageForUser(string userId)
        {
            var messages = _unitOfWork.UsersMessage.GetAll(u => u.UserId == userId).ToList();

            return Json(messages);
        }
    }
}
