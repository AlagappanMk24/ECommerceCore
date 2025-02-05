using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        public void Update(ApplicationUser applicationUser);
        Task<bool> UpdateUserAsync(ApplicationUser user);
    }
}