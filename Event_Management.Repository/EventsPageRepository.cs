using Dapper;
using Event_Management.Entities.Context;
using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Repository
{
    public class EventsPageRepository : IEventsPageRepository
    {
        private readonly DapperContext _context;

        public EventsPageRepository(DapperContext dapperContext)
        {
            _context = dapperContext;
        }
        public List<Event> EventsDataList()
        {
            var query = "SELECT e.event_id AS EventId,title AS EventTitle,[path] AS EventImage,first_name +' '+ last_name AS Creator,vanue AS Vanue, start_date AS StartDate, end_date AS EndDate,description AS EventDesc " +
                "FROM tblEvent e " +
                "JOIN tblUser u ON e.created_by = u.user_id " +
                "LEFT JOIN(select * from(SELECT *, ROW_NUMBER() over(partition by  event_id order by event_id) as row_num  FROM tblEventMedia r) temp Where row_num = 1) as em on e.event_id = em.event_id";
            using (var connection = _context.CreateConnection())
            {
                try
                {

                var result = connection.Query<Event>(query).AsList();
                return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }
    }
}
