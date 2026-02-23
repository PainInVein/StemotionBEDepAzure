using AutoMapper;
using PayOS.Models.V2.PaymentRequests;
using PayOS.Models.Webhooks;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.RequestDTOs.StudentReqDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.DTO.ResponseDTOs.StudentResponseDTO;
using STEMotion.Application.Extensions;
using STEMotion.Domain.Entities;

namespace STEMotion.Application.Middleware
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDTO>()
                    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                    .ReverseMap();

            CreateMap<CreateUserRequestDTO, User>().ReverseMap();

            CreateMap<UpdateUserRequestDTO, User>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Grade, GradeResponseDTO>().ReverseMap();
            CreateMap<GradeRequestDTO, Grade>().ReverseMap();
            CreateMap<UpdateGradeRequest, Grade>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Subject, SubjectResponseDTO>()
                .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.Grade.GradeLevel))
                .ReverseMap();
            CreateMap<SubjectRequestDTO, Subject>().ReverseMap();
            CreateMap<UpdateSubjectRequestDTO, Subject>()
                .IgnoreAllNonExisting()
                .ReverseMap()
                .ForMember(dest => dest.GradeLevel, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Chapter, ChapterResponseDTO>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
                    .ForMember(dest => dest.GradeLevel,
        opt => opt.MapFrom(src => src.Subject.Grade.GradeLevel))
                .ReverseMap();

            CreateMap<ChapterRequestDTO, Chapter>().IgnoreAllNonExisting().ReverseMap();
            CreateMap<UpdateChapterRequestDTO, Chapter>().IgnoreAllNonExisting().ReverseMap();

            CreateMap<Lesson, LessonResponseDTO>()
                .ForMember(dest => dest.ChapterName, opt => opt.MapFrom(src => src.Chapter.ChapterName))
                    .ForMember(dest => dest.GradeLevel,
        opt => opt.MapFrom(src => src.Chapter.Subject.Grade.GradeLevel))
                .ReverseMap();

            CreateMap<LessonRequestDTO, Lesson>().IgnoreAllNonExisting().ReverseMap();
            CreateMap<UpdateLessonRequestDTO, Lesson>().IgnoreAllNonExisting().ReverseMap();

            CreateMap<GoogleRequestDTO, User>()
                .IgnoreAllNonExisting()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Subscription, SubscriptionResponseDTO>().ReverseMap();

            CreateMap<CreatePaymentLinkResponse, PaymentResponseDTO>()
                .ForMember(dest => dest.CheckoutUrl, opt => opt.MapFrom(src => src.CheckoutUrl))
                .ForMember(dest => dest.PaymentLinkId, opt => opt.MapFrom(src => src.PaymentLinkId))
                .ForMember(dest => dest.OrderCode, opt => opt.MapFrom(src => src.OrderCode))
                .ReverseMap();

            CreateMap<WebhookData, SubscriptionPayment>()
            .ForMember(dest => dest.CounterAccountBankId, opt => opt.MapFrom(src => src.CounterAccountBankId))
            .ForMember(dest => dest.CounterAccountName, opt => opt.MapFrom(src => src.CounterAccountName))
            .ForMember(dest => dest.CounterAccountNumber, opt => opt.MapFrom(src => src.CounterAccountNumber));

            CreateMap<LessonContent, LessonContentResponseDTO>();

            CreateMap<CreateLessonContentRequestDTO, LessonContent>()
                .IgnoreAllNonExisting();

            CreateMap<UpdateLessonContentRequestDTO, LessonContent>()
                .IgnoreAllNonExisting();


            CreateMap<CreateGameDTO, Game>()
                .ForMember(dest => dest.GameId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.GameResults, opt => opt.Ignore());

            CreateMap<CreateGameRequestDto, Game>()
                .ForMember(dest => dest.GameId, opt => opt.Ignore())
                .ForMember(dest => dest.LessonId, opt => opt.Ignore())
                .ForMember(dest => dest.ConfigData, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.GameResults, opt => opt.Ignore());

            CreateMap<UpdateGameDto, Game>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Game, GameResponseDTO>()
                .ForMember(dest => dest.LessonName,
                    opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.LessonName : null));

            CreateMap<Game, PlayGameDTO>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<SubmitGameResultRequestDto, GameResult>()
                .ForMember(dest => dest.GameResultId, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.PlayDuration, opt => opt.MapFrom(src => src.PlayDurations))
                .ForMember(dest => dest.PlayedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore());

            CreateMap<GameResult, GameResultResponseDTO>()
                .ForMember(dest => dest.GameName,
                    opt => opt.MapFrom(src => src.Game != null ? src.Game.Name : null));

            CreateMap<GameResult, HistoryGameResultDto>()
                .ForMember(dest => dest.GameName,
                    opt => opt.MapFrom(src => src.Game != null ? src.Game.Name : null));

            CreateMap<GameResult, StudentGameStatsResponseDTO>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId))
                .ForMember(dest => dest.GameName,
                    opt => opt.MapFrom(src => src.Game != null ? src.Game.Name : null))
                .ForMember(dest => dest.BestScore, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.BestCorrectAnswers, opt => opt.MapFrom(src => src.CorrectAnswers))
                .ForMember(dest => dest.LastPlayedAt, opt => opt.MapFrom(src => src.PlayedAt))
                .ForMember(dest => dest.AttemptCount, opt => opt.Ignore());

            // StudentProgress mappings
            CreateMap<StudentProgress, LessonProgressResponseDTO>()
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson.LessonName))
                .ForMember(dest => dest.EstimatedTime, opt => opt.MapFrom(src => src.Lesson.EstimatedTime));

            //Student mappings
            CreateMap<AddChildrenRequestDTO, Student>();
            CreateMap<Student, AddChildrenResponseDTO>();
            CreateMap<Student, StudentLoginResonseDTO>();

        }
    }
}
