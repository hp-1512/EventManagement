using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Entities.Models
{
    public class EventsStatusData
    {
        public int CompletedEvents { get; set; }
        public int OngoingEvents { get; set; }
        public int UpcomingEvents { get; set; }
    }
    public class UserStatusData
    {
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
    }
    public class AnnualEventsData
    {
        public string? Month { get; set; }
        public int TotalEvents { get; set; }
    }
}
