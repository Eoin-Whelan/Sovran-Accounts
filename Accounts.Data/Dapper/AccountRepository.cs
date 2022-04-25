using Accounts.Data.Contracts;
using Accounts.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Sovran.Logger;
using System.Data;
using System.Reflection;

namespace Accounts.Data.Dapper
{
    /// <summary>
    /// AccountRepository is the main data access layer for Sovran's MySQL database.
    /// <br></br><br></br>
    /// It uses a Sovran Logger and IConfiguration file for logging and Config file access, respectively.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;
        private readonly ISovranLogger _logger;

        public AccountRepository(IConfiguration config, ISovranLogger logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Method takes new MerchantAccount object, passing in all new account values to RDS/SQL table.
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        public int AddMerchant(MerchantAccount newAccount)
        {
            try
            {
                var conn = GetConnection();
                using (conn)
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO Merchants(MerchantId,
                                                                Username, 
                                                                StripeId, 
                                                                CatalogId, 
                                                                MerchantEmail, 
                                                                PhoneNumber, 
                                                                Password, 
                                                                FirstName, 
                                                                Surname, 
                                                                DateOfBirth, 
                                                                MerchantAddressLineOne, 
                                                                MerchantAddressLineTwo, 
                                                                City, 
                                                                County, 
                                                                Postcode, 
                                                                SupportPhone, 
                                                                SupportEmail) 
                                                       VALUES (@MerchantId, 
                                                               @Username, 
                                                               @StripeId, 
                                                               @CatalogId, 
                                                               @MerchantEmail, 
                                                               @PhoneNumber, 
                                                               @Password, 
                                                               @FirstName, 
                                                               @Surname, 
                                                               @DateOfBirth, 
                                                               @MerchantAddressLineOne, 
                                                               @MerchantAddressLineTwo, 
                                                               @City, 
                                                               @County, 
                                                               @Postcode, 
                                                               @SupportPhone, 
                                                               @SupportEmail)";
                    var result = conn.Execute(insertQuery, newAccount);
                    _logger.LogActivity(Assembly.GetExecutingAssembly().FullName + "Insertion result : " + result);
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(Assembly.GetExecutingAssembly().FullName + "Skipping doesExistCheck : " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// GetByIdAsync returns a merchant account by their id. Unused in any use case as of 22/04/2022
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Merchant Account Object</returns>
        public async Task<MerchantAccount> GetByIdAsync(int id)
        {
            var conn = GetConnection();
            var query = "Select * FROM Merchants WHERE MerchantId = @id";
            
            try
            {
                using (conn)
                {
                    conn.Open();
                    var result = await conn.QuerySingleOrDefaultAsync<MerchantAccount>(query, new { Id = id });

                    if(result == null)
                    {
                        throw new Exception("No matching Id found.");
                    }
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        /// <summary>
        /// GetByUsername returns a merchant account by their id. Used in update account use case.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Merchant Account Object</returns>
        public async Task<MerchantAccount> GetByUsername(string username)
        {
            var conn = GetConnection();
            var query = "Select MerchantId FROM Merchants WHERE Username = @username";
            MerchantAccount userAccount = null;
            try
            {

                using (conn)
                {
                    conn.Open();
                    userAccount = await conn.QuerySingleOrDefaultAsync<MerchantAccount>(query, new { Username = username });

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
            return userAccount;
        }

        /// <summary>
        /// Login Attempt
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<int> AttemptLogin(string username, string password)
        {
            _logger.LogActivity("New login attemptL " + username);
            try
            {
                var procedure = "login";
                var values = new { userlogin = username, userpw = password };
                var conn = GetConnection();
                using (conn)
                {
                    conn.Open();
                    var result = await conn.QueryFirstAsync<int>(procedure, values, commandType: CommandType.StoredProcedure);
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Login attempt. Ex: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Updates a given merchant's details asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<int> UpdateAsync(MerchantAccount updatedAccount)
        {
            try
            {
                _logger.LogActivity("Update flow initiated for " + updatedAccount.Username);
                var conn = GetConnection();
                using (conn)
                {
                    conn.Open();
                    string updateQuery = @"UPDATE Merchants
                                                            SET  
                                                                MerchantEmail = @MerchantEmail, 
                                                                PhoneNumber = @PhoneNumber, 
                                                                Password = @Password, 
                                                                FirstName = @FirstName, 
                                                                Surname = @Surname, 
                                                                DateOfBirth = DateOfBirth, 
                                                                MerchantAddressLineOne = @MerchantAddressLineOne, 
                                                                MerchantAddressLineTwo = @MerchantAddressLineTwo, 
                                                                City = @City, 
                                                                County = @County, 
                                                                Postcode = @Postcode, 
                                                                SupportPhone = @SupportPhone, 
                                                                SupportEmail = @SupportEmail
                                                                WHERE MerchantId=@MerchantId";
                    var result = await conn.ExecuteAsync(updateQuery, updatedAccount);
                    _logger.LogActivity(Assembly.GetExecutingAssembly().FullName + " Update result : " + result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(Assembly.GetExecutingAssembly().FullName + "Update Error: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Method verifies is a given username already exists in database.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>True/False value</returns>
        public bool DoesExist(string username)
        {
            try
            {
                var conn = GetConnection();
                using (conn)
                {
                    var parameters = new { UserName = username };
                    var sql = "SELECT Username FROM Merchants where Username = @UserName";
                    var result = conn.Query(sql, parameters).Any();
                    return result;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Pulls stripe id by a given username. Used in payment flow.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<string> GetStripeAccount(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception(username + " does not exist.");
            }
            else
            {
                try
                {
                    _logger.LogActivity("Pulling stripe Id for " + username);

                    var conn = GetConnection();
                    using (conn)
                    {
                        var parameters = new { UserName = username };
                        var sql = "SELECT StripeId FROM Merchants where Username = @UserName";
                        var response = conn.QueryFirstAsync<string>(sql, parameters);
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error retrieving stripe Id " + username);

                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Private method to generate SQL connection. Used in all implemented methods.
        /// </summary>
        /// <returns>Valid SQL connection pulled from config files.</returns>
        private MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(_config.GetConnectionString("aws"));
            }
            catch(Exception e)
            {
                _logger.LogError("Exception encountered in " + Assembly.GetExecutingAssembly().FullName + ":" + e.Message);
            }
            return null;
        }
    }
}
