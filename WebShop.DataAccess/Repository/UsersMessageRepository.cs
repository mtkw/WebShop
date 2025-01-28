using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Data;
using WebShop.Models.Models;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.DataAccess.Repository
{
    public class UsersMessageRepository : Repository<UsersMessage>, IUsersMessageRepository
    {
        private ApplicationDbContext _context;
        public UsersMessageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(UsersMessage usersMessage)
        {
            _context.UsersMessages.Update(usersMessage);
        }
    }
}
