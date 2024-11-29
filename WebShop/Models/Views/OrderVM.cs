namespace WebShop.Models.Views
{
    public class OrderVM
    {
        public IEnumerable<OrderHeader> Orders { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
