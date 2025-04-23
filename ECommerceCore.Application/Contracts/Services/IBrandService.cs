using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.Services
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllBrands();
    }
}
