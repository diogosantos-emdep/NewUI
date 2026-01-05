using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Entities;
using System.Data;

namespace BusinessLogic
{
    public static class CommonManager
    {
        public static string ConnString;
        public static Dictionary<string, string> DictonarySites = new Dictionary<string, string>();
        public static Dictionary<string, string> DictonarySitesWithIds = new Dictionary<string, string>();
        public static Dictionary<string, int> Dictonarycurrency = new Dictionary<string, int>();

        public static Dictionary<string, string> GetAllSites()
        {
            //Dictionary<string, string> DictonarySites = new Dictionary<string, string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("GeosApiGetAllSites", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (!DictonarySites.ContainsKey(dr["ShortName"].ToString()) &&
                                !DictonarySites.ContainsValue(dr["DatabaseIP"].ToString()))
                                DictonarySites.Add(dr["ShortName"].ToString(), dr["DatabaseIP"].ToString());
                            if(!DictonarySitesWithIds.ContainsKey(dr["ShortName"].ToString()))
                                DictonarySitesWithIds.Add(dr["ShortName"].ToString(), dr["idSite"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DictonarySites;
        }
        public static int GetIdCurrency(string Currency)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("currency_GetCurrencyByName", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Name", Currency);
                    int CurrencyID = Convert.ToInt16(command.ExecuteScalar());
                    return CurrencyID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
