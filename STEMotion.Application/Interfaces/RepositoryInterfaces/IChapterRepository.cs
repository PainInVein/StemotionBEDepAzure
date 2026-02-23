using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface IChapterRepository : IGenericRepository<Chapter>
    {
        Task<IEnumerable<Chapter>> GetAllChapterAsync();
        Task<Chapter?> GetChapterWithSubjectAndGradeAsync(Guid chapterId);

    }
}
