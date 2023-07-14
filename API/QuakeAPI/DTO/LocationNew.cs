using FluentValidation;

namespace QuakeAPI.DTO
{
    public class LocationNew
    {
        public string Name {get;set;} = null!;
        public string Description {get;set;}  = null!;
        public int MaxPlayers {get;set;}
        public IFormFile Poster {get;set;} = null!;
        public IFormFile Location {get;set;} = null!;
    }
}