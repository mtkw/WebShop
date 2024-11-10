namespace WebShop.Models.Views
{
    public class ProductsByCategoryVM
    {
        public IQueryable<Product> Products { get; set; }
        public IQueryable<ProductCategory> ProductCategories { get; set; }
        public int SelectedCategory { get; set; }
    }
}
