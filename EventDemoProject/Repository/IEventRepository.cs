using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventDemoProject.Models.Enums;
using EventDemoProject.Models.Modules;
using EventDemoProject.Models.Modules.ApplicationClass;

namespace EventDemoProject.Repository
{
    public interface IEventRepository : IDisposable
    {
        IEnumerable<Event> GetAllEventList();

        void CreateNewEvent(EventAc events,string username);

        EventJoin EventChange(EventJoinEnum status, int eventId,string currentUser);
    }
}
