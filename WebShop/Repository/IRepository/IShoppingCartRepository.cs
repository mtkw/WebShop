using System.Linq.Expressions;
using WebShop.Models.Models;

namespace WebShop.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}
