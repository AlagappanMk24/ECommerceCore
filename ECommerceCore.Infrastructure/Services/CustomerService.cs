using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Application.Contracts.ViewModels.Customers;
using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Services
{
    public class CustomerService(IUnitOfWork unitOfWork) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _unitOfWork.Customers
                .Query()
                .Include(c => c.Company)
                .ToListAsync();
        }
        public async Task<Customer> GetCustomerById(int id)
        {
            return await _unitOfWork.Customers
                .Query()
                .Include(c => c.Company)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Customer> GetCustomerWithDetails(int id)
        {
            return await _unitOfWork.Customers
                .Query()
                .Include(c => c.Company)
                .Include(c => c.Orders)
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<PaginatedResult<CustomerDto>> GetCustomersPaginatedAsync(CustomerQueryParameters parameters)
        {
            try
            {
                // Base query
                var query = _unitOfWork.Customers
                    .Query()
                    .Include(c => c.Company)
                    .AsQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    string searchTerm = parameters.SearchTerm.ToLower().Trim();
                    query = query.Where(c =>
                        c.Name.ToLower().Contains(searchTerm) ||
                        (c.Email != null && c.Email.ToLower().Contains(searchTerm)) ||
                        (c.PhoneNumber != null && c.PhoneNumber.Contains(searchTerm)) ||
                        (c.Company != null && c.Company.Name.ToLower().Contains(searchTerm))
                    );
                }

                // Apply company filter
                if (parameters.CompanyId.HasValue)
                {
                    query = query.Where(c => c.CompanyId == parameters.CompanyId.Value);
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(parameters.SortColumn))
                {
                    query = parameters.SortColumn.ToLower() switch
                    {
                        "name" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                        "email" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email),
                        "phonenumber" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.PhoneNumber) : query.OrderByDescending(c => c.PhoneNumber),
                        "company" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.Company.Name) : query.OrderByDescending(c => c.Company.Name),
                        _ => query.OrderBy(c => c.Name)
                    };
                }
                else
                {
                    // Default sort by name
                    query = query.OrderBy(c => c.Name);
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var items = await query
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        Address = new AddressDto
                        {
                            Address1 = c.Address.Address1,
                            Address2 = c.Address.Address2,
                            City = c.Address.City,
                            State = c.Address.State,
                            Country = c.Address.Country,
                            ZipCode = c.Address.ZipCode
                        },
                        CompanyId = c.CompanyId,
                        CompanyName = c.Company.Name
                    })
                    .ToListAsync();

                return new PaginatedResult<CustomerDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AddCustomer(Customer customer)
        {
            try
            {
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            try
            {
                _unitOfWork.Customers.Update(customer);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetAsync(c => c.Id == id);
                if (customer == null)
                {
                    return false;
                }

                // Check if customer has orders
                var hasOrders = await _unitOfWork.OrderHeaders.Query().AnyAsync(o => o.CustomerId == id);

                // Check if customer has invoices
                var hasInvoices = await _unitOfWork.Invoices.Query().AnyAsync(i => i.CustomerId == id);

                if (hasOrders || hasInvoices)
                {
                    return false;
                }

                _unitOfWork.Customers.Delete(customer);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}