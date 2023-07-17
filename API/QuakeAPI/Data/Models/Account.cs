using System.Text.Json.Serialization;

namespace QuakeAPI.Data.Models
{
    public class Account
    {
        public int Id {get;set;}
        public string Username {get;set;} = null!;
        [JsonIgnore]
        public string Email {get;set;} = null!;
        [JsonIgnore]
        public string PasswordHash {get;set;} = null!;
        public string Role {get;set;} = null!;
        public DateTime LastLoggedAt {get;set;}
        public DateTime? DeletedAt {get;set;}
        public int TelegramChatId {get;set;}

        [JsonIgnore]
        public List<ActiveAccount> ActiveAccounts {get;set;} = new();
        [JsonIgnore]
        public List<Token> Tokens {get;set;} = new();
        [JsonIgnore]
        public List<Notification> Notifications {get;set;} = new();
    }
}