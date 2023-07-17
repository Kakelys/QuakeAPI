using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuakeAPI.DTO.Notifications
{
    public class EmailNotification
    {
        public string subject {get;set;} = null!;
        public string message {get;set;} = null!;
    }
}