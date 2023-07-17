using System.Text.Json.Serialization;
using QuakeAPI.Extensions;

namespace QuakeAPI.DTO.Telegram
{
    public class Update
    {
        [JsonPropertyName("update_id")]
        public int Id { get;set; }
        public Message Message { get;set; } = null!;
    }
}