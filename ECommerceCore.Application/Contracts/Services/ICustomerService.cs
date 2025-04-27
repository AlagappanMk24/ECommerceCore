using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Customers;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int id);
        Task<Customer> GetCustomerWithDetails(int id);
        Task<PaginatedResult<CustomerDto>> GetCustomersPaginatedAsync(CustomerQueryParameters parameters);
        Task<bool> AddCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int id);
    }
}
