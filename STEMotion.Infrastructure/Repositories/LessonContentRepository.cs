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
    public class LessonContentRepository : GenericRepository<LessonContent>, ILessonContentRepository
    {
        public LessonContentRepository(StemotionContext context) : base(context)
        {
        }
    }
}
