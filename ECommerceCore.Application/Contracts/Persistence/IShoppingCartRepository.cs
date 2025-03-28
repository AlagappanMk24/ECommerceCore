﻿using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}
