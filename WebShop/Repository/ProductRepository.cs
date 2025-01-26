﻿using WebShop.DataAccess.Data;
using WebShop.Models.Models;
using WebShop.Repository.IRepository;

namespace WebShop.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            _context.Update(product);
        }
    }
}
