using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Repository
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private ApplicationDbContext _context;
        public ProductCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ProductCategory>> GetAllAsync()
        {
            return _context.Categories.ToListAsync();
        }

        public void Update(ProductCategory productCategory)
        {
            var existingEntity = _context.Categories.Local.FirstOrDefault(x => x.Id == productCategory.Id);
            if (existingEntity == null)
            {
                _context.Categories.Attach(productCategory);
            }
            else
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(productCategory);
            }
        }

        
    }
}
