using Dapper;
using Event_Management.Entities.Context;
using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
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
        public List<Event>? EventsDataList(long userId)
        {
            var query = "SELECT e.event_id AS EventId,p.user_id as ParticipatedUser,title AS EventTitle,[path] AS EventImage,first_name +' '+ last_name AS Creator,vanue AS Vanue, start_date AS StartDate, end_date AS EndDate,description AS EventDesc,max_participant AS MaxPrticipant " +
                "FROM tblEvent e " +
                "JOIN tblUser u ON e.created_by = u.user_id " +
                "LEFT JOIN(select * from(SELECT *, ROW_NUMBER() OVER(partition by  event_id order by event_id) AS row_num  FROM tblEventMedia r) temp WHERE row_num = 1) AS em ON e.event_id = em.event_id " +
                "LEFT JOIN tblParticipatedEvents AS p ON p.event_id = e.event_id AND p.user_id =@userId " +
                "WHERE e.deleted_at IS NULL";
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = connection.Query<Event>(query, new {userId = userId}).AsList();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }
        
        public Event? Participate(long userId, long eventId)
        {
            try
            {
                var ifParticipated = "SELECT event_id FROM tblParticipatedEvents WHERE user_id = @userId AND event_id = @eventId";

                var query = "INSERT INTO tblParticipatedEvents(user_id,status,event_id) VALUES(@userId,1,@eventId);" +
                    "UPDATE tblEvent SET max_participant = max_participant - 1 WHERE event_id = @eventId;" +
                    "SELECT title AS EventTitle, start_date AS StartDate,end_date AS EndDate FROM tblEvent WHERE event_id = @eventId";
                var parameters = new DynamicParameters();
                parameters.Add("@userId", userId, DbType.Int64);
                parameters.Add("@eventId", eventId, DbType.Int64);
                using (var connection = _context.CreateConnection())
                {
                    var ifParticipatedResult = connection.Query(ifParticipated, parameters);
                    if (!ifParticipatedResult.Any())
                    {
                        var eventObj = connection.QuerySingle<Event>(query, parameters);
                        return eventObj;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                //return null;
                throw;

            }
        }

        public void DeleteImage(EventImages missionMedia)
        {
            string missImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EventMedia/", missionMedia.Path);

            if (System.IO.File.Exists(missImagePath))
            {
                System.IO.File.Delete(missImagePath);
            }
            var deleteImage = "DELETE FROM  tblEventMedia WHERE path = @path";
            using (var connection = _context.CreateConnection())
            {
                var user = connection.Execute(deleteImage, new { path = missionMedia.Path});
            }
        }
        public void SaveEventImages(long eventId, List<EventImages> eventImagesList, List<IFormFile> missionImageFiles, string[] preloadedmissimage)
        {
            foreach (var missionMedia in eventImagesList)
            {

                if (preloadedmissimage.Length < 1)
                {
                    DeleteImage(missionMedia);
                }
                else
                {
                    bool flag = false;
                    for (int i = 0; i < preloadedmissimage.Length; i++)
                    {
                        string imgName = preloadedmissimage[i][14..];

                        if (imgName.Equals(missionMedia.Path))
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        DeleteImage(missionMedia);
                    }
                }

            }

            if (missionImageFiles?.Count > 0)
            {
                foreach (var image in missionImageFiles)
                {
                    string imgExt = Path.GetExtension(image.FileName);
                    if (imgExt == ".jpg" || imgExt == ".png" || imgExt == ".jpeg")
                    {
                        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string imgSaveTo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EventMedia/" + imageName);
                        using (FileStream stream = new(imgSaveTo, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }
                        // logic to save image in db
                        var queryEventMedia = "INSERT INTO tblEventMedia([event_id],[path]) VALUES (@eventId,@path)";
                        var parameters = new DynamicParameters();
                        parameters.Add("@eventId", eventId, DbType.Int64);
                        parameters.Add("@path", imageName, DbType.String);
                        using (var connection = _context.CreateConnection())
                        {
                            var imageInsert = connection.Execute(queryEventMedia, parameters);
                        }
                    }
                }
            }
        }
        public bool CreateEventDb(EventCreation eventObj, List<IFormFile> eventImages, string[] preloaded)
        {
            try
            {
                var queryEvent = "INSERT INTO tblEvent([title],[description],[note],[start_date],[end_date],[start_time],[end_time],[vanue],[created_by],[max_participant])" +
                        "VALUES(@eventTitle,@eventDesc,@note,@startDate,@endDate,@startTime,@endTime,@vanue,@createdBy,@maxParticipant)" +
                        "SELECT CAST(SCOPE_IDENTITY() as bigint)";

                var parameters = new DynamicParameters();
                parameters.Add("@eventTitle", eventObj.EventTitle, DbType.String);
                parameters.Add("@eventDesc", eventObj.EventDesc, DbType.String);
                parameters.Add("@note", eventObj.Note, DbType.String);
                parameters.Add("@startDate", eventObj.StartDate, DbType.DateTime);
                parameters.Add("@endDate", eventObj.EndDate, DbType.DateTime);
                parameters.Add("@startTime", eventObj.StartTime, DbType.Time);
                parameters.Add("@endTime", eventObj.EndTime, DbType.Time);
                parameters.Add("@vanue", eventObj.Vanue, DbType.String);
                parameters.Add("@createdBy", eventObj.CreatedBy, DbType.Int64);
                parameters.Add("@maxParticipant", eventObj.MaxParticipants, DbType.Int32);


                var queryEventImages = "Select * from tblEventMedia Where event_id = @eventId";
                using (var connection = _context.CreateConnection())
                {
                    var eventId = connection.QuerySingle<long>(queryEvent, parameters);
                    var eventImagesList = connection.Query<EventImages>(queryEventImages, new { eventId }).AsList();
                    SaveEventImages(eventId, eventImagesList, eventImages, preloaded);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Event>? ParticipatedEventsList(long userId)
        {
            var query = "SELECT e.event_id AS EventId,p.user_id as ParticipatedUser,title AS EventTitle,[path] AS EventImage,first_name +' '+ last_name AS Creator,vanue AS Vanue, start_date AS StartDate, end_date AS EndDate,description AS EventDesc,max_participant AS MaxPrticipant " +
                "FROM tblEvent e " +
                "JOIN tblUser u ON e.created_by = u.user_id " +
                "LEFT JOIN(select * from(SELECT *, ROW_NUMBER() OVER(partition by  event_id order by event_id) AS row_num  FROM tblEventMedia r) temp WHERE row_num = 1) AS em ON e.event_id = em.event_id " +
                "JOIN tblParticipatedEvents AS p ON p.event_id = e.event_id AND p.user_id =@userId " +
                "WHERE e.deleted_at IS NULL";
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = connection.Query<Event>(query, new { userId = userId }).AsList();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }
        public List<Event>? CreatedEventsList(long userId)
        {
            var query = "SELECT e.event_id AS EventId,u.user_id as ParticipatedUser,title AS EventTitle,[path] AS EventImage,first_name +' '+ last_name AS Creator,vanue AS Vanue, start_date AS StartDate, end_date AS EndDate,description AS EventDesc,max_participant AS MaxPrticipant " +
                "FROM tblEvent e " +
                "JOIN tblUser u ON e.created_by = u.user_id AND u.user_id = @userId " +
                "LEFT JOIN(select * from(SELECT *, ROW_NUMBER() OVER(partition by  event_id order by event_id) AS row_num  FROM tblEventMedia r) temp WHERE row_num = 1) AS em ON e.event_id = em.event_id " +
                "WHERE e.deleted_at IS NULL";
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var result = connection.Query<Event>(query, new { userId }).AsList();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public EventUpdation GetEventForUpdate(long eventId)
        {
            var query = "SELECT title AS EventTitle,description AS EventDesc, note AS Note, vanue AS Vanue,start_date AS StartDate,end_date AS EndDate, start_time AS StartTime,end_time AS EndTime, username AS CreatedBy, max_participant AS MaxParticipants   FROM tblEvent AS event " +
                "JOIN tblUser AS usertbl ON event.created_by = usertbl.user_id  AND event_id = @eventId;" +
                "SELECT * FROM tblEventMedia WHERE event_id =  @eventId;";
            using(var connection = _context.CreateConnection())
            {
                var eventToBeUpdated =  connection.QueryMultiple(query, new { eventId });
                var eventData = eventToBeUpdated.ReadFirst<EventUpdation>();
                var eventMedia = eventToBeUpdated.Read<EventImages>().AsList();

                eventData.EventMedia = eventMedia;

                return eventData;
            }
        }
        public List<UpdateDeleteEventClass> UpdateEvent(EventUpdation eventToBeUpdate, List<IFormFile> eventImages, string[] preloaded)
        {
            try
            {
                var queryEvent = "UPDATE tblEvent SET " +
                    "[title] = @eventTitle, [description] = @eventDesc,[note] = @note,[start_date] = @startDate,[end_date] = @endDate, " +
                    "[start_time] = @startTime,[end_time] = @endTime ,[vanue] = @vanue,[max_participant] = @maxParticipant " +
                    "WHERE event_id = @eventId;" +
                    "SELECT * FROM tblEventMedia WHERE event_id = @eventId;" +
                    "SELECT email AS Email,title AS EventTitle,description AS EventDesc,start_date AS StartDate,end_date AS EndDate,vanue AS Vanue FROM tblParticipatedEvents AS p JOIN tblUser AS u ON p.user_id = u.user_id AND event_id = @eventId " +
                    "JOIN tblEvent as e On p.event_id = e.event_id";

                var parameters = new DynamicParameters();
                parameters.Add("@eventTitle", eventToBeUpdate.EventTitle, DbType.String);
                parameters.Add("@eventDesc", eventToBeUpdate.EventDesc, DbType.String);
                parameters.Add("@note", eventToBeUpdate.Note, DbType.String);
                parameters.Add("@startDate", eventToBeUpdate.StartDate, DbType.DateTime);
                parameters.Add("@endDate", eventToBeUpdate.EndDate, DbType.DateTime);
                parameters.Add("@startTime", eventToBeUpdate.StartTime, DbType.Time);
                parameters.Add("@endTime", eventToBeUpdate.EndTime, DbType.Time);
                parameters.Add("@vanue", eventToBeUpdate.Vanue, DbType.String);
                parameters.Add("@maxParticipant", eventToBeUpdate.MaxParticipants, DbType.Int32);
                parameters.Add("@eventId", eventToBeUpdate.EventId, DbType.Int64);

                using (var connection = _context.CreateConnection())
                {
                    var result = connection.QueryMultiple(queryEvent, parameters);
                    var eventImagesList = result.Read<EventImages>().AsList();
                    var eventMailingData = result.Read<UpdateDeleteEventClass>().AsList();
                    SaveEventImages(eventToBeUpdate.EventId, eventImagesList, eventImages, preloaded);
                return eventMailingData;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<UpdateDeleteEventClass> DeleteEvent(long eventId, string resasonToDelete)
        {
            var query = "UPDATE tblEvent SET deleted_at = GETDATE() WHERE event_id = @eventId;" +
                "SELECT email AS Email, title AS EventTitle FROM tblParticipatedEvents AS p " +
                "JOIN tblEvent AS e ON p.event_id = e.event_id AND e.event_id = @eventId " +
                "JOIN tblUser AS u ON u.user_id = p.user_id ;" +
                "SELECT * FROM tblEventMedia WHERE event_id = @eventId ;" +
                "DELETE FROM  tblEventMedia WHERE event_id = @eventId ;";
            using (var connection = _context.CreateConnection())
            {
                var result = connection.QueryMultiple(query, new { eventId });
                var participatedUsers = result.Read<UpdateDeleteEventClass>().AsList();
                var eventImages = result.Read<EventImages>().AsList();
                foreach(var image in eventImages)
                {
                    string eventImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EventMedia/", image.Path);

                    if (System.IO.File.Exists(eventImagePath))
                    {
                        System.IO.File.Delete(eventImagePath);
                    }
                }
                return participatedUsers;
                
            }
        }
    }
}
