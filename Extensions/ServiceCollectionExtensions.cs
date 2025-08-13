using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NicheVinDecoder.Core.Factory;
using NicheVinDecoder.Core.Interfaces;

namespace NicheVinDecoder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNicheVinDecoding(this IServiceCollection services)
        {
            // Register all IVinDecoder implementations
            var decoderTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(IVinDecoder).IsAssignableFrom(type) &&
                               !type.IsInterface &&
                               !type.IsAbstract);

            foreach (var type in decoderTypes)
            {
                services.AddTransient(typeof(IVinDecoder), type);
            }

            // Register the factory
            services.AddTransient<VinDecoderFactory>();

            return services;
        }
    }
}
