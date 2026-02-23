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
    public class ChapterRepository : GenericRepository<Chapter>, IChapterRepository
    {
        public ChapterRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Chapter>> GetAllChapterAsync()
        {
            return await FindAll(false).Include(x => x.Subject).ToListAsync();
        }
        public async Task<Chapter?> GetChapterWithSubjectAndGradeAsync(Guid chapterId)
        {
            return await FindByCondition(x => x.ChapterId == chapterId, false)
        .Include(x => x.Subject)
        .ThenInclude(s => s.Grade)
        .FirstOrDefaultAsync();

        }
    }
}
