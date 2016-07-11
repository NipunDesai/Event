using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventDemoProject.Models.Modules.ApplicationClass
{
    public class EventAc
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ImageGuidUrl { get; set; }
        public string EndDateTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public virtual List<GoogleAc> GoogleAc { get; set; }
        public int GoingCount { get; set; }
        public int MayBe { get; set; }
        public int NotGoing { get; set; }
        public virtual List<EventJoin> EventJoin { get; set; }
        public DateTime EndTime { get; set; }
        public int EventSelect { get; set; }
        }
}