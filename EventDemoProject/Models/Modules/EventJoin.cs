using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using EventDemoProject.Models.Enums;
using EventDemoProject.Models.Modules.Global;

namespace EventDemoProject.Models.Modules
{
    public class EventJoin:EventBase
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public EventJoinEnum Status { get; set; }

        [ForeignKey("UserId")]
        public virtual UserDetails User { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

    }
}