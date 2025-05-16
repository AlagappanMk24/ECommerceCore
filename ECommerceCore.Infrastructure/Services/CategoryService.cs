using AutoMapper;
using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels.Categories;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Services
{
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }
        public async Task<Category>? GetCategoryById(int id)
        {
            return await _unitOfWork.Categories.GetAsync(c => c.Id == id);
        }
        public async Task<PaginatedResult<CategoryDto>> GetCategoriesPaginatedAsync(CategoryQueryParameters parameters)
        {
            try
            {
                // Base query
                var query = _unitOfWork.Categories.Query()
                    .Include(c => c.ParentCategory)
                    .AsQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    string searchTerm = parameters.SearchTerm.ToLower().Trim();
                    query = query.Where(c =>
                        c.Name.ToLower().Contains(searchTerm) ||
                        (c.Description != null && c.Description.ToLower().Contains(searchTerm))
                    );
                }

                // Apply parent category filter
                if (parameters.ParentCategoryId.HasValue)
                {
                    query = query.Where(c => c.ParentCategoryId == parameters.ParentCategoryId.Value);
                }

                // Apply active status filter
                if (parameters.IsActive.HasValue)
                {
                    query = query.Where(c => c.IsActive == parameters.IsActive.Value);
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(parameters.SortColumn))
                {
                    query = parameters.SortColumn.ToLower() switch
                    {
                        "name" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                        "displayorder" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.DisplayOrder) : query.OrderByDescending(c => c.DisplayOrder),
                        "parentcategory" => parameters.SortDirection == "asc" ?
                            query.OrderBy(c => c.ParentCategory.Name) : query.OrderByDescending(c => c.ParentCategory.Name),
                        _ => query.OrderBy(c => c.DisplayOrder)
                    };
                }
                else
                {
                    // Default sort by display order
                    query = query.OrderBy(c => c.DisplayOrder);
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var items = await query
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        DisplayOrder = c.DisplayOrder,
                        IsActive = c.IsActive,
                        ParentCategoryId = c.ParentCategoryId,
                        ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null
                    })
                    .ToListAsync();

                return new PaginatedResult<CategoryDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error in GetCategoriesPaginatedAsync");
                throw;
            }
        }

    }
}