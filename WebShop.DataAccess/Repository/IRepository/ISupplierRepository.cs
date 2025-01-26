using WebShop.Models.Models;

namespace WebShop.DataAccess.Repository.IRepository
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        void Update(Supplier supplier);
    }
}
