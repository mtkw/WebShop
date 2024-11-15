using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Areas.Admin.Controllers
{
    [Controller]
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll().ToList();
            return View(users);
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.ApplicationUser.Remove(user);
            _unitOfWork.Save();
            return Json(new { success = true, message = "User Deleted" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll().ToList();
            return Json(new { data = users });
        }


        




        [HttpPatch]
        public IActionResult BlockUser(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            Console.WriteLine($"Received ID: {id}");

            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            user.IsBlocked = true;
            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.Save();

            return Json(new { success = true, message = "User blocked successfully" });
        }

        [HttpPatch]
        public IActionResult UnblockUser(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            user.IsBlocked = false;
            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.Save();

            return Json(new { success = true, message = "User unblocked successfully" });
        }


    }
}

