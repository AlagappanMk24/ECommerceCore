﻿using System.Linq.Expressions;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IRepository<T> where T : class
    {
        //T - category
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}