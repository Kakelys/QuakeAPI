using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuakeAPI.Data.Models
{
    public class ActiveAccount
    {
        public int Id { get;set; }
        public int AccountId {get;set;}
        public int SessionId {get;set;}
        public DateTime Connected { get;set; }
        public DateTime? Disconnected { get;set; }

        public Account Account {get;set;} = null!;
        public Session Session {get;set;} = null!;
    }
}