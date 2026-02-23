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
    public class GradeRepository : GenericRepository<Grade>, IGradeRepository
    {
        public GradeRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<Grade?> GetGradeByLevelAsync(int gradeLevel)
        {
            return await FindByCondition(g => g.GradeLevel == gradeLevel).FirstOrDefaultAsync();
        }
    }
}
