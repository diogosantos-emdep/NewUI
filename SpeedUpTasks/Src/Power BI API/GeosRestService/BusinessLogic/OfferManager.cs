using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Entities;

namespace BusinessLogic
{
    public class OpportunityManager
    {
        string _ConnString;
        //static Dictionary<string, string> DictonarySites = new Dictionary<string, string>();
        //static Dictionary<string, int> Dictonarycurrency = new Dictionary<string, int>();
        public OpportunityManager(string ConnString)
        {
            this._ConnString = ConnString;
        }
        #region Methods
        public List<Offer> GetOfferProductionData(DateTime FromDate, DateTime ToDate, string AllPlants)
        {
            List<Offer> Offers = new List<Offer>();
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
                        Offers.AddRange(GetPlantWiseOfferProductionData(FromDate, ToDate, plant.Value, plant.Key));
                    }
                }
                else
                {
                    string[] plants = AllPlants.Split(',');
                    foreach (string plant in plants)
                    {
                        if (CommonManager.DictonarySites.ContainsKey(plant))
                            Offers.AddRange(GetPlantWiseOfferProductionData(FromDate, ToDate, CommonManager.DictonarySites[plant], plant));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Offers;
        }
        public List<Offer> GetPlantWiseOfferProductionData(DateTime FromDate, DateTime ToDate,string DatabaseIP, string SiteName)
        {
            List<Offer> Offers = new List<Offer>();
            string connstr = "Data Source = " + DatabaseIP + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("offers_GetProductionData", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idcurrency", 0);
                    command.Parameters.AddWithValue("_accountingYearFrom", FromDate);
                    command.Parameters.AddWithValue("_accountingYearTo", ToDate);
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Offer offer = new Offer();
                            offer.Products = new List<Product>();
                            offer.ID= dr["idoffer"].ToString();
                            offer.Code= dr["OfferCode"].ToString();
                            offer.Project = dr["CarProjectName"].ToString();
                            offer.Description = dr["OfferTitle"].ToString();
                            offer.Group = dr["CustomerGroup"].ToString();
                            offer.Plant  = dr["SiteName"].ToString(); 
                            offer.Contry = dr["CustomerCountry"].ToString();
                            offer.Region = dr["CustomerZone"].ToString();
                            offer.BusinessUnit = dr["BusinessUnit"].ToString();
                            offer.Source = dr["Source"].ToString();
                            offer.DeliveryDate = Convert.ToDateTime(dr["OfferExpectedPODate"]).ToString("yyyy-MM-dd");
                            offer.Status = dr["OfferStatus"].ToString();
                            offer.EMDEPSite = offer.Plant;
                            offer.IdProductSubCategory = dr["IdProductSubCategory"].ToString();
                            offer.IdSalesOwner= dr["IdSalesOwner"].ToString();
                            Offers.Add(offer);
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdOption"].ToString() != "6" && dr["IdOption"].ToString() != "19"
                                    && dr["IdOption"].ToString() != "21" && dr["IdOption"].ToString() != "23"
                                    && dr["IdOption"].ToString() != "25" && dr["IdOption"].ToString() != "27")
                                {
                                    Offer offer = Offers.Find(i => i.ID == dr["idoffer"].ToString());

                                    if (offer != null)
                                    {
                                        Product product = new Product();
                                        product.Name = dr["offeroption"].ToString();
                                        product.Qty = dr["Quantity"].ToString();
                                        offer.Products.Add(product);
                                    }
                                }

                            }
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdProductSubCategory"] != null)
                                {
                                    Offer offer = Offers.Find(i => i.IdProductSubCategory == dr["IdProductSubCategory"].ToString());
                                    if (offer != null)
                                    {
                                        if (dr["Category"] != DBNull.Value)
                                            offer.Category1 = dr["Category"].ToString();

                                        if (dr["Category"] == DBNull.Value)
                                        {
                                            offer.Category1 = dr["ProductSubCategory"].ToString();
                                        }
                                        else
                                        {
                                            offer.Category2 = dr["ProductSubCategory"].ToString();
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
                                    Offer offer = Offers.Find(i => i.IdSalesOwner == dr["IdPerson"].ToString());
                                    if (offer != null)
                                    {
                                        if (dr["SalesOwner"] != DBNull.Value)
                                            offer.SalesOwner = dr["SalesOwner"].ToString();
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
            return Offers;
        }
        public List<Opportunities> GetOffers(DateTime FromDate, DateTime ToDate, string AllPlants,
            string currency = "EUR")
        {
            List<Opportunities> offers = new List<Opportunities>();
            int IdCurrency = 0;
            try
            {
                //Dictionary<string, string> dicPlants = new Dictionary<string, string>();
                //dicPlants = GetAllSites();
                if (CommonManager.Dictonarycurrency != null || CommonManager.Dictonarycurrency.Count == 0)
                {
                    if (CommonManager.Dictonarycurrency.ContainsKey(currency))
                    {
                        IdCurrency = CommonManager.Dictonarycurrency[currency];
                    }
                    else
                    {
                        CommonManager.ConnString = this._ConnString;
                        IdCurrency = CommonManager.GetIdCurrency(currency);
                        CommonManager.Dictonarycurrency.Add(currency, IdCurrency);
                    }
                }
                if (CommonManager.DictonarySites == null || CommonManager.DictonarySites.Count == 0)
                {
                    CommonManager.ConnString = this._ConnString;
                    CommonManager.DictonarySites = CommonManager.GetAllSites();
                }

                if (AllPlants == "0")
                {
                    foreach (KeyValuePair<string, string> plant in CommonManager.DictonarySites)
                    {
                        offers.AddRange(GetPlantwiseOfferDetails(FromDate, ToDate, plant.Value, plant.Key, IdCurrency));
                    }
                }
                else
                {
                    string[] plants = AllPlants.Split(',');
                    foreach (string plant in plants)
                    {
                        if (CommonManager.DictonarySites.ContainsKey(plant))
                            offers.AddRange(GetPlantwiseOfferDetails(FromDate, ToDate, CommonManager.DictonarySites[plant], plant, IdCurrency));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return offers;
        }

        public List<Opportunities> GetPlantwiseOfferDetails(DateTime FromDate, DateTime ToDate, string DatabaseIP
            , string SiteName, int IdCurrency)
        {
            List<Opportunities> offers = new List<Opportunities>();
            try
            {
                string connstr = "Data Source = " + DatabaseIP + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";

                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("offers_GetRestfulTimeData", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600;
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idcurrency", IdCurrency);
                    command.Parameters.AddWithValue("_idZone", 0);
                    command.Parameters.AddWithValue("_accountingYearFrom", FromDate);
                    command.Parameters.AddWithValue("_accountingYearTo", ToDate);
                    command.Parameters.AddWithValue("_idPermission", 22);
                    command.Parameters.AddWithValue("_idEmdepSite", 0);

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Opportunities offer = new Opportunities();
                            offer.Products = new List<Product>();

                            if (dr["idoffer"] != DBNull.Value)
                                offer.Id = Convert.ToInt64(dr["idoffer"].ToString());
                            offer.Code = dr["OfferCode"].ToString();
                            if (dr["IdProductSubCategory"] != null)
                                offer.IdProductCategory = dr["IdProductSubCategory"].ToString();
                            if (dr["IdSalesOwner"] != null)
                                offer.IdSalesOwner = dr["IdSalesOwner"].ToString();
                            offer.Description = dr["OfferTitle"].ToString();
                            offer.Project = dr["CarProjectName"].ToString();
                            offer.Group = dr["CustomerGroup"].ToString();
                            offer.Plant = dr["SiteName"].ToString();
                            offer.Country = dr["CustomerCountry"].ToString();
                            offer.Region = dr["CustomerZone"].ToString();
                            offer.BusinessUnit = dr["BusinessUnit"].ToString();
                            offer.Source = dr["Source"].ToString();

                            if (dr["OfferExpectedPODate"] != DBNull.Value)
                                offer.CloseDate = Convert.ToDateTime(dr["OfferExpectedPODate"]).ToString("yyyy-MM-dd");

                            if (dr["OfferConfidentialLevel"] != DBNull.Value)
                                offer.ConfidentialLevel = Convert.ToInt32(dr["OfferConfidentialLevel"].ToString());

                            if (dr["OfferAmount"] != DBNull.Value)
                                offer.Amount = Convert.ToDouble(dr["OfferAmount"].ToString());
                            offer.Currency = dr["OfferCurrency"].ToString();
                            offer.Rfq = dr["Rfq"].ToString();
                            if (dr["RFQReception"] != DBNull.Value)
                                offer.RFQReception = Convert.ToDateTime(dr["RFQReception"]).ToString("yyyy-MM-dd");

                            if (dr["SendIn"] != DBNull.Value)
                                offer.QuoteSentDate = Convert.ToDateTime(dr["SendIn"]).ToString("yyyy-MM-dd");

                            if (dr["CarOEM"] != DBNull.Value)
                                offer.OEM = dr["CarOEM"].ToString();

                            if (dr["OfferStatus"] != DBNull.Value)
                                offer.Status = dr["OfferStatus"].ToString();




                            //if (dr["Category"] != DBNull.Value)
                            //    offer.Category1 = dr["Category"].ToString();
                            //if (dr["ProductSubCategory"] != DBNull.Value)
                            //    offer.Category2 = dr["ProductSubCategory"].ToString();

                            offer.EmdepSite = SiteName;

                            offers.Add(offer);
                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdOption"].ToString() != "6" && dr["IdOption"].ToString() != "19"
                                    && dr["IdOption"].ToString() != "21" && dr["IdOption"].ToString() != "23"
                                    && dr["IdOption"].ToString() != "25" && dr["IdOption"].ToString() != "27")
                                {
                                    Opportunities offer = offers.Find(o => o.Id == Convert.ToInt64(dr["IdOffer"].ToString()));

                                    if (offer != null)
                                    {
                                        Product product = new Product();
                                        product.Name = dr["offeroption"].ToString();
                                        product.Qty = dr["Quantity"].ToString();
                                        offer.Products.Add(product);
                                    }
                                }

                            }
                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdOffer"] != DBNull.Value)
                                {
                                    Opportunities offer = offers.Find(i => i.Id == Convert.ToInt64(dr["IdOffer"].ToString()));
                                    if (offer != null)
                                    {
                                        offer.LostReason = dr["Reason"].ToString();
                                        offer.LostDescription = dr["Comments"].ToString();
                                        offer.Competitor = dr["Name"].ToString();
                                    }
                                }
                            }
                        }
                        if (dr.NextResult()) { }
                        if (dr.NextResult()) { }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdProductSubCategory"] != null)
                                {
                                    Opportunities offer = offers.Find(i => i.IdProductCategory == dr["IdProductSubCategory"].ToString());
                                    if (offer != null)
                                    {
                                        if (dr["Category"] != DBNull.Value)
                                            offer.Category1 = dr["Category"].ToString();

                                        if (dr["Category"] == DBNull.Value)
                                        {
                                            offer.Category1 = dr["ProductSubCategory"].ToString();
                                        }
                                        else
                                        {
                                            offer.Category2 = dr["ProductSubCategory"].ToString();
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
                                    Opportunities offer = offers.Find(i => i.IdSalesOwner == dr["IdPerson"].ToString());
                                    if (offer != null)
                                    {
                                        if (dr["SalesOwner"] != DBNull.Value)
                                            offer.SalesOwner = dr["SalesOwner"].ToString();
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
            return offers;
        }

        //public Dictionary<string, string> GetAllSites()
        //{
        //    //Dictionary<string, string> DictonarySites = new Dictionary<string, string>();

        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(_ConnString))
        //        {
        //            conn.Open();
        //            MySqlCommand command = new MySqlCommand("GeosApiGetAllSites", conn);
        //            command.CommandType = CommandType.StoredProcedure;

        //            using (MySqlDataReader dr = command.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    if (!DictonarySites.ContainsKey(dr["ShortName"].ToString()) &&
        //                        !DictonarySites.ContainsValue(dr["DatabaseIP"].ToString()))
        //                        DictonarySites.Add(dr["ShortName"].ToString(), dr["DatabaseIP"].ToString());
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return DictonarySites;
        //}

        //public int GetIdCurrency(string Currency)
        //{
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(_ConnString))
        //        {
        //            conn.Open();
        //            MySqlCommand command = new MySqlCommand("currency_GetCurrencyByName", conn);
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("_Name", Currency);
        //            int CurrencyID = Convert.ToInt16(command.ExecuteScalar());
        //            return CurrencyID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
    }

}
