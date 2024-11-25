namespace WebShop.Models.Views
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> Products { get; set; }
        public int TotalCountOfProducts { get; set; }
        public double TotalPrice { get; set; }
    }
}
