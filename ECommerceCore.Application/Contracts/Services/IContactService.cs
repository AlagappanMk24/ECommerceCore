using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IContactUsService
    {
        Task SubmitContactForm(ContactUs contactUs);
    }
}
