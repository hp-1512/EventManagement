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

        #region Unique User Name, Email Validation and Active User validation for Login
        public async Task<User> GetUserName(string userName)
        {
            var query = "SELECT username FROM tblUser WHERE isActive = 1 AND username = @UserName";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { userName });
                return user;
            }
        }
        public async Task<User> GetEmail(string email)
        {
            var query = "SELECT email FROM tblUser WHERE email = @email";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { email });
                return user;
            }
        }

        public async Task<User> IfActiveUser(string userName, string password)
        {
            password = GetHashPassword(password);
            var query = "SELECT username,email,password FROM tblUser WHERE isActive = 1 AND username = @UserName AND password = @Password";

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, DbType.String);
            parameters.Add("@Password",password,DbType.String);

            using (var connection = _context.CreateConnection())
            {
                 var user = await connection.QuerySingleOrDefaultAsync<User>(query, parameters);
                
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
        //Storing Token When Register Button hit
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

        //Fetching and Validating the token for the user While Verifying Email
        public async Task<RegistrationStatus> GetRegistrationToken(string email, string token)
        {
            var query = "SELECT user_id,email,token FROM tblRegistrationStatus WHERE email = @email AND token = @token AND deleted_at IS NULL";

            using (var connection = _context.CreateConnection())
            {
                var regUser = await connection.QuerySingleOrDefaultAsync<RegistrationStatus>(query, new { email, token });
                return regUser;
            }
        }

        //After Successful Verification Updating Flag in Database for Successfull Registration and performing Soft delete for token
        public async Task ApproveRegStatus(string email)
        {
            var query = "UPDATE tblUser SET isRegSuccess = @IsRegSuccess,isActive = @IsActive WHERE email = @Email" +" "+
                 "UPDATE tblRegistrationStatus SET deleted_at = @CurrentDate WHERE email = @Email";

            var parameters = new DynamicParameters();
            parameters.Add("@IsRegSuccess",1, DbType.Int32);
            parameters.Add("@IsActive",0, DbType.Int32);
            parameters.Add("@Email", email, DbType.String);
            parameters.Add("@CurrentDate", DateTime.Now, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                try
                {

                await connection.QueryMultipleAsync(query, parameters);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }

            }
        }
        #endregion
    }
}
