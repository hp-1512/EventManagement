using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Entities.Models
{
    public class NotificationList
    {
        public long NotifId { get; set; }
        public string NotifMsg { get; set; }
        public int IsRead { get; set; }
    }
    public class Notification
    {
        public List<NotificationList> NotificationData { get; set; }
        public int NotificationCout { get; set; }
    }
}
