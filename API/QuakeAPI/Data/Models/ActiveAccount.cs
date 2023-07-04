using System.ComponentModel.DataAnnotations;

namespace QuakeAPI.Data.Models
{
    public class ActiveAccount
    {
        //keys needed only for mocking
        [Key]
        public int AccountId {get;set;}
        [Key]
        public int SessionId {get;set;}

        public Account Account {get;set;} = null!;
        public Session Session {get;set;} = null!;
    }
}