namespace WebShop.Models.Models.Views
{
    public class ProductsByCategoryVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public int SelectedCategory { get; set; }
    }
}
