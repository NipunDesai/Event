using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using EventDemoProject.DataRepsitory;
using EventDemoProject.Helper;
using EventDemoProject.Models.Enums;
using EventDemoProject.Models.Modules;
using EventDemoProject.Models.Modules.ApplicationClass;
using EventDemoProject.Models.Modules.Global;
using EventDemoProject.Repository;

namespace EventDemoProject.Controllers
{
    public class EventController :ApiController
    {
        #region Private Member
        private readonly IEventRepository _eventRepository;
        private readonly IDataRepository<Event> _eventDataRepository;
        private readonly IDataRepository<EventJoin> _eventJoinDataRepository;
        private readonly IDataRepository<UserDetails> _userDataRepository; 

        #endregion

        #region Constructor
        public EventController(IEventRepository eventRepository, IDataRepository<Event> eventDataRepository, IDataRepository<EventJoin> eventJoinDataRepository, IDataRepository<UserDetails> userDataRepository)
        {
            _eventRepository = eventRepository;
            _eventDataRepository = eventDataRepository;
            _eventJoinDataRepository = eventJoinDataRepository;
            _userDataRepository = userDataRepository;
        }
        #endregion

        [HttpGet]
        [Route("api/Event/getAllEventList")]
        public List<EventAc> GetAllEventList()
        {
            try
            {
                var currentUser = HttpContext.Current.User.Identity.Name;
                var user = _userDataRepository.FirstOrDefault(x => x.EmailId == currentUser);
                var contentList = _eventRepository.GetAllEventList();
                var eventCollection = new List<EventAc>();
                foreach (var events in contentList)
                {
                    var eventAc = new EventAc();
                    if (events.EndDateTime >= DateTime.UtcNow)
                    {
                       var googleCollection = new List<GoogleAc>();
                        var googleAc = new GoogleAc();
                        eventAc = ApplicationClassHelper.ConvertType<Event, EventAc>(events);
                        eventAc.EventId = events.Id;
                        eventAc.Name = events.Name;
                        eventAc.UserId = events.User.Id;
                        eventAc.UserName = events.User.EmailId;
                        eventAc.Description = events.Description;
                        eventAc.EndTime = events.EndDateTime;

                        eventAc.Location = events.Location;
                        var eventSection =
                            _eventJoinDataRepository.FirstOrDefault(
                                x => x.EventId == events.Id && x.UserId == user.Id);
                        if (eventSection == null)
                        {
                            eventAc.EventSelect = 0;
                        }
                        else
                        {
                            eventAc.EventSelect = (int) eventSection.Status;
                        }
                        eventAc.GoingCount = events.EventJoin.Where(x => x.Status == EventJoinEnum.Going && x.EventId == events.Id).Count();
                        eventAc.MayBe = events.EventJoin.Where(x => x.Status == EventJoinEnum.MayBe && x.EventId == events.Id).Count();
                        //eventAc.NotGoing = events.EventJoin.GroupBy(x => x.Status == EventJoinEnum.NotGoing && x.EventId == events.Id).Count();
                       // googleAc.Location = events.Location;
                        googleAc.latitude = events.Latitude;
                        googleAc.longitude = events.Longitude;

                     
                        googleCollection.Add(googleAc);
                        eventAc.GoogleAc = googleCollection;
                        eventCollection.Add(eventAc);
                       
                    }
                    else
                    {
                        _eventDataRepository.Delete(events.Id);
                        _eventDataRepository.SaveChanges();
                    }
                   
                }
                return eventCollection;
            }
            catch (Exception)
            {
                
                throw;
            }
           // return null;
        }

        #region Create New Event

        [HttpPost]
        [Route("api/Event/createNewEvent")]
        public IHttpActionResult CreateNewEvent(EventAc events)
        {
            try
            {
                var currentuser = HttpContext.Current.User.Identity.Name;

                _eventRepository.CreateNewEvent(events, currentuser);
                return Ok();
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        #endregion

        [HttpPost]
        [Route("api/Event/eventChange/{eventJoinAc}/{eventId}")]
        public IHttpActionResult EventChange(EventJoinEnum eventJoinAc, int eventId)
        {
            try
            {
                var currentUser = HttpContext.Current.User.Identity.Name;
                var events = _eventRepository.EventChange(eventJoinAc,eventId,currentUser);

                var currentEvent = _eventDataRepository.FirstOrDefault(x => x.Id == events.EventId);
             
                        var eventAc = new EventAc();
                        eventAc.EventId = currentEvent.Id;
                        eventAc.Name = currentEvent.Name;
                        eventAc.UserId = currentEvent.User.Id;
                        eventAc.UserName = currentEvent.User.EmailId;
                        eventAc.Description = currentEvent.Description;
                        eventAc.EndTime = currentEvent.EndDateTime;
                        eventAc.Location = currentEvent.Location;
                        eventAc.GoingCount = currentEvent.EventJoin.Where(x => x.Status == EventJoinEnum.Going && x.EventId == currentEvent.Id).Count();
                        eventAc.MayBe = currentEvent.EventJoin.Where(x => x.Status == EventJoinEnum.MayBe && x.EventId == currentEvent.Id).Count();
                        eventAc.EventSelect = (int)events.Status;




                        return Ok(eventAc);
            }
            catch (Exception)
            {
                
                throw;
            }

      
        }
    }
}