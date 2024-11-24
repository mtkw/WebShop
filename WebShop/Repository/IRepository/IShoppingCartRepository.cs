using WebShop.Models;

namespace WebShop.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task<List<ShoppingCart>> GetAllAsync();
        void Update(ShoppingCart shoppingCart);
    }
}
