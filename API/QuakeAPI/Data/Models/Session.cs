using System.Text.Json.Serialization;

namespace QuakeAPI.Data.Models
{
    public class Session
    {
        public int Id { get;set; }
        public string Name { get;set; } = null!;
        public int LocationId { get;set; }
        public int MaxPlayers { get;set; }
        // json ignore for prevent cycling with ActiveAccount on serializing
        [JsonIgnore]
        public Location Location { get;set; } = null!;
        [JsonIgnore]
        public List<ActiveAccount> ActiveAccounts { get;set; } = new List<ActiveAccount>();
    }
}