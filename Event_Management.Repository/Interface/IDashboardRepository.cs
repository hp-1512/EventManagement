using Event_Management.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Repository.Interface
{
    public interface IDashboardRepository
    {
        public EventsStatusData EventsDataForPieChart();
        public UserStatusData UsersDataForPieChart();
        public List<AnnualEventsData> AnnualEventsData();
        public User GetThisUser(string email);
        public Notification Notifications(long userId);
        public bool UpdateReadFlagNotification(long notiId);
        public bool ClearNotification(long userId);

    }
}
