using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentsByParentIdAsync(Guid parentId);
        Task<Student?> GetStudentByIdWithParentAsync(Guid studentId);
        Task<Student?> GetStudentByUsernameAsync(string? username, bool trackChanges);
    }
}
