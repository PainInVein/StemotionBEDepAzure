using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        
        IRoleRepository RoleRepository { get; }

        IGradeRepository GradeRepository { get; }

        ISubjectRepository SubjectRepository { get; }

        IChapterRepository ChapterRepository { get; }

        ILessonRepository LessonRepository { get; }
        ILessonContentRepository LessonContentRepository { get; }
        IGameRepository GameRepository { get; }
        IGameResultRepository GameResultRepository { get; }

        IPaymentRepository PaymentRepository { get; }

        ISubscriptionPaymentRepository SubscriptionPaymentRepository { get; }

        ISubscriptionRepository SubscriptionRepository { get; }

        IStudentProgressRepository StudentProgressRepository { get; }

        IStudentRepository StudentRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
