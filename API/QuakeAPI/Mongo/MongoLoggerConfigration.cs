using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuakeAPI.Mongo
{
    public class MongoLoggerConfigration
    {
        public int EventId { get; set; }
        public Dictionary<LogLevel, bool> MongoLogLevel {get;set;} = new();
    }
}