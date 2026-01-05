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

        public static Dictionary<int,string> DictonaryBusinessUnits = new Dictionary<int, string>();
        public static Dictionary<int,string> DictonarySources = new Dictionary<int, string>();
        public static Dictionary<Int64, string> DictonaryCarProjects = new Dictionary<Int64, string>();
        public static Dictionary<int, string> DictonaryCarOems = new Dictionary<int, string>();

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

        public static Dictionary<int,string> GetAllBusinessUnit()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("lookupvalues_GetlookupvaluesWithoutRestrictedBU", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idPermission", 22);
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DictonaryBusinessUnits.Add(Convert.ToInt32(dr["IdLookupValue"].ToString()), dr["Value"].ToString());
                        }
                    }
                   
                    return DictonaryBusinessUnits;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<int, string> GetAllSource()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("lookupvalues_GetlookupvaluesByKey", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_IdLookupKey", 4);
                    
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DictonarySources.Add(Convert.ToInt32(dr["IdLookupValue"].ToString()), dr["Value"].ToString());
                        }
                    }

                    return DictonarySources;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<Int64, string> GetAllCarProject()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("carProjects_GetAllCarProjects", conn);
                    command.CommandType = CommandType.StoredProcedure;
                   
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DictonaryCarProjects.Add(Convert.ToInt64(dr["IdCarProject"].ToString()), dr["ProjectName"].ToString());
                        }
                    }

                    return DictonaryCarProjects;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<int, string> GetAllCarOEM()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("caroems_GetCarOEM", conn);
                    command.CommandType = CommandType.StoredProcedure;
                  
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DictonaryCarOems.Add(Convert.ToInt32(dr["IdCarOEM"].ToString()), dr["Name"].ToString());
                        }
                    }

                    return DictonaryCarOems;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
              
    }
}
