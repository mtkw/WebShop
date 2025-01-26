using WebShop.Models.Models;

namespace WebShop.Repository.IRepository
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        void Update(Supplier supplier);
    }
}
