namespace QuakeAPI.Data.Models
{
    public class Account
    {
        public int Id {get;set;}
        public string Username {get;set;} = null!;
        public string Email {get;set;} = null!;
        public string PasswordHash {get;set;} = null!;
        public string Role {get;set;} = null!;

        public List<ActiveAccount> ActiveAccounts {get;set;} = new();
        public List<Token> Tokens {get;set;} = new();
    }
}