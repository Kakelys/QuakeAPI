using QuakeAPI.Data.Models;

namespace QuakeAPI.Models.Session
{
    public class SessionDetail
    {
        public int Id { get;set; }
        public string Name { get;set; } = null!;
        public int MaxPlayers { get;set; }
        public int ActivePlayers { get;set; }
        public Location Location { get;set; } = null!;
    }
}