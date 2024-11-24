namespace WebShop.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        ISupplierRepository Supplier { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IUserRepository ApplicationUser { get; } 

        void Save();
    }
}
