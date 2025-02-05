using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Infrastructure.Services
{
    public class ContactUsService(IUnitOfWork unitOfWork) : IContactUsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Submits a contact form by adding the contact information to the database.
        /// </summary>
        /// <param name="contactUs">The contact form data to submit.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SubmitContactForm(ContactUs contactUs)
        {
            await _unitOfWork.ContactUs.AddAsync(contactUs);
            await _unitOfWork.SaveAsync();
        }
    }
}
