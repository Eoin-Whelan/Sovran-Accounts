using MySql.Data.MySqlClient;
using System.Data;

namespace Accounts.Business.Login
{
    public class LoginRequestValidator : ILoginRequestValidator
    {
        public LoginRequestValidator()
        {

        }

        public bool AttemptLogin()
        {
            var connectionString = @"Server=sovran-accounts.cihpzkqwv66o.eu-west-1.rds.amazonaws.com;Database=sovran_accounts;User=notary;Password=B4rth0l0m3w!;";
            using (var conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("LOGIN", conn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("username", "NeoGThreads");
                cmd.Parameters.AddWithValue("pwd", "geronimo205!");

                try
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dt);
                    Console.WriteLine(dt.ToString());

                    DataRow[] validUser = dt.Select("username = 'NeoGThreads' AND password='geronimo205!?'");
                    if (validUser.Length == 0)
                    {
                        conn.Close();
                        return false;
                    }
                    conn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
                conn.Close();
                return false;
            };

        }
    }
}
