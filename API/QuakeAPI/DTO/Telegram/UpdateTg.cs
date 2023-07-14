using System.Text.Json.Serialization;
using QuakeAPI.Extensions;

namespace QuakeAPI.DTO.Telegram
{
    public class UpdateTg
    {
        [JsonPropertyName("update_id")]
        public int Id { get;set; }
        public MessageTg Message { get;set; } = null!;
    }

    public class MessageTg
    {
        public string Text {get;set;} = null!;
        [JsonPropertyName("message_id")]
        public int Id {get;set;}
        [JsonConverter(typeof(TimestampToDateTimeConverter))]
        public DateTime Date {get;set;}
    }
}