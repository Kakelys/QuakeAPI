using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace QuakeAPI.Mongo
{
    [ProviderAlias("MongoLogger")]
    public class MongoLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable? _onChangeToken;
        private MongoLoggerConfigration _currentConfig;
        private readonly ConcurrentDictionary<string, MongoLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly IOptions<MongoDatabaseOptions> _options;

        public MongoLoggerProvider(
            IOptionsMonitor<MongoLoggerConfigration> config,
            IOptions<MongoDatabaseOptions> options)
        {
            _options = options;

            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName) => 
            _loggers.GetOrAdd(categoryName, name => new MongoLogger(name, _options, GetCurrentConfig));

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken?.Dispose();
        }

        private MongoLoggerConfigration GetCurrentConfig() => _currentConfig;
    }
}