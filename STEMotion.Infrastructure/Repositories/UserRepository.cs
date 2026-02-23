using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailWithRoleAsync(string email, bool trackChanges)
        {
            return await FindByCondition(u => u.Email == email, trackChanges)
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithChildrenAsync(Guid parentId, bool trackChanges)
        {
            return await FindByCondition(u => u.UserId == parentId, trackChanges)
                            .Include(u => u.Students)
                            .FirstOrDefaultAsync();
        }
    }
}

