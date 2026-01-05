using ChinhDo.Transactions;
using DevExpress.Spreadsheet;
using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.DataAccess;
using MySql.Data.MySqlClient;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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
                    CreateIfNotExists(ApplicationLogFilePath); //[GEOS2-5404][rdixit][13.08.2024]
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                    Log4NetLogger.Logger.Log(string.Format("SAMManager()..... Constructor Executed"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch
            {
                throw;
            }
        }

        //[GEOS2-5404][rdixit][13.08.2024]
        void CreateIfNotExists(string config_path)
        {
            string log4netConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <configuration>
                                          <configSections>
                                            <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
                                          </configSections>
                                          <log4net debug=""true"">
                                            <appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
                                              <file value=""C:\Temp\LogsService.txt""/>
                                              <appendToFile value=""true"" />
                                              <rollingStyle value=""Size"" />
                                              <maxSizeRollBackups value=""10"" />
                                              <maximumFileSize value=""10MB"" />
                                              <staticLogFileName value=""true"" />
                                              <layout type=""log4net.Layout.PatternLayout"">
                                                <conversionPattern value=""%-5p %d %5rms - %m%n"" />
                                              </layout>
                                            </appender>
                                            <root>
                                              <level value=""Info"" />
                                              <appender-ref ref=""RollingLogFileAppender"" />
                                            </root>
                                          </log4net>
                                        </configuration>";

            if (!File.Exists(config_path))
            {
                File.WriteAllText(config_path, log4netConfig);
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
							//[nsatpute][01-07-2025][GEOS2-8783]
                            if (warehouseReader["StartDate"] != DBNull.Value)
                                otWorkingTime.StartTime = Convert.ToDateTime(Convert.ToDateTime(warehouseReader["StartDate"]));

                            //if (warehouseReader["StartTime"] != DBNull.Value)
                            //    otWorkingTime.StartTimeInHoursAndMinutes = warehouseReader["StartTime"].ToString();
							//[nsatpute][01-07-2025][GEOS2-8783]
                            if (warehouseReader["EndDate"] != DBNull.Value)
                                otWorkingTime.EndTime = Convert.ToDateTime(Convert.ToDateTime(warehouseReader["EndDate"]));

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

                                    //string[] stringExtension = otAttachment.FileName.Split('.');
                                    //if (stringExtension.Count() > 0)
                                    //{
                                    //    if (!string.IsNullOrEmpty(stringExtension[1]))
                                    //    {
                                    //        otAttachment.FileExtension = "." + stringExtension[1];
                                    //    }
                                    //}

                                    //chitra.girigosavi [GEOS2-5326] Wrong extension in attached document]
                                    if (!string.IsNullOrEmpty(otAttachment.FileName))
                                    {
                                        string fileExtension = Path.GetExtension(otAttachment.FileName);
                                        otAttachment.FileExtension = fileExtension;
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
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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

        public bool UpdateWorkLog(Company company, OTWorkingTime otWorkingTime)
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

        public List<Ots> GetPendingWorkorders_V2090(Company company, string userProfileImageFilePath)
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2090() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2090() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2090() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2090() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2090().", category: Category.Info, priority: Priority.Low);
            return ots;
        }


        //[GEOS2-2906]
        public List<Ots> GetPendingWorkorders_V2170(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();
            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2170", connOts);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2170() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2170() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2170() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2170() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2170().", category: Category.Info, priority: Priority.Low);
            return ots;
        }
        public bool UpdateWorkflowStatusInOT(string MainServerConnectionString, UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            bool status;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTS_UpdateWorkflowStatus", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                        mySqlCommand.Parameters.AddWithValue("_IdWorkflowStatus", IdWorkflowStatus);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        status = true;
                    }
                    AddCommentsOrLogEntriesByOT(MainServerConnectionString, LogEntriesByOTList);
                    UpdateInOT(MainServerConnectionString, IdOT, IdUser);
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateWorkflowStatusInOT(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return status;
        }

        /// <summary>
        /// This method is used to insert Comments Or Log Entries By OT
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="LogEntriesByOTList">Get Log entry or comment list details.</param>
        public void AddCommentsOrLogEntriesByOT(string MainServerConnectionString, List<LogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                if (LogEntriesByOTList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (LogEntriesByOT logEntriesByOT in LogEntriesByOTList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("SAM_log_entries_by_OTS_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdOT", logEntriesByOT.IdOT);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logEntriesByOT.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_Comments", logEntriesByOT.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", logEntriesByOT.IdLogEntryType);
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", logEntriesByOT.IsRtfText);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCommentsOrLogEntriesByOT(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void UpdateInOT(string MainServerConnectionString, UInt64 IdOT, int IdUser)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_UpdateInOT", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCommentsOrLogEntriesByOT(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowStatus> GetAllWorkflowStatus(string connectionString)
        {

            List<WorkflowStatus> workflowStatusList = new List<WorkflowStatus>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetAllWorkflowStatus", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            WorkflowStatus workflowStatus = new WorkflowStatus();

                            workflowStatus.IdWorkflowStatus = Convert.ToByte(mySqlDataReader["IdWorkflowStatus"]);

                            if (mySqlDataReader["IdWorkflow"] != DBNull.Value)
                                workflowStatus.IdWorkflow = Convert.ToUInt32(mySqlDataReader["IdWorkflow"].ToString());

                            if (mySqlDataReader["Name"] != DBNull.Value)
                                workflowStatus.Name = mySqlDataReader["Name"].ToString();

                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                                workflowStatus.HtmlColor = mySqlDataReader["HtmlColor"].ToString();

                            if (mySqlDataReader["IdWorkflowScope"] != DBNull.Value)
                                workflowStatus.IdWorkflowScope = Convert.ToUInt32(mySqlDataReader["IdWorkflowScope"].ToString());

                            workflowStatusList.Add(workflowStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllWorkflowStatus(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return workflowStatusList;
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowTransition> GetAllWorkflowTransitions(string connectionString)
        {

            List<WorkflowTransition> workflowTransitionList = new List<WorkflowTransition>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_Get_workflow_transitions", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            WorkflowTransition workflowTransition = new WorkflowTransition();

                            workflowTransition.IdWorkflowTransition = Convert.ToUInt32(mySqlDataReader["IdWorkflowTransition"]);

                            if (mySqlDataReader["IdWorkflow"] != DBNull.Value)
                                workflowTransition.IdWorkflow = Convert.ToUInt32(mySqlDataReader["IdWorkflow"]);

                            if (mySqlDataReader["Name"] != DBNull.Value)
                                workflowTransition.Name = mySqlDataReader["Name"].ToString();

                            if (mySqlDataReader["IdWorkflowStatusFrom"] != DBNull.Value)
                                workflowTransition.IdWorkflowStatusFrom = Convert.ToByte(mySqlDataReader["IdWorkflowStatusFrom"]);

                            if (mySqlDataReader["IdWorkflowStatusTo"] != DBNull.Value)
                                workflowTransition.IdWorkflowStatusTo = Convert.ToByte(mySqlDataReader["IdWorkflowStatusTo"]);

                            if (mySqlDataReader["IsCommentRequired"] != DBNull.Value)
                                workflowTransition.IsCommentRequired = Convert.ToByte(mySqlDataReader["IsCommentRequired"]);

                            if (mySqlDataReader["IdWorkflowScope"] != DBNull.Value)
                                workflowTransition.IdWorkflowScope = Convert.ToUInt32(mySqlDataReader["IdWorkflowScope"]);

                            workflowTransitionList.Add(workflowTransition);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllWorkflowTransitions(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return workflowTransitionList;
        }



        /// <summary>
        /// This method is to get workorder details
        /// </summary>
        /// [cpatil][18-12-2019][GEOS2-1760]Open Work Order details when double-click in Order
        /// [cpatil][24-07-2021][GEOS2-2902]
        /// <param name="idOt">Get id ot</param>
        /// <param name="company">Get company details</param>
        /// <returns>Get ots details</returns>
        public Ots GetWorkOrderByIdOt_V2170(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2170", connOT);
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }



                            }
                            ot.OtItems = GetOtItemsByIdOt_V2170(connectionString, idOt);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetWorkOrderByIdOt_V2170(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetWorkOrderByIdOt_V2170().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        public List<OtItem> GetOtItemsByIdOt_V2170(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetOtItemsByIdOt_V2170", connOtItem);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }


                }
            }

            //Log4NetLogger.Logger.Log("Executed GetOtItemsByIdOt().", category: Category.Info, priority: Priority.Low);
            return otItems;
        }


        /// <summary>
        /// This method is to get workorder details
        /// </summary>
        /// [cpatil][30-07-2021][GEOS2-2906]
        /// <param name="idOt">Get id ot</param>
        /// <param name="company">Get company details</param>
        /// <returns>Get ots details</returns>
        public Ots GetStructureWorkOrderByIdOt_V2170(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2170", connOT);
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
                            ot.LstDetection = new List<Detection>();
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }



                            }
                            ot.OtItems = GetStructureOtItemsByIdOt_V2170(connectionString, idOt);

                            if (ot.OtItems != null && ot.OtItems.Count > 0)
                                ot.LstDetection = ot.OtItems.FirstOrDefault().LstDetection;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureWorkOrderByIdOt_V2170(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetStructureWorkOrderByIdOt_V2170().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        public List<OtItem> GetStructureOtItemsByIdOt_V2170(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();
            List<Detection> otDetectionLst = new List<Detection>();
            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetStructureOtItemsByIdOt_V2170", connOtItem);
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

                                otItem.RevisionItem.CPProduct = new CPProduct();
                                otItem.RevisionItem.CPProduct.Reference = Convert.ToString(otItemReader["Reference"]);

                                if (otItemReader["IdProduct"] != DBNull.Value)
                                    otItem.RevisionItem.IdProduct = Convert.ToInt64(otItemReader["IdProduct"]);

                                if (otItemReader["IdCPType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.IdCPType = Convert.ToByte(otItemReader["IdCPType"]);

                                if (otItemReader["ProductType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.ProductTypeName = Convert.ToString(otItemReader["ProductType"]);

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
                            otItem.RevisionItem.CPProduct.LstCPDetection = new List<CPDetection>();
                            otItem.LstDetection = new List<Detection>();
                            otItems.Add(otItem);




                            keyId++;

                            //i = otItem.KeyId + 1;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    if (otItemReader.NextResult())
                        while (otItemReader.Read())
                        {
                            try
                            {
                                if (otItemReader["CPProductID"] != DBNull.Value)
                                {
                                    if (otItems.Any(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])))
                                    {
                                        List<OtItem> lstotitems = otItems.Where(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])).ToList();

                                        CPDetection cpdetection = new CPDetection();
                                        cpdetection.CPProductID = Convert.ToInt64(otItemReader["CPProductID"]);
                                        if (otItemReader["DetectionID"] != DBNull.Value)
                                            cpdetection.DetectionID = Convert.ToInt32(otItemReader["DetectionID"]);
                                        if (otItemReader["Name"] != DBNull.Value)
                                            cpdetection.DetectionName = Convert.ToString(otItemReader["Name"]);
                                        if (otItemReader["NumDetections"] != DBNull.Value)
                                            cpdetection.NumDetections = Convert.ToInt32(otItemReader["NumDetections"]);

                                        lstotitems.ForEach(x => { x.RevisionItem.CPProduct.LstCPDetection.Add(cpdetection); });
                                        if (!otDetectionLst.Any(i => i.IdDetection == cpdetection.DetectionID))
                                        {
                                            Detection detection = new Detection();
                                            detection.IdDetection = Convert.ToUInt32(cpdetection.DetectionID);
                                            detection.Name = Convert.ToString(cpdetection.DetectionName);
                                            otDetectionLst.Add(detection);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }


                }
            }
            otItems.ForEach(x => { x.LstDetection = otDetectionLst; });
            //Log4NetLogger.Logger.Log("Executed GetOtItemsByIdOt().", category: Category.Info, priority: Priority.Low);
            return otItems;
        }

        /// <summary>
        /// This method is to get workorder details
        /// </summary>
        /// [cpatil][18-12-2019][GEOS2-1760]Open Work Order details when double-click in Order
        /// [cpatil][24-07-2021][GEOS2-2902]
        /// [adhatkar][GEOS2-2961][01-09-2021]
        /// <param name="idOt">Get id ot</param>
        /// <param name="company">Get company details</param>
        /// <returns>Get ots details</returns>
        public Ots GetWorkOrderByIdOt_V2180(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2180", connOT);
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }

                                if (otReader["Observations"] != DBNull.Value)
                                    ot.Observations = Convert.ToString(otReader["Observations"]);

                            }
                            ot.OtItems = GetOtItemsByIdOt_V2170(connectionString, idOt);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetWorkOrderByIdOt_V2180(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetWorkOrderByIdOt_V2180().", category: Category.Info, priority: Priority.Low);
            return ot;
        }


        public bool UpdateOTAssignedUser_V2180(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser)
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

                    if (OldRemark != Remark)
                        UpdateOTSObservation(connectionString, IdOT, Remark, IdUser);

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
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOTAssignedUser_V2180(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }


        public void UpdateOTSObservation(string MainServerConnectionString, Int64 IdOT, string Observations, int IdUser)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTS_UpdateObservation", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                    mySqlCommand.Parameters.AddWithValue("_Observations", Observations);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOTSObservation(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void UpdateOTAttachment(string MainServerConnectionString, Int64 IdOT, string otsAttachmentFiles, int IdUser)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTS_UpdateAttachment", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                    mySqlCommand.Parameters.AddWithValue("_OTAttachment", otsAttachmentFiles);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOTAttachment(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[GEOS2-2961]
        public List<Ots> GetPendingWorkorders_V2180(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();
            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2180", connOts);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

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

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2180().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        public bool UpdateOTFromGrid_V2180(Company company, Ots ot)
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

                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_UpdateOTFromGrid_V2180", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOT", ot.IdOT);
                        mySqlCommand.Parameters.AddWithValue("_Progress", ot.Progress);
                        mySqlCommand.Parameters.AddWithValue("_ExpectedStartDate", ot.ExpectedStartDate);
                        mySqlCommand.Parameters.AddWithValue("_ExpectedEndDate", ot.ExpectedEndDate);
                        mySqlCommand.Parameters.AddWithValue("_Observations", ot.Observations);

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


        //[GEOS2-2959]
        public List<Ots> GetPendingWorkordersForDashboard_V2180(Company company, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();
            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkordersForDashboard_V2180", connOts);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

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
                                    ot.Quotation.Site.Name = Convert.ToString(otReader["Customer"]);
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

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2180() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            //Only the orders with “Planned Start Date” will appear in Dashboard


            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2180().", category: Category.Info, priority: Priority.Low);
            return ots;
        }



        //[GEOS2-2961]
        public List<Ots> GetAllAssignedWorkordersForPlanning_V2250(Company company, string userProfileImageFilePath,
            out List<OTWorkingTime> listLoggedHoursForOT_User_Date, out List<PlannedHoursForOT_User_Date> listPlannedHoursForOT_User_Date)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();
            listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
            listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetAllAssignedWorkordersForPlanning_V2250", connOts);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

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

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetAllAssignedWorkordersForPlanning_V2250() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                {
                    // List<OTWorkingTime> 
                    //otWorkingTimes = new List<OTWorkingTime>();

                    foreach (DataRow otReader in dataSet.Tables[1].Rows)
                    {
                        try
                        {
                            OTWorkingTime otworkingtime = new OTWorkingTime();

                            if (otReader["IdOTWorkingTime"] != DBNull.Value)
                                otworkingtime.IdOTWorkingTime = Convert.ToInt64(otReader["IdOTWorkingTime"]);

                            if (otReader["IdOT"] != DBNull.Value)
                                otworkingtime.IdOT = Convert.ToInt64(otReader["IdOT"]);

                            if (otReader["IdStage"] != DBNull.Value)
                                otworkingtime.IdStage = Convert.ToByte(otReader["IdStage"]);

                            if (otReader["IdOperator"] != DBNull.Value)
                                otworkingtime.IdOperator = Convert.ToInt32(otReader["IdOperator"]);

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
                            listLoggedHoursForOT_User_Date.Add(otworkingtime);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetAllAssignedWorkordersForPlanning_V2250() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in listLoggedHoursForOT_User_Date.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
                            otupdate.RealEndDate = item.ToList().Max(i => i.EndTime);
                            TimeSpan worklogTotalTime = new TimeSpan(item.ToList().Sum(r => r.TotalTime.Ticks));
                            //[001] changed the date formate
                            int Hours = worklogTotalTime.Days * 24 + worklogTotalTime.Hours;
                            otupdate.RealDuration = string.Format("{0}H {1}M", Hours, worklogTotalTime.Minutes);

                        }
                    }
                }


                if (dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
                {
                    //// List<PlannedHoursForOT_User_Date> 
                    //ListPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();

                    foreach (DataRow otReader in dataSet.Tables[2].Rows)
                    {
                        try
                        {
                            PlannedHoursForOT_User_Date plannedHoursForOT_User_Date = new PlannedHoursForOT_User_Date();

                            if (otReader["IdOTUserPlanning"] != DBNull.Value)
                                plannedHoursForOT_User_Date.IdOTUserPlanning = Convert.ToInt64(otReader["IdOTUserPlanning"]);

                            if (otReader["IdOT"] != DBNull.Value)
                                plannedHoursForOT_User_Date.IdOT = Convert.ToInt64(otReader["IdOT"]);

                            if (otReader["IdUser"] != DBNull.Value)
                                plannedHoursForOT_User_Date.IdUser = Convert.ToInt32(otReader["IdUser"]);

                            if (otReader["Date"] != DBNull.Value)
                                plannedHoursForOT_User_Date.PlanningDate = Convert.ToDateTime(otReader["Date"]);

                            if (otReader["TimeEstimation"] != DBNull.Value)
                                plannedHoursForOT_User_Date.TimeEstimationInHours = Convert.ToSingle(otReader["TimeEstimation"]);

                            listPlannedHoursForOT_User_Date.Add(plannedHoursForOT_User_Date);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetAllAssignedWorkordersForPlanning_V2250() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetAllAssignedWorkordersForPlanning_V2250().", category: Category.Info, priority: Priority.Low);
            return ots;
        }


        public bool UpdateOTUserPlanningsFromGrid_V2250(Company company, List<PlannedHoursForOT_User_Date> listLoggedHoursForOT_User_Date)
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

                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_UpdateOTUserPlanningsFromGrid_V2250", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        foreach (PlannedHoursForOT_User_Date item in listLoggedHoursForOT_User_Date)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdOT", item.IdOT);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", item.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Date", item.PlanningDate);


                            Log4NetLogger.Logger.Log(
                                $"Default CultureInfo: DisplayName={System.Globalization.CultureInfo.CurrentCulture.DisplayName}" +
                                $", CurrencyDecimalSeparator={System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}" +
                                $", NumberGroupSeparator={System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator}",
                                category: Category.Info, priority: Priority.Low);

                            if (!item.TimeEstimationInHours.HasValue) item.TimeEstimationInHours = 0;

                            float timeEstimationInFloat = item.TimeEstimationInHours.Value;
                            mySqlCommand.Parameters.AddWithValue("_TimeEstimation", timeEstimationInFloat);

                            Log4NetLogger.Logger.Log($"TimeEstimationInHoursString={item.TimeEstimationInHours}", category: Category.Info, priority: Priority.Low);
                            Log4NetLogger.Logger.Log($"timeEstimationInFloat={timeEstimationInFloat}", category: Category.Info, priority: Priority.Low);

                            mySqlCommand.ExecuteNonQuery();
                        }
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
            Log4NetLogger.Logger.Log("Executed UpdateOTUserPlanningsFromGrid_V2250().", category: Category.Info, priority: Priority.Low);
            return isUpdated;
        }





        public List<Ots> GetSAMWorkOrdersReport_V2260(Company company, string userProfileImageFilePath, DateTime fromDate, DateTime toDate, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();
            var listLoggedHoursForOT_User_Date = new List<OTWorkingTime>();
            var listPlannedHoursForOT_User_Date = new List<PlannedHoursForOT_User_Date>();
            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetSAMWorkOrdersReport_V2260", connOts);
                otsCommand.CommandType = CommandType.StoredProcedure;
                otsCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);
                otsCommand.Parameters.AddWithValue("_FromDate", fromDate);
                otsCommand.Parameters.AddWithValue("_ToDate", toDate);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

                                }

                                ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                if (otReader["IdOfferType"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);

                                    ot.Quotation.Offer.OfferType = new OfferType();
                                    ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                    ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);

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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
                                }

                                if (otReader["Region"] != DBNull.Value)
                                {
                                    ot.Quotation.Site.Region = Convert.ToString(otReader["Region"]);
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

                                    }

                                }

                            }

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();

                            if (otReader["IdProductCategory"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.ProductCategory = new ProductCategory();

                                ot.Quotation.Offer.ProductCategory.IdProductCategory = Convert.ToInt64(otReader["IdProductCategory"]);
                                ot.Quotation.Offer.ProductCategory.Name = otReader["ProductSubCategory"].ToString();

                                if (otReader["IdProductCategory"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.ProductCategory.Category = new ProductCategory();
                                    if (otReader["IdCategory"] != DBNull.Value)
                                        ot.Quotation.Offer.ProductCategory.Category.IdProductCategory = Convert.ToInt64(otReader["IdCategory"].ToString());

                                    ot.Quotation.Offer.ProductCategory.Category.Name = otReader["ProductCategory"].ToString();
                                }
                            }


                            Shipment shipment = new Shipment();
                            if (otReader["IdShipment"] != DBNull.Value)
                            {
                                shipment.IdShipment = Convert.ToInt64(otReader["IdShipment"].ToString());
                                shipment.EmdepCode = otReader["ShipmentEmdepCode"].ToString();
                                shipment.DeliveryDate = Convert.ToDateTime(otReader["ShipmentDeliveryDate"].ToString());
                                shipment.ShipmentNumber = otReader["ShipmentNumber"].ToString();
                            }
                            if (otReader["ShippingDate"] != DBNull.Value)
                            {
                                shipment.ShippingDate = Convert.ToDateTime(otReader["ShippingDate"].ToString());
                            }

                            if (otReader["CreatedIn"] != DBNull.Value)
                            {
                                shipment.CreatedIn = Convert.ToDateTime(otReader["CreatedIn"].ToString());
                            }

                            ot.FirstShipment = shipment;

                            if (otReader["TimeEstimation"] != DBNull.Value)
                            {
                                ot.TimeEstimationInHours = Convert.ToSingle(otReader["TimeEstimation"].ToString());
                            }

                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetSAMWorkOrdersReport_V2260() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
                }

                if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow otReader in dataSet.Tables[1].Rows)
                    {
                        try
                        {
                            OTWorkingTime otworkingtime = new OTWorkingTime();

                            if (otReader["IdOTWorkingTime"] != DBNull.Value)
                                otworkingtime.IdOTWorkingTime = Convert.ToInt64(otReader["IdOTWorkingTime"]);

                            if (otReader["IdOT"] != DBNull.Value)
                                otworkingtime.IdOT = Convert.ToInt64(otReader["IdOT"]);

                            if (otReader["IdStage"] != DBNull.Value)
                                otworkingtime.IdStage = Convert.ToByte(otReader["IdStage"]);

                            if (otReader["IdOperator"] != DBNull.Value)
                                otworkingtime.IdOperator = Convert.ToInt32(otReader["IdOperator"]);

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
                            listLoggedHoursForOT_User_Date.Add(otworkingtime);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetSAMWorkOrdersReport_V2260() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in listLoggedHoursForOT_User_Date.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
                            otupdate.RealEndDate = item.ToList().Max(i => i.EndTime);
                            TimeSpan worklogTotalTime = new TimeSpan(item.ToList().Sum(r => r.TotalTime.Ticks));
                            otupdate.RealDuration = worklogTotalTime.TotalHours.ToString("#.##");
                            otupdate.RealDurationDouble = Math.Round(worklogTotalTime.TotalHours, 2);

                        }
                    }
                }

                if (dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow otReader in dataSet.Tables[2].Rows)
                    {
                        try
                        {
                            if (otReader["idot"] != DBNull.Value)
                            {
                                if (ots.Any(ot => ot.IdOT == Convert.ToInt64(otReader["idot"])))
                                {

                                    ots.Where(ot => ot.IdOT == Convert.ToInt64(otReader["idot"])).ToList().ForEach(x => x.Modules = Convert.ToInt64(otReader["Modules"]));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetSAMWorkOrdersReport_V2260() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetSAMWorkOrdersReport_V2260().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        public byte[] GetCountryIconFileInBytes(string countryiso, string filepath)
        {
            byte[] bytes = null;
            try
            {
                string filenamepng = countryiso + ".png";
                string filenamejpg = countryiso + ".jpg";
                string filepathjpg = Path.Combine(filepath, filenamejpg);
                string filepathpng = Path.Combine(filepath, filenamepng);
                if (File.Exists(filepathjpg))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathjpg, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                else if (File.Exists(filepathpng))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathpng, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountryIconFileInBytes() ISO-{0}. ErrorMessage- {1}", countryiso, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            catch (DirectoryNotFoundException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountryIconFileInBytes() ISO-{0}. ErrorMessage- {1}", countryiso, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCountryIconFileInBytes() ISO-{0}. ErrorMessage- {1}", countryiso, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[GEOS2-3417]
        public List<Ots> GetPendingWorkorders_V2270(Company company, string userProfileImageFilePath, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();

            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2270", connOts);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
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

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2270() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2270() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2270() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2270() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2270().", category: Category.Info, priority: Priority.Low);
            return ots;
        }


        //[GEOS2-3585]
        public List<Ots> GetPendingWorkorders_V2280(Company company, string userProfileImageFilePath, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();

            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2280", connOts);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
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

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2280() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2280() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2280() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2280() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2280().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowStatus> GetAllWorkflowStatusForQuality(string connectionString)
        {

            List<WorkflowStatus> workflowStatusList = new List<WorkflowStatus>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetAllWorkflowStatusForQuality", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            WorkflowStatus workflowStatus = new WorkflowStatus();

                            workflowStatus.IdWorkflowStatus = Convert.ToByte(mySqlDataReader["IdWorkflowStatus"]);

                            if (mySqlDataReader["IdWorkflow"] != DBNull.Value)
                                workflowStatus.IdWorkflow = Convert.ToUInt32(mySqlDataReader["IdWorkflow"].ToString());

                            if (mySqlDataReader["Name"] != DBNull.Value)
                                workflowStatus.Name = mySqlDataReader["Name"].ToString();

                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                                workflowStatus.HtmlColor = mySqlDataReader["HtmlColor"].ToString();

                            if (mySqlDataReader["IdWorkflowScope"] != DBNull.Value)
                                workflowStatus.IdWorkflowScope = Convert.ToUInt32(mySqlDataReader["IdWorkflowScope"].ToString());

                            workflowStatusList.Add(workflowStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllWorkflowStatusForQuality(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return workflowStatusList;
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowTransition> GetAllWorkflowTransitionsForQuality(string connectionString)
        {

            List<WorkflowTransition> workflowTransitionList = new List<WorkflowTransition>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_Get_workflow_transitionsForQuality", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            WorkflowTransition workflowTransition = new WorkflowTransition();

                            workflowTransition.IdWorkflowTransition = Convert.ToUInt32(mySqlDataReader["IdWorkflowTransition"]);

                            if (mySqlDataReader["IdWorkflow"] != DBNull.Value)
                                workflowTransition.IdWorkflow = Convert.ToUInt32(mySqlDataReader["IdWorkflow"]);

                            if (mySqlDataReader["Name"] != DBNull.Value)
                                workflowTransition.Name = mySqlDataReader["Name"].ToString();

                            if (mySqlDataReader["IdWorkflowStatusFrom"] != DBNull.Value)
                                workflowTransition.IdWorkflowStatusFrom = Convert.ToByte(mySqlDataReader["IdWorkflowStatusFrom"]);

                            if (mySqlDataReader["IdWorkflowStatusTo"] != DBNull.Value)
                                workflowTransition.IdWorkflowStatusTo = Convert.ToByte(mySqlDataReader["IdWorkflowStatusTo"]);

                            if (mySqlDataReader["IsCommentRequired"] != DBNull.Value)
                                workflowTransition.IsCommentRequired = Convert.ToByte(mySqlDataReader["IsCommentRequired"]);

                            if (mySqlDataReader["IdWorkflowScope"] != DBNull.Value)
                                workflowTransition.IdWorkflowScope = Convert.ToUInt32(mySqlDataReader["IdWorkflowScope"]);

                            workflowTransitionList.Add(workflowTransition);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllWorkflowTransitionsForQuality(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return workflowTransitionList;
        }
        public bool ExistsDirectory(string path)
        {
            return Directory.Exists(path);

        }
        public void CreateDirectory(string path)
        {

            Directory.CreateDirectory(path);


        }
        public bool UpdateOTQuality(Company company, Int64 IdOT, int IdUser, string otAttachment, string GUIDString, string year, string quotationCode, string filePath, List<OTAttachment> lstAttachmentDeleted, string OldRemark, string Remark)
        {
            bool isUpdated = false;
            string connectionString = company.ConnectPlantConstr;

            TransactionScope transactionScope = null;
            try
            {
                using (transactionScope = new TransactionScope())
                {
                    string[] files = null;

                    if (!string.IsNullOrEmpty(GUIDString))
                    {
                        string additionalInfoPath = Path.Combine(filePath, year, quotationCode, "Additional Information");

                        if (!Directory.Exists(additionalInfoPath))
                        {
                            CreateDirectory(additionalInfoPath);
                        }

                        string tempFolder = Path.Combine(additionalInfoPath, "_tmp", GUIDString.ToString());
                        string zipFilePath = Path.Combine(additionalInfoPath, "_tmp", GUIDString.ToString() + ".Zip");

                        // Extract zip file
                        ZipFile.ExtractToDirectory(zipFilePath, tempFolder);

                        // Delete Zip Folder
                        File.Delete(zipFilePath);

                        // Get all files in Extract folder in _tmp
                        files = Directory.GetFiles(tempFolder);
                    }

                    UpdateOTAttachment(connectionString, IdOT, otAttachment, IdUser);

                    if (files != null)
                    {
                        string destinationFolder = Path.Combine(filePath, year, quotationCode, "Additional Information");

                        foreach (string sourceFile in files)
                        {
                            string fileName = Path.GetFileName(sourceFile);

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                string destinationFile = Path.Combine(destinationFolder, fileName);

                                // This will automatically replace if file already exists
                                System.IO.File.Copy(sourceFile, destinationFile, true);

                                // Delete the temporary file
                                File.Delete(sourceFile);
                            }
                        }

                        // Clean up the temporary directory
                        string tempExtractFolder = Path.Combine(filePath, year, quotationCode, "Additional Information", "_tmp", GUIDString.ToString());
                        if (Directory.Exists(tempExtractFolder))
                        {
                            Directory.Delete(tempExtractFolder, true);
                        }
                    }

                    if (lstAttachmentDeleted != null && lstAttachmentDeleted.Count > 0)
                    {
                        string additionalInfoPath = Path.Combine(filePath, year, quotationCode, "Additional Information");

                        foreach (OTAttachment item in lstAttachmentDeleted)
                        {
                            string fileToDelete = Path.Combine(additionalInfoPath, item.FileName);

                            if (File.Exists(fileToDelete))
                            {
                                File.Delete(fileToDelete);
                            }
                        }
                    }

                    if (OldRemark != Remark)
                        UpdateOTSObservation(connectionString, IdOT, Remark, IdUser);

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
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOTQuality(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }
        public List<OTAttachment> GetQualityOTAttachment(Company company, Int64 idOT, string workOrderPath)
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
                                    otAttachment.OriginalFileName = item;
                                    otAttachment.SavedFileName = item;
                                    otAttachment.FileUploadName = item;


                                    string[] stringExtension = otAttachment.FileName.Split('.');
                                    if (stringExtension.Count() > 0)
                                    {
                                        if (!string.IsNullOrEmpty(stringExtension[1]))
                                        {
                                            otAttachment.FileExtension = "." + stringExtension[1];
                                            otAttachment.FileType = otAttachment.FileExtension;
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
                        otAttachment.FileSizeInInt = fi.Length;
                        //string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
                        //if (fi.Length == 0)
                        //    otAttachment.FileSize = "0" + suf[0];
                        //long bytes = Math.Abs(fi.Length);
                        //int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                        //double num = Math.Round(bytes / Math.Pow(1024, place));
                        //otAttachment.FileSize = (Math.Sign(fi.Length) * num).ToString() + " " + suf[place];
                    }
                }
            }

            return otAttachments;
        }

        public bool UpdateWorkflowStatusInOTQC(string MainServerConnectionString, UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            bool status;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTS_UpdateWorkflowStatus_QC", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                        mySqlCommand.Parameters.AddWithValue("_IdWorkflowStatus", IdWorkflowStatus);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        status = true;
                    }
                    AddCommentsOrLogEntriesByOT(MainServerConnectionString, LogEntriesByOTList);
                    UpdateInOT(MainServerConnectionString, IdOT, IdUser);
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateWorkflowStatusInOTQC(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return status;
        }


        /// <summary>
        /// This method is to get workorder details
        /// </summary>
        /// [cpatil][18-12-2019][GEOS2-1760]Open Work Order details when double-click in Order
        /// [cpatil][24-07-2021][GEOS2-2902]
        /// [adhatkar][GEOS2-2961][01-09-2021]
        /// [cpatil][GEOS2-3586][11-06-2022]
        /// <param name="idOt">Get id ot</param>
        /// <param name="company">Get company details</param>
        /// <returns>Get ots details</returns>
        public Ots GetWorkOrderByIdOtForQC(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2180", connOT);
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
                                    ot.Quotation.Site.Alias = Convert.ToString(otReader["ShortName"]);
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }

                                if (otReader["Observations"] != DBNull.Value)
                                    ot.Observations = Convert.ToString(otReader["Observations"]);

                            }
                            ot.OtItems = GetOtItemsByIdOt_V2170(connectionString, idOt);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetWorkOrderByIdOt_V2180(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetWorkOrderByIdOt_V2180().", category: Category.Info, priority: Priority.Low);
            return ot;
        }


        //[GEOS2-3915]
        public List<Ots> GetPendingWorkorders_V2301(Company company, string userProfileImageFilePath, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();

            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2301", connOts);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);

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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
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

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2301() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2280() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2280() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2280() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2301().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        //[pjadhav][GEOS2-3681][12/09/2022]
        public List<OTs> GetAllOrderItemsList(string PCMConnectionString, Company company)
        {
            List<OTs> ots = new List<OTs>();
            PCMConnectionString = company.ConnectPlantConstr;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    DataSet dataSet = new DataSet();
                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetSAMOrderItems_V2340", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTs ot = new OTs();

                            ot.IdOT = Convert.ToUInt32(reader["IdOT"]);
                            ot.ValidateItem = new ValidateItem();
                            ot.WOItem = new WOItem();
                            ot.Country = new Country();
                            ot.Article = new Article();
                            if (reader["IdWorkflowStatus"] != DBNull.Value)
                            {
                                ot.IdWorkflowStatus = Convert.ToByte(reader["IdWorkflowStatus"]);
                                ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(reader["IdWorkflowStatus"]);
                                ot.WorkflowStatus.Name = Convert.ToString(reader["WorkFlowStatus"]);
                                ot.WorkflowStatus.HtmlColor = Convert.ToString(reader["WorkFlowStatusHtmlColor"]);

                            }
                            if (reader["IdTemplate"] != DBNull.Value)
                            {
                                if (ot.Quotation == null)
                                {
                                    ot.Quotation = new Quotation();
                                }
                                ot.Quotation.IdDetectionsTemplate = Convert.ToByte(reader["IdTemplate"]);
                                ot.Quotation.Template = new Template();
                                ot.Quotation.Template.IdTemplate = ot.Quotation.IdDetectionsTemplate;
                                ot.Quotation.Template.Name = Convert.ToString(reader["Template"]);
                                ot.Quotation.Template.IdTemplate = Convert.ToByte(reader["TemplateType"]);
                            }
                            if (reader["PartNumber"] != DBNull.Value)
                            {

                                ot.ValidateItem.PartNumberCode = Convert.ToString(reader["PartNumber"]);
                            }

                            if (reader["WorkOrder"] != DBNull.Value)
                            {

                                ot.WOItem.WorkOrder = Convert.ToString(reader["WorkOrder"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {

                                ot.WOItem.OriginalQty = Convert.ToInt64(reader["Quantity"]);
                            }
                            if (reader["CustomerName"] != DBNull.Value)
                            {
                                // ot.ValidateItem = new ValidateItem();
                                ot.ValidateItem.Customer = Convert.ToString(reader["CustomerName"]);
                            }
                            if (reader["CountryName"] != DBNull.Value)
                            {
                                ot.Country.Name = Convert.ToString(reader["CountryName"]);
                            }
                            if (reader["IdRevisionItem"] != DBNull.Value)
                            {

                                if (reader["NumItem"] != DBNull.Value)
                                {
                                    ot.NumItem = Convert.ToString(reader["NumItem"]);
                                }
                                if (reader["Quantity"] != DBNull.Value)
                                {
                                    ot.Quantity = Convert.ToInt32(reader["Quantity"]);
                                }
                            }
                            if (reader["IdArticle"] != DBNull.Value)
                            {
                                ot.Article.IdArticle = Convert.ToInt32(reader["IdArticle"]);
                            }
                            if (reader["Reference"] != DBNull.Value)
                            {
                                ot.Article.Reference = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                ot.Article.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {
                                ot.Quantity = Convert.ToInt32(reader["Quantity"]);
                            }
                            if (reader["ExpectedDeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(reader["ExpectedDeliveryDate"]);

                            }
                            if (reader["DeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                                ot.Delay = (int)(ot.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;

                            }
                            if (reader["DeliveryDate"] != DBNull.Value)
                            {
                                CultureInfo myCI = CultureInfo.CurrentCulture;
                                Calendar myCal = myCI.Calendar;
                                CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                                DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                                int Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(ot.DeliveryDate), myCWR, myFirstDOW);
                                ot.ExpectedDeliveryWeek = Convert.ToString(ot.DeliveryDate.Value.Year + "CW" + Week);
                            }
                            try
                            {
                                if (reader["IdArticle"] != DBNull.Value)
                                {
                                    ot.IdArticle = Convert.ToInt32(reader["IdArticle"]);
                                }
                                if (reader["IdRevisionItem"] != DBNull.Value)
                                {
                                    ot.IdRevisionItem = Convert.ToInt32(reader["IdRevisionItem"]);
                                }
                                //ot.OtItems = GetSAMOrderItemsByIdOt_V2340(PCMConnectionString, ot.IdOT);
                            }
                            catch (Exception ex)
                            {
                            }
                            ots.Add(ot);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOrderItemsList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ots;
        }
        #region GEOS2-3682
        //Shubham[skadam] GEOS2-3682 Implement in SAM the items in the WO that must be passed in Bancos Stage (3/9) 09 12 2022
        List<SAMLogEntries> LogEntriesByOT = new List<SAMLogEntries>();
        List<SAMLogEntries> CommentsList = new List<SAMLogEntries>();
        public OTs GetSAMOrderItemsInformationByIdOt_V2340(Int64 idOt, Company company, UInt32 IdArticle, string PCMConnectionString)
        {
            string connectionString = company.ConnectPlantConstr;
            OTs ot = null;
            try
            {
                using (MySqlConnection connOT = new MySqlConnection(connectionString))
                {
                    connOT.Open();

                    MySqlCommand otCommand = new MySqlCommand("SAM_GetSAMOrderItemsInformationByIdOt_V2340", connOT);
                    otCommand.CommandType = CommandType.StoredProcedure;
                    otCommand.Parameters.AddWithValue("_IdOt", idOt);
                    otCommand.CommandTimeout = 3000;
                    using (MySqlDataReader otReader = otCommand.ExecuteReader())
                    {
                        if (otReader.Read())
                        {
                            try
                            {
                                ot = new OTs();
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

                                    if (otReader["ProducerIdCountryGroup"] != DBNull.Value)
                                    {
                                        ot.ProducerIdCountryGroup = Convert.ToInt64(otReader["ProducerIdCountryGroup"]);
                                        ot.CountryGroup = new CountryGroup();
                                        ot.CountryGroup.IdCountryGroup = Convert.ToInt64(otReader["ProducerIdCountryGroup"]);
                                        ot.CountryGroup.Name = Convert.ToString(otReader["ProducerCountryGroup"]);
                                        ot.CountryGroup.HtmlColor = Convert.ToString(otReader["ProducerCountryGroupColor"]);
                                    }

                                    if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                    {
                                        ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                        ot.WorkflowStatus = new WorkflowStatus();
                                        ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                        ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                        ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                    }

                                    if (otReader["Observations"] != DBNull.Value)
                                        ot.Observations = Convert.ToString(otReader["Observations"]);

                                }
                                ot.OtItems = GetSAMOrderItemsByIdOt_V2340(connectionString, idOt);
                                if (ot.ArticleDecompostionList == null)
                                {
                                    ot.ArticleDecompostionList = new List<ArticleDecomposition>();
                                }
                                // PCMManager PCMManager = new PCMManager();
                                //ot.ArticleDecompostionList = PCMManager.GetArticleDeCompostionByIdArticle(PCMConnectionString, IdArticle);
                                WarehouseManager WarehouseManager = new WarehouseManager();
                                ot.ArticleDecompostionList = WarehouseManager.GetArticleDeCompostionByIdArticle(connectionString, Convert.ToInt32(IdArticle));
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetSAMOrderItemsInformationByIdOt(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetSAMOrderItemsInformationByIdOt(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            if (ot.OtLogEntries == null)
            {
                ot.OtLogEntries = new List<SAMLogEntries>();
            }
            ot.OtLogEntries.AddRange(LogEntriesByOT);
            if (ot.OtComments == null)
            {
                ot.OtComments = new List<SAMLogEntries>();
            }
            ot.OtComments.AddRange(CommentsList);
            return ot;
        }
        public List<OtItem> GetSAMOrderItemsByIdOt_V2340(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetSAMOrderItemsByIdOt_V2340", connOtItem);
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
                                    article.IsOrientativeVisualAid = Convert.ToByte(otItemReader["IsOrientativeVisualAid"]);

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
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetSAMOrderItemsByIdOt_V2340(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    #region LogEntries
                    if (otItemReader.NextResult())
                    {
                        while (otItemReader.Read())
                        {

                            if (otItemReader["IdRevisionItem"] != DBNull.Value)
                            {
                                OtItem otItem = otItems.Where(w => w.IdRevisionItem == Convert.ToInt64(otItemReader["IdRevisionItem"])).FirstOrDefault();
                                if (otItem != null)
                                {
                                    if (otItem.LogEntriesByOT == null)
                                    {
                                        otItem.LogEntriesByOT = new List<SAMLogEntries>();
                                    }
                                    SAMLogEntries LogEntries = new SAMLogEntries();
                                    if (otItemReader["IdRevisionItem"] != DBNull.Value)
                                    {
                                        LogEntries.IdRevisionItem = Convert.ToInt64(otItemReader["IdRevisionItem"]);
                                    }
                                    if (otItemReader["IdUser"] != DBNull.Value)
                                    {
                                        LogEntries.IdUser = Convert.ToInt32(otItemReader["IdUser"]);
                                    }
                                    if (otItemReader["Text"] != DBNull.Value)
                                    {
                                        LogEntries.Comments = Convert.ToString(otItemReader["Text"]);
                                    }
                                    if (otItemReader["CreationDate"] != DBNull.Value)
                                    {
                                        LogEntries.Datetime = Convert.ToDateTime(otItemReader["CreationDate"]);
                                    }

                                    if (LogEntries.People == null)
                                    {
                                        LogEntries.People = new People();
                                    }
                                    if (otItemReader["IdUser"] != DBNull.Value)
                                    {
                                        LogEntries.People.IdPerson = Convert.ToInt32(otItemReader["IdUser"]);
                                    }
                                    if (otItemReader["Name"] != DBNull.Value)
                                    {
                                        LogEntries.People.Name = Convert.ToString(otItemReader["Name"]);
                                    }
                                    if (otItemReader["Surname"] != DBNull.Value)
                                    {
                                        LogEntries.People.Surname = Convert.ToString(otItemReader["Surname"]);
                                    }
                                    if (otItemReader["IdUser"] != DBNull.Value)
                                    {
                                        LogEntries.IdPerson = Convert.ToInt32(otItemReader["IdUser"]);
                                    }
                                    if (otItemReader["Name"] != DBNull.Value)
                                    {
                                        LogEntries.Name = Convert.ToString(otItemReader["Name"]);
                                    }
                                    if (otItemReader["Surname"] != DBNull.Value)
                                    {
                                        LogEntries.Surname = Convert.ToString(otItemReader["Surname"]);
                                    }
                                    otItem.LogEntriesByOT.Add(LogEntries);
                                    LogEntriesByOT.Add(LogEntries);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Comments
                    if (otItemReader.NextResult())
                    {
                        while (otItemReader.Read())
                        {

                            if (otItemReader["IdRevisionItem"] != DBNull.Value)
                            {
                                OtItem otItem = otItems.Where(w => w.IdRevisionItem == Convert.ToInt64(otItemReader["IdRevisionItem"])).FirstOrDefault();
                                if (otItem != null)
                                {
                                    if (otItem.Comments == null)
                                    {
                                        otItem.Comments = new List<SAMLogEntries>();
                                    }
                                    SAMLogEntries Comments = new SAMLogEntries();
                                    if (otItemReader["IdRevisionItem"] != DBNull.Value)
                                    {
                                        Comments.IdRevisionItem = Convert.ToInt64(otItemReader["IdRevisionItem"]);
                                    }
                                    if (otItemReader["IdComment"] != DBNull.Value)
                                    {
                                        Comments.IdComment = Convert.ToInt32(otItemReader["IdComment"]);
                                    }
                                    if (otItemReader["IdUser"] != DBNull.Value)
                                    {
                                        Comments.IdUser = Convert.ToInt32(otItemReader["IdUser"]);
                                    }
                                    if (otItemReader["Text"] != DBNull.Value)
                                    {
                                        Comments.Comments = Convert.ToString(otItemReader["Text"]);
                                    }
                                    if (otItemReader["CreationDate"] != DBNull.Value)
                                    {
                                        Comments.Datetime = Convert.ToDateTime(otItemReader["CreationDate"]);
                                    }

                                    if (Comments.People == null)
                                    {
                                        Comments.People = new People();
                                    }
                                    if (otItemReader["IdUser"] != DBNull.Value)
                                    {
                                        Comments.People.IdPerson = Convert.ToInt32(otItemReader["IdUser"]);
                                    }
                                    if (otItemReader["Name"] != DBNull.Value)
                                    {
                                        Comments.People.Name = Convert.ToString(otItemReader["Name"]);
                                    }
                                    if (otItemReader["Surname"] != DBNull.Value)
                                    {
                                        Comments.People.Surname = Convert.ToString(otItemReader["Surname"]);
                                    }
                                    if (otItemReader["IdUser"] != DBNull.Value)
                                    {
                                        Comments.IdPerson = Convert.ToInt32(otItemReader["IdUser"]);
                                    }
                                    if (otItemReader["Name"] != DBNull.Value)
                                    {
                                        Comments.Name = Convert.ToString(otItemReader["Name"]);
                                    }
                                    if (otItemReader["Surname"] != DBNull.Value)
                                    {
                                        Comments.Surname = Convert.ToString(otItemReader["Surname"]);
                                    }
                                    Comments.TransactionOperation = ModelBase.TransactionOperations.Modify;
                                    otItem.Comments.Add(Comments);
                                    CommentsList.Add(Comments);
                                }
                            }
                        }
                    }

                    #endregion

                }
            }
            return otItems;
        }

        public List<Common.PCM.PCMArticleImage> GetPCMArticleImagesByIdPCMArticle(string PCMConnectionString, string articleVisualAidsPath, Int32 IdArticle, string ImagePath, string ArticleReference)
        {
            List<Common.PCM.PCMArticleImage> pCMArticleImages = new List<Common.PCM.PCMArticleImage>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetArticleImagesByIdPCMArticle_V2340", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Common.PCM.PCMArticleImage image = new Common.PCM.PCMArticleImage();

                            if (reader["IdPCMArticleImage"] != DBNull.Value)
                                image.IdPCMArticleImage = Convert.ToUInt32(reader["IdPCMArticleImage"]);
                            if (reader["IdArticle"] != DBNull.Value)
                                image.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                image.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                image.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                image.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                image.Position = Convert.ToUInt64(reader["Position"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                image.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            //image.PCMArticleImageInBytes = GetPCMArticleImage(Convert.ToString(image.IdPCMArticleImage), ImagePath, image.SavedFileName, ArticleReference);
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                string articlesImagePath = Convert.ToString(reader["ImagePath"]);

                                if (!string.IsNullOrEmpty(articlesImagePath))
                                    image.PCMArticleImageInBytes = GetArticleImageInBytes(articleVisualAidsPath, articlesImagePath);
                            }
                            pCMArticleImages.Add(image);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleImagesByIdPCMArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pCMArticleImages;
        }
        public byte[] GetPCMArticleImage(string IdPCMArticleImage, string ImagePath, string SavedFileName, string ArticleReference)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}\{2}", ImagePath, ArticleReference, IdPCMArticleImage + "_" + SavedFileName);

            try
            {
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
                }

                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleImage(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                return bytes;
            }
        }
        public byte[] GetArticleImageInBytes(string ArticleVisualAidsPath, string ImagePath)
        {
            if (!Directory.Exists(ArticleVisualAidsPath))
            {
                return null;
            }

            string fileUploadPath = ArticleVisualAidsPath + ImagePath;

            if (!File.Exists(fileUploadPath))
            {
                return null;
            }

            if (ImagePath != null && !string.IsNullOrEmpty(ImagePath))
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
                    Log4NetLogger.Logger.Log(string.Format("Error GetArticleImageInBytes() article ImagePath-{0}. ErrorMessage- {1}", ImagePath, ex.Message), category: Category.Exception, priority: Priority.Low);
                    //throw;
                }
            }

            return null;
        }
        #endregion


        public OtItemsComment AddObservationCommentItem(OtItemsComment Items, Company company, string mainServerConnectionString)
        {

            string connectionString = company.ConnectPlantConstr;
            if (Items != null)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        try
                        {
                            MySqlCommand conCommand = new MySqlCommand("SRM_AddItemComment", conn);
                            conCommand.CommandType = CommandType.StoredProcedure;
                            //  conCommand.Parameters.AddWithValue("_IdItemComment", idCommentItem);
                            //conCommand.Parameters.AddWithValue("_IdComment", Items.IdComment);
                            conCommand.Parameters.AddWithValue("_Comments", Items.Comments);
                            conCommand.Parameters.AddWithValue("_CreationDate", Items.CommentDate);
                            conCommand.Parameters.AddWithValue("_IdUser", Items.IdUser);
                            conCommand.Parameters.AddWithValue("_IdItemType", Items.IdEntryType);
                            conCommand.Parameters.AddWithValue("_IdRevisionItem", Items.Idrevisionitem);

                            conCommand.ExecuteNonQuery();
                            //if (Items.IdComment > 0)
                            //{
                            //    DeleteComment_V2340(Items.IdComment, Items.CommentListDelete, mainServerConnectionString);
                            //   // transactionScope.Complete();
                            //}
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }

                }

                catch (Exception ex)
                {
                    throw;
                }
            }
            return Items;
        }

        //public List<SAMLogEntries> DeleteComment_V2340(Int64 idCommentItem, List<SAMLogEntries> CommentItems, string mainServerConnectionString)
        //{
        //    //bool isactionPerformed = false;
        //    //if (logEntriesByActionItems == null || logEntriesByActionItems.Count == 0)
        //    //{
        //    //    isactionPerformed = true;
        //    //}
        //    if (CommentItems != null)
        //    {
        //        TransactionScope transactionScope = null;
        //        try
        //        {

        //            foreach (SAMLogEntries item in CommentItems)
        //            {
        //                if (item.TransactionOperation == ModelBase.TransactionOperations.Delete)
        //                {
        //                    using (MySqlConnection conn = new MySqlConnection(mainServerConnectionString))
        //                    {
        //                        conn.Open();

        //                        try
        //                        {

        //                            MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTCommentDelete", conn);
        //                            mySqlCommand.CommandType = CommandType.StoredProcedure;
        //                            mySqlCommand.Parameters.AddWithValue("_IdComment", idCommentItem);
        //                            mySqlCommand.ExecuteNonQuery();

        //                            mySqlCommand.ExecuteNonQuery();

        //                            //if (item.IdLogEntriesByActionItem > 0)
        //                            //{
        //                            //    isactionPerformed = true;
        //                            //}

        //                            conn.Close();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            throw;
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }

        //    }
        //    return CommentItems;
        //}
        public bool DeleteComment_V2340(Int64 idComment, Company company)
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

                        //foreach (SAMLogEntries Item in ItemsComment)
                        //{
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTCommentDelete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdComment", idComment);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        //}
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

        //[pjadhav][GEOS2-3686][12/26/2022]
        public List<OTWorkingTime> GetOTWorkingTimeDetails_V2350(Int64 otItems, Company company, String userProfileImageFilePath)
        {
            List<OTWorkingTime> LstOTWorkingTime = new List<OTWorkingTime>();
            List<UserShortDetail> UserShortDetailForbytes = new List<UserShortDetail>();
            string connectionString = company.ConnectPlantConstr;
            using (MySqlConnection connWarehouse = new MySqlConnection(connectionString))
            {
                connWarehouse.Open();

                MySqlCommand warehouseCommand = new MySqlCommand("SAM_GetOTWorkingTimeDetails_V2350", connWarehouse);
                warehouseCommand.CommandType = CommandType.StoredProcedure;
                warehouseCommand.Parameters.AddWithValue("_IdOtItem", otItems);

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

                            if (warehouseReader["IdOTItem"] != DBNull.Value)
                                otWorkingTime.IdOTItem = Convert.ToInt64(warehouseReader["IdOTItem"]);

                            if (warehouseReader["IdStage"] != DBNull.Value)
                                otWorkingTime.IdStage = Convert.ToByte(warehouseReader["IdStage"]);
							//[nsatpute][01-07-2025][GEOS2-8783]
                            if (warehouseReader["StartDate"] != DBNull.Value)
                                otWorkingTime.StartTime = Convert.ToDateTime(Convert.ToDateTime(warehouseReader["StartDate"]));

                            //if (warehouseReader["StartTime"] != DBNull.Value)
                            //    otWorkingTime.StartTimeInHoursAndMinutes = warehouseReader["StartTime"].ToString();
							//[nsatpute][01-07-2025][GEOS2-8783]
                            if (warehouseReader["EndDate"] != DBNull.Value)
                                otWorkingTime.EndTime = Convert.ToDateTime(Convert.ToDateTime(warehouseReader["EndDate"]));

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
                            otWorkingTime.TransactionOperation = ModelBase.TransactionOperations.Modify;

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

        public OTWorkingTime AddOtWorkingTimeWorkLogItem(Int64 IdOT, Int64 idOTItems, OTWorkingTime Items, Company company, string mainServerConnectionString)
        {

            string connectionString = company.ConnectPlantConstr;
            if (Items != null)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        try
                        {
                            MySqlCommand conCommand = new MySqlCommand("SAM_AddWorkLogItems", conn);
                            conCommand.CommandType = CommandType.StoredProcedure;
                            conCommand.Parameters.AddWithValue("_IdOT", IdOT);
                            conCommand.Parameters.AddWithValue("_StartTime", Items.StartTime);
                            conCommand.Parameters.AddWithValue("_EndTime", Items.EndTime);
                            conCommand.Parameters.AddWithValue("_IdUser", Items.IdOperator);
                            conCommand.Parameters.AddWithValue("_IdOtItem", idOTItems);
                            conCommand.ExecuteNonQuery();
                            //if (Items.IdComment > 0)
                            //{
                            //    DeleteComment_V2340(Items.IdComment, Items.CommentListDelete, mainServerConnectionString);
                            //   // transactionScope.Complete();
                            //}
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }

                }

                catch (Exception ex)
                {
                    throw;
                }
            }
            return Items;
        }

        public List<OTs> GetAllOrderItemsList_V2350(string PCMConnectionString, Company company)
        {
            List<OTs> ots = new List<OTs>();
            PCMConnectionString = company.ConnectPlantConstr;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    DataSet dataSet = new DataSet();
                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetSAMOrderItems_V2350", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTs ot = new OTs();

                            ot.IdOT = Convert.ToUInt32(reader["IdOT"]);
                            ot.ValidateItem = new ValidateItem();
                            ot.WOItem = new WOItem();
                            ot.Country = new Country();
                            ot.Article = new Article();
                            if (reader["IdWorkflowStatus"] != DBNull.Value)
                            {
                                ot.IdWorkflowStatus = Convert.ToByte(reader["IdWorkflowStatus"]);
                                ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(reader["IdWorkflowStatus"]);
                                ot.WorkflowStatus.Name = Convert.ToString(reader["WorkFlowStatus"]);
                                ot.WorkflowStatus.HtmlColor = Convert.ToString(reader["WorkFlowStatusHtmlColor"]);

                            }
                            if (reader["IdTemplate"] != DBNull.Value)
                            {
                                if (ot.Quotation == null)
                                {
                                    ot.Quotation = new Quotation();
                                }
                                ot.Quotation.IdDetectionsTemplate = Convert.ToByte(reader["IdTemplate"]);
                                ot.Quotation.Template = new Template();
                                ot.Quotation.Template.IdTemplate = ot.Quotation.IdDetectionsTemplate;
                                ot.Quotation.Template.Name = Convert.ToString(reader["Template"]);
                                ot.Quotation.Template.IdTemplate = Convert.ToByte(reader["TemplateType"]);
                            }
                            if (reader["PartNumber"] != DBNull.Value)
                            {

                                ot.ValidateItem.PartNumberCode = Convert.ToString(reader["PartNumber"]);
                            }

                            if (reader["WorkOrder"] != DBNull.Value)
                            {

                                ot.WOItem.WorkOrder = Convert.ToString(reader["WorkOrder"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {

                                ot.WOItem.OriginalQty = Convert.ToInt64(reader["Quantity"]);
                            }
                            if (reader["CustomerName"] != DBNull.Value)
                            {
                                // ot.ValidateItem = new ValidateItem();
                                ot.ValidateItem.Customer = Convert.ToString(reader["CustomerName"]);
                            }
                            if (reader["CountryName"] != DBNull.Value)
                            {
                                ot.Country.Name = Convert.ToString(reader["CountryName"]);
                            }
                            if (reader["IdRevisionItem"] != DBNull.Value)
                            {

                                if (reader["NumItem"] != DBNull.Value)
                                {
                                    ot.NumItem = Convert.ToString(reader["NumItem"]);
                                }
                                if (reader["Quantity"] != DBNull.Value)
                                {
                                    ot.Quantity = Convert.ToInt32(reader["Quantity"]);
                                }
                            }
                            if (reader["IdArticle"] != DBNull.Value)
                            {
                                ot.Article.IdArticle = Convert.ToInt32(reader["IdArticle"]);
                            }
                            if (reader["Reference"] != DBNull.Value)
                            {
                                ot.Article.Reference = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                ot.Article.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {
                                ot.Quantity = Convert.ToInt32(reader["Quantity"]);
                            }
                            if (reader["ExpectedDeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(reader["ExpectedDeliveryDate"]);

                            }
                            if (reader["DeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                                ot.Delay = (int)(ot.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;

                            }
                            if (reader["DeliveryDate"] != DBNull.Value)
                            {
                                CultureInfo myCI = CultureInfo.CurrentCulture;
                                Calendar myCal = myCI.Calendar;
                                CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                                DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                                int Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(ot.DeliveryDate), myCWR, myFirstDOW);
                                ot.ExpectedDeliveryWeek = Convert.ToString(ot.DeliveryDate.Value.Year + "CW" + Week);
                            }
                            try
                            {
                                if (reader["IdArticle"] != DBNull.Value)
                                {
                                    ot.IdArticle = Convert.ToInt32(reader["IdArticle"]);
                                }
                                if (reader["IdRevisionItem"] != DBNull.Value)
                                {
                                    ot.IdRevisionItem = Convert.ToInt32(reader["IdRevisionItem"]);
                                }
                                //ot.OtItems = GetSAMOrderItemsByIdOt_V2340(PCMConnectionString, ot.IdOT);
                            }
                            catch (Exception ex)
                            {
                            }
                            ots.Add(ot);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOrderItemsList_V2350(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ots;
        }

        #region V2430
        //[rdixit][28.08.2023][GEOS2-4754]
        public List<Ots> GetPendingWorkorders_V2430(Company company, string userProfileImageFilePath, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();

            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2430", connOts);
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
                            if (otReader["DeliveryWeek"] != DBNull.Value)
                            {
                                //CultureInfo myCI = CultureInfo.CurrentCulture;
                                //Calendar myCal = myCI.Calendar;
                                //CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                                //DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                                //int Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(ot.DeliveryDate), myCWR, myFirstDOW);
                                ot.DeliveryWeek = Convert.ToString(otReader["DeliveryWeek"]);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);
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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
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

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2430() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2430() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2430() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2430() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2430().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        //[rdixit][28.08.2023][GEOS2-4754]
        public List<OTs> GetAllOrderItemsList_V2430(string PCMConnectionString, Company company)
        {
            List<OTs> ots = new List<OTs>();
            PCMConnectionString = company.ConnectPlantConstr;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    DataSet dataSet = new DataSet();
                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetSAMOrderItems_V2430", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OTs ot = new OTs();

                            ot.IdOT = Convert.ToUInt32(reader["IdOT"]);
                            ot.ValidateItem = new ValidateItem();
                            ot.WOItem = new WOItem();
                            ot.Country = new Country();
                            ot.Article = new Article();
                            if (reader["IdWorkflowStatus"] != DBNull.Value)
                            {
                                ot.IdWorkflowStatus = Convert.ToByte(reader["IdWorkflowStatus"]);
                                ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(reader["IdWorkflowStatus"]);
                                ot.WorkflowStatus.Name = Convert.ToString(reader["WorkFlowStatus"]);
                                ot.WorkflowStatus.HtmlColor = Convert.ToString(reader["WorkFlowStatusHtmlColor"]);

                            }
                            if (reader["IdTemplate"] != DBNull.Value)
                            {
                                if (ot.Quotation == null)
                                {
                                    ot.Quotation = new Quotation();
                                }
                                ot.Quotation.IdDetectionsTemplate = Convert.ToByte(reader["IdTemplate"]);
                                ot.Quotation.Template = new Template();
                                ot.Quotation.Template.IdTemplate = ot.Quotation.IdDetectionsTemplate;
                                ot.Quotation.Template.Name = Convert.ToString(reader["Template"]);
                                ot.Quotation.Template.IdTemplate = Convert.ToByte(reader["TemplateType"]);
                            }
                            if (reader["PartNumber"] != DBNull.Value)
                            {

                                ot.ValidateItem.PartNumberCode = Convert.ToString(reader["PartNumber"]);
                            }

                            if (reader["WorkOrder"] != DBNull.Value)
                            {

                                ot.WOItem.WorkOrder = Convert.ToString(reader["WorkOrder"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {

                                ot.WOItem.OriginalQty = Convert.ToInt64(reader["Quantity"]);
                            }
                            if (reader["CustomerName"] != DBNull.Value)
                            {
                                // ot.ValidateItem = new ValidateItem();
                                ot.ValidateItem.Customer = Convert.ToString(reader["CustomerName"]);
                            }
                            if (reader["CountryName"] != DBNull.Value)
                            {
                                ot.Country.Name = Convert.ToString(reader["CountryName"]);
                            }
                            if (reader["IdRevisionItem"] != DBNull.Value)
                            {

                                if (reader["NumItem"] != DBNull.Value)
                                {
                                    ot.NumItem = Convert.ToString(reader["NumItem"]);
                                }
                                if (reader["Quantity"] != DBNull.Value)
                                {
                                    ot.Quantity = Convert.ToInt32(reader["Quantity"]);
                                }
                            }
                            if (reader["IdArticle"] != DBNull.Value)
                            {
                                ot.Article.IdArticle = Convert.ToInt32(reader["IdArticle"]);
                            }
                            if (reader["Reference"] != DBNull.Value)
                            {
                                ot.Article.Reference = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                ot.Article.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {
                                ot.Quantity = Convert.ToInt32(reader["Quantity"]);
                            }
                            if (reader["ExpectedDeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(reader["ExpectedDeliveryDate"]);

                            }
                            if (reader["DeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                                ot.Delay = (int)(ot.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;

                            }
                            //if (reader["DeliveryDate"] != DBNull.Value)
                            //{
                            //    CultureInfo myCI = CultureInfo.CurrentCulture;
                            //    Calendar myCal = myCI.Calendar;
                            //    CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                            //    DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                            //    int Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(ot.DeliveryDate), myCWR, myFirstDOW);
                            //    ot.ExpectedDeliveryWeek = Convert.ToString(ot.DeliveryDate.Value.Year + "CW" + Week);
                            //}
                            if (reader["ExpectedDeliveryWeek"] != DBNull.Value)
                            {
                                ot.ExpectedDeliveryWeek = Convert.ToString(reader["ExpectedDeliveryWeek"]);
                            }
                            try
                            {
                                if (reader["IdArticle"] != DBNull.Value)
                                {
                                    ot.IdArticle = Convert.ToInt32(reader["IdArticle"]);
                                }
                                if (reader["IdRevisionItem"] != DBNull.Value)
                                {
                                    ot.IdRevisionItem = Convert.ToInt32(reader["IdRevisionItem"]);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            ots.Add(ot);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOrderItemsList_V2430(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ots;
        }
        #endregion

        //[rdixit][12.03.2024][GEOS2-5361]
        public List<Ots> GetPendingWorkorders_V2500(Company company, string userProfileImageFilePath, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();

            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2500", connOts);
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
                            if (otReader["DeliveryWeek"] != DBNull.Value)
                            {
                                ot.DeliveryWeek = Convert.ToString(otReader["DeliveryWeek"]);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);
                                }

                                ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                if (otReader["IdOfferType"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);
                                    ot.Quotation.Offer.OfferType = new OfferType();
                                    ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                    ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);
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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
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
                                    }
                                }
                            }

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);

                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2500().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        /// <summary>
        /// This method is to get workorder details
        /// </summary>
        /// [rgadhave][18-06-2024][GEOS2-5583]
        /// <param name="idOt">Get id ot</param>
        /// <param name="company">Get company details</param>
        /// <returns>Get ots details</returns>
        public Ots GetStructureWorkOrderByIdOt_V2530(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2170", connOT);
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
                            ot.LstDetection = new List<Detection>();
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }



                            }
                            ot.OtItems = GetStructureOtItemsByIdOt_V2530(connectionString, idOt);

                            if (ot.OtItems != null && ot.OtItems.Count > 0)
                                ot.LstDetection = ot.OtItems.FirstOrDefault().LstDetection;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureWorkOrderByIdOt_V2170(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetStructureWorkOrderByIdOt_V2170().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        public List<OtItem> GetStructureOtItemsByIdOt_V2530(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();
            List<Detection> otDetectionLst = new List<Detection>();
            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetStructureOtItemsByIdOt_V2170", connOtItem);
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

                                otItem.RevisionItem.CPProduct = new CPProduct();
                                otItem.RevisionItem.CPProduct.Reference = Convert.ToString(otItemReader["Reference"]);

                                if (otItemReader["IdProduct"] != DBNull.Value)
                                    otItem.RevisionItem.IdProduct = Convert.ToInt64(otItemReader["IdProduct"]);

                                if (otItemReader["IdCPType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.IdCPTypeNew = Convert.ToInt64(otItemReader["IdCPType"]);

                                if (otItemReader["ProductType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.ProductTypeName = Convert.ToString(otItemReader["ProductType"]);

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
                            otItem.RevisionItem.CPProduct.LstCPDetection = new List<CPDetection>();
                            otItem.LstDetection = new List<Detection>();
                            otItems.Add(otItem);




                            keyId++;

                            //i = otItem.KeyId + 1;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    if (otItemReader.NextResult())
                        while (otItemReader.Read())
                        {
                            try
                            {
                                if (otItemReader["CPProductID"] != DBNull.Value)
                                {
                                    if (otItems.Any(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])))
                                    {
                                        List<OtItem> lstotitems = otItems.Where(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])).ToList();

                                        CPDetection cpdetection = new CPDetection();
                                        cpdetection.CPProductID = Convert.ToInt64(otItemReader["CPProductID"]);
                                        if (otItemReader["DetectionID"] != DBNull.Value)
                                            cpdetection.DetectionID = Convert.ToInt32(otItemReader["DetectionID"]);
                                        if (otItemReader["Name"] != DBNull.Value)
                                            cpdetection.DetectionName = Convert.ToString(otItemReader["Name"]);
                                        if (otItemReader["NumDetections"] != DBNull.Value)
                                            cpdetection.NumDetections = Convert.ToInt32(otItemReader["NumDetections"]);

                                        lstotitems.ForEach(x => { x.RevisionItem.CPProduct.LstCPDetection.Add(cpdetection); });
                                        if (!otDetectionLst.Any(i => i.IdDetection == cpdetection.DetectionID))
                                        {
                                            Detection detection = new Detection();
                                            detection.IdDetection = Convert.ToUInt32(cpdetection.DetectionID);
                                            detection.Name = Convert.ToString(cpdetection.DetectionName);
                                            otDetectionLst.Add(detection);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }


                }
            }
            otItems.ForEach(x => { x.LstDetection = otDetectionLst; });
            //Log4NetLogger.Logger.Log("Executed GetOtItemsByIdOt().", category: Category.Info, priority: Priority.Low);
            return otItems;
        }

        // [GEOS2-5472][nsatpute][04-07-2024]
        public Ots GetStructureWorkOrderByIdOt_V2540(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2170", connOT);
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
                            ot.LstDetection = new List<Detection>();
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }



                            }
                            //ot.OtItems = GetStructureOtItemsByIdOt_V2530(connectionString, idOt);
                            //[pramod.misal][GEOS2-5472][04.07.2024]
                            ot.OtItems = GetStructureOtItemsByIdOt_V2540(connectionString, idOt);

                            if (ot.OtItems != null && ot.OtItems.Count > 0)
                                ot.LstDetection = ot.OtItems.FirstOrDefault().LstDetection;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureWorkOrderByIdOt_V2170(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetStructureWorkOrderByIdOt_V2170().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        // [GEOS2-5472][nsatpute][04-07-2024]
        public List<OtItem> GetStructureOtItemsByIdOt_V2540(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();
            List<Detection> otDetectionLst = new List<Detection>();
            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetStructureOtItemsByIdOt_V2540", connOtItem);
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

                                otItem.RevisionItem.CPProduct = new CPProduct();
                                otItem.RevisionItem.CPProduct.Reference = Convert.ToString(otItemReader["Reference"]);

                                if (otItemReader["IdProduct"] != DBNull.Value)
                                    otItem.RevisionItem.IdProduct = Convert.ToInt64(otItemReader["IdProduct"]);

                                if (otItemReader["IdCPType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.IdCPTypeNew = Convert.ToInt64(otItemReader["IdCPType"]);

                                if (otItemReader["ProductType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.ProductTypeName = Convert.ToString(otItemReader["ProductType"]);

                                if (otItemReader["Iditemotstatus"] != DBNull.Value)
                                {
                                    otItem.IdItemOtStatus = Convert.ToByte(otItemReader["Iditemotstatus"]);

                                    otItem.Status = new ItemOTStatusType();
                                    otItem.Status.IdItemOtStatus = otItem.IdItemOtStatus;
                                    otItem.Status.Name = Convert.ToString(otItemReader["Status"]);
                                }
                                if (otItemReader["IdDrawing"] != DBNull.Value)
                                {
                                    otItem.RevisionItem.IdDrawing = Convert.ToInt64(otItemReader["IdDrawing"]);
                                    otItem.RevisionItem.DrawingPath = Convert.ToString(otItemReader["DrawingPath"]);
                                    otItem.RevisionItem.SolidworksDrawingFileName = Convert.ToString(otItemReader["SolidworksDrawingFileName"]);
                                }

                                if (otItemReader["AssignedTo"] != DBNull.Value)
                                {
                                    otItem.AssignedTo = Convert.ToInt32(otItemReader["AssignedTo"]);

                                    otItem.AssignedToUser = new People();
                                    otItem.AssignedToUser.Name = Convert.ToString(otItemReader["AssignedName"]);
                                    otItem.AssignedToUser.Surname = Convert.ToString(otItemReader["AssignedSurname"]);
                                }


                            }
                            otItem.RevisionItem.CPProduct.LstCPDetection = new List<CPDetection>();
                            otItem.LstDetection = new List<Detection>();
                            otItems.Add(otItem);




                            keyId++;

                            //i = otItem.KeyId + 1;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2540(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    if (otItemReader.NextResult())
                        while (otItemReader.Read())
                        {
                            try
                            {
                                if (otItemReader["CPProductID"] != DBNull.Value)
                                {
                                    if (otItems.Any(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])))
                                    {
                                        List<OtItem> lstotitems = otItems.Where(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])).ToList();

                                        CPDetection cpdetection = new CPDetection();
                                        cpdetection.CPProductID = Convert.ToInt64(otItemReader["CPProductID"]);
                                        if (otItemReader["DetectionID"] != DBNull.Value)
                                            cpdetection.DetectionID = Convert.ToInt32(otItemReader["DetectionID"]);
                                        if (otItemReader["Name"] != DBNull.Value)
                                            cpdetection.DetectionName = Convert.ToString(otItemReader["Name"]);
                                        if (otItemReader["NumDetections"] != DBNull.Value)
                                            cpdetection.NumDetections = Convert.ToInt32(otItemReader["NumDetections"]);

                                        lstotitems.ForEach(x => { x.RevisionItem.CPProduct.LstCPDetection.Add(cpdetection); });
                                        if (!otDetectionLst.Any(i => i.IdDetection == cpdetection.DetectionID))
                                        {
                                            Detection detection = new Detection();
                                            detection.IdDetection = Convert.ToUInt32(cpdetection.DetectionID);
                                            detection.Name = Convert.ToString(cpdetection.DetectionName);
                                            otDetectionLst.Add(detection);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }


                }
            }
            otItems.ForEach(x => { x.LstDetection = otDetectionLst; });
            //Log4NetLogger.Logger.Log("Executed GetOtItemsByIdOt().", category: Category.Info, priority: Priority.Low);
            return otItems;
        }

        // [GEOS2-5472][nsatpute][04-07-2024]
        public byte[] GetSolidworksDrawingFileImageInBytes(string darwingPath, string SolidworksDrawingFileName, string solidWorksPath)
        {
            byte[] fileBytes = null;
            try
            {
                //string filePath = Path.Combine(solidWorksPath, darwingPath, SolidworksDrawingFileName + ".Pdf");
                string filePath = string.Format(@"{0}\{1}\{2}", solidWorksPath, darwingPath, SolidworksDrawingFileName + ".Pdf");
                if (File.Exists(filePath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        fileBytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(fileBytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetSolidworksDrawingFileImageInBytes(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return fileBytes;
        }
        /// <summary>
        /// [000][nsatpute][15-07-2024][GEOS2-5408] CCI-370 & CCI-420 Download Test Table Documentation from ECOS & Documentation in USB Pen Drive (4/10) 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;

            using (var context = new WorkbenchContext())
            {
                list = (from records in context.LookupValues where records.IdLookupKey == key select records).OrderBy(y => y.Position).ToList();
            }

            return list;
        }

        //[pramod.misal][GEOS2-5473][09.07.2024]
        public List<Article> GetAllReference(bool seeAllArticles, string connectionString)
        {

            List<Article> referenceList = new List<Article>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetAllReference", mySqlConnection);
                    mySqlCommand.Parameters.AddWithValue("_ShowAllReferences", seeAllArticles);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            Article reference = new Article();

                            if (mySqlDataReader["idArticle"] != DBNull.Value)
                                reference.IdArticle = Convert.ToInt32(mySqlDataReader["idArticle"].ToString());

                            if (mySqlDataReader["Reference"] != DBNull.Value)
                                reference.Reference = mySqlDataReader["Reference"].ToString();

                            if (mySqlDataReader["Description"] != DBNull.Value)
                                reference.Description = mySqlDataReader["Description"].ToString();

                            if (mySqlDataReader["ImagePath"] != DBNull.Value && Convert.ToString(mySqlDataReader["ImagePath"]).Trim() != string.Empty)
                            {
                                reference.ImagePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/ARTICLE%20VISUAL%20AIDS/" + Convert.ToString(mySqlDataReader["ImagePath"]);
                            }

                            referenceList.Add(reference);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllReference(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return referenceList;
        }

        //[pramod.misal][GEOS2-5473][12.07.2024]
        public bool DeleteItems(string MainServerConnectionString, List<OtItem> deletedOtItems)
        {
            bool status = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (var otItem in deletedOtItems)
                        {
                            using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_DeleteotitemsByIdOtItem", mySqlConnection))
                            {
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdOtItem", otItem.IdOTItem);
                                mySqlCommand.Parameters.AddWithValue("_IdOT", otItem.IdOT);
                                mySqlCommand.Parameters.AddWithValue("_IdRevisionItem", otItem.IdRevisionItem);

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                        status = true;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in DeleteItems(). ErrorMessage - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    throw;
                }
            }
            return status;
        }

        //[pramod.misal][GEOS2-5473][15.07.2024]
        public Ots GetWorkOrderByIdOt_V2540(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2180", connOT);
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }

                                if (otReader["Observations"] != DBNull.Value)
                                    ot.Observations = Convert.ToString(otReader["Observations"]);

                            }
                            //ot.OtItems = GetOtItemsByIdOt_V2170(connectionString, idOt);
                            //[pramod.misal][GEOS2-5473][15.07.2024]
                            ot.OtItems = GetOtItemsByIdOt_V2540(connectionString, idOt);
                            ot.OtItemsOb = new ObservableCollection<OtItem>(ot.OtItems);
                            //[pramod.misal][GEOS2-5474][22.07.2024]
                            ot.OTLogEntries = GetAllOtItemLogEntriesByIdOt(connectionString, idOt);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetWorkOrderByIdOt_V2180(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetWorkOrderByIdOt_V2180().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        public bool UpdateOTAssignedUser_V2540(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser, List<OtItem> DeletedOtItemList, List<OtItem> AddedOtItemList, List<OtItem> UpdatedOtItemList, List<LogEntriesByOT> LogEntriesByOTList)
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

                    if (OldRemark != Remark)
                        UpdateOTSObservation(connectionString, IdOT, Remark, IdUser);

                    if (DeletedOtItemList != null)
                    {
                        DeleteItems(connectionString, DeletedOtItemList);
                    }

                    if (UpdatedOtItemList != null)
                    {
                        UpdateItems(connectionString, UpdatedOtItemList);
                    }
                    if (AddedOtItemList != null)
                    {
                        AddOtItems(connectionString, AddedOtItemList, IdOT);
                    }
                    //[rdixit][GEOS2-5474][26.07.2024]
                    if (LogEntriesByOTList != null)
                        AddCommentsOrLogEntriesByOT(connectionString, LogEntriesByOTList);

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
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOTAssignedUser_V2540(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }


        //[pramod.misal][GEOS2-5473][12.07.2024]
        public bool AddOtItems(string MainServerConnectionString, List<OtItem> AddedOtItems, Int64 IdOT)
        {
            bool status = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (var otItem in AddedOtItems)
                        {
                            if (otItem.RevisionItem.NumItem.Contains("."))
                            {
                                string[] parts = otItem.RevisionItem.NumItem.Split('.');
                                string numItem = parts[0];
                                string number = "." + parts[1];
                                using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_InsertOitemWarehouseComponents", mySqlConnection))
                                {
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", otItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                                    mySqlCommand.Parameters.AddWithValue("_Quantity", otItem.RevisionItem.WarehouseProduct.Article.Quantity);
                                    mySqlCommand.Parameters.AddWithValue("_NumItem", numItem);
                                    mySqlCommand.Parameters.AddWithValue("_Number", number);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_InsertNewOtItem", mySqlConnection))
                                {
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_Quantity", otItem.RevisionItem.Quantity);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", otItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                                    mySqlCommand.Parameters.AddWithValue("_ArticleDescription", otItem.RevisionItem.WarehouseProduct.Article.Description);
                                    mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                                    mySqlCommand.Parameters.AddWithValue("_IdRevision", otItem.RevisionItem.IdRevision);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        mySqlConnection.Close();
                        status = true;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in AddOtItems(). ErrorMessage - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    throw;
                }
            }
            return status;
        }

        public List<OtItem> GetOtItemsByIdOt_V2540(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetOtItemsByIdOt_V2540", connOtItem);
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
                            if (otItemReader["IdRevision"] != DBNull.Value)
                            {
                                otItem.IdRevision = Convert.ToInt64(otItemReader["IdRevision"]);

                            }
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }


                }
            }

            //Log4NetLogger.Logger.Log("Executed GetOtItemsByIdOt().", category: Category.Info, priority: Priority.Low);
            return otItems;
        }
        // [nsatpute][18-07-2024] [GEOS2-5409] 
        public void StartSavingFile(string saveDirectorPath, string fileName)
        {
            try
            {
                string filePath = Path.Combine(saveDirectorPath, fileName);

                if (!Directory.Exists(saveDirectorPath))
                {
                    Directory.CreateDirectory(saveDirectorPath);
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error StartSavingFile(File Name-{0}). ErrorMessage- {1}", fileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        // [nsatpute][18-07-2024] [GEOS2-5409] 
        public void SavePartData(string saveDirectorPath, string fileName, byte[] partData)
        {
            try
            {
                string filePath = Path.Combine(saveDirectorPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    fileStream.Write(partData, 0, partData.Length);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error SavePartData(File Name-{0}). ErrorMessage- {1}", fileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[pramod.misal][GEOS2-5473][12.07.2024]
        public bool UpdateItems(string MainServerConnectionString, List<OtItem> updatedOtitems)
        {
            bool status = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (var otItem in updatedOtitems)
                        {
                            if (!otItem.RevisionItem.NumItem.Contains("."))
                            {
                                using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_UpdateOtItem", mySqlConnection))
                                {
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdRevisionItem", otItem.RevisionItem.IdRevisionItem);
                                    mySqlCommand.Parameters.AddWithValue("_Quantity", otItem.RevisionItem.Quantity);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", otItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        mySqlConnection.Close();
                        status = true;
                    }
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in EditItems(). ErrorMessage - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    throw;
                }
            }
            return status;
        }
        // [nsatpute][19-07-2024] [GEOS2-5409]
        public List<OTAttachment> GetQualityOTAndItemAttachments(Company company, Int64 idOT, string workOrderPath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAttachment> otAttachments = new List<OTAttachment>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_GetQualityOTAndItemAttachment_V2550", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdOT", idOT);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    while (reader.Read())
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
                                    otAttachment.OriginalFileName = item;
                                    otAttachment.SavedFileName = item;
                                    otAttachment.FileUploadName = item;


                                    string[] stringExtension = otAttachment.FileName.Split('.');
                                    if (stringExtension.Count() > 0)
                                    {
                                        if (!string.IsNullOrEmpty(stringExtension[1]))
                                        {
                                            otAttachment.FileExtension = "." + stringExtension[1];
                                            otAttachment.FileType = otAttachment.FileExtension;
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
                        otAttachment.FileSizeInInt = fi.Length;
                    }
                }
            }

            return otAttachments;
        }

        //[pramod.misal][GEOS2-5474][08.07.2024]
        public List<LogEntriesByOT> GetAllOtItemLogEntriesByIdOt(string connectionString, Int64 idOt)
        {
            List<LogEntriesByOT> logEntriesByOT = new List<LogEntriesByOT>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_GetLogEntriesByOT", connOtItem);
                otItemCommand.CommandType = CommandType.StoredProcedure;
                otItemCommand.Parameters.AddWithValue("_IdOT", idOt);

                using (MySqlDataReader otItemReader = otItemCommand.ExecuteReader())
                {

                    while (otItemReader.Read())
                    {
                        try
                        {
                            LogEntriesByOT LogEntriesByOT = new LogEntriesByOT();


                            if (otItemReader["IdLogEntryByOT"] != DBNull.Value)
                                LogEntriesByOT.IdLogEntryByOT = Convert.ToInt32(otItemReader["IdLogEntryByOT"].ToString());

                            if (otItemReader["IdOT"] != DBNull.Value)
                                LogEntriesByOT.IdOT = Convert.ToInt32(otItemReader["IdOT"].ToString());

                            if (otItemReader["IdUser"] != DBNull.Value)
                            {
                                LogEntriesByOT.People = new People();
                                LogEntriesByOT.People.IdPerson = Convert.ToInt32(otItemReader["IdUser"].ToString());
                                LogEntriesByOT.People.Name = otItemReader["Name"].ToString();
                                LogEntriesByOT.People.Surname = otItemReader["Surname"].ToString();
                            }

                            if (otItemReader["Datetime"] != DBNull.Value)
                                LogEntriesByOT.Datetime = Convert.ToDateTime(otItemReader["Datetime"].ToString());

                            if (otItemReader["Comments"] != DBNull.Value)
                                LogEntriesByOT.Comments = otItemReader["Comments"].ToString();

                            if (otItemReader["IdLogEntryType"] != DBNull.Value)
                                LogEntriesByOT.IdLogEntryType = Convert.ToByte(otItemReader["IdLogEntryType"]);

                            if (otItemReader["IsRtfText"] != DBNull.Value)
                                LogEntriesByOT.IsRtfText = Convert.ToBoolean(otItemReader["IsRtfText"]);

                            logEntriesByOT.Add(LogEntriesByOT);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetOtItemsByIdOt_V2170(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }


            return logEntriesByOT;
        }
        //[nsatpute][GEOS2-5473][26.07.2024]
        public List<Article> GetComponentArticlesByIdArticle(int idArticle, string connectionString)
        {

            List<Article> referenceList = new List<Article>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetComponentArticlesByIdArticle", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticle", idArticle);
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            Article reference = new Article();

                            if (mySqlDataReader["idArticle"] != DBNull.Value)
                                reference.IdArticle = Convert.ToInt32(mySqlDataReader["idArticle"].ToString());

                            if (mySqlDataReader["Reference"] != DBNull.Value)
                                reference.Reference = mySqlDataReader["Reference"].ToString();

                            if (mySqlDataReader["Description"] != DBNull.Value)
                                reference.Description = mySqlDataReader["Description"].ToString();

                            reference.Quantity = Convert.ToDouble(mySqlDataReader["Quantity"]);
                            referenceList.Add(reference);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllReference(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return referenceList;
        }
        //[nsatpute][02-08-2024][GEOS2-5410]
        public TechnicalSpecifications GetTechnicalSpecificationForReport(Company company, long idOt)
        {
            TechnicalSpecifications techSpecification = new TechnicalSpecifications();
            techSpecification.Properties = new List<Detection>();
            try
            {
                string connectionString = company.ConnectPlantConstr;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetTechnicalSpecificationsOfZTypeOt_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOT", idOt);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            techSpecification.ProjectName = Convert.ToString(rdr["ProjectName"]);
                            techSpecification.Reference = Convert.ToString(rdr["Reference"]);
                            techSpecification.Model = Convert.ToString(rdr["Model"]);
                            techSpecification.Oem = Convert.ToString(rdr["Oem"]);
                            techSpecification.Voltage = $"{Convert.ToString(rdr["Voltage"])} V";
                            techSpecification.Amperage = $"{Convert.ToString(rdr["Amperage"])} A";
                            techSpecification.Frequency = $"{Convert.ToString(rdr["MinFrequency"])}/{Convert.ToString(rdr["MaxFrequency"])} Hz";
                            techSpecification.Pressure = $"{Convert.ToString(rdr["MinPressure"])}-{Convert.ToString(rdr["MaxPressure"])} bar";
                            techSpecification.Category = Convert.ToString(rdr["Category"]);
                            if (rdr["IdCarOEM"] != DBNull.Value)
                                techSpecification.IdOem = Convert.ToInt32(rdr["IdCarOEM"]);
                        }
                        if (rdr.NextResult())
                        {
                            while (rdr.Read())
                            {
                                Detection det = new Detection();
                                det.IdGroup = Convert.ToInt32(rdr["IdGroup"]);
                                det.GroupName = Convert.ToString(rdr["GroupName"]);
                                det.Name = Convert.ToString(rdr["Name"]);
                                det.Quantity = Convert.ToInt16(rdr["NumDetections"]);
                                det.IdDetectionType = Convert.ToInt32(rdr["IdDetectionType"]);
                                techSpecification.Properties.Add(det);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTechnicalSpecificationForReport(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return techSpecification;
        }

        //[nsatpute][02-08-2024][GEOS2-5410]
        public List<ModuleReference> GetModuleReferencesForReport(Company company, long idOt)
        {
            List<ModuleReference> lstReferences = new List<ModuleReference>();
            try
            {
                string connectionString = company.ConnectPlantConstr;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetModuleDetails_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOT", idOt);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ModuleReference reference = new ModuleReference();
                            reference.Detections = new List<Detection>();
                            reference.NumItem = Convert.ToString(rdr["NumItem"]);
                            reference.Reference = Convert.ToString(rdr["Reference"]);
                            reference.Type = Convert.ToString(rdr["Type"]);
                            reference.Quantity = Convert.ToInt32(rdr["Quantity"]);
                            reference.IdConnector = Convert.ToInt32(rdr["IdConnector"]);
                            reference.IdRevisionItem = Convert.ToInt32(rdr["IdRevisionItem"]);
                            reference.EcosNavigateUrl = Convert.ToString(rdr["EcosNavigateUrl"]);
                            reference.ConnectorImageApiUrl = Convert.ToString(rdr["ConnectorImageApiUrl"]);
                            lstReferences.Add(reference);
                        }
                        if (rdr.NextResult())
                        {
                            while (rdr.Read())
                            {
                                int idRevisionItem = Convert.ToInt32(rdr["IdRevisionItem"]);
                                if (lstReferences.Any(x => x.IdRevisionItem == idRevisionItem))
                                {
                                    Detection det = new Detection();
                                    det.IdDetection = Convert.ToUInt32(rdr["IdDetection"]);
                                    det.Name = Convert.ToString(rdr["Name"]);
                                    det.Quantity = Convert.ToInt32(rdr["NumDetections"]);
                                    det.IdDetectionType = Convert.ToInt32(rdr["IdDetectionType"]);
                                    lstReferences.FirstOrDefault(x => x.IdRevisionItem == idRevisionItem).Detections.Add(det);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTechnicalSpecificationForReport(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return lstReferences;
        }
        //[nsatpute][07-08-2024][GEOS2-5410]
        public List<OTAttachment> GetStructureOTAttachment(Company company, Int64 idOT, string workOrderPath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAttachment> otAttachments = new List<OTAttachment>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_GetStructureOtAttachment_V2550", con);
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
                                    otAttachment.OriginalFileName = item;
                                    otAttachment.SavedFileName = item;
                                    otAttachment.FileUploadName = item;


                                    string[] stringExtension = otAttachment.FileName.Split('.');
                                    if (stringExtension.Count() > 0)
                                    {
                                        if (!string.IsNullOrEmpty(stringExtension[1]))
                                        {
                                            otAttachment.FileExtension = "." + stringExtension[1];
                                            otAttachment.FileType = otAttachment.FileExtension;
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
                        otAttachment.FileSizeInInt = fi.Length;
                    }
                }
            }

            return otAttachments;
        }
        //[nsatpute][02-08-2024][GEOS2-5410]
        public List<Article> GetTestboardElectrificationOtArticles(Company company, long idOt)
        {
            List<Article> lstArticles = new List<Article>();
            try
            {
                string connectionString = company.ConnectPlantConstr;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetTestboardElectrificationOtArticles_V2550", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOT", idOt);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Article reference = new Article();
                            reference.ArticleCategory = new ArticleCategory();
                            reference.ArticleCategory.CategoryName = Convert.ToString(rdr["PCMCategory"]);
                            //[rdixit][29.08.2024][GEOS2-5410]
                            if (rdr["ParentFirst"] != DBNull.Value)
                            {
                                reference.FirstParent = Convert.ToString(rdr["ParentFirst"]);
                            }
                            if (rdr["ParentSecond"] != DBNull.Value)
                            {
                                reference.ParentSecond = Convert.ToString(rdr["ParentSecond"]);
                            }
                            reference.Reference = Convert.ToString(rdr["Reference"]);
                            reference.Description = Convert.ToString(rdr["Description"]);
                            reference.Quantity = Convert.ToInt32(rdr["Quantity"]);
                            reference.ImagePath = Convert.ToString(rdr["OriginalFileName"]);
                            lstArticles.Add(reference);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTechnicalSpecificationForReport(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return lstArticles;
        }
        // [nsatpute][12-08-2024][GEOS2-5412]
        public List<OTAttachment> GetItemAttachments(Company company, Int64 idOT, string workOrderPath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAttachment> otAttachments = new List<OTAttachment>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_GetItemAttachments_V2550", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdOT", idOT);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    while (reader.Read())
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
                                    otAttachment.OriginalFileName = item;
                                    otAttachment.SavedFileName = item;
                                    otAttachment.FileUploadName = item;


                                    string[] stringExtension = otAttachment.FileName.Split('.');
                                    if (stringExtension.Count() > 0)
                                    {
                                        if (!string.IsNullOrEmpty(stringExtension[1]))
                                        {
                                            otAttachment.FileExtension = "." + stringExtension[1];
                                            otAttachment.FileType = otAttachment.FileExtension;
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
                        otAttachment.FileSizeInInt = fi.Length;
                    }
                }
            }

            return otAttachments;
        }
        public byte[] GetstructureOtPlanolInBytes(Company company, Int64 idOt, string solidWorksPath)
        {
            byte[] fileBytes = null;
            string drawingPath = string.Empty;
            string solidworksDrawingFileName = string.Empty;

            try
            {
                string connectionString = company.ConnectPlantConstr;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand concommand = new MySqlCommand("SAM_GetStructureOtPlanolFile_V2550", con);
                    concommand.CommandType = CommandType.StoredProcedure;
                    concommand.Parameters.AddWithValue("_IdOT", idOt);
                    using (MySqlDataReader reader = concommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            drawingPath = Convert.ToString(reader["Path"]);
                            solidworksDrawingFileName = Convert.ToString(reader["SolidworksDrawingFileName"]);
                        }
                    }
                }

                //string filePath = Path.Combine(solidWorksPath, darwingPath, SolidworksDrawingFileName + ".Pdf");
                string filePath = string.Format(@"{0}\{1}\{2}", solidWorksPath, drawingPath, solidworksDrawingFileName + ".Pdf");
                if (File.Exists(filePath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        fileBytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(fileBytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetstructurePlanolInBytes(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return fileBytes;
        }
        // [nsatpute][14-08-2024][GEOS2-5411]
        public List<byte[]> GetstructureOtElectricalDiagramsInBytes(Company company, Int64 idOt, string electricalDiagramPath)
        {
            List<byte[]> lstElectricalDiagram = new List<byte[]>();
            try
            {
                string connectionString = company.ConnectPlantConstr;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand concommand = new MySqlCommand("SAM_GetStructureOtElectricalDiagram_V2550", con);
                    concommand.CommandType = CommandType.StoredProcedure;
                    concommand.Parameters.AddWithValue("_IdOT", idOt);
                    using (MySqlDataReader reader = concommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fileName = Convert.ToString(reader["FilePath"]);

                            if (!electricalDiagramPath.EndsWith("\\"))
                                electricalDiagramPath += "\\";

                            fileName = fileName.TrimStart('\\');

                            string fullFileName = Path.Combine(electricalDiagramPath, fileName);
                            if (File.Exists(fullFileName))
                            {
                                using (System.IO.FileStream stream = new System.IO.FileStream(fullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                {
                                    byte[] fileBytes = new byte[stream.Length];
                                    int numBytesToRead = (int)stream.Length;
                                    int numBytesRead = 0;

                                    while (numBytesToRead > 0)
                                    {
                                        // Read may return anything from 0 to numBytesToRead.
                                        int n = stream.Read(fileBytes, numBytesRead, numBytesToRead);

                                        // Break when the end of the file is reached.
                                        if (n == 0)
                                            break;
                                        numBytesRead += n;
                                        numBytesToRead -= n;
                                    }
                                    lstElectricalDiagram.Add(fileBytes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetstructureOtElectricalDiagramsInBytes(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return lstElectricalDiagram;
        }
        public ProductTypes GetstructureOtProductDetails(Company company, Int64 idOt, string productTypeImagesPath)
        {
            ProductTypes productType = new ProductTypes();
            productType.Image = new ProductTypeImage();
            //productType.Image.ProductTypeImageInBytes 

            try
            {
                string connectionString = company.ConnectPlantConstr;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand concommand = new MySqlCommand("SAM_GetStructureOtCptypeImage_v2550", con);
                    concommand.CommandType = CommandType.StoredProcedure;
                    concommand.Parameters.AddWithValue("_IdOT", idOt);
                    using (MySqlDataReader reader = concommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["code"] != DBNull.Value)
                                productType.Code = Convert.ToString(reader["code"]);
                            if (reader["SavedFileName"] != DBNull.Value)
                                productType.Image.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            if (reader["IdCPTypeImage"] != DBNull.Value)
                                productType.Image.IdCPTypeImage = Convert.ToUInt32(reader["IdCPTypeImage"]);
                        }
                    }
                }

                string filePath = string.Format(@"{0}\{1}\{2}", productTypeImagesPath, productType.Image.IdCPTypeImage, productType.Image.SavedFileName);
                if (File.Exists(filePath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        productType.Image.ProductTypeImageInBytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(productType.Image.ProductTypeImageInBytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetstructurePlanolInBytes(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return productType;
        }
        // [nsatpute][21-08-2024][GEOS2-5412]
        public byte[] GenerateAndGetDeclarationOfConformity(Company company, Int64 idOt, string strucutreTemplatePath, string workingOrderPath)
        {
            byte[] fileData = null;
            string productReference = string.Empty;
            string offerNumber = string.Empty;
            DateTime date = DateTime.Now;
            string signatureJobTitle = string.Empty;
            string signatureName = string.Empty;
            string manufacturerStreet = string.Empty;
            string manufacturerZipCode = string.Empty;
            string manufacturerCity = string.Empty;
            string manufacturerState = string.Empty;
            string manufacturerCountry = string.Empty;
            string manufacturerName = string.Empty;
            string manufacturerNumber = string.Empty;
            string productName = string.Empty;
            string productCategory = string.Empty;


            //string originalFilePath = @"C:\Personal\DeclarationOfConformity.xlsx";
            string originalFilePath = Path.Combine(strucutreTemplatePath, @"DeclarationOfConformity.xlsx");
            // string pdfOutputPath = @"C:\Personal\IndexSheet.pdf";
            string otPath = GetStructureOtAttachmentPath(company, idOt);
            string pdfOutputPath = Path.Combine(workingOrderPath, otPath, "DeclarationOfConformity.pdf");


            string connectionString = company.ConnectPlantConstr;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_GetZTypeOtDetailsForDeclarationConformity_V2550", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdOT", idOt);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["Reference"] != DBNull.Value)
                            productReference = Convert.ToString(reader["Reference"]);

                        if (reader["OfferCode"] != DBNull.Value)
                            offerNumber = Convert.ToString(reader["OfferCode"]);

                        if (reader["JobTitle"] != DBNull.Value)
                            signatureJobTitle = Convert.ToString(reader["JobTitle"]);

                        if (reader["SignatureName"] != DBNull.Value)
                            signatureName = Convert.ToString(reader["SignatureName"]);

                        if (reader["Street"] != DBNull.Value)
                            manufacturerStreet = Convert.ToString(reader["Street"]);

                        if (reader["ZipCode"] != DBNull.Value)
                            manufacturerZipCode = Convert.ToString(reader["ZipCode"]);

                        if (reader["City"] != DBNull.Value)
                            manufacturerCity = Convert.ToString(reader["City"]);

                        if (reader["State"] != DBNull.Value)
                            manufacturerState = Convert.ToString(reader["State"]);

                        if (reader["Country"] != DBNull.Value)
                            manufacturerCountry = Convert.ToString(reader["Country"]);

                        if (reader["RegisteredName"] != DBNull.Value)
                            manufacturerName = Convert.ToString(reader["RegisteredName"]);

                        if (reader["Cif"] != DBNull.Value)
                            manufacturerNumber = Convert.ToString(reader["Cif"]);

                        if (reader["ProductName"] != DBNull.Value)
                            productName = Convert.ToString(reader["ProductName"]);

                        if (reader["Category"] != DBNull.Value)
                            productCategory = Convert.ToString(reader["Category"]);
                    }
                }
            }

            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(originalFilePath));
            if (File.Exists(originalFilePath))
            {
                // Example values to be passed
                if (!Directory.Exists(Path.GetDirectoryName(pdfOutputPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(pdfOutputPath));

                File.Copy(originalFilePath, tempFilePath, true);
                UpdateAndExportExcel(tempFilePath, pdfOutputPath, productReference, offerNumber, date,
                     signatureJobTitle, signatureName, manufacturerStreet, manufacturerZipCode, manufacturerCity,
                     manufacturerState, manufacturerCountry, manufacturerName, manufacturerNumber,
                     productName, productCategory);
            }
            if (File.Exists(pdfOutputPath))
            {
                UpdateStructureOtAttachmentFileName(company, idOt, Path.GetFileName(pdfOutputPath));
                using (System.IO.FileStream stream = new System.IO.FileStream(pdfOutputPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    fileData = new byte[stream.Length];
                    int numBytesToRead = (int)stream.Length;
                    int numBytesRead = 0;

                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = stream.Read(fileData, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                }
            }
            return fileData;
        }
        // [nsatpute][21-08-2024][GEOS2-5412]
        private void UpdateAndExportExcel(string excelFilePath, string pdfOutputPath,
           string productReference, string offerNumber, DateTime date,
           string signatureJobTitle, string signatureName, string manufacturerStreet,
           string manufacturerZipCode, string manufacturerCity, string manufacturerState,
           string manufacturerCountry, string manufacturerName, string manufacturerNumber,
           string productName, string productCategory)
        {
            // Load the original workbook
            Workbook originalWorkbook = new Workbook();
            originalWorkbook.LoadDocument(excelFilePath);

            // Access the RowData sheet
            Worksheet rowDataSheet = originalWorkbook.Worksheets["RawData"];

            // Add values to column B based on values in column A
            for (int i = 0; i < rowDataSheet.Rows.LastUsedIndex; i++)
            {
                string cellValue = rowDataSheet.Cells[i, 0].Value.ToString();

                switch (cellValue)
                {
                    case "Index_ProductReference":
                        rowDataSheet.Cells[i, 1].Value = productReference;
                        break;
                    case "Index_OfferNumber":
                        rowDataSheet.Cells[i, 1].Value = offerNumber;
                        break;
                    case "Index_Date":
                        rowDataSheet.Cells[i, 1].Value = date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,CultureInfo.CurrentCulture);
                        break;
                    case "Index_SignatureJobTitle":
                        rowDataSheet.Cells[i, 1].Value = signatureJobTitle;
                        break;
                    case "Index_SignatureName":
                        rowDataSheet.Cells[i, 1].Value = signatureName;
                        break;
                    case "Index_ManufacturerStreet":
                        rowDataSheet.Cells[i, 1].Value = manufacturerStreet;
                        break;
                    case "Index_ManufacturerZipCode":
                        rowDataSheet.Cells[i, 1].Value = manufacturerZipCode;
                        break;
                    case "Index_ManufacturerCity":
                        rowDataSheet.Cells[i, 1].Value = manufacturerCity;
                        break;
                    case "Index_ManufacturerState":
                        rowDataSheet.Cells[i, 1].Value = manufacturerState;
                        break;
                    case "Index_ManufacturerCountry":
                        rowDataSheet.Cells[i, 1].Value = manufacturerCountry;
                        break;
                    case "Index_ManufacturerName":
                        rowDataSheet.Cells[i, 1].Value = manufacturerName;
                        break;
                    case "Index_ManufacturerNumber":
                        rowDataSheet.Cells[i, 1].Value = manufacturerNumber;
                        break;
                    case "Index_ProductName":
                        rowDataSheet.Cells[i, 1].Value = productName;
                        break;
                    case "Index_ProductCategory":
                        rowDataSheet.Cells[i, 1].Value = productCategory;
                        break;
                }
            }

            // Save the changes to the original workbook
            originalWorkbook.SaveDocument(excelFilePath);

            // Create a new workbook for the export
            Workbook exportWorkbook = originalWorkbook;
            RemoveFormulasAndReplaceWithValues(exportWorkbook.Worksheets[0]);
            for (int i = exportWorkbook.Worksheets.Count - 1; i > 0; i--)
            {
                exportWorkbook.Worksheets.RemoveAt(i);
            }
            // Export the new workbook with the Index sheet to PDF
            using (FileStream pdfStream = new FileStream(pdfOutputPath, FileMode.Create, FileAccess.Write))
            {
                exportWorkbook.ExportToPdf(pdfStream);
            }
            if (File.Exists(excelFilePath))
            {
                File.Delete(excelFilePath);
            }

        }
        // [nsatpute][21-08-2024][GEOS2-5412]
        private void RemoveFormulasAndReplaceWithValues(Worksheet worksheet)
        {
            // Iterate through all cells in the worksheet
            for (int rowIndex = 0; rowIndex < worksheet.Rows.LastUsedIndex; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < worksheet.Columns.LastUsedIndex; columnIndex++)
                {
                    Cell cell = worksheet.Cells[rowIndex, columnIndex];

                    // Check if the cell contains a formula
                    if (cell.HasFormula)
                    {
                        // Replace the formula with its calculated value
                        cell.Value = cell.Value;
                    }
                }
            }
        }
        // [nsatpute][22-08-2024][GEOS2-5412]
        public byte[] GetCustomerLogoById(int idCustomer, string customerImagePath)
        {
            byte[] fileData = null;
            string fullFilePath = Path.Combine(customerImagePath, idCustomer.ToString());

            if (Directory.Exists(fullFilePath))
            {
                // Get the first PNG file from the directory
                string[] files = Directory.GetFiles(fullFilePath, "*.png");

                if (files.Length > 0)
                {
                    string filePath = files[0]; // Take the first PNG file found

                    using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        fileData = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            int n = stream.Read(fileData, numBytesRead, numBytesToRead);
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
            }
            return fileData;
        }

        // [nsatpute][22-08-2024][GEOS2-5412]
        private string GetStructureOtAttachmentPath(Company company, Int64 idOt)
        {
            string path = string.Empty;
            string connectionString = company.ConnectPlantConstr;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_GetStructureOtAttachmentPath_V2550", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdOT", idOt);
                path = Convert.ToString(concommand.ExecuteScalar());
            }
            return path;
        }
        // [nsatpute][22-08-2024][GEOS2-5412]
        private void UpdateStructureOtAttachmentFileName(Company company, Int64 idOt, string fileName)
        {
            string path = string.Empty;
            string connectionString = company.ConnectPlantConstr;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("SAM_UpdateStructureOtAttachmentFileName_V2550", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdOT", idOt);
                concommand.Parameters.AddWithValue("_FName", fileName);
                concommand.ExecuteNonQuery();
            }
        }
        //[nsatpute][03-09-2024][GEOS2-5414]
        public QcPassLabelDetails GetQCPassLabelDetails(Company company, long idOt)
        {
            QcPassLabelDetails labelDetails = new QcPassLabelDetails();
            try
            {
                string connectionString = company.ConnectPlantConstr;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetQCPassLabelDetails_V2560", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdOT", idOt);
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            labelDetails.SiteShortName = Convert.ToString(rdr["ShortName"]);
                            labelDetails.SupplierName = Convert.ToString(rdr["SupplierName"]);
                            labelDetails.TaxNumber = Convert.ToString(rdr["TaxNumber"]);
                            labelDetails.CustomerName = Convert.ToString(rdr["CustomerName"]);
                            labelDetails.Order = Convert.ToString(rdr["code"]);
                            labelDetails.Product = Convert.ToString(rdr["Product"]);
                            labelDetails.Reference = Convert.ToString(rdr["Reference"]);
                            labelDetails.Voltage = $"{Convert.ToString(rdr["Voltage"])} V";
                            labelDetails.Amperage = $"{Convert.ToString(rdr["Amperage"])} A";
                            labelDetails.Frequency = $"{Convert.ToString(rdr["MinFrequency"])}/{Convert.ToString(rdr["MaxFrequency"])} Hz";
                            labelDetails.Pressure = $"{Convert.ToString(rdr["MinPressure"])}-{Convert.ToString(rdr["MaxPressure"])} bar";
                            labelDetails.SupplierCountry = Convert.ToString(rdr["CountryName"]);
                            labelDetails.QRCodeData = Convert.ToString(rdr["QRData"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTechnicalSpecificationForReport(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return labelDetails;
        }

        // [nsatpute][12-11-2024][GEOS2-5890]
        public bool UpdateWorkflowStatusInOTQC_V2580(string MainServerConnectionString, UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            bool status;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTS_UpdateWorkflowStatus_QC_V2580", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                        mySqlCommand.Parameters.AddWithValue("_IdWorkflowStatus", IdWorkflowStatus);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        status = true;
                    }
                    AddCommentsOrLogEntriesByOT(MainServerConnectionString, LogEntriesByOTList);
                    UpdateInOT(MainServerConnectionString, IdOT, IdUser);
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateWorkflowStatusInOTQC(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return status;
        }
        // [nsatpute][13-11-2024][GEOS2-5889]
        public List<Ots> GetPendingWorkorders_V2580(Company company, string userProfileImageFilePath, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();

            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2580", connOts);
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
                            if (otReader["DeliveryWeek"] != DBNull.Value)
                            {
                                ot.DeliveryWeek = Convert.ToString(otReader["DeliveryWeek"]);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);
                                }

                                ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                if (otReader["IdOfferType"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);
                                    ot.Quotation.Offer.OfferType = new OfferType();
                                    ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                    ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);
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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
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
                                    }
                                }
                            }

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);
                            try
                            {
                                if (otReader["EndTime"] != DBNull.Value)
                                    ot.EndDateTime = Convert.ToDateTime(otReader["EndTime"]);

                                if (otReader["EndTimeFlag"] != DBNull.Value)
                                    ot.EndTimeFlag = Convert.ToInt32(otReader["EndTimeFlag"]);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() . ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2500().", category: Category.Info, priority: Priority.Low);
            return ots;
        }

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public List<OtItem> GetStructureOtItemsByIdOt_V2640(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();
            List<Detection> otDetectionLst = new List<Detection>();
            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("SAM_getstructureotitemsbyidot_V2640", connOtItem);
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

                                otItem.RevisionItem.CPProduct = new CPProduct();
                                otItem.RevisionItem.CPProduct.Reference = Convert.ToString(otItemReader["Reference"]);

                                if (otItemReader["IdProduct"] != DBNull.Value)
                                    otItem.RevisionItem.IdProduct = Convert.ToInt64(otItemReader["IdProduct"]);

                                if (otItemReader["IdCPType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.IdCPTypeNew = Convert.ToInt64(otItemReader["IdCPType"]);

                                if (otItemReader["ProductType"] != DBNull.Value)
                                    otItem.RevisionItem.CPProduct.ProductTypeName = Convert.ToString(otItemReader["ProductType"]);

                                if (otItemReader["Iditemotstatus"] != DBNull.Value)
                                {
                                    otItem.IdItemOtStatus = Convert.ToByte(otItemReader["Iditemotstatus"]);

                                    otItem.Status = new ItemOTStatusType();
                                    otItem.Status.IdItemOtStatus = otItem.IdItemOtStatus;
                                    otItem.Status.Name = Convert.ToString(otItemReader["Status"]);
                                
                                }
                                
                                if (otItemReader["IdDrawing"] != DBNull.Value)
                                {
                                    otItem.RevisionItem.IdDrawing = Convert.ToInt64(otItemReader["IdDrawing"]);
                                    otItem.RevisionItem.DrawingPath = Convert.ToString(otItemReader["DrawingPath"]);
                                    otItem.RevisionItem.SolidworksDrawingFileName = Convert.ToString(otItemReader["SolidworksDrawingFileName"]);
                                }
                              
                                if (otItemReader["AssignedTo"] != DBNull.Value)
                                {
                                    otItem.AssignedTo = Convert.ToInt32(otItemReader["AssignedTo"]);

                                    otItem.AssignedToUser = new People();
                                    otItem.AssignedToUser.Name = Convert.ToString(otItemReader["AssignedName"]);
                                    otItem.AssignedToUser.Surname = Convert.ToString(otItemReader["AssignedSurname"]);
                                }
                            }
                            otItem.RevisionItem.CPProduct.LstCPDetection = new List<CPDetection>();
                            otItem.LstDetection = new List<Detection>();
                            otItems.Add(otItem);




                            keyId++;

                            //i = otItem.KeyId + 1;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2640(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    if (otItemReader.NextResult())
                        while (otItemReader.Read())
                        {
                            try
                            {
                                if (otItemReader["CPProductID"] != DBNull.Value)
                                {
                                    if (otItems.Any(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])))
                                    {
                                        List<OtItem> lstotitems = otItems.Where(i => i.RevisionItem.IdProduct == Convert.ToInt64(otItemReader["CPProductID"])).ToList();

                                        CPDetection cpdetection = new CPDetection();
                                        cpdetection.CPProductID = Convert.ToInt64(otItemReader["CPProductID"]);
                                        if (otItemReader["DetectionID"] != DBNull.Value)
                                            cpdetection.DetectionID = Convert.ToInt32(otItemReader["DetectionID"]);
                                        if (otItemReader["Name"] != DBNull.Value)
                                            cpdetection.DetectionName = Convert.ToString(otItemReader["Name"]);
                                        if (otItemReader["NumDetections"] != DBNull.Value)
                                            cpdetection.NumDetections = Convert.ToInt32(otItemReader["NumDetections"]);

                                        lstotitems.ForEach(x => { x.RevisionItem.CPProduct.LstCPDetection.Add(cpdetection); });
                                        if (!otDetectionLst.Any(i => i.IdDetection == cpdetection.DetectionID))
                                        {
                                            Detection detection = new Detection();
                                            detection.IdDetection = Convert.ToUInt32(cpdetection.DetectionID);
                                            detection.Name = Convert.ToString(cpdetection.DetectionName);
                                            otDetectionLst.Add(detection);
                                        }
                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2640(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }
                    if (otItemReader.NextResult())
                        while (otItemReader.Read())
                        {
                            try
                            {
                                long idOTItems = Convert.ToInt64(otItemReader["idotitem"]);
                                OtItem oi = otItems.FirstOrDefault(x => x.IdOTItem == idOTItems);

                                FileDetail fd = new FileDetail();

                                if (oi.RevisionItem.Files == null)
                                    oi.RevisionItem.Files = new List<FileDetail>();

                                if (otItemReader["IdElectricalDiagram"] != DBNull.Value)
                                {
                                    fd.IdElectricalDiagram = Convert.ToInt64(otItemReader["IdElectricalDiagram"]);
                                }

                                if (otItemReader["FilePath"] != DBNull.Value)
                                {                                    
                                    fd.FilePath= otItemReader["FilePath"].ToString();
                                    fd.FileName = Path.GetFileNameWithoutExtension(fd.FilePath = otItemReader["FilePath"].ToString());
                                    fd.ReferenceName = Convert.ToString(otItemReader["Reference"]);
                                    fd.Description = Convert.ToString(otItemReader["Description"]);
                                    fd.Operation = OperationDb.Nothing;
                                    oi.RevisionItem.Files.Add(fd);
                                }
                            }

                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetStructureOtItemsByIdOt_V2640(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                                throw;
                            }
                        }

                }
            }
            otItems.ForEach(x => { x.LstDetection = otDetectionLst; });
            //Log4NetLogger.Logger.Log("Executed GetOtItemsByIdOt().", category: Category.Info, priority: Priority.Low);
            return otItems;
        }

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public Ots GetStructureWorkOrderByIdOt_V2640(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection connOT = new MySqlConnection(connectionString))
            {
                connOT.Open();

                MySqlCommand otCommand = new MySqlCommand("SAM_GetMaterialOtInformationByIdOt_V2170", connOT);
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
                            ot.LstDetection = new List<Detection>();
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

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["Status"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["StatusHtmlColor"]);
                                }

                            }
                         
                            ot.OtItems = GetStructureOtItemsByIdOt_V2640(connectionString, idOt);

                            if (ot.OtItems != null && ot.OtItems.Count > 0)
                                ot.LstDetection = ot.OtItems.FirstOrDefault().LstDetection;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetStructureWorkOrderByIdOt_V2640(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetStructureWorkOrderByIdOt_V2640().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public List<ElectricalDiagram> GetElectricalDiagram(string MainServerConnectionString, string ElectricalDiagramFilePath)
        {
            List<ElectricalDiagram> structureElectricalDiagram = new List<ElectricalDiagram>();
            List<int> IntList = new List<int>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("SAM_GetStructureElectricalDiagrams_V2640", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ElectricalDiagram electricalDiagrams = new ElectricalDiagram();
                            if (rdr["KeyName"] != DBNull.Value)
                                electricalDiagrams.Key = Convert.ToString(rdr["KeyName"]);
                            if (rdr["SubElectricalDiagramID"] != DBNull.Value)
                                electricalDiagrams.SubElectricalDiagramID = Convert.ToInt32(rdr["SubElectricalDiagramID"]);
                            if (rdr["ParentElectricalDiagramID"] != DBNull.Value)
                                electricalDiagrams.ParentElectricalDiagramID = Convert.ToInt32(rdr["ParentElectricalDiagramID"]);
                            if (rdr["Parent_Category"] != DBNull.Value)
                                electricalDiagrams.Parent = Convert.ToString(rdr["Parent_Category"]);
                            if (rdr["Name"] != DBNull.Value)
                                electricalDiagrams.Name = Convert.ToString(rdr["Name"]);
                            if (rdr["Description"] != DBNull.Value)
                                electricalDiagrams.Description = Convert.ToString(rdr["Description"]);
                            if (rdr["Reference"] != DBNull.Value)
                                electricalDiagrams.Reference = Convert.ToString(rdr["Reference"]);
                            if (rdr["Revision"] != DBNull.Value)
                                electricalDiagrams.Revision = Convert.ToInt32(rdr["Revision"]);
                            if (rdr["IsObsolete"] != DBNull.Value)
                                electricalDiagrams.IsObsolete = Convert.ToInt32(rdr["IsObsolete"]);
                            if (rdr["PCBAllowed"] != DBNull.Value)
                                electricalDiagrams.PCBAllowed = Convert.ToInt32(rdr["PCBAllowed"]);
                            if (rdr["ReviewedIn"] != DBNull.Value)
                                electricalDiagrams.ReviewedIn = Convert.ToDateTime(rdr["ReviewedIn"]);
                            if (rdr["FilePath"] != DBNull.Value)
                                electricalDiagrams.FilePath = Convert.ToString(rdr["FilePath"]);

                            structureElectricalDiagram.Add(electricalDiagrams);
                        }
                        
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetElectricalDiagram().  ErrorMessage- {1}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return structureElectricalDiagram;
        }
      
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        public byte[] GetElectricalDiagramsFileImageInBytes(string ElectricalDiagramFileName, string sAMElectricalDiagramsFilePath)
        {
            byte[] fileBytes = null;
            try
            {
                
                string filePath = Path.Combine(sAMElectricalDiagramsFilePath,ElectricalDiagramFileName.TrimStart('\\'));
                if (File.Exists(filePath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        fileBytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(fileBytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetElectricalDiagramsFileImageInBytes(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return fileBytes;
        }

        // [GEOS2-6728][pallavi.kale][14-04-2025]
		// [nsatpute][13-05-2025][GEOS2-6728]
        public bool AddEditDeleteElectricalDiagramForIdDrawing(string mainServerConnectionString, string localServerConnectionString, List<OtItem> otItems)
        {
            bool status = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (OtItem oi in otItems)
                        {
                            if (oi.RevisionItem != null && oi.RevisionItem.Files != null)
                            {
                                foreach (var item in oi.RevisionItem.Files) 
                                {
                                    if (item.Operation == OperationDb.New)
                                    {
                                        using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_AddElectricalDiagrams_V2640", mySqlConnection))
                                        {
                                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                                            mySqlCommand.Parameters.AddWithValue("_IdElectricalDiagram", item.IdElectricalDiagram);
                                            mySqlCommand.Parameters.AddWithValue("_IdDrawing", oi.RevisionItem.IdDrawing);
                                            mySqlCommand.ExecuteNonQuery();
                                        }
                                    }
                                    else if(item.Operation == OperationDb.Delete)
                                    {
                                        using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_DeleteElectricalDiagrams_V2640", mySqlConnection))
                                        {
                                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                                            mySqlCommand.Parameters.AddWithValue("_IdElectricalDiagram", item.IdElectricalDiagram);
                                            mySqlCommand.Parameters.AddWithValue("_IdDrawing", oi.RevisionItem.IdDrawing);
                                            mySqlCommand.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                        mySqlConnection.Close();
                        status = true;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in AddEditDeleteElectricalDiagramForIdDrawing(). ErrorMessage - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    throw;
                }
            }
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(localServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (OtItem oi in otItems)
                        {
                            if (oi.RevisionItem != null && oi.RevisionItem.Files != null)
                            {
                                foreach (var item in oi.RevisionItem.Files)
                                {
                                    if (item.Operation == OperationDb.New)
                                    {
                                        using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_AddElectricalDiagramsChangeLog_V2640", mySqlConnection))
                                        {
                                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                                            mySqlCommand.Parameters.AddWithValue("_IdRevisionItem", oi.IdRevisionItem);
                                            mySqlCommand.Parameters.AddWithValue("_IdUser", item.IdUser);
                                            mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                            mySqlCommand.Parameters.AddWithValue("_Text", item.Comments);
                                            mySqlCommand.ExecuteNonQuery();
                                        }
                                    }else if(item.Operation == OperationDb.Delete)
                                    {
                                        using (MySqlCommand mySqlCommand = new MySqlCommand("SAM_DeleteElectricalDiagramsChangeLog_V2640", mySqlConnection))
                                        {
                                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                                            mySqlCommand.Parameters.AddWithValue("_IdRevisionItem", oi.IdRevisionItem);
                                            mySqlCommand.Parameters.AddWithValue("_IdUser", item.IdUser);
                                            mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                            mySqlCommand.Parameters.AddWithValue("_Text", item.Comments);
                                            mySqlCommand.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                        mySqlConnection.Close();
                        status = true;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error in AddElectricalDiagramForIdDrawing(). ErrorMessage - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    throw;
                }
            }

            return status;
        }
        //[nsatpute][12.08.2025][GEOS2-9163]
        public List<Ots> GetPendingWorkorders_V2660(Company company, string userProfileImageFilePath, string countryIconFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<Ots> ots = new List<Ots>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            DataSet dataSet = new DataSet();

            List<string> countryISOs = new List<string>();

            using (MySqlConnection connOts = new MySqlConnection(connectionString))
            {
                connOts.Open();

                MySqlCommand otsCommand = new MySqlCommand("SAM_GetPendingWorkorders_V2660", connOts);
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
                            if (otReader["DeliveryWeek"] != DBNull.Value)
                            {
                                ot.DeliveryWeek = Convert.ToString(otReader["DeliveryWeek"]);
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
                                ot.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.OfferCode = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.IdOffer = Convert.ToInt64(otReader["IdOffer"]);
                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.IdOffer = ot.Quotation.IdOffer;

                                if (otReader["IsCritical"] != DBNull.Value)
                                    ot.Quotation.Offer.IsCritical = Convert.ToByte(otReader["IsCritical"]);

                                if (otReader["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus = new Common.SRM.WorkflowStatus();
                                    ot.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(otReader["IdWorkflowStatus"]);
                                    ot.WorkflowStatus.Name = Convert.ToString(otReader["WorkFlowStatus"]);
                                    ot.WorkflowStatus.HtmlColor = Convert.ToString(otReader["WorkFlowStatusHtmlColor"]);
                                }

                                ot.Quotation.Offer.Code = Convert.ToString(otReader["OfferCode"]);
                                ot.Quotation.Offer.Description = Convert.ToString(otReader["Description"]);


                                if (otReader["IdOfferType"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdOfferType = Convert.ToByte(otReader["IdOfferType"]);
                                    ot.Quotation.Offer.OfferType = new OfferType();
                                    ot.Quotation.Offer.OfferType.IdOfferType = Convert.ToByte(ot.Quotation.Offer.IdOfferType);
                                    ot.Quotation.Offer.OfferType.Name = Convert.ToString(otReader["OfferType"]);
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
                                    ot.Quotation.Site.Country.Iso = Convert.ToString(otReader["Iso"]);
                                    if (!countryISOs.Any(co => co.ToString() == ot.Quotation.Site.Country.Iso))
                                    {
                                        countryISOs.Add(ot.Quotation.Site.Country.Iso);
                                    }
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
                                    }
                                }
                            }

                            if (otReader["Observations"] != DBNull.Value)
                                ot.Observations = Convert.ToString(otReader["Observations"]);
                            try
                            {
                                if (otReader["EndTime"] != DBNull.Value)
                                    ot.EndDateTime = Convert.ToDateTime(otReader["EndTime"]);

                                if (otReader["EndTimeFlag"] != DBNull.Value)
                                    ot.EndTimeFlag = Convert.ToInt32(otReader["EndTimeFlag"]);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() . ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            ot.Quotation.Offer.OptionsByOffersGrid = new List<OptionsByOfferGrid>();
                            ots.Add(ot);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                foreach (string item in countryISOs)
                {
                    byte[] bytes = null;
                    bytes = GetCountryIconFileInBytes(item, countryIconFilePath);
                    ots.Where(ot => ot.Quotation.Site.Country.Iso == item).ToList().ForEach(ot => ot.Quotation.Site.Country.CountryIconBytes = bytes);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    foreach (var item in otWorkingTimes.GroupBy(owt => owt.IdOT))

                    {

                        if (ots.Any(ot => ot.IdOT == item.Key))
                        {
                            Ots otupdate = ots.FirstOrDefault(ot => ot.IdOT == item.Key);

                            otupdate.RealStartDate = item.ToList().Min(i => i.StartTime);
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
                        if (OptionsByOffers != null && OptionsByOffers.Count > 0)
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingWorkorders_V2500() 1. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

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

            Log4NetLogger.Logger.Log("Executed GetPendingWorkorders_V2500().", category: Category.Info, priority: Priority.Low);
            return ots;
        }
        // [Rahul.Gadhave][GEOS2-8713][Date:03/11/2025]
        public List<Company> GetAllCompaniesDetails_V2680(Int32 idUser, string connectionString)
        {
            List<Company> connectionstrings = new List<Company>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand command = new MySqlCommand("SAM_GetAllPlantsDetails_V2680", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("_idUser", idUser);
                List<string> strDatabaseIP = new List<string>();
                // Parse the connection string
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);
                // Extract components
                string ConnecteddataSource = builder.Server;
                string database = builder.Database;
                string userId = builder.UserID;
                string password = builder.Password;
                string connectedPlantName = GetConnectedPlantNameFromDataSource(ConnecteddataSource.ToUpper(), connectionString);
                string dataSource = "";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        if (string.IsNullOrEmpty(connectedPlantName))
                        {
                            dataSource = ConnecteddataSource.Replace(ConnecteddataSource, reader["DatabaseIP"].ToString());
                        }
                        else
                        {
                            dataSource = ConnecteddataSource.Replace(connectedPlantName, reader["ShortName"].ToString());
                            //This will removed from code when we update everywhere logic to hit service through service provider url not from database connection string
                        }
                        string connstr = connectionString.Replace(ConnecteddataSource, dataSource);
                        if (connectionstrings.Count == 0)
                        {
                            strDatabaseIP.Add(reader["DatabaseIP"].ToString());
                            company.IdCompany = Convert.ToInt32(reader["idSite"].ToString());
                            company.Alias = reader["ShortName"].ToString();
                            company.ShortName = reader["ShortName"].ToString();
                            company.Name = reader["Name"].ToString();
                            company.ConnectPlantId = reader["idSite"].ToString();
                            company.ConnectPlantConstr = connstr;
                            company.ServiceProviderUrl = reader["ServiceProviderUrl"].ToString();
                            company.Country = new Country();
                            company.Country.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                            company.Country.Name = reader["name"].ToString();
                            connectionstrings.Add(company);
                        }
                        if (strDatabaseIP.Exists(cs => cs.ToString() == reader["DatabaseIP"].ToString()))
                        {
                        }
                        else
                        {
                            strDatabaseIP.Add(reader["DatabaseIP"].ToString());
                            company.IdCompany = Convert.ToInt32(reader["idSite"].ToString());
                            company.Alias = reader["ShortName"].ToString();
                            company.ShortName = reader["ShortName"].ToString();
                            company.Name = reader["Name"].ToString();
                            company.Country = new Country();
                            company.Country.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                            company.Country.Name = reader["name"].ToString();
                            company.ConnectPlantId = reader["idSite"].ToString();
                            company.ConnectPlantConstr = connstr;
                            company.ServiceProviderUrl = reader["ServiceProviderUrl"].ToString();
                            connectionstrings.Add(company);
                        }
                    }
                }
            }
            return connectionstrings;
        }
        private string GetConnectedPlantNameFromDataSource(string datasource, string connectionString)
        {
            string connectedPlantName = "";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand("CRM_GetAllPlantNameToCheckDataSource", conn);
                command.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader itemRow = command.ExecuteReader())
                {
                    if (itemRow.HasRows)
                    {
                        while (itemRow.Read())
                        {
                            string plantShortName = itemRow["ShortName"].ToString();
                            if (datasource.Contains(plantShortName.ToUpper()))
                            {
                                connectedPlantName = plantShortName.ToUpper();
                            }
                        }
                    }
                }
            }
            return connectedPlantName;
        }
    }
}
