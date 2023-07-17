using System.Text.Json.Serialization;
using QuakeAPI.Extensions;

namespace QuakeAPI.DTO.Telegram
{
    public class Message
    {
        [JsonPropertyName("message_id")]
        public int Id {get;set;}
        public Chat Chat {get;set;} = null!;
        public string Text {get;set;} = null!;
        [JsonConverter(typeof(TimestampToDateTimeConverter))]
        public DateTime Date {get;set;}
    }
}