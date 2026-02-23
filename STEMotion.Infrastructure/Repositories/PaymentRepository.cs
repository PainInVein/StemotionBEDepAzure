using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;

namespace STEMotion.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<bool> CheckUserPaymentAsync(Guid userId)
        {
            var payment = await FindByCondition(p => p.UserId == userId).FirstOrDefaultAsync();
            return payment != null;
        }
    }
}
