using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuakeAPI.Mongo.Models;

namespace QuakeAPI.Mongo
{
    public class MongoLogger : ILogger
    {
        private readonly string _name;
        private readonly IMongoCollection<Log> _logsCollection;
        private readonly Func<MongoLoggerConfigration> _loggerConfig;

        public MongoLogger(string name, IOptions<MongoDatabaseOptions> options, Func<MongoLoggerConfigration> loggerConfig)
        {
            _name = name;
            _loggerConfig = loggerConfig;

            var optionsValue = options.Value;

            var mongoClient = new MongoClient(optionsValue.ConnectionString);
            _logsCollection = mongoClient.GetDatabase(optionsValue.DatabaseName).GetCollection<Log>(optionsValue.LogsCollectionName);
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            if(!_loggerConfig().MongoLogLevel.ContainsKey(logLevel))
                return false;

            return _loggerConfig().MongoLogLevel[logLevel];
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if(!IsEnabled(logLevel))
                return;

            var config = _loggerConfig();
            
            if(config.EventId != 0 && config.EventId != eventId.Id)
                return;
            var log = new Log(logLevel, $"{state}", exception);
            await _logsCollection.InsertOneAsync(log);
            // ConsoleColor originalColor = Console.ForegroundColor;
            // Console.ForegroundColor = ConsoleColor.DarkMagenta;
            // Console.WriteLine(log);
            // Console.ForegroundColor = originalColor;
        }
    }
}