using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventDemoProject.Models.Modules.Global
{
    public class UserDetails : EventBase
    {
        public string EmailId { get; set; }
        public string FirstName { get; set; }
    }
}