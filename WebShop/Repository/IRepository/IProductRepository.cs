﻿using WebShop.Models.Models;

namespace WebShop.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
