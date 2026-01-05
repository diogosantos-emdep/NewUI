using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.DataAccess;
using MySql.Data.MySqlClient;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Emdep.Geos.Data.BusinessLogic
{

    public class PLMManager 
    {
        public List<BasePrice> GetBasePricesByYear(string PLMConnectionString)
        {
            List<BasePrice> basePrices = new List<BasePrice>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PLMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_GetBasePriceListByYear", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            BasePrice basePrice = new BasePrice();
                            basePrice.IdBasePriceList = Convert.ToUInt64(reader["IdBasePriceList"]);

                            if (reader["Code"] != DBNull.Value)
                            {
                                basePrice.Code = Convert.ToString(reader["Code"]);
                            }
                            if (reader["Name"] != DBNull.Value)
                            {
                                basePrice.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                basePrice.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["ExpiryDate"] != DBNull.Value)
                            {
                                basePrice.ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]);
                            }
                            if (reader["IdStatus"] != DBNull.Value)
                            {
                                basePrice.IdStatus = Convert.ToUInt32(reader["IdStatus"]);
                                basePrice.Status = new LookupValue();
                                basePrice.Status.IdLookupValue = Convert.ToInt32(reader["IdStatus"]);
                                if (reader["Status"] != DBNull.Value)
                                {
                                    basePrice.Status.Value = Convert.ToString(reader["Status"]);
                                }
                            }
                            if (reader["CreatedIn"] != DBNull.Value)
                            {
                                basePrice.CreationDate = Convert.ToDateTime(reader["CreatedIn"]);
                            }
                            if (reader["LastUpdate"] != DBNull.Value)
                            {
                                basePrice.LastUpdated = Convert.ToDateTime(reader["LastUpdate"]);
                            }
                            if (reader["IdCurrency"] != DBNull.Value)
                            {
                                basePrice.IdCurrency = Convert.ToByte(reader["IdCurrency"]);
                                basePrice.Currency = new Currency();
                                basePrice.Currency.IdCurrency = Convert.ToByte(reader["IdCurrency"]);
                                if (reader["Currency"] != DBNull.Value)
                                {
                                    basePrice.Currency.Name = Convert.ToString(reader["Currency"]);
                                }
                            }
                            if (reader["PVPCurrency"] != DBNull.Value)
                            {
                                basePrice.PVPCurrencies = Convert.ToString(reader["PVPCurrency"]);
                            }
                            if (reader["Plants"] != DBNull.Value)
                            {
                                basePrice.Plants = Convert.ToString(reader["Plants"]);
                            }
                            if (reader["ArticleCount"] != DBNull.Value)
                            {
                                if(Convert.ToInt32(reader["ArticleCount"])>0)
                                    basePrice.ArticleCount = Convert.ToInt32(reader["ArticleCount"]);
                            }
                            if (reader["ModuleCount"] != DBNull.Value)
                            {
                                if (Convert.ToInt32(reader["ModuleCount"]) > 0)
                                    basePrice.ModuleCount = Convert.ToInt32(reader["ModuleCount"]);
                            }
                            if (reader["DetectionCount"] != DBNull.Value)
                            {
                                if (Convert.ToInt32(reader["DetectionCount"]) > 0)
                                    basePrice.DetectionCount = Convert.ToInt32(reader["DetectionCount"]);
                            }
                            basePrices.Add(basePrice);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetBasePricesByYear(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return basePrices;
        }

        public bool IsDeletedBasePriceList(UInt64 idBasePriceList, string MainServerConnectionString)
        {
            bool isDeleted = false;
            if (idBasePriceList > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("PLM_BasePriceList_Delete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", idBasePriceList);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        isDeleted = true;
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsDeletedBasePriceList(). IdProductType- {0} ErrorMessage- {1}", idBasePriceList, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }

        #region connection methods

        /// <summary>
        /// This method is used to get connection string name exists or not by name.
        /// </summary>
        public bool IsConnectionStringNameExist(string Name)
        {
            bool isExist = false;
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.Name == Name)
                    {
                        isExist = true;
                        return isExist;
                    }
                }
            }
            return isExist;
        }

        #endregion
    }

}