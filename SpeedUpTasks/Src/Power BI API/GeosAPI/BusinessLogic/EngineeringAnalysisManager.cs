using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace BusinessLogic
{
    public class EngineeringAnalysisManager
    {
        string _ConnString;
        public EngineeringAnalysisManager(string ConnString)
        {
            this._ConnString = ConnString;

        }
        public List<EngineeringAnalysis> GetEngineeringAnalysisDetails(DateTime FromDate, DateTime ToDate, string AllPlants)
        {
            List<EngineeringAnalysis> lstengineeringAnalysis = new List<EngineeringAnalysis>();

            try
            {

                if (CommonManager.DictonarySites == null || CommonManager.DictonarySites.Count == 0)
                {
                    CommonManager.ConnString = this._ConnString;
                    CommonManager.DictonarySites = CommonManager.GetAllSites();
                }
                if (AllPlants == "0")
                {
                    foreach (KeyValuePair<string, string> plant in CommonManager.DictonarySites)
                    {
                        lstengineeringAnalysis.AddRange(GetPlantwiseEngineeringAnalysisDetails(FromDate, ToDate, plant.Value, plant.Key));
                    }
                }
                else
                {
                    string[] plants = AllPlants.Split(',');
                    foreach (string plant in plants)
                    {
                        if (CommonManager.DictonarySites.ContainsKey(plant))
                            lstengineeringAnalysis.AddRange(GetPlantwiseEngineeringAnalysisDetails(FromDate, ToDate, CommonManager.DictonarySites[plant], plant));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstengineeringAnalysis;
        }

        public List<EngineeringAnalysis> GetPlantwiseEngineeringAnalysisDetails(DateTime FromDate, DateTime ToDate, string DatabaseIP, string SiteName)
        {
            List<EngineeringAnalysis> lstEngineeringAnalysis = new List<EngineeringAnalysis>();
            string connstr = "Data Source = " + DatabaseIP + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("offers_GetOffersEngAnalysisDateWiseAndpermission", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600;
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idcurrency", 0);
                    command.Parameters.AddWithValue("_idZone", 0);
                    command.Parameters.AddWithValue("_accountingYearFrom", FromDate);
                    command.Parameters.AddWithValue("_accountingYearTo", ToDate);
                    command.Parameters.AddWithValue("_idPermission", 22);

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            
                            EngineeringAnalysis engineeringAnalysis = new EngineeringAnalysis();

                            if (dr["IdProductCategory"] != DBNull.Value)
                                engineeringAnalysis.IdProductCategory = dr["IdProductCategory"].ToString();
                            if (dr["IdSalesOwner"] != DBNull.Value)
                                engineeringAnalysis.IdSalesOwner = dr["IdSalesOwner"].ToString();
                            engineeringAnalysis.ID = dr["IdOffer"].ToString();
                            engineeringAnalysis.Code = dr["Code"].ToString();
                            engineeringAnalysis.Project = dr["CarProject"].ToString();
                            engineeringAnalysis.Description = dr["OfferTitle"].ToString();
                            engineeringAnalysis.Group = dr["CustomerGroup"].ToString();
                            engineeringAnalysis.Plant = dr["SiteName"].ToString();
                            engineeringAnalysis.Country = dr["CustomerCountry"].ToString();
                            engineeringAnalysis.Region = dr["CustomerZone"].ToString();
                            engineeringAnalysis.BusinessUnit = dr["BusinessUnit"].ToString();
                            engineeringAnalysis.Source = dr["Source"].ToString();

                            if (dr["StartDate"] != DBNull.Value)
                                engineeringAnalysis.RequestDate = Convert.ToDateTime(dr["StartDate"]).ToString("yyyy-MM-dd");

                            if (dr["DeliveryDate"] != DBNull.Value)
                                engineeringAnalysis.DeliveryDate = Convert.ToDateTime(dr["OtDeliveryDate"]).ToString("yyyy-MM-dd");

                            if (dr["EndDate"] != DBNull.Value)
                                engineeringAnalysis.CloseDate = Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd");

                            if (string.IsNullOrEmpty(engineeringAnalysis.CloseDate))
                                engineeringAnalysis.EngineerOwner = dr["Name"].ToString() + " " + dr["Surname"].ToString();
                            else
                                engineeringAnalysis.EngineerValidator = dr["Name"].ToString() + " " + dr["Surname"].ToString();

                            engineeringAnalysis.Status = dr["OfferStatus"].ToString();
                            engineeringAnalysis.OEM = dr["CarOEM"].ToString();
                            engineeringAnalysis.EMDEPSite = SiteName;

                            lstEngineeringAnalysis.Add(engineeringAnalysis);

                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdProductSubCategory"] != null)
                                {
                                    List<EngineeringAnalysis> engineeringAnalysis = lstEngineeringAnalysis.FindAll(i => i.IdProductCategory == dr["IdProductSubCategory"].ToString());
                                    if (engineeringAnalysis.Count > 0)
                                    {
                                        foreach (var item in engineeringAnalysis)
                                        {
                                            if (dr["Category"] != DBNull.Value)
                                                item.Category1 = dr["Category"].ToString();

                                            if (dr["Category"] == DBNull.Value)
                                            {
                                                item.Category1 = dr["ProductSubCategory"].ToString();
                                            }
                                            else
                                            {
                                                item.Category2 = dr["ProductSubCategory"].ToString();
                                            }
                                        }
                                        
                                    }
                                }
                            }
                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdPerson"] != null)
                                {
                                    List<EngineeringAnalysis> engineeringAnalysis = lstEngineeringAnalysis.FindAll(i => i.IdSalesOwner == dr["IdPerson"].ToString());
                                    if (engineeringAnalysis.Count > 0)
                                    {
                                        foreach (var item in engineeringAnalysis)
                                        {
                                            if (dr["SalesOwner"] != DBNull.Value)
                                                item.SalesOwner = dr["SalesOwner"].ToString();
                                        }
                                       
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = SiteName;
                throw ex;
            }
            return lstEngineeringAnalysis;
        }
    }
}
