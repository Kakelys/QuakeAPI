using System.Text.Json.Serialization;

namespace QuakeAPI.Data.Models
{
    public class Notification
    {
        public Guid Id { get;set; }
        public int AccountId {get;set;}
        public string Name {get;set;} = null!;
        public string Data {get;set;} = null!;
        public DateTime CreatedAt {get;set;}
        public DateTime? ReadAt {get;set;}

        [JsonIgnore]
        public Account Account {get;set;} = null!;
    }
}