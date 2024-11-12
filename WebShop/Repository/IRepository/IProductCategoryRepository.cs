using WebShop.Models;

namespace WebShop.Repository.IRepository
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        void Update(ProductCategory productCategory);
    }
}
