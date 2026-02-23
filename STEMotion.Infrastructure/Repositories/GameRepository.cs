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
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly StemotionContext _context;

        public GameRepository(StemotionContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Game?> GetByIdAsync(Guid gameId)
        {
            return await _context.Games
                .Include(g => g.Lesson)
               .FirstOrDefaultAsync(g => g.GameId == gameId && g.Status);
        }

        public async Task<IEnumerable<Game>> GetByLessonIdAsync(Guid lessonId)
        {
            return await _context.Games
                .Where(g => g.LessonId == lessonId && g.Status)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<bool> GameCodeExistsAsync(string gameCode)
        {
            return await _context.Games
                .AnyAsync(g => g.GameCode == gameCode);
        }
    }
}
