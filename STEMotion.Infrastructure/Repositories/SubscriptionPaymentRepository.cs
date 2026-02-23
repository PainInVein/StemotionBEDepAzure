using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;

namespace STEMotion.Infrastructure.Repositories
{
    public class SubscriptionPaymentRepository : GenericRepository<SubscriptionPayment>, ISubscriptionPaymentRepository
    {
        public SubscriptionPaymentRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<SubscriptionPayment?> GetSubscriptionPaymentByOrderCodeAsync(long orderCode)
        {
            return await FindByCondition(sp => sp.OrderCode == orderCode).Include(p => p.Payment).FirstOrDefaultAsync();

        }
    }
}
