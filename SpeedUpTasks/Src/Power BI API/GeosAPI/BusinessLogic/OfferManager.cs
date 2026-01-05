using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Entities;
using System.Linq;

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
        public List<Offer> GetPlantWiseOfferProductionData(DateTime FromDate, DateTime ToDate, string DatabaseIP, string SiteName)
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
                            offer.ID = dr["idoffer"].ToString();
                            offer.Code = dr["OfferCode"].ToString();
                            offer.Project = dr["CarProjectName"].ToString();
                            offer.Description = dr["OfferTitle"].ToString();
                            offer.Group = dr["CustomerGroup"].ToString();
                            offer.Plant = dr["SiteName"].ToString();
                            offer.Contry = dr["CustomerCountry"].ToString();
                            offer.Region = dr["CustomerZone"].ToString();
                            offer.BusinessUnit = dr["BusinessUnit"].ToString();
                            offer.Source = dr["Source"].ToString();
                            offer.DeliveryDate = Convert.ToDateTime(dr["OfferExpectedPODate"]).ToString("yyyy-MM-dd");
                            offer.Status = dr["OfferStatus"].ToString();
                            offer.EMDEPSite = offer.Plant;
                            offer.IdProductSubCategory = dr["IdProductSubCategory"].ToString();
                            offer.IdSalesOwner = dr["IdSalesOwner"].ToString();
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
                                    List<Offer> lstoffer = Offers.FindAll(i => i.IdProductSubCategory == dr["IdProductSubCategory"].ToString());
                                    if (lstoffer.Count > 0)
                                    {
                                        foreach (var item in lstoffer)
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
                                    List<Offer> lstoffer = Offers.FindAll(i => i.IdSalesOwner == dr["IdPerson"].ToString());
                                    if (lstoffer.Count > 0)
                                    {
                                        foreach (var item in lstoffer)
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
                    command.Parameters.AddWithValue("_idCurrentUser", 0);
                    command.Parameters.AddWithValue("_accountingYearFrom", FromDate);
                    command.Parameters.AddWithValue("_accountingYearTo", ToDate);
                    command.Parameters.AddWithValue("_idPermission", 22);
                    command.Parameters.AddWithValue("_idEmdepSite", 0);

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr["OfferStatus"].ToString() == "Cancelled")
                            {
                                continue;
                            }
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

                            //if (dr["PoDate"] != DBNull.Value)
                            //    offer.PoDate = Convert.ToDateTime(dr["PoDate"]).ToString("yyyy-MM-dd");

                            //if (dr["PoCode"] != DBNull.Value)
                            //    offer.Po = Convert.ToString(dr["PoCode"]);

                            //if (dr["PoType"] != DBNull.Value)
                            //    offer.PoType = Convert.ToString(dr["PoType"]);

                            if (dr["CarOEM"] != DBNull.Value)
                                offer.OEM = dr["CarOEM"].ToString();

                            if (dr["OfferStatus"] != DBNull.Value)
                                offer.Status = dr["OfferStatus"].ToString();


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
                                    List<Opportunities> lstOffers = offers.FindAll(i => i.IdProductCategory == dr["IdProductSubCategory"].ToString());
                                    if (lstOffers.Count > 0)
                                    {
                                        foreach (var item in lstOffers)
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
                                    List<Opportunities> lstoffer = offers.FindAll(i => i.IdSalesOwner == dr["IdPerson"].ToString());
                                    if (lstoffer.Count > 0)
                                    {
                                        foreach (var item in lstoffer)
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

        public List<Tracking> GetCounterPartTrackingData(DateTime FromDate, DateTime ToDate, string AllPlants)
        {
            List<Tracking> Trackings = new List<Tracking>();
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
                        Trackings.AddRange(GetPlantWiseCounterPartTrackingData(FromDate, ToDate, plant.Value, plant.Key));
                    }
                }
                else
                {
                    string[] plants = AllPlants.Split(',');
                    foreach (string plant in plants)
                    {
                        if (CommonManager.DictonarySites.ContainsKey(plant))
                            Trackings.AddRange(GetPlantWiseCounterPartTrackingData(FromDate, ToDate, CommonManager.DictonarySites[plant], plant));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Trackings;

        }
        public Connector GetConnectorDetailByReference(string reference, string OtherReference, string SiteName, string connstr)
        {
            Connector connector = new Connector();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("RefConClients_GetConnectorDetailsByReference", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_Reference", reference);
                    command.Parameters.AddWithValue("_OtherReference", OtherReference);
                    command.CommandTimeout = 600;
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            connector.ConnectorFamily = dr["ConnectorFamily"].ToString();
                            connector.ConnectorGender = dr["ConnectorGender"].ToString();
                            connector.Cavities = dr["Ways"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = SiteName;
                throw ex;
            }

            return connector;
        }

        public List<Tracking> GetPlantWiseCounterPartTrackingData(DateTime FromDate, DateTime ToDate, string DatabaseIP, string SiteName)
        {
            List<Tracking> Trackings = new List<Tracking>();
            string connstr = "Data Source = " + DatabaseIP + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("counterpartstracking_GetcounterpartstrackingDetail", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_FromDate", FromDate);
                    command.Parameters.AddWithValue("_ToDate", ToDate);
                    command.CommandTimeout = 3000;
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Tracking tracking = new Tracking();

                            tracking.Id = dr["Id"].ToString();
                            tracking.SerialNumber = dr["SerialNumber"].ToString();
                            tracking.WorkOrder = dr["WorkOrder"].ToString();

                            tracking.OfferCode = dr["OfferCode"].ToString();
                            tracking.OfferTitle = dr["OfferTitle"].ToString();
                            tracking.OfferType = dr["OfferType"].ToString();
                            tracking.Group = dr["Group"].ToString();
                            tracking.Plant = dr["Plant"].ToString();
                            tracking.Country = dr["Country"].ToString();
                            tracking.Region = dr["Region"].ToString();
                            tracking.Project = dr["Project"].ToString();
                            tracking.Priority = dr["Priority"].ToString();
                            tracking.CustomerSpecifications = dr["CustomerSpecifications"].ToString();
                            if (dr["GoAheadDate"] != DBNull.Value)
                                tracking.GoAheadDate = Convert.ToDateTime(dr["GoAheadDate"]).ToString("yyyy-MM-dd");
                            if (dr["PODate"] != DBNull.Value)
                                tracking.PODate = Convert.ToDateTime(dr["PODate"]).ToString("yyyy-MM-dd");
                            if (dr["ExpectedDeliveryDate"] != DBNull.Value)
                                tracking.ExpectedDeliveryDate = Convert.ToDateTime(dr["ExpectedDeliveryDate"]).ToString("yyyy-MM-dd");

                            tracking.OfferOwner = dr["OfferOwner"].ToString();
                            tracking.OfferStatus = dr["OfferStatus"].ToString();
                            tracking.Template = dr["Template"].ToString();
                            tracking.ItemStatus = dr["ItemStatus"].ToString();
                            if (dr["OpenDate"] != DBNull.Value)
                                tracking.OpenDate = Convert.ToDateTime(dr["OpenDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                            if (dr["StartDate"] != DBNull.Value)
                                tracking.StartDate = Convert.ToDateTime(dr["StartDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                            if (dr["CloseDate"] != DBNull.Value)
                                tracking.CloseDate = Convert.ToDateTime(dr["CloseDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                            if (dr["Rework"] != DBNull.Value)
                            {
                                if (dr["Rework"].ToString() == "1")
                                    tracking.Result = "Failed";
                                else
                                    tracking.Result = "Success";

                            }
                            tracking.WorkingTime = dr["WorkingTime"].ToString();

                            if (dr["IdStage"] != DBNull.Value)
                            {
                                tracking.IdStage = Convert.ToByte(dr["IdStage"].ToString());
                            }
                            if (dr["IdOperator"] != DBNull.Value)
                            {
                                tracking.IdPerson = Convert.ToInt32(dr["IdOperator"].ToString());
                            }

                            tracking.EmdepSite = SiteName;

                            if (dr["IdRevisionItem"] != DBNull.Value)
                                tracking.IdRevisionItem = Convert.ToInt64(dr["IdRevisionItem"]);

                            Trackings.Add(tracking);
                        }


                        if (dr.NextResult())
                        {
                            //   List<Int64> idRevisionItemList = new List<long>();
                            while (dr.Read())
                            {
                                if (dr["IdRevisionItem"] != null)
                                {


                                    List<Tracking> lsttracking = Trackings.FindAll(i => i.IdRevisionItem == Convert.ToInt64(dr["IdRevisionItem"]));
                                    if (lsttracking.Count > 0)
                                    {
                                        foreach (var item in lsttracking)
                                        {

                                            item.IdDrawing = dr["IdDrawing"].ToString();
                                            item.Item = dr["NumItem"].ToString();
                                            item.Type = dr["Type"].ToString();
                                            if (dr["Reference"] != DBNull.Value)
                                            {
                                                Connector Connector = new Connector();
                                                Connector = GetConnectorDetailByReference(dr["Reference"].ToString(), null, SiteName, connstr);
                                                item.ConnectorFamily = Connector.ConnectorFamily;
                                                item.ConnectorGender = Connector.ConnectorGender;
                                                item.Cavities = Connector.Cavities;
                                            }
                                            else if (dr["OtherReference"] != DBNull.Value)
                                            {
                                                Connector Connector = new Connector();
                                                Connector = GetConnectorDetailByReference(null, dr["OtherReference"].ToString(), SiteName, connstr);
                                                item.ConnectorFamily = Connector.ConnectorFamily;
                                                item.ConnectorGender = Connector.ConnectorGender;
                                                item.Cavities = Connector.Cavities;
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
                                if (dr["IdStage"] != null)
                                {
                                    List<Tracking> lsttracking = Trackings.FindAll(i => i.IdStage == Convert.ToByte(dr["IdStage"]));
                                    if (lsttracking.Count > 0)
                                    {
                                        foreach (var item in lsttracking)
                                        {
                                            item.Stage = dr["Name"].ToString();

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
                                    List<Tracking> lsttracking = Trackings.FindAll(i => i.IdPerson == Convert.ToInt32(dr["IdPerson"]));
                                    if (lsttracking.Count > 0)
                                    {
                                        foreach (var item in lsttracking)
                                        {
                                            item.OperatorName = dr["OperatorName"].ToString();

                                        }

                                    }
                                }
                            }
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdCounterpart"] != null)
                                {
                                    List<Tracking> lsttracking = Trackings.FindAll(i => i.Id == dr["IdCounterpart"].ToString());
                                    if (lsttracking.Count > 0)
                                    {
                                        foreach (var item in lsttracking)
                                        {
                                            if (item.Result == "Failed")
                                                item.FailureCodes = dr["FailureCodes"].ToString();

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
            return Trackings;
        }


        public List<TechnicalService> GetPartNumberTracking(DateTime FromDate, DateTime ToDate, string AllPlants)
        {
            List<TechnicalService> TechnicalServices = new List<TechnicalService>();
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
                        TechnicalServices.AddRange(GetPartNumberTracking(FromDate, ToDate, plant.Value, plant.Key));
                    }
                }
                else
                {
                    string[] plants = AllPlants.Split(',');
                    foreach (string plant in plants)
                    {
                        if (CommonManager.DictonarySites.ContainsKey(plant))
                            TechnicalServices.AddRange(GetPartNumberTracking(FromDate, ToDate, CommonManager.DictonarySites[plant], plant));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TechnicalServices;

        }


        public List<TechnicalServiceList> GetWorkorders(DateTime FromDate, DateTime ToDate, string AllPlants)
        {
            List<TechnicalServiceList> TechnicalServices = new List<TechnicalServiceList>();
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
                        TechnicalServices.AddRange(GetWorkorders(FromDate, ToDate, plant.Value, plant.Key));
                    }
                }
                else
                {
                    string[] plants = AllPlants.Split(',');
                    foreach (string plant in plants)
                    {
                        if (CommonManager.DictonarySites.ContainsKey(plant))
                            TechnicalServices.AddRange(GetWorkorders(FromDate, ToDate, CommonManager.DictonarySites[plant], plant));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TechnicalServices;

        }

        public List<TechnicalService> GetPartNumberTracking(DateTime FromDate, DateTime ToDate, string DatabaseIP, string SiteName)
        {
            List<TechnicalService> TechnicalServices = new List<TechnicalService>();
            string connstr = "Data Source = " + DatabaseIP + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("partnumbertracking_GetpartstrackingDetail", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_FromDate", FromDate);
                    command.Parameters.AddWithValue("_ToDate", ToDate);
                    command.Parameters.AddWithValue("_Plants", SiteName);
                    command.CommandTimeout = 3000;
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            TechnicalService technicalService = new TechnicalService();

                            technicalService.EmdepSite = dr["ShortName"].ToString();

                            technicalService.Code = dr["SerialNumber"].ToString();
                            technicalService.Reference = dr["Reference"].ToString();
                            technicalService.Title = dr["Title"].ToString();

                            technicalService.WorkOrder = dr["WorkOrder"].ToString();
                            technicalService.Project = dr["Project"].ToString();
                            technicalService.Description = dr["Description"].ToString();

                            technicalService.Group = dr["Group"].ToString();
                            technicalService.Plant = dr["Plant"].ToString();
                            technicalService.Country = dr["Country"].ToString();
                            technicalService.Region = dr["Region"].ToString();

                            technicalService.BusinessUnit = dr["BusinessUnit"].ToString();

                            technicalService.POCode = dr["POCode"].ToString();
                            if (dr["PODate"] != DBNull.Value)
                                technicalService.PODate = Convert.ToDateTime(dr["PODate"]).ToString("yyyy-MM-dd");
                            if (dr["DeliveryDate"] != DBNull.Value)
                                technicalService.DeliveryDate = Convert.ToDateTime(dr["DeliveryDate"]).ToString("yyyy-MM-dd");


                            if (dr["OpenDate"] != DBNull.Value)
                                technicalService.OpenDate = Convert.ToDateTime(dr["OpenDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                            if (dr["EndDate"] != DBNull.Value)
                                technicalService.EndDate = Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                            if (dr["InternalComment"] != DBNull.Value)
                                technicalService.Comments = dr["InternalComment"].ToString();

                            if (dr["Validator"] != DBNull.Value)
                                technicalService.Validator = dr["Validator"].ToString();

                            technicalService.EmdepSite = SiteName;
                            TechnicalServices.Add(technicalService);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
             //  ex.HelpLink = technicalService.EmdepSite;
                throw ex;
            }
            return TechnicalServices;
        }


        public List<TechnicalServiceList> GetWorkorders(DateTime FromDate, DateTime ToDate, string DatabaseIP, string SiteName)
        {
            List<TechnicalServiceList> TechnicalServiceLists = new List<TechnicalServiceList>();
            string connstr = "Data Source = " + DatabaseIP + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("ots_GetWorkorderDetail", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_FromDate", FromDate);
                    command.Parameters.AddWithValue("_ToDate", ToDate);
                    command.Parameters.AddWithValue("_Plants", SiteName);
                    command.CommandTimeout = 3000;
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            TechnicalServiceList technicalService = new TechnicalServiceList();
                            if (dr["IdOT"] != DBNull.Value)
                                technicalService.IdOT = Convert.ToInt64(dr["IdOT"].ToString());

                            technicalService.Code = dr["WorkOrder"].ToString();

                            technicalService.Project = dr["Project"].ToString();
                            technicalService.Description = dr["Description"].ToString();

                            technicalService.Group = dr["Group"].ToString();
                            technicalService.Plant = dr["Plant"].ToString();
                            technicalService.Country = dr["Country"].ToString();
                            technicalService.Region = dr["Region"].ToString();

                            technicalService.BusinessUnit = dr["BusinessUnit"].ToString();

                            technicalService.POCode = dr["POCode"].ToString();
                            if (dr["PODate"] != DBNull.Value)
                                technicalService.PODate = Convert.ToDateTime(dr["PODate"]).ToString("yyyy-MM-dd");

                            if (dr["DeliveryDate"] != DBNull.Value)
                                technicalService.DeliveryDate = Convert.ToDateTime(dr["DeliveryDate"]).ToString("yyyy-MM-dd");


                            if (dr["CreationDate"] != DBNull.Value)
                                technicalService.CreationDate = Convert.ToDateTime(dr["CreationDate"]).ToString("yyyy-MM-dd");

                            if (dr["EndDate"] != DBNull.Value)
                                technicalService.EndDate = Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd HH:mm:ss");

                            technicalService.EmdepSite = SiteName;
                            TechnicalServiceLists.Add(technicalService);
                        }

                        List<ArticleReferenceList> articleReferenceLists = new List<ArticleReferenceList>();
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdOT"] != null)
                                {
                                    if (dr["Reference"] != DBNull.Value)
                                    {
                                        ArticleReferenceList articleReferenceList = new ArticleReferenceList();
                                        if (dr["IdOT"] != DBNull.Value)
                                            articleReferenceList.Id = Convert.ToInt64(dr["IdOT"]);
                                        if (dr["Reference"] != DBNull.Value)
                                            articleReferenceList.Reference = Convert.ToString(dr["Reference"]);
                                        articleReferenceLists.Add(articleReferenceList);
                                    }
                                }
                            }
                        }


                        foreach (var item in articleReferenceLists.GroupBy(arl => arl.Id))
                        {

                            TechnicalServiceList technicalService = TechnicalServiceLists.Where(i => i.IdOT == item.Key).FirstOrDefault();

                            technicalService.Services = String.Join(",", item.ToList().Select(x => x.Reference));
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                //ex.HelpLink = technicalService.EmdepSite;
                throw ex;
            }
            return TechnicalServiceLists;
        }

    }

}