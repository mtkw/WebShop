namespace WebShop.Models.Views
{
    public class ShoppingCartVM
    {
        //public IEnumerable<ShoppingCart> Products { get; set; }
        public ShoppingCart Products {get; set;}
        public OrderHeader OrderHeader { get; set; }
        public int TotalCountOfProducts { get; set; }
    }
}
