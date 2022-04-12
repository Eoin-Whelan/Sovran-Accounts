﻿using Accounts.Data.Contracts;
using Accounts.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Accounts.Data.Dapper
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;

        public AccountRepository(IConfiguration config)
        {
            _config = config;
        }

        public string AddMerchant(MerchantAccount newAccount)
        {
            try
            {
                if (!DoesExist(newAccount.Username)){
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
                                                                MerchantAddressLineThree, 
                                                                County, 
                                                                Postcode, 
                                                                SupportPhone, 
                                                                SupportEmail,
                                                                ProfileImg) 
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
                                                               @MerchantAddressLineThree, 
                                                               @County, 
                                                               @Postcode, 
                                                               @SupportPhone, 
                                                               @SupportEmail,
                                                               @ProfileImg)";
                        var result = conn.Execute(insertQuery, newAccount);
                        if (result != 0)
                        {
                            return "Success";
                        }
                        else
                        {
                            return "Failure";
                        }
                    }
                }
                else
                {
                    return "Duplicate Username Found. Please try again.";
                }
            }
            catch (Exception ex)
            {
                return "Internal Error Occurred. Please contact system administrator @ EoinWhelanDev@Gmail.com";
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


        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(@"Server=sovran-accounts.cihpzkqwv66o.eu-west-1.rds.amazonaws.com;
                        Database=sovran_accounts;
                        User=notary;
                        Password=B4rth0l0m3w!;
                        Convert Zero Datetime=True;");
        }
    }
}
