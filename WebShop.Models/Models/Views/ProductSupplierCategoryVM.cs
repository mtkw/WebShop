namespace WebShop.Models.Models.Views
{
    public class ProductSupplierCategoryVM
    {
        public IQueryable<ProductCategory>? Categories { get; set; }
        public IQueryable<Product>? Products { get; set; }
        public IQueryable<Supplier>? Suppliers { get; set; }

        public string? CurrentSupplierName { get; set; }

        public int? SelectedSupplierId { get; set; }
        public int? SelectedCategory { get; set; }
    }
}
