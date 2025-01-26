using System.Linq.Expressions;
using WebShop.Models.Models;

namespace WebShop.Repository.IRepository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        void Update(CartItem cartItem);
    }
}
