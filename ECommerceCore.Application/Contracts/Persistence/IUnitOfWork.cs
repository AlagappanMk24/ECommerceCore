﻿using ECommerceCore.Application.Contracts.Persistence;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IBrandRepository Brands { get; }
        IProductRepository Products { get; }
        ICompanyRepository Companies { get; }
        IShoppingCartRepository ShoppingCarts { get; }
        IUserRepository ApplicationUsers { get; }
        IOrderDetailRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeaders { get; }
        IProductImageRepository ProductImages { get; }
        IInvoiceRepository Invoices { get; }
        ICustomerRepository Customers { get; }
        IContactUsRepository ContactUs { get; }
        Task SaveAsync();
    }
}
