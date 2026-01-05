using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Entities;


namespace BusinessLogic
{
    public class OrderManager
    {
        string _ConnString;

        public OrderManager(string ConnString)
        {
            this._ConnString = ConnString;
        }
        #region Methods
        public List<Order> GetOrders(DateTime FromDate, DateTime ToDate, string AllPlants,
            string currency = "EUR")
        {
            List<Order> orders = new List<Order>();
            int IdCurrency = 0;
            try
            {
                //Dictionary<string, string> dicPlants = new Dictionary<string, string>();
                //OpportunityManager offermaneger = new OpportunityManager(_ConnString);
                //dicPlants = offermaneger.GetAllSites();
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
                        orders.AddRange(GetPlantwiseOrderDetails(FromDate, ToDate, plant.Value, plant.Key,IdCurrency));
                    }
                }
                else
                {
                    string[] plants = AllPlants.Split(',');
                    foreach (string plant in plants)
                    {
                        if (CommonManager.DictonarySites.ContainsKey(plant))
                            orders.AddRange(GetPlantwiseOrderDetails(FromDate, ToDate, CommonManager.DictonarySites[plant], plant,IdCurrency));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orders;
        }

        public List<Order> GetPlantwiseOrderDetails(DateTime FromDate, DateTime ToDate, string DatabaseIP, string SiteName, int IdCurrency)
        {
            List<Order> orders = new List<Order>();
            string connstr = "Data Source = " + DatabaseIP + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";
            try
            {
                
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("offers_GetOrdersDatatableDateWise", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600;
                    command.Parameters.AddWithValue("_idcurrency", IdCurrency);
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idZone", 0);
                    command.Parameters.AddWithValue("_accountingYearFrom", FromDate);
                    command.Parameters.AddWithValue("_accountingYearTo", ToDate);
                    command.Parameters.AddWithValue("_idPermission", 22);

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Order order = new Order();
                            if (dr["idoffer"] != DBNull.Value)
                                order.Id = Convert.ToInt64(dr["idoffer"].ToString());
                            order.Code = dr["OfferCode"].ToString();
                            order.Description = dr["OfferTitle"].ToString();
                            order.Project = dr["CarProjectName"].ToString();
                            order.Group = dr["CustomerGroup"].ToString();
                            order.Plant = dr["CustomerName"].ToString();
                            order.Country = dr["CustomerCountry"].ToString();
                            order.Region = dr["CustomerZone"].ToString();
                            order.BusinessUnit = dr["BusinessUnit"].ToString();
                            order.Source = dr["Source"].ToString();
                            if (dr["PoReceptionDate"] != DBNull.Value)
                                order.PoDate = Convert.ToDateTime(dr["PoReceptionDate"]).ToString("yyyy-MM-dd");
                            if (dr["OfferAmount"] != DBNull.Value)
                                order.Amount = Convert.ToDouble(dr["OfferAmount"].ToString());

                            order.Currency = dr["OfferCurrency"].ToString();
                            if (dr["DeliveryDate"] != DBNull.Value)
                                order.DeliveryDate = Convert.ToDateTime(dr["DeliveryDate"]).ToString("yyyy-MM-dd");

                            if (dr["OfferStatus"] != DBNull.Value)
                                order.Status = dr["OfferStatus"].ToString();
                            if (dr["SalesOwner"] != DBNull.Value)
                                order.SalesOwner = dr["SalesOwner"].ToString();

                            if (dr["Category"] != DBNull.Value)
                                order.Category1 = dr["Category"].ToString();

                            if (dr["Category"] == DBNull.Value)
                            {
                                order.Category1 = dr["ProductSubCategory"].ToString();
                            }
                            else
                            {
                                order.Category2 = dr["ProductSubCategory"].ToString();
                            }


                            //if (dr["Category"] != DBNull.Value)
                            //    order.Category1 = dr["Category"].ToString();

                            //if (dr["ProductSubCategory"] != DBNull.Value)
                            //    order.Category2 = dr["ProductSubCategory"].ToString();

                            order.OEM = dr["CarOEM"].ToString();

                            if (dr["SendIn"] != DBNull.Value)
                                order.QuoteSentDate = Convert.ToDateTime(dr["SendIn"]).ToString("yyyy-MM-dd");

                            order.EmdepSite = SiteName;

                            if (dr["RFQReception"] != DBNull.Value)
                                order.RfqReceptionDate = Convert.ToDateTime(dr["RFQReception"]).ToString("yyyy-MM-dd");

                            if (dr["DeliveryDate"] != DBNull.Value)
                                order.PO = Convert.ToDateTime(dr["DeliveryDate"]).ToString("yyyy-MM-dd");

                            orders.Add(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            orders = GetInvoicesAmountDateWise(FromDate, ToDate, ref orders,IdCurrency);
            return GetshipmentDate(connstr,ref orders);
        }
        public List<Order> GetInvoicesAmountDateWise(DateTime FromDate, DateTime ToDate, ref List<Order> orders,int Idcurrency)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("offers_GetInvoicesAmountDateWise", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600;
                    command.Parameters.AddWithValue("_idcurrency", Idcurrency);
                    command.Parameters.AddWithValue("_idUser", 0);
                    command.Parameters.AddWithValue("_idZone", 0);
                    command.Parameters.AddWithValue("_accountingYearFrom", FromDate);
                    command.Parameters.AddWithValue("_accountingYearTo", ToDate);
                    command.Parameters.AddWithValue("_idPermission", 22);

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Order order = orders.Find(o => o.Id == Convert.ToInt64(dr["IdOffer"].ToString()));
                            if(order!=null)
                            {
                                order.SourceInvoiceCurrency = dr["InvoiceCurrency"].ToString();
                                if (dr["InvoiceAmount"] != DBNull.Value)
                                    order.SourceInvoiceAmount = Convert.ToDouble(dr["InvoiceAmount"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orders;
        }

        public List<Order> GetshipmentDate(string strconn, ref List<Order> orders)
        {
            string IDOffers = String.Join(",", ((List<Order>)orders).ConvertAll<string>(d => d.Id.ToString()).ToArray());
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strconn))
                {
                    conn.Open();
                    MySqlCommand Command = new MySqlCommand("offers_GetAllShipmentDateBySelectedIdOffer", conn);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("_idoffer", IDOffers);

                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["idoffer"] != DBNull.Value)
                            {
                                Order offer = orders.Where(i => i.Id == Convert.ToInt64(reader["idoffer"].ToString())).FirstOrDefault();
                                if (reader["ShipmentDate"] != DBNull.Value)
                                    offer.ShipmentDate = Convert.ToDateTime(reader["ShipmentDate"].ToString()).ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return orders;
        }
        #endregion
    }
}
