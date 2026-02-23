using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<Subject?> GetSubjectByIdWithGradeAsync(Guid id)
        {
            return await FindByCondition(x => x.SubjectId == id, false).Include(x => x.Grade).FirstOrDefaultAsync();
        }

        public async Task<Subject?> GetSubjectByNameAndGradeAsync(string subjectName, Guid gradeId)
        {
            return await FindByCondition(s => s.SubjectName.ToLower() == subjectName
                                     && s.GradeId == gradeId).FirstOrDefaultAsync();
        }
    }
}
