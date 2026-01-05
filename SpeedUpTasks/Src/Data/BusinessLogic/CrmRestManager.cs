using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using Emdep.Geos.Data.Common;
namespace Emdep.Geos.Data.BusinessLogic
{
    public class CrmRestManager
    {
        public List<PeopleDetails> GetPeoples(string connectionstring)
        {
            List<PeopleDetails> peoples = new List<PeopleDetails>();
            
            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(connectionstring))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("people_GetAllPeoples", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;

                using (MySqlDataReader reader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        PeopleDetails people = new PeopleDetails();
                        people.IdPerson = Convert.ToInt32(reader["IdPerson"].ToString());
                        people.Name = reader["Name"].ToString();
                        people.Surname = reader["Surname"].ToString();
                        people.Email = reader["Email"].ToString();

                        peoples.Add(people);
                    }
                }
            }

            return peoples;
        }
    }
}
