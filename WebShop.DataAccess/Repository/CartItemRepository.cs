using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Data;
using WebShop.Models.Models;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private ApplicationDbContext _context;
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CartItem cartItem)
        {

        }

        
    }
}
