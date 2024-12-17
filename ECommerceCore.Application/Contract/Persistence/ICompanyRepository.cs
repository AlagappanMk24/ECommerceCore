using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}
