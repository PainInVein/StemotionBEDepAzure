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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await FindByCondition(r => r.Name.ToLower() == roleName.ToLower())
                                     .FirstOrDefaultAsync();
        }
    }
}
