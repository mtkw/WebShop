using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Data;
using WebShop.Models.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }
    }
}
