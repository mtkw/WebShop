﻿namespace WebShop.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        ISupplierRepository Supplier { get; }

        IUserRepository ApplicationUser { get; } 

        void Save();
    }
}
