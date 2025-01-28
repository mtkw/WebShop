namespace WebShop.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        ISupplierRepository Supplier { get; }
        IShoppingCartRepository ShoppingCart { get; }
        ICartItemRepository CartItem { get; }
        IUserRepository ApplicationUser { get; } 
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IUsersMessageRepository UsersMessage { get; }
        void Save();
    }
}
