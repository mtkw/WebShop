using WebShop.Models;

namespace WebShop.Repository.IRepository
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<List<ProductCategory>> GetAllAsync();
        void Update(ProductCategory productCategory);
    }
}
