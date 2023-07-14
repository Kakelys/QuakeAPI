using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace QuakeAPI.Mongo
{
    public static class MongoLoggerExtensions
    {
        public static ILoggingBuilder AddMongoLogger(
        this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, MongoLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <MongoLoggerConfigration, MongoLoggerProvider>(builder.Services);

            return builder;
        }
    }
}