namespace WebShop.Models.Views
{
    public class ProductSupplierCategoryVM
    {
        public IQueryable<ProductCategory> Categories { get; set; }
        public IQueryable<Product> Products { get; set; }
        public IQueryable<Supplier> Suppliers { get; set; }
    }
}
