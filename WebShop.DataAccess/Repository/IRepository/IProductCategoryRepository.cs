using WebShop.Models.Models;

namespace WebShop.DataAccess.Repository.IRepository
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<List<ProductCategory>> GetAllAsync();
        void Update(ProductCategory productCategory);
    }
}
