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
    public class QuotaManager
    {
        string _ConnString;
        public QuotaManager(string ConnString)
        {
            _ConnString = ConnString;
        }

        public List<SalesQuota> GetSalesQuotas(string Currency, string Year)
        {
            List<SalesQuota> SalesQuotas = new List<SalesQuota>();
            int IdCurrency = 0;
            try
            {
                if (CommonManager.Dictonarycurrency != null || CommonManager.Dictonarycurrency.Count == 0)
                {
                    if (CommonManager.Dictonarycurrency.ContainsKey(Currency))
                    {
                        IdCurrency = CommonManager.Dictonarycurrency[Currency];
                    }
                    else
                    {
                        CommonManager.ConnString = this._ConnString;
                        IdCurrency = CommonManager.GetIdCurrency(Currency);
                        CommonManager.Dictonarycurrency.Add(Currency, IdCurrency);
                    }
                }
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("salesusers_GetAllSalesUserPeopleDetailsWithPlantByDate", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idcurrency", IdCurrency);
                    command.Parameters.AddWithValue("_accountingYear", Year);
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            SalesQuota salesQuota = new SalesQuota();
                            salesQuota.EmployeeFullName = dr["Name"].ToString() + " " + dr["Surname"].ToString();
                            if (!string.IsNullOrEmpty(dr["ExchangeRateDate"].ToString()))
                                salesQuota.Year = Convert.ToDateTime(dr["ExchangeRateDate"]).ToString("yyyy");
                            salesQuota.Amount = dr["SalesQuotaAmount"].ToString();
                            salesQuota.Currency = Currency;
                            //if(!string.IsNullOrEmpty(dr["IdSalesQuotaCurrency"].ToString()))
                            //{
                            //    var Curr = CommonManager.Dictonarycurrency.FirstOrDefault(c => c.Value == Convert.ToInt32(dr["IdSalesQuotaCurrency"]));
                            //    salesQuota.TargetCurrency = Curr.Key;
                            //}

                            SalesQuotas.Add(salesQuota);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return SalesQuotas;
        }

        public List<PlantTarget> GetPlantTarget(string Currency, string FromDate, string ToDate)
        {
            List<PlantTarget> PlantTarget = new List<PlantTarget>();
            int IdCurrency = 0;
            try
            {
                if (CommonManager.Dictonarycurrency != null || CommonManager.Dictonarycurrency.Count == 0)
                {
                    if (CommonManager.Dictonarycurrency.ContainsKey(Currency))
                    {
                        IdCurrency = CommonManager.Dictonarycurrency[Currency];
                    }
                    else
                    {
                        CommonManager.ConnString = this._ConnString;
                        IdCurrency = CommonManager.GetIdCurrency(Currency);
                        CommonManager.Dictonarycurrency.Add(Currency, IdCurrency);
                    }
                }
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("GetPlantSalesQuotaWithYearByIdUserPermissionDatewise", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idcurrency", IdCurrency);
                    command.Parameters.AddWithValue("_accountingYearFrom", FromDate);
                    command.Parameters.AddWithValue("_accountingYearTo", ToDate);
                    command.Parameters.AddWithValue("_idPermission", 22);


                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            PlantTarget plantQuota = new PlantTarget();
                            

                            plantQuota =  PlantTarget.Find(pt => pt.Year == dr["Year"].ToString() && pt.Plant == dr["ShortName"].ToString() && pt.BusinessUnit == dr["Value"].ToString());
                            if(plantQuota == null)
                            {
                                plantQuota = new PlantTarget();
                                plantQuota.Plant = dr["ShortName"].ToString();
                                plantQuota.BusinessUnit = dr["Value"].ToString();
                                plantQuota.Year = dr["Year"].ToString();
                                plantQuota.Amount = dr["SalesQuotaAmount"].ToString();
                                plantQuota.Currency = Currency;
                                //if (!string.IsNullOrEmpty(dr["IdSalesQuotaCurrency"].ToString()))
                                //{
                                //    var Curr = CommonManager.Dictonarycurrency.FirstOrDefault(c => c.Value == Convert.ToInt32(dr["IdSalesQuotaCurrency"]));
                                //    plantQuota.TargetCurrency = Curr.Key;
                                //}

                                PlantTarget.Add(plantQuota);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
            return PlantTarget;
        }

        public List<CustomerTarget> GetCustomerTarget(string Currency, string FromYear, string ToYear)
        {
            List<CustomerTarget> customerTargets = new List<CustomerTarget>();
            List<string> AllPlants = new List<string>();
            string IdSites = string.Empty;
            int IdCurrency = 0;
            try
            {
                if (CommonManager.Dictonarycurrency != null || CommonManager.Dictonarycurrency.Count == 0)
                {
                    if (CommonManager.Dictonarycurrency.ContainsKey(Currency))
                    {
                        IdCurrency = CommonManager.Dictonarycurrency[Currency];
                    }
                    else
                    {
                        CommonManager.ConnString = this._ConnString;
                        IdCurrency = CommonManager.GetIdCurrency(Currency);
                        CommonManager.Dictonarycurrency.Add(Currency, IdCurrency);
                    }
                }
                if (CommonManager.DictonarySitesWithIds == null || CommonManager.DictonarySitesWithIds.Count == 0)
                {
                    CommonManager.ConnString = _ConnString;
                    CommonManager.GetAllSites();
                }


                foreach (KeyValuePair<string, string> item in CommonManager.DictonarySitesWithIds)
                {
                    AllPlants.Add(item.Value);
                }



                IdSites = String.Join(",", ((List<string>)AllPlants));
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("sites_GetTargetByCustomerYearDatewiseByPlant", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idZone", 0);
                    command.Parameters.AddWithValue("_idcurrency", IdCurrency);
                    command.Parameters.AddWithValue("_accountingFromYear", FromYear);
                    command.Parameters.AddWithValue("_accountingToYear", ToYear);
                    command.Parameters.AddWithValue("_idSite", IdSites);

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CustomerTarget customerTarget = new CustomerTarget();

                            //customerTarget.CustomerFullName = dr["CustomerGroup"].ToString() + " " + dr["CustomerName"].ToString();
                            customerTarget.Group= dr["CustomerGroup"].ToString();
                            customerTarget.Plant = dr["CustomerName"].ToString();
                            customerTarget.Country = dr["CustomerCountry"].ToString();
                            customerTarget.Year = dr["Year"].ToString();
                            customerTarget.Amount = dr["TargetAmountWithExchangeRate"].ToString();
                            customerTarget.Currency = dr["TargetSalesCurrency"].ToString();

                            customerTargets.Add(customerTarget);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return customerTargets;
        }

        private string GetCurrencybyId(int IdCurrency)
        {
            return "";
        }
    }
}
