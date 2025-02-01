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
        [HttpPost]
        public async Task<IActionResult> ReadMessage(string id)
        {

            if (!Guid.TryParse(id, out Guid messageId))
            {
                return BadRequest("Invalid ID format.");
            }

            var message = await _unitOfWork.UsersMessage.GetAsync(x => x.Id == messageId);

            if (message == null)
            {
                return NotFound("Message not found.");
            }
            if(message.IsRead == true)
            {
                message.IsRead = false;
            }
            else
            {
                message.IsRead = true;
            }
            await _unitOfWork.SaveAsync();

            return Ok();
        }
    }
}
