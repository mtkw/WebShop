using WebShop.Data;
using WebShop.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private ApplicationDbContext _context;
        public SupplierRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Supplier supplier)
        {
            throw new NotImplementedException();
        }
    }
}
