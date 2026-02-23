using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayOS;

namespace STEMotion.Infrastructure.Configuration
{
    public static class PayOSConfiguration
    {
        public static IServiceCollection AddPayOSConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<PayOSClient>(sp =>
            {
                var payOSSettings = new PayOSClient
                (
                    configuration["PayOS:ClientId"]!,
                    configuration["PayOS:ApiKey"]!,
                    configuration["PayOS:ChecksumKey"]!
                );
                return payOSSettings;
            });


            return services;
        }
    }
}
