namespace QuakeAPI.DTO.Session
{
    public class SessionDto
    {
        public int Id { get;set; }
        public string Name { get;set; } = null!;
        public int MaxPlayers { get;set; }
        public int ActivePlayers { get;set; }
        public string LocationName { get;set; } = null!;
    }
}