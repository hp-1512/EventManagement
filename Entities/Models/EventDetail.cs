using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Entities.Models
{
    public class Event
    {
        public long EventId { get; set; }
        public string? EventImage { get; set; }
        public string? EventTitle { get; set; }
        public string? Creator { get; set; }
        public string? Vanue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? EventDesc { get; set; }
        public int? MaxPrticipant { get; set; }
        public long? ParticipatedUser { get; set; }
    }
    public class EventsDetailList
    {
        public IEnumerable<Event>? ListOfEvents { get; set; }
    }
   
}
