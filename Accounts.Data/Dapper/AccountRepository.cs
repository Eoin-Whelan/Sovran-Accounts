﻿using Accounts.Data.Contracts;
using Accounts.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Sovran.Logger;
using System.Data;

namespace Accounts.Data.Dapper
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;
        private readonly ISovranLogger _logger;

        public AccountRepository(IConfiguration config, ISovranLogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public int AddMerchant(MerchantAccount newAccount)
        {
            var conn = GetConnection();
            try
            {
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
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<MerchantAccount>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

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

        public async Task<int> GetByUsername(string username)
        {
            var conn = GetConnection();
            var query = "Select MerchantId FROM Merchants WHERE Username = @username";
            int result = 0;

            try
            {

                using (conn)
                {
                    conn.Open();
                    result = await conn.QuerySingleOrDefaultAsync<int>(query, new { Username = username });

                    if (result == 0)
                    {
                        throw new Exception("No matching username found.");
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public async Task<int> AttemptLogin(string username, string password)
        {

            var procedure = "login";
            var values = new { userlogin = username, userpw = password};
            var conn = GetConnection();
            try
            {
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
                return -1;
            }
        }

        public Task<int> UpdateAsync(MerchantAccount entity)
        {
            throw new NotImplementedException();
        }

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
                    throw new Exception(ex.Message);
                }
            }
        }

        private MySqlConnection GetConnection()
        {
            string conn = _config.GetConnectionString("aws");
            _logger.LogActivity(conn);
            return new MySqlConnection(_config.GetConnectionString("aws"));
        }
    }
}
