namespace ECommerceCore.Infrastructure.Data.DbInitializer
{
    public interface IDbInitializer
    {
        Task Initialize(CancellationToken cancellationToken);
    }
}
