using Event_Management.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Repository.Interface
{
    public interface IEventsPageRepository
    {
        public List<Event>? EventsDataList(long userId);
        public bool CreateEventDb(EventCreation eventObj, List<IFormFile> eventImages, string[] preloaded);
        public Event? Participate(long userId, long eventId);
        public List<Event>? ParticipatedEventsList(long userId);
        public List<Event>? CreatedEventsList(long userId);
    }
}
