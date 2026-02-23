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
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            return await FindByCondition(s => s.ParentId == parentId && s.Status == "Active", false)
                         .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdWithParentAsync(Guid studentId)
        {
            return await FindByCondition(s => s.StudentId == studentId, false)
                         .Include(s => s.Parent)
                         .FirstOrDefaultAsync();
        }

        public async Task<Student?> GetStudentByUsernameAsync(string? username, bool trackChanges)
        {
            return await FindByCondition(u => u.Username == username, trackChanges)
                            .FirstOrDefaultAsync();
        }
    }
}
