using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

        //TestRestApi
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var category = _unitOfWork.ProductCategory.GetAll().Where(x=>x.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductCategory.Remove(category);
            _unitOfWork.Save();
            return NoContent();
        }

        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var categoryToUpdate = _unitOfWork.ProductCategory.GetAll().Where(x=> x.Id == id).First();
            return View(categoryToUpdate);
        }
    }
}
