using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Data;
using WebShop.Models.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Repository
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
