namespace QuakeAPI.DTO.Session
{
    public class SessionNew
    {
        public string Name { get;set; } = null!;
        public int LocationId { get;set; }
        public int MaxPlayers { get;set; }
    }
}