using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.Repository.IRepository;
using WebShop.Models.Models;

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
                return Json(new { success = false, message = "Error while deleteing" });
            }
            _unitOfWork.ProductCategory.Remove(category);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product Category Deleted" });
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<ProductCategory> categories = _unitOfWork.ProductCategory.GetAll().ToList();
            return Json(new {data = categories});
        }

        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var categoryToUpdate = _unitOfWork.ProductCategory.GetAll().Where(x=> x.Id == id).First();
            return View(categoryToUpdate);
        }

        [HttpPatch]
        public IActionResult Edit([FromBody] ProductCategory category)
        {
                _unitOfWork.ProductCategory.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return Json(new { success = true, message = "Product Category Updated" });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductCategory category)
        {
            _unitOfWork.ProductCategory.Add(category);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
