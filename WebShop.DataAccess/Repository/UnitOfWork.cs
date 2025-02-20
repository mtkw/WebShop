﻿using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository.IRepository;

namespace WebShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _context;
        public IProductRepository Product { get; private set; }

        public IProductCategoryRepository ProductCategory {  get; private set; }

        public ISupplierRepository Supplier {  get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }   
        public ICartItemRepository CartItem { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; } 
        public IUserRepository ApplicationUser {  get; private set; }
        public IUsersMessageRepository UsersMessage { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Product = new ProductRepository(_context);
            ProductCategory = new ProductCategoryRepository(_context);
            Supplier = new SupplierRepository(_context);
            ApplicationUser = new UserRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            CartItem = new CartItemRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);
            OrderDetail = new OrderDetailRepository(_context);
            UsersMessage = new UsersMessageRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
