using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuakeAPI.Data.Models
{
    public class ActiveAccount
    {
        public int Id { get;set; }
        public int AccountId {get;set;}
        public int SessionId {get;set;}
        public DateTime ConnectedAt { get;set; }
        public DateTime? DisconnectedAt { get;set; }

        public Account Account {get;set;} = null!;
        public Session Session {get;set;} = null!;
    }
}