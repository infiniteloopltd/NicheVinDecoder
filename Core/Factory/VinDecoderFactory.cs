using NicheVinDecoder.Core.Interfaces;
using System.Reflection;

namespace NicheVinDecoder.Core.Factory
{
    public class VinDecoderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IVinDecoder> _decoders;

        public VinDecoderFactory(IServiceProvider serviceProvider, IEnumerable<IVinDecoder> decoders)
        {
            _serviceProvider = serviceProvider;
            _decoders = decoders;
        }

        // Fallback constructor for non-DI scenarios
        public VinDecoderFactory()
        {
            _decoders = LoadDecodersDirectly();
        }

        public IVinDecoder? GetDecoder(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentException("VIN cannot be null or empty", nameof(vin));

            return _decoders.FirstOrDefault(decoder => decoder.CanDecode(vin));
        }

        public IEnumerable<IVinDecoder> GetAllDecoders()
        {
            return _decoders;
        }

        public T? GetDecoder<T>() where T : class, IVinDecoder
        {
            return _decoders.OfType<T>().FirstOrDefault();
        }

        private static IEnumerable<IVinDecoder> LoadDecodersDirectly()
        {
            var decoders = new List<IVinDecoder>();

            try
            {
                var decoderTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly =>
                    {
                        try
                        {
                            return assembly.GetTypes();
                        }
                        catch (ReflectionTypeLoadException)
                        {
                            return Array.Empty<Type>();
                        }
                    })
                    .Where(type => typeof(IVinDecoder).IsAssignableFrom(type) &&
                           !type.IsInterface &&
                           !type.IsAbstract)
                    .ToList();

                foreach (var type in decoderTypes)
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is IVinDecoder decoder)
                        {
                            decoders.Add(decoder);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                // Return empty list if reflection fails
            }

            return decoders;
        }
    }
}
