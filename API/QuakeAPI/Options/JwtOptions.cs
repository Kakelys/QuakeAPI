using System.ComponentModel.DataAnnotations;

namespace QuakeAPI.Options
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";
        
        [Required]
        public string Issuer { get;set; } = "";
        [Required]
        public string Audience { get;set; } = "";
        [Required]
        public string AccessSecret { get;set; } = "";
        [Required]
        public string TgSecret {get;set;} = "";
        [Required]
        public string RefreshSecret { get;set; } = "";
        [Range(0, int.MaxValue)]
        public int AccessLifetimeInMinutes { get;set; } = 0;
        [Range(0, int.MaxValue)]
        public int RefreshLifetimeInMinutes { get;set; } = 0;
        [Range(0, int.MaxValue)]
        public int TgLifetimeInMinutes {get;set;} = 0;
        [Range(0, int.MaxValue)]
        public int MaxTokenCount { get;set; } = 0;
    }
}