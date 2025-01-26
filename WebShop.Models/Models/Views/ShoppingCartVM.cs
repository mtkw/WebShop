namespace WebShop.Models.Models.Views
{
    public class ShoppingCartVM
    {
        public ShoppingCart Products {get; set;}
        public OrderHeader OrderHeader { get; set; }
        public int TotalCountOfProducts { get; set; }
    }
}
