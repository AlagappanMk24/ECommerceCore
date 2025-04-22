using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        void Update(Company obj);
    }
}
