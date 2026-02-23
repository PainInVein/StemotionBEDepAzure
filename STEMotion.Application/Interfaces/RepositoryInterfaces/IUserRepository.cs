using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailWithRoleAsync(string email, bool trackChanges);
        Task<User?> GetUserWithChildrenAsync(Guid parentId, bool trackChanges);
    }
}
