using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebShop.DataAccess.Repository.IRepository;
using WebShop.Models.Models;
using WebShop.Models.Models.Views;

namespace WebShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class SupplierController : Controller
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierController(ILogger<SupplierController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }






        //public IActionResult Index(int? supplierId, int? categoryId)
        //{
        //    ProductCategory categories1 = _unitOfWork.ProductCategory.
        //        GetFirstOrDefault(x => x.Id == (categoryId.HasValue ? categoryId.Value : 12));

        //    IQueryable < Product > products;

        //    if( categories1.Name == "All Products")
        //    {
        //         products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category");
        //    }
        //    else
        //    {
        //        products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category").
        //            Where(x=>x.ProductCategoryId == categoryId);
        //    }

        //    var categories = _unitOfWork.ProductCategory.GetAll();



        //    if (supplierId.HasValue)
        //    {
        //        products = products.Where(p => p.SupplierId == supplierId.Value);
        //    }





        //    var suppliers = _unitOfWork.Supplier.GetAll();

        //    var viewModel = new ProductSupplierCategoryVM
        //    {
        //        Products = products,
        //        Categories = categories,
        //        Suppliers = suppliers,
        //        SelectedCategory = categoryId,
        //        SelectedSupplierId = supplierId
        //    };

        //    return View("~/Areas/Customer/Views/Suppliers/Index.cshtml", viewModel);

        //}

        public IActionResult Index(int? supplierId, int? categoryId)
        {
            
            int defaultCategoryId = categoryId ?? 12;

            
            IQueryable<Product> products = _unitOfWork.Product.GetAll(includProperties: "Supplier,Category");

            
            if (defaultCategoryId != 12)
            {
                products = products.Where(x => x.ProductCategoryId == defaultCategoryId);
            }

           
            if (supplierId.HasValue)
            {
                products = products.Where(p => p.SupplierId == supplierId.Value);
            }

           
            var categories = _unitOfWork.ProductCategory.GetAll();
            var suppliers = _unitOfWork.Supplier.GetAll();

            List<Product> productList = products.ToList(); 

            
            var viewModel = new ProductSupplierCategoryVM
            {
                Products = products,  
                Categories = categories,
                Suppliers = suppliers,
                SelectedCategory = categoryId,
                SelectedSupplierId = supplierId
            };

            
            return View("~/Areas/Customer/Views/Suppliers/Index.cshtml", viewModel);
        }







    }
}

        
       

      
    


