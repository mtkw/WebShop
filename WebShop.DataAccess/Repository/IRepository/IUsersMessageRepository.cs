using System.Linq.Expressions;
using WebShop.Models.Models;

namespace WebShop.DataAccess.Repository.IRepository
{
    public interface IUsersMessageRepository : IRepository<UsersMessage>
    {
        void Update(UsersMessage message);
    }
}
