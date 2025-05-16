using ECommerceCore.Domain.Entities.Identity;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        public void Update(ApplicationUser applicationUser);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByIdAsync(string id);
    }
}