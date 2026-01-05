using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace BusinessLogic
{
    public class Authentication
    {
        public static bool CheckAuthentication(string UserName,string Token,string connectionString)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand("GeosApiCheckAuthentication", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_UserName", UserName);
                command.Parameters.AddWithValue("_ApiKey", Token);
                object count = command.ExecuteScalar();
                return count == null ? false: true;
               
            }
        }
    }
}
