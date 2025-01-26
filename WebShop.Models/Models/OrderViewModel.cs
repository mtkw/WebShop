namespace WebShop.Models.Models
{
    public class OrderViewModel
    {
        public List<OrderHeader> orderHeader { get; set; } = new List<OrderHeader>();
        public List<OrderDetail> orderDetail { get; set; } = new List<OrderDetail>();
    }
}
