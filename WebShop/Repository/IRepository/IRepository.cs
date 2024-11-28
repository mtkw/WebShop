﻿using System.Linq.Expressions;
using WebShop.Models;

namespace WebShop.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includProperties = null);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includProperties = null);
        void Add(T entity);
        void Remove(T entity);

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includProperties = null); 

        public T Get(Expression<Func<T, bool>> filter, string? includProperties = null);

    }
}
