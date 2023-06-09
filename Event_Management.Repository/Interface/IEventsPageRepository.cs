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
        public List<Event> EventsDataList();
        public bool CreateEventDb(EventCreation eventObj, List<IFormFile> eventImages, string[] preloaded);
    }
}
