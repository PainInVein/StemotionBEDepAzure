using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Infrastructure.DBContext;

namespace STEMotion.Presentation.Extensions
{
    public static class MigrationsExtension
    {
        public static IHost ApplyMigrations(this IHost host)
        {
            host.MigrateDatabase<StemotionContext>((context, services) =>
            {
                var logger = services.GetRequiredService<ILogger<StemotionContextSeed>>();
                var passwordService = services.GetRequiredService<IPasswordService>();

                StemotionContextSeed.SeedProductAsync(context, logger, passwordService).Wait();
            });
            return host;
        }
    }
}
