using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuakeAPI.Data.Models
{
    public class Location
    {
        public int Id {get;set;}
        public string Name {get;set;} = null!;
        public string Description {get;set;} = null!;
        public int MaxPlayers {get;set;} = 0;
        public string PosterPath {get;set;} = null!;
        public string LocationPath {get;set;} = null!;

        [JsonIgnore]
        public List<Session> Sessions {get;set;} = new List<Session>();
    }
}