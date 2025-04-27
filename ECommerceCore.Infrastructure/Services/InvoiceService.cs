using AutoMapper;
using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Application.Contracts.ViewModels;
using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Services
{
    public class InvoiceService(IUnitOfWork unitOfWork) : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<PaginatedResult<InvoiceDto>> GetInvoicesPaginatedAsync(InvoiceQueryParameters parameters)
        {
            try
            {
                // Base query with includes for navigation properties
                var query = _unitOfWork.Invoices.Query()
                    .Include(i => i.Customer)
                    .AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    string searchTerm = parameters.SearchTerm.ToLower().Trim();
                    query = query.Where(i =>
                        i.InvoiceNumber.ToLower().Contains(searchTerm) ||
                        i.PONumber.ToLower().Contains(searchTerm) ||
                        i.Notes.ToLower().Contains(searchTerm) ||
                        i.ExternalReference.ToLower().Contains(searchTerm) ||
                        i.Customer.Name.ToLower().Contains(searchTerm) ||
                        i.Company.Name.ToLower().Contains(searchTerm)
                    );
                }

                // Apply customer filter
                if (parameters.CustomerId.HasValue)
                {
                    query = query.Where(i => i.CustomerId == parameters.CustomerId.Value);
                }

                // Apply status filter
                if (!string.IsNullOrEmpty(parameters.Status))
                {
                    if (Enum.TryParse<InvoiceStatus>(parameters.Status, out var status))
                    {
                        query = query.Where(i => i.Status == status);
                    }
                }

                // Apply type filter
                if (!string.IsNullOrEmpty(parameters.Type))
                {
                    if (Enum.TryParse<InvoiceType>(parameters.Type, out var type))
                    {
                        query = query.Where(i => i.InvoiceType == type);
                    }
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(parameters.SortColumn))
                {
                    query = parameters.SortColumn.ToLower() switch
                    {
                        "invoicenumber" => parameters.SortDirection == "asc" ?
                            query.OrderBy(i => i.InvoiceNumber) :
                            query.OrderByDescending(i => i.InvoiceNumber),
                        "issuedate" => parameters.SortDirection == "asc" ?
                            query.OrderBy(i => i.IssueDate) :
                            query.OrderByDescending(i => i.IssueDate),
                        "totalamount" => parameters.SortDirection == "asc" ?
                            query.OrderBy(i => i.TotalAmount) :
                            query.OrderByDescending(i => i.TotalAmount),
                        "customer" => parameters.SortDirection == "asc" ?
                            query.OrderBy(i => i.Customer.Name) :
                            query.OrderByDescending(i => i.Customer.Name),
                        "status" => parameters.SortDirection == "asc" ?
                            query.OrderBy(i => i.Status) :
                            query.OrderByDescending(i => i.Status),
                        "type" => parameters.SortDirection == "asc" ?
                            query.OrderBy(i => i.InvoiceType) :
                            query.OrderByDescending(i => i.InvoiceType),
                        _ => query.OrderBy(i => i.InvoiceNumber)
                    };
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var items = await query
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Select(i => new InvoiceDto
                     {
                          Id = i.Id,
                          InvoiceNumber = i.InvoiceNumber,
                          CustomerName = i.Customer.Name,
                          TotalAmount = i.TotalAmount,
                          Status = i.Status.ToString(),
                          InvoiceType = i.InvoiceType.ToString(),
                          IssueDate = i.IssueDate
                     }).ToListAsync();

                return new PaginatedResult<InvoiceDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                // Log error (assuming a logger is injected or available)
                throw new Exception("Error fetching invoices", ex);
            }
        }
    }
}
