using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuakeAPI.Data.Models
{
    public class ActiveAccount
    {
        public int AccountId {get;set;}
        public int SessionId {get;set;}

        public Account Account {get;set;} = null!;
        public Session Session {get;set;} = null!;
    }
}