using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuakeAPI.Data.Models
{
    public class Session
    {
        public int Id { get;set; }
        public string Name { get;set; } = null!;
        public int LocationId { get;set; }

        public Location Location { get;set; } = null!;
        public List<ActiveAccount> ActiveAccounts { get;set; } = new List<ActiveAccount>();
    }
}