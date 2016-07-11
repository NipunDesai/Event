using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using EventDemoProject.Models.Enums;
using EventDemoProject.Models.Modules.Global;

namespace EventDemoProject.Models.Modules
{
    public class Event : EventBase
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ImageGuidUrl { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Description { get; set; }
       public string Location { get; set; }
       public float Latitude { get; set; }
       public float Longitude { get; set; }
       public int UserId { get; set; }
       //public EventJoin EventJoin { get; set; }
       public Int32 NotGoing { get; set; }
       public Int32 Going { get; set; }
       public Int32 Maybe { get; set; }
       [ForeignKey("UserId")]
       public virtual UserDetails User { get; set; }
       public virtual ICollection<EventJoin> EventJoin { get; set; } 
    }
}