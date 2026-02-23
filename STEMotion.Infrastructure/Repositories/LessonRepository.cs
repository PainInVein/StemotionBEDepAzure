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
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(StemotionContext context) : base(context)
        {
        }
    }
}
