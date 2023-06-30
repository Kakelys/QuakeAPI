namespace QuakeAPI.Data.Models
{
    public class Session
    {
        public int Id { get;set; }
        public string Name { get;set; } = null!;
        public int LocationId { get;set; }
        public int MaxPlayers { get;set; }

        public Location Location { get;set; } = null!;
        public List<ActiveAccount> ActiveAccounts { get;set; } = new List<ActiveAccount>();
    }
}