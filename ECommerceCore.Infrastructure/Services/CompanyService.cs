using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Companies;
using ECommerceCore.Application.Contracts.ViewModels.Customers;
using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Services
{
    public class CompanyService(IUnitOfWork unitOfWork) : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return (List<Company>) await _unitOfWork.Companies.GetAllAsync();
        }
        public async Task<PaginatedResult<CompanyDto>> GetCompaniesPaginatedAsync(CompanyQueryParameters parameters)
        {
            try
            {
                // Base query
                var query = _unitOfWork.Companies.Query()
                    .Include(c => c.Locations)
                    .AsQueryable();

                // Company state filter
                if (!string.IsNullOrEmpty(parameters.State))
                {
                    query = query.Where(o => o.State == parameters.State);
                }

                // Apply search filter
                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    string searchTerm = parameters.SearchTerm.ToLower().Trim();
                    query = query.Where(c =>
                        c.Name.ToLower().Contains(searchTerm) ||
                        (c.City != null && c.City.ToLower().Contains(searchTerm)) ||
                        (c.State != null && c.State.ToLower().Contains(searchTerm))
                    );
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(parameters.SortColumn))
                {
                    query = parameters.SortColumn.ToLower() switch
                    {
                        "name" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                        "city" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.City) : query.OrderByDescending(c => c.City),
                        "state" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.State) : query.OrderByDescending(c => c.State),
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
                    .Select(c => new CompanyDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StreetAddress = c.StreetAddress,
                        City = c.City,
                        State = c.State,
                        PostalCode = c.PostalCode,
                        PhoneNumber = c.PhoneNumber,
                        LocationCount = c.Locations != null ? c.Locations.Count : 0
                    })
                    .ToListAsync();

                return new PaginatedResult<CompanyDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                // Log error
                throw;
            }
        }
        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await Task.Run(() => _unitOfWork.Companies.GetAsync(u => u.Id == id));
        }
        public async Task AddCompanyAsync(Company company)
        {
            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateCompanyAsync(Company company)
        {
            _unitOfWork.Companies.Update(company);
            await _unitOfWork.SaveAsync(); 
        }
        public async Task DeleteCompanyAsync(Company company)
        {
            await _unitOfWork.Companies.RemoveAsync(company);
            await _unitOfWork.SaveAsync();
        }
        public async Task<List<string>> GetCompanyStates()
        {
            try
            {
                // Create a query to get all unique non-null states from companies
                var query = _unitOfWork.Companies.Query()
                    .Where(c => !string.IsNullOrEmpty(c.State))
                    .Select(c => c.State)
                    .Distinct()
                    .OrderBy(s => s);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}