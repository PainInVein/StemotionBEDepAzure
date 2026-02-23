using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.Middleware;
using STEMotion.Application.Services;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using STEMotion.Infrastructure.Repositories;
using STEMotion.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace STEMotion.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfranstructureToApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<StemotionContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ISubjectService, SubjectService>();

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddScoped<ISubscriptionPaymentRepository, SubscriptionPaymentRepository>();
            services.AddScoped<ISubscriptionPaymentService, SubscriptionPaymentService>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();

            services.AddScoped(typeof(IEmailService), typeof(SmtpEmailService));
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<ILessonContentRepository, LessonContentRepository>();
            services.AddScoped<ILessonContentService, LessonContentService>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameResultRepository, GameResultRepository>();
            services.AddScoped<IGameResultService, GameResultService>();
            services.AddScoped<IStudentProgressRepository, StudentProgressRepository>();
            services.AddScoped<IStudentProgressService, StudentProgressService>();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

                opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["accessToken"];
                        return Task.CompletedTask;
                    }
                };

            })
            .AddCookie()
            .AddGoogle(opt =>
            {
                opt.ClientId = configuration["Authentication:Google:ClientId"];
                opt.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });

            // Email
            var emailSettings = configuration.GetSection(nameof(SmtpEmailSettings)).Get<SmtpEmailSettings>();
            services.AddSingleton(emailSettings);

            return services;
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetSection("CacheSettings:ConnectionString").Value;
            if (string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentNullException("Redis Connection string is not configured");
            }
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });
        }
    }
}
