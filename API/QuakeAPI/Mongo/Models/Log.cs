using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuakeAPI.Mongo.Models
{
    public class Log
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Message { get; set; } = null!;
        public string Level { get; set; } = null!;
        [BsonIgnoreIfNull]
        public string? ExceptionMessage { get; set; }
        public DateTime Timestamp { get; set; }

        public Log()
        {}

        public Log(LogLevel logLevel, string message, Exception? exception)
        {
            Level = logLevel.ToString();
            Message = message;
            ExceptionMessage = exception?.ToString();
            Timestamp = DateTime.UtcNow;
        }

        public Log(LogLevel logLevel, string message) 
            : this(logLevel, message, null)
        {}

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{Timestamp}] {Level}:");
            if(!string.IsNullOrEmpty(Message))
                sb.AppendLine($"{Message}");
            if(ExceptionMessage != null)
                sb.AppendLine(ExceptionMessage);

            return sb.ToString();
        }
    }
}