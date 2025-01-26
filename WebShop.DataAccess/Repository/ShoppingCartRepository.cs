using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Data;
using WebShop.Models.Models;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            var existingEntity = _context.ShoppingCarts.Local.FirstOrDefault(x => x.Id == shoppingCart.Id);
            if (existingEntity == null)
            {
                _context.ShoppingCarts.Attach(shoppingCart);
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(shoppingCart);
            }
        }
    }
}
