using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Areas.Admin.Controllers
{
    [Controller]
    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductCategory> categories = _unitOfWork.ProductCategory.GetAll().ToList();

            return View(categories);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            if (id == 0) 
            { 
                return NotFound();
            }

            var categoryToDelete = _unitOfWork.ProductCategory.GetAll().Where(x=>x.Id == id).First();
            _unitOfWork.ProductCategory.Remove(categoryToDelete);
            _unitOfWork.Save();

            return Json(new {data = "Delete Successfuly"});
        }

        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var categoryToUpdate = _unitOfWork.ProductCategory.GetAll().Where(x=> x.Id == id).First();
            return View(categoryToUpdate);
        }
    }
}
