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
        public Task<User> GetUserName(string UserName);
        public Task<long> CreateUser(User userModel);
        public  string GetHashPassword(string text);
        public Task StoreToken(RegistrationStatus tokenModel);
    }
}
