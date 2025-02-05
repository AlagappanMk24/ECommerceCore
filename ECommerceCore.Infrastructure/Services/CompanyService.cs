using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Infrastructure.Services
{
    public class CompanyService(IUnitOfWork unitOfWork) : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Retrieves a list of all companies from the database.
        /// </summary>
        /// <returns>A list of all companies.</returns>
        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return (List<Company>) await _unitOfWork.Company.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a specific company by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>The company with the specified ID.</returns>
        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await Task.Run(() => _unitOfWork.Company.GetAsync(u => u.Id == id));
        }

        /// <summary>
        /// Adds a new company to the database.
        /// </summary>
        /// <param name="company">The company object to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddCompanyAsync(Company company)
        {
            await _unitOfWork.Company.AddAsync(company);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Updates an existing company in the database.
        /// </summary>
        /// <param name="company">The company object with updated data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateCompanyAsync(Company company)
        {
            _unitOfWork.Company.Update(company);
            await _unitOfWork.SaveAsync(); 
        }

        /// <summary>
        /// Deletes a company from the database.
        /// </summary>
        /// <param name="company">The company object to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteCompanyAsync(Company company)
        {
            await _unitOfWork.Company.RemoveAsync(company);
            await _unitOfWork.SaveAsync();
        }
    }
}