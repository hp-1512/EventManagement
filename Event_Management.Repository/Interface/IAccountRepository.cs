using Event_Management.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Repository.Interface
{
    public interface IAccountRepository
    {
        public Task<User> GetUserName(string userName);
        public Task<User> GetEmail(string email);
        public Task<User> IfActiveUser(string userName,string password);
        public  string GetHashPassword(string text);
        public Task<long> CreateUser(User userModel);
        public Task StoreToken(RegistrationStatus tokenModel);
        public Task<RegistrationStatus> GetRegistrationToken(string token, string email);
        public Task ApproveRegStatus(string email);
    }
}
