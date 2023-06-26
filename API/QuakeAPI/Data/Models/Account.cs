using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuakeAPI.Data.Models
{
    public class Account
    {
        public int Id {get;set;}
        public string Email {get;set;} = null!;
        public string PasswordHash {get;set;} = null!;

        public ActiveAccount? ActiveAccount {get;set;}
    }
}