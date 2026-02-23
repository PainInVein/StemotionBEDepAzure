using Microsoft.EntityFrameworkCore;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<Game?> GetByIdAsync(Guid gameId);
        Task<IEnumerable<Game>> GetByLessonIdAsync(Guid lessonId);
        Task<bool> GameCodeExistsAsync(string gameCode);
    }
}
