namespace QuakeAPI.Data.Models
{
    public class Token
    {
        public int Id { get;set; }        
        public int AccountId { get;set; }
        public string RefreshToken { get;set; } = null!;
        public DateTime Expires { get;set; }

        public Account Account { get;set; } = null!;
    }
}