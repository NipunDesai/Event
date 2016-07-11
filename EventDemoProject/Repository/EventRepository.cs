using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventDemoProject.DataRepsitory;
using EventDemoProject.Models.Enums;
using EventDemoProject.Models.Modules;
using EventDemoProject.Models.Modules.ApplicationClass;
using EventDemoProject.Models.Modules.Global;

namespace EventDemoProject.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly IDataRepository<Event> _eventDataRepository;
        private readonly IDataRepository<UserDetails> _userDataRepository;
        private readonly IDataRepository<EventJoin> _eventJoinDataRepository;

        public EventRepository(IDataRepository<Event> eventDataRepository, IDataRepository<UserDetails> userDataRepository, IDataRepository<EventJoin> eventJoinDataRepository)
        {
            _eventDataRepository = eventDataRepository;
            _userDataRepository = userDataRepository;
            _eventJoinDataRepository = eventJoinDataRepository;
        }
       

        public IEnumerable<Event> GetAllEventList()
        {
            var contentList = _eventDataRepository.GetAll().ToList();
            return contentList;
        }

        public void CreateNewEvent(EventAc events, string username)
        {
            
            try
            {
              
                var user = _userDataRepository.FirstOrDefault(x => x.EmailId == username);
                var eventContent = new Event
                {
                    UserId = user.Id,
                    Name = events.Name,
                    Description = events.Description,
                    ImageGuidUrl = events.ImageGuidUrl,
                    EndDateTime = DateTimeOffset.Parse(events.EndDateTime).UtcDateTime,
                    Location = events.Location,
                    Latitude = events.Latitude,
                    Longitude = events.Longitude,
                    CreatedDateTime = DateTime.UtcNow
                };
                _eventDataRepository.Add(eventContent);
                _eventDataRepository.SaveChanges();
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        public EventJoin EventChange(EventJoinEnum status, int eventId,string userName)
        {
            try
            {
                var currentUser = _userDataRepository.FirstOrDefault(x => x.EmailId == userName);
                var currentEvent = _eventJoinDataRepository.FirstOrDefault(x => x.EventId == eventId && x.UserId == currentUser.Id);
                if (currentEvent == null)
                {
                    var eventJoints = new EventJoin
                    {
                        CreatedDateTime = DateTime.UtcNow,
                        UserId = currentUser.Id,
                        EventId = eventId,
                        Status = status,
                        
                    };
                    _eventJoinDataRepository.Add(eventJoints);
                    _eventJoinDataRepository.SaveChanges();
                    return eventJoints;
                }
                else
                {
                    if (status == EventJoinEnum.Going)
                    {
                        currentEvent.CreatedDateTime = DateTime.UtcNow;
                        currentEvent.Status = EventJoinEnum.Going;
                        _eventJoinDataRepository.Update(currentEvent);
                                     
                    }
                    else if (status == EventJoinEnum.MayBe)
                    {
                        currentEvent.CreatedDateTime = DateTime.UtcNow;
                        currentEvent.Status = EventJoinEnum.MayBe;
                        _eventJoinDataRepository.Update(currentEvent);
                    }
                    else
                    {
                        currentEvent.CreatedDateTime = DateTime.UtcNow;
                        currentEvent.Status = EventJoinEnum.NotGoing;
                        _eventJoinDataRepository.Update(currentEvent);
                   }
                    _eventJoinDataRepository.SaveChanges();
                    return currentEvent;
                }
               

            }
            catch (Exception)
            {
                
                throw;
            }
         
        }

        public void Dispose()
        {
            _eventDataRepository.Dispose();
        }
    }
}