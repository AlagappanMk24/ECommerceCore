using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Infrastructure.Services
{
    public class BrandService(IUnitOfWork unitOfWork) : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Retrieves all brands from the database.
        /// </summary>
        /// <returns>An enumerable list of all brands.</returns>
        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            return await _unitOfWork.Brands.GetAllAsync();
        }
    }
}
