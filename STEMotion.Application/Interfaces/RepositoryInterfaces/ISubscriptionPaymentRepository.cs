using STEMotion.Domain.Entities;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface ISubscriptionPaymentRepository : IGenericRepository<SubscriptionPayment>
    {
        Task<SubscriptionPayment?> GetSubscriptionPaymentByOrderCodeAsync(long orderCode);
    }
}
