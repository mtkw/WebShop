using WebShop.Data;
using WebShop.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ShoppingCart cart)
        {
            _context.Update(cart);
        }
    }
}
