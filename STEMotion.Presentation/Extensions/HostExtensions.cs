using Microsoft.EntityFrameworkCore;

namespace STEMotion.Presentation.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContent>(this IHost host, Action<TContent, IServiceProvider> seeder) where TContent : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContent>>();
                var context = services.GetService<TContent>();

                try
                {
                    logger.LogInformation("Migrating sql server database");

                    ExecuteMigrations(context);

                    logger.LogInformation("Migrating sql server database");
                    InvokeSeeder(seeder, context, services);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while migrating the sql server database");
                }
            }
            return host;
        }


        private static void ExecuteMigrations<TContext>(TContext context) where TContext : DbContext
        {
            context.Database.Migrate();
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            seeder(context, services);
        }
    }
}
