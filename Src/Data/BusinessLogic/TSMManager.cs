using DevExpress.XtraSpreadsheet.Model;
using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.TSM;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using MySql.Data.MySqlClient;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class TSMManager
    {
        //  [GEOS2-5388][pallavi.kale][13.01.2025]
        public TSMManager()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "log4net.config";
                    CreateIfNotExists(ApplicationLogFilePath);
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                    Log4NetLogger.Logger.Log(string.Format("TSMManager()..... Constructor Executed"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch
            {
                throw;
            }
        }

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

        //  [GEOS2-5388][pallavi.kale][22.01.2025]
        public List<TSMUsers> GetUserDetailsList_V2600(string connectionString)
        {
            List<TSMUsers> usersList = new List<TSMUsers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_GetUserDetails_V2600", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.CommandTimeout = 6000;

                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        #region MyRegion
                        while (rdr.Read())
                        {
                            TSMUsers users = new TSMUsers();
                            if (rdr["FullName"] != DBNull.Value)
                            {
                                users.FullName = Convert.ToString(rdr["FullName"]);
                            }
                            if (rdr["IdEmployee"] != DBNull.Value)
                            {
                                users.IdEmployee = Convert.ToUInt32(rdr["IdEmployee"]);
                            }
                            if (rdr["IdUser"] != DBNull.Value)
                            {
                                users.IdTechnicianUser = Convert.ToUInt32(rdr["IdUser"]);
                            }

                            if (rdr["EmployeeCode"] != DBNull.Value)
                            {
                                users.EmployeeCode = Convert.ToString(rdr["EmployeeCode"]);
                                users.EmployeeCodeWithIdGender = users.EmployeeCode + "_" + users.IdGender;
                            }
                            if (rdr["IdGender"] != DBNull.Value)
                            {
                                users.IdGender = Convert.ToUInt32(rdr["IdGender"]);

                                users.Gender = new LookupValue();
                                users.Gender.IdLookupValue = Convert.ToInt32(users.IdGender);
                                users.Gender.Value = Convert.ToString(rdr["Gender"]);
                            }
                            if (rdr["Organization"] != DBNull.Value)
                            {
                                users.Organization = Convert.ToString(rdr["Organization"]);
                            }
                            if (rdr["Country"] != DBNull.Value)
                            {
                                users.Country = Convert.ToString(rdr["Country"]);
                            }
                            if (rdr["Region"] != DBNull.Value)
                            {
                                users.Region = Convert.ToString(rdr["Region"]);
                            }
                            if (rdr["UserName"] != DBNull.Value)
                            {
                                users.UserName = Convert.ToString(rdr["UserName"]);
                            }
                            if (rdr["EngineeringCustomerApplication"] != DBNull.Value)
                            {
                                users.IdPermissions = new List<int>();
                                string permissions = Convert.ToString(rdr["EngineeringCustomerApplication"]);
                                if (!string.IsNullOrWhiteSpace(permissions)) 
                                {
                                    users.IdPermissions = permissions.Split(',')
                                                                     .Select(s => s.Trim()) 
                                                                     .Where(s => int.TryParse(s, out _)) 
                                                                     .Select(int.Parse) 
                                                                     .ToList();
                                }
                            }
                            usersList.Add(users);
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetUsersList_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return usersList;
        }
        //  [GEOS2-5388][pallavi.kale][22.01.2025]
        public List<LookupValue> GetLookupValues(string ConnectionString, byte key)
        {
            List<LookupValue> UnTrustedExtentionList = new List<LookupValue>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_GetlookupvaluesByIdLookupKey", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdLookupKey", key);
                    using (MySqlDataReader empReader = mySqlCommand.ExecuteReader())
                    {
                        while (empReader.Read())
                        {
                            LookupValue lookupval = new LookupValue();
                            try
                            {
                                if (empReader["IdLookupValue"] != DBNull.Value)
                                    lookupval.IdLookupValue = Convert.ToInt32(empReader["IdLookupValue"]);

                                if (empReader["Abbreviation"] != DBNull.Value)
                                    lookupval.Abbreviation = Convert.ToString(empReader["Abbreviation"]);

                                if (empReader["Value"] != DBNull.Value)
                                    lookupval.Value = empReader["Value"].ToString();
                                if (empReader["Position"] != DBNull.Value)
                                    lookupval.Position = Convert.ToInt32(empReader["Position"]);
                            }
                            catch (Exception ex)
                            { }
                            UnTrustedExtentionList.Add(lookupval);

                        }
                    }
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLookupValues(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return UnTrustedExtentionList;
        }
        //[nsatpute][GEOS2-5388][30-01-2025]
        public bool UpdateUserPermissions(TSMUsers user, List<LookupValue> permissions, string ConnectionString)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();

                    // Begin a transaction
                    using (MySqlTransaction transaction = mySqlConnection.BeginTransaction())
                    {
                        try
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("TSM_UpdateUserPermissions_V2600", mySqlConnection, transaction);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            foreach (LookupValue permission in permissions)
                            {
                                mySqlCommand.Parameters.Clear();
                                mySqlCommand.Parameters.AddWithValue("_IdTechnicianUser", user.IdTechnicianUser);

                                if (user.IdPermissions != null && user.IdPermissions.Contains(permission.IdLookupValue))
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerApplication", permission.IdLookupValue);
                                    mySqlCommand.Parameters.AddWithValue("_RecordToInsert", 1); // Insert
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerApplication", permission.IdLookupValue);
                                    mySqlCommand.Parameters.AddWithValue("_RecordToInsert", 0); // Delete
                                }

                                mySqlCommand.ExecuteNonQuery();
                            }

                            // Commit transaction if all commands succeed
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            // Rollback in case of an error
                            transaction.Rollback();
                            Log4NetLogger.Logger.Log(
                                   $"Error UpdateUserPermissions(). ErrorMessage- {e.Message}",
                                   category: Category.Exception,
                                   priority: Priority.Low
                               );
                            throw;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(
                    $"Error UpdateUserPermissions(). ErrorMessage- {ex.Message}",
                    category: Category.Exception,
                    priority: Priority.Low
                );
                throw;
            }
        }
       
        //[GEOS2-6993][pallavi.kale][26.02.2025]
        public List<TSMUsers> GetUserDetailsList_V2610(string connectionString)
        {
            List<TSMUsers> usersList = new List<TSMUsers>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_GetUserDetails_V2610", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.CommandTimeout = 6000;

                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        #region MyRegion
                        while (rdr.Read())
                        {
                            TSMUsers users = new TSMUsers();
                            if (rdr["FullName"] != DBNull.Value)
                            {
                                users.FullName = Convert.ToString(rdr["FullName"]);
                            }
                            if (rdr["IdEmployee"] != DBNull.Value)
                            {
                                users.IdEmployee = Convert.ToUInt32(rdr["IdEmployee"]);
                            }
                            if (rdr["IdUser"] != DBNull.Value)
                            {
                                users.IdTechnicianUser = Convert.ToUInt32(rdr["IdUser"]);
                            }

                            if (rdr["EmployeeCode"] != DBNull.Value)
                            {
                                users.EmployeeCode = Convert.ToString(rdr["EmployeeCode"]);
                                users.EmployeeCodeWithIdGender = users.EmployeeCode + "_" + users.IdGender;
                            }
                            if (rdr["IdGender"] != DBNull.Value)
                            {
                                users.IdGender = Convert.ToUInt32(rdr["IdGender"]);

                                users.Gender = new LookupValue();
                                users.Gender.IdLookupValue = Convert.ToInt32(users.IdGender);
                                users.Gender.Value = Convert.ToString(rdr["Gender"]);
                            }
                            if (rdr["Organization"] != DBNull.Value)
                            {
                                users.Organization = Convert.ToString(rdr["Organization"]);
                            }
                            if (rdr["Country"] != DBNull.Value)
                            {
                                users.CountryObj = new Country();
                                users.CountryObj.Name = Convert.ToString(rdr["Country"]);
                                users.CountryObj.Iso = Convert.ToString(rdr["Iso"]);
                                users.CountryObj.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + users.CountryObj.Iso + ".png";
                            }
                            if (rdr["Region"] != DBNull.Value)
                            {
                                users.Region = Convert.ToString(rdr["Region"]);
                            }
                            if (rdr["UserName"] != DBNull.Value)
                            {
                                users.UserName = Convert.ToString(rdr["UserName"]);
                            }
                            if (rdr["EngineeringCustomerApplication"] != DBNull.Value)
                            {
                                users.IdPermissions = new List<int>();
                                string permissions = Convert.ToString(rdr["EngineeringCustomerApplication"]);
                                if (!string.IsNullOrWhiteSpace(permissions))
                                {
                                    users.IdPermissions = permissions.Split(',')
                                                                     .Select(s => s.Trim())
                                                                     .Where(s => int.TryParse(s, out _))
                                                                     .Select(int.Parse)
                                                                     .ToList();
                                }
                            }
                            usersList.Add(users);
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetUsersList_V2600(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return usersList;
        }
        
        //[GEOS2-8963][pallavi.kale][28.11.2025]
        public List<Company> GetAllCompaniesDetails_V2690(Int32 idUser, string connectionString)
        {
            List<Company> connectionstrings = new List<Company>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand command = new MySqlCommand("TSM_GetAllPlantsDetails_V2690", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idUser", idUser);
                    List<string> strDatabaseIP = new List<string>();
                    MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);
                    string ConnecteddataSource = builder.Server;
                    string database = builder.Database;
                    string userId = builder.UserID;
                    string password = builder.Password;
                    string connectedPlantName = GetConnectedPlantNameFromDataSource_V2690(ConnecteddataSource.ToUpper(), connectionString);
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
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCompaniesDetails_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;

            }
           
            return connectionstrings;
        }
        
        //[GEOS2-8963][pallavi.kale][28.11.2025]
        private string GetConnectedPlantNameFromDataSource_V2690(string datasource, string connectionString)
        {
            string connectedPlantName = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("TSM_GetAllPlantNameToCheckDataSource_V2690", conn);
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
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetConnectedPlantNameFromDataSource_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
          
            return connectedPlantName;
        }
        
        //[GEOS2-8963][pallavi.kale][28.11.2025]
        public ObservableCollection<Ots> GetPendingOrdersByPlant_V2690(Company company, string connectionString)
        {
            ObservableCollection<Ots> ots = new ObservableCollection<Ots>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_PendingOrder_V2690", con);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdSite", company.IdCompany);
                    mySqlCommand.CommandTimeout = 8000;
                    using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Ots ot = new Ots();
                            ot.Quotation = new Quotation
                            {
                                Offer = new Offer
                                {
                                    BusinessUnit = new LookupValue(),
                                    CarOEM = new CarOEM(),
                                    CarProject = new CarProject(),
                                    Currency= new Currency()
                                },
                                Site = new Company
                                {
                                    Customer = new Customer(),
                                    Country = new Country()
                                }
                            };

                            ot.TSMWorkflowStatus = new TSMWorkflowStatus();
                            ot.IdOffer = Convert.ToInt32(rdr["IdOffer"]);
                            ot.IdOT = Convert.ToInt32(rdr["IdOT"]);
                            if (rdr["OtCode"] != DBNull.Value)
                            {
                                ot.Code = Convert.ToString(rdr["OtCode"]);
                                ot.MergeCode = Convert.ToString(rdr["MergeCode"]);
                            }
                            if (rdr["OrderCode"] != DBNull.Value)
                            {
                                ot.OfferCode = Convert.ToString(rdr["OrderCode"]);
                            }
                            if (rdr["Description"] != DBNull.Value)
                            {
                               
                                ot.Quotation.Offer.Description = Convert.ToString(rdr["Description"]);
                            }
                            if (rdr["Group"] != DBNull.Value)
                            {
                                ot.Quotation.Site.Customer.CustomerName = Convert.ToString(rdr["Group"]);
                            }
                            if (rdr["Plant"] != DBNull.Value)
                            {
                                ot.Quotation.Site.Name = Convert.ToString(rdr["Plant"]);
                            }
                            if (rdr["Country"] != DBNull.Value)
                            {
                                ot.Quotation.Site.Country.Name = Convert.ToString(rdr["Country"]);
                                ot.Quotation.Site.Country.Iso = Convert.ToString(rdr["Iso"]);
                                ot.Quotation.Site.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + ot.Quotation.Site.Country.Iso + ".png";
                            }
                            if (rdr["BusinessUnit"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.BusinessUnit.Value = Convert.ToString(rdr["BusinessUnit"]);
                            }
                            if (rdr["Category1"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.Category1 = Convert.ToString(rdr["Category1"]);
                            }
                            if (rdr["Category2"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.Category2 = Convert.ToString(rdr["Category2"]);
                            }
                            if (rdr["RFQ"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.Rfq = Convert.ToString(rdr["RFQ"]);
                            }
                            if (rdr["OEM"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.CarOEM.Name = Convert.ToString(rdr["OEM"]);
                            }
                            if (rdr["Project"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.CarProject.Name = Convert.ToString(rdr["Project"]);
                            }
                            if (rdr["Price"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.OtPrice = Convert.ToDouble(rdr["Price"]);
                            }
                            if (rdr["Symbol"] != DBNull.Value)
                            {
                                ot.Quotation.Offer.Currency.Symbol = Convert.ToString(rdr["Symbol"]);
                            }
                            if (rdr["DeliveryDate"] != DBNull.Value)
                            {
                                ot.DeliveryDate = Convert.ToDateTime(rdr["DeliveryDate"]);
                            }
                            if (rdr["Assignee"] != DBNull.Value)
                            {
                                ot.Assignee = Convert.ToString(rdr["Assignee"]);
                            }
                            if (rdr["Status"] != DBNull.Value)
                            {
                                ot.TSMWorkflowStatus.Name = Convert.ToString(rdr["Status"]);
                            }
                            if (rdr["HtmlColor"] != DBNull.Value)
                            {
                                ot.TSMWorkflowStatus.HtmlColor = Convert.ToString(rdr["HtmlColor"]);
                            }
                            if (rdr["Services"] != DBNull.Value)
                            {
                                ot.Services= Convert.ToInt32(rdr["Services"]);
                            }
                            ots.Add(ot);
                        }

                    }
                }
            
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPendingOrdersByPlant_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return ots;
        }

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public Ots GetWorkOrderByIdOt_V2690(Int64 idOt, Company company)
        {
            string connectionString = company.ConnectPlantConstr;
            Ots ot = null;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("TSM_WorkOrderInformationByIdOt_V2690", con);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdOt", idOt);
                mySqlCommand.CommandTimeout = 3000;
                using (MySqlDataReader rdr = mySqlCommand.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        try
                        {
                            ot = new Ots();
                            ot.IdOT = idOt;

                            if (rdr["OtCode"] != DBNull.Value)
                                ot.Code = Convert.ToString(rdr["OtCode"]);

                            ot.NumOT = Convert.ToByte(rdr["NumOT"]);

                            if (rdr["Comments"] != DBNull.Value)
                                ot.Comments = Convert.ToString(rdr["Comments"]);

                            if (rdr["CreatedBy"] != DBNull.Value)
                            {
                                ot.CreatedBy = Convert.ToInt32(rdr["CreatedBy"]);

                                ot.CreatedByPerson = new People();
                                ot.CreatedByPerson.IdPerson = ot.CreatedBy;
                                ot.CreatedByPerson.Name = Convert.ToString(rdr["CreatedByName"]);
                                ot.CreatedByPerson.Surname = Convert.ToString(rdr["CreatedBySurname"]);
                            }

                            ot.ReviewedBy = Convert.ToInt32(rdr["ReviewedBy"]);

                            //CreatedBy
                            if (rdr["CreatedBy"] != DBNull.Value)
                            {
                                ot.CreatedBy = Convert.ToInt32(rdr["CreatedBy"]);

                                ot.CreatedByPerson = new People();
                                ot.CreatedByPerson.IdPerson = ot.CreatedBy;
                                ot.CreatedByPerson.Name = Convert.ToString(rdr["CreatedByName"]);
                                ot.CreatedByPerson.Surname = Convert.ToString(rdr["CreatedBySurname"]);
                            }

                            if (rdr["CreationDate"] != DBNull.Value)
                                ot.CreationDate = Convert.ToDateTime(rdr["CreationDate"]);

                            //ModifiedBy
                            if (rdr["ModifiedBy"] != DBNull.Value)
                            {
                                ot.ModifiedBy = Convert.ToInt32(rdr["ModifiedBy"]);

                                ot.ModifiedByPerson = new People();
                                ot.ModifiedByPerson.IdPerson = ot.ModifiedBy;
                                ot.ModifiedByPerson.Name = Convert.ToString(rdr["ModifiedByName"]);
                                ot.ModifiedByPerson.Surname = Convert.ToString(rdr["ModifiedBySurname"]);
                            }

                            if (rdr["ModifiedIn"] != DBNull.Value)
                                ot.ModifiedIn = Convert.ToDateTime(rdr["ModifiedIn"]);

                            if (rdr["DeliveryDate"] != DBNull.Value)
                                ot.DeliveryDate = Convert.ToDateTime(rdr["DeliveryDate"]);

                            if (rdr["WareHouseLockSession"] != DBNull.Value)
                                ot.WareHouseLockSession = Convert.ToString(rdr["WareHouseLockSession"]);

                            if (rdr["AttachedFiles"] != DBNull.Value)
                                ot.AttachedFiles = Convert.ToString(rdr["AttachedFiles"]);

                            if (rdr["IdShippingAddress"] != DBNull.Value)
                                ot.IdShippingAddress = Convert.ToInt64(rdr["IdShippingAddress"]);

                            ot.IdQuotation = Convert.ToInt64(rdr["IdQuotation"]);
                            ot.Quotation = new Quotation();
                            ot.Quotation.Code = Convert.ToString(rdr["QuotationCode"]);

                            if (rdr["Year"] != DBNull.Value)
                                ot.Quotation.Year = Convert.ToInt32(rdr["Year"]);

                            if (rdr["Description"] != DBNull.Value)
                                ot.Quotation.Description = Convert.ToString(rdr["Description"]);

                            if (rdr["ProjectName"] != DBNull.Value)
                                ot.Quotation.ProjectName = Convert.ToString(rdr["ProjectName"]);


                            if (rdr["ProjectName"] != DBNull.Value)
                                ot.Quotation.ProjectName = Convert.ToString(rdr["ProjectName"]);

                            //Customer
                            if (rdr["IdSite"] != DBNull.Value)
                            {
                                ot.Quotation.Site = new Company();
                                ot.Quotation.Site.IdCompany = Convert.ToInt32(rdr["IdSite"]);
                                ot.Quotation.Site.SiteNameWithoutCountry = Convert.ToString(rdr["SiteName"]);

                                if (rdr["ShortName"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(rdr["ShortName"])))
                                {
                                    ot.Quotation.Site.Name = Convert.ToString(rdr["ShortName"]);
                                }
                                else
                                {
                                    ot.Quotation.Site.Name = Convert.ToString(rdr["Customer"]);
                                }

                                if (rdr["idCountry"] != DBNull.Value)
                                {
                                    ot.Quotation.Site.IdCountry = Convert.ToByte(rdr["idCountry"]);
                                    ot.Quotation.Site.Country = new Country();
                                    ot.Quotation.Site.Country.IdCountry = Convert.ToByte(rdr["idCountry"]);
                                    ot.Quotation.Site.Country.Name = rdr["CountryName"].ToString();
                                    ot.Quotation.Site.Country.Iso = rdr["iso"].ToString();

                                    if (rdr["EuroZone"] != DBNull.Value)
                                    {
                                        ot.Quotation.Site.Country.EuroZone = Convert.ToSByte(rdr["EuroZone"]);
                                    }

                                    if (rdr["IdCountryGroup"] != DBNull.Value)
                                    {
                                        ot.Quotation.Site.CountryGroup = new CountryGroup();
                                        ot.Quotation.Site.CountryGroup.IdCountryGroup = Convert.ToInt64(rdr["IdCountryGroup"]);

                                        if (rdr["CountryGroup"] != DBNull.Value)
                                            ot.Quotation.Site.CountryGroup.Name = Convert.ToString(rdr["CountryGroup"]);

                                        if (rdr["IsFreeTrade"] != DBNull.Value)
                                            ot.Quotation.Site.CountryGroup.IsFreeTrade = Convert.ToByte(rdr["IsFreeTrade"]);
                                    }
                                }

                                ot.Quotation.Site.Customer = new Customer();
                                ot.Quotation.Site.Customer.IdCustomer = Convert.ToInt32(rdr["IdCustomer"]);
                                ot.Quotation.Site.Customer.CustomerName = Convert.ToString(rdr["CustomerName"]);
                            }

                            //Detections Template
                            if (rdr["IdTemplate"] != DBNull.Value)
                            {
                                ot.Quotation.Template = new Template();
                                ot.Quotation.Template.IdTemplate = Convert.ToByte(rdr["IdTemplate"]);
                                ot.Quotation.Template.Name = Convert.ToString(rdr["Template"]);
                                ot.Quotation.Template.Type = Convert.ToInt64(rdr["IdTemplateType"]);
                            }

                            if (rdr["IdOffer"] != DBNull.Value)
                            {
                                ot.Quotation.IdOffer = Convert.ToInt32(rdr["IdOffer"]);

                                ot.Quotation.Offer = new Offer();
                                ot.Quotation.Offer.Description = Convert.ToString(rdr["Description"]);

                                if (rdr["IdCarriageMethod"] != DBNull.Value)
                                {
                                    ot.Quotation.Offer.IdCarriageMethod = Convert.ToByte(rdr["IdCarriageMethod"]);
                                    LookupValue carriage = new LookupValue();
                                    carriage.IdLookupValue = Convert.ToInt32(ot.Quotation.Offer.IdCarriageMethod);
                                    carriage.Value = Convert.ToString(rdr["CarriageValue"]);
                                    carriage.IdImage = Convert.ToInt64(rdr["CarriageImage"]);

                                    ot.Quotation.Offer.CarriageMethod = carriage;
                                }

                                if (rdr["PODate"] != DBNull.Value)
                                    ot.Quotation.Offer.POReceivedInDate = Convert.ToDateTime(rdr["PODate"]);

                                if (rdr["ProducerIdCountryGroup"] != DBNull.Value)
                                {
                                    ot.ProducerIdCountryGroup = Convert.ToInt64(rdr["ProducerIdCountryGroup"]);
                                    ot.CountryGroup = new CountryGroup();
                                    ot.CountryGroup.IdCountryGroup = Convert.ToInt64(rdr["ProducerIdCountryGroup"]);
                                    ot.CountryGroup.Name = Convert.ToString(rdr["ProducerCountryGroup"]);
                                    ot.CountryGroup.HtmlColor = Convert.ToString(rdr["ProducerCountryGroupColor"]);
                                }

                                if (rdr["IdWorkflowStatus"] != DBNull.Value)
                                {
                                    ot.IdWorkflowStatus = Convert.ToByte(rdr["IdWorkflowStatus"]);
                                    ot.TSMWorkflowStatus = new TSMWorkflowStatus();
                                    ot.TSMWorkflowStatus.IdWorkflowStatus = Convert.ToByte(rdr["IdWorkflowStatus"]);
                                    ot.TSMWorkflowStatus.Name = Convert.ToString(rdr["Status"]);
                                    ot.TSMWorkflowStatus.HtmlColor = Convert.ToString(rdr["StatusHtmlColor"]);
                                }

                                if (rdr["Observations"] != DBNull.Value)
                                    ot.Observations = Convert.ToString(rdr["Observations"]);

                            }
                            ot.OtItems = GetOtItemsByIdOt_V2690(connectionString, idOt);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetWorkOrderByIdOt_V2690(idOt-{0}, idCompany-{1}). ErrorMessage- {2} ", idOt, company.IdCompany, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetWorkOrderByIdOt_V2180().", category: Category.Info, priority: Priority.Low);
            return ot;
        }

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public List<OtItem> GetOtItemsByIdOt_V2690(string connectionString, Int64 idOt)
        {
            List<OtItem> otItems = new List<OtItem>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("TSM_GetOtItemsByIdOt_V2690", connOtItem);
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
                                if (otItemReader["WorkflowStatus"] != DBNull.Value)
                                {
                                    otItem.WorkflowStatusName = Convert.ToString(otItemReader["WorkflowStatus"]);
                                    otItem.WorkflowStatusHtmlColor = Convert.ToString(otItemReader["WorkflowStatusColor"]);
                                    
                                }
                                if (otItemReader["AssignedTo"] != DBNull.Value)
                                {
                                    otItem.AssignedTo = Convert.ToInt32(otItemReader["AssignedTo"]);

                                    otItem.AssignedToUser = new People();
                                    otItem.AssignedToUser.Name = Convert.ToString(otItemReader["AssignedName"]);
                                    otItem.AssignedToUser.Surname = Convert.ToString(otItemReader["AssignedSurname"]);
                                }


                            }
                            List<OtItem> decomposedOtItems = GetArticlesForDecomposition_V2690(connectionString, idOt, otItem, ref keyId);
                            otItems.Add(otItem);
                            if (decomposedOtItems != null && decomposedOtItems.Count > 0)
                            {

                                otItems.AddRange(decomposedOtItems);

                            }
                            keyId++;
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetOtItemsByIdOt_V2690(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            return otItems;
        }
        
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public List<OtItem> GetArticlesForDecomposition_V2690(string connectionString, Int64 idOt, OtItem parentOtItem, ref int keyId)
        {
            List<OtItem> otItems = new List<OtItem>();

            using (MySqlConnection connOtItem = new MySqlConnection(connectionString))
            {
                connOtItem.Open();

                MySqlCommand otItemCommand = new MySqlCommand("TSM_GetArticlesForDecomposition_V2690", connOtItem);
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
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticlesForDecomposition_V2690(idOt-{0}). ErrorMessage- {1}", idOt, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                }
            }

            return otItems;
        }
        
        //[GEOS2-8973][pallavi.kale][28.11.2025]
        public bool UpdateOTAssignedUser_V2690(Company company, List<OTAssignedUser> otAssignedUsers, Int64 IdOT, string OldRemark, string Remark, int IdUser)
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
                                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_AddOTAssignedUser_V2690", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdOT", otAssignedUser.IdOT);
                                    mySqlCommand.Parameters.AddWithValue("_IdStage", otAssignedUser.IdStage);
                                    mySqlCommand.Parameters.AddWithValue("_IdUser", otAssignedUser.IdUser);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (otAssignedUser.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {

                                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_RemoveOTAssignedUser_V2690", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdOTAssignedUser", otAssignedUser.IdOTAssignedUser);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        mySqlConnection.Close();
                    }

                    if (OldRemark != Remark)
                        UpdateOTSObservation_V2690(connectionString, IdOT, Remark, IdUser);

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
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOTAssignedUser_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }
        
        //[GEOS2-8973][pallavi.kale][28.11.2025]
        public void UpdateOTSObservation_V2690(string MainServerConnectionString, Int64 IdOT, string Observations, int IdUser)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_OTS_UpdateObservation_V2690", mySqlConnection);
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
                Log4NetLogger.Logger.Log(string.Format("Error UpdateOTSObservation_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        
        //[GEOS2-8973][pallavi.kale][28.11.2025]
        public List<OTAssignedUser> GetUsersToAssignedOT_V2690(Company company, string userProfileImageFilePath, Int32 idCompany)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAssignedUser> otAssignedUsers = new List<OTAssignedUser>();

            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_GetUsersToAssignedOT_V2690", mySqlConnection);
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
                Log4NetLogger.Logger.Log(string.Format("Error GetUsersToAssignedOT_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            foreach (UserShortDetail itemUSD in userShortDetails)
            {
                byte[] UserImageInBytes = GetUserProfileImageInBytes_V2690(itemUSD.Login, userProfileImageFilePath);
                otAssignedUsers.Where(owt => owt.IdUser == itemUSD.IdUser).ToList().ForEach(x => x.UserShortDetail.UserImageInBytes = UserImageInBytes);
            }

            return otAssignedUsers;
        }

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        private byte[] GetUserProfileImageInBytes_V2690(string LoginName, string filePath)
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

        //[GEOS2-8973][pallavi.kale][28.11.2025]
        public List<OTAssignedUser> GetOTAssignedUsers_V2690(Company company, Int64 idOT, string userProfileImageFilePath)
        {
            string connectionString = company.ConnectPlantConstr;
            List<OTAssignedUser> otAssignedUsers = new List<OTAssignedUser>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {

                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_GetOTAssignedUsers_V2690", mySqlConnection);
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
                Log4NetLogger.Logger.Log(string.Format("Error GetOTAssignedUsers_V2690( IdOT-{0}). ErrorMessage- {1}", idOT, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            foreach (UserShortDetail itemUSD in userShortDetails)
            {
                byte[] UserImageInBytes = GetUserProfileImageInBytes_V2690(itemUSD.Login, userProfileImageFilePath);
                otAssignedUsers.Where(owt => owt.IdUser == itemUSD.IdUser).ToList().ForEach(x => x.UserShortDetail.UserImageInBytes = UserImageInBytes);
            }

            return otAssignedUsers;
        }

        //[GEOS2-8981][pallavi.kale][28.11.2025]
        public List<TSMWorkLogReport> GetOTWorkLogTimesByPeriodAndSite_V2690(DateTime FromDate, DateTime ToDate, int IdSite, Company plant)
        {
            string connectionString = plant.ConnectPlantConstr;

            List<TSMWorkLogReport> WorklogList = new List<TSMWorkLogReport>();
            List<UserShortDetail> userShortDetails = new List<UserShortDetail>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_GetOTWorkLogTimesByPeriodAndSite_V2690", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_FromDate", FromDate);
                    mySqlCommand.Parameters.AddWithValue("_ToDate", ToDate);
                    mySqlCommand.Parameters.AddWithValue("_IdSite", IdSite);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            TSMWorkLogReport worklog = new TSMWorkLogReport();
                            worklog.Ot = new Ots
                            {
                                Quotation = new Quotation
                                {
                                    Offer = new Offer(),
                                    Site = new Company
                                    {
                                        Customer = new Customer(),
                                        Country = new Country(),
                                        People = new People()
                                    }
                                }
                                
                            };
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
                                worklog.STime = Convert.ToDateTime(Convert.ToDateTime(mySqlDataReader["StartTime"]));

                            if (mySqlDataReader["EndTime"] != DBNull.Value)
                                worklog.ETime = Convert.ToDateTime(Convert.ToDateTime(mySqlDataReader["EndTime"]));

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
                                worklog.WorklogUser = new TSMWorklogUser
                                {
                                    People = new People()
                                };
                                worklog.WorklogUser.IdUser = Convert.ToInt32(mySqlDataReader["IdUser"]);
                                worklog.WorklogUser.FirstName = Convert.ToString(mySqlDataReader["Name"]);
                                worklog.WorklogUser.LastName = Convert.ToString(mySqlDataReader["Surname"]);
                                worklog.WorklogUser.People.EmployeeCode = Convert.ToString(mySqlDataReader["EmployeeCode"]);
                                worklog.WorklogUser.IdGender = Convert.ToInt32(mySqlDataReader["IdGender"]);
                                worklog.WorklogUser.EmployeeCodeWithIdGender = worklog.WorklogUser.People.EmployeeCode + "_" + worklog.WorklogUser.IdGender;
                            }

                            if (mySqlDataReader["IdSite"] != DBNull.Value)
                            {
                                worklog.IdSite = Convert.ToInt32(mySqlDataReader["IdSite"]);
                            }
                            if (mySqlDataReader["Description"] != DBNull.Value)
                            {
                                worklog.Description = Convert.ToString(mySqlDataReader["Description"]);
                            }
                            if (mySqlDataReader["OrderCode"] != DBNull.Value)
                            {
                                worklog.OfferCode = Convert.ToString(mySqlDataReader["OrderCode"]);
                            }
                            if (mySqlDataReader["Group"] != DBNull.Value)
                            {
                                worklog.Ot.Quotation.Site.Customer.CustomerName = Convert.ToString(mySqlDataReader["Group"]);
                            }
                            if (mySqlDataReader["Plant"] != DBNull.Value)
                            {
                                worklog.Ot.Quotation.Site.Name = Convert.ToString(mySqlDataReader["Plant"]);
                            }
                            if (mySqlDataReader["Country"] != DBNull.Value)
                            {
                                worklog.Ot.Quotation.Site.Country.Name = Convert.ToString(mySqlDataReader["Country"]);
                                worklog.Ot.Quotation.Site.Country.Iso = Convert.ToString(mySqlDataReader["Iso"]);
                                worklog.Ot.Quotation.Site.Country.CountryIconUrl = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Countries/" + worklog.Ot.Quotation.Site.Country.Iso + ".png";
                            }
                            if (mySqlDataReader["Operators"] != DBNull.Value)
                            {
                                worklog.Ot.Operators = Convert.ToString(mySqlDataReader["Operators"]);
                                worklog.Ot.UserShortDetails = new List<UserShortDetail>();

                                foreach (string itemIdUser in worklog.Ot.Operators.Split(','))
                                {
                                    if (!worklog.Ot.UserShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                    {
                                        UserShortDetail usd = new UserShortDetail();
                                        usd.IdUser = Convert.ToInt32(itemIdUser);

                                        worklog.Ot.UserShortDetails.Add(usd);
                                        if (!userShortDetails.Any(i => i.IdUser.ToString() == itemIdUser))
                                        {
                                            userShortDetails.Add(usd);
                                        }
                                    }
                                }
                            }

                            worklog.Site = plant;
                            WorklogList.Add(worklog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOTWorkLogTimesByPeriodAndSite_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return WorklogList;
        }
       
        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public bool UpdateWorkflowStatusInOT_V2690(string MainServerConnectionString, UInt64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<TSMLogEntriesByOT> LogEntriesByOTList)
        {
            bool status;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SAM_OTS_UpdateWorkflowStatus_V2690", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                        mySqlCommand.Parameters.AddWithValue("_IdWorkflowStatus", IdWorkflowStatus);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        status = true;
                    }
                    AddCommentsOrLogEntriesByOT_V2690(MainServerConnectionString, LogEntriesByOTList);
                    UpdateInOT_V2690(MainServerConnectionString, IdOT, IdUser);
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateWorkflowStatusInOT_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return status;
        }

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public void AddCommentsOrLogEntriesByOT_V2690(string MainServerConnectionString, List<TSMLogEntriesByOT> LogEntriesByOTList)
        {
            try
            {
                if (LogEntriesByOTList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (TSMLogEntriesByOT logEntriesByOT in LogEntriesByOTList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("TSM_log_entries_by_OTS_Insert_V2690", mySqlConnection);
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
                Log4NetLogger.Logger.Log(string.Format("Error AddCommentsOrLogEntriesByOT_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        //[GEOS2-8965][pallavi.kale][28.11.2025]
        public void UpdateInOT_V2690(string MainServerConnectionString, UInt64 IdOT, int IdUser)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("TSM_UpdateInOT_V2690", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdOT", IdOT);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateInOT_V2690(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
    }
}

