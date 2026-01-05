using ChinhDo.Transactions;
using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;

using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SAM;
using MySql.Data.MySqlClient;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class SAMManager
    {
        public SAMManager()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "log4net.config";
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                }
            }
            catch
            {
                throw;
            }
        }


        public List<Ots> GetPendingWorkorders(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders", connOts);
                otsCommand.CommandType = CommandType.StoredProcedure;
                otsCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);
                otsCommand.CommandTimeout = 8000;

                using (MySqlDataReader otReader = otsCommand.ExecuteReader())
                {
                    if (otReader.HasRows)
                    {
                        while (otReader.Read())
                        {
                            try
                            {
                                Ots ot = new Ots();
                                ot.IdOT = Convert.ToInt64(otReader["IdOT"]);
                                ot.NumOT = Convert.ToByte(otReader["NumOT"]);

                                if (otReader["OtCode"] != DBNull.Value)
                                {
                                    ot.Code = Convert.ToString(otReader["OtCode"]);
                                    ot.MergeCode = Convert.ToString(otReader["MergeCode"]);
                                }

                                if (otReader["DeliveryDate"] != DBNull.Value)
                                {
                                    ot.DeliveryDate = Convert.ToDateTime(otReader["DeliveryDate"]);
                                    ot.Delay = (int)(ot.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;
                                }

                                ot.Quotation = new Quotation();

                                if (otReader["IdQuotation"] != DBNull.Value)
                                {
                                    ot.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                    ot.Quotation.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                    ot.Quotation.Code = Convert.ToString(otReader["QuotationCode"]);
                                }

                                if (otReader["IdTemplate"] != DBNull.Value)
                                {
                                    ot.Quotation.IdDetectionsTemplate = Convert.ToByte(otReader["IdTemplate"]);

                                    ot.Quotation.Template = new Template();
                                    ot.Quotation.Template.IdTemplate = ot.Quotation.IdDetectionsTemplate;
                                    ot.Quotation.Template.Name = Convert.ToString(otReader["Template"]);
                                    ot.Quotation.Template.IdTemplate = Convert.ToByte(otReader["TemplateType"]);
                                }

                                if (otReader["IdOffer"] != DBNull.Value)
                                {
                                    ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                    ot.Quotation.Offer = new Offer();
                                    ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                    if (otReader["IsCritical"] != DBNull.Value)
                                        ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                    if (otReader["IdStatus"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdStatus = Convert.ToInt64(otReader["IdStatus"]);
                                        ot.Quotation.Offer.GeosStatus = new GeosStatus();
                                        ot.Quotation.Offer.GeosStatus.IdOfferStatusType = Convert.ToInt64(otReader["IdStatus"]);
                                        ot.Quotation.Offer.GeosStatus.Name = Convert.ToString(otReader["OfferStatus"]);
                                        ot.Quotation.Offer.GeosStatus.HtmlColor = Convert.ToString(otReader["OfferStatusHtmlColor"]);

                                        if (otReader["IdSalesStatusType"] != DBNull.Value)
                                        {
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType = new SalesStatusType();
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.IdSalesStatusType = Convert.ToInt64(otReader["IdSalesStatusType"]);
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.Name = Convert.ToString(otReader["SalesStatusType"]);
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.HtmlColor = Convert.ToString(otReader["SalesStatusHtmlColor"]);
                                            if (otReader["SalesStatusIdImage"] != DBNull.Value)
                                                ot.Quotation.Offer.GeosStatus.SalesStatusType.IdImage = Convert.ToInt64(otReader["SalesStatusIdImage"]);
                                        }
                                    }

                                    ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                    ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                    if (otReader["IdOfferType"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);

                                        ot.Quotation.Offer.OfferType = new OfferType();
                                        ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                        ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);

                                        //ot.MergeCode = ot.Quotation.Code + " " + ot.Quotation.Offer.OfferType.Name + " " + ot.NumOT;
                                    }
                                    if (otReader["CarProject"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.CarProject = new CarProject();
                                        ot.Quotation.Offer.CarProject.Name = Convert.ToString(otReader["CarProject"]);
                                    }
                                    if (otReader["CarOEM"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.CarOEM = new CarOEM();
                                        ot.Quotation.Offer.CarOEM.Name = Convert.ToString(otReader["CarOEM"]);
                                    }
                                    if (otReader["BusinessUnit"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.BusinessUnit = new LookupValue();
                                        ot.Quotation.Offer.BusinessUnit.Value = Convert.ToString(otReader["BusinessUnit"]);
                                    }
                                    if (otReader["IdCarriageMethod"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdCarriageMethod = Convert.ToByte(otReader["IdCarriageMethod"]);
                                        ot.Quotation.Offer.CarriageMethod = new LookupValue();
                                        ot.Quotation.Offer.CarriageMethod.IdLookupValue = Convert.ToByte(ot.Quotation.Offer.IdCarriageMethod);
                                        ot.Quotation.Offer.CarriageMethod.Value = Convert.ToString(otReader["CarriageMethod"]);

                                        if (otReader["CarriageImage"] != DBNull.Value)
                                            ot.Quotation.Offer.CarriageMethod.IdImage = Convert.ToInt64(otReader["CarriageImage"]);
                                    }
                                }

                                //if (otReader["IdWarehouse"] != DBNull.Value)
                                //{
                                //    ot.Quotation.IdWarehouse = Convert.ToInt64(otReader["IdWarehouse"]);
                                //}

                                if (otReader["IdSite"] != DBNull.Value)
                                {
                                    ot.Quotation.IdCustomer = Convert.ToInt32(otReader["IdSite"]);
                                    ot.Quotation.Site = new Company();
                                    ot.Quotation.Site.IdCompany = ot.Quotation.IdCustomer;
                                    ot.Quotation.Site.SiteNameWithoutCountry = Convert.ToString(otReader["SiteName"]);

                                    if (otReader["ShortName"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(otReader["ShortName"])))
                                    {
                                        ot.Quotation.Site.ShortName = Convert.ToString(otReader["ShortName"]);
                                        ot.Quotation.Site.Name = ot.Quotation.Site.ShortName;
                                    }
                                    else
                                    {
                                        ot.Quotation.Site.ShortName = null;
                                        ot.Quotation.Site.Name = Convert.ToString(otReader["SiteName"]);
                                    }

                                    ot.Quotation.Site.Customer = new Customer();
                                    ot.Quotation.Site.Customer.IdCustomer = Convert.ToInt32(otReader["IdCustomer"]);
                                    ot.Quotation.Site.Customer.CustomerName = Convert.ToString(otReader["CustomerName"]);

                                    if (otReader["IdCountry"] != DBNull.Value)
                                    {
                                        ot.Quotation.Site.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                        ot.Quotation.Site.Country = new Country();
                                        ot.Quotation.Site.Country.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                        ot.Quotation.Site.Country.Name = Convert.ToString(otReader["CountryName"]);
                                    }
                                }



                                if (otReader["PODate"] != DBNull.Value)
                                    ot.PoDate = Convert.ToDateTime(otReader["PODate"]);

                                if (otReader["ProducerCountryGroupName"] != DBNull.Value)
                                {
                                    ot.CountryGroup = new CountryGroup();
                                    ot.CountryGroup.Name = otReader["ProducerCountryGroupName"].ToString();
                                    if (otReader["ProducerCountryGroupHTMLColor"] != DBNull.Value)
                                        ot.CountryGroup.HtmlColor = otReader["ProducerCountryGroupHTMLColor"].ToString();
                                }

                                if (otReader["ExpectedStartDate"] != DBNull.Value)
                                    ot.ExpectedStartDate = Convert.ToDateTime(otReader["ExpectedStartDate"]);

                                if (otReader["ExpectedEndDate"] != DBNull.Value)
                                    ot.ExpectedEndDate = Convert.ToDateTime(otReader["ExpectedEndDate"]);

                                if (otReader["Progress"] != DBNull.Value)
                                    ot.Progress = Convert.ToByte(otReader["Progress"]);

                                if (otReader["Operators"] != DBNull.Value)
                                {
                                    ot.Operators = Convert.ToString(otReader["Operators"]);
                                    ot.UserShortDetails = new List<UserShortDetail>();

                                    foreach (string itemIdUser in ot.Operators.Split(','))
                                    {
                                        if (!ot.UserShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                        {
                                            UserShortDetail usd = new UserShortDetail();
                                            usd.IdUser = Convert.ToInt32(itemIdUser);

                                            ot.UserShortDetails.Add(usd);
                                            if (!userShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                            {
                                                userShortDetails.Add(usd);
                                            }

                                            // usd.UserImageInBytes = GetUserProfileImageInBytes(itemLogin, userProfileImageFilePath);

                                        }

                                    }


                                }

                                // ot.Modules = 0;

                                //ot.RemainingQuantity = ot.ActualQuantity + ot.DownloadedQuantity;
                                //if (ot.ActualQuantity > 0)
                                //{
                                //    ot.Status = (Int16)((Math.Abs(ot.DownloadedQuantity) / ot.ActualQuantity) * 100);
                                //}
                                //ot.OtItems = new List<OtItem>();
                                ots.Add(ot);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkOrders() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                        if (otReader.NextResult())
                        {
                            while (otReader.Read())
                            {
                                try
                                {
                                    if (otReader["IdOffer"] != DBNull.Value)
                                    {
                                        if (ots.Any(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])))
                                        {

                                            ots.Where(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])).ToList().ForEach(x => x.Modules = Convert.ToInt64(otReader["Modules"]));
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkOrders() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                        }
                    }


                }
            }
            // ots = ots.Where(x => x.Status < 100).ToList();
            string getIdUsers = string.Join(",", userShortDetails.Select(i => i.IdUser).ToList());
            userShortDetails = GetUserShortDetail(getIdUsers, connectionString, userProfileImageFilePath);
            foreach (Ots itemots in ots)
            {
                if (itemots.Operators != null)
                {
                    List<string> names = new List<string>();

                    foreach (string itemidUser in itemots.Operators.Split(','))
                    {
                        names.Add(userShortDetails.Where(i => i.IdUser.ToString() == itemidUser).Select(i => i.UserName).FirstOrDefault());
                    }

                    itemots.OperatorNames = string.Join(",", names.ToList());
                }

                if (itemots.UserShortDetails != null)
                {
                    foreach (UserShortDetail itemOPUSD in itemots.UserShortDetails)
                    {
                        if (userShortDetails.Any(usd => usd.IdUser == itemOPUSD.IdUser))
                        {
                            UserShortDetail usdItem = userShortDetails.Where(usd => usd.IdUser == itemOPUSD.IdUser).FirstOrDefault();
                            itemOPUSD.IdUserGender = usdItem.IdUserGender;
                            itemOPUSD.UserName = usdItem.UserName;
                            itemOPUSD.UserImageInBytes = usdItem.UserImageInBytes;
                        }
                    }
                }

            }

            Log4NetLogger.Logger.Log("Executed GetPendingWorkOrders().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        private List<UserShortDetail> GetUserShortDetail(string getIdUsers, string connectionString, string userProfileImageFilePath)
        {
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_GetUserShortDetail", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdUsers", getIdUsers);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserShortDetail userShortDetail = new UserShortDetail();

                        if (reader["IdUser"] != DBNull.Value)
                            userShortDetail.IdUser = Convert.ToInt32(reader["IdUser"]);

                        if (reader["Login"] != DBNull.Value)
                            userShortDetail.Login = Convert.ToString(reader["Login"]);

                        if (reader["UserName"] != DBNull.Value)
                            userShortDetail.UserName = Convert.ToString(reader["UserName"]);

                        if (reader["IdUserGender"] != DBNull.Value)
                            userShortDetail.IdUserGender = Convert.ToByte(reader["IdUserGender"]);


                        userShortDetails.Add(userShortDetail);
                    }
                }
            }

            foreach (UserShortDetail userShortDetail in userShortDetails)
            {
                userShortDetail.UserImageInBytes = GetUserProfileImageInBytes(userShortDetail.Login, userProfileImageFilePath);
            }

            return userShortDetails;
        }

        private byte[] GetUserProfileImageInBytes(string LoginName, string filePath)
        {
            byte[] bytes = null;
            string fileUploadPath = "";

            if (File.Exists(filePath + LoginName + ".png"))
            {
                fileUploadPath = filePath + LoginName + ".png";
            }
            else
            {
                fileUploadPath = filePath + LoginName + ".jpg";
            }
            if (File.Exists(fileUploadPath))
            {
                using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

                return bytes;
            }
            else
            {
                return null;
            }

        }


        public List<OTWorkingTime> GetOTWorkingTimeByIdOT(Company company, Int64 idOT, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTWorkingTime> otWorkingTimes = new List<OTWorkingTime>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetOTWorkingTimeByIdOT", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idOT", idOT);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTWorkingTime otWorkingTime = new OTWorkingTime();
                            otWorkingTime.Company = company;
                            if (reader["IdOT"] != DBNull.Value)
                                otWorkingTime.IdOT = Convert.ToInt64(reader["IdOT"]);

                            if (reader["IdStage"] != DBNull.Value)
                            {
                                otWorkingTime.IdStage = Convert.ToByte(reader["IdStage"]);
                                otWorkingTime.Stage = new Stage();
                                otWorkingTime.Stage.IdStage = Convert.ToByte(reader["IdStage"]);
                                otWorkingTime.Stage.Name = Convert.ToString(reader["StageName"]);
                            }

                            if (reader["TotalWorkedHours"] != DBNull.Value)
                            {
                                otWorkingTime.TotalTime = (TimeSpan)reader["TotalWorkedHours"];

                                otWorkingTime.TotalTimeInString = string.Format("{0:00}:{1:00}", (Int32)otWorkingTime.TotalTime.TotalHours, otWorkingTime.TotalTime.Minutes);
                            }
                            else
                            {
                                otWorkingTime.TotalTime = new TimeSpan(0, 0, 0);
                                otWorkingTime.TotalTimeInString = string.Format("{0:00}:{1:00}", 0, 0);
                            }

                            if (reader["IdOperator"] != DBNull.Value)
                            {
                                otWorkingTime.IdOperator = Convert.ToInt32(reader["IdOperator"]);

                                otWorkingTime.UserShortDetail = new UserShortDetail();
                                otWorkingTime.UserShortDetail.IdUser = Convert.ToInt32(reader["IdOperator"]);
                                if (reader["IdUserGender"] != DBNull.Value)
                                    otWorkingTime.UserShortDetail.IdUserGender = Convert.ToByte(reader["IdUserGender"]);
                                otWorkingTime.UserShortDetail.Login = Convert.ToString(reader["Login"]);
                                otWorkingTime.UserShortDetail.UserName = Convert.ToString(reader["UserName"]);

                                if (reader["CompanyCode"] != DBNull.Value)
                                {
                                    otWorkingTime.UserShortDetail.CompanyCode = Convert.ToString(reader["CompanyCode"]);
                                }

                                if (!userShortDetails.Any(usd => usd.IdUser == otWorkingTime.IdOperator))
                                {
                                    userShortDetails.Add(otWorkingTime.UserShortDetail);
                                }
                            }

                            otWorkingTimes.Add(otWorkingTime);
                        }

                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTWorkingTimeByIdOT( IdOT-{0}). ErrorMessage- {1}", idOT, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            foreach (UserShortDetail itemUSD in userShortDetails)
            {
                byte[] UserImageInBytes = GetUserProfileImageInBytes(itemUSD.Login, userProfileImageFilePath);
                otWorkingTimes.Where(owt => owt.IdOperator == itemUSD.IdUser).ToList().ForEach(x => x.UserShortDetail.UserImageInBytes = UserImageInBytes);
            }

            return otWorkingTimes;
        }


        public OTWorkingTime AddOTWorkingTime(Company company, OTWorkingTime otWorkingTime)
        {
            string connectionString = company.ConnectPlantConstr;
            //bool isAdded = false;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_InsertOTWorkingTime", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOT", otWorkingTime.IdOT);
                        mySqlCommand.Parameters.AddWithValue("_IdOperator", otWorkingTime.IdOperator);
                        mySqlCommand.Parameters.AddWithValue("_IdStage", otWorkingTime.IdStage);
                        mySqlCommand.Parameters.AddWithValue("_StartDate", DateTime.Now);

                        otWorkingTime.IdOTWorkingTime = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        //isAdded = true;
                        mySqlConnection.Close();
                    }


                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();

                if (transactionScope != null)
                    transactionScope.Dispose();

                throw;
            }

            return otWorkingTime;
        }


        public bool UpdateOTWorkingTime(Company company, OTWorkingTime otWorkingTime)
        {
            string connectionString = company.ConnectPlantConstr;
            bool isUpdated = false;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_UpdateOTWorkingTime", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOTWorkingTime", otWorkingTime.IdOTWorkingTime);
                        mySqlCommand.Parameters.AddWithValue("_EndDate", DateTime.Now);

                        mySqlCommand.ExecuteNonQuery();
                        isUpdated = true;
                        mySqlConnection.Close();
                    }


                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();

                if (transactionScope != null)
                    transactionScope.Dispose();
                isUpdated = false;
                throw;
            }

            return isUpdated;
        }


        public List<Ots> GetPendingWorkorders_V2038(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders", connOts);
                otsCommand.CommandType = CommandType.StoredProcedure;
                otsCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);
                otsCommand.CommandTimeout = 8000;

                using (MySqlDataReader otReader = otsCommand.ExecuteReader())
                {
                    if (otReader.HasRows)
                    {
                        while (otReader.Read())
                        {
                            try
                            {
                                Ots ot = new Ots();
                                ot.IdOT = Convert.ToInt64(otReader["IdOT"]);
                                ot.NumOT = Convert.ToByte(otReader["NumOT"]);
                                ot.Site = company;
                                if (otReader["OtCode"] != DBNull.Value)
                                {
                                    ot.Code = Convert.ToString(otReader["OtCode"]);
                                    ot.MergeCode = Convert.ToString(otReader["MergeCode"]);
                                }

                                if (otReader["DeliveryDate"] != DBNull.Value)
                                {
                                    ot.DeliveryDate = Convert.ToDateTime(otReader["DeliveryDate"]);
                                    ot.Delay = (int)(ot.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;
                                }

                                ot.Quotation = new Quotation();

                                if (otReader["IdQuotation"] != DBNull.Value)
                                {
                                    ot.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                    ot.Quotation.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                    ot.Quotation.Code = Convert.ToString(otReader["QuotationCode"]);
                                }

                                if (otReader["IdTemplate"] != DBNull.Value)
                                {
                                    ot.Quotation.IdDetectionsTemplate = Convert.ToByte(otReader["IdTemplate"]);

                                    ot.Quotation.Template = new Template();
                                    ot.Quotation.Template.IdTemplate = ot.Quotation.IdDetectionsTemplate;
                                    ot.Quotation.Template.Name = Convert.ToString(otReader["Template"]);
                                    ot.Quotation.Template.IdTemplate = Convert.ToByte(otReader["TemplateType"]);
                                }

                                if (otReader["IdOffer"] != DBNull.Value)
                                {
                                    ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                    ot.Quotation.Offer = new Offer();
                                    ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                    if (otReader["IsCritical"] != DBNull.Value)
                                        ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                    if (otReader["IdStatus"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdStatus = Convert.ToInt64(otReader["IdStatus"]);
                                        ot.Quotation.Offer.GeosStatus = new GeosStatus();
                                        ot.Quotation.Offer.GeosStatus.IdOfferStatusType = Convert.ToInt64(otReader["IdStatus"]);
                                        ot.Quotation.Offer.GeosStatus.Name = Convert.ToString(otReader["OfferStatus"]);
                                        ot.Quotation.Offer.GeosStatus.HtmlColor = Convert.ToString(otReader["OfferStatusHtmlColor"]);

                                        if (otReader["IdSalesStatusType"] != DBNull.Value)
                                        {
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType = new SalesStatusType();
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.IdSalesStatusType = Convert.ToInt64(otReader["IdSalesStatusType"]);
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.Name = Convert.ToString(otReader["SalesStatusType"]);
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.HtmlColor = Convert.ToString(otReader["SalesStatusHtmlColor"]);
                                            if (otReader["SalesStatusIdImage"] != DBNull.Value)
                                                ot.Quotation.Offer.GeosStatus.SalesStatusType.IdImage = Convert.ToInt64(otReader["SalesStatusIdImage"]);
                                        }
                                    }

                                    ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                    ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                    if (otReader["IdOfferType"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);

                                        ot.Quotation.Offer.OfferType = new OfferType();
                                        ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                        ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);

                                        //ot.MergeCode = ot.Quotation.Code + " " + ot.Quotation.Offer.OfferType.Name + " " + ot.NumOT;
                                    }
                                    if (otReader["CarProject"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.CarProject = new CarProject();
                                        ot.Quotation.Offer.CarProject.Name = Convert.ToString(otReader["CarProject"]);
                                    }
                                    if (otReader["CarOEM"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.CarOEM = new CarOEM();
                                        ot.Quotation.Offer.CarOEM.Name = Convert.ToString(otReader["CarOEM"]);
                                    }
                                    if (otReader["BusinessUnit"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.BusinessUnit = new LookupValue();
                                        ot.Quotation.Offer.BusinessUnit.Value = Convert.ToString(otReader["BusinessUnit"]);
                                    }
                                    if (otReader["IdCarriageMethod"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdCarriageMethod = Convert.ToByte(otReader["IdCarriageMethod"]);
                                        ot.Quotation.Offer.CarriageMethod = new LookupValue();
                                        ot.Quotation.Offer.CarriageMethod.IdLookupValue = Convert.ToByte(ot.Quotation.Offer.IdCarriageMethod);
                                        ot.Quotation.Offer.CarriageMethod.Value = Convert.ToString(otReader["CarriageMethod"]);

                                        if (otReader["CarriageImage"] != DBNull.Value)
                                            ot.Quotation.Offer.CarriageMethod.IdImage = Convert.ToInt64(otReader["CarriageImage"]);
                                    }
                                }

                                //if (otReader["IdWarehouse"] != DBNull.Value)
                                //{
                                //    ot.Quotation.IdWarehouse = Convert.ToInt64(otReader["IdWarehouse"]);
                                //}

                                if (otReader["IdSite"] != DBNull.Value)
                                {
                                    ot.Quotation.IdCustomer = Convert.ToInt32(otReader["IdSite"]);
                                    ot.Quotation.Site = new Company();
                                    ot.Quotation.Site.IdCompany = ot.Quotation.IdCustomer;
                                    ot.Quotation.Site.SiteNameWithoutCountry = Convert.ToString(otReader["SiteName"]);

                                    if (otReader["ShortName"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(otReader["ShortName"])))
                                    {
                                        ot.Quotation.Site.ShortName = Convert.ToString(otReader["ShortName"]);
                                        ot.Quotation.Site.Name = ot.Quotation.Site.ShortName;
                                    }
                                    else
                                    {
                                        ot.Quotation.Site.ShortName = null;
                                        ot.Quotation.Site.Name = Convert.ToString(otReader["SiteName"]);
                                    }

                                    ot.Quotation.Site.Customer = new Customer();
                                    ot.Quotation.Site.Customer.IdCustomer = Convert.ToInt32(otReader["IdCustomer"]);
                                    ot.Quotation.Site.Customer.CustomerName = Convert.ToString(otReader["CustomerName"]);

                                    if (otReader["IdCountry"] != DBNull.Value)
                                    {
                                        ot.Quotation.Site.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                        ot.Quotation.Site.Country = new Country();
                                        ot.Quotation.Site.Country.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                        ot.Quotation.Site.Country.Name = Convert.ToString(otReader["CountryName"]);
                                    }
                                }



                                if (otReader["PODate"] != DBNull.Value)
                                    ot.PoDate = Convert.ToDateTime(otReader["PODate"]);

                                if (otReader["ProducerCountryGroupName"] != DBNull.Value)
                                {
                                    ot.CountryGroup = new CountryGroup();
                                    ot.CountryGroup.Name = otReader["ProducerCountryGroupName"].ToString();
                                    if (otReader["ProducerCountryGroupHTMLColor"] != DBNull.Value)
                                        ot.CountryGroup.HtmlColor = otReader["ProducerCountryGroupHTMLColor"].ToString();
                                }

                                if (otReader["ExpectedStartDate"] != DBNull.Value)
                                    ot.ExpectedStartDate = Convert.ToDateTime(otReader["ExpectedStartDate"]);

                                if (otReader["ExpectedEndDate"] != DBNull.Value)
                                    ot.ExpectedEndDate = Convert.ToDateTime(otReader["ExpectedEndDate"]);

                                if (otReader["Progress"] != DBNull.Value)
                                    ot.Progress = Convert.ToByte(otReader["Progress"]);

                                if (otReader["Operators"] != DBNull.Value)
                                {
                                    ot.Operators = Convert.ToString(otReader["Operators"]);
                                    ot.UserShortDetails = new List<UserShortDetail>();

                                    foreach (string itemIdUser in ot.Operators.Split(','))
                                    {
                                        if (!ot.UserShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                        {
                                            UserShortDetail usd = new UserShortDetail();
                                            usd.IdUser = Convert.ToInt32(itemIdUser);

                                            ot.UserShortDetails.Add(usd);
                                            if (!userShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                            {
                                                userShortDetails.Add(usd);
                                            }

                                            // usd.UserImageInBytes = GetUserProfileImageInBytes(itemLogin, userProfileImageFilePath);

                                        }

                                    }


                                }

                                // ot.Modules = 0;

                                //ot.RemainingQuantity = ot.ActualQuantity + ot.DownloadedQuantity;
                                //if (ot.ActualQuantity > 0)
                                //{
                                //    ot.Status = (Int16)((Math.Abs(ot.DownloadedQuantity) / ot.ActualQuantity) * 100);
                                //}
                                //ot.OtItems = new List<OtItem>();
                                ots.Add(ot);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkOrders() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                        if (otReader.NextResult())
                        {
                            while (otReader.Read())
                            {
                                try
                                {
                                    if (otReader["IdOffer"] != DBNull.Value)
                                    {
                                        if (ots.Any(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])))
                                        {

                                            ots.Where(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])).ToList().ForEach(x => x.Modules = Convert.ToInt64(otReader["Modules"]));
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkOrders() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                        }
                    }


                }
            }
            // ots = ots.Where(x => x.Status < 100).ToList();
            string getIdUsers = string.Join(",", userShortDetails.Select(i => i.IdUser).ToList());
            userShortDetails = GetUserShortDetail(getIdUsers, connectionString, userProfileImageFilePath);
            foreach (Ots itemots in ots)
            {
                if (itemots.Operators != null)
                {
                    List<string> names = new List<string>();

                    foreach (string itemidUser in itemots.Operators.Split(','))
                    {
                        names.Add(userShortDetails.Where(i => i.IdUser.ToString() == itemidUser).Select(i => i.UserName).FirstOrDefault());
                    }

                    itemots.OperatorNames = string.Join(",", names.ToList());
                }

                if (itemots.UserShortDetails != null)
                {
                    foreach (UserShortDetail itemOPUSD in itemots.UserShortDetails)
                    {
                        if (userShortDetails.Any(usd => usd.IdUser == itemOPUSD.IdUser))
                        {
                            UserShortDetail usdItem = userShortDetails.Where(usd => usd.IdUser == itemOPUSD.IdUser).FirstOrDefault();
                            itemOPUSD.IdUserGender = usdItem.IdUserGender;
                            itemOPUSD.UserName = usdItem.UserName;
                            itemOPUSD.UserImageInBytes = usdItem.UserImageInBytes;
                        }
                    }
                }

            }

            Log4NetLogger.Logger.Log("Executed GetPendingWorkOrders().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        public List<OTWorkingTime> GetOTWorkingTimeDetails(Int64 idOT, Company company, String userProfileImageFilePath)
        {
            List<OTWorkingTime> LstOTWorkingTime = new List<OTWorkingTime>();
            List<UserShortDetail> UserShortDetailForbytes = new List<UserShortDetail>();
            string connectionString = company.ConnectPlantConstr;
            using (MySqlConnection connWarehouse = new MySqlConnection(connectionString))
            {
                connWarehouse.Open();

                MySqlCommand warehouseCommand = new MySqlCommand("SAM_GetOTWorkingTimeDetails", connWarehouse);
                warehouseCommand.CommandType = CommandType.StoredProcedure;
                warehouseCommand.Parameters.AddWithValue("_IdOT", idOT);

                using (MySqlDataReader warehouseReader = warehouseCommand.ExecuteReader())
                {
                    while (warehouseReader.Read())
                    {
                        try
                        {
                            OTWorkingTime otWorkingTime = new OTWorkingTime();

                            if (warehouseReader["IdOTWorkingTime"] != DBNull.Value)
                                otWorkingTime.IdOTWorkingTime = Convert.ToInt64(warehouseReader["IdOTWorkingTime"]);

                            if (warehouseReader["IdOT"] != DBNull.Value)
                                otWorkingTime.IdOT = Convert.ToInt64(warehouseReader["IdOT"]);

                            if (warehouseReader["IdStage"] != DBNull.Value)
                                otWorkingTime.IdStage = Convert.ToByte(warehouseReader["IdStage"]);

                            if (warehouseReader["StartDate"] != DBNull.Value)
                                otWorkingTime.StartTime = Convert.ToDateTime(Convert.ToDateTime(warehouseReader["StartDate"]).ToString("dd/MM/yyyy HH:mm"));

                            //if (warehouseReader["StartTime"] != DBNull.Value)
                            //    otWorkingTime.StartTimeInHoursAndMinutes = warehouseReader["StartTime"].ToString();

                            if (warehouseReader["EndDate"] != DBNull.Value)
                                otWorkingTime.EndTime = Convert.ToDateTime(Convert.ToDateTime(warehouseReader["EndDate"]).ToString("dd/MM/yyyy HH:mm"));

                            if (warehouseReader["TotalTime"] != DBNull.Value)
                            {
                                otWorkingTime.TotalTime = (TimeSpan)warehouseReader["TotalTime"];
                                otWorkingTime.TotalTime = new TimeSpan(otWorkingTime.TotalTime.Days, otWorkingTime.TotalTime.Hours, otWorkingTime.TotalTime.Minutes, 00);

                                if (otWorkingTime.TotalTime.Days > 0)
                                {
                                    int Hours = otWorkingTime.TotalTime.Days * 24 + otWorkingTime.TotalTime.Hours;
                                    otWorkingTime.Hours = string.Format("{0}H {1}M", Hours, otWorkingTime.TotalTime.Minutes);
                                }
                                else
                                {
                                    otWorkingTime.Hours = string.Format("{0}H {1}M", otWorkingTime.TotalTime.Hours, otWorkingTime.TotalTime.Minutes);
                                }
                            }

                            //if (warehouseReader["EndTime"] != DBNull.Value)
                            //    otWorkingTime.EndTimeInHoursAndMinutes =warehouseReader["EndTime"].ToString();

                            if (warehouseReader["IdOperator"] != DBNull.Value)
                            {
                                otWorkingTime.IdOperator = Convert.ToInt32(warehouseReader["IdOperator"]);

                                otWorkingTime.UserShortDetail = new UserShortDetail();
                                otWorkingTime.UserShortDetail.IdUser = Convert.ToInt32(warehouseReader["IdOperator"]);
                                otWorkingTime.UserShortDetail.Login = Convert.ToString(warehouseReader["Login"]);
                                otWorkingTime.UserShortDetail.UserName = Convert.ToString(warehouseReader["UserName"]);

                                if (warehouseReader["IdUserGender"] != DBNull.Value)
                                    otWorkingTime.UserShortDetail.IdUserGender = Convert.ToByte(warehouseReader["IdUserGender"]);

                                if (UserShortDetailForbytes.Any(usd => usd.Login == otWorkingTime.UserShortDetail.Login))
                                {
                                    otWorkingTime.UserShortDetail.UserImageInBytes = UserShortDetailForbytes.Where(usd => usd.Login == otWorkingTime.UserShortDetail.Login).FirstOrDefault().UserImageInBytes;
                                }
                                else
                                {
                                    otWorkingTime.UserShortDetail.UserImageInBytes = GetUserProfileImageInBytes(otWorkingTime.UserShortDetail.Login, userProfileImageFilePath);
                                    UserShortDetail userShortDetail = new UserShortDetail();
                                    userShortDetail.IdUser = otWorkingTime.UserShortDetail.IdUser;
                                    userShortDetail.IdUserGender = otWorkingTime.UserShortDetail.IdUserGender;
                                    userShortDetail.Login = otWorkingTime.UserShortDetail.Login;
                                    userShortDetail.UserName = otWorkingTime.UserShortDetail.UserName;

                                    userShortDetail.UserImageInBytes = otWorkingTime.UserShortDetail.UserImageInBytes;
                                    UserShortDetailForbytes.Add(userShortDetail);
                                }
                            }


                            LstOTWorkingTime.Add(otWorkingTime);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetOTWorkingTimeDetails(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }



            return LstOTWorkingTime;
        }

        /// <summary>
        /// [cpatil][12-12-2019][GEOS2-1760]Open Work Order details when double-click in Order
        /// </summary>
        /// <param name="reference">Get article reference</param>
        /// <param name="articleVisualAidsPath">Get article file path</param>
        /// <param name="company">Get company details</param>
        /// <returns>Article details</returns>
        public Article GetArticleDetails(Int32 idArticle, string articleVisualAidsPath, string connectionString)
        {
            Article article = new Article();

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetArticleDetails", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_idArticle", idArticle);


                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            if (reader["idArticle"] != DBNull.Value)
                                article.IdArticle = Convert.ToInt32(reader["idArticle"]);


                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);

                                if (!string.IsNullOrEmpty(article.ImagePath))
                                    article.ArticleImageInBytes = GetArticleImageInBytes(articleVisualAidsPath, article);
                            }

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = reader["Reference"].ToString();


                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);


                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticleDetails(). IdArticle-{0} ErrorMessage- {1}", article.IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }


            }

            return article;
        }

        /// <summary>
        /// This method is to get articles image in bytes
        /// </summary>
        /// <param name="ArticleVisualAidsPath">Get articles image path</param>
        /// <param name="article">Get articles details</param>
        /// <returns>Article image bytes</returns>
        public byte[] GetArticleImageInBytes(string ArticleVisualAidsPath, Article article)
        {
            if (!Directory.Exists(ArticleVisualAidsPath))
            {
                return null;
            }

            string fileUploadPath = ArticleVisualAidsPath + article.ImagePath;

            if (!File.Exists(fileUploadPath))
            {
                return null;
            }

            if (article != null && !string.IsNullOrEmpty(article.ImagePath))
            {

                byte[] bytes = null;

                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

                    return bytes;
                }

                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error GetArticleImageInBytes() article ImagePath-{0}. ErrorMessage- {1}", article.ImagePath, ex.Message), category: Category.Exception, priority: Priority.Low);
                    //throw;
                }
            }

            return null;
        }

        /// <summary>
        /// This method is to get workorder details
        /// </summary>
        /// [cpatil][18-12-2019][GEOS2-1760]Open Work Order details when double-click in Order
        /// <param name="idOt">Get id ot</param>
        /// <param name="company">Get company details</param>
        /// <returns>Get ots details</returns>
        public Ots GetWorkOrderByIdOt(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("warehouse_GetMaterialOtInformationByIdOt", connOT);
                otCommand.CommandType = CommandType.StoredProcedure;
                otCommand.Parameters.AddWithValue("_IdOt", idOt);
                otCommand.CommandTimeout = 3000;
                using (MySqlDataReader otReader = otCommand.ExecuteReader())
                {
                    if (otReader.Read())
                    {
                        try
                        {
                            ot = new Ots();
                            ot.IdOT = idOt;

                            if (otReader["OtCode"] != DBNull.Value)
                                ot.Code = Convert.ToString(otReader["OtCode"]);

                            ot.NumOT = Convert.ToByte(otReader["NumOT"]);

                            if (otReader["Comments"] != DBNull.Value)
                                ot.Comments = Convert.ToString(otReader["Comments"]);

                            if (otReader["CreatedBy"] != DBNull.Value)
                            {
                                ot.CreatedBy = Convert.ToInt32(otReader["CreatedBy"]);

                                ot.CreatedByPerson = new People();
                                ot.CreatedByPerson.IdPerson = ot.CreatedBy;
                                ot.CreatedByPerson.Name = Convert.ToString(otReader["CreatedByName"]);
                                ot.CreatedByPerson.Surname = Convert.ToString(otReader["CreatedBySurname"]);
                            }

                            ot.ReviewedBy = Convert.ToInt32(otReader["ReviewedBy"]);

                            //CreatedBy
                            if (otReader["CreatedBy"] != DBNull.Value)
                            {
                                ot.CreatedBy = Convert.ToInt32(otReader["CreatedBy"]);

                                ot.CreatedByPerson = new People();
                                ot.CreatedByPerson.IdPerson = ot.CreatedBy;
                                ot.CreatedByPerson.Name = Convert.ToString(otReader["CreatedByName"]);
                                ot.CreatedByPerson.Surname = Convert.ToString(otReader["CreatedBySurname"]);
                            }

                            if (otReader["CreationDate"] != DBNull.Value)
                                ot.CreationDate = Convert.ToDateTime(otReader["CreationDate"]);

                            //ModifiedBy
                            if (otReader["ModifiedBy"] != DBNull.Value)
                            {
                                ot.ModifiedBy = Convert.ToInt32(otReader["ModifiedBy"]);

                                ot.ModifiedByPerson = new People();
                                ot.ModifiedByPerson.IdPerson = ot.ModifiedBy;
                                ot.ModifiedByPerson.Name = Convert.ToString(otReader["ModifiedByName"]);
                                ot.ModifiedByPerson.Surname = Convert.ToString(otReader["ModifiedBySurname"]);
                            }

                            if (otReader["ModifiedIn"] != DBNull.Value)
                                ot.ModifiedIn = Convert.ToDateTime(otReader["ModifiedIn"]);

                            if (otReader["DeliveryDate"] != DBNull.Value)
                                ot.DeliveryDate = Convert.ToDateTime(otReader["DeliveryDate"]);

                            if (otReader["WareHouseLockSession"] != DBNull.Value)
                                ot.WareHouseLockSession = Convert.ToString(otReader["WareHouseLockSession"]);

                            if (otReader["AttachedFiles"] != DBNull.Value)
                                ot.AttachedFiles = Convert.ToString(otReader["AttachedFiles"]);

                            if (otReader["IdShippingAddress"] != DBNull.Value)
                                ot.IdShippingAddress = Convert.ToInt64(otReader["IdShippingAddress"]);

                            ot.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                            ot.Quotation = new Quotation();
                            ot.Quotation.Code = Convert.ToString(otReader["QuotationCode"]);

                            if (otReader["Year"] != DBNull.Value)
                                ot.Quotation.Year = Convert.ToInt32(otReader["Year"]);

                            if (otReader["Description"] != DBNull.Value)
                                ot.Quotation.Description = Convert.ToString(otReader["Description"]);

                            if (otReader["ProjectName"] != DBNull.Value)
                                ot.Quotation.ProjectName = Convert.ToString(otReader["ProjectName"]);


                            if (otReader["ProjectName"] != DBNull.Value)
                                ot.Quotation.ProjectName = Convert.ToString(otReader["ProjectName"]);

                            //Customer
                            if (otReader["IdSite"] != DBNull.Value)
                            {
                                ot.Quotation.Site = new Company();
                                ot.Quotation.Site.IdCompany = Convert.ToInt32(otReader["IdSite"]);
                                ot.Quotation.Site.SiteNameWithoutCountry = Convert.ToString(otReader["SiteName"]);

                                if (otReader["ShortName"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(otReader["ShortName"])))
                                {
                                    ot.Quotation.Site.Name = Convert.ToString(otReader["ShortName"]);
                                }
                                else
                                {
                                    ot.Quotation.Site.Name = Convert.ToString(otReader["Customer"]);
                                }

                                if (otReader["idCountry"] != DBNull.Value)
                                {
                                    ot.Quotation.Site.IdCountry = Convert.ToByte(otReader["idCountry"]);
                                    ot.Quotation.Site.Country = new Country();
                                    ot.Quotation.Site.Country.IdCountry = Convert.ToByte(otReader["idCountry"]);
                                    ot.Quotation.Site.Country.Name = otReader["CountryName"].ToString();
                                    ot.Quotation.Site.Country.Iso = otReader["iso"].ToString();

                                    if (otReader["EuroZone"] != DBNull.Value)
                                    {
                                        ot.Quotation.Site.Country.EuroZone = Convert.ToSByte(otReader["EuroZone"]);
                                    }

                                    if (otReader["IdCountryGroup"] != DBNull.Value)
                                    {
                                        ot.Quotation.Site.CountryGroup = new CountryGroup();
                                        ot.Quotation.Site.CountryGroup.IdCountryGroup = Convert.ToInt64(otReader["IdCountryGroup"]);

                                        if (otReader["CountryGroup"] != DBNull.Value)
                                            ot.Quotation.Site.CountryGroup.Name = Convert.ToString(otReader["CountryGroup"]);

                                        if (otReader["IsFreeTrade"] != DBNull.Value)
                                            ot.Quotation.Site.CountryGroup.IsFreeTrade = Convert.ToByte(otReader["IsFreeTrade"]);
                                    }
                                }

                                ot.Quotation.Site.Customer = new Customer();
                                ot.Quotation.Site.Customer.IdCustomer = Convert.ToInt32(otReader["IdCustomer"]);
                                ot.Quotation.Site.Customer.CustomerName = Convert.ToString(otReader["CustomerName"]);
                            }

                            //Detections Template
                            if (otReader["IdTemplate"] != DBNull.Value)
                            {
                                ot.Quotation.Template = new Template();
                                ot.Quotation.Template.IdTemplate = Convert.ToByte(otReader["IdTemplate"]);
                                ot.Quotation.Template.Name = Convert.ToString(otReader["Template"]);
                                ot.Quotation.Template.Type = Convert.ToInt64(otReader["IdTemplateType"]);
                            }

                            if (otReader["IdOffer"] != DBNull.Value)
                            {
                                ot.Quotation.IdOffer = Convert.ToInt32(otReader["IdOffer"]);

                                ot.Quotation.Offer = new Offer();
                                //ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);

                                if (otReader["IdCarriageMethod"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdCarriageMethod = Convert.ToByte(otReader["IdCarriageMethod"]);
                                    LookupValue carriage = new LookupValue();
                                    carriage.IdLookupValue = Convert.ToInt32(ot.Quotation.Offer.IdCarriageMethod);
                                    carriage.Value = Convert.ToString(otReader["CarriageValue"]);
                                    carriage.IdImage = Convert.ToInt64(otReader["CarriageImage"]);

                                    ot.Quotation.Offer.CarriageMethod = carriage;
                                }

                                if (otReader["PODate"] != DBNull.Value)
                                    ot.Quotation.Offer.POReceivedInDate = Convert.ToDateTime(otReader["PODate"]);

                                //if (otReader["IdOffer"] != DBNull.Value)
                                //{
                                //    ot.Quotation.Offer.OfferType = new OfferType();
                                //    ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);
                                //    ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);
                                //}

                                if (otReader["ProducerIdCountryGroup"] != DBNull.Value)
                                {
                                    ot.ProducerIdCountryGroup = Convert.ToInt64(otReader["ProducerIdCountryGroup"]);
                                    ot.CountryGroup = new CountryGroup();
                                    ot.CountryGroup.IdCountryGroup = Convert.ToInt64(otReader["ProducerIdCountryGroup"]);
                                    ot.CountryGroup.Name = Convert.ToString(otReader["ProducerCountryGroup"]);
                                    ot.CountryGroup.HtmlColor = Convert.ToString(otReader["ProducerCountryGroupColor"]);
                                }

                            }
                            ot.OtItems = GetOtItemsByIdOt(connectionString, idOt);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetWorkOrderByIdOt(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetWorkOrderByIdOt().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        public List<OtItem> GetOtItemsByIdOt(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetOtItemsByIdOt", connOtItem);
                otItemCommand.CommandType = CommandType.StoredProcedure;
                otItemCommand.Parameters.AddWithValue("_IdOt", idOt);

                using (MySqlDataReader otItemReader = otItemCommand.ExecuteReader())
                {
                    int keyId = 0;
                    while (otItemReader.Read())
                    {
                        try
                        {
                            OtItem otItem = new OtItem();

                            otItem.KeyId = keyId;
                            otItem.ParentId = -1;

                            otItem.IdOT = idOt;

                            otItem.IdOTItem = Convert.ToInt64(otItemReader["IdOtItem"]);
                            otItem.IsBatch = Convert.ToByte(otItemReader["IsBatch"]);

                            if (otItemReader["AttachedFiles"] != DBNull.Value)
                                otItem.AttachedFiles = Convert.ToString(otItemReader["AttachedFiles"]);

                            otItem.Rework = Convert.ToByte(otItemReader["Rework"]);

                            if (otItemReader["IdRevisionItem"] != DBNull.Value)
                            {
                                otItem.IdRevisionItem = Convert.ToInt64(otItemReader["IdRevisionItem"]);

                                otItem.RevisionItem = new RevisionItem();
                                otItem.RevisionItem.IdRevisionItem = otItem.IdRevisionItem;
                                otItem.RevisionItem.NumItem = Convert.ToString(otItemReader["NumItem"]);
                                otItem.RevisionItem.Quantity = Convert.ToDecimal(otItemReader["Quantity"]);
                                otItem.RevisionItem.UnitPrice = Convert.ToDecimal(otItemReader["UnitPrice"]);

                                if (otItemReader["CustomerCommentOT"] != DBNull.Value)
                                    otItem.RevisionItem.InternalComment = Convert.ToString(otItemReader["CustomerCommentOT"]);

                                if (otItemReader["CustomerCommentRevision"] != DBNull.Value)
                                    otItem.RevisionItem.CustomerComment = Convert.ToString(otItemReader["CustomerCommentRevision"]);

                                otItem.RevisionItem.WarehouseProduct = new WarehouseProduct();
                                otItem.RevisionItem.WarehouseProduct.IdWarehouseProduct = Convert.ToInt64(otItemReader["IdWarehouseproduct"]);

                                if (otItemReader["WpDescription"] != DBNull.Value)
                                    otItem.RevisionItem.WarehouseProduct.Description = Convert.ToString(otItemReader["WpDescription"]);

                                if (otItemReader["IdArticle"] != DBNull.Value)
                                {
                                    otItem.RevisionItem.WarehouseProduct.IdArticle = Convert.ToInt32(otItemReader["IdArticle"]);

                                    Article article = new Article();
                                    article.IdArticle = Convert.ToInt32(otItemReader["IdArticle"]);
                                    article.IsGeneric = Convert.ToSByte(otItemReader["IsGeneric"]);
                                    article.Reference = Convert.ToString(otItemReader["Reference"]);
                                    article.IsObsolete = Convert.ToSByte(otItemReader["IsObsolete"]);

                                    if (otItemReader["ArticleDescription"] != DBNull.Value)
                                        article.Description = Convert.ToString(otItemReader["ArticleDescription"]);

                                    if (otItemReader["Description_es"] != DBNull.Value)
                                        article.Description_es = Convert.ToString(otItemReader["Description_es"]);

                                    if (otItemReader["Description_fr"] != DBNull.Value)
                                        article.Description_fr = Convert.ToString(otItemReader["Description_fr"]);

                                    article.ImagePath = Convert.ToString(otItemReader["ImagePath"]);
                                    article.RegisterSerialNumber = Convert.ToByte(otItemReader["RegisterSerialNumber"]);

                                    if (otItemReader["Location"] != DBNull.Value)
                                        article.Location = Convert.ToString(otItemReader["Location"]);

                                    if (otItemReader["Weight"] != DBNull.Value)
                                        article.Weight = Convert.ToDecimal(otItemReader["Weight"]);

                                    if (otItemReader["IdArticleType"] != DBNull.Value)
                                        article.IdArticleType = Convert.ToInt64(otItemReader["IdArticleType"]);

                                    otItem.RevisionItem.WarehouseProduct.Article = article;
                                }

                                if (otItemReader["Iditemotstatus"] != DBNull.Value)
                                {
                                    otItem.IdItemOtStatus = Convert.ToByte(otItemReader["Iditemotstatus"]);

                                    otItem.Status = new ItemOTStatusType();
                                    otItem.Status.IdItemOtStatus = otItem.IdItemOtStatus;
                                    otItem.Status.Name = Convert.ToString(otItemReader["Status"]);
                                }

                                if (otItemReader["AssignedTo"] != DBNull.Value)
                                {
                                    otItem.AssignedTo = Convert.ToInt32(otItemReader["AssignedTo"]);

                                    otItem.AssignedToUser = new People();
                                    otItem.AssignedToUser.Name = Convert.ToString(otItemReader["AssignedName"]);
                                    otItem.AssignedToUser.Surname = Convert.ToString(otItemReader["AssignedSurname"]);
                                }


                            }



                            List<OtItem> decomposedOtItems = GetArticlesForDecomposition(connectionString, idOt, otItem, ref keyId);
                            otItems.Add(otItem);
                            if (decomposedOtItems != null && decomposedOtItems.Count > 0)
                            {

                                otItems.AddRange(decomposedOtItems);

                            }




                            keyId++;

                            //i = otItem.KeyId + 1;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetOtItemsByIdOt(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }


                }
            }

            //Log4NetLogger.Logger.Log("Executed GetOtItemsByIdOt().", category: Category.Info, priority: Priority.Low);
            return otItems;
        }

        public List<OtItem> GetArticlesForDecomposition(string connectionString, Int64 idOt, OtItem parentOtItem, ref int keyId)
        {
            List<OtItem> otItems = new List<OtItem>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetArticlesForDecomposition", connOtItem);
                otItemCommand.CommandType = CommandType.StoredProcedure;
                otItemCommand.Parameters.AddWithValue("_IdOTitem", parentOtItem.IdOTItem);

                using (MySqlDataReader otItemReader = otItemCommand.ExecuteReader())
                {
                    while (otItemReader.Read())
                    {
                        try
                        {
                            keyId++;


                            OtItem otItem = new OtItem();
                            otItem.IdOT = idOt;
                            otItem.IdOTItem = Convert.ToInt64(otItemReader["IdOtItem"]);

                            if (otItemReader["IdItemOtStatus"] != DBNull.Value)
                                otItem.IdItemOtStatus = Convert.ToByte(otItemReader["IdItemOtStatus"]);


                            otItem.Rework = Convert.ToByte(otItemReader["Rework"]);

                            if (otItemReader["IdRevisionItem"] != DBNull.Value)
                            {
                                otItem.IdRevisionItem = Convert.ToInt64(otItemReader["IdRevisionItem"]);

                                otItem.RevisionItem = new RevisionItem();
                                otItem.RevisionItem.IdRevisionItem = otItem.IdRevisionItem;
                                otItem.RevisionItem.NumItem = Convert.ToString(otItemReader["NumItem"]);



                                otItem.RevisionItem.WarehouseProduct = new WarehouseProduct();
                                otItem.RevisionItem.WarehouseProduct.IdWarehouseProduct = Convert.ToInt64(otItemReader["IdWarehouseproduct"]);

                                if (otItemReader["IdComponent"] != DBNull.Value)
                                {
                                    otItem.RevisionItem.WarehouseProduct.IdComponent = Convert.ToInt64(otItemReader["IdComponent"]);
                                }


                                otItem.KeyId = keyId;

                                if (otItemReader["parentArticleType"] != DBNull.Value)
                                {
                                    otItem.ParentArticleType = Convert.ToInt64(otItemReader["parentArticleType"]);
                                }

                                if (otItemReader["idParent"] != DBNull.Value)
                                {
                                    otItem.ParentId = Convert.ToInt32(otItemReader["idParent"]);
                                }

                                if (otItem.ParentId == -1)
                                {
                                    otItem.ParentId = parentOtItem.KeyId;
                                    otItem.RevisionItem.Quantity = parentOtItem.RevisionItem.Quantity * Convert.ToDecimal(otItemReader["Quantity"]);
                                }
                                else
                                {

                                    decimal qty = otItems.First(x => x.RevisionItem.WarehouseProduct.IdComponent == otItem.ParentId).RevisionItem.Quantity;
                                    otItem.RevisionItem.Quantity = qty * Convert.ToDecimal(otItemReader["Quantity"]);
                                    otItem.ParentId = otItems.First(x => x.RevisionItem.WarehouseProduct.IdComponent == otItem.ParentId).KeyId;
                                }



                                if (otItemReader["IdArticle"] != DBNull.Value)
                                {
                                    otItem.RevisionItem.WarehouseProduct.IdArticle = Convert.ToInt32(otItemReader["IdArticle"]);

                                    Article article = new Article();
                                    article.IdArticle = Convert.ToInt32(otItemReader["IdArticle"]);
                                    article.IsGeneric = Convert.ToSByte(otItemReader["IsGeneric"]);
                                    article.Reference = Convert.ToString(otItemReader["Reference"]);
                                    article.IsObsolete = Convert.ToSByte(otItemReader["IsObsolete"]);

                                    if (otItemReader["Description"] != DBNull.Value)
                                        article.Description = Convert.ToString(otItemReader["Description"]);

                                    if (otItemReader["Description_es"] != DBNull.Value)
                                        article.Description_es = Convert.ToString(otItemReader["Description_es"]);

                                    if (otItemReader["Description_fr"] != DBNull.Value)
                                        article.Description_fr = Convert.ToString(otItemReader["Description_fr"]);

                                    article.ImagePath = Convert.ToString(otItemReader["ImagePath"]);
                                    article.RegisterSerialNumber = Convert.ToByte(otItemReader["RegisterSerialNumber"]);

                                    if (otItemReader["Weight"] != DBNull.Value)
                                        article.Weight = Convert.ToDecimal(otItemReader["Weight"]);

                                    if (otItemReader["Location"] != DBNull.Value)
                                        article.Location = Convert.ToString(otItemReader["Location"]);

                                    if (otItemReader["IdArticleType"] != DBNull.Value)
                                        article.IdArticleType = Convert.ToInt64(otItemReader["IdArticleType"]);

                                    otItem.RevisionItem.WarehouseProduct.Article = article;
                                }

                                if (otItemReader["Iditemotstatus"] != DBNull.Value)
                                {
                                    otItem.IdItemOtStatus = Convert.ToByte(otItemReader["Iditemotstatus"]);

                                }


                            }


                            otItems.Add(otItem);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticlesForDecomposition(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                }
            }

            return otItems;
        }


        public List<OTAssignedUser> GetOTAssignedUsers(Company company, Int64 idOT, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAssignedUser> otAssignedUsers = new List<OTAssignedUser>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetOTAssignedUsers", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idOT", idOT);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTAssignedUser otAssignedUser = new OTAssignedUser();

                            if (reader["IdOTAssignedUser"] != DBNull.Value)
                                otAssignedUser.IdOTAssignedUser = Convert.ToInt64(reader["IdOTAssignedUser"]);

                            if (reader["IdOT"] != DBNull.Value)
                                otAssignedUser.IdOT = Convert.ToInt64(reader["IdOT"]);

                            if (reader["IdStage"] != DBNull.Value)
                            {
                                otAssignedUser.IdStage = Convert.ToByte(reader["IdStage"]);
                            }

                            if (reader["IdOperator"] != DBNull.Value)
                            {
                                otAssignedUser.IdUser = Convert.ToInt32(reader["IdOperator"]);

                                otAssignedUser.UserShortDetail = new UserShortDetail();
                                otAssignedUser.UserShortDetail.IdUser = Convert.ToInt32(reader["IdOperator"]);
                                if (reader["IdUserGender"] != DBNull.Value)
                                    otAssignedUser.UserShortDetail.IdUserGender = Convert.ToByte(reader["IdUserGender"]);

                                if (reader["Phone"] != DBNull.Value)
                                    otAssignedUser.UserShortDetail.PhoneNo = Convert.ToString(reader["Phone"]);

                                otAssignedUser.UserShortDetail.Login = Convert.ToString(reader["Login"]);
                                otAssignedUser.UserShortDetail.UserName = Convert.ToString(reader["UserName"]);
                                if (reader["CompanyEmail"] != DBNull.Value)
                                    otAssignedUser.UserShortDetail.CompanyEmail = Convert.ToString(reader["CompanyEmail"]);

                                if (!userShortDetails.Any(usd => usd.IdUser == otAssignedUser.IdUser))
                                {
                                    userShortDetails.Add(otAssignedUser.UserShortDetail);
                                }
                            }

                            otAssignedUsers.Add(otAssignedUser);
                        }

                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTAssignedUsers( IdOT-{0}). ErrorMessage- {1}", idOT, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            foreach (UserShortDetail itemUSD in userShortDetails)
            {
                byte[] UserImageInBytes = GetUserProfileImageInBytes(itemUSD.Login, userProfileImageFilePath);
                otAssignedUsers.Where(owt => owt.IdUser == itemUSD.IdUser).ToList().ForEach(x => x.UserShortDetail.UserImageInBytes = UserImageInBytes);
            }

            return otAssignedUsers;
        }


        public List<OTAssignedUser> GetUsersToAssignedOT(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAssignedUser> otAssignedUsers = new List<OTAssignedUser>();

            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetUsersToAssignedOT", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTAssignedUser otAssignedUser = new OTAssignedUser();

                            if (reader["IdUser"] != DBNull.Value)
                            {
                                UserShortDetail userShortDetail = new UserShortDetail();
                                userShortDetail.IdUser = Convert.ToInt32(reader["IdUser"]);

                                if (reader["IdUserGender"] != DBNull.Value)
                                    userShortDetail.IdUserGender = Convert.ToByte(reader["IdUserGender"]);

                                if (reader["Phone"] != DBNull.Value)
                                    userShortDetail.PhoneNo = Convert.ToString(reader["Phone"]);

                                userShortDetail.Login = Convert.ToString(reader["Login"]);
                                userShortDetail.UserName = Convert.ToString(reader["UserName"]);

                                if (reader["CompanyEmail"] != DBNull.Value)
                                    userShortDetail.CompanyEmail = Convert.ToString(reader["CompanyEmail"]);

                                if (!userShortDetails.Any(usd => usd.IdUser == userShortDetail.IdUser))
                                {
                                    userShortDetails.Add(userShortDetail);
                                }

                                otAssignedUser.UserShortDetail = userShortDetail;
                                otAssignedUsers.Add(otAssignedUser);
                            }


                        }

                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetUsersToAssignedOT(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            foreach (UserShortDetail itemUSD in userShortDetails)
            {
                byte[] UserImageInBytes = GetUserProfileImageInBytes(itemUSD.Login, userProfileImageFilePath);
                otAssignedUsers.Where(owt => owt.IdUser == itemUSD.IdUser).ToList().ForEach(x => x.UserShortDetail.UserImageInBytes = UserImageInBytes);
            }

            return otAssignedUsers;
        }
        public bool UpdateOTAssignedUser(Company company, List<OTAssignedUser> otAssignedUsers)
        {
            bool isUpdated = false;
            string connectionString = company.ConnectPlantConstr;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();

                        if (otAssignedUsers != null)
                            foreach (OTAssignedUser otAssignedUser in otAssignedUsers)
                            {
                                if (otAssignedUser.TransactionOperation == ModelBase.TransactionOperations.Add)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_AddOTAssignedUser", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdOT", otAssignedUser.IdOT);
                                    mySqlCommand.Parameters.AddWithValue("_IdStage", otAssignedUser.IdStage);
                                    mySqlCommand.Parameters.AddWithValue("_IdUser", otAssignedUser.IdUser);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (otAssignedUser.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {

                                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_RemoveOTAssignedUser", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdOTAssignedUser", otAssignedUser.IdOTAssignedUser);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        mySqlConnection.Close();
                    }


                    transactionScope.Complete();
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {

                if (Transaction.Current != null)
                    Transaction.Current.Rollback();

                if (transactionScope != null)
                    transactionScope.Dispose();

                isUpdated = false;
                throw;
            }
            return isUpdated;
        }


        public List<OTWorkingTime> GetOTWorkingTimeByIdOT_V2040(Company company, Int64 idOT, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTWorkingTime> otWorkingTimes = new List<OTWorkingTime>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetOTWorkingTimeByIdOT_V2040", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idOT", idOT);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTWorkingTime otWorkingTime = new OTWorkingTime();

                            otWorkingTime.Company = company;

                            if (reader["IdOT"] != DBNull.Value)
                                otWorkingTime.IdOT = Convert.ToInt64(reader["IdOT"]);

                            if (reader["IdOTWorkingTime"] != DBNull.Value)
                                otWorkingTime.IdOTWorkingTime = Convert.ToInt64(reader["IdOTWorkingTime"]);

                            if (reader["IdStage"] != DBNull.Value)
                            {
                                otWorkingTime.IdStage = Convert.ToByte(reader["IdStage"]);
                                otWorkingTime.Stage = new Stage();
                                otWorkingTime.Stage.IdStage = Convert.ToByte(reader["IdStage"]);
                                otWorkingTime.Stage.Name = Convert.ToString(reader["StageName"]);
                            }

                            if (reader["TotalWorkedHours"] != DBNull.Value)
                            {
                                otWorkingTime.TotalTime = (TimeSpan)reader["TotalWorkedHours"];

                                otWorkingTime.TotalTimeInString = string.Format("{0:00}:{1:00}", (Int32)otWorkingTime.TotalTime.TotalHours, otWorkingTime.TotalTime.Minutes);
                            }
                            else
                            {
                                otWorkingTime.TotalTime = new TimeSpan(0, 0, 0);
                                otWorkingTime.TotalTimeInString = string.Format("{0:00}:{1:00}", 0, 0);
                            }

                            if (reader["IdOperator"] != DBNull.Value)
                            {
                                otWorkingTime.IdOperator = Convert.ToInt32(reader["IdOperator"]);

                                otWorkingTime.UserShortDetail = new UserShortDetail();
                                otWorkingTime.UserShortDetail.IdUser = Convert.ToInt32(reader["IdOperator"]);

                                if (reader["IdUserGender"] != DBNull.Value)
                                    otWorkingTime.UserShortDetail.IdUserGender = Convert.ToByte(reader["IdUserGender"]);

                                otWorkingTime.UserShortDetail.Login = Convert.ToString(reader["Login"]);
                                otWorkingTime.UserShortDetail.UserName = Convert.ToString(reader["UserName"]);

                                if (reader["CompanyCode"] != DBNull.Value)
                                {
                                    otWorkingTime.UserShortDetail.CompanyCode = Convert.ToString(reader["CompanyCode"]);
                                }

                                if (!userShortDetails.Any(usd => usd.IdUser == otWorkingTime.IdOperator))
                                {
                                    userShortDetails.Add(otWorkingTime.UserShortDetail);
                                }
                            }

                            if (reader["IsTimerStarted"] != DBNull.Value)
                                otWorkingTime.IsTimerStarted = Convert.ToBoolean(reader["IsTimerStarted"]);

                            otWorkingTimes.Add(otWorkingTime);
                        }

                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTWorkingTimeByIdOT_V2040( IdOT-{0}). ErrorMessage- {1}", idOT, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            foreach (UserShortDetail itemUSD in userShortDetails)
            {
                byte[] UserImageInBytes = GetUserProfileImageInBytes(itemUSD.Login, userProfileImageFilePath);
                otWorkingTimes.Where(owt => owt.IdOperator == itemUSD.IdUser).ToList().ForEach(x => x.UserShortDetail.UserImageInBytes = UserImageInBytes);
            }

            return otWorkingTimes;
        }


        public List<ValidateItem> GetWorkOrderItemsToValidate(Company company, Int64 idOT)
        {
            string connectionString = company.ConnectPlantConstr;
            List<ValidateItem> validateItems = new List<ValidateItem>();

            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetWorkOrderItemsToValidate", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idOT", idOT);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ValidateItem validateItem = new ValidateItem();

                            if (reader["IdOT"] != DBNull.Value)
                                validateItem.IdOT = Convert.ToInt64(reader["IdOT"]);

                            if (reader["MergeCode"] != DBNull.Value)
                                validateItem.Code = Convert.ToString(reader["MergeCode"]);

                            if (reader["Quantity"] != DBNull.Value)
                                validateItem.Quantity = Convert.ToInt64(reader["Quantity"]);

                            if (reader["PartNumberCode"] != DBNull.Value)
                                validateItem.PartNumberCode = Convert.ToString(reader["PartNumberCode"]);

                            if (reader["Barcodestring"] != DBNull.Value)
                                validateItem.Barcodestring = Convert.ToString(reader["Barcodestring"]);

                            if (reader["CustomerName"] != DBNull.Value)
                                validateItem.Customer = Convert.ToString(reader["CustomerName"]);

                            if (reader["SiteName"] != DBNull.Value)
                                validateItem.SiteName = Convert.ToString(reader["SiteName"]);

                            if (reader["idArticle"] != DBNull.Value)
                            {
                                validateItem.IdArticle = Convert.ToInt64(reader["idArticle"]);
                                validateItem.Reference = Convert.ToString(reader["Reference"]);
                                validateItem.Description = Convert.ToString(reader["Description"]);
                                if (reader["ImagePath"] != DBNull.Value)
                                    validateItem.ImagePath = Convert.ToString(reader["ImagePath"]);
                            }

                            if (reader["Item"] != DBNull.Value)
                                validateItem.NumItem = Convert.ToString(reader["Item"]);

                            if (reader["idPartNumberTracking"] != DBNull.Value)
                            {
                                validateItem.IdPartNumberTracking = Convert.ToInt64(reader["idPartNumberTracking"]);
                                DateTime? EndDate = null;
                                sbyte Rework = 0;
                                if (reader["EndDate"] != DBNull.Value)
                                    EndDate = Convert.ToDateTime(reader["EndDate"]);
                                if (reader["Rework"] != DBNull.Value)
                                    Rework = Convert.ToSByte(reader["Rework"]);

                                if (EndDate == null)
                                {
                                    validateItem.IdItemStatus = 274;
                                }
                                else if (EndDate != null && Rework == 0)
                                {
                                    validateItem.IdItemStatus = 275;
                                }
                                else if (EndDate != null && Rework == 1)
                                {
                                    validateItem.IdItemStatus = 276;
                                }

                            }
                            validateItems.Add(validateItem);
                        }

                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWorkOrderItemsToValidate( IdOT-{0}). ErrorMessage- {1}", idOT, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }


            return validateItems;
        }

        public bool UpdateTestBoardPartNumberTracking(Company company, Int64 idPartNumberTracking, Int32 status, Int32 idOperator)
        {
            string connectionString = company.ConnectPlantConstr;
            bool isUpdated = false;

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_UpdateTestBoardPartNumberTracking", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idPartNumberTracking", idPartNumberTracking);
                    mySqlCommand.Parameters.AddWithValue("_status", status); //status.ToString().ToUpper()
                    mySqlCommand.Parameters.AddWithValue("_idOperator", idOperator);
                    mySqlCommand.ExecuteNonQuery();
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateTestBoardPartNumberTracking( IdPartNumberTracking-{0}). ErrorMessage- {1}", idPartNumberTracking, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return isUpdated;
        }

        public List<Ots> GetPendingWorkorders_V2043(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2043", connOts);
                otsCommand.CommandType = CommandType.StoredProcedure;
                otsCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);
                otsCommand.CommandTimeout = 8000;

                using (MySqlDataReader otReader = otsCommand.ExecuteReader())
                {
                    if (otReader.HasRows)
                    {
                        while (otReader.Read())
                        {
                            try
                            {
                                Ots ot = new Ots();
                                ot.IdOT = Convert.ToInt64(otReader["IdOT"]);
                                ot.NumOT = Convert.ToByte(otReader["NumOT"]);
                                ot.Site = company;
                                if (otReader["OtCode"] != DBNull.Value)
                                {
                                    ot.Code = Convert.ToString(otReader["OtCode"]);
                                    ot.MergeCode = Convert.ToString(otReader["MergeCode"]);
                                }

                                if (otReader["DeliveryDate"] != DBNull.Value)
                                {
                                    ot.DeliveryDate = Convert.ToDateTime(otReader["DeliveryDate"]);
                                    ot.Delay = (int)(ot.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;
                                }

                                ot.Quotation = new Quotation();

                                if (otReader["IdQuotation"] != DBNull.Value)
                                {
                                    ot.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                    ot.Quotation.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                    ot.Quotation.Code = Convert.ToString(otReader["QuotationCode"]);
                                }

                                if (otReader["IdTemplate"] != DBNull.Value)
                                {
                                    ot.Quotation.IdDetectionsTemplate = Convert.ToByte(otReader["IdTemplate"]);

                                    ot.Quotation.Template = new Template();
                                    ot.Quotation.Template.IdTemplate = ot.Quotation.IdDetectionsTemplate;
                                    ot.Quotation.Template.Name = Convert.ToString(otReader["Template"]);
                                    ot.Quotation.Template.IdTemplate = Convert.ToByte(otReader["TemplateType"]);
                                }

                                if (otReader["IdOffer"] != DBNull.Value)
                                {
                                    ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                    ot.Quotation.Offer = new Offer();
                                    ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                    if (otReader["IsCritical"] != DBNull.Value)
                                        ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                    if (otReader["IdStatus"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdStatus = Convert.ToInt64(otReader["IdStatus"]);
                                        ot.Quotation.Offer.GeosStatus = new GeosStatus();
                                        ot.Quotation.Offer.GeosStatus.IdOfferStatusType = Convert.ToInt64(otReader["IdStatus"]);
                                        ot.Quotation.Offer.GeosStatus.Name = Convert.ToString(otReader["OfferStatus"]);
                                        ot.Quotation.Offer.GeosStatus.HtmlColor = Convert.ToString(otReader["OfferStatusHtmlColor"]);

                                        if (otReader["IdSalesStatusType"] != DBNull.Value)
                                        {
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType = new SalesStatusType();
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.IdSalesStatusType = Convert.ToInt64(otReader["IdSalesStatusType"]);
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.Name = Convert.ToString(otReader["SalesStatusType"]);
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.HtmlColor = Convert.ToString(otReader["SalesStatusHtmlColor"]);
                                            if (otReader["SalesStatusIdImage"] != DBNull.Value)
                                                ot.Quotation.Offer.GeosStatus.SalesStatusType.IdImage = Convert.ToInt64(otReader["SalesStatusIdImage"]);
                                        }
                                    }

                                    ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                    ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                    if (otReader["IdOfferType"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);

                                        ot.Quotation.Offer.OfferType = new OfferType();
                                        ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                        ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);

                                        //ot.MergeCode = ot.Quotation.Code + " " + ot.Quotation.Offer.OfferType.Name + " " + ot.NumOT;
                                    }
                                    if (otReader["CarProject"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.CarProject = new CarProject();
                                        ot.Quotation.Offer.CarProject.Name = Convert.ToString(otReader["CarProject"]);
                                    }
                                    if (otReader["CarOEM"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.CarOEM = new CarOEM();
                                        ot.Quotation.Offer.CarOEM.Name = Convert.ToString(otReader["CarOEM"]);
                                    }
                                    if (otReader["BusinessUnit"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.BusinessUnit = new LookupValue();
                                        ot.Quotation.Offer.BusinessUnit.Value = Convert.ToString(otReader["BusinessUnit"]);
                                    }
                                    if (otReader["IdCarriageMethod"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.IdCarriageMethod = Convert.ToByte(otReader["IdCarriageMethod"]);
                                        ot.Quotation.Offer.CarriageMethod = new LookupValue();
                                        ot.Quotation.Offer.CarriageMethod.IdLookupValue = Convert.ToByte(ot.Quotation.Offer.IdCarriageMethod);
                                        ot.Quotation.Offer.CarriageMethod.Value = Convert.ToString(otReader["CarriageMethod"]);

                                        if (otReader["CarriageImage"] != DBNull.Value)
                                            ot.Quotation.Offer.CarriageMethod.IdImage = Convert.ToInt64(otReader["CarriageImage"]);
                                    }
                                }

                                //if (otReader["IdWarehouse"] != DBNull.Value)
                                //{
                                //    ot.Quotation.IdWarehouse = Convert.ToInt64(otReader["IdWarehouse"]);
                                //}

                                if (otReader["IdSite"] != DBNull.Value)
                                {
                                    ot.Quotation.IdCustomer = Convert.ToInt32(otReader["IdSite"]);
                                    ot.Quotation.Site = new Company();
                                    ot.Quotation.Site.IdCompany = ot.Quotation.IdCustomer;
                                    ot.Quotation.Site.SiteNameWithoutCountry = Convert.ToString(otReader["SiteName"]);

                                    if (otReader["ShortName"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(otReader["ShortName"])))
                                    {
                                        ot.Quotation.Site.ShortName = Convert.ToString(otReader["ShortName"]);
                                        ot.Quotation.Site.Name = ot.Quotation.Site.ShortName;
                                    }
                                    else
                                    {
                                        ot.Quotation.Site.ShortName = null;
                                        ot.Quotation.Site.Name = Convert.ToString(otReader["SiteName"]);
                                    }

                                    ot.Quotation.Site.Customer = new Customer();
                                    ot.Quotation.Site.Customer.IdCustomer = Convert.ToInt32(otReader["IdCustomer"]);
                                    ot.Quotation.Site.Customer.CustomerName = Convert.ToString(otReader["CustomerName"]);

                                    if (otReader["IdCountry"] != DBNull.Value)
                                    {
                                        ot.Quotation.Site.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                        ot.Quotation.Site.Country = new Country();
                                        ot.Quotation.Site.Country.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                        ot.Quotation.Site.Country.Name = Convert.ToString(otReader["CountryName"]);
                                    }
                                }



                                if (otReader["PODate"] != DBNull.Value)
                                    ot.PoDate = Convert.ToDateTime(otReader["PODate"]);

                                if (otReader["ProducerCountryGroupName"] != DBNull.Value)
                                {
                                    ot.CountryGroup = new CountryGroup();
                                    ot.CountryGroup.Name = otReader["ProducerCountryGroupName"].ToString();
                                    if (otReader["ProducerCountryGroupHTMLColor"] != DBNull.Value)
                                        ot.CountryGroup.HtmlColor = otReader["ProducerCountryGroupHTMLColor"].ToString();
                                }

                                if (otReader["ExpectedStartDate"] != DBNull.Value)
                                    ot.ExpectedStartDate = Convert.ToDateTime(otReader["ExpectedStartDate"]);

                                if (otReader["ExpectedEndDate"] != DBNull.Value)
                                    ot.ExpectedEndDate = Convert.ToDateTime(otReader["ExpectedEndDate"]);

                                if (otReader["Progress"] != DBNull.Value)
                                    ot.Progress = Convert.ToByte(otReader["Progress"]);

                                if (otReader["Operators"] != DBNull.Value)
                                {
                                    ot.Operators = Convert.ToString(otReader["Operators"]);
                                    ot.UserShortDetails = new List<UserShortDetail>();

                                    foreach (string itemIdUser in ot.Operators.Split(','))
                                    {
                                        if (!ot.UserShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                        {
                                            UserShortDetail usd = new UserShortDetail();
                                            usd.IdUser = Convert.ToInt32(itemIdUser);

                                            ot.UserShortDetails.Add(usd);
                                            if (!userShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                            {
                                                userShortDetails.Add(usd);
                                            }

                                            // usd.UserImageInBytes = GetUserProfileImageInBytes(itemLogin, userProfileImageFilePath);

                                        }

                                    }


                                }

                                // ot.Modules = 0;

                                //ot.RemainingQuantity = ot.ActualQuantity + ot.DownloadedQuantity;
                                //if (ot.ActualQuantity > 0)
                                //{
                                //    ot.Status = (Int16)((Math.Abs(ot.DownloadedQuantity) / ot.ActualQuantity) * 100);
                                //}
                                //ot.OtItems = new List<OtItem>();
                                ots.Add(ot);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2043() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                        if (otReader.NextResult())
                        {
                            while (otReader.Read())
                            {
                                try
                                {
                                    if (otReader["IdOffer"] != DBNull.Value)
                                    {
                                        if (ots.Any(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])))
                                        {

                                            ots.Where(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])).ToList().ForEach(x => x.Modules = Convert.ToInt64(otReader["Modules"]));
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2043() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                        }
                    }


                }
            }
            // ots = ots.Where(x => x.Status < 100).ToList();
            string getIdUsers = string.Join(",", userShortDetails.Select(i => i.IdUser).ToList());
            userShortDetails = GetUserShortDetail(getIdUsers, connectionString, userProfileImageFilePath);
            foreach (Ots itemots in ots)
            {
                if (itemots.Operators != null)
                {
                    List<string> names = new List<string>();

                    foreach (string itemidUser in itemots.Operators.Split(','))
                    {
                        names.Add(userShortDetails.Where(i => i.IdUser.ToString() == itemidUser).Select(i => i.UserName).FirstOrDefault());
                    }

                    itemots.OperatorNames = string.Join(",", names.ToList());
                }

                if (itemots.UserShortDetails != null)
                {
                    foreach (UserShortDetail itemOPUSD in itemots.UserShortDetails)
                    {
                        if (userShortDetails.Any(usd => usd.IdUser == itemOPUSD.IdUser))
                        {
                            UserShortDetail usdItem = userShortDetails.Where(usd => usd.IdUser == itemOPUSD.IdUser).FirstOrDefault();
                            itemOPUSD.IdUserGender = usdItem.IdUserGender;
                            itemOPUSD.UserName = usdItem.UserName;
                            itemOPUSD.UserImageInBytes = usdItem.UserImageInBytes;
                        }
                    }
                }

            }

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2043().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        public List<OTAttachment> GetOTAttachment(Company company, Int64 idOT, string workOrderPath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAttachment> otAttachments = new List<OTAttachment>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_GetOTAttachment", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdOT", idOT);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    if (reader.Read())
                    {


                        if (reader["AttachedFiles"] != DBNull.Value)
                        {

                            string attachedFiles = Convert.ToString(reader["AttachedFiles"]);
                            foreach (string item in attachedFiles.Split(';').ToList())
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    OTAttachment otAttachment = new OTAttachment();

                                    if (reader["IdOT"] != DBNull.Value)
                                        otAttachment.IdOT = Convert.ToInt64(reader["IdOT"]);

                                    if (reader["QuotationYear"] != DBNull.Value)
                                        otAttachment.QuotationYear = Convert.ToString(reader["QuotationYear"]);

                                    if (reader["QuotationCode"] != DBNull.Value)
                                        otAttachment.QuotationCode = Convert.ToString(reader["QuotationCode"]);

                                    otAttachment.FileName = item;
                                    otAttachment.FileType = "Specs";

                                    string[] stringExtension = otAttachment.FileName.Split('.');
                                    if (stringExtension.Count() > 0)
                                    {
                                        if (!string.IsNullOrEmpty(stringExtension[1]))
                                        {
                                            otAttachment.FileExtension = "." + stringExtension[1];
                                        }
                                    }


                                    otAttachments.Add(otAttachment);
                                }

                            }

                        }
                    }
                }
            }
            foreach (OTAttachment otAttachment in otAttachments)
            {
                if (!string.IsNullOrEmpty(otAttachment.FileName))
                {
                    string filePath = Path.Combine(workOrderPath, otAttachment.QuotationYear, otAttachment.QuotationCode, "Additional Information", otAttachment.FileName);

                    FileInfo fi = new FileInfo(filePath);
                    if (fi.Exists)
                    {
                        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
                        if (fi.Length == 0)
                            otAttachment.FileSize = "0" + suf[0];
                        long bytes = Math.Abs(fi.Length);
                        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                        double num = Math.Round(bytes / Math.Pow(1024, place));
                        otAttachment.FileSize = (Math.Sign(fi.Length) * num).ToString() + " " + suf[place];
                    }
                }
            }

            return otAttachments;
        }

        public byte[] GetOTAttachmentInBytes(string fileName, string workOrderPath, string quotationYear, string quotationCode)
        {
            byte[] bytes = null;
            string filePath = "";
            filePath = Path.Combine(workOrderPath, quotationYear, quotationCode, "Additional Information", fileName);

            if (File.Exists(filePath))
            {
                using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

                return bytes;
            }
            else
            {
                return null;
            }

        }

        public List<OTAssignedUser> GetUsersToAssignedOT_V2044(Company company, string userProfileImageFilePath, Int32 idCompany)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAssignedUser> otAssignedUsers = new List<OTAssignedUser>();

            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetUsersToAssignedOT_V2044", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCompany", idCompany);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTAssignedUser otAssignedUser = new OTAssignedUser();

                            if (reader["IdUser"] != DBNull.Value)
                            {
                                UserShortDetail userShortDetail = new UserShortDetail();
                                userShortDetail.IdUser = Convert.ToInt32(reader["IdUser"]);

                                if (reader["IdUserGender"] != DBNull.Value)
                                    userShortDetail.IdUserGender = Convert.ToByte(reader["IdUserGender"]);

                                if (reader["Phone"] != DBNull.Value)
                                    userShortDetail.PhoneNo = Convert.ToString(reader["Phone"]);

                                userShortDetail.Login = Convert.ToString(reader["Login"]);
                                userShortDetail.UserName = Convert.ToString(reader["UserName"]);

                                if (reader["CompanyEmail"] != DBNull.Value)
                                    userShortDetail.CompanyEmail = Convert.ToString(reader["CompanyEmail"]);

                                if (!userShortDetails.Any(usd => usd.IdUser == userShortDetail.IdUser))
                                {
                                    userShortDetails.Add(userShortDetail);
                                }

                                otAssignedUser.UserShortDetail = userShortDetail;
                                otAssignedUsers.Add(otAssignedUser);
                            }


                        }

                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetUsersToAssignedOT(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            foreach (UserShortDetail itemUSD in userShortDetails)
            {
                byte[] UserImageInBytes = GetUserProfileImageInBytes(itemUSD.Login, userProfileImageFilePath);
                otAssignedUsers.Where(owt => owt.IdUser == itemUSD.IdUser).ToList().ForEach(x => x.UserShortDetail.UserImageInBytes = UserImageInBytes);
            }

            return otAssignedUsers;
        }

        public List<Ots> GetPendingWorkorders_V2044(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();
            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2044", connOts);
                otsCommand.CommandType = CommandType.StoredProcedure;
                otsCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);
                otsCommand.CommandTimeout = 8000;
                using (MySqlDataAdapter adap = new MySqlDataAdapter(otsCommand))
                {
                    adap.Fill(dataSet);
                }
            }
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    foreach (DataRow otReader in dataSet.Tables[0].Rows)
                    {
                        try
                        {
                            Ots ot = new Ots();
                            ot.IdOT = Convert.ToInt64(otReader["IdOT"]);
                            ot.NumOT = Convert.ToByte(otReader["NumOT"]);
                            if (otReader["OTIdSite"] != DBNull.Value)
                                ot.IdSite = Convert.ToInt32(otReader["OTIdSite"]);
                            ot.Site = company;
                            if (otReader["OtCode"] != DBNull.Value)
                            {
                                ot.Code = Convert.ToString(otReader["OtCode"]);
                                ot.MergeCode = Convert.ToString(otReader["MergeCode"]);
                            }

                            if (otReader["DeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(otReader["DeliveryDate"]);
                                ot.Delay = (int)(ot.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;
                            }

                            ot.Quotation = new Quotation();

                            if (otReader["IdQuotation"] != DBNull.Value)
                            {
                                ot.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                ot.Quotation.IdQuotation = Convert.ToInt64(otReader["IdQuotation"]);
                                ot.Quotation.Code = Convert.ToString(otReader["QuotationCode"]);
                            }

                            if (otReader["IdTemplate"] != DBNull.Value)
                            {
                                ot.Quotation.IdDetectionsTemplate = Convert.ToByte(otReader["IdTemplate"]);

                                ot.Quotation.Template = new Template();
                                ot.Quotation.Template.IdTemplate = ot.Quotation.IdDetectionsTemplate;
                                ot.Quotation.Template.Name = Convert.ToString(otReader["Template"]);
                                ot.Quotation.Template.IdTemplate = Convert.ToByte(otReader["TemplateType"]);
                            }

                            if (otReader["IdOffer"] != DBNull.Value)
                            {
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdStatus"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdStatus = Convert.ToInt64(otReader["IdStatus"]);
                                    ot.Quotation.Offer.GeosStatus = new GeosStatus();
                                    ot.Quotation.Offer.GeosStatus.IdOfferStatusType = Convert.ToInt64(otReader["IdStatus"]);
                                    ot.Quotation.Offer.GeosStatus.Name = Convert.ToString(otReader["OfferStatus"]);
                                    ot.Quotation.Offer.GeosStatus.HtmlColor = Convert.ToString(otReader["OfferStatusHtmlColor"]);

                                    if (otReader["IdSalesStatusType"] != DBNull.Value)
                                    {
                                        ot.Quotation.Offer.GeosStatus.SalesStatusType = new SalesStatusType();
                                        ot.Quotation.Offer.GeosStatus.SalesStatusType.IdSalesStatusType = Convert.ToInt64(otReader["IdSalesStatusType"]);
                                        ot.Quotation.Offer.GeosStatus.SalesStatusType.Name = Convert.ToString(otReader["SalesStatusType"]);
                                        ot.Quotation.Offer.GeosStatus.SalesStatusType.HtmlColor = Convert.ToString(otReader["SalesStatusHtmlColor"]);
                                        if (otReader["SalesStatusIdImage"] != DBNull.Value)
                                            ot.Quotation.Offer.GeosStatus.SalesStatusType.IdImage = Convert.ToInt64(otReader["SalesStatusIdImage"]);
                                    }
                                }

                                ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                if (otReader["IdOfferType"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);

                                    ot.Quotation.Offer.OfferType = new OfferType();
                                    ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                    ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);

                                    //ot.MergeCode = ot.Quotation.Code + " " + ot.Quotation.Offer.OfferType.Name + " " + ot.NumOT;
                                }
                                if (otReader["CarProject"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.CarProject = new CarProject();
                                    ot.Quotation.Offer.CarProject.Name = Convert.ToString(otReader["CarProject"]);
                                }
                                if (otReader["CarOEM"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.CarOEM = new CarOEM();
                                    ot.Quotation.Offer.CarOEM.Name = Convert.ToString(otReader["CarOEM"]);
                                }
                                if (otReader["BusinessUnit"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.BusinessUnit = new LookupValue();
                                    ot.Quotation.Offer.BusinessUnit.Value = Convert.ToString(otReader["BusinessUnit"]);
                                }
                                if (otReader["IdCarriageMethod"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdCarriageMethod = Convert.ToByte(otReader["IdCarriageMethod"]);
                                    ot.Quotation.Offer.CarriageMethod = new LookupValue();
                                    ot.Quotation.Offer.CarriageMethod.IdLookupValue = Convert.ToByte(ot.Quotation.Offer.IdCarriageMethod);
                                    ot.Quotation.Offer.CarriageMethod.Value = Convert.ToString(otReader["CarriageMethod"]);

                                    if (otReader["CarriageImage"] != DBNull.Value)
                                        ot.Quotation.Offer.CarriageMethod.IdImage = Convert.ToInt64(otReader["CarriageImage"]);
                                }

                                if (otReader["IdProductCategory"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdProductCategory = Convert.ToInt64(otReader["IdProductCategory"]);
                                }
                            }

                            //if (otReader["IdWarehouse"] != DBNull.Value)
                            //{
                            //    ot.Quotation.IdWarehouse = Convert.ToInt64(otReader["IdWarehouse"]);
                            //}

                            if (otReader["IdSite"] != DBNull.Value)
                            {
                                ot.Quotation.IdCustomer = Convert.ToInt32(otReader["IdSite"]);
                                ot.Quotation.Site = new Company();
                                ot.Quotation.Site.IdCompany = ot.Quotation.IdCustomer;
                                ot.Quotation.Site.SiteNameWithoutCountry = Convert.ToString(otReader["SiteName"]);

                                if (otReader["ShortName"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(otReader["ShortName"])))
                                {
                                    ot.Quotation.Site.ShortName = Convert.ToString(otReader["ShortName"]);
                                    ot.Quotation.Site.Name = ot.Quotation.Site.ShortName;
                                }
                                else
                                {
                                    ot.Quotation.Site.ShortName = null;
                                    ot.Quotation.Site.Name = Convert.ToString(otReader["SiteName"]);
                                }

                                ot.Quotation.Site.Customer = new Customer();
                                ot.Quotation.Site.Customer.IdCustomer = Convert.ToInt32(otReader["IdCustomer"]);
                                ot.Quotation.Site.Customer.CustomerName = Convert.ToString(otReader["CustomerName"]);

                                if (otReader["IdCountry"] != DBNull.Value)
                                {
                                    ot.Quotation.Site.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                    ot.Quotation.Site.Country = new Country();
                                    ot.Quotation.Site.Country.IdCountry = Convert.ToByte(otReader["IdCountry"]);
                                    ot.Quotation.Site.Country.Name = Convert.ToString(otReader["CountryName"]);
                                }
                            }



                            if (otReader["PODate"] != DBNull.Value)
                                ot.PoDate = Convert.ToDateTime(otReader["PODate"]);

                            if (otReader["ProducerCountryGroupName"] != DBNull.Value)
                            {
                                ot.CountryGroup = new CountryGroup();
                                ot.CountryGroup.Name = otReader["ProducerCountryGroupName"].ToString();
                                if (otReader["ProducerCountryGroupHTMLColor"] != DBNull.Value)
                                    ot.CountryGroup.HtmlColor = otReader["ProducerCountryGroupHTMLColor"].ToString();
                            }

                            if (otReader["ExpectedStartDate"] != DBNull.Value)
                                ot.ExpectedStartDate = Convert.ToDateTime(otReader["ExpectedStartDate"]);

                            if (otReader["ExpectedEndDate"] != DBNull.Value)
                                ot.ExpectedEndDate = Convert.ToDateTime(otReader["ExpectedEndDate"]);

                            if (otReader["Progress"] != DBNull.Value)
                                ot.Progress = Convert.ToByte(otReader["Progress"]);

                            if (otReader["Operators"] != DBNull.Value)
                            {
                                ot.Operators = Convert.ToString(otReader["Operators"]);
                                ot.UserShortDetails = new List<UserShortDetail>();

                                foreach (string itemIdUser in ot.Operators.Split(','))
                                {
                                    if (!ot.UserShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                    {
                                        UserShortDetail usd = new UserShortDetail();
                                        usd.IdUser = Convert.ToInt32(itemIdUser);

                                        ot.UserShortDetails.Add(usd);
                                        if (!userShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                        {
                                            userShortDetails.Add(usd);
                                        }

                                        // usd.UserImageInBytes = GetUserProfileImageInBytes(itemLogin, userProfileImageFilePath);

                                    }

                                }


                            }

                            // ot.Modules = 0;

                            //ot.RemainingQuantity = ot.ActualQuantity + ot.DownloadedQuantity;
                            //if (ot.ActualQuantity > 0)
                            //{
                            //    ot.Status = (Int16)((Math.Abs(ot.DownloadedQuantity) / ot.ActualQuantity) * 100);
                            //}
                            //ot.OtItems = new List<OtItem>();
                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2044() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                {

                    foreach (DataRow otReader in dataSet.Tables[1].Rows)
                    {
                        try
                        {
                            if (otReader["IdOffer"] != DBNull.Value)
                            {
                                if (ots.Any(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])))
                                {

                                    ots.Where(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])).ToList().ForEach(x => x.Modules = Convert.ToInt64(otReader["Modules"]));
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2044() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
                if (dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
                {
                    List<OTWorkingTime> otWorkingTimes = new List<OTWorkingTime>();
                    foreach (DataRow otReader in dataSet.Tables[2].Rows)
                    {
                        try
                        {
                            OTWorkingTime otworkingtime = new OTWorkingTime();
                            if (otReader["IdOT"] != DBNull.Value)
                                otworkingtime.IdOT = Convert.ToInt64(otReader["IdOT"]);
                            if (otReader["StartTime"] != DBNull.Value)
                                otworkingtime.StartTime = Convert.ToDateTime(otReader["StartTime"]);
                            if (otReader["EndTime"] != DBNull.Value)
                                otworkingtime.EndTime = Convert.ToDateTime(otReader["EndTime"]);
                            if (otReader["TotalTime"] != DBNull.Value)
                            {
                                otworkingtime.TotalTime = (TimeSpan)otReader["TotalTime"];
                                otworkingtime.TotalTime = new TimeSpan(otworkingtime.TotalTime.Days, otworkingtime.TotalTime.Hours, otworkingtime.TotalTime.Minutes, 00);

                                if (otworkingtime.TotalTime.Days > 0)
                                {
                                    int Hours = otworkingtime.TotalTime.Days * 24 + otworkingtime.TotalTime.Hours;
                                    otworkingtime.Hours = string.Format("{0}H {1}M", Hours, otworkingtime.TotalTime.Minutes);
                                }
                                else
                                {
                                    otworkingtime.Hours = string.Format("{0}H {1}M", otworkingtime.TotalTime.Hours, otworkingtime.TotalTime.Minutes);
                                }
                            }
                            otWorkingTimes.Add(otworkingtime);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2044() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt=>owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i=>i.StartTime);
                            otupdate.RealEndDate = item.ToList().Max(i => i.EndTime);
                            TimeSpan worklogTotalTime = new TimeSpan(item.ToList().Sum(r => r.TotalTime.Ticks));
                            //[001] changed the date formate
                            int Hours = worklogTotalTime.Days * 24 + worklogTotalTime.Hours;
                            otupdate.RealDuration = string.Format("{0}H {1}M", Hours, worklogTotalTime.Minutes);

                        }
                    }
                }

                if (dataSet.Tables[3] != null && dataSet.Tables[3].Rows.Count > 0)
                {

                    foreach (DataRow otReader in dataSet.Tables[3].Rows)
                    {
                        List<Ots> otsInOffer = ots.Where(t => t.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"].ToString())).ToList();
                        List<OptionsByOfferGrid> OptionsByOffers = new List<OptionsByOfferGrid>();
                        OptionsByOfferGrid optionsByOfferGrid = new OptionsByOfferGrid();
                        optionsByOfferGrid.IdOffer = Convert.ToInt64(otReader["IdOffer"].ToString());
                        optionsByOfferGrid.IdOption = Convert.ToInt64(otReader["IdOption"].ToString());
                       // optionsByOfferGrid.IdOfferPlant = Convert.ToInt32(timelineParams.connectSiteDetailParams.idSite);
                        if (otReader["Quantity"] != DBNull.Value)
                        {
                            optionsByOfferGrid.Quantity = Convert.ToInt32(otReader["Quantity"].ToString());
                            if (optionsByOfferGrid.Quantity > 0)
                                optionsByOfferGrid.IsSelected = true;
                            else
                                optionsByOfferGrid.IsSelected = false;
                        }
                        else
                        {
                            optionsByOfferGrid.IsSelected = false;
                        }
                        optionsByOfferGrid.OfferOption = otReader["offeroption"].ToString();

                        OptionsByOffers.Add(optionsByOfferGrid);
                        if(OptionsByOffers!=null && OptionsByOffers.Count>0)
                        ots.Where(t => t.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"].ToString())).ToList().ForEach(t => t.Quotation.Offer.OptionsByOffersGrid.AddRange(OptionsByOffers));

                    }
                }

                if (dataSet.Tables[4] != null && dataSet.Tables[4].Rows.Count > 0)
                {

                    foreach (DataRow otReader in dataSet.Tables[4].Rows)
                    {

                        if (otReader["IdProductSubCategory"] != DBNull.Value)
                        {
                            ots.Where(i => i.Quotation.Offer.IdProductCategory == Convert.ToInt64(otReader["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.Quotation.Offer.ProductCategoryGrid = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(otReader["IdProductSubCategory"].ToString()), Name = otReader["ProductSubCategory"].ToString() }; });

                            if (otReader["IdCategory"] != DBNull.Value)
                            {
                                ots.Where(i => i.Quotation.Offer.IdProductCategory == Convert.ToInt64(otReader["IdProductSubCategory"].ToString())).ToList().ForEach(u => { u.Quotation.Offer.ProductCategoryGrid.IdParent = Convert.ToInt64(otReader["IdCategory"].ToString()); u.Quotation.Offer.ProductCategoryGrid.Category = new ProductCategoryGrid() { IdProductCategory = Convert.ToInt64(otReader["IdCategory"].ToString()), Name = otReader["Category"].ToString() }; });
                            }
                        }

                    }
                }
                if (dataSet.Tables[5] != null && dataSet.Tables[5].Rows.Count > 0)
                {

                    foreach (DataRow otReader in dataSet.Tables[5].Rows)
                    {
                        try
                        {
                            if (otReader["IdOffer"] != DBNull.Value)
                            {
                                if (ots.Any(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])))
                                {
                                    if (otReader["ProducedModules"] != DBNull.Value)
                                        ots.Where(ot => ot.Quotation.IdOffer == Convert.ToInt64(otReader["IdOffer"])).ToList().ForEach(x => x.ProducedModules = Convert.ToInt64(otReader["ProducedModules"]));
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2044() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }


                
            
            // ots = ots.Where(x => x.Status < 100).ToList();
            string getIdUsers = string.Join(",", userShortDetails.Select(i => i.IdUser).ToList());
            userShortDetails = GetUserShortDetail(getIdUsers, connectionString, userProfileImageFilePath);
            foreach (Ots itemots in ots)
            {
                if (itemots.Operators != null)
                {
                    List<string> names = new List<string>();

                    foreach (string itemidUser in itemots.Operators.Split(','))
                    {
                        names.Add(userShortDetails.Where(i => i.IdUser.ToString() == itemidUser).Select(i => i.UserName).FirstOrDefault());
                    }

                    itemots.OperatorNames = string.Join(",", names.ToList());
                }

                if (itemots.UserShortDetails != null)
                {
                    foreach (UserShortDetail itemOPUSD in itemots.UserShortDetails)
                    {
                        if (userShortDetails.Any(usd => usd.IdUser == itemOPUSD.IdUser))
                        {
                            UserShortDetail usdItem = userShortDetails.Where(usd => usd.IdUser == itemOPUSD.IdUser).FirstOrDefault();
                            itemOPUSD.IdUserGender = usdItem.IdUserGender;
                            itemOPUSD.UserName = usdItem.UserName;
                            itemOPUSD.UserImageInBytes = usdItem.UserImageInBytes;
                        }
                    }
                }

            }

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2044().", category: Category.Info, priority: Priority.Low);
            return ots;
        }


        public bool UpdateOTFromGrid(Company company, Ots ot)
        {
            string connectionString = company.ConnectPlantConstr;
            bool isUpdated = false;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_UpdateOTFromGrid", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOT", ot.IdOT);
                        mySqlCommand.Parameters.AddWithValue("_Progress", ot.Progress);
                        mySqlCommand.Parameters.AddWithValue("_ExpectedStartDate", ot.ExpectedStartDate);
                        mySqlCommand.Parameters.AddWithValue("_ExpectedEndDate", ot.ExpectedEndDate);

                        mySqlCommand.ExecuteNonQuery();
                        isUpdated = true;
                        mySqlConnection.Close();
                    }


                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (Transaction.Current != null)
                    Transaction.Current.Rollback();

                if (transactionScope != null)
                    transactionScope.Dispose();
                isUpdated = false;
                throw;
            }

            return isUpdated;
        }

        /// <summary>
        /// This method is used to get all WorkLog User List By Period And Site.
        /// </summary>
        /// <param name="SAMConnectionString">The SAM connection string.</param>
        /// <returns>WorkLog User List By Period And Site..</returns>
        public List<WorklogUser> GetWorkLogUserListByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, string userProfileImageFilePath, Company plant)
        {
            string connectionString = plant.ConnectPlantConstr;
            List<WorklogUser> UserList = new List<WorklogUser>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetWorkLogUserListByPeriodAndSite", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                    mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            WorklogUser user = new WorklogUser();

                            user.IdUser = Convert.ToInt32(mySqlDataReader["IdUser"]);
                            user.Login = Convert.ToString(mySqlDataReader["Login"]);

                            if (mySqlDataReader["Name"] != DBNull.Value)
                            {
                                user.FirstName = Convert.ToString(mySqlDataReader["Name"]);
                            }

                            if (mySqlDataReader["Surname"] != DBNull.Value)
                            {
                                user.LastName = Convert.ToString(mySqlDataReader["Surname"]);
                            }

                            if (mySqlDataReader["hours"] != DBNull.Value)
                            {
                                user.Hours = Convert.ToString(mySqlDataReader["hours"]);
                            }
                            if (mySqlDataReader["IdPersonGender"] != DBNull.Value)
                            {
                                user.IdGender = Convert.ToInt32(mySqlDataReader["IdPersonGender"]);
                            }
                            if (mySqlDataReader["seconds_in"] != DBNull.Value)
                            {
                                user.Seconds = Convert.ToInt32(mySqlDataReader["seconds_in"]);
                            }
                            if (mySqlDataReader["sec"] != DBNull.Value)
                            {
                                user.ExtraSeconds = Convert.ToInt32(mySqlDataReader["sec"]);
                            }
                            user.ProfileImageInBytes = GetUserProfileImageInBytes(user.Login, userProfileImageFilePath);
                            UserList.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWorkLogUserListByPeriodAndSite(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return UserList;
        }


        /// <summary>
        /// This method is used to get all WorkLog hours By Period And Site.
        /// </summary>
        /// <param name="SAMConnectionString">The SAM connection string.</param>
        /// <returns>WorkLog hours By Period And Site..</returns>
        public List<TempWorklog> GetWorkLogHoursByPeriodAndSite(string SAMConnectionString, DateTime FromDate, DateTime ToDate, int IdSite)
        {
            List<TempWorklog> WorklogList = new List<TempWorklog>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(SAMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetWorkLogHoursByPeriodAndSite", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                    mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            TempWorklog worklog = new TempWorklog();

                            if (mySqlDataReader["date"] != DBNull.Value)
                            {
                                worklog.StartTime = Convert.ToDateTime(mySqlDataReader["date"]);
                            }
                            if (mySqlDataReader["EndDate"] != DBNull.Value)
                            {
                                worklog.EndTime = Convert.ToDateTime(mySqlDataReader["EndDate"]);
                            }

                            if (mySqlDataReader["hours"] != DBNull.Value)
                            {
                                worklog.Description = Convert.ToString(mySqlDataReader["hours"]);
                            }


                            WorklogList.Add(worklog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWorkLogHoursByPeriodAndSite(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return WorklogList;
        }

        /// <summary>
        /// This method is used to get all WorkLog hours By Period And Site.
        /// </summary>
        /// <param name="SAMConnectionString">The SAM connection string.</param>
        /// <returns>WorkLog hours By Period And Site..</returns>
        public List<TempWorklog> GetWorkLogOTWithHoursByPeriodAndSiteAndUser(string SAMConnectionString, DateTime FromDate, DateTime ToDate, int IdSite, int IdUser)
        {
            List<TempWorklog> WorklogList = new List<TempWorklog>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(SAMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetWorkLogOTWithHoursByPeriodAndSiteAndUser", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                    mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);
                    mySqlCommand.Parameters.AddWithValue("_IdUser", IdUser);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            TempWorklog worklog = new TempWorklog();
                            if (mySqlDataReader["IdOT"] != DBNull.Value)
                            {
                                worklog.IdOT = Convert.ToInt64(mySqlDataReader["IdOT"]);
                            }
                            if (mySqlDataReader["date"] != DBNull.Value)
                            {
                                worklog.StartTime = Convert.ToDateTime(mySqlDataReader["date"]);
                            }
                            if (mySqlDataReader["EndDate"] != DBNull.Value)
                            {
                                worklog.EndTime = Convert.ToDateTime(mySqlDataReader["EndDate"]);
                            }

                            if (mySqlDataReader["hours"] != DBNull.Value)
                            {
                                worklog.Description = Convert.ToString(mySqlDataReader["hours"]);
                            }

                            WorklogList.Add(worklog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWorkLogHoursByPeriodAndSite(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return WorklogList;
        }

        /// <summary>
        /// This method is used to get all WorkLog OT with hours and users By Period And Site.
        /// </summary>
        /// <param name="SAMConnectionString">The SAM connection string.</param>
        /// <returns>all WorkLog OT with hours and users By Period And Site</returns>
        public List<TempWorklog> GetWorkLogOTWithHoursAndUserByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company plant)
        {
            string connectionString = plant.ConnectPlantConstr;
            List<TempWorklog> WorklogList = new List<TempWorklog>();
            int IdOT_neg = -2;
            int iduser = 0;
            DateTime? date = DateTime.Now;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetWorkLogOTWithHoursAndUserByPeriodAndSite", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                    mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            TempWorklog worklog = new TempWorklog();
                            if (mySqlDataReader["IdOT"] != DBNull.Value)
                            {
                                worklog.IdOT = Convert.ToInt64(mySqlDataReader["IdOT"]);
                            }
                            if (mySqlDataReader["date"] != DBNull.Value)
                            {
                                worklog.StartTime = Convert.ToDateTime(mySqlDataReader["date"]);
                            }
                            if (mySqlDataReader["EndDate"] != DBNull.Value)
                            {
                                worklog.EndTime = Convert.ToDateTime(mySqlDataReader["EndDate"]);
                            }
                            if (mySqlDataReader["hours"] != DBNull.Value)
                            {
                                worklog.Description = Convert.ToString(mySqlDataReader["hours"]);
                            }
                            if (mySqlDataReader["IdOperator"] != DBNull.Value)
                            {
                                worklog.IdUser = Convert.ToInt32(mySqlDataReader["IdOperator"]);
                            }
                            if (mySqlDataReader["CustomerSiteCountryName"] != DBNull.Value)
                            {
                                worklog.CustomerName = Convert.ToString(mySqlDataReader["CustomerSiteCountryName"]);
                            }
                            if (mySqlDataReader["OtCode"] != DBNull.Value)
                            {
                                worklog.OtCode = Convert.ToString(mySqlDataReader["OtCode"]);
                            }
                            if (mySqlDataReader["TotalHours"] != DBNull.Value)
                            {
                                worklog.Hours = Convert.ToString(mySqlDataReader["TotalHours"]);
                            }
                            if (mySqlDataReader["seconds"] != DBNull.Value)
                            {
                                worklog.Seconds = Convert.ToInt32(mySqlDataReader["seconds"]);
                            }
                            if (mySqlDataReader["sec"] != DBNull.Value)
                            {
                                worklog.ExtraSeconds = Convert.ToInt32(mySqlDataReader["sec"]);
                            }
                            if (WorklogList.Count == 0)
                            {
                                TempWorklog worklog1 = new TempWorklog();
                                worklog1.IdOT = IdOT_neg;
                                worklog1.StartTime = worklog.StartTime;
                                worklog1.EndTime = worklog.EndTime;
                                worklog1.IdUser = worklog.IdUser;
                                worklog1.OtCode = "Total Hours";
                                worklog1.Hours = worklog.Hours;
                                worklog1.Description = worklog.Hours;

                                WorklogList.Add(worklog1);

                                iduser = worklog1.IdUser;
                                date = worklog1.StartTime;
                            }
                            else
                            {
                                if (iduser != worklog.IdUser || date != worklog.StartTime)
                                {
                                    TempWorklog worklog1 = new TempWorklog();
                                    worklog1.IdOT = IdOT_neg;
                                    worklog1.StartTime = worklog.StartTime;
                                    worklog1.EndTime = worklog.EndTime;
                                    worklog1.IdUser = worklog.IdUser;
                                    worklog1.OtCode = "Total Hours";
                                    worklog1.Hours = worklog.Hours;
                                    worklog1.Description = worklog.Hours;

                                    WorklogList.Add(worklog1);

                                    iduser = worklog1.IdUser;
                                    date = worklog1.StartTime;
                                }
                            }
                            WorklogList.Add(worklog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWorkLogOTWithHoursAndUserByPeriodAndSite(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return WorklogList;

            //var seconds = Worklogs.Where(a => a.StartTime == date).Sum(a => a.Seconds);
            //seconds = seconds - Worklogs.Where(a => a.StartTime == date).Sum(a => a.ExtraSeconds);
            //TimeSpan ts = TimeSpan.FromSeconds(seconds);
            //var float_number = ts.TotalHours;
            //var result = float_number - Math.Truncate(float_number);
            //var anwer = result * 60;
            //worklog.Hours = Math.Truncate(float_number) + "H " + Math.Truncate(anwer) + "M";
            //worklog.Description = Math.Truncate(float_number) + "H " + Math.Truncate(anwer) + "M";
        }


        /// <summary>
        /// This method is used to get all WorkLog OT time By Period And Site.
        /// </summary>
        /// <param name="SAMConnectionString">The SAM connection string.</param>
        /// <returns>all WorkLog OT time By Period And Site</returns>
        public List<TempWorklog> GetOTWorkLogTimesByPeriodAndSite(DateTime FromDate, DateTime ToDate, int IdSite, Company plant)
        {
            string connectionString = plant.ConnectPlantConstr;

            List<TempWorklog> WorklogList = new List<TempWorklog>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetOTWorkLogTimesByPeriodAndSite", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                    mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            TempWorklog worklog = new TempWorklog();
                            if (mySqlDataReader["IdOT"] != DBNull.Value)
                            {
                                worklog.IdOT = Convert.ToInt64(mySqlDataReader["IdOT"]);
                            }
                            if (mySqlDataReader["OTCode"] != DBNull.Value)
                            {
                                worklog.OtCode = Convert.ToString(mySqlDataReader["OTCode"]);
                            }
                            if (mySqlDataReader["StartDate"] != DBNull.Value)
                            {
                                worklog.StartTime = Convert.ToDateTime(mySqlDataReader["StartDate"]);
                            }
                            if (mySqlDataReader["EndDate"] != DBNull.Value)
                            {
                                worklog.EndTime = Convert.ToDateTime(mySqlDataReader["EndDate"]);
                            }
                            if (mySqlDataReader["StartTime"] != DBNull.Value)
                                worklog.STime = Convert.ToDateTime(Convert.ToDateTime(mySqlDataReader["StartTime"]).ToString("dd/MM/yyyy HH:mm"));

                            if (mySqlDataReader["EndTime"] != DBNull.Value)
                                worklog.ETime = Convert.ToDateTime(Convert.ToDateTime(mySqlDataReader["EndTime"]).ToString("dd/MM/yyyy HH:mm"));

                            if (mySqlDataReader["totalHours"] != DBNull.Value)
                            {
                                worklog.TotalTime = (TimeSpan)mySqlDataReader["totalHours"];
                                worklog.TotalTime = new TimeSpan(worklog.TotalTime.Days, worklog.TotalTime.Hours, worklog.TotalTime.Minutes, 00);

                                if (worklog.TotalTime.Days > 0)
                                {
                                    int Hours = worklog.TotalTime.Days * 24 + worklog.TotalTime.Hours;
                                    worklog.Hours = string.Format("{0}H {1}M", Hours, worklog.TotalTime.Minutes);
                                }
                                else
                                {
                                    worklog.Hours = string.Format("{0}H {1}M", worklog.TotalTime.Hours, worklog.TotalTime.Minutes);
                                }
                            }
                            if (mySqlDataReader["IdUser"] != DBNull.Value)
                            {
                                worklog.IdUser = Convert.ToInt32(mySqlDataReader["IdUser"]);
                                worklog.WorklogUser = new WorklogUser();
                                worklog.WorklogUser.IdUser = Convert.ToInt32(mySqlDataReader["IdUser"]);
                                worklog.WorklogUser.FirstName = Convert.ToString(mySqlDataReader["Name"]);
                                worklog.WorklogUser.LastName = Convert.ToString(mySqlDataReader["Surname"]);
                            }
                           
                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                worklog.IdSite = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            worklog.Site = plant;
                            WorklogList.Add(worklog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWorkLogOTWithHoursAndUserByPeriodAndSite(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return WorklogList;
        }

        public bool DeleteWorkLog(Company company, Int64 idOTWorkingTime)
        {
            bool isDeleted = false;
            string connectionString = company.ConnectPlantConstr;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTWorkingTimeDelete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOTWorkingTime", idOTWorkingTime);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }


                    transactionScope.Complete();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {

                if (Transaction.Current != null)
                    Transaction.Current.Rollback();

                if (transactionScope != null)
                    transactionScope.Dispose();

                isDeleted = false;
                throw;
            }
            return isDeleted;
        }

        public bool UpdateWorkLog(Company company,OTWorkingTime otWorkingTime)
        {
            bool isUpdated = false;
            string connectionString = company.ConnectPlantConstr;
            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTWorkingTimeUpdate", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOTWorkingTime", otWorkingTime.IdOTWorkingTime);
                        mySqlCommand.Parameters.AddWithValue("_IdOperator", otWorkingTime.IdOperator);
                        mySqlCommand.Parameters.AddWithValue("_StartTime", otWorkingTime.StartTime);
                        mySqlCommand.Parameters.AddWithValue("_EndTime", otWorkingTime.EndTime);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }


                    transactionScope.Complete();
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {

                if (Transaction.Current != null)
                    Transaction.Current.Rollback();

                if (transactionScope != null)
                    transactionScope.Dispose();

                isUpdated = false;
                throw;
            }
            return isUpdated;
        }
    }
}
