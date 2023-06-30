using System.ComponentModel.DataAnnotations;

namespace QuakeAPI.DTO
{
    public class LocationNew
    {

        [Required]
        public string Name {get;set;} = null!;
        [Required]
        public string Description {get;set;}  = null!;
        [Range(2,64)]
        public int MaxPlayers {get;set;}

        [Required]
        public IFormFile Poster {get;set;} = null!;
        [Required]
        public IFormFile Location {get;set;} = null!;
    }
}