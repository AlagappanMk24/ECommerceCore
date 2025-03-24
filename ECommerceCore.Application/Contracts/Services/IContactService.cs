using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IContactUsService
    {
        Task SubmitContactForm(ContactUs contactUs);
    }
}
