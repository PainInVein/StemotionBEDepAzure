using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<Subscription?> GetSubscriptionByIdAsync(Guid subscriptionId)
        {
            return await FindByCondition(g => g.SubscriptionId == subscriptionId).FirstOrDefaultAsync();
        }
    }
}
