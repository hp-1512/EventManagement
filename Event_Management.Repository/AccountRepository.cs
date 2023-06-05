using Dapper;
using Event_Management.Entities.Context;
using Event_Management.Entities.Models;
using Event_Management.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DapperContext _context;

        public AccountRepository (DapperContext context)
        {
            _context = context;
        }
        #region Unique User Name Validation
        public async Task<User> GetUserName(string UserName)
        {
            var query = "SELECT username FROM tblUser WHERE isActive = 1 AND username = @UserName";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { UserName });
                return user;
            }
        }
        #endregion

        #region Password Encryption
        public string GetHashPassword(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        #endregion

        #region Create New User
        public async Task<long> CreateUser(User userModel)
        {
            var query = "INSERT INTO tblUser (first_name, last_name, email,password,username,isRegSuccess,isActive) VALUES (@FirstName, @LastName, @Email, @Password,@UserName,@IsRegSuccess,@IsActive)"+
                "SELECT CAST(SCOPE_IDENTITY() as bigint)";

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", userModel.FirstName, DbType.String);
            parameters.Add("@LastName", userModel.LastName, DbType.String);
            parameters.Add("@Email", userModel.Email, DbType.String);
            parameters.Add("@Password", userModel.Password, DbType.String);
            parameters.Add("@UserName", userModel.UserName, DbType.String);
            parameters.Add("@IsRegSuccess", userModel.IsRegSuccess, DbType.Int32);
            parameters.Add("@IsActive", userModel.IsActive, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<long>(query, parameters);
                return id;
            }
            
        }
        #endregion

        #region Email Confirmation Operations
        public async Task StoreToken(RegistrationStatus tokenModel)
        {
            var query = "INSERT INTO tblRegistrationStatus (user_id,email,token) VALUES(@UserId,@Email,@Token)";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", tokenModel.UserId, DbType.Int64);
            parameters.Add("@Email", tokenModel.Email, DbType.String);
            parameters.Add("@Token", tokenModel.Token, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.QuerySingleAsync(query, parameters);

            }
        }
        #endregion
    }
}
