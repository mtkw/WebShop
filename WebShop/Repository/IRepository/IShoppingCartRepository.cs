using WebShop.Models;

namespace WebShop.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart cart);
    }
}
