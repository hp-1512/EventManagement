﻿using Dapper;
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
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperContext _context;

        public DashboardRepository(DapperContext context)
        {
            _context = context;
        }
        public EventsStatusData EventsDataForPieChart()
        {
            var query = "SELECT COUNT(event_id) AS CompletedEvents FROM tblEvent WHERE CONVERT(DATE, end_date) < CONVERT(DATE, GETDATE()); " +
                "SELECT COUNT(event_id) AS OngoingEvents FROM tblEvent WHERE CONVERT(DATE, start_date) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, end_date) >= CONVERT(DATE, GETDATE()); " +
                "SELECT COUNT(event_id) AS UpcomingEvents FROM tblEvent WHERE CONVERT(DATE, start_date) > CONVERT(DATE, GETDATE()) ";
            using (var connection = _context.CreateConnection())
            {

                using (var multi = connection.QueryMultiple(query))
                {
                    var eventsStatusData = new EventsStatusData();

                    eventsStatusData.CompletedEvents = multi.ReadSingleOrDefault<int>();
                    eventsStatusData.OngoingEvents = multi.ReadSingleOrDefault<int>();
                    eventsStatusData.UpcomingEvents = multi.ReadSingleOrDefault<int>();

                    return eventsStatusData;
                }
            }
        }
        public UserStatusData UsersDataForPieChart()
        {
            var query = "SELECT SUM(CASE WHEN isActive = 1 THEN 1 ELSE 0 END) AS ActiveUsers," +
                "SUM(CASE WHEN isActive = 0 THEN 1 ELSE 0 END) AS InactiveUsers FROM tblUser";
            using (var connection = _context.CreateConnection())
            {

                using (var multi = connection.QueryMultiple(query))
                {
                    var userStatusData = new UserStatusData();

                    var results = multi.Read();
                    userStatusData.ActiveUsers = results.Single().ActiveUsers;
                    userStatusData.InactiveUsers = results.Single().InactiveUsers;

                    return userStatusData;
                }
            }
        }
        public List<AnnualEventsData> AnnualEventsData()
        {
            var query = "SELECT DATEPART(MONTH,start_date) AS Month, COUNT(event_id) AS TotalEvents FROM tblEvent GROUP BY DATEPART(MONTH,start_date)";
            using (var connection = _context.CreateConnection())
            {

                var events = connection.Query<AnnualEventsData>(query).AsList();
                return events;
                
            }
        }
    }
}