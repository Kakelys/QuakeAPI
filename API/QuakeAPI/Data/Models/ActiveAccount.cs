namespace QuakeAPI.Data.Models
{
    public class ActiveAccount
    {
        public int AccountId {get;set;}
        public int SessionId {get;set;}

        public Account Account {get;set;} = null!;
        public Session Session {get;set;} = null!;
    }
}