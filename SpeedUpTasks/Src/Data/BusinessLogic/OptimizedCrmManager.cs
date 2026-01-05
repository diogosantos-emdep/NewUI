using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Xml;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Transactions;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Utility;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class OptimizedCrmManager
    {

        public List<TempAppointment> GetDailyEvents(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<TempAppointment> tempAppointments = new List<TempAppointment>();
            TempAppointment tempAppointment = null;

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(compdetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("offers_GetDailyEvents", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);
                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        tempAppointment = new TempAppointment();
                        if (offerreader["OfferExpectedPODate"] != DBNull.Value)
                            tempAppointment.StartTime = Convert.ToDateTime(offerreader["OfferExpectedPODate"].ToString());
                        if (offerreader["OfferExpectedPODate"] != DBNull.Value)
                            tempAppointment.EndTime = Convert.ToDateTime(offerreader["OfferExpectedPODate"].ToString());
                        tempAppointment.IsOfferDonePO = false;
                        if (offerreader["IdSalesStatusType"] != DBNull.Value)
                            tempAppointment.Label = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString());
                        if (offerreader["OfferStatusid"] != DBNull.Value && offerreader["IdSalesStatusType"] != DBNull.Value)
                            tempAppointment.GeosStatus = new GeosStatus { IdOfferStatusType = Convert.ToInt64(offerreader["OfferStatusid"].ToString()), Name = offerreader["OfferStatus"].ToString(), HtmlColor = offerreader["OfferStatusColor"].ToString(), SalesStatusType = new SalesStatusType { IdSalesStatusType = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString()), Name = offerreader["SalesStatusType"].ToString(), HtmlColor = offerreader["SalesStatusTypeColor"].ToString() } };
                        if (offerreader["idoffer"] != DBNull.Value)
                            tempAppointment.IdOffer = Convert.ToInt64(offerreader["idoffer"].ToString());
                        tempAppointment.Subject = offerreader["OfferTitle"].ToString();
                        tempAppointment.Description = offerreader["OfferTitle"].ToString();
                        tempAppointment.ConnectPlantId = compdetails.ConnectPlantId;
                        if (offerreader["OfferExpectedPODate"] != DBNull.Value)
                            tempAppointment.OfferExpectedDate = Convert.ToDateTime(offerreader["OfferExpectedPODate"].ToString());
                        if (offerreader["RFQReception"] != DBNull.Value)
                            tempAppointment.RfqReception = Convert.ToDateTime(offerreader["RFQReception"].ToString());
                        if (offerreader["SendIn"] != DBNull.Value)
                            tempAppointment.SendIn = Convert.ToDateTime(offerreader["SendIn"].ToString());

                        tempAppointments.Add(tempAppointment);
                    }

                    if (offerreader.NextResult())
                    {
                        while (offerreader.Read())
                        {
                            if (tempAppointments.Any(mapp => mapp.IdOffer != Convert.ToInt64(offerreader["idoffer"].ToString())))
                            {
                                tempAppointment = new TempAppointment();
                                if (offerreader["PoReceptionDate"] != DBNull.Value)
                                    tempAppointment.StartTime = Convert.ToDateTime(offerreader["PoReceptionDate"].ToString());
                                if (offerreader["PoReceptionDate"] != DBNull.Value)
                                    tempAppointment.EndTime = Convert.ToDateTime(offerreader["PoReceptionDate"].ToString());
                                tempAppointment.IsOfferDonePO = true;
                                if (offerreader["IdSalesStatusType"] != DBNull.Value)
                                    tempAppointment.Label = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString());

                                if (offerreader["IdStatus"] != DBNull.Value && offerreader["IdSalesStatusType"] != DBNull.Value)
                                    tempAppointment.GeosStatus = new GeosStatus { IdOfferStatusType = Convert.ToInt64(offerreader["IdStatus"].ToString()), Name = offerreader["OfferStatus"].ToString(), HtmlColor = offerreader["OfferStatusColor"].ToString(), SalesStatusType = new SalesStatusType { IdSalesStatusType = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString()), Name = offerreader["Status"].ToString(), HtmlColor = offerreader["SalesStatusTypeColor"].ToString() } };

                                if (offerreader["idoffer"] != DBNull.Value)
                                    tempAppointment.IdOffer = Convert.ToInt64(offerreader["idoffer"].ToString());
                                tempAppointment.Subject = offerreader["OfferTitle"].ToString();
                                tempAppointment.Description = offerreader["OfferTitle"].ToString();
                                tempAppointment.ConnectPlantId = compdetails.ConnectPlantId;
                                if (offerreader["IsGoAhead"] != DBNull.Value)
                                    tempAppointment.IsGoAhead = Convert.ToByte(offerreader["IsGoAhead"].ToString());
                                if (offerreader["PoReceptionDate"] != DBNull.Value)
                                    tempAppointment.PoReceivedInDate = Convert.ToDateTime(offerreader["PoReceptionDate"].ToString());
                                if (offerreader["DeliveryDate"] != DBNull.Value)
                                    tempAppointment.OtsDeliveryDate = Convert.ToDateTime(offerreader["DeliveryDate"].ToString());

                                tempAppointments.Add(tempAppointment);
                            }
                        }

                    }
                }
            }

            return tempAppointments;
        }

        public List<CarProjectDetail> GetTopCarProjectOffersOptimization(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 projectLimit, Int32 idUserPermission, Company companydetails, Int32 idCurrentUser = 0)
        {
            List<CarProjectDetail> carProjects = new List<CarProjectDetail>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(companydetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("offers_GetTopCarProjectALLOffers", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_projectLimit", projectLimit);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);

                using (MySqlDataReader itemRow = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    if (itemRow.HasRows)
                    {
                        while (itemRow.Read())
                        {
                            CarProjectDetail carProject = new CarProjectDetail();
                            carProject.IdCarProject = Convert.ToInt64(itemRow["IdCarProject"].ToString());
                            carProject.Name = itemRow["Name"].ToString();

                            if (itemRow["OfferAmount"] != DBNull.Value)
                                carProject.ProjectOfferAmount = Convert.ToDouble(itemRow["OfferAmount"].ToString());

                            carProject.Offers = new List<OfferDetail>();
                            carProjects.Add(carProject);
                        }
                    }
                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {
                            OfferDetail offer = new OfferDetail();

                            if (itemRow["idOffer"] != DBNull.Value)
                                offer.IdOffer = Convert.ToInt64(itemRow["idOffer"].ToString());

                            offer.Code = itemRow["Code"].ToString();
                            offer.Description = itemRow["OfferTitle"].ToString();
                            offer.SiteName = itemRow["CustomerName"].ToString();
                            offer.CountryName = itemRow["Country"].ToString();

                            offer.ConnectPlantId = Convert.ToInt32(companydetails.ConnectPlantId);

                            if (itemRow["IdCustomer"] != DBNull.Value)
                                offer.IdCustomer = Convert.ToInt32(itemRow["IdCustomer"].ToString());

                            offer.CustomerName = itemRow["CustomerGroup"].ToString();

                            if (itemRow["OfferAmount"] != DBNull.Value)
                                offer.Value = Convert.ToDouble(itemRow["OfferAmount"].ToString());

                            offer.CurrName = itemRow["OfferCurrency"].ToString();
                            offer.CurrSymbol = itemRow["Symbol"].ToString();

                            if (itemRow["IdCarProject"] != DBNull.Value)
                                offer.IdCarProject = Convert.ToInt64(itemRow["IdCarProject"].ToString());

                            if (itemRow["ExpectedPoReceptionDate"] != DBNull.Value)
                                offer.OfferExpectedDate = Convert.ToDateTime(itemRow["ExpectedPoReceptionDate"].ToString());

                            if (itemRow["IdCarOEM"] != DBNull.Value)
                            {
                                offer.IdCarOEM = Convert.ToInt32(itemRow["IdCarOEM"].ToString());

                                offer.CarOEMName = itemRow["CarOEM"].ToString();
                            }
                            if (itemRow["IdCarProject"] == DBNull.Value)
                            {
                                CarProjectDetail carProject = carProjects.Where(i => i.IdCarProject == 0).FirstOrDefault();
                                carProject.Offers.Add(offer);
                            }
                            else
                            {
                                CarProjectDetail carProject = carProjects.Where(i => i.IdCarProject == Convert.ToInt64(itemRow["IdCarProject"])).FirstOrDefault();
                                carProject.Offers.Add(offer);
                            }


                        }
                    }
                }
            }

            return carProjects;
        }

        public List<OfferDetail> GetSalesStatusWithTarget(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offers = new List<OfferDetail>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(companydetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("offers_GetSalesStatusWithTarget", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", idPermission);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);


                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        OfferDetail offer = new OfferDetail();
                        if (offerreader["OfferStatusId"] != DBNull.Value)
                            offer.IdStatus = Convert.ToInt64(offerreader["OfferStatusId"].ToString());
                        offer.ConnectPlantId = Convert.ToInt32(companydetails.ConnectPlantId);
                        offer.GeosStatusName = offerreader["OfferStatusName"].ToString();
                        offer.SaleStatusName = offerreader["Status"].ToString();
                        if (offerreader["StatusIdImage"] != DBNull.Value)
                            offer.SaleStatusIdImage = Convert.ToInt64(offerreader["StatusIdImage"].ToString());
                        if (offerreader["NumberOfOffers"] != DBNull.Value)
                            offer.NumberOfOffers = Convert.ToInt64(offerreader["NumberOfOffers"].ToString());
                        if (offerreader["TotalOfferAmount"] != DBNull.Value)
                            offer.Value = Convert.ToDouble(offerreader["TotalOfferAmount"].ToString());
                        offer.CurrName = offerreader["OfferCurrency"].ToString();
                        offer.CurrSymbol = offerreader["Symbol"].ToString();

                        offers.Add(offer);
                    }

                    if (offerreader.NextResult())
                    {
                        while (offerreader.Read())
                        {
                            OfferDetail offertarget = new OfferDetail();

                            offertarget.ConnectPlantId = Convert.ToInt32(companydetails.ConnectPlantId);

                            offertarget.SaleStatusName = "TARGET";
                            offertarget.SaleStatusIdImage = 0;
                            if (offerreader["SalesTarget"] != DBNull.Value)
                                offertarget.Value = Convert.ToDouble(offerreader["SalesTarget"]);
                            offertarget.CurrName = offerreader["Name"].ToString();
                            offertarget.CurrSymbol = offerreader["Symbol"].ToString();
                            offers.Add(offertarget);
                        }
                    }
                }
            }

            return offers;
        }



        public List<OfferDetail> GetSalesStatusByMonthAllPermission(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offers = new List<OfferDetail>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(companyDetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();

                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("offers_GetSalesStatusByMonthAllPermission", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);


                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        OfferDetail offer = new OfferDetail();
                        if (offerreader["OfferStatusId"] != DBNull.Value)
                            offer.IdStatus = Convert.ToInt64(offerreader["OfferStatusId"].ToString());
                        if (offerreader["CurrentMonth"] != DBNull.Value)
                            offer.CurrentMonth = Convert.ToInt32(offerreader["CurrentMonth"].ToString());
                        if (offerreader["CurrentYear"] != DBNull.Value)
                            offer.CurrentYear = Convert.ToInt32(offerreader["CurrentYear"].ToString());
                        offer.GeosStatusName = offerreader["OfferStatusName"].ToString();
                        if (offerreader["NumberOfOffers"] != DBNull.Value)
                            offer.NumberOfOffers = Convert.ToInt64(offerreader["NumberOfOffers"].ToString());

                        if (offerreader["TotalOfferAmount"] != DBNull.Value)
                            offer.Value = Convert.ToDouble(offerreader["TotalOfferAmount"].ToString());

                        offer.CurrName = offerreader["OfferCurrency"].ToString();
                        offer.CurrSymbol = offerreader["Symbol"].ToString();

                        offer.ConnectPlantId = Convert.ToInt32(companyDetails.ConnectPlantId);

                        offer.SaleStatusName = offerreader["Status"].ToString();

                        offers.Add(offer);
                    }
                }
            }
            // }
            return offers;
        }

        public List<DailyEventActivity> GetDailyEventActivities(string idOwner, Int32 idPermission, string idPlant, string mainconnectionstring)
        {
            List<DailyEventActivity> listactivity = new List<DailyEventActivity>();

            using (MySqlConnection congetactivities = new MySqlConnection(mainconnectionstring))
            {
                congetactivities.Open();
                MySqlCommand congetactivitiescommand = new MySqlCommand("activities_GetactivitiesByIdPermission", congetactivities);
                congetactivitiescommand.CommandType = CommandType.StoredProcedure;
                congetactivitiescommand.CommandTimeout = 3000;
                congetactivitiescommand.Parameters.AddWithValue("_IdOwner", idOwner);
                congetactivitiescommand.Parameters.AddWithValue("_IdPermission", idPermission);
                congetactivitiescommand.Parameters.AddWithValue("_IdPlant", idPlant);

                using (MySqlDataReader activityreader = congetactivitiescommand.ExecuteReader())
                {
                    while (activityreader.Read())
                    {
                        DailyEventActivity activity = new DailyEventActivity();
                        try
                        {
                            if (activityreader["IdActivity"] != DBNull.Value)
                                activity.IdActivity = Convert.ToInt64(activityreader["IdActivity"].ToString());

                            activity.Subject = activityreader["Subject"].ToString();

                            if (activityreader["IdActivityType"] != DBNull.Value)
                            {

                                activity.IdActivityType = Convert.ToInt32(activityreader["IdActivityType"].ToString());
                                activity.ActivityType = activityreader["ActivityType"].ToString();
                                activity.ActivityTypeHtmlColor = activityreader["ActivityTypeHtmlColor"].ToString();

                            }

                            if (activityreader["FromDate"] != DBNull.Value)
                                activity.FromDate = Convert.ToDateTime(activityreader["FromDate"].ToString());

                            if (activityreader["ToDate"] != DBNull.Value)
                                activity.ToDate = Convert.ToDateTime(activityreader["ToDate"].ToString());



                            if (activityreader["IdOwner"] != DBNull.Value)
                                activity.IdOwner = Convert.ToInt32(activityreader["IdOwner"].ToString());


                            listactivity.Add(activity);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }

                    }
                }
            }

            return listactivity;
        }


        public List<OfferMonthDetail> GetSalesStatusByMonthWithTarget(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false)
        {
            List<OfferMonthDetail> offers = new List<OfferMonthDetail>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(companyDetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();

                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("offers_GetSalesStatusByMonthWithTarget", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;

                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;

                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_assignedPlant", assignedPlant);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_isSiteTarget", isSiteTarget);

                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        OfferMonthDetail offer = new OfferMonthDetail();
                        if (offerreader["OfferStatusId"] != DBNull.Value)
                            offer.IdStatus = Convert.ToInt64(offerreader["OfferStatusId"].ToString());
                        if (offerreader["CurrentMonth"] != DBNull.Value)
                            offer.CurrentMonth = Convert.ToInt32(offerreader["CurrentMonth"].ToString());
                        if (offerreader["CurrentYear"] != DBNull.Value)
                            offer.CurrentYear = Convert.ToInt32(offerreader["CurrentYear"].ToString());
                        offer.GeosStatusName = offerreader["OfferStatusName"].ToString();

                        if (offerreader["TotalOfferAmount"] != DBNull.Value)
                            offer.Value = Convert.ToDouble(offerreader["TotalOfferAmount"].ToString());

                        offer.CurrName = offerreader["OfferCurrency"].ToString();
                        offer.CurrSymbol = offerreader["Symbol"].ToString();

                        offer.ConnectPlantId = Convert.ToInt32(companyDetails.ConnectPlantId);

                        offer.SaleStatusName = offerreader["Status"].ToString();

                        offers.Add(offer);
                    }
                    if (isSiteTarget)
                    {
                        if (offerreader.NextResult())
                        {
                            if (offerreader.Read())
                            {
                                if (offerreader["Year"] != DBNull.Value)
                                    offers[0].Year = Convert.ToInt32(offerreader["Year"].ToString());

                                if (offerreader["SalesQuotaAmount"] != DBNull.Value)
                                    offers[0].SalesQuotaAmount = Convert.ToDouble(offerreader["SalesQuotaAmount"].ToString());

                                if (offerreader["SalesQuotaAmountWithExchangeRate"] != DBNull.Value)
                                    offers[0].QuotaAmountWithExchangeRate = Convert.ToDouble(offerreader["SalesQuotaAmountWithExchangeRate"].ToString());

                                if (offerreader["IdSalesQuotaCurrency"] != DBNull.Value)
                                    offers[0].IdSalesQuotaCurrency = Convert.ToByte(offerreader["IdSalesQuotaCurrency"].ToString());

                                if (offerreader["ExchangeRateDate"] != DBNull.Value)
                                    offers[0].ExchangeRateDate = Convert.ToDateTime(offerreader["ExchangeRateDate"].ToString());
                            }
                        }
                    }
                }
            }
            // }
            return offers;
        }

        public List<OrderGrid> GetOrderGridDetails(OrderParams orderParams)
        {
            List<OrderGrid> orderGridRows = new List<Common.OrderGrid>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(orderParams.connectSiteDetailParams.ConnectionString))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("offers_GetRestfulOrders", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;

                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", orderParams.idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", orderParams.idsSelectedUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", orderParams.idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", orderParams.accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", orderParams.accountingYearTo);
                if (orderParams.Roles == RoleType.SalesAssistant)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 20);
                else if (orderParams.Roles == RoleType.SalesPlantManager)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 21);
                else if (orderParams.Roles == RoleType.SalesGlobalManager)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 22);

                using (MySqlDataReader itemRow = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    if (itemRow.HasRows)
                    {
                        while (itemRow.Read())
                        {
                            OrderGrid orderGrid = new OrderGrid();
                            orderGrid.IdOffer = Convert.ToInt64(itemRow["idoffer"].ToString());
                            orderGrid.Code = itemRow["OfferCode"].ToString();
                            orderGrid.Description = itemRow["OfferTitle"].ToString();
                            orderGrid.Plant = itemRow["CustomerName"].ToString();

                            if (itemRow["IdSalesOwner"] != DBNull.Value)
                            {
                                orderGrid.IdSalesOwner = Convert.ToInt32(itemRow["IdSalesOwner"].ToString());
                            }

                            if (itemRow["IdSalesResponsible"] != DBNull.Value)
                            {
                                orderGrid.IdSalesResponsible = Convert.ToInt32(itemRow["IdSalesResponsible"].ToString());
                            }

                            if (itemRow["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            {
                                orderGrid.IdSalesResponsibleAssemblyBU = Convert.ToInt32(itemRow["IdSalesResponsibleAssemblyBU"].ToString());
                            }

                            orderGrid.ConnectPlantId = orderParams.connectSiteDetailParams.idSite;
                            orderGrid.Group = itemRow["CustomerGroup"].ToString();
                            orderGrid.Country = itemRow["CustomerCountry"].ToString();
                            orderGrid.Region = itemRow["CustomerZone"].ToString();
                            orderGrid.Status = itemRow["OfferStatus"].ToString();
                            orderGrid.HtmlColor = itemRow["OfferStatusColor"].ToString();

                            if (itemRow["OfferAmount"] != DBNull.Value)
                                orderGrid.Amount = Convert.ToDouble(itemRow["OfferAmount"].ToString());
                            else
                                orderGrid.Amount = 0;

                            //  offer.InvoiceAmount = (from record in offerInvoices where record.IdOffer == offer.IdOffer select record.Value).SingleOrDefault();
                            orderGrid.Currency = itemRow["OfferCurrency"].ToString();

                            if (itemRow["PoReceptionDate"] != DBNull.Value)
                                orderGrid.PoReceptionDate = Convert.ToDateTime(itemRow["PoReceptionDate"].ToString()).ToString("yyyy-MM-dd"); ;

                            if (itemRow["DeliveryDate"] != DBNull.Value)
                                orderGrid.OfferCloseDate = Convert.ToDateTime(itemRow["DeliveryDate"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.OfferCloseDate = null;

                            if (itemRow["RFQReception"] != DBNull.Value)
                                orderGrid.RfqReceptionDate = Convert.ToDateTime(itemRow["RFQReception"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.RfqReceptionDate = null;

                            if (itemRow["SendIn"] != DBNull.Value)
                                orderGrid.QuoteSentDate = Convert.ToDateTime(itemRow["SendIn"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.QuoteSentDate = null;

                            if (itemRow["IdBusinessUnit"] != DBNull.Value)
                                orderGrid.IdBusinessUnit = Convert.ToInt32(itemRow["IdBusinessUnit"].ToString());

                            if (itemRow["IdCarProject"] != DBNull.Value)
                                orderGrid.IdCarProject = Convert.ToInt64(itemRow["IdCarProject"].ToString());

                            if (itemRow["IdCarOEM"] != DBNull.Value)
                                orderGrid.IdCarOEM = Convert.ToInt32(itemRow["IdCarOEM"].ToString());

                            if (itemRow["IdSource"] != DBNull.Value)
                                orderGrid.IdSource = Convert.ToInt32(itemRow["IdSource"].ToString());


                            if (itemRow["IdProductSubCategory"] != DBNull.Value)
                            {
                                orderGrid.IdProductCategory = Convert.ToInt64(itemRow["IdProductSubCategory"].ToString());
                                orderGrid.ProductCategory = new ProductCategoryGrid();
                            }


                            orderGrid.OptionsByOffers = new List<OptionsByOfferGrid>();

                            orderGridRows.Add(orderGrid);
                        }
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {
                            OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();

                            OptionsByOfferGrid optionsByOfferGrid = new OptionsByOfferGrid();
                            optionsByOfferGrid.IdOffer = Convert.ToInt64(itemRow["IdOffer"].ToString());
                            optionsByOfferGrid.IdOption = Convert.ToInt64(itemRow["IdOption"].ToString());
                            optionsByOfferGrid.IdOfferPlant = Convert.ToInt32(orderParams.connectSiteDetailParams.idSite);
                            if (itemRow["Quantity"] != DBNull.Value)
                            {
                                optionsByOfferGrid.Quantity = Convert.ToInt32(itemRow["Quantity"].ToString());
                                if (optionsByOfferGrid.Quantity > 0)
                                    optionsByOfferGrid.IsSelected = true;
                                else
                                    optionsByOfferGrid.IsSelected = false;
                            }
                            else
                            {
                                optionsByOfferGrid.IsSelected = false;
                            }
                            optionsByOfferGrid.OfferOption = itemRow["offeroption"].ToString();

                            orderGridRow.OptionsByOffers.Add(optionsByOfferGrid);
                        }
                    }


                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {
                            if (itemRow["IdOffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();

                                if (itemRow["InvoiceAmount"] != DBNull.Value)
                                    orderGridRow.InvoiceAmount = Convert.ToDouble(itemRow["InvoiceAmount"].ToString());
                            }

                        }
                    }


                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["IdProductSubCategory"] != DBNull.Value)
                            {
                                orderGridRows.Where(i => i.IdProductCategory == Convert.ToInt64(itemRow["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.ProductCategory = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(itemRow["IdProductSubCategory"].ToString()), Name = itemRow["ProductSubCategory"].ToString() }; });

                                if (itemRow["IdCategory"] != DBNull.Value)
                                {
                                    orderGridRows.Where(i => i.IdProductCategory == Convert.ToInt64(itemRow["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.ProductCategory.IdParent = Convert.ToInt64(itemRow["IdCategory"].ToString()); u.ProductCategory.Category = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(itemRow["IdCategory"].ToString()), Name = itemRow["Category"].ToString() }; });
                                }
                            }

                        }
                    }

                    if (itemRow.NextResult())
                    {
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["idoffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();


                                if (orderGridRow != null)
                                {
                                    if (string.IsNullOrEmpty(orderGridRow.CustomerPOCodes))
                                    {
                                        orderGridRow.CustomerPOCodes = string.Format(itemRow["Code"].ToString());
                                    }
                                    else
                                    {
                                        orderGridRow.CustomerPOCodes = string.Format(orderGridRow.CustomerPOCodes + "\n" + itemRow["Code"].ToString());
                                    }
                                }
                            }

                        }
                    }

                }
            }

            string result = String.Join(",", ((List<OrderGrid>)orderGridRows).ConvertAll<string>(d => d.IdOffer.ToString()).ToArray());

            try
            {
                using (MySqlConnection conn = new MySqlConnection(orderParams.connectSiteDetailParams.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand Command = new MySqlCommand("offers_GetShipmentDatesOfIdOffers", conn);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("_idoffer", result);

                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["idoffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(reader["IdOffer"].ToString())).FirstOrDefault();

                                if (reader["ShipmentDate"] != DBNull.Value)
                                    orderGridRow.ShipmentDate = Convert.ToDateTime(reader["ShipmentDate"].ToString()).ToString("yyyy-MM-dd"); ;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return orderGridRows;
        }


        public List<OrderGrid> GetOrderGridDetails_V2035(OrderParams orderParams)
        {
            List<OrderGrid> orderGridRows = new List<Common.OrderGrid>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(orderParams.connectSiteDetailParams.ConnectionString))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("CRM_GetRestfulOrders_V2035", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 8000;

                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", orderParams.idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", orderParams.idsSelectedUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", orderParams.idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", orderParams.accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", orderParams.accountingYearTo);
                if (orderParams.Roles == RoleType.SalesAssistant)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 20);
                else if (orderParams.Roles == RoleType.SalesPlantManager)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 21);
                else if (orderParams.Roles == RoleType.SalesGlobalManager)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 22);

                using (MySqlDataReader itemRow = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    if (itemRow.HasRows)
                    {
                        while (itemRow.Read())
                        {
                            OrderGrid orderGrid = new OrderGrid();
                            orderGrid.IdOffer = Convert.ToInt64(itemRow["idoffer"].ToString());
                            orderGrid.Code = itemRow["OfferCode"].ToString();
                            orderGrid.Description = itemRow["OfferTitle"].ToString();
                            orderGrid.Plant = itemRow["CustomerName"].ToString();

                            if (itemRow["IdSalesOwner"] != DBNull.Value)
                            {
                                orderGrid.IdSalesOwner = Convert.ToInt32(itemRow["IdSalesOwner"].ToString());
                            }

                            if (itemRow["IdSalesResponsible"] != DBNull.Value)
                            {
                                orderGrid.IdSalesResponsible = Convert.ToInt32(itemRow["IdSalesResponsible"].ToString());
                            }

                            if (itemRow["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            {
                                orderGrid.IdSalesResponsibleAssemblyBU = Convert.ToInt32(itemRow["IdSalesResponsibleAssemblyBU"].ToString());
                            }

                            orderGrid.ConnectPlantId = orderParams.connectSiteDetailParams.idSite;
                            orderGrid.Group = itemRow["CustomerGroup"].ToString();
                            orderGrid.Country = itemRow["CustomerCountry"].ToString();
                            orderGrid.Region = itemRow["CustomerZone"].ToString();
                            orderGrid.Status = itemRow["OfferStatus"].ToString();
                            orderGrid.HtmlColor = itemRow["OfferStatusColor"].ToString();

                            if (itemRow["OfferAmount"] != DBNull.Value)
                                orderGrid.Amount = Convert.ToDouble(itemRow["OfferAmount"].ToString());
                            else
                                orderGrid.Amount = 0;

                            //  offer.InvoiceAmount = (from record in offerInvoices where record.IdOffer == offer.IdOffer select record.Value).SingleOrDefault();
                            orderGrid.Currency = itemRow["OfferCurrency"].ToString();

                            if (itemRow["PoReceptionDate"] != DBNull.Value)
                                orderGrid.PoReceptionDate = Convert.ToDateTime(itemRow["PoReceptionDate"].ToString()).ToString("yyyy-MM-dd"); ;

                            if (itemRow["DeliveryDate"] != DBNull.Value)
                                orderGrid.OfferCloseDate = Convert.ToDateTime(itemRow["DeliveryDate"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.OfferCloseDate = null;

                            if (itemRow["RFQReception"] != DBNull.Value)
                                orderGrid.RfqReceptionDate = Convert.ToDateTime(itemRow["RFQReception"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.RfqReceptionDate = null;

                            if (itemRow["SendIn"] != DBNull.Value)
                                orderGrid.QuoteSentDate = Convert.ToDateTime(itemRow["SendIn"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.QuoteSentDate = null;

                            if (itemRow["IdBusinessUnit"] != DBNull.Value)
                                orderGrid.IdBusinessUnit = Convert.ToInt32(itemRow["IdBusinessUnit"].ToString());

                            if (itemRow["IdCarProject"] != DBNull.Value)
                                orderGrid.IdCarProject = Convert.ToInt64(itemRow["IdCarProject"].ToString());

                            if (itemRow["IdCarOEM"] != DBNull.Value)
                                orderGrid.IdCarOEM = Convert.ToInt32(itemRow["IdCarOEM"].ToString());

                            if (itemRow["IdSource"] != DBNull.Value)
                                orderGrid.IdSource = Convert.ToInt32(itemRow["IdSource"].ToString());


                            if (itemRow["IdProductSubCategory"] != DBNull.Value)
                            {
                                orderGrid.IdProductCategory = Convert.ToInt64(itemRow["IdProductSubCategory"].ToString());
                                orderGrid.ProductCategory = new ProductCategoryGrid();
                            }


                            orderGrid.OptionsByOffers = new List<OptionsByOfferGrid>();

                            orderGridRows.Add(orderGrid);
                        }
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {
                            OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();

                            OptionsByOfferGrid optionsByOfferGrid = new OptionsByOfferGrid();
                            optionsByOfferGrid.IdOffer = Convert.ToInt64(itemRow["IdOffer"].ToString());
                            optionsByOfferGrid.IdOption = Convert.ToInt64(itemRow["IdOption"].ToString());
                            optionsByOfferGrid.IdOfferPlant = Convert.ToInt32(orderParams.connectSiteDetailParams.idSite);
                            if (itemRow["Quantity"] != DBNull.Value)
                            {
                                optionsByOfferGrid.Quantity = Convert.ToInt32(itemRow["Quantity"].ToString());
                                if (optionsByOfferGrid.Quantity > 0)
                                    optionsByOfferGrid.IsSelected = true;
                                else
                                    optionsByOfferGrid.IsSelected = false;
                            }
                            else
                            {
                                optionsByOfferGrid.IsSelected = false;
                            }
                            optionsByOfferGrid.OfferOption = itemRow["offeroption"].ToString();

                            orderGridRow.OptionsByOffers.Add(optionsByOfferGrid);
                        }
                    }


                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {
                            if (itemRow["IdOffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();

                                if (itemRow["InvoiceAmount"] != DBNull.Value)
                                    orderGridRow.InvoiceAmount = Convert.ToDouble(itemRow["InvoiceAmount"].ToString());
                            }

                        }
                    }


                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["IdProductSubCategory"] != DBNull.Value)
                            {
                                orderGridRows.Where(i => i.IdProductCategory == Convert.ToInt64(itemRow["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.ProductCategory = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(itemRow["IdProductSubCategory"].ToString()), Name = itemRow["ProductSubCategory"].ToString() }; });

                                if (itemRow["IdCategory"] != DBNull.Value)
                                {
                                    orderGridRows.Where(i => i.IdProductCategory == Convert.ToInt64(itemRow["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.ProductCategory.IdParent = Convert.ToInt64(itemRow["IdCategory"].ToString()); u.ProductCategory.Category = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(itemRow["IdCategory"].ToString()), Name = itemRow["Category"].ToString() }; });
                                }
                            }

                        }
                    }

                    if (itemRow.NextResult())
                    {
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["idoffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();


                                if (orderGridRow != null)
                                {
                                    if (string.IsNullOrEmpty(orderGridRow.CustomerPOCodes))
                                    {
                                        orderGridRow.CustomerPOCodes = string.Format(itemRow["Code"].ToString());
                                    }
                                    else
                                    {
                                        orderGridRow.CustomerPOCodes = string.Format(orderGridRow.CustomerPOCodes + "\n" + itemRow["Code"].ToString());
                                    }
                                }
                            }

                        }
                    }

                }
            }

            string result = String.Join(",", ((List<OrderGrid>)orderGridRows).ConvertAll<string>(d => d.IdOffer.ToString()).ToArray());

            try
            {
                using (MySqlConnection conn = new MySqlConnection(orderParams.connectSiteDetailParams.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand Command = new MySqlCommand("offers_GetShipmentDatesOfIdOffers", conn);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("_idoffer", result);

                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["idoffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(reader["IdOffer"].ToString())).FirstOrDefault();

                                if (reader["ShipmentDate"] != DBNull.Value)
                                    orderGridRow.ShipmentDate = Convert.ToDateTime(reader["ShipmentDate"].ToString()).ToString("yyyy-MM-dd"); ;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return orderGridRows;
        }

        public List<OfferDetail> GetSalesStatusWithTarget_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offers = new List<OfferDetail>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(companydetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("CRM_GetSalesStatusWithTarget_V2035", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", idPermission);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);


                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        OfferDetail offer = new OfferDetail();
                        if (offerreader["OfferStatusId"] != DBNull.Value)
                            offer.IdStatus = Convert.ToInt64(offerreader["OfferStatusId"].ToString());
                        offer.ConnectPlantId = Convert.ToInt32(companydetails.ConnectPlantId);
                        offer.GeosStatusName = offerreader["OfferStatusName"].ToString();
                        offer.SaleStatusName = offerreader["Status"].ToString();
                        if (offerreader["StatusIdImage"] != DBNull.Value)
                            offer.SaleStatusIdImage = Convert.ToInt64(offerreader["StatusIdImage"].ToString());
                        if (offerreader["NumberOfOffers"] != DBNull.Value)
                            offer.NumberOfOffers = Convert.ToInt64(offerreader["NumberOfOffers"].ToString());
                        if (offerreader["TotalOfferAmount"] != DBNull.Value)
                            offer.Value = Convert.ToDouble(offerreader["TotalOfferAmount"].ToString());
                        offer.CurrName = offerreader["OfferCurrency"].ToString();
                        offer.CurrSymbol = offerreader["Symbol"].ToString();

                        offers.Add(offer);
                    }

                    if (offerreader.NextResult())
                    {
                        while (offerreader.Read())
                        {
                            OfferDetail offertarget = new OfferDetail();

                            offertarget.ConnectPlantId = Convert.ToInt32(companydetails.ConnectPlantId);

                            offertarget.SaleStatusName = "TARGET";
                            offertarget.SaleStatusIdImage = 0;
                            if (offerreader["SalesTarget"] != DBNull.Value)
                                offertarget.Value = Convert.ToDouble(offerreader["SalesTarget"]);
                            offertarget.CurrName = offerreader["Name"].ToString();
                            offertarget.CurrSymbol = offerreader["Symbol"].ToString();
                            offers.Add(offertarget);
                        }
                    }
                }
            }

            return offers;
        }


        public List<TempAppointment> GetDailyEvents_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<TempAppointment> tempAppointments = new List<TempAppointment>();
            TempAppointment tempAppointment = null;

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(compdetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("CRM_GetDailyEvents_V2035", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);
                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        tempAppointment = new TempAppointment();
                        if (offerreader["OfferExpectedPODate"] != DBNull.Value)
                            tempAppointment.StartTime = Convert.ToDateTime(offerreader["OfferExpectedPODate"].ToString());
                        if (offerreader["OfferExpectedPODate"] != DBNull.Value)
                            tempAppointment.EndTime = Convert.ToDateTime(offerreader["OfferExpectedPODate"].ToString());
                        tempAppointment.IsOfferDonePO = false;
                        if (offerreader["IdSalesStatusType"] != DBNull.Value)
                            tempAppointment.Label = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString());
                        if (offerreader["OfferStatusid"] != DBNull.Value && offerreader["IdSalesStatusType"] != DBNull.Value)
                            tempAppointment.GeosStatus = new GeosStatus { IdOfferStatusType = Convert.ToInt64(offerreader["OfferStatusid"].ToString()), Name = offerreader["OfferStatus"].ToString(), HtmlColor = offerreader["OfferStatusColor"].ToString(), SalesStatusType = new SalesStatusType { IdSalesStatusType = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString()), Name = offerreader["SalesStatusType"].ToString(), HtmlColor = offerreader["SalesStatusTypeColor"].ToString() } };
                        if (offerreader["idoffer"] != DBNull.Value)
                            tempAppointment.IdOffer = Convert.ToInt64(offerreader["idoffer"].ToString());
                        tempAppointment.Subject = offerreader["OfferTitle"].ToString();
                        tempAppointment.Description = offerreader["OfferTitle"].ToString();
                        tempAppointment.ConnectPlantId = compdetails.ConnectPlantId;
                        if (offerreader["OfferExpectedPODate"] != DBNull.Value)
                            tempAppointment.OfferExpectedDate = Convert.ToDateTime(offerreader["OfferExpectedPODate"].ToString());
                        if (offerreader["RFQReception"] != DBNull.Value)
                            tempAppointment.RfqReception = Convert.ToDateTime(offerreader["RFQReception"].ToString());
                        if (offerreader["SendIn"] != DBNull.Value)
                            tempAppointment.SendIn = Convert.ToDateTime(offerreader["SendIn"].ToString());

                        tempAppointments.Add(tempAppointment);
                    }

                    if (offerreader.NextResult())
                    {
                        while (offerreader.Read())
                        {
                            if (tempAppointments.Any(mapp => mapp.IdOffer != Convert.ToInt64(offerreader["idoffer"].ToString())))
                            {
                                tempAppointment = new TempAppointment();
                                if (offerreader["PoReceptionDate"] != DBNull.Value)
                                    tempAppointment.StartTime = Convert.ToDateTime(offerreader["PoReceptionDate"].ToString());
                                if (offerreader["PoReceptionDate"] != DBNull.Value)
                                    tempAppointment.EndTime = Convert.ToDateTime(offerreader["PoReceptionDate"].ToString());
                                tempAppointment.IsOfferDonePO = true;
                                if (offerreader["IdSalesStatusType"] != DBNull.Value)
                                    tempAppointment.Label = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString());

                                if (offerreader["IdStatus"] != DBNull.Value && offerreader["IdSalesStatusType"] != DBNull.Value)
                                    tempAppointment.GeosStatus = new GeosStatus { IdOfferStatusType = Convert.ToInt64(offerreader["IdStatus"].ToString()), Name = offerreader["OfferStatus"].ToString(), HtmlColor = offerreader["OfferStatusColor"].ToString(), SalesStatusType = new SalesStatusType { IdSalesStatusType = Convert.ToInt64(offerreader["IdSalesStatusType"].ToString()), Name = offerreader["Status"].ToString(), HtmlColor = offerreader["SalesStatusTypeColor"].ToString() } };

                                if (offerreader["idoffer"] != DBNull.Value)
                                    tempAppointment.IdOffer = Convert.ToInt64(offerreader["idoffer"].ToString());
                                tempAppointment.Subject = offerreader["OfferTitle"].ToString();
                                tempAppointment.Description = offerreader["OfferTitle"].ToString();
                                tempAppointment.ConnectPlantId = compdetails.ConnectPlantId;
                                if (offerreader["IsGoAhead"] != DBNull.Value)
                                    tempAppointment.IsGoAhead = Convert.ToByte(offerreader["IsGoAhead"].ToString());
                                if (offerreader["PoReceptionDate"] != DBNull.Value)
                                    tempAppointment.PoReceivedInDate = Convert.ToDateTime(offerreader["PoReceptionDate"].ToString());
                                if (offerreader["DeliveryDate"] != DBNull.Value)
                                    tempAppointment.OtsDeliveryDate = Convert.ToDateTime(offerreader["DeliveryDate"].ToString());

                                tempAppointments.Add(tempAppointment);
                            }
                        }

                    }
                }
            }

            return tempAppointments;
        }


        public List<OfferDetail> GetSalesStatusByMonthAllPermission_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offers = new List<OfferDetail>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(companyDetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();

                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("CRM_GetSalesStatusByMonthAllPermission_V2035", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);


                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        OfferDetail offer = new OfferDetail();
                        if (offerreader["OfferStatusId"] != DBNull.Value)
                            offer.IdStatus = Convert.ToInt64(offerreader["OfferStatusId"].ToString());
                        if (offerreader["CurrentMonth"] != DBNull.Value)
                            offer.CurrentMonth = Convert.ToInt32(offerreader["CurrentMonth"].ToString());
                        if (offerreader["CurrentYear"] != DBNull.Value)
                            offer.CurrentYear = Convert.ToInt32(offerreader["CurrentYear"].ToString());
                        offer.GeosStatusName = offerreader["OfferStatusName"].ToString();
                        if (offerreader["NumberOfOffers"] != DBNull.Value)
                            offer.NumberOfOffers = Convert.ToInt64(offerreader["NumberOfOffers"].ToString());

                        if (offerreader["TotalOfferAmount"] != DBNull.Value)
                            offer.Value = Convert.ToDouble(offerreader["TotalOfferAmount"].ToString());

                        offer.CurrName = offerreader["OfferCurrency"].ToString();
                        offer.CurrSymbol = offerreader["Symbol"].ToString();

                        offer.ConnectPlantId = Convert.ToInt32(companyDetails.ConnectPlantId);

                        offer.SaleStatusName = offerreader["Status"].ToString();

                        offers.Add(offer);
                    }
                }
            }
            // }
            return offers;
        }

        public List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_V2035(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false)
        {
            List<OfferMonthDetail> offers = new List<OfferMonthDetail>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(companyDetails.ConnectPlantConstr))
            {
                conofferwithoutpurchaseorder.Open();

                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("CRM_GetSalesStatusByMonthWithTarget_V2035", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;

                conofferwithoutpurchaseordercommand.CommandTimeout = 3000;

                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_assignedPlant", assignedPlant);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", accountingYearTo);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", idCurrentUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_isSiteTarget", isSiteTarget);

                using (MySqlDataReader offerreader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    while (offerreader.Read())
                    {
                        OfferMonthDetail offer = new OfferMonthDetail();
                        if (offerreader["OfferStatusId"] != DBNull.Value)
                            offer.IdStatus = Convert.ToInt64(offerreader["OfferStatusId"].ToString());
                        if (offerreader["CurrentMonth"] != DBNull.Value)
                            offer.CurrentMonth = Convert.ToInt32(offerreader["CurrentMonth"].ToString());
                        if (offerreader["CurrentYear"] != DBNull.Value)
                            offer.CurrentYear = Convert.ToInt32(offerreader["CurrentYear"].ToString());
                        offer.GeosStatusName = offerreader["OfferStatusName"].ToString();

                        if (offerreader["TotalOfferAmount"] != DBNull.Value)
                            offer.Value = Convert.ToDouble(offerreader["TotalOfferAmount"].ToString());

                        offer.CurrName = offerreader["OfferCurrency"].ToString();
                        offer.CurrSymbol = offerreader["Symbol"].ToString();

                        offer.ConnectPlantId = Convert.ToInt32(companyDetails.ConnectPlantId);

                        offer.SaleStatusName = offerreader["Status"].ToString();

                        offers.Add(offer);
                    }
                    if (isSiteTarget)
                    {
                        if (offerreader.NextResult())
                        {
                            if (offerreader.Read())
                            {
                                if (offerreader["Year"] != DBNull.Value)
                                    offers[0].Year = Convert.ToInt32(offerreader["Year"].ToString());

                                if (offerreader["SalesQuotaAmount"] != DBNull.Value)
                                    offers[0].SalesQuotaAmount = Convert.ToDouble(offerreader["SalesQuotaAmount"].ToString());

                                if (offerreader["SalesQuotaAmountWithExchangeRate"] != DBNull.Value)
                                    offers[0].QuotaAmountWithExchangeRate = Convert.ToDouble(offerreader["SalesQuotaAmountWithExchangeRate"].ToString());

                                if (offerreader["IdSalesQuotaCurrency"] != DBNull.Value)
                                    offers[0].IdSalesQuotaCurrency = Convert.ToByte(offerreader["IdSalesQuotaCurrency"].ToString());

                                if (offerreader["ExchangeRateDate"] != DBNull.Value)
                                    offers[0].ExchangeRateDate = Convert.ToDateTime(offerreader["ExchangeRateDate"].ToString());
                            }
                        }
                    }
                }
            }
            // }
            return offers;
        }


        public List<OrderGrid> GetOrderGridDetails_V2037(OrderParams orderParams)
        {
            List<OrderGrid> orderGridRows = new List<Common.OrderGrid>();

            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(orderParams.connectSiteDetailParams.ConnectionString))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("CRM_GetRestfulOrders_V2037", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;
                conofferwithoutpurchaseordercommand.CommandTimeout = 8000;

                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idcurrency", orderParams.idCurrency);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idUser", orderParams.idsSelectedUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idCurrentUser", orderParams.idUser);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearFrom", orderParams.accountingYearFrom);
                conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_accountingYearTo", orderParams.accountingYearTo);
                if (orderParams.Roles == RoleType.SalesAssistant)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 20);
                else if (orderParams.Roles == RoleType.SalesPlantManager)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 21);
                else if (orderParams.Roles == RoleType.SalesGlobalManager)
                    conofferwithoutpurchaseordercommand.Parameters.AddWithValue("_idPermission", 22);

                using (MySqlDataReader itemRow = conofferwithoutpurchaseordercommand.ExecuteReader())
                {
                    if (itemRow.HasRows)
                    {
                        while (itemRow.Read())
                        {
                            OrderGrid orderGrid = new OrderGrid();
                            orderGrid.IdOffer = Convert.ToInt64(itemRow["idoffer"].ToString());
                            orderGrid.Code = itemRow["OfferCode"].ToString();
                            orderGrid.Description = itemRow["OfferTitle"].ToString();
                            orderGrid.Plant = itemRow["CustomerName"].ToString();

                            if (itemRow["IdSalesOwner"] != DBNull.Value)
                            {
                                orderGrid.IdSalesOwner = Convert.ToInt32(itemRow["IdSalesOwner"].ToString());
                            }

                            if (itemRow["IdSalesResponsible"] != DBNull.Value)
                            {
                                orderGrid.IdSalesResponsible = Convert.ToInt32(itemRow["IdSalesResponsible"].ToString());
                            }

                            if (itemRow["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            {
                                orderGrid.IdSalesResponsibleAssemblyBU = Convert.ToInt32(itemRow["IdSalesResponsibleAssemblyBU"].ToString());
                            }

                            orderGrid.ConnectPlantId = orderParams.connectSiteDetailParams.idSite;
                            orderGrid.Group = itemRow["CustomerGroup"].ToString();
                            orderGrid.Country = itemRow["CustomerCountry"].ToString();
                            orderGrid.Region = itemRow["CustomerZone"].ToString();
                            orderGrid.Status = itemRow["OfferStatus"].ToString();
                            orderGrid.HtmlColor = itemRow["OfferStatusColor"].ToString();

                            if (itemRow["OfferAmount"] != DBNull.Value)
                                orderGrid.Amount = Convert.ToDouble(itemRow["OfferAmount"].ToString());
                            else
                                orderGrid.Amount = 0;

                            //  offer.InvoiceAmount = (from record in offerInvoices where record.IdOffer == offer.IdOffer select record.Value).SingleOrDefault();
                            orderGrid.Currency = itemRow["OfferCurrency"].ToString();

                            if (itemRow["PoReceptionDate"] != DBNull.Value)
                                orderGrid.PoReceptionDate = Convert.ToDateTime(itemRow["PoReceptionDate"].ToString()).ToString("yyyy-MM-dd"); ;

                            if (itemRow["DeliveryDate"] != DBNull.Value)
                                orderGrid.OfferCloseDate = Convert.ToDateTime(itemRow["DeliveryDate"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.OfferCloseDate = null;

                            if (itemRow["RFQReception"] != DBNull.Value)
                                orderGrid.RfqReceptionDate = Convert.ToDateTime(itemRow["RFQReception"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.RfqReceptionDate = null;

                            if (itemRow["SendIn"] != DBNull.Value)
                                orderGrid.QuoteSentDate = Convert.ToDateTime(itemRow["SendIn"].ToString()).ToString("yyyy-MM-dd");
                            else
                                orderGrid.QuoteSentDate = null;

                            if (itemRow["IdBusinessUnit"] != DBNull.Value)
                                orderGrid.IdBusinessUnit = Convert.ToInt32(itemRow["IdBusinessUnit"].ToString());

                            if (itemRow["IdCarProject"] != DBNull.Value)
                                orderGrid.IdCarProject = Convert.ToInt64(itemRow["IdCarProject"].ToString());

                            if (itemRow["IdCarOEM"] != DBNull.Value)
                                orderGrid.IdCarOEM = Convert.ToInt32(itemRow["IdCarOEM"].ToString());

                            if (itemRow["IdSource"] != DBNull.Value)
                                orderGrid.IdSource = Convert.ToInt32(itemRow["IdSource"].ToString());


                            if (itemRow["IdProductSubCategory"] != DBNull.Value)
                            {
                                orderGrid.IdProductCategory = Convert.ToInt64(itemRow["IdProductSubCategory"].ToString());
                                orderGrid.ProductCategory = new ProductCategoryGrid();
                            }

                            if (itemRow["OfferedBy"] != DBNull.Value)
                            {
                                orderGrid.OfferedBy = Convert.ToInt32(itemRow["OfferedBy"].ToString());
                            }

                            orderGrid.OptionsByOffers = new List<OptionsByOfferGrid>();

                            orderGridRows.Add(orderGrid);
                        }
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {
                            OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();

                            OptionsByOfferGrid optionsByOfferGrid = new OptionsByOfferGrid();
                            optionsByOfferGrid.IdOffer = Convert.ToInt64(itemRow["IdOffer"].ToString());
                            optionsByOfferGrid.IdOption = Convert.ToInt64(itemRow["IdOption"].ToString());
                            optionsByOfferGrid.IdOfferPlant = Convert.ToInt32(orderParams.connectSiteDetailParams.idSite);
                            if (itemRow["Quantity"] != DBNull.Value)
                            {
                                optionsByOfferGrid.Quantity = Convert.ToInt32(itemRow["Quantity"].ToString());
                                if (optionsByOfferGrid.Quantity > 0)
                                    optionsByOfferGrid.IsSelected = true;
                                else
                                    optionsByOfferGrid.IsSelected = false;
                            }
                            else
                            {
                                optionsByOfferGrid.IsSelected = false;
                            }
                            optionsByOfferGrid.OfferOption = itemRow["offeroption"].ToString();

                            orderGridRow.OptionsByOffers.Add(optionsByOfferGrid);
                        }
                    }


                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {
                            if (itemRow["IdOffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();

                                if (itemRow["InvoiceAmount"] != DBNull.Value)
                                    orderGridRow.InvoiceAmount = Convert.ToDouble(itemRow["InvoiceAmount"].ToString());
                            }

                        }
                    }


                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["IdProductSubCategory"] != DBNull.Value)
                            {
                                orderGridRows.Where(i => i.IdProductCategory == Convert.ToInt64(itemRow["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.ProductCategory = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(itemRow["IdProductSubCategory"].ToString()), Name = itemRow["ProductSubCategory"].ToString() }; });

                                if (itemRow["IdCategory"] != DBNull.Value)
                                {
                                    orderGridRows.Where(i => i.IdProductCategory == Convert.ToInt64(itemRow["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.ProductCategory.IdParent = Convert.ToInt64(itemRow["IdCategory"].ToString()); u.ProductCategory.Category = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(itemRow["IdCategory"].ToString()), Name = itemRow["Category"].ToString() }; });
                                }
                            }

                        }
                    }

                    if (itemRow.NextResult())
                    {
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["idoffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(itemRow["IdOffer"].ToString())).FirstOrDefault();


                                if (orderGridRow != null)
                                {
                                    if (string.IsNullOrEmpty(orderGridRow.CustomerPOCodes))
                                    {
                                        orderGridRow.CustomerPOCodes = string.Format(itemRow["Code"].ToString());
                                    }
                                    else
                                    {
                                        orderGridRow.CustomerPOCodes = string.Format(orderGridRow.CustomerPOCodes + "\n" + itemRow["Code"].ToString());
                                    }
                                }
                            }

                        }
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["IdPerson"] != DBNull.Value)
                            {
                                orderGridRows.Where(i => i.OfferedBy == Convert.ToInt32(itemRow["IdPerson"].ToString())).ToList().ForEach(u => { u.OfferOwner = itemRow["OfferOwner"].ToString(); });

                            }
                        }
                    }

                    if (itemRow.NextResult())
                    {
                        while (itemRow.Read())
                        {

                            if (itemRow["idoffer"] != DBNull.Value)
                            {
                                orderGridRows.Where(i => i.IdOffer == Convert.ToInt64(itemRow["idoffer"].ToString())).ToList().ForEach(u => { u.OfferedTo = itemRow["OfferedTo"].ToString(); });

                            }
                        }
                    }

                }
            }

            string result = String.Join(",", ((List<OrderGrid>)orderGridRows).ConvertAll<string>(d => d.IdOffer.ToString()).ToArray());

            try
            {
                using (MySqlConnection conn = new MySqlConnection(orderParams.connectSiteDetailParams.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand Command = new MySqlCommand("offers_GetShipmentDatesOfIdOffers", conn);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("_idoffer", result);

                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["idoffer"] != DBNull.Value)
                            {
                                OrderGrid orderGridRow = orderGridRows.Where(t => t.IdOffer == Convert.ToInt64(reader["IdOffer"].ToString())).FirstOrDefault();

                                if (reader["ShipmentDate"] != DBNull.Value)
                                    orderGridRow.ShipmentDate = Convert.ToDateTime(reader["ShipmentDate"].ToString()).ToString("yyyy-MM-dd"); ;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return orderGridRows;
        }

        public List<GeosProviderDetail> GetGeosProviderDetail(string connectionString)
        {
            List<GeosProviderDetail> geosProviderDetails = new List<GeosProviderDetail>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand Command = new MySqlCommand("GetGeosProviderDetails", conn);
                    Command.CommandType = CommandType.StoredProcedure;
                
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GeosProviderDetail geosProviderDetail = new GeosProviderDetail();
                            if (reader["IdCompany"] != DBNull.Value)
                                geosProviderDetail.IdCompany = Convert.ToInt32(reader["IdCompany"]);

                            if (reader["IdGeosProvider"] != DBNull.Value)
                                geosProviderDetail.IdGeosProvider = Convert.ToInt32(reader["IdGeosProvider"]);

                            if (reader["ServiceServerPublicPort"] != DBNull.Value)
                                geosProviderDetail.ServiceServerPublicPort = Convert.ToString(reader["ServiceServerPublicPort"]);

                            if (reader["ServiceProviderUrl"] != DBNull.Value)
                                geosProviderDetail.ServiceProviderUrl = Convert.ToString(reader["ServiceProviderUrl"]);

                            if (reader["Alias"] != DBNull.Value)
                                geosProviderDetail.Alias = Convert.ToString(reader["Alias"]);

                            geosProviderDetails.Add(geosProviderDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return geosProviderDetails;
        }

        public Tuple<string,byte[]> GetOfferEngAnalysisAttachments(string workingOrdersPath, OfferParams offerParams)
        {
          
            string fileName = null;
            byte[] bytes = null;
            string path = workingOrdersPath + @"\" + offerParams.offerYear + @"\" + offerParams.offerCode + @"\01 Project Specifications\";
            if (Directory.Exists(path))
            {
                if (Directory.GetFiles(path).Count() > 0)
                {
                   
                    fileName = GUIDCode.GUIDCodeString();
                    string startPath = path;
                    string zipPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + @"\Temp_" + fileName + ".zip";

                    ZipFile.CreateFromDirectory(startPath, zipPath);
                  
                    if (File.Exists(zipPath))
                    {
                        using (System.IO.FileStream stream = new System.IO.FileStream(zipPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            bytes = new byte[stream.Length];
                            int numBytesToRead = (int)stream.Length;
                            int numBytesRead = 0;
                            while (numBytesToRead > 0)
                            {
                                // Read may return anything from 0 to numBytesToRead.
                                int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                                // Break when the end of the file is reached.
                                if (n == 0)
                                    break;

                                numBytesRead += n;
                                numBytesToRead -= n;
                            }
                        }
                      

                    }
                    DeleteTempFile(zipPath);
                }
            }
            Tuple<string, byte[]> fileDetail = null;

            if (bytes!=null)
            {
                fileDetail = Tuple.Create(fileName, bytes);
            }
            
            
             return fileDetail;
        }

              

        /// <summary>
        /// Method for delete TempFolder folders.
        /// </summary>
        private void DeleteTempFile(string filePath)
        {
           string tempPath = filePath ;
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }
}
