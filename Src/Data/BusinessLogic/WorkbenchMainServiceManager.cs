using System;
using System.Collections.Generic;
using System.Linq;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.BusinessLogic.Logging;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using Prism.Logging;
using Emdep.Geos.Data.Common.Epc;
using System.Transactions;
using ChinhDo.Transactions;
using System.Globalization;
using NodaTime;
using System.Windows;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
//using Emdep.Geos.Data.Common.SRM;
using System.Text;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class WorkbenchMainServiceManager
    {
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        double FinalArticleCostAvg;
        double FinalArticleCostValue;
        double FinalArticleAdditionalCost;
        Int32 IdCurrency;
        DateTime ExchangeRateDate;
        public WorkbenchMainServiceManager()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "log4net.config";
                    CreateIfNotExists(ApplicationLogFilePath);
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                    Log4NetLogger.Logger.Log(string.Format("WorkbenchMainServiceManager()..... Constructor Executed"), category: Category.Info, priority: Priority.Low);
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
        public bool IsConnectionStringNameExist_V2550(string Name)
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


        /// <summary>
        /// This method is used to insert ways, detections, options, spare parts
        /// </summary>
        /// <param name="detectionDetails">Get detection details.</param>
        /// <param name="DetectionAttachedDocPath">Get file path.</param>
        /// <param name="DetectionImagePath">Get image path.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public DetectionDetails AddDetection_V2550(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup_V2550(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_createdBy", detectionDetails.CreatedBy);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);
                        if (IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);
                        }

                        detectionDetails.IdDetections = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    if (detectionDetails.IdDetections > 0)
                    {
                        AddDetectionAttachedDocByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                        AddDetectionAttachedLinkByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                        AddDetectionImageByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                        AddCustomersRegionsByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerList, detectionDetails.IdDetectionType, detectionDetails.CreatedBy);
                        AddDetectionLogEntry_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetection(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return detectionDetails;
        }
        /// <summary>
        /// This method is used to Add/Update/Delete Detection group
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetectionType">Get Detection type.</param>
        /// <param name="DetectionGroupList">The list of group.</param>
        public UInt32 AddUpdateDeleteDetectionGroup_V2550(string MainServerConnectionString, UInt32 IdDetectionType, List<DetectionGroup> DetectionGroupList, DetectionOrderGroup DetectionOrderGroup)
        {
            UInt32 idGroup = 0;
            try
            {
                if (DetectionGroupList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (DetectionGroup detectionGroupList in DetectionGroupList)
                        {
                            if (detectionGroupList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand1 = new MySqlCommand("PCM_DetectionGroupsByGroup_Update", mySqlConnection);
                                mySqlCommand1.CommandType = CommandType.StoredProcedure;
                                mySqlCommand1.Parameters.AddWithValue("_IdGroup", detectionGroupList.IdGroup);
                                mySqlCommand1.ExecuteNonQuery();

                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Groups_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionGroupList.IdGroup);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (detectionGroupList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionGroupList.IdGroup > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Groups_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionGroupList.IdGroup);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                    mySqlCommand.Parameters.AddWithValue("_Name", detectionGroupList.Name);
                                    mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionGroupList.OrderNumber);
                                    mySqlCommand.Parameters.AddWithValue("_Name_es", detectionGroupList.Name_es);
                                    mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionGroupList.Name_fr);
                                    mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionGroupList.Name_pt);
                                    mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionGroupList.Name_ro);
                                    mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionGroupList.Name_zh);
                                    mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionGroupList.Name_ru);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionGroupList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_Description_es", detectionGroupList.Description_es);
                                    mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionGroupList.Description_fr);
                                    mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionGroupList.Description_pt);
                                    mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionGroupList.Description_ro);
                                    mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionGroupList.Description_zh);
                                    mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionGroupList.Description_ru);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionGroupList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (detectionGroupList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Groups_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_Name", detectionGroupList.Name);
                                mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionGroupList.OrderNumber);
                                mySqlCommand.Parameters.AddWithValue("_Name_es", detectionGroupList.Name_es);
                                mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionGroupList.Name_fr);
                                mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionGroupList.Name_pt);
                                mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionGroupList.Name_ro);
                                mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionGroupList.Name_zh);
                                mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionGroupList.Name_ru);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionGroupList.Description);
                                mySqlCommand.Parameters.AddWithValue("_Description_es", detectionGroupList.Description_es);
                                mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionGroupList.Description_fr);
                                mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionGroupList.Description_pt);
                                mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionGroupList.Description_ro);
                                mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionGroupList.Description_zh);
                                mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionGroupList.Description_ru);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionGroupList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                detectionGroupList.IdGroup = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (DetectionOrderGroup != null && DetectionOrderGroup.Name == detectionGroupList.Name)
                                {
                                    idGroup = detectionGroupList.IdGroup;
                                }
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionGroup_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return idGroup;
        }

        /// <summary>
        /// This method is used to insert files by Detection
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionAttachedDocList">Get File list details.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection Attached Doc Path.</param>
        public void AddDetectionAttachedDocByDetection_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedDoc> DetectionAttachedDocList, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedDoc detectionAttachedDocList in DetectionAttachedDocList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedDocList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);

                            detectionAttachedDocList.IdDetectionAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                            {
                                AddDetectionAttachedDocToPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionAttachedDocByDetection_V2550(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// This method is used to Add Detection Attached Doc to path
        /// </summary>
        /// <param name="DetectionAttachedDoc">Get Detection Attached Doc details.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection Attached Doc Path.</param>
        public bool AddDetectionAttachedDocToPath_V2550(DetectionAttachedDoc DetectionAttachedDoc, string DetectionAttachedDocPath)
        {
            if (DetectionAttachedDoc.DetectionAttachedDocInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionAttachedDocPath, DetectionAttachedDoc.IdDetectionAttachedDoc);
                    string filePath = completePath + "\\" + DetectionAttachedDoc.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, DetectionAttachedDoc.DetectionAttachedDocInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetectionAttachedDocToPath_V2550()- Filename - {0}. ErrorMessage- {1}", DetectionAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }
        /// <summary>
        /// This method is used to insert Detection Attached Link
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection ID.</param>
        /// <param name="DetectionAttachedLinkList">Get Detection Attached Link details.</param>
        public void AddDetectionAttachedLinkByDetection_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedLink> DetectionAttachedLinkList)
        {
            try
            {
                if (DetectionAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedLink detectionAttachedLinkList in DetectionAttachedLinkList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_Name", detectionAttachedLinkList.Name);
                            mySqlCommand.Parameters.AddWithValue("_Address", detectionAttachedLinkList.Address);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedLinkList.Description);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedLinkList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionAttachedLinkByDetection_V2550(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// This method is used to insert detection image
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="DetectionImageList">Get detection Image details.</param>
        /// <param name="DetectionImagePath">Get detection Image Path.</param>
        public void AddDetectionImageByDetection_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionImage> DetectionImageList, string DetectionImagePath)
        {
            try
            {
                if (DetectionImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionImage detectionImageList in DetectionImageList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionImageList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionImageList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionImageList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionImageList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_Position", detectionImageList.Position);

                            detectionImageList.IdDetectionImage = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (detectionImageList.IdDetectionImage > 0)
                            {
                                AddDetectionImageToPath_V2550(detectionImageList, DetectionImagePath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionImageByDetection_V2550(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// This method is used to insert customers and regions by detection
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="CustomerList">Get customer list details.</param>
        /// <param name="IdDetectiontype">Get detection type id.</param>
        public void AddCustomersRegionsByDetection_V2550(string MainServerConnectionString, UInt32 IdDetection, List<RegionsByCustomer> CustomerList, UInt32 IdDetectiontype, UInt32 IdCreator)
        {
            try
            {
                if (CustomerList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (RegionsByCustomer customerList in CustomerList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCustomersRegionsByDetection_V2550(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// This method is used to Add detection image to path
        /// </summary>
        /// <param name="detectionImage">Get detection Image details.</param>
        /// <param name="DetectionImagePath">Get detection Image Path.</param>
        public bool AddDetectionImageToPath_V2550(DetectionImage detectionImage, string DetectionImagePath)
        {
            if (detectionImage.DetectionImageInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionImagePath, detectionImage.IdDetectionImage);
                    string filePath = completePath + "\\" + detectionImage.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, detectionImage.DetectionImageInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetectionImageToPath_V2550()- Filename - {0}. ErrorMessage- {1}", detectionImage.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to insert detection log entry
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="IdDetectionType">Get detection type id.</param>
        /// <param name="LogList">Get log list details.</param>
        public void AddDetectionLogEntry_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionLogEntry> LogList, UInt32 IdDetectionType)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByDetection_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionLogEntry(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// This method is used to update ways, detections, options, spare parts
        /// </summary>
        /// <param name="detectionDetails">Get detection details.</param>
        /// <param name="DetectionAttachedDocPath">Get file path.</param>
        /// <param name="DetectionImagePath">Get image path.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool UpdateDetection_V2550(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup_V2550(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionDetails.IdDetections);
                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionDetails.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);

                        if (IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);
                        }

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }

                    //add/update/delete ways, detections, options, spare parts
                    AddUpdateDeleteDetectionFiles_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                    AddUpdateDeleteDetectionLinks_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                    AddUpdateDeleteDetectionImages_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                    AddDeleteCustomersRegionsByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerList, detectionDetails.IdDetectionType, detectionDetails.ModifiedBy);
                    AddDetectionLogEntry_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateDetection(). IdDetection- {0} Error- {1}", detectionDetails.IdDetections, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Detection Files
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionAttachedDocPath">Get file path.</param>
        /// <param name="DetectionAttachedDocList">The list of files.</param>
        public void AddUpdateDeleteDetectionFiles_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedDoc> DetectionAttachedDocList, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedDoc detectionAttachedDocList in DetectionAttachedDocList)
                        {
                            if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedDoc", detectionAttachedDocList.IdDetectionAttachedDoc);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteDetectionAttachedDocFromPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                            }
                            else if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedDoc", detectionAttachedDocList.IdDetectionAttachedDoc);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionAttachedDocList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddDetectionAttachedDocToPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                                }
                            }
                            else if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedDocList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);

                                detectionAttachedDocList.IdDetectionAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                                {
                                    AddDetectionAttachedDocToPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionFiles(). IdCatalogueItem- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// This method is to delete Detection Attached Doc from path
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="DetectionAttachedDoc">Get Detection Attached Doc details.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection Attached Doc Path.</param>
        public bool IsDeleteDetectionAttachedDocFromPath_V2550(DetectionAttachedDoc DetectionAttachedDoc, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocPath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionAttachedDocPath, DetectionAttachedDoc.IdDetectionAttachedDoc);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteDetectionAttachedDocFromPath()- Filename - {0}. ErrorMessage- {1}", DetectionAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Detection Links
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionAttachedLinkList">The list of links.</param>
        public void AddUpdateDeleteDetectionLinks_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedLink> DetectionAttachedLinkList)
        {
            try
            {
                if (DetectionAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedLink detectionAttachedLinkList in DetectionAttachedLinkList)
                        {
                            if (detectionAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedLink", detectionAttachedLinkList.IdDetectionAttachedLink);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (detectionAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionAttachedLinkList.IdDetectionAttachedLink > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedLink", detectionAttachedLinkList.IdDetectionAttachedLink);
                                    mySqlCommand.Parameters.AddWithValue("_Name", detectionAttachedLinkList.Name);
                                    mySqlCommand.Parameters.AddWithValue("_Address", detectionAttachedLinkList.Address);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedLinkList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionAttachedLinkList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (detectionAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_Name", detectionAttachedLinkList.Name);
                                mySqlCommand.Parameters.AddWithValue("_Address", detectionAttachedLinkList.Address);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedLinkList.Description);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedLinkList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionLinks(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        /// <summary>
        /// This method is used to Add/Update/Delete Detection Images
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionImagePath">Get image path.</param>
        /// <param name="DetectionImageList">The list of images.</param>
        public void AddUpdateDeleteDetectionImages_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionImage> DetectionImageList, string DetectionImagePath)
        {
            try
            {
                if (DetectionImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionImage detectionImageList in DetectionImageList)
                        {
                            if (detectionImageList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionImage", detectionImageList.IdDetectionImage);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteDetectionImageFromPath_V2550(detectionImageList, DetectionImagePath);
                            }
                            else if (detectionImageList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionImageList.IdDetectionImage > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionImage", detectionImageList.IdDetectionImage);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionImageList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionImageList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionImageList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionImageList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_Position", detectionImageList.Position);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddDetectionImageToPath_V2550(detectionImageList, DetectionImagePath);
                                }
                            }
                            else if (detectionImageList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionImageList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionImageList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionImageList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionImageList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_Position", detectionImageList.Position);

                                detectionImageList.IdDetectionImage = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (detectionImageList.IdDetectionImage > 0)
                                {
                                    AddDetectionImageToPath_V2550(detectionImageList, DetectionImagePath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionImages_V2550(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is to delete detection image from path
        /// </summary>
        /// <param name="DetectionImage">Get detection Image details.</param>
        /// <param name="DetectionImagePath">Get detection Image Path.</param>
        public bool IsDeleteDetectionImageFromPath_V2550(DetectionImage DetectionImage, string DetectionImagePath)
        {
            try
            {
                if (DetectionImagePath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionImagePath, DetectionImage.IdDetectionImage);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteDetectionImageFromPath_V2550()- Filename - {0}. ErrorMessage- {1}", DetectionImage.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }
        /// <summary>
        /// This method is used to Add/Delete customers and regions by detection
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="CustomersList">The list of customer.</param>
        /// <param name="IdDetectiontype">Get detection type id.</param>
        public void AddDeleteCustomersRegionsByDetection_V2550(string MainServerConnectionString, UInt32 IdDetection, List<RegionsByCustomer> CustomersList, UInt32 IdDetectiontype, UInt32 IdCreator)
        {
            try
            {
                if (CustomersList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (RegionsByCustomer customersList in CustomersList)
                        {
                            if (customersList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (customersList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                                mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteCustomersRegionsByDetection_V2550(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Delete product type
        /// </summary>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool IsDeletedProductType_V2550(UInt64 IdCPType, string MainServerConnectionString)
        {
            bool isDeleted = false;
            if (IdCPType > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_Delete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        isDeleted = true;
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsDeletedProductType_V2550(). IdProductType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }

        public bool IsUpdatedPCMArticleCategoryOrder_V2550(string MainServerConnectionString, List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier)
        {
            bool isupdate = false;
            try
            {
                if (pcmArticleCategoryList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategoryList)
                        {
                            if (pcmArticleCategory.IdPCMArticleCategory > 0)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_ArticleCategoryPosition_Update", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pcmArticleCategory.IdPCMArticleCategory);
                                mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                                if (pcmArticleCategory.Parent == 0 || pcmArticleCategory.Parent == null)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_Parent", null);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                                }
                                mySqlCommand.Parameters.AddWithValue("_IdModifier", IdModifier);
                                mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                        isupdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsUpdatedPCMArticleCategoryOrder_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isupdate;
        }
        public bool IsDeletePCMArticleCategory_V2550(string MainServerConnectionString, List<PCMArticleCategory> PCMArticleCategoryList, string ImagePath)
        {
            bool isDeleted = false;
            try
            {
                if (PCMArticleCategoryList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PCMArticleCategory pCMArticleCategory in PCMArticleCategoryList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_pcmArticleCategory_Delete", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pCMArticleCategory.IdPCMArticleCategory);
                            mySqlCommand.ExecuteNonQuery();

                            IsDeletePCMArticleCategoryImageFromPath_V2550(pCMArticleCategory, ImagePath);
                            isDeleted = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isDeleted;
        }

        public bool IsDeletePCMArticleCategoryImageFromPath_V2550(PCMArticleCategory PCMArticleCategory, string ImageFolderPath)
        {
            try
            {
                if (ImageFolderPath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", ImageFolderPath, PCMArticleCategory.IdPCMArticleCategory);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleCategoryImageFromPath()-  ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }
        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2550(string MainServerConnectionString, uint IdPCMArticleCategory, Articles Article, string PCMArticleImagePath, string PCMArticleDocPath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (Article != null && Article.IdArticle > 0 && IdPCMArticleCategory > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_ArticleCategoryWithStatus_Update_V2110", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);
                            mySqlCommand.Parameters.AddWithValue("_idArticle", Article.IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdPCMStatus", Article.IdPCMStatus);
                            mySqlCommand.Parameters.AddWithValue("_IdModifier", Article.IdModifier);
                            mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", Article.IsRtfText);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription", Article.PCMDescription);
                            mySqlCommand.Parameters.AddWithValue("_IsImageShareWithCustomer", Article.IsImageShareWithCustomer);
                            mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", Article.PurchaseQtyMin);
                            mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", Article.PurchaseQtyMax);
                            mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", Article.IsShareWithCustomer);
                            mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", Article.IsSparePartOnly);
                            mySqlCommand.Parameters.AddWithValue("_ArticleName", Article.Description);

                            mySqlCommand.ExecuteNonQuery();

                            mySqlConnection.Close();

                            isUpdated = true;
                        }

                        AddUpdateDeleteArticleCompatibilities_V2550(MainServerConnectionString, Article.IdArticle, Article.ArticleCompatibilityList);
                        AddPCMArticleLogEntry_V2550(MainServerConnectionString, Article.IdArticle, Article.PCMArticleLogEntiryList);
                        AddUpdateDeletePCMArticleImages_V2550(MainServerConnectionString, Article.IdArticle, Article.PCMArticleImageList, PCMArticleImagePath, Article.Reference);
                        AddUpdateDeletePCMArticleDocs_V2550(MainServerConnectionString, Article.IdArticle, Article.PCMArticleAttachmentList, PCMArticleDocPath, Article.Reference);
                        AddWarehouseArticleLogEntry_V2550(MainServerConnectionString, Article.IdArticle, Article.WarehouseArticleLogEntiryList);

                        isUpdated = true;
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticleCategoryInArticleWithStatus_V2550(). IdArticle- {0} ErrorMessage- {1}", Article.IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Article Capabilities
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdArticle">Get Article id.</param>
        /// <param name="CompatibilityList">The list of Compatibility.</param>
        public void AddUpdateDeleteArticleCompatibilities_V2550(string MainServerConnectionString, UInt32 IdArticle, List<ArticleCompatibility> CompatibilityList)
        {
            try
            {
                if (CompatibilityList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ArticleCompatibility articleCompatibility in CompatibilityList)
                        {
                            if (articleCompatibility.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_article_compatibilities_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCompatibility", articleCompatibility.IdCompatibility);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (articleCompatibility.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (articleCompatibility.IdCompatibility > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_article_compatibilities_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCompatibility", articleCompatibility.IdCompatibility);
                                    mySqlCommand.Parameters.AddWithValue("_MinimumElements", articleCompatibility.MinimumElements);
                                    mySqlCommand.Parameters.AddWithValue("_MaximumElements", articleCompatibility.MaximumElements);
                                    if (articleCompatibility.IdRelationshipType == 0)
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                                    }
                                    else
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", articleCompatibility.IdRelationshipType);
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_Quantity", articleCompatibility.Quantity);
                                    mySqlCommand.Parameters.AddWithValue("_Remarks", articleCompatibility.Remarks);
                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", articleCompatibility.IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (articleCompatibility.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_article_compatibilities_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                mySqlCommand.Parameters.AddWithValue("_IdArticleCompatibility", articleCompatibility.IdArticleCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_IdCPtypeCompatibility", articleCompatibility.IdCPtypeCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_IdTypeCompatibility", articleCompatibility.IdTypeCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_MinimumElements", articleCompatibility.MinimumElements);
                                mySqlCommand.Parameters.AddWithValue("_MaximumElements", articleCompatibility.MaximumElements);
                                if (articleCompatibility.IdRelationshipType == 0)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", articleCompatibility.IdRelationshipType);
                                }
                                mySqlCommand.Parameters.AddWithValue("_Quantity", articleCompatibility.Quantity);
                                mySqlCommand.Parameters.AddWithValue("_Remarks", articleCompatibility.Remarks);
                                mySqlCommand.Parameters.AddWithValue("_IdCreator", articleCompatibility.IdCreator);
                                mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteArticleCompatibilities_V2550(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddPCMArticleLogEntry_V2550(string MainServerConnectionString, UInt32 IdArticle, List<PCMArticleLogEntry> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PCMArticleLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByPCMArticle_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleLogEntry_V2550(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddUpdateDeletePCMArticleImages_V2550(string MainServerConnectionString, UInt32 IdArticle, List<PCMArticleImage> ImageList, string ImagePath, string ArticleReference)
        {
            try
            {
                if (ImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PCMArticleImage Image in ImageList)
                        {
                            if (Image.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Images_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdPCMArticleImage", Image.IdPCMArticleImage);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeletePCMArticleImageFromPath_V2550(Image, ImagePath, ArticleReference);
                            }
                            else if (Image.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (Image.IdPCMArticleImage > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Images_Update_V2090", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdPCMArticleImage", Image.IdPCMArticleImage);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", Image.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", Image.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", Image.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", Image.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_Position", Image.Position);
                                    mySqlCommand.Parameters.AddWithValue("_IsImageShareWithCustomer", Image.IsImageShareWithCustomer);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddPCMArticleImageToPath_V2550(Image, ImagePath, ArticleReference);
                                }
                            }
                            else if (Image.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Images_Insert_V2090", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", Image.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", Image.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", Image.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", Image.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_Position", Image.Position);
                                mySqlCommand.Parameters.AddWithValue("_IsImageShareWithCustomer", Image.IsImageShareWithCustomer);

                                Image.IdPCMArticleImage = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (Image.IdPCMArticleImage > 0)
                                {
                                    AddPCMArticleImageToPath_V2550(Image, ImagePath, ArticleReference);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeletePCMArticleImages_V2550(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public bool IsDeletePCMArticleImageFromPath_V2550(PCMArticleImage Image, string ImagePath, string ArticleReference)
        {
            try
            {
                if (ImagePath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", ImagePath, ArticleReference);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        completePath = completePath + "\\" + Image.IdPCMArticleImage + "_" + Image.SavedFileName;

                        if (File.Exists(completePath))
                        {
                            File.Delete(completePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleImageFromPath_V2550()- Filename - {0}. ErrorMessage- {1}", Image.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }
        public bool AddPCMArticleImageToPath_V2550(PCMArticleImage Image, string ImagePath, string ArticleReference)
        {
            if (Image.PCMArticleImageInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", ImagePath, ArticleReference);
                    string filePath = completePath + "\\" + Image.IdPCMArticleImage + "_" + Image.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    string[] filePaths = Directory.GetFiles(completePath, Image.IdPCMArticleImage + "_*", SearchOption.AllDirectories);
                    foreach (string file in filePaths)
                    {
                        File.Delete(file);
                    }
                    File.WriteAllBytes(filePath, Image.PCMArticleImageInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleImageToPath_V2550()- Filename - {0}. ErrorMessage- {1}", Image.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        public void AddUpdateDeletePCMArticleDocs_V2550(string MainServerConnectionString, UInt32 IdArticle, List<ArticleDocument> DocList, string DocPath, string ArticleReference)
        {
            try
            {
                if (DocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ArticleDocument doc in DocList)
                        {
                            if (doc.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Docs_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdArticleDoc", doc.IdArticleDoc);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeletePCMArticleDocFromPath_V2550(doc, DocPath, ArticleReference);
                            }
                            else if (doc.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (doc.IdArticleDoc > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Docs_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdArticleDoc", doc.IdArticleDoc);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", doc.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", doc.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", doc.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", doc.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_idDocType", doc.IdDocType);
                                    mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", doc.IsShareWithCustomer);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddPCMArticleDocToPath_V2550(doc, DocPath, ArticleReference);
                                }
                            }
                            else if (doc.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Docs_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", doc.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", doc.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", doc.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", doc.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_idDocType", doc.IdDocType);
                                mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", doc.IsShareWithCustomer);

                                doc.IdArticleDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (doc.IdArticleDoc > 0)
                                {
                                    AddPCMArticleDocToPath_V2550(doc, DocPath, ArticleReference);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeletePCMArticleDocs(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public bool IsDeletePCMArticleDocFromPath_V2550(ArticleDocument doc, string DocPath, string ArticleReference)
        {
            try
            {
                if (DocPath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", DocPath, ArticleReference);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        completePath = completePath + "\\" + doc.SavedFileName;

                        if (File.Exists(completePath))
                        {
                            File.Delete(completePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleDocFromPath_V2550()- Filename - {0}. ErrorMessage- {1}", doc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }

        public bool AddPCMArticleDocToPath_V2550(ArticleDocument doc, string DocPath, string ArticleReference)
        {
            if (doc.PCMArticleFileInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", DocPath, ArticleReference);
                    string filePath = completePath + "\\" + doc.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    File.WriteAllBytes(filePath, doc.PCMArticleFileInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleDocToPath_V2550()- Filename - {0}. ErrorMessage- {1}", doc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        public void AddWarehouseArticleLogEntry_V2550(string MainServerConnectionString, UInt32 IdArticle, List<LogEntriesByArticle> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (LogEntriesByArticle logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByWarehouseArticleDocs_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_logDateTime", logList.LogDateTime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddWarehouseArticleLogEntry_V2550(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public bool AddDeletePCMArticle_V2550(string MainServerConnectionString, uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser, string PCMArticleImagePath)
        {
            bool isUpdated = false;
            string IdDeletedArticles = string.Join(",", ArticleList.Where(a => a.TransactionOperation == ModelBase.TransactionOperations.Delete).Select(a => a.IdArticle));
            GetPCMImagesAndDeleteByIdArticle_V2550(MainServerConnectionString, IdDeletedArticles, PCMArticleImagePath);

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (ArticleList != null)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            foreach (Articles Article in ArticleList)
                            {
                                if (Article.IdArticle > 0)
                                {
                                    if (Article.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                    {
                                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_pcmArticles_Delete_V2120", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                                        mySqlCommand.Parameters.AddWithValue("_IdArticle", Article.IdArticle);

                                        mySqlCommand.ExecuteNonQuery();
                                    }
                                    else if (Article.TransactionOperation == ModelBase.TransactionOperations.Add)
                                    {
                                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_pcmArticles_Insert_V2140", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                                        mySqlCommand.Parameters.AddWithValue("_IdArticle", Article.IdArticle);
                                        mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);
                                        mySqlCommand.Parameters.AddWithValue("_IdCreator", IdUser);
                                        mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                        mySqlCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            mySqlConnection.Close();
                        }
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDeletePCMArticle_V2140().  ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }
        public void GetPCMImagesAndDeleteByIdArticle_V2550(string PCMConnectionString, string IdArticles, string PCMArticleImagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(IdArticles))
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetArticleImagesByIdPCMArticles_ForDelete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdArticles", IdArticles);
                        using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IsDeletePCMArticleImageFromPath_V2550(Convert.ToUInt32(reader["IdPCMArticleImage"]), Convert.ToString(reader["SavedFileName"]), PCMArticleImagePath, Convert.ToString(reader["Reference"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMImagesAndDeleteByIdArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public bool IsDeletePCMArticleImageFromPath_V2550(uint IdPCMArticleImage, string SavedFileName, string ImagePath, string ArticleReference)
        {
            try
            {
                if (ImagePath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", ImagePath, ArticleReference);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        completePath = completePath + "\\" + IdPCMArticleImage + "_" + SavedFileName;

                        if (File.Exists(completePath))
                        {
                            File.Delete(completePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleImageFromPath_V2550()- Filename - {0}. ErrorMessage- {1}", SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }
        public bool UpdateDetectionForAddDetectionViewModel_V2550(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3 || detectionDetails.IdDetectionType == 4)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup_V2550(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Update_V2180", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionDetails.IdDetections);
                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionDetails.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", detectionDetails.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", detectionDetails.PurchaseQtyMin);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", detectionDetails.PurchaseQtyMax);
                        mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", detectionDetails.IsShareWithCustomer);
                        mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", detectionDetails.IsSparePartOnly);
                        if (IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);
                        }

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }

                    //add/update/delete ways, detections, options, spare parts
                    AddUpdateDeleteDetectionFiles_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                    AddUpdateDeleteDetectionLinks_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                    AddUpdateDeleteDetectionImages_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                    AddDeleteCustomersRegionsByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerList, detectionDetails.IdDetectionType, detectionDetails.ModifiedBy);
                    AddDetectionLogEntry_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);
                    //
                    AddUpdateDeletePLMDetectionPrices_V2550(MainServerConnectionString, detectionDetails.ModifiedPLMDetectionList, detectionDetails.IdDetections, detectionDetails.ModifiedBy);
                    AddBasePriceListLogEntry_V2550(MainServerConnectionString, detectionDetails.BasePriceLogEntryList);
                    AddCustomerPriceListLogEntry_V2550(MainServerConnectionString, detectionDetails.CustomerPriceLogEntryList);
                    //
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error PCM_Detections_Update_V2550(). IdDetection- {0} ErrorMessage- {1}", detectionDetails.IdDetections, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }
        public void AddUpdateDeletePLMDetectionPrices_V2550(string MainServerConnectionString,
          List<PLMDetectionPrice> PLMDetectionPriceList,
          UInt32 IdDetection, UInt32? IdModifier)
        {
            try
            {
                if (PLMDetectionPriceList != null)
                {
                    DetectionDetails.UpdateAllMaxCostFromNullToZero(PLMDetectionPriceList);

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PLMDetectionPrice pLMDetectionPrice in PLMDetectionPriceList)
                        {
                            if (pLMDetectionPrice.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                if (pLMDetectionPrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_base_price_list_by_detections_Delete", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMDetectionPrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMDetectionPrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_customer_price_list_by_detections_Delete", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMDetectionPrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (pLMDetectionPrice.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (pLMDetectionPrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_base_price_list_by_detections_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMDetectionPrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMDetectionPrice.IdRule);
                                    pLMDetectionPrice.RuleValue = pLMDetectionPrice.RuleValue;
                                    if (pLMDetectionPrice.IdRule == 1518)
                                    {
                                        pLMDetectionPrice.RuleValue = 0;
                                    }
                                    else if (pLMDetectionPrice.IdRule == 308)
                                    {
                                        if (pLMDetectionPrice.RuleValue == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMDetectionPrice.IdRule == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMDetectionPrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_MaxCost", pLMDetectionPrice.MaxCost);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMDetectionPrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_customer_price_list_by_detections_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMDetectionPrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMDetectionPrice.IdRule);
                                    pLMDetectionPrice.RuleValue = pLMDetectionPrice.RuleValue;
                                    if (pLMDetectionPrice.IdRule == 1518)
                                    {
                                        pLMDetectionPrice.RuleValue = 0;
                                    }
                                    else if (pLMDetectionPrice.IdRule == 308)
                                    {
                                        if (pLMDetectionPrice.RuleValue == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMDetectionPrice.IdRule == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMDetectionPrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (pLMDetectionPrice.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                if (pLMDetectionPrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_base_price_list_by_detections_Insert", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMDetectionPrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                    pLMDetectionPrice.RuleValue = pLMDetectionPrice.RuleValue;
                                    if (pLMDetectionPrice.IdRule == 1518)
                                    {
                                        pLMDetectionPrice.RuleValue = 0;
                                    }
                                    else if (pLMDetectionPrice.IdRule == 308)
                                    {
                                        if (pLMDetectionPrice.RuleValue == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = null;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMDetectionPrice.IdRule == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMDetectionPrice.IdRule);
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMDetectionPrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_MaxCost", pLMDetectionPrice.MaxCost);
                                    mySqlCommand.Parameters.AddWithValue("_Royalty", 0);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMDetectionPrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_customer_price_list_by_detections_Insert", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMDetectionPrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                    pLMDetectionPrice.RuleValue = pLMDetectionPrice.RuleValue;
                                    if (pLMDetectionPrice.IdRule == 1518)
                                    {
                                        pLMDetectionPrice.RuleValue = 0;
                                    }
                                    else if (pLMDetectionPrice.IdRule == 308)
                                    {
                                        if (pLMDetectionPrice.RuleValue == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = null;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMDetectionPrice.IdRule == 0)
                                        {
                                            pLMDetectionPrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMDetectionPrice.IdRule);
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMDetectionPrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeletePLMDetectionPrices_V2550(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddBasePriceListLogEntry_V2550(string MainServerConnectionString, List<BasePriceLogEntry> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (BasePriceLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PLM_LogEntriesByBasePriceList_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", logList.IdBasePriceList);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddBasePriceListLogEntry_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddCustomerPriceListLogEntry_V2550(string MainServerConnectionString, List<CustomerPriceLogEntry> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CustomerPriceLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PLM_LogEntriesByCustomerPriceList_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", logList.IdCustomerPriceList);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCustomerPriceListLogEntry_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public bool ImportArticleCostPriceCalculate_V2550(Common.Company itemCompany, string mainServerConnectionString, string workbenchConnectionString, UInt64 itemArticle, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, List<ArticlesByArticle> LstAllArticlesByArticle, List<PODetail> ArticlesByArticleComponentpoDetailLst)
        {
            bool isCalculated = false;
            List<ArticleCostPrice> articleCostPriceLst = new List<ArticleCostPrice>();
            List<ArticleCostPrice> articleCostPriceLstForAllCompanies = new List<ArticleCostPrice>();
            string connectionstring = itemCompany.ConnectPlantConstr;


            List<PODetail> EWHQpoDetailLst = GetArticleMaxPODeliveryDateDetailFromEWHQ_V2550(connectionstring, itemArticle);
            //List<PODetail> EWHQArticlesByArticleComponentpoDetailLst = GetAllArticlesByArticleComponentMaxPOFromEWHQ(connectionstring);
            //List<ArticlesByArticle> LstAllArticlesByArticle = GetAllArticlesByArticle(connectionstring);

            List<PODetail> poDetailLst = GetPCMArticleMaxPODeliveryDateDetail_V2550(itemCompany.ConnectPlantConstr, itemArticle);
            //  List<PODetail> ArticlesByArticleComponentpoDetailLst = GetAllArticlesByArticleComponentMaxPOFromPlant(itemCompany.ConnectPlantConstr);
            // allPCMidsArticles.Where(pa => pa == 6951)

            try
            {

                ArticleCostPrice articleCostPrice = new ArticleCostPrice();
                PODetail poDetail = null;
                try
                {
                    poDetail = poDetailLst.Where(pdl => pdl.IdArticle == itemArticle).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0} Company - {1}", ex.Message, itemCompany.Alias), category: Category.Exception, priority: Priority.Low);
                    poDetail = null;
                    throw ex;
                }

                if (poDetail == null)
                {
                    PODetail poDetailEWHQ = null;
                    try
                    {
                        poDetailEWHQ = EWHQpoDetailLst.Where(pdl => pdl.IdArticle == itemArticle).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        poDetailEWHQ = null;
                        throw ex;
                    }
                    if (poDetailEWHQ == null)
                    {
                        if (LstAllArticlesByArticle != null && LstAllArticlesByArticle.Any(a => a.IdArticle == Convert.ToInt32(itemArticle)))
                        {
                            //List<ArticlesByArticle> LstArticlesByArticle = GetAllArticlesByArticle(itemCompany.ConnectPlantConstr, Convert.ToInt32(itemArticle));
                            List<ArticlesByArticle> LstArticlesByArticle = LstAllArticlesByArticle.Where(a => a.IdArticle == Convert.ToInt32(itemArticle)).ToList();
                            FinalArticleCostValue = 0;
                            ExchangeRateDate = DateTime.MinValue;
                            IdCurrency = 0;
                            if (LstArticlesByArticle != null && LstArticlesByArticle.Count > 0)
                            {
                                CalculateComponentArticleCost_V2550(LstArticlesByArticle, itemCompany, workbenchConnectionString, LstAllArticlesByArticle, ArticlesByArticleComponentpoDetailLst, EWHQArticlesByArticleComponentpoDetailLst, Convert.ToInt32(itemArticle));
                                if (FinalArticleCostValue > 0)
                                {
                                    articleCostPrice.IdArticle = itemArticle;
                                    articleCostPrice.IdCompany = Convert.ToUInt32(itemCompany.IdCompany);
                                    articleCostPrice.ArticleCostValue = FinalArticleCostValue;
                                    articleCostPrice.IdCurrency = Convert.ToUInt32(itemCompany.IdCurrency);
                                    articleCostPrice.ExchangeRateDate = ExchangeRateDate;
                                    if (!IsExistRecord_V2550(connectionstring, articleCostPrice))
                                    {
                                        try
                                        {

                                            using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                                            {
                                                mySqlConnection.Open();
                                                MySqlCommand mySqlCommand = new MySqlCommand("PLM_InsertArticleCostPrice_V2160", mySqlConnection);
                                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                                mySqlCommand.CommandTimeout = 3000;

                                                mySqlCommand.Parameters.AddWithValue("_IdArticle", articleCostPrice.IdArticle);
                                                mySqlCommand.Parameters.AddWithValue("_ArticleCostValue", articleCostPrice.ArticleCostValue);
                                                mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now);
                                                mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now);
                                                mySqlCommand.Parameters.AddWithValue("_ExchangeRateDate", articleCostPrice.ExchangeRateDate);
                                                mySqlCommand.Parameters.AddWithValue("_IdCompany", articleCostPrice.IdCompany);
                                                mySqlCommand.Parameters.AddWithValue("_IdCurrency", articleCostPrice.IdCurrency);
                                                mySqlCommand.Parameters.AddWithValue("_IdModifier", 164);
                                                mySqlCommand.Parameters.AddWithValue("_IdCreator", 164);

                                                mySqlCommand.ExecuteNonQuery();
                                                mySqlConnection.Close();

                                            }

                                            isCalculated = true;
                                        }
                                        catch (Exception ex)
                                        {

                                            Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                            isCalculated = false;
                                            throw ex;

                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        articleCostPrice.IdArticle = itemArticle;
                        articleCostPrice.IdCompany = Convert.ToUInt32(itemCompany.IdCompany);
                        articleCostPrice.ArticleCostValue = poDetailEWHQ.UnitPrice;
                        articleCostPrice.IdCurrency = poDetailEWHQ.IdCurrency;
                        articleCostPrice.ExchangeRateDate = poDetailEWHQ.CreationDate;
                        if (!IsExistRecord_V2550(connectionstring, articleCostPrice))
                        {
                            try
                            {

                                using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                                {
                                    mySqlConnection.Open();
                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_InsertArticleCostPrice_V2160", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.CommandTimeout = 3000;

                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", articleCostPrice.IdArticle);
                                    mySqlCommand.Parameters.AddWithValue("_ArticleCostValue", articleCostPrice.ArticleCostValue);
                                    mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now);
                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now);
                                    mySqlCommand.Parameters.AddWithValue("_ExchangeRateDate", articleCostPrice.ExchangeRateDate);
                                    mySqlCommand.Parameters.AddWithValue("_IdCompany", articleCostPrice.IdCompany);
                                    mySqlCommand.Parameters.AddWithValue("_IdCurrency", articleCostPrice.IdCurrency);
                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", 164);
                                    mySqlCommand.Parameters.AddWithValue("_IdCreator", 164);

                                    mySqlCommand.ExecuteNonQuery();
                                    mySqlConnection.Close();

                                }

                                isCalculated = true;
                            }
                            catch (Exception ex)
                            {

                                Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                isCalculated = false;
                                throw ex;

                            }

                        }
                    }
                }
                else
                {
                    if (poDetail.IsEmdep != 1)
                    {
                        articleCostPrice.IdArticle = itemArticle;
                        articleCostPrice.IdCompany = Convert.ToUInt32(itemCompany.IdCompany);
                        articleCostPrice.ArticleCostValue = poDetail.UnitPrice;
                        articleCostPrice.IdCurrency = poDetail.IdCurrency;
                        articleCostPrice.ExchangeRateDate = poDetail.CreationDate;
                        if (!IsExistRecord_V2550(connectionstring, articleCostPrice))
                        {
                            try
                            {

                                using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                                {
                                    mySqlConnection.Open();
                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_InsertArticleCostPrice_V2160", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.CommandTimeout = 3000;

                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", articleCostPrice.IdArticle);
                                    mySqlCommand.Parameters.AddWithValue("_ArticleCostValue", articleCostPrice.ArticleCostValue);
                                    mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now);
                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now);
                                    mySqlCommand.Parameters.AddWithValue("_ExchangeRateDate", articleCostPrice.ExchangeRateDate);
                                    mySqlCommand.Parameters.AddWithValue("_IdCompany", articleCostPrice.IdCompany);
                                    mySqlCommand.Parameters.AddWithValue("_IdCurrency", articleCostPrice.IdCurrency);
                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", 164);
                                    mySqlCommand.Parameters.AddWithValue("_IdCreator", 164);

                                    mySqlCommand.ExecuteNonQuery();
                                    mySqlConnection.Close();

                                }

                                isCalculated = true;
                            }
                            catch (Exception ex)
                            {

                                Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                isCalculated = false;
                                throw ex;

                            }

                        }
                    }
                    else
                    {
                        PODetail poDetailEWHQ = null;
                        try
                        {
                            poDetailEWHQ = EWHQpoDetailLst.Where(pdl => pdl.IdArticle == itemArticle).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            poDetailEWHQ = null;
                        }

                        if (poDetailEWHQ == null)
                        {
                            if (LstAllArticlesByArticle != null && LstAllArticlesByArticle.Any(a => a.IdArticle == Convert.ToInt32(itemArticle)))
                            {
                                List<ArticlesByArticle> LstArticlesByArticle = LstAllArticlesByArticle.Where(a => a.IdArticle == Convert.ToInt32(itemArticle)).ToList();
                                FinalArticleCostValue = 0;
                                ExchangeRateDate = DateTime.MinValue;
                                IdCurrency = 0;
                                if (LstArticlesByArticle != null && LstArticlesByArticle.Count > 0)
                                {
                                    CalculateComponentArticleCost_V2550(LstArticlesByArticle, itemCompany, workbenchConnectionString, LstAllArticlesByArticle, ArticlesByArticleComponentpoDetailLst, EWHQArticlesByArticleComponentpoDetailLst, Convert.ToInt32(itemArticle));
                                    if (FinalArticleCostValue > 0)
                                    {
                                        articleCostPrice.IdArticle = itemArticle;
                                        articleCostPrice.IdCompany = Convert.ToUInt32(itemCompany.IdCompany);
                                        articleCostPrice.ArticleCostValue = FinalArticleCostValue;
                                        articleCostPrice.IdCurrency = Convert.ToUInt32(itemCompany.IdCurrency);
                                        articleCostPrice.ExchangeRateDate = ExchangeRateDate;
                                        if (!IsExistRecord_V2550(connectionstring, articleCostPrice))
                                        {
                                            try
                                            {

                                                using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                                                {
                                                    mySqlConnection.Open();
                                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_InsertArticleCostPrice_V2160", mySqlConnection);
                                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                                    mySqlCommand.CommandTimeout = 3000;

                                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", articleCostPrice.IdArticle);
                                                    mySqlCommand.Parameters.AddWithValue("_ArticleCostValue", articleCostPrice.ArticleCostValue);
                                                    mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now);
                                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now);
                                                    mySqlCommand.Parameters.AddWithValue("_ExchangeRateDate", articleCostPrice.ExchangeRateDate);
                                                    mySqlCommand.Parameters.AddWithValue("_IdCompany", articleCostPrice.IdCompany);
                                                    mySqlCommand.Parameters.AddWithValue("_IdCurrency", articleCostPrice.IdCurrency);
                                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", 164);
                                                    mySqlCommand.Parameters.AddWithValue("_IdCreator", 164);

                                                    mySqlCommand.ExecuteNonQuery();
                                                    mySqlConnection.Close();

                                                }

                                                isCalculated = true;
                                            }
                                            catch (Exception ex)
                                            {

                                                Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                                isCalculated = false;
                                                throw ex;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            articleCostPrice.IdArticle = itemArticle;
                            articleCostPrice.IdCompany = Convert.ToUInt32(itemCompany.IdCompany);
                            articleCostPrice.ArticleCostValue = poDetailEWHQ.UnitPrice;
                            articleCostPrice.IdCurrency = poDetailEWHQ.IdCurrency;
                            articleCostPrice.ExchangeRateDate = poDetailEWHQ.CreationDate;
                            if (!IsExistRecord_V2550(connectionstring, articleCostPrice))
                            {
                                try
                                {

                                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                                    {
                                        mySqlConnection.Open();
                                        MySqlCommand mySqlCommand = new MySqlCommand("PLM_InsertArticleCostPrice_V2160", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                                        mySqlCommand.CommandTimeout = 3000;

                                        mySqlCommand.Parameters.AddWithValue("_IdArticle", articleCostPrice.IdArticle);
                                        mySqlCommand.Parameters.AddWithValue("_ArticleCostValue", articleCostPrice.ArticleCostValue);
                                        mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now);
                                        mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now);
                                        mySqlCommand.Parameters.AddWithValue("_ExchangeRateDate", articleCostPrice.ExchangeRateDate);
                                        mySqlCommand.Parameters.AddWithValue("_IdCompany", articleCostPrice.IdCompany);
                                        mySqlCommand.Parameters.AddWithValue("_IdCurrency", articleCostPrice.IdCurrency);
                                        mySqlCommand.Parameters.AddWithValue("_IdModifier", 164);
                                        mySqlCommand.Parameters.AddWithValue("_IdCreator", 164);

                                        mySqlCommand.ExecuteNonQuery();
                                        mySqlConnection.Close();

                                    }

                                    isCalculated = true;
                                }
                                catch (Exception ex)
                                {

                                    Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                                    isCalculated = false;
                                    throw ex;
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                isCalculated = false;
                Log4NetLogger.Logger.Log(string.Format("Error ImportArticleCostPriceCalculate(). ErrorMessage- {0} Company - {1} Article{2}", ex.Message, itemCompany.Alias, itemArticle), category: Category.Exception, priority: Priority.Low);
                FinalArticleCostValue = 0;
                ExchangeRateDate = DateTime.MinValue;
                IdCurrency = 0;
                throw ex;
            }









            return isCalculated;
        }
        public List<PODetail> GetArticleMaxPODeliveryDateDetailFromEWHQ_V2550(string plantconnectionstring, UInt64 itemArticle)
        {
            List<PODetail> poDetailLst = new List<PODetail>();
            string connectionstring = GetEWHQDatabaseDetail_V2550(plantconnectionstring);
            using (MySqlConnection con = new MySqlConnection(connectionstring))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("PCM_GetPCMArticleMaxPODeliveryDateDetailFromEWHQ", con);

                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdArticle", itemArticle);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PODetail poDetail = new PODetail();
                        if (reader["CreatedIn"] != DBNull.Value)
                            poDetail.CreationDate = Convert.ToDateTime(reader["CreatedIn"]);
                        if (reader["idCurrency"] != DBNull.Value)
                            poDetail.IdCurrency = Convert.ToUInt32(reader["idCurrency"]);
                        if (reader["unitPrice"] != DBNull.Value)
                            poDetail.UnitPrice = Convert.ToDouble(reader["unitPrice"]);
                        if (reader["IsEmdep"] != DBNull.Value)
                            poDetail.IsEmdep = Convert.ToSByte(reader["IsEmdep"]);
                        if (reader["idArticle"] != DBNull.Value)
                            poDetail.IdArticle = Convert.ToUInt32(reader["idArticle"]);

                        poDetailLst.Add(poDetail);
                    }
                }
            }
            return poDetailLst;
        }

        public string GetEWHQDatabaseDetail_V2550(string connectionstring)
        {
            string connstr = null;

            using (MySqlConnection consite = new MySqlConnection(connectionstring))
            {
                consite.Open();
                MySqlCommand consitecommand = new MySqlCommand("GetEWHQDatabaseDetail", consite);
                consitecommand.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader sitereader = consitecommand.ExecuteReader())
                {
                    if (sitereader.Read())
                    {
                        connstr = "Data Source = " + sitereader["DatabaseIP"].ToString() + "; Database = geos; User ID = GeosUsr; Password = GEOS;Convert Zero Datetime=True";
                    }
                }
            }
            return connstr;
        }
        public List<PODetail> GetPCMArticleMaxPODeliveryDateDetail_V2550(string connectionstring, UInt64 idArticle)
        {
            List<PODetail> poDetailLst = new List<PODetail>();

            using (MySqlConnection con = new MySqlConnection(connectionstring))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("PCM_GetPCMArticleMaxPODeliveryDateDetail", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdArticle", idArticle);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PODetail poDetail = new PODetail();
                        if (reader["CreatedIn"] != DBNull.Value)
                            poDetail.CreationDate = Convert.ToDateTime(reader["CreatedIn"]);
                        if (reader["idCurrency"] != DBNull.Value)
                            poDetail.IdCurrency = Convert.ToUInt32(reader["idCurrency"]);
                        if (reader["unitPrice"] != DBNull.Value)
                            poDetail.UnitPrice = Convert.ToDouble(reader["unitPrice"]);
                        if (reader["IsEmdep"] != DBNull.Value)
                            poDetail.IsEmdep = Convert.ToSByte(reader["IsEmdep"]);
                        if (reader["idArticle"] != DBNull.Value)
                            poDetail.IdArticle = Convert.ToUInt32(reader["idArticle"]);

                        poDetailLst.Add(poDetail);
                    }
                }
            }
            return poDetailLst;
        }

        public void CalculateComponentArticleCost_V2550(List<ArticlesByArticle> LstArticlesByArticle, Common.Company plant, string workbenchConnectionString, List<ArticlesByArticle> LstAllArticlesByArticle, List<PODetail> ArticlesByArticleComponentpoDetailLst, List<PODetail> EWHQArticlesByArticleComponentpoDetailLst, Int32 idArticle)
        {
            if (LstArticlesByArticle != null && LstArticlesByArticle.Count > 0)
            {
                foreach (ArticlesByArticle item in LstArticlesByArticle)
                {
                    PODetail poDetail = ArticlesByArticleComponentpoDetailLst.Where(i => i.IdArticle == item.IdComponent).FirstOrDefault();
                    //GetArticleMaxPODeliveryDateDetail(plant.ConnectPlantConstr, item.IdComponent);

                    if (poDetail == null)
                    {
                        PODetail poDetailEWHQ = null;
                        try
                        {
                            poDetailEWHQ = EWHQArticlesByArticleComponentpoDetailLst.Where(i => i.IdArticle == item.IdComponent).FirstOrDefault();
                            // GetArticleMaxPODeliveryDateDetailFromEWHQ(EWHQConnStr, item.IdComponent);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error CalculateArticleCostPrice(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            poDetailEWHQ = null;
                        }
                        if (poDetailEWHQ == null)
                        {
                            if (LstAllArticlesByArticle != null && LstAllArticlesByArticle.Any(a => a.IdArticle == item.IdComponent))
                            {
                                // CalculateComponentArticleCost(GetAllArticlesByArticle(plant.ConnectPlantConstr, item.IdComponent), plant, workbenchConnectionString, EWHQConnStr);
                                CalculateComponentArticleCost_V2550(LstAllArticlesByArticle.Where(a => a.IdArticle == item.IdComponent).ToList(), plant, workbenchConnectionString, LstAllArticlesByArticle, ArticlesByArticleComponentpoDetailLst, EWHQArticlesByArticleComponentpoDetailLst, item.IdArticle);
                            }
                            else
                            {
                                FinalArticleCostValue = 0;
                                return;
                            }
                        }
                        else
                        {
                            float parentQty = 0;
                            bool isParentQty = false;
                            if (idArticle != item.IdArticle)
                            {
                                isParentQty = true;
                                if (LstAllArticlesByArticle.Any(i => i.IdArticle == idArticle && i.IdComponent == item.IdArticle))
                                    parentQty = LstAllArticlesByArticle.Where(i => i.IdArticle == idArticle && i.IdComponent == item.IdArticle).FirstOrDefault().Quantity;
                            }
                            if ((isParentQty ? ((poDetailEWHQ.UnitPrice * item.Quantity) * parentQty) : (poDetailEWHQ.UnitPrice * item.Quantity)) > 0)
                            {
                                if (ExchangeRateDate.Date < poDetailEWHQ.CreationDate.Date)
                                {
                                    ExchangeRateDate = poDetailEWHQ.CreationDate;
                                }

                                CurrencyConversion currencyConversion = null;
                                if (plant.IdCurrency != Convert.ToInt32(poDetailEWHQ.IdCurrency))
                                {
                                    currencyConversion = GetCurrencyConversionByTOFROMDATE_V2550(workbenchConnectionString, Convert.ToByte(plant.IdCurrency), Convert.ToByte(poDetailEWHQ.IdCurrency), poDetailEWHQ.CreationDate);

                                }
                                if (currencyConversion != null)
                                {
                                    FinalArticleCostValue = FinalArticleCostValue + ((isParentQty ? ((poDetailEWHQ.UnitPrice * item.Quantity) * parentQty) : (poDetailEWHQ.UnitPrice * item.Quantity)) * currencyConversion.ExchangeRate);


                                }
                                else
                                {

                                    FinalArticleCostValue = FinalArticleCostValue + (isParentQty ? ((poDetailEWHQ.UnitPrice * item.Quantity) * parentQty) : (poDetailEWHQ.UnitPrice * item.Quantity));
                                }


                            }
                            else
                            {
                                FinalArticleCostValue = 0;
                                return;
                            }



                        }
                    }
                    else
                    {
                        if (poDetail.IsEmdep != 1)
                        {

                            float parentQty = 0;
                            bool isParentQty = false;
                            if (idArticle != item.IdArticle)
                            {
                                isParentQty = true;
                                if (LstAllArticlesByArticle.Any(i => i.IdArticle == idArticle && i.IdComponent == item.IdArticle))
                                    parentQty = LstAllArticlesByArticle.Where(i => i.IdArticle == idArticle && i.IdComponent == item.IdArticle).FirstOrDefault().Quantity;
                            }
                            if ((isParentQty ? ((poDetail.UnitPrice * item.Quantity) * parentQty) : (poDetail.UnitPrice * item.Quantity)) > 0)
                            {
                                if (ExchangeRateDate.Date < poDetail.CreationDate.Date)
                                {
                                    ExchangeRateDate = poDetail.CreationDate;
                                }
                                CurrencyConversion currencyConversion = null;
                                if (plant.IdCurrency != Convert.ToInt32(poDetail.IdCurrency))
                                {
                                    currencyConversion = GetCurrencyConversionByTOFROMDATE_V2550(workbenchConnectionString, Convert.ToByte(plant.IdCurrency), Convert.ToByte(poDetail.IdCurrency), poDetail.CreationDate);

                                }
                                if (currencyConversion != null)
                                {
                                    FinalArticleCostValue = FinalArticleCostValue + ((isParentQty ? ((poDetail.UnitPrice * item.Quantity) * parentQty) : (poDetail.UnitPrice * item.Quantity)) * currencyConversion.ExchangeRate);
                                }
                                else
                                {
                                    FinalArticleCostValue = FinalArticleCostValue + (isParentQty ? ((poDetail.UnitPrice * item.Quantity) * parentQty) : (poDetail.UnitPrice * item.Quantity));
                                }

                            }
                            else
                            {
                                FinalArticleCostValue = 0;
                                return;
                            }


                        }
                        else
                        {
                            PODetail poDetailEWHQ = null;
                            try
                            {
                                poDetailEWHQ = EWHQArticlesByArticleComponentpoDetailLst.Where(i => i.IdArticle == item.IdComponent).FirstOrDefault();
                                //GetArticleMaxPODeliveryDateDetailFromEWHQ(EWHQConnStr, item.IdComponent);
                            }
                            catch (Exception ex)
                            {
                                Log4NetLogger.Logger.Log(string.Format("Error CalculateArticleCostPrice(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                poDetailEWHQ = null;
                            }

                            if (poDetailEWHQ == null)
                            {
                                if (LstAllArticlesByArticle != null && LstAllArticlesByArticle.Any(a => a.IdArticle == item.IdComponent))
                                {
                                    //  CalculateComponentArticleCost(GetAllArticlesByArticle(plant.ConnectPlantConstr, item.IdComponent), plant, workbenchConnectionString, EWHQConnStr);
                                    // CalculateComponentArticleCost(GetAllArticlesByArticle(plant.ConnectPlantConstr, item.IdComponent), plant, workbenchConnectionString, EWHQConnStr);
                                    CalculateComponentArticleCost_V2550(LstAllArticlesByArticle.Where(a => a.IdArticle == item.IdComponent).ToList(), plant, workbenchConnectionString, LstAllArticlesByArticle, ArticlesByArticleComponentpoDetailLst, EWHQArticlesByArticleComponentpoDetailLst, item.IdArticle);
                                }
                                else
                                {
                                    FinalArticleCostValue = 0;
                                    return;
                                }
                            }
                            else
                            {

                                float parentQty = 0;
                                bool isParentQty = false;
                                if (idArticle != item.IdArticle)
                                {
                                    isParentQty = true;
                                    if (LstAllArticlesByArticle.Any(i => i.IdArticle == idArticle && i.IdComponent == item.IdArticle))
                                        parentQty = LstAllArticlesByArticle.Where(i => i.IdArticle == idArticle && i.IdComponent == item.IdArticle).FirstOrDefault().Quantity;
                                }
                                if ((isParentQty ? ((poDetailEWHQ.UnitPrice * item.Quantity) * parentQty) : (poDetailEWHQ.UnitPrice * item.Quantity)) > 0)
                                {

                                    if (ExchangeRateDate.Date < poDetailEWHQ.CreationDate.Date)
                                    {
                                        ExchangeRateDate = poDetailEWHQ.CreationDate;
                                    }

                                    CurrencyConversion currencyConversion = null;
                                    if (plant.IdCurrency != Convert.ToInt32(poDetailEWHQ.IdCurrency))
                                    {
                                        currencyConversion = GetCurrencyConversionByTOFROMDATE_V2550(workbenchConnectionString, Convert.ToByte(plant.IdCurrency), Convert.ToByte(poDetailEWHQ.IdCurrency), poDetailEWHQ.CreationDate);
                                    }
                                    if (currencyConversion != null)
                                    {
                                        FinalArticleCostValue = FinalArticleCostValue + ((isParentQty ? ((poDetailEWHQ.UnitPrice * item.Quantity) * parentQty) : (poDetailEWHQ.UnitPrice * item.Quantity)) * currencyConversion.ExchangeRate);
                                    }
                                    else
                                    {
                                        FinalArticleCostValue = FinalArticleCostValue + (isParentQty ? ((poDetailEWHQ.UnitPrice * item.Quantity) * parentQty) : (poDetailEWHQ.UnitPrice * item.Quantity));
                                    }
                                }
                                else
                                {
                                    FinalArticleCostValue = 0;
                                    return;
                                }


                            }
                        }
                    }
                }
            }
            else
            {
                FinalArticleCostValue = 0;
                return;
            }

        }
        public CurrencyConversion GetCurrencyConversionByTOFROMDATE_V2550(string workbenchConnectionstring, byte idCurrencyConversionTo, byte idCurrencyConversionFrom, DateTime exchangeRateDate)
        {
            CurrencyConversion currencyConversion = null;

            using (MySqlConnection con = new MySqlConnection(workbenchConnectionstring))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("PLM_GetCurrencyConversionByTOFROMDATE", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_idCurrencyConversionTo", idCurrencyConversionTo);
                concommand.Parameters.AddWithValue("_idCurrencyConversionFrom", idCurrencyConversionFrom);
                concommand.Parameters.AddWithValue("_ExchangeRateDate", exchangeRateDate);
                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currencyConversion = new CurrencyConversion();

                        if (reader["CurrencyConversionRate"] != DBNull.Value)
                            currencyConversion.ExchangeRate = (float)(reader["CurrencyConversionRate"]);
                    }
                }
            }
            return currencyConversion;
        }
        public bool IsExistRecord_V2550(string connectionstring, ArticleCostPrice articleCostPrice)
        {
            bool isExist = false;

            using (MySqlConnection con = new MySqlConnection(connectionstring))
            {
                con.Open();
                MySqlCommand concommand = new MySqlCommand("PLM_check_article_details_exists", con);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_IdArticle", articleCostPrice.IdArticle);
                concommand.Parameters.AddWithValue("_IdCompany", articleCostPrice.IdCompany);
                concommand.Parameters.AddWithValue("_IdCurrency", articleCostPrice.IdCurrency);
                concommand.Parameters.AddWithValue("_ExchangeRateDate", articleCostPrice.ExchangeRateDate);
                concommand.Parameters.AddWithValue("_ArticleCostValue", articleCostPrice.ArticleCostValue);

                using (MySqlDataReader reader = concommand.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        if (reader["IdArticleCostPrice"] != DBNull.Value)
                            isExist = true;
                    }
                }
            }
            return isExist;
        }

        public bool IsUpdatePCMArticleCategory_V2550(string MainServerConnectionString, PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList, string ImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Update_V2290", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pcmArticleCategory.IdPCMArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Name", pcmArticleCategory.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", pcmArticleCategory.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", pcmArticleCategory.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", pcmArticleCategory.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", pcmArticleCategory.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", pcmArticleCategory.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", pcmArticleCategory.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", pcmArticleCategory.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", pcmArticleCategory.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", pcmArticleCategory.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", pcmArticleCategory.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", pcmArticleCategory.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", pcmArticleCategory.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", pcmArticleCategory.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                        mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", pcmArticleCategory.IdArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                        mySqlCommand.Parameters.AddWithValue("_IsLeaf", pcmArticleCategory.IsLeaf);
                        mySqlCommand.Parameters.AddWithValue("_IdModifier", pcmArticleCategory.IdModifier);
                        mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_ImagePath", pcmArticleCategory.ImagePath);
                        if (pcmArticleCategory.InUse.ToUpper() == "YES")
                            mySqlCommand.Parameters.AddWithValue("_pcmCategoryInUse", 1);
                        else
                            mySqlCommand.Parameters.AddWithValue("_pcmCategoryInUse", 0);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    //update position of pcm article categories
                    UpdatePCMArticleCategoryPosition_V2550(MainServerConnectionString, pcmArticleCategoryList, pcmArticleCategory.IdModifier);
                    AddPCMArticleCategoryImageToPath_V2550(pcmArticleCategory, ImagePath);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticleCategory_V2290(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }
        /// <summary>
        /// This method is used to Update PCM article category position
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="pcmArticleCategoryList">The list of pcm article category.</param>
        public void UpdatePCMArticleCategoryPosition_V2550(string MainServerConnectionString, List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier)
        {
            try
            {
                if (pcmArticleCategoryList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategoryList)
                        {
                            if (pcmArticleCategory.IdPCMArticleCategory > 0)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Position_Update", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pcmArticleCategory.IdPCMArticleCategory);
                                mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                                mySqlCommand.Parameters.AddWithValue("_IdModifier", IdModifier);
                                mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdatePCMArticleCategoryPosition_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public bool AddPCMArticleCategoryImageToPath_V2550(PCMArticleCategory PCMArticleCategory, string ImageFolderPath)
        {
            if (PCMArticleCategory.PCMArticleCategoryImageInByte != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", ImageFolderPath, PCMArticleCategory.IdPCMArticleCategory);
                    string filePath = completePath + "\\" + PCMArticleCategory.ImagePath;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, PCMArticleCategory.PCMArticleCategoryImageInByte);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleCategoryImageToPath()- ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }
        public PCMArticleCategory AddPCMArticleCategory_V2550(string MainServerConnectionString, PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList, string ImagePath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Insert_V2290", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_Name", pcmArticleCategory.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", pcmArticleCategory.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", pcmArticleCategory.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", pcmArticleCategory.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", pcmArticleCategory.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", pcmArticleCategory.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", pcmArticleCategory.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", pcmArticleCategory.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", pcmArticleCategory.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", pcmArticleCategory.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", pcmArticleCategory.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", pcmArticleCategory.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", pcmArticleCategory.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", pcmArticleCategory.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                        mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", pcmArticleCategory.IdArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                        mySqlCommand.Parameters.AddWithValue("_IsLeaf", pcmArticleCategory.IsLeaf);
                        mySqlCommand.Parameters.AddWithValue("_IdCreator", pcmArticleCategory.IdCreator);
                        mySqlCommand.Parameters.AddWithValue("_ImagePath", pcmArticleCategory.ImagePath);
                        if (pcmArticleCategory.InUse.ToUpper() == "YES")
                            mySqlCommand.Parameters.AddWithValue("_pcmCategoryInUse", 1);
                        else
                            mySqlCommand.Parameters.AddWithValue("_pcmCategoryInUse", 0);
                        pcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    //update position of pcm article categories
                    UpdatePCMArticleCategoryPosition_V2550(MainServerConnectionString, pcmArticleCategoryList, pcmArticleCategory.IdCreator);
                    AddPCMArticleCategoryImageToPath_V2550(pcmArticleCategory, ImagePath);
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleCategory_V2290(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return pcmArticleCategory;
        }
        /// <summary>
        /// This method is used to update MicroSiga Information
        /// </summary>
        /// <param name="MicroSigainformation">MicroSiga Information.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// 
        public bool IsUpdatePCM_MicroSigaInformation_V2550(List<MicroSigainformation> MicroSigainformation, string MainServerConnectionString)
        {
            List<MicroSiga_Article_taxes> article_taxesList = GetAllArticle_taxes_V2550(MainServerConnectionString);
            List<MicroSiga_Article> articleList = GetAllArticle_V2550(MainServerConnectionString);
            bool isUpdated = true;
            bool flag = true;
            string comments_IPI = string.Empty;
            string comment_NCM = string.Empty;
            try
            {
                foreach (List<MicroSigainformation> itemGroup in MicroSigainformation.GroupBy(g => g.Reference).Select(grp => grp.ToList()))
                {
                    MicroSigainformation item = itemGroup.LastOrDefault();
                    if (item != null)
                    {
                        MicroSigainformation TempmicroSigainformationList = new MicroSigainformation();
                        try
                        {
                            MicroSiga_Article oldarticle = articleList.Where(a => a.Reference.Equals(item.Reference)).FirstOrDefault();
                            if (oldarticle != null)
                            {
                                MicroSiga_Article_taxes oldarticle_taxes = article_taxesList.Where(at => at.IdArticle == oldarticle.IdArticle).DefaultIfEmpty(null).FirstOrDefault();
                                if (oldarticle_taxes != null)
                                {

                                    //if (item.IPI.Equals(oldarticle_taxes.Value.ToString()))
                                    if (Math.Round(Convert.ToDouble(item.IPI), 2) == Math.Round(Convert.ToDouble(oldarticle_taxes.Value.ToString().Replace(',', '.')), 2))
                                    {
                                        TempmicroSigainformationList.IPI = oldarticle_taxes.Value.ToString();
                                        //comments_IPI = "";
                                        comments_IPI = null;
                                    }
                                    else
                                    {
                                        //TempmicroSigainformationList.IPI = item.IPI;
                                        //comments_IPI = "Modified IPI tax from " + Convert.ToDouble(oldarticle_taxes.Value) + "% to " + TempmicroSigainformationList.IPI + "%";

                                        double IPIValue = Math.Round(Convert.ToDouble(item.IPI), 2);
                                        TempmicroSigainformationList.IPI = item.IPI;
                                        comments_IPI = "Modified IPI tax from " + Convert.ToDouble(oldarticle_taxes.Value) + "% to " + item.IPI.Replace(".", ",") + "%";

                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. Message- {0}", item.IPI), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. Message- {0}", TempmicroSigainformationList.IPI), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage- {0}", item.IPI.Replace(",", ".")), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage- {0}", TempmicroSigainformationList.IPI.Replace(",", ".")), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage  ToString()- {0}", item.IPI.ToString().Replace(',', '.')), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage  ToString() - {0}", TempmicroSigainformationList.IPI.ToString().Replace(',', '.')), category: Category.Info, priority: Priority.Low);


                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ******"), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage- {0}", item.IPI.Replace(".", ",")), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage- {0}", TempmicroSigainformationList.IPI.Replace(".", ",")), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage  ToString()- {0}", item.IPI.ToString().Replace('.', ',')), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ReplaceMessage  ToString() - {0}", TempmicroSigainformationList.IPI.ToString().Replace('.', ',')), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300. ******"), category: Category.Info, priority: Priority.Low);

                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300.oldarticle_taxes Message- {0}", oldarticle_taxes.Value), category: Category.Info, priority: Priority.Low);
                                        Log4NetLogger.Logger.Log(string.Format("INFO IsUpdatePCM_MicroSigaInformation_V2300.oldarticle_taxes Message- {0}", oldarticle_taxes.Value.ToString().Replace(',', '.')), category: Category.Info, priority: Priority.Low);

                                    }
                                    if (oldarticle.NCM_Code == null || oldarticle.NCM_Code.Equals(string.Empty))
                                    {
                                        TempmicroSigainformationList.Reference = item.Reference;
                                        TempmicroSigainformationList.NCM = item.NCM;
                                        comment_NCM = "Added NCM code " + item.NCM;
                                    }
                                    else if (item.NCM.Equals(oldarticle.NCM_Code))
                                    {
                                        TempmicroSigainformationList.Reference = oldarticle.Reference;
                                        TempmicroSigainformationList.NCM = oldarticle.NCM_Code;
                                        //comment_NCM = "";
                                        comment_NCM = null;
                                    }
                                    else
                                    {
                                        TempmicroSigainformationList.Reference = item.Reference;
                                        TempmicroSigainformationList.NCM = item.NCM;
                                        comment_NCM = "Modified NCM code from " + oldarticle.NCM_Code + " to " + item.NCM;
                                    }
                                }
                                else
                                {
                                    TempmicroSigainformationList.IPI = item.IPI;
                                    comments_IPI = "Added IPI tax " + Convert.ToDouble(item.IPI) + "%";
                                    if (oldarticle.NCM_Code == null || oldarticle.NCM_Code.Equals(string.Empty))
                                    {
                                        TempmicroSigainformationList.Reference = item.Reference;
                                        TempmicroSigainformationList.NCM = item.NCM;
                                        comment_NCM = "Added NCM code " + item.NCM;
                                    }
                                    else if (item.NCM.Equals(oldarticle.NCM_Code))
                                    {
                                        TempmicroSigainformationList.Reference = oldarticle.Reference;
                                        TempmicroSigainformationList.NCM = oldarticle.NCM_Code;
                                        //comment_NCM = "";
                                        comment_NCM = null;
                                    }
                                    else
                                    {
                                        TempmicroSigainformationList.Reference = item.Reference;
                                        TempmicroSigainformationList.NCM = item.NCM;
                                        comment_NCM = "Modified NCM code from " + oldarticle.NCM_Code + " to " + item.NCM;
                                    }
                                }
                            }
                            if (comment_NCM != null || comments_IPI != null)
                            {
                                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                                {
                                    try
                                    {
                                        mySqlConnection.Open();
                                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_ArticlesNCMCodeUpdate_2180", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                                        mySqlCommand.Parameters.Clear();
                                        if (TempmicroSigainformationList.Reference != null)
                                        {
                                            mySqlCommand.Parameters.AddWithValue("_Reference", TempmicroSigainformationList.Reference);
                                            mySqlCommand.Parameters.AddWithValue("_IPI", TempmicroSigainformationList.IPI);
                                            //mySqlCommand.Parameters.AddWithValue("_IPI", string.IsNullOrEmpty(TempmicroSigainformationList.IPI) ? "0" :TempmicroSigainformationList.IPI);                                      
                                            mySqlCommand.Parameters.AddWithValue("_NCM_Code", TempmicroSigainformationList.NCM);
                                            mySqlCommand.Parameters.AddWithValue("_IdTaxType", 1522);
                                            mySqlCommand.Parameters.AddWithValue("_IdCompany", 20);
                                            mySqlCommand.Parameters.AddWithValue("_comment_IPI", comments_IPI);
                                            mySqlCommand.Parameters.AddWithValue("_comment_NCM", comment_NCM);
                                            mySqlCommand.ExecuteNonQuery();
                                            mySqlConnection.Close();
                                            isUpdated = true;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        flag = false;
                                        mySqlConnection.Close();
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            flag = false;
                            Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCM_MicroSigaInformation_V2300(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                        }

                    }

                }

            }
            catch (Exception ex)
            {

                flag = false;
                Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCM_MicroSigaInformation_V2300(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            if (flag == false)
            {
                isUpdated = false;
            }
            return isUpdated;
        }
        /// <summary>
        /// This method is used to get all article_taxes
        /// </summary>
        /// <param name="ConnectionString">The main server connection string.</param>
        public List<MicroSiga_Article_taxes> GetAllArticle_taxes_V2550(string ConnectionString)
        {
            List<MicroSiga_Article_taxes> article_taxesList = new List<MicroSiga_Article_taxes>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    MySqlCommand Command = new MySqlCommand("PCM_GetAllArticle_Taxes", conn);
                    Command.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            MicroSiga_Article_taxes article_taxes = new MicroSiga_Article_taxes();
                            //  SELECT IdArticleTax,IdCompany,IdArticle,IdTaxType,Value from article_taxes;
                            if (Reader["IdArticleTax"] != DBNull.Value)
                            {
                                article_taxes.IdArticleTax = Convert.ToInt32(Reader["IdArticleTax"].ToString());
                            }
                            if (Reader["IdCompany"] != DBNull.Value)
                            {
                                article_taxes.IdCompany = Convert.ToInt32(Reader["IdCompany"].ToString());
                            }
                            if (Reader["IdArticle"] != DBNull.Value)
                            {
                                article_taxes.IdArticle = Convert.ToInt32(Reader["IdArticle"].ToString());
                            }
                            if (Reader["IdTaxType"] != DBNull.Value)
                            {
                                article_taxes.IdTaxType = Convert.ToInt32(Reader["IdTaxType"].ToString());
                            }
                            if (Reader["Value"] != DBNull.Value)
                            {
                                article_taxes.Value = Convert.ToDouble(Reader["Value"].ToString());
                            }
                            article_taxesList.Add(article_taxes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllArticle_taxes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return article_taxesList;
        }
        /// <summary>
        /// This method is used to get all Article
        /// </summary>
        /// <param name="ConnectionString">The main server connection string.</param>
        public List<MicroSiga_Article> GetAllArticle_V2550(string ConnectionString)
        {
            List<MicroSiga_Article> articleList = new List<MicroSiga_Article>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    MySqlCommand Command = new MySqlCommand("PCM_GetAllArticle", conn);
                    Command.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            MicroSiga_Article article = new MicroSiga_Article();
                            if (Reader["IdArticle"] != DBNull.Value)
                            {
                                article.IdArticle = Convert.ToInt32(Reader["IdArticle"].ToString());
                            }
                            if (Reader["Reference"] != DBNull.Value)
                            {
                                article.Reference = Convert.ToString(Reader["Reference"].ToString());
                            }
                            if (Reader["NCM_Code"] != DBNull.Value)
                            {
                                article.NCM_Code = Convert.ToString(Reader["NCM_Code"].ToString());
                            }
                            articleList.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllArticle_taxes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return articleList;
        }
        
        public bool UpdateDetection_ForAddDetectionViewModeln_V2550(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3 || detectionDetails.IdDetectionType == 4)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup_V2550(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Update_V2330", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionDetails.IdDetections);
                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionDetails.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", detectionDetails.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", detectionDetails.PurchaseQtyMin);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", detectionDetails.PurchaseQtyMax);
                        mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", detectionDetails.IsShareWithCustomer);
                        mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", detectionDetails.IsSparePartOnly);
                        mySqlCommand.Parameters.AddWithValue("_IdScope", detectionDetails.IdScope);
                        if (IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);
                        }

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }

                    //add/update/delete ways, detections, options, spare parts
                    AddUpdateDeleteDetectionFiles_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                    AddUpdateDeleteDetectionLinks_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                    AddUpdateDeleteDetectionImages_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                    AddDeleteCustomersRegionsByDetectionForAddDetectionViewModel_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerListByDetection, detectionDetails.IdDetectionType, detectionDetails.ModifiedBy);
                    AddDetectionLogEntry_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);
                    //
                    AddUpdateDeletePLMDetectionPrices_V2550(MainServerConnectionString, detectionDetails.ModifiedPLMDetectionList, detectionDetails.IdDetections, detectionDetails.ModifiedBy);
                    AddBasePriceListLogEntry_V2550(MainServerConnectionString, detectionDetails.BasePriceLogEntryList);
                    AddCustomerPriceListLogEntry_V2550(MainServerConnectionString, detectionDetails.CustomerPriceLogEntryList);
                    //
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateDetection_V2330(). IdDetection- {0} ErrorMessage- {1}", detectionDetails.IdDetections, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        public void AddDeleteCustomersRegionsByDetectionForAddDetectionViewModel_V2550(string MainServerConnectionString, UInt32 IdDetection, List<CPLCustomer> CustomersList, UInt32 IdDetectiontype, UInt32 IdCreator)
        {
            try
            {
                if (CustomersList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CPLCustomer customersList in CustomersList)
                        {
                            if (customersList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Delete_V2260", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                if (customersList.IdGroup == 0)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", null);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                }
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdCountry", customersList.IdCountry);
                                mySqlCommand.Parameters.AddWithValue("_IdSite", customersList.IdPlant);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (customersList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Insert_V2280", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                if (customersList.IdGroup == 0)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", null);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                }
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdCountry", customersList.IdCountry);
                                mySqlCommand.Parameters.AddWithValue("_IdSite", customersList.IdPlant);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                                mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);
                                mySqlCommand.Parameters.AddWithValue("_IsIncluded", customersList.IsIncluded);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteCustomersRegionsByDetection_V2280(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public bool IsUpdatePCM_DetectionECOSVisibility_Update_V2550(string MainServerConnectionString, DetectionDetails detectionDetails)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (detectionDetails.IdDetections >= 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionECOSVisibility_Update__V2340", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", detectionDetails.PurchaseQtyMin);
                            mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", detectionDetails.PurchaseQtyMax);
                            mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", detectionDetails.IsShareWithCustomer);
                            mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", detectionDetails.IsSparePartOnly);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionDetails.IdDetections);
                            mySqlCommand.Parameters.AddWithValue("_IdStatus", detectionDetails.IdStatus);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description.Trim());
                            mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                            mySqlCommand.ExecuteNonQuery();
                            mySqlConnection.Close();

                            isUpdated = true;
                        }
                        AddDetectionLogEntry_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);

                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCM_DetectionECOSVisibility_Update_V2550(). IdArticle- {0} ErrorMessage- {1}", detectionDetails.IdDetections, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        public bool IsUpdatePCMArticle_V2550(string MainServerConnectionString, uint IdPCMArticleCategory, Articles Article, string PCMArticleImagePath, string PCMArticleDocPath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (Article != null && Article.IdArticle > 0 && IdPCMArticleCategory > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_PCMArticle_Update", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);
                            mySqlCommand.Parameters.AddWithValue("_idArticle", Article.IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdPCMStatus", Article.IdPCMStatus);
                            mySqlCommand.Parameters.AddWithValue("_IdModifier", Article.IdModifier);
                            mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", Article.IsRtfText);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription", Article.PCMDescription);
                            mySqlCommand.Parameters.AddWithValue("_IsImageShareWithCustomer", Article.IsImageShareWithCustomer);
                            mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", Article.PurchaseQtyMin);
                            mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", Article.PurchaseQtyMax);
                            mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", Article.IsShareWithCustomer);
                            mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", Article.IsSparePartOnly);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription_es", Article.PCMDescription_es);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription_fr", Article.PCMDescription_fr);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription_ro", Article.PCMDescription_ro);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription_zh", Article.PCMDescription_zh);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription_pt", Article.PCMDescription_pt);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription_ru", Article.PCMDescription_ru);

                            mySqlCommand.ExecuteNonQuery();

                            isUpdated = true;
                        }

                        AddUpdateDeleteArticleCompatibilities_V2550(MainServerConnectionString, Article.IdArticle, Article.ArticleCompatibilityList);
                        AddPCMArticleLogEntry_V2550(MainServerConnectionString, Article.IdArticle, Article.PCMArticleLogEntiryList);
                        AddUpdateDeletePCMArticleImages_V2550(MainServerConnectionString, Article.IdArticle, Article.PCMArticleImageList, PCMArticleImagePath, Article.Reference);
                        AddUpdateDeletePCMArticleDocs_V2550(MainServerConnectionString, Article.IdArticle, Article.PCMArticleAttachmentList, PCMArticleDocPath, Article.Reference);
                        AddWarehouseArticleLogEntry_V2550(MainServerConnectionString, Article.IdArticle, Article.WarehouseArticleLogEntiryList);
                        UpdateWarehouseArticle_V2550(MainServerConnectionString, Article.IdArticle, Article.WarehouseArticle, Article.IdModifier);
                        AddUpdateDeletePLMArticlePrices_V2550(MainServerConnectionString, Article.ModifiedPLMArticleList, Article.IdArticle, Article.IdModifier);
                        AddBasePriceListLogEntry_V2550(MainServerConnectionString, Article.BasePriceLogEntryList);
                        AddCustomerPriceListLogEntry_V2550(MainServerConnectionString, Article.CustomerPriceLogEntryList);
                        AddArticlesByCustomer_V2550(MainServerConnectionString, Article.IdCreator, Article.IdArticle, Article.ArticleCustomerList, Article.IdModifier);
                        isUpdated = true;
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticle_V2260(). IdArticle- {0} ErrorMessage- {1}", Article.IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }
        public void UpdateWarehouseArticle_V2550(string MainServerConnectionString, UInt32 IdArticle, Article WarehouseArticle, uint? IdModifier)
        {
            try
            {
                if (WarehouseArticle != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();


                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_WarehouseArticle_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_idArticle", WarehouseArticle.IdArticle);
                        mySqlCommand.Parameters.AddWithValue("_IdModifier", IdModifier);
                        mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_ArticleName", WarehouseArticle.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", WarehouseArticle.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", WarehouseArticle.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", WarehouseArticle.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", WarehouseArticle.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", WarehouseArticle.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", WarehouseArticle.Description_ru);

                        mySqlCommand.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateWarehouseArticle_V2550(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddUpdateDeletePLMArticlePrices_V2550(string MainServerConnectionString, List<PLMArticlePrice> PLMArticlePriceList, UInt32 IdArticle, UInt32? IdModifier)
        {
            try
            {
                if (PLMArticlePriceList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PLMArticlePrice pLMArticlePrice in PLMArticlePriceList)
                        {
                            if (pLMArticlePrice.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                if (pLMArticlePrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_base_price_list_by_articles_Delete", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMArticlePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMArticlePrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_customer_price_list_by_articles_Delete", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMArticlePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (pLMArticlePrice.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (pLMArticlePrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_base_price_list_by_articles_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMArticlePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMArticlePrice.IdRule);
                                    pLMArticlePrice.RuleValue = pLMArticlePrice.RuleValue;
                                    if (pLMArticlePrice.IdRule == 1518)
                                    {
                                        pLMArticlePrice.RuleValue = 0;
                                    }
                                    else if (pLMArticlePrice.IdRule == 308)
                                    {
                                        if (pLMArticlePrice.RuleValue == 0)
                                        {
                                            pLMArticlePrice.RuleValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMArticlePrice.IdRule == 0)
                                        {
                                            pLMArticlePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMArticlePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMArticlePrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_customer_price_list_by_articles_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMArticlePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMArticlePrice.IdRule);
                                    pLMArticlePrice.RuleValue = pLMArticlePrice.RuleValue;
                                    if (pLMArticlePrice.IdRule == 1518)
                                    {
                                        pLMArticlePrice.RuleValue = 0;
                                    }
                                    else if (pLMArticlePrice.IdRule == 308)
                                    {
                                        if (pLMArticlePrice.RuleValue == 0)
                                        {
                                            pLMArticlePrice.RuleValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMArticlePrice.IdRule == 0)
                                        {
                                            pLMArticlePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMArticlePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (pLMArticlePrice.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                if (pLMArticlePrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_base_price_list_by_articles_Insert", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMArticlePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                    pLMArticlePrice.RuleValue = pLMArticlePrice.RuleValue;
                                    if (pLMArticlePrice.IdRule == 1518)
                                    {
                                        pLMArticlePrice.RuleValue = 0;
                                    }
                                    else if (pLMArticlePrice.IdRule == 308)
                                    {
                                        if (pLMArticlePrice.RuleValue == 0)
                                        {
                                            pLMArticlePrice.RuleValue = null;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMArticlePrice.IdRule == 0)
                                        {
                                            pLMArticlePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMArticlePrice.IdRule);
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMArticlePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_MaxCost", 0);
                                    mySqlCommand.Parameters.AddWithValue("_Royalty", 0);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMArticlePrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_customer_price_list_by_articles_Insert", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMArticlePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                    pLMArticlePrice.RuleValue = pLMArticlePrice.RuleValue;
                                    if (pLMArticlePrice.IdRule == 1518)
                                    {
                                        pLMArticlePrice.RuleValue = 0;
                                    }
                                    else if (pLMArticlePrice.IdRule == 308)
                                    {
                                        if (pLMArticlePrice.RuleValue == 0)
                                        {
                                            pLMArticlePrice.RuleValue = null;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMArticlePrice.IdRule == 0)
                                        {
                                            pLMArticlePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMArticlePrice.IdRule);
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMArticlePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeletePLMArticlePrices(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddArticlesByCustomer_V2550(string MainServerConnectionString, UInt32 IdCreator, UInt64 IdArticle, List<ArticleCustomer> ArticleList, uint? IdModifier)
        {
            try
            {
                if (ArticleList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (ArticleCustomer Article in ArticleList)
                        {
                            if (Article.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_idArticle_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdArticleCustomerReferences", Article.IdArticleCustomerReferences);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (Article.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_idArticle_Insert_V2350", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                if (Article.IdGroup > 0)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", Article.IdGroup);

                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", null);
                                }
                                if (Article.IdCountry > 0)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCountry", Article.IdCountry);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdCountry", null);
                                }
                                //  mySqlCommand.Parameters.AddWithValue("_IdCustomer", Article.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", Article.IdRegion);
                                //  mySqlCommand.Parameters.AddWithValue("_IdCountry", Article.IdCountry);


                                mySqlCommand.Parameters.AddWithValue("_IdSite", Article.IdPlant);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdCreator);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_Reference", Article.ReferenceCustomer.Trim());
                                //mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (Article.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (Article.IdArticleList > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_idArticle_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdArticleCustomerReferences", Article.IdArticleCustomerReferences);
                                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                    if (Article.IdGroup > 0)
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdCustomer", Article.IdGroup);

                                    }
                                    else
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdCustomer", null);
                                    }
                                    if (Article.IdCountry > 0)
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdCountry", Article.IdCountry);
                                    }
                                    else
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdCountry", null);
                                    }
                                    // mySqlCommand.Parameters.AddWithValue("_IdCustomer", Article.IdGroup);
                                    mySqlCommand.Parameters.AddWithValue("_IdRegion", Article.IdRegion);
                                    //  mySqlCommand.Parameters.AddWithValue("_IdCountry", Article.IdCountry);


                                    mySqlCommand.Parameters.AddWithValue("_IdSite", Article.IdPlant);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdCreator);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_Reference", Article.ReferenceCustomer.Trim());
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddArticlesByCustomer_V2350(). IdBasePriceList- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public DetectionDetails AddDetectionForEditDetectionViewModel_V2550(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3 || detectionDetails.IdDetectionType == 4)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup_V2550(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Insert_V2330", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_createdBy", detectionDetails.CreatedBy);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", detectionDetails.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", detectionDetails.PurchaseQtyMin);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", detectionDetails.PurchaseQtyMax);
                        mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", detectionDetails.IsShareWithCustomer);
                        mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", detectionDetails.IsSparePartOnly);
                        mySqlCommand.Parameters.AddWithValue("_IdScope", detectionDetails.IdScope);
                        if (detectionDetails.IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", null);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }

                        detectionDetails.IdDetections = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    if (detectionDetails.IdDetections > 0)
                    {
                        AddDetectionAttachedDocByDetectionForEditDetectionViewModel_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                        AddDetectionAttachedLinkByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                        AddDetectionImageByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                        AddCustomersRegionsByDetectionForEditDetectionViewModel_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerListByDetection, detectionDetails.IdDetectionType, detectionDetails.CreatedBy);
                        AddDetectionLogEntry_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);
                        AddUpdateDeletePLMDetectionPrices_V2550(MainServerConnectionString, detectionDetails.ModifiedPLMDetectionList, detectionDetails.IdDetections, detectionDetails.CreatedBy);
                        AddRelatedModulesByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.ProductTypesList);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetection_V2400(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return detectionDetails;
        }

        public void AddDetectionAttachedDocByDetectionForEditDetectionViewModel_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedDoc> DetectionAttachedDocList, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedDoc detectionAttachedDocList in DetectionAttachedDocList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Insert_V2340", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedDocList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);
                            mySqlCommand.Parameters.AddWithValue("_IdAttachmentType", detectionAttachedDocList.AttachmentType.IdLookupValue);

                            detectionAttachedDocList.IdDetectionAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                            {
                                AddDetectionAttachedDocToPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionAttachedDocByDetection_V2340(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddCustomersRegionsByDetectionForEditDetectionViewModel_V2550(string MainServerConnectionString, UInt32 IdDetection, List<CPLCustomer> CustomerList, UInt32 IdDetectiontype, UInt32 IdCreator)
        {
            try
            {
                if (CustomerList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CPLCustomer customerList in CustomerList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Insert_V2280", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            if (customerList.IdGroup == 0)
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", null);
                            }
                            else
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                            }
                            mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                            mySqlCommand.Parameters.AddWithValue("_IdCountry", customerList.IdCountry);
                            mySqlCommand.Parameters.AddWithValue("_IdSite", customerList.IdPlant);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);
                            mySqlCommand.Parameters.AddWithValue("_IsIncluded", customerList.IsIncluded);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCustomersRegionsByDetection_V2280(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddRelatedModulesByDetection_V2550(string connectionString, UInt32 IdDetection, List<ProductTypes> productTypeList)
        {
            try
            {
                if (productTypeList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypes productTypes in productTypeList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Related_Modules_By_Detection_Insert_V2400", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", productTypes.IdTemplate);
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", productTypes.IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_OrderNumber", productTypes.OrderNumber);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddRelatedModulesByDetection(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public FreePlugins AddUpdateFreePlugins_V2550(string PLMConnectionString, FreePlugins freePlugins)
        {
            try
            {
                int itemCount;
                if (freePlugins != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(PLMConnectionString))
                    {
                        mySqlConnection.Open();
                        if (freePlugins.TransactionOperation == ModelBase.TransactionOperations.Add)
                        {
                            MySqlCommand command = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                            command.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);


                            List<int> selectedRegion = freePlugins.SelectedRegion;
                            List<int> selectedCountry = freePlugins.SelectedCountry;
                            List<UInt32> selectedPlant = freePlugins.SelectedPlant;

                            if (selectedRegion != null && selectedCountry != null && selectedPlant != null)
                            {
                                itemCount = Math.Max(selectedRegion.Count, Math.Max(selectedCountry.Count, selectedPlant.Count));

                                for (int i = 0; i < itemCount; i++)
                                {
                                    int region = i < selectedRegion.Count ? selectedRegion[i] : 0;
                                    int country = i < selectedCountry.Count ? selectedCountry[i] : 0;
                                    UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                    MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                    subCommand.CommandType = CommandType.StoredProcedure;

                                    subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                    subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                    subCommand.Parameters.AddWithValue("_IdRegion", region);
                                    subCommand.Parameters.AddWithValue("_IdCountry", country);
                                    subCommand.Parameters.AddWithValue("_IdSite", plant);

                                    subCommand.ExecuteNonQuery();
                                }

                            }
                            else
                            {
                                if (selectedRegion == null && selectedCountry != null && selectedPlant != null)
                                {
                                    itemCount = Math.Max(selectedCountry.Count, selectedPlant.Count);
                                    for (int i = 0; i < itemCount; i++)
                                    {

                                        int country = i < selectedCountry.Count ? selectedCountry[i] : 0;
                                        UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                        subCommand.Parameters.AddWithValue("_IdRegion", null);
                                        subCommand.Parameters.AddWithValue("_IdCountry", country);
                                        subCommand.Parameters.AddWithValue("_IdSite", plant);

                                        subCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (selectedRegion == null && selectedCountry == null && selectedPlant != null)
                                {
                                    itemCount = selectedPlant.Count;
                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                        subCommand.Parameters.AddWithValue("_IdRegion", null);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", plant);

                                        subCommand.ExecuteNonQuery();
                                    }
                                }

                                else if (selectedRegion != null && selectedCountry == null && selectedPlant == null)
                                {
                                    itemCount = selectedRegion.Count;
                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;


                                        MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", null);

                                        subCommand.ExecuteNonQuery();
                                    }
                                }

                                else if (selectedRegion != null && selectedCountry == null && selectedPlant != null)
                                {
                                    itemCount = Math.Max(selectedRegion.Count, selectedPlant.Count);

                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;

                                        UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", plant);

                                        subCommand.ExecuteNonQuery();
                                    }
                                }

                                else if (selectedPlant == null && selectedCountry == null && selectedRegion != null)
                                {
                                    itemCount = selectedRegion.Count;

                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;


                                        MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", null);

                                        subCommand.ExecuteNonQuery();
                                    }
                                }

                                else if (selectedRegion == null && selectedCountry == null && selectedPlant != null)
                                {
                                    itemCount = selectedPlant.Count;
                                    for (int i = 0; i < itemCount; i++)
                                    {

                                        UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                        subCommand.Parameters.AddWithValue("_IdRegion", null);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", plant);

                                        subCommand.ExecuteNonQuery();
                                    }
                                }

                                else if (selectedRegion == null && selectedCountry == null && selectedPlant == null)
                                {
                                    MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                    subCommand.CommandType = CommandType.StoredProcedure;

                                    subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                    subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                    subCommand.Parameters.AddWithValue("_IdRegion", null);
                                    subCommand.Parameters.AddWithValue("_IdCountry", null);
                                    subCommand.Parameters.AddWithValue("_IdSite", null);

                                    subCommand.ExecuteNonQuery();
                                }

                                else if (selectedPlant == null && selectedCountry != null && selectedRegion != null)
                                {
                                    itemCount = Math.Max(selectedRegion.Count, selectedCountry.Count);

                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;
                                        int country = i < selectedCountry.Count ? selectedCountry[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", country);
                                        subCommand.Parameters.AddWithValue("_IdSite", null);

                                        subCommand.ExecuteNonQuery();
                                    }
                                }






                            }




                        }

                        else if (freePlugins.TransactionOperation == ModelBase.TransactionOperations.Update)
                        {
                            MySqlCommand command1 = new MySqlCommand("PCM_DeleteFreePlugin_V2440", mySqlConnection);
                            command1.CommandType = CommandType.StoredProcedure;
                            command1.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                            command1.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                            if (freePlugins.IdRegion == 0)
                            {
                                command1.Parameters.AddWithValue("_IdRegion", null);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("_IdRegion", freePlugins.IdRegionPrevious);
                            }
                            if (freePlugins.IdCountry == 0)
                            {
                                command1.Parameters.AddWithValue("_IdCountry", null);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("_IdCountry", freePlugins.IdCountryPrevious);
                            }
                            if (freePlugins.IdSite == 0)
                            {
                                command1.Parameters.AddWithValue("_IdSite", null);
                            }
                            else
                            {
                                command1.Parameters.AddWithValue("_IdSite", freePlugins.IdPlantPrevious);
                            }

                            command1.ExecuteNonQuery();
                            mySqlConnection.Close();
                            mySqlConnection.Open();

                            MySqlCommand command = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                            command.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                            List<int> selectedRegion = freePlugins.SelectedRegion;
                            List<int> selectedCountry = freePlugins.SelectedCountry;
                            List<UInt32> selectedPlant = freePlugins.SelectedPlant;

                            if (selectedRegion != null && selectedCountry != null && selectedPlant != null)
                            {
                                itemCount = Math.Max(selectedRegion.Count, Math.Max(selectedCountry.Count, selectedPlant.Count));

                                for (int i = 0; i < itemCount; i++)
                                {
                                    int region = i < selectedRegion.Count ? selectedRegion[i] : 0;
                                    int country = i < selectedCountry.Count ? selectedCountry[i] : 0;
                                    UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                    MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                    subCommand.CommandType = CommandType.StoredProcedure;

                                    subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                    subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                    subCommand.Parameters.AddWithValue("_IdRegion", region);
                                    subCommand.Parameters.AddWithValue("_IdCountry", country);
                                    subCommand.Parameters.AddWithValue("_IdSite", plant);


                                    subCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                if (selectedRegion == null && selectedCountry != null && selectedPlant != null)
                                {
                                    itemCount = Math.Max(selectedCountry.Count, selectedPlant.Count);
                                    for (int i = 0; i < itemCount; i++)
                                    {

                                        int country = i < selectedCountry.Count ? selectedCountry[i] : 0;
                                        UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                        subCommand.Parameters.AddWithValue("_IdRegion", null);
                                        subCommand.Parameters.AddWithValue("_IdCountry", country);
                                        subCommand.Parameters.AddWithValue("_IdSite", plant);


                                        subCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (selectedRegion == null && selectedCountry == null && selectedPlant != null)
                                {
                                    itemCount = selectedPlant.Count;
                                    for (int i = 0; i < itemCount; i++)
                                    {

                                        UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                        subCommand.Parameters.AddWithValue("_IdRegion", null);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", plant);


                                        subCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (selectedRegion != null && selectedCountry == null && selectedPlant == null)
                                {
                                    itemCount = selectedRegion.Count;

                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;


                                        MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", null);


                                        subCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (selectedRegion != null && selectedCountry == null && selectedPlant != null)
                                {
                                    itemCount = Math.Max(selectedRegion.Count, selectedPlant.Count);

                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;

                                        UInt32 plant = i < selectedPlant.Count ? selectedPlant[i] : 0;

                                        MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", plant);


                                        subCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (selectedPlant == null && selectedCountry != null && selectedRegion != null)
                                {
                                    itemCount = Math.Max(selectedRegion.Count, selectedCountry.Count);

                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;
                                        int country = i < selectedCountry.Count ? selectedCountry[i] : 0;


                                        MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", country);
                                        subCommand.Parameters.AddWithValue("_IdSite", null);


                                        subCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (selectedPlant == null && selectedCountry == null && selectedRegion != null)
                                {
                                    itemCount = selectedRegion.Count;
                                    for (int i = 0; i < itemCount; i++)
                                    {
                                        int region = i < selectedRegion.Count ? selectedRegion[i] : 0;


                                        MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                        subCommand.CommandType = CommandType.StoredProcedure;

                                        subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                        subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                        subCommand.Parameters.AddWithValue("_IdRegion", region);
                                        subCommand.Parameters.AddWithValue("_IdCountry", null);
                                        subCommand.Parameters.AddWithValue("_IdSite", null);


                                        subCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (selectedRegion == null && selectedCountry == null && selectedPlant == null)
                                {
                                    MySqlCommand subCommand = new MySqlCommand("PCM_UpdateFreePlugin_V2430", mySqlConnection);
                                    subCommand.CommandType = CommandType.StoredProcedure;

                                    subCommand.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                    subCommand.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);

                                    subCommand.Parameters.AddWithValue("_IdRegion", null);
                                    subCommand.Parameters.AddWithValue("_IdCountry", null);
                                    subCommand.Parameters.AddWithValue("_IdSite", null);

                                    subCommand.ExecuteNonQuery();
                                }


                            }

                        }
                        else if (freePlugins.TransactionOperation == ModelBase.TransactionOperations.Delete)
                        {
                            MySqlCommand command = new MySqlCommand("PCM_DeleteFreePlugin_V2440", mySqlConnection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                            command.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                            if (freePlugins.IdRegion == 0)
                            {
                                command.Parameters.AddWithValue("_IdRegion", null);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("_IdRegion", freePlugins.IdRegion);
                            }
                            if (freePlugins.IdCountry == 0)
                            {
                                command.Parameters.AddWithValue("_IdCountry", null);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("_IdCountry", freePlugins.IdCountry);
                            }
                            if (freePlugins.IdSite == 0)
                            {
                                command.Parameters.AddWithValue("_IdSite", null);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("_IdSite", freePlugins.IdSite);
                            }

                            command.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateFreePlugins(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return freePlugins;
        }
        public bool AddUpdateFreePluginsForAddPluginsViewModel_V2550(string PLMConnectionString, List<FreePlugins> freePluginsList)
        {
            int itemCount;
            bool result = false;
            try
            {
                foreach (FreePlugins freePlugins in freePluginsList)
                {
                    if (freePlugins != null)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(PLMConnectionString))
                        {
                            mySqlConnection.Open();
                            if (freePlugins.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand command = new MySqlCommand("PCM_InsertFreePlugins", mySqlConnection);
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                command.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                if (freePlugins.IdRegion == 0)
                                {
                                    command.Parameters.AddWithValue("_IdRegion", null);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("_IdRegion", freePlugins.IdRegion);
                                }
                                if (freePlugins.IdCountry == 0)
                                {
                                    command.Parameters.AddWithValue("_IdCountry", null);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("_IdCountry", freePlugins.IdCountry);
                                }
                                if (freePlugins.IdSite == 0)
                                {
                                    command.Parameters.AddWithValue("_IdSite", null);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("_IdSite", freePlugins.IdSite);
                                }
                                command.ExecuteNonQuery();
                                result = true;
                            }
                            else if (freePlugins.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                //MySqlCommand command1 = new MySqlCommand("PCM_DeleteFreePlugin_V2440", mySqlConnection);
                                //command1.CommandType = CommandType.StoredProcedure;
                                //command1.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                //command1.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                //if (freePlugins.IdRegion == 0)
                                //{
                                //    command1.Parameters.AddWithValue("_IdRegion", null);
                                //}
                                //else
                                //{
                                //    command1.Parameters.AddWithValue("_IdRegion", freePlugins.IdRegionPrevious);
                                //}
                                //if (freePlugins.IdCountry == 0)
                                //{
                                //    command1.Parameters.AddWithValue("_IdCountry", null);
                                //}
                                //else
                                //{
                                //    command1.Parameters.AddWithValue("_IdCountry", freePlugins.IdCountryPrevious);
                                //}
                                //if (freePlugins.IdSite == 0)
                                //{
                                //    command1.Parameters.AddWithValue("_IdSite", null);
                                //}
                                //else
                                //{
                                //    command1.Parameters.AddWithValue("_IdSite", freePlugins.IdPlantPrevious);
                                //}

                                //command1.ExecuteNonQuery();
                                result = true;
                            }
                            else if (freePlugins.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand command = new MySqlCommand("PCM_DeleteFreePlugin_V2440", mySqlConnection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("_IdPlugin", freePlugins.IdPlugin);
                                command.Parameters.AddWithValue("_IdCustomer", freePlugins.IdCustomer);
                                if (freePlugins.IdRegion == 0)
                                {
                                    command.Parameters.AddWithValue("_IdRegion", null);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("_IdRegion", freePlugins.IdRegion);
                                }
                                if (freePlugins.IdCountry == 0)
                                {
                                    command.Parameters.AddWithValue("_IdCountry", null);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("_IdCountry", freePlugins.IdCountry);
                                }
                                if (freePlugins.IdSite == 0)
                                {
                                    command.Parameters.AddWithValue("_IdSite", null);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("_IdSite", freePlugins.IdSite);
                                }
                                command.ExecuteNonQuery();
                                result = true;
                            }

                            mySqlConnection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateFreePlugins(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return result;
        }

        public bool AddHardLockLicense_V2550(string connectionString, UInt32 IdArticle, List<HardLockPlugins> pluginList)
        {
            bool isAdded = false;
            try
            {
                if (pluginList != null && pluginList.Count > 0)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        foreach (HardLockPlugins item in pluginList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("AddHardLockLicense_V2450", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdPlugin", item.IdPlugin);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                        isAdded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddHardLockLicense_V2450(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isAdded;
        }

        public bool DeleteSupportedPluginForHardLockLicense_V2550(string connectionString, UInt32 idPlugin, UInt32 idArticle)
        {
            bool isDeleted = false;

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DeleteSupportedPluginForHardLockLicense_V2450", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdArticle", idArticle);
                    mySqlCommand.Parameters.AddWithValue("_IdPlugin", idPlugin);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                    isDeleted = true;
                }


            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteSupportedPluginForHardLockLicense_V2450(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isDeleted;
        }

        public bool AddHardLockPlugin_V2550(string connectionString, UInt32 idPlugin, string Name)
        {
            bool isAdded = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_AddHardLockPlugin_V2450", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.Parameters.AddWithValue("_IdPlugin", idPlugin);
                    mySqlCommand.Parameters.AddWithValue("_Name", Name);
                    mySqlCommand.ExecuteNonQuery();

                    mySqlConnection.Close();
                    isAdded = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddHardLockPlugin_V2450(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isAdded;
        }

        public bool AddDeleteArticleCategoryMapping_V2550(string connectionString, List<ArticleCategorieMapping> ArticleCategoryMappingList)
        {
            bool isUpdated = false;
            TransactionScope transactionScope = null;

            using (transactionScope = new TransactionScope())
            {
                try
                {
                    if (ArticleCategoryMappingList != null)
                    {
                        foreach (var item in ArticleCategoryMappingList)
                        {
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                                {
                                    mySqlConnection.Open();
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_AddCategoryMapping", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdWarehouseCategory", item.IdWMSArticleCategory);
                                    mySqlCommand.Parameters.AddWithValue("_IdPCMCategory", item.IdPCMArticleCategory);
                                    mySqlCommand.ExecuteNonQuery();
                                    mySqlConnection.Close();

                                }
                            }
                            if (item.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                                {
                                    mySqlConnection.Open();
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DeleteCategoryMapping", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdWarehouseCategory", item.IdWMSArticleCategory);
                                    mySqlCommand.Parameters.AddWithValue("_IdPCMCategory", item.IdPCMArticleCategory);
                                    mySqlCommand.ExecuteNonQuery();
                                    mySqlConnection.Close();

                                }
                            }
                        }
                    }
                    isUpdated = true;
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDeleteArticleCategoryMapping(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return isUpdated;
        }
        public Discounts AddDiscount_V2550(Discounts discount, string MainServerConnectionString)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_AddDiscounts", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_Name", discount.Name.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_es", discount.Name_es.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", discount.Name_fr.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", discount.Name_pt.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", discount.Name_ro.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", discount.Name_zh.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", discount.Name_ru.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description", discount.Description.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_es", discount.Description_es.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", discount.Description_fr.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", discount.Description_ro.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", discount.Description_zh.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", discount.Description_pt.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", discount.Description_ru.Trim());
                        mySqlCommand.Parameters.AddWithValue("_StartDate", discount.StartDate);
                        mySqlCommand.Parameters.AddWithValue("_EndDate", discount.EndDate);
                        mySqlCommand.Parameters.AddWithValue("_IdScope", discount.IdScope);
                        mySqlCommand.Parameters.AddWithValue("_IdPlatform", discount.IdPlatform);
                        mySqlCommand.Parameters.AddWithValue("_Value", discount.Value);
                        if (discount.IsReadOnly == true)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IsReadOnly", 1);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IsReadOnly", 0);
                        }
                        if (discount.InUse.ToLower() == "no")
                        {
                            mySqlCommand.Parameters.AddWithValue("_InUse", 0);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_InUse", 1);
                        }
                        mySqlCommand.Parameters.AddWithValue("_createdBy", discount.IdCreator);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        discount.Id = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (discount.DiscountArticles != null)
                    {
                        AddDiscountArticles_V2550(MainServerConnectionString, discount);
                    }
                    if (discount.PlantList != null)
                    {
                        AddDiscountPlants_V2550(MainServerConnectionString, discount);
                    }
                    if (discount.DiscountLogEntryList != null)
                    {
                        AddDiscountChangeLog_V2550(discount, MainServerConnectionString);
                    }
                    if (discount.NewDiscountCustomer != null)
                    {
                        AddCustomersByCustomerDiscount_V2550(MainServerConnectionString, discount.Id, discount.NewDiscountCustomer, discount.IdCreator);
                    }
                    if (discount.DiscountCommentsList != null)
                    {
                        AddDiscountCommentsList_V2550(discount, MainServerConnectionString);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();

                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDiscount_V2470(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return discount;
        }
        public void AddDiscountArticles_V2550(string MainServerConnectionString, Discounts discount)
        {
            try
            {
                if (discount.DiscountArticles != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (DiscountArticles DiscountArticles in discount.DiscountArticles)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_AddDiscountArticles", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                            mySqlCommand.Parameters.AddWithValue("_IdArticle", DiscountArticles.IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_Value", DiscountArticles.Value);
                            mySqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDiscountArticles(). IdCustomerDiscount- {0} ErrorMessage- {1}", discount.Id, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddDiscountPlants_V2550(string MainServerConnectionString, Discounts discount)
        {
            try
            {
                if (discount.PlantList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (Site Plant in discount.PlantList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_AddDiscountPlants", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                            mySqlCommand.Parameters.AddWithValue("_IdSite", Plant.IdSite);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", discount.CreationDate);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", discount.IdCreator);
                            mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_ModifiedBy", discount.IdModifier);
                            mySqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDiscountArticles(). IdCustomerDiscount- {0} ErrorMessage- {1}", discount.Id, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddDiscountChangeLog_V2550(Discounts discount, string MainServerConnectionString)
        {

            try
            {

                if (discount.DiscountLogEntryList != null)
                {
                    foreach (DiscountLogEntry DiscountLogEntry in discount.DiscountLogEntryList)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByDiscount_Insert_V2470", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", DiscountLogEntry.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", DiscountLogEntry.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", DiscountLogEntry.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 258);

                            mySqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDiscountChangeLog_V2470(). IdCustomerDiscount- {0} ErrorMessage- {1}", discount.Id, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

        }

        public void AddCustomersByCustomerDiscount_V2550(string MainServerConnectionString, int customer_DiscountId, List<DiscountCustomers> CustomerList, uint IdCreator)
        {
            try
            {
                if (CustomerList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DiscountCustomers Customer in CustomerList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CustomerDiscountListCustomers_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", customer_DiscountId);
                            if (Customer.IdGroup == 0)
                            {
                                Customer.IdGroup = null;
                            }
                            mySqlCommand.Parameters.AddWithValue("_IdCustomer", Customer.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_IdRegion", Customer.IdRegion);
                            mySqlCommand.Parameters.AddWithValue("_IdCountry", Customer.IdCountry);
                            mySqlCommand.Parameters.AddWithValue("_IdSite", Customer.IdPlant);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdCreator);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            Customer.IdCustomerDiscountCustomer = Convert.ToUInt32(mySqlCommand.ExecuteScalar());
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCustomersByCustomerDiscount(). IdCustomerDiscount- {0} ErrorMessage- {1}", customer_DiscountId, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddDiscountCommentsList_V2550(Discounts discount, string MainServerConnectionString)
        {

            try
            {

                if (discount.DiscountLogEntryList != null)
                {
                    foreach (DiscountLogEntry DiscountLogEntry in discount.DiscountCommentsList)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByDiscount_Insert_V2470", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", DiscountLogEntry.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", DiscountLogEntry.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", DiscountLogEntry.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 257);

                            mySqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDiscountChangeLog_V2470(). IdCustomerDiscount- {0} ErrorMessage- {1}", discount.Id, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

        }
        public Discounts UpdateDiscount_V2550(Discounts discount, Discounts prevDiscount, string MainServerConnectionString)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_EditDiscounts", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                        mySqlCommand.Parameters.AddWithValue("_Name", discount.Name.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_es", discount.Name_es.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", discount.Name_fr.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", discount.Name_pt.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", discount.Name_ro.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", discount.Name_zh.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", discount.Name_ru.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description", discount.Description.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_es", discount.Description_es.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", discount.Description_fr.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", discount.Description_ro.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", discount.Description_zh.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", discount.Description_pt.Trim());
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", discount.Description_ru.Trim());
                        mySqlCommand.Parameters.AddWithValue("_StartDate", discount.StartDateNew);
                        mySqlCommand.Parameters.AddWithValue("_EndDate", discount.EndDateNew);
                        mySqlCommand.Parameters.AddWithValue("_IdScope", discount.IdScope);
                        mySqlCommand.Parameters.AddWithValue("_IdPlatform", discount.IdPlatform);
                        mySqlCommand.Parameters.AddWithValue("_Value", discount.Value);
                        if (discount.IsReadOnly == true)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IsReadOnly", 1);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IsReadOnly", 0);
                        }
                        if (discount.InUse.ToLower() == "no")
                        {
                            mySqlCommand.Parameters.AddWithValue("_InUse", 0);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_InUse", 1);
                        }
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", discount.IdModifier);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        discount.Id = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                        mySqlConnection.Close();
                    }
                    if (discount.DiscountArticles == null && prevDiscount.DiscountArticles != null)//Scope has been changed from product to order
                    {
                        DeleteDiscountArticles_V2550(MainServerConnectionString, prevDiscount.Id, prevDiscount.DiscountArticles);
                    }
                    if (discount.DeletedPlantList != null || discount.AddedPlantList != null)//If some of the Plants are added or deleted
                    {
                        AddDeletePlants_Discounts_V2550(MainServerConnectionString, discount);
                    }
                    if (discount.DiscountArticles != null && prevDiscount.DiscountArticles != null)//Scope has NOt been changed from product to order
                    {
                        UpdateDiscountArticles_V2550(MainServerConnectionString, discount);
                        foreach (var item in prevDiscount.DiscountArticles)
                        {
                            if (!discount.DiscountArticles.Any(i => i.IdArticle == item.IdArticle))
                            {
                                if (discount.DeletedDiscountArticles == null)
                                    discount.DeletedDiscountArticles = new List<DiscountArticles>();
                                discount.DeletedDiscountArticles.Add(prevDiscount.DiscountArticles.Where(i => i.IdArticle == item.IdArticle).FirstOrDefault());
                            }
                        }
                        if (discount.DeletedDiscountArticles != null)
                            DeleteDiscountArticles_V2550(MainServerConnectionString, discount.Id, discount.DeletedDiscountArticles);
                    }
                    if (discount.DiscountArticles != null && prevDiscount.DiscountArticles == null)//Scope has been changed from order to product
                    {
                        AddDiscountArticles_V2550(MainServerConnectionString, discount);
                    }
                    if (discount.UpdateDiscountCustomer != null)
                        AddUpdateDeleteCustomersByDicountCustomer_V2550(MainServerConnectionString, discount.Id, discount.UpdateDiscountCustomer, discount.IdModifier);


                    if (discount.DiscountLogEntryList != null)
                    {
                        AddDiscountChangeLog_V2550(discount, MainServerConnectionString);
                    }
                    if (discount.DiscountCommentsList != null)
                    {
                        AddUpdateDeleteDiscountComments_V2550(MainServerConnectionString, discount.Id, discount.DiscountCommentsList);
                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateDiscount_V2470(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return discount;
        }

        public void DeleteDiscountArticles_V2550(string MainServerConnectionString, int customer_DiscountId, List<DiscountArticles> discountArticles)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();
                    foreach (DiscountArticles Article in discountArticles)
                    {
                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Delete_Discount_articles", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", customer_DiscountId);
                        mySqlCommand.Parameters.AddWithValue("_IdArticle", Article.IdArticle);
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error Delete_Articles_Discounts(). IdCustomerDiscount- {0} ErrorMessage- {1}", customer_DiscountId, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
       
        public void UpdateDiscountArticles_V2550(string MainServerConnectionString, Discounts discount)
        {
            try
            {
                if (discount.DiscountArticles != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (DiscountArticles DiscountArticles in discount.DiscountArticles)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_EditDiscount_Articles", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                            mySqlCommand.Parameters.AddWithValue("_IdArticle", DiscountArticles.IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_Value", DiscountArticles.Value);
                            mySqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdateDiscountArticles(). IdCustomerDiscount- {0} ErrorMessage- {1}", discount.Id, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddDeletePlants_Discounts_V2550(string MainServerConnectionString, Discounts discount)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();
                    if (discount.DeletedPlantList != null)
                    {
                        foreach (Site site in discount.DeletedPlantList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Delete_Discount_Plants", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                            mySqlCommand.Parameters.AddWithValue("_IdPlant", site.IdSite);
                            mySqlCommand.ExecuteNonQuery();
                        }
                    }
                    if (discount.AddedPlantList != null)
                    {
                        foreach (Site site in discount.AddedPlantList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Insert_Discount_Plants", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", discount.Id);
                            mySqlCommand.Parameters.AddWithValue("_IdPlant", site.IdSite);
                            mySqlCommand.Parameters.AddWithValue("_createdBy", discount.IdModifier);
                            mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeletePlants_Discounts(). IdCustomerDiscount- {0} ErrorMessage- {1}", discount.Id, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddUpdateDeleteCustomersByDicountCustomer_V2550(string MainServerConnectionString, int customer_DiscountId, List<DiscountCustomers> CustomerList, uint? IdModifier)
        {
            try
            {
                if (CustomerList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DiscountCustomers Customer in CustomerList)
                        {
                            if (Customer.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CustomerDiscountListCustomers_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscountCustomer", Customer.IdCustomerDiscountCustomer);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", customer_DiscountId);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (Customer.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (Customer.IdCustomerDiscountCustomer > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_CustomerDiscountListCustomers_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscountCustomer", Customer.IdCustomerDiscountCustomer);
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", customer_DiscountId);
                                    if (Customer.IdGroup == 0)
                                    {
                                        Customer.IdGroup = null;
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", Customer.IdGroup);
                                    mySqlCommand.Parameters.AddWithValue("_IdRegion", Customer.IdRegion);
                                    mySqlCommand.Parameters.AddWithValue("_IdCountry", Customer.IdCountry);
                                    mySqlCommand.Parameters.AddWithValue("_IdSite", Customer.IdPlant);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (Customer.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CustomerDiscountListCustomers_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCustomerDiscount", customer_DiscountId);
                                if (Customer.IdGroup == 0)
                                {
                                    Customer.IdGroup = null;
                                }
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", Customer.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", Customer.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdCountry", Customer.IdCountry);
                                mySqlCommand.Parameters.AddWithValue("_IdSite", Customer.IdPlant);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdModifier);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                Customer.IdCustomerDiscountCustomer = Convert.ToUInt32(mySqlCommand.ExecuteScalar());
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteCustomersByDicountCustomer(). IdCustomerDiscount- {0} ErrorMessage- {1}", customer_DiscountId, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddUpdateDeleteDiscountComments_V2550(string connectionString, Int32 idDiscountPrice, List<DiscountLogEntry> commentList)
        {
            try
            {
                if (commentList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DiscountLogEntry commentType in commentList)
                        {
                            if (commentType.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Discount_Comments_Delete_V2470", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdLogEntryByDiscount", commentType.IdLogEntryByDiscount);
                                mySqlCommand.ExecuteNonQuery();


                            }
                            else if (commentType.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (commentType.IdLogEntryByDiscount > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Discount_Comments_Update_V2470", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdLogEntryByDiscount", commentType.IdLogEntryByDiscount);
                                    mySqlCommand.Parameters.AddWithValue("_Comments", commentType.Comments);
                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", commentType.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", commentType.Datetime);
                                    mySqlCommand.Parameters.AddWithValue("_Datetime", commentType.Datetime);
                                    // mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 257);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (commentType.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CommentsEntriesByDiscount_Insert_V2470", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdDiscountPrice", idDiscountPrice);
                                mySqlCommand.Parameters.AddWithValue("_IdUser", commentType.IdUser);
                                mySqlCommand.Parameters.AddWithValue("_Datetime", commentType.Datetime);
                                mySqlCommand.Parameters.AddWithValue("_Comments", commentType.Comments);
                                mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 257);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteCustomerPriceComments_V2470(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public List<Articles> AddWMSTOPCMArticlesByCategories_V2550(string connectionString, string mainServerConnectionString, List<ArticleCategorieMapping> articleCategoryMappingList)
        {
            TransactionScope transactionScope = null;
            List<Articles> AllImportedArticles = new List<Articles>();
            using (transactionScope = new TransactionScope())
            {
                try
                {
                    if (articleCategoryMappingList != null)
                    {
                        List<Articles> WmsArticles = new List<Articles>();
                        foreach (var item in articleCategoryMappingList)
                        {

                            #region Read All WMS Articles
                            using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                            {

                                mySqlConnection.Open();
                                MySqlCommand mySqlCommand = new MySqlCommand("GetWmsArticlesToImportInPCMArticles", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdWarehouseCategory", item.IdWMSArticleCategory);
                                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Articles WmsArticle = new Articles();
                                        WmsArticle.ArticleCategory = new ArticleCategories();
                                        WmsArticle.ArticleCategory.Name = item.WMSName;
                                        WmsArticle.ArticleCategory.IdArticleCategory = Convert.ToUInt32(item.IdWMSArticleCategory);

                                        WmsArticle.PcmArticleCategory = new PCMArticleCategory();
                                        WmsArticle.PcmArticleCategory.IdPCMArticleCategory = item.IdPCMArticleCategory;
                                        WmsArticle.PcmArticleCategory.Name = item.PCMName;

                                        if (reader["idArticle"] != DBNull.Value)
                                            WmsArticle.IdArticle = Convert.ToUInt32(reader["idArticle"]);

                                        if (reader["Reference"] != DBNull.Value)
                                            WmsArticle.Reference = Convert.ToString(reader["Reference"]);

                                        if (reader["Description"] != DBNull.Value)
                                            WmsArticle.Description = Convert.ToString(reader["Description"]);

                                        if (reader["IdPCMStatus"] != DBNull.Value)
                                            WmsArticle.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                                        WmsArticles.Add(WmsArticle);

                                    }
                                }
                                mySqlConnection.Close();
                            }
                        }
                        #endregion

                        #region Update All WMS articles into PCM Articles
                        if (WmsArticles != null)
                        {

                            using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerConnectionString))
                            {
                                mySqlConnection.Open();
                                MySqlCommand mySqlCommand = new MySqlCommand("WMS_InsertWMSToPCMArticlesByCategory", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                foreach (var WmsArticle in WmsArticles)
                                {
                                    mySqlCommand.Parameters.Clear();
                                    mySqlCommand.Parameters.AddWithValue("_idArticle", WmsArticle.IdArticle);
                                    mySqlCommand.Parameters.AddWithValue("_IdPCMStatus", WmsArticle.IdPCMStatus);
                                    mySqlCommand.Parameters.AddWithValue("_IdPCMCategory", WmsArticle.PcmArticleCategory.IdPCMArticleCategory);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                mySqlConnection.Close();
                            }
                        }

                        #endregion

                        AllImportedArticles.AddRange(WmsArticles);

                    }
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddWMSTOPCMArticlesByCategories(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();
                    throw;
                }
            }
            return AllImportedArticles;
        }
        public DetectionDetails AddDetectionForNewAddDetectionViewModel_V2550(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3 || detectionDetails.IdDetectionType == 4)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup_V2550(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Insert_V2330", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_createdBy", detectionDetails.CreatedBy);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", detectionDetails.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", detectionDetails.PurchaseQtyMin);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", detectionDetails.PurchaseQtyMax);
                        mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", detectionDetails.IsShareWithCustomer);
                        mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", detectionDetails.IsSparePartOnly);
                        mySqlCommand.Parameters.AddWithValue("_IdScope", detectionDetails.IdScope);
                        if (detectionDetails.IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", null);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }

                        detectionDetails.IdDetections = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    if (detectionDetails.IdDetections > 0)
                    {
                        AddDetectionAttachedDocByDetectionForEditDetectionViewModel_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                        AddDetectionAttachedLinkByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                        AddDetectionImageByDetection_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                        AddCustomersRegionsByDetectionForEditDetectionViewModel_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerListByDetection, detectionDetails.IdDetectionType, detectionDetails.CreatedBy);
                        AddDetectionLogEntryNew_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);
                        AddUpdateDeletePLMDetectionPrices_V2550(MainServerConnectionString, detectionDetails.ModifiedPLMDetectionList, detectionDetails.IdDetections, detectionDetails.CreatedBy);
                        AddDetectionCommentsNew_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionCommentsList, detectionDetails.IdDetectionType);



                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetection_V2470(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return detectionDetails;
        }

        public void AddDetectionLogEntryNew_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionLogEntry> LogList, UInt32 IdDetectionType)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByDetection_Insert_V2470", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 258);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionLogEntry_V2470(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddDetectionCommentsNew_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionLogEntry> LogList, UInt32 IdDetectionType)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByDetection_Insert_V2470", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 257);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionComments_V2470(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public bool UpdateDetectionForEditDetectionViewModelNew_V2550(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3 || detectionDetails.IdDetectionType == 4)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup_V2550(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Update_V2340", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionDetails.IdDetections);
                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionDetails.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", detectionDetails.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMin", detectionDetails.PurchaseQtyMin);
                        mySqlCommand.Parameters.AddWithValue("_PurchaseQtyMax", detectionDetails.PurchaseQtyMax);
                        mySqlCommand.Parameters.AddWithValue("_IsShareWithCustomer", detectionDetails.IsShareWithCustomer);
                        mySqlCommand.Parameters.AddWithValue("_IsSparePartOnly", detectionDetails.IsSparePartOnly);
                        mySqlCommand.Parameters.AddWithValue("_IdScope", detectionDetails.IdScope);
                        if (IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);
                        }

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }

                    //add/update/delete ways, detections, options, spare parts
                    //AddUpdateDeleteDetectionFiles(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                    AddUpdateDeleteDetectionFilesNew_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);//[GEOS2-4074][12.12.2022][rdixit]
                    AddUpdateDeleteDetectionLinks_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                    AddUpdateDeleteDetectionImages_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                    AddDeleteCustomersRegionsByDetectionForAddDetectionViewModel_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerListByDetection, detectionDetails.IdDetectionType, detectionDetails.ModifiedBy);
                    AddDetectionLogEntryNew_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);
                    AddUpdateDeleteDetectionComments_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionCommentsList, detectionDetails.IdDetectionType);

                    AddUpdateDeleteDetectionRelatedModule_V2550(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.ProductTypesList);

                    //
                    AddUpdateDeletePLMDetectionPrices_V2550(MainServerConnectionString, detectionDetails.ModifiedPLMDetectionList, detectionDetails.IdDetections, detectionDetails.ModifiedBy);
                    AddBasePriceListLogEntry_V2550(MainServerConnectionString, detectionDetails.BasePriceLogEntryList);
                    AddCustomerPriceListLogEntry_V2550(MainServerConnectionString, detectionDetails.CustomerPriceLogEntryList);
                    if (detectionDetails.ProductTypesList.Count > 0 && detectionDetails.ProductTypeChangeLogList.Count > 0)
                    {
                        List<ulong> IdCpTypeList = detectionDetails.ProductTypesList.Select(i => i.IdCPType).Distinct().ToList();
                        foreach (var item in IdCpTypeList)
                        {
                            var ChangeLogList = detectionDetails.ProductTypeChangeLogList.Where(i => i.IdCPType == item).ToList();
                            AddProductTypeLogEntry_V2550(MainServerConnectionString, item, ChangeLogList);
                        }

                    }
                    //
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateDetection_V2470(). IdDetection- {0} ErrorMessage- {1}", detectionDetails.IdDetections, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }
        public void AddUpdateDeleteDetectionFilesNew_V2550(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedDoc> DetectionAttachedDocList, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedDoc detectionAttachedDocList in DetectionAttachedDocList)
                        {
                            if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedDoc", detectionAttachedDocList.IdDetectionAttachedDoc);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteDetectionAttachedDocFromPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                            }
                            else if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Update_V2340", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedDoc", detectionAttachedDocList.IdDetectionAttachedDoc);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionAttachedDocList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);
                                    mySqlCommand.Parameters.AddWithValue("_IdAttachmentType", detectionAttachedDocList.AttachmentType.IdLookupValue);
                                    mySqlCommand.ExecuteNonQuery();

                                    AddDetectionAttachedDocToPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                                }
                            }
                            else if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Insert_V2340", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedDocList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);
                                mySqlCommand.Parameters.AddWithValue("_IdAttachmentType", detectionAttachedDocList.AttachmentType.IdLookupValue);

                                detectionAttachedDocList.IdDetectionAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                                {
                                    AddDetectionAttachedDocToPath_V2550(detectionAttachedDocList, DetectionAttachedDocPath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionFiles_V2340(). IdCatalogueItem- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddUpdateDeleteDetectionComments_V2550(string connectionString, UInt64 idDetection, List<DetectionLogEntry> commentList, UInt32 IdDetectionType)
        {
            try
            {
                if (commentList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionLogEntry productType in commentList)
                        {
                            if (productType.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detection_Comments_Delete_V2470", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdLogEntryByDetection", productType.IdLogEntryByDetection);
                                mySqlCommand.ExecuteNonQuery();


                            }
                            else if (productType.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (productType.IdLogEntryByDetection > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detection_Comments_Update_V2470", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdLogEntryByDetection", productType.IdLogEntryByDetection);
                                    mySqlCommand.Parameters.AddWithValue("_Comments", productType.Comments);
                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", productType.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", productType.Datetime);
                                    mySqlCommand.Parameters.AddWithValue("_Datetime", productType.Datetime);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                    // mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 257);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (productType.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CommentsEntriesByDetection_Insert_V2470", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdDetection", idDetection);
                                mySqlCommand.Parameters.AddWithValue("_IdUser", productType.IdUser);
                                mySqlCommand.Parameters.AddWithValue("_Datetime", productType.Datetime);
                                mySqlCommand.Parameters.AddWithValue("_Comments", productType.Comments);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 257);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionComments_V2470(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddUpdateDeleteDetectionRelatedModule_V2550(string MainServerConnectionString, UInt32 IdDetection, List<ProductTypes> DetectionRelatedModuleList)
        {
            try
            {
                if (DetectionRelatedModuleList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypes detectionRelatedModuleList in DetectionRelatedModuleList)
                        {
                            if (detectionRelatedModuleList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_RelatedModules_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", detectionRelatedModuleList.IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdCpType", detectionRelatedModuleList.IdCPType);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (detectionRelatedModuleList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_RelatedModules_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", detectionRelatedModuleList.IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdCpType", detectionRelatedModuleList.IdCPType);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionRelatedModule(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddProductTypeLogEntry_V2550(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeLogEntry> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByCptype_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeLogEntry(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public bool IsDeletedDetection_V2550(UInt32 IdDetection, string detectionName, string MainServerConnectionString)
        {
            bool isDeleted = false;
            if (IdDetection > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Delete_V2500", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                        mySqlCommand.Parameters.AddWithValue("_DetectionName", detectionName);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        isDeleted = true;
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsDeletedDetection_V2500(). IdProductType- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }
        public ProductTypes AddProductType_V2550(ProductTypes productType, string MainServerConnectionString, string ProductTypeImagePath, string ProductTypeAttachedDocPath, string PCMConnectionString)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
            {
                try
                {
                    string lastestProductTypeReference = "";
                    lastestProductTypeReference = GetLatestProuductTypeReference_V2550(MainServerConnectionString);

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        if (!string.IsNullOrEmpty(lastestProductTypeReference) && lastestProductTypeReference != productType.Reference)
                        {
                            mySqlCommand.Parameters.AddWithValue("_Reference", lastestProductTypeReference);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_Reference", productType.Reference);
                        }

                        mySqlCommand.Parameters.AddWithValue("_Name", productType.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", productType.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", productType.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", productType.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", productType.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", productType.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", productType.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", productType.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", productType.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", productType.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", productType.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", productType.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", productType.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", productType.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", productType.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", productType.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_ExtraCost", productType.ExtraCost);
                        mySqlCommand.Parameters.AddWithValue("_MinimumTestPoints", productType.MinimumTestPoints);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", productType.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDefaultWayType", productType.IdDefaultWayType);
                        mySqlCommand.Parameters.AddWithValue("_Standard", productType.Standard);
                        mySqlCommand.Parameters.AddWithValue("_Code", productType.Code);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", productType.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_createdBy", productType.CreatedBy);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsEnabled", productType.IsEnabled);

                        productType.IdCPType = Convert.ToUInt64(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    if (productType.IdCPType > 0)
                    {
                        // AddProductTypeByTemplate(MainServerConnectionString, productType.IdTemplate, productType.IdCPType);

                        AddProductTypeByTemplate_V2530(MainServerConnectionString, productType.IdTemplate, productType.IdCPType);

                        // AddWaysByProductType_V2250(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.WayList);

                        AddWaysByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.WayList);


                        // AddDetectionsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.DetectionList);

                        AddDetectionsByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.DetectionList);

                        // AddOptionsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.OptionList);

                        AddOptionsByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.OptionList);

                        //AddSparePartsByProductType_V2140(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.SparePartList);

                        AddSparePartsByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.SparePartList);

                        //  AddConnectorFamiliesByProductType(MainServerConnectionString, productType.IdCPType, productType.FamilyList);

                        AddConnectorFamiliesByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.FamilyList);

                        //  AddProductTypeAttachedDocByProductType_V2340(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedDocList, ProductTypeAttachedDocPath);

                        AddProductTypeAttachedDocByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedDocList, ProductTypeAttachedDocPath);

                        //  AddProductTypeAttachedLinkByProductType(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedLinkList);

                        AddProductTypeAttachedLinkByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedLinkList);

                        //  AddProductTypeImageByProductType(MainServerConnectionString, productType.IdCPType, productType.ProductTypeImageList, ProductTypeImagePath);

                        AddProductTypeImageByProductType_V2530(MainServerConnectionString, productType.IdCPType, productType.ProductTypeImageList, ProductTypeImagePath);

                        // AddCustomersRegionsByProductType_V2280(MainServerConnectionString, productType.IdCPType, productType.CustomerListByCPType, productType.CreatedBy);

                        AddCustomersRegionsByProductType_V2550(MainServerConnectionString, productType.IdCPType, productType.CustomerListByCPType, productType.CreatedBy);

                        // AddSitesByProductType_V2260(MainServerConnectionString, productType.IdCPType, productType.CustomerListByCPType, PCMConnectionString);

                        AddSitesByProductType_V2550(MainServerConnectionString, productType.IdCPType, productType.CustomerListByCPType, PCMConnectionString);

                        //  AddProductTypeLogEntry_V2470(MainServerConnectionString, productType.IdCPType, productType.ProductTypeLogEntryList);

                        AddProductTypeLogEntryNew_V2550(MainServerConnectionString, productType.IdCPType, productType.ProductTypeLogEntryList);

                        //  AddProductTypeCommentEntry_V2470(MainServerConnectionString, productType.IdCPType, productType.ProductTypeCommentList);

                        AddProductTypeCommentEntry_V2550(MainServerConnectionString, productType.IdCPType, productType.ProductTypeCommentList);


                        //  AddCompatibilitiesByProductType(MainServerConnectionString, (byte)productType.IdCPType, productType.ProductTypeCompatibilityList);


                        AddCompatibilitiesByProductType_V2550(MainServerConnectionString, (long)productType.IdCPType, productType.ProductTypeCompatibilityList);
                        try
                        {
                            //AddUpdateDeletePLMModulePrices_V2490(MainServerConnectionString, productType.ModifiedPLMModuleList, productType.IdCPType, productType.CreatedBy);

                            AddUpdateDeletePLMModulePrices_V2550(MainServerConnectionString, productType.ModifiedPLMModuleList, productType.IdCPType, productType.CreatedBy);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error AddProductType_V2530(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddProductType_V2530(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return productType;
        }
        public string GetLatestProuductTypeReference_V2550(string PCMConnectionString)
        {
            UInt32 idCPType;
            string cptypeReference;
            string Characters = "CP";
            Int16 Year = 0;
            string Seperator = ".";
            Int16 Number = 0;

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetLatestProuductTypeReference", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["IdCPType"] != DBNull.Value)
                                idCPType = Convert.ToUInt32(reader["IdCPType"]);

                            if (reader["cptypeReference"] != DBNull.Value)
                                cptypeReference = Convert.ToString(reader["cptypeReference"]);

                            if (reader["Characters"] != DBNull.Value)
                                Characters = Convert.ToString(reader["Characters"]);

                            if (reader["Year"] != DBNull.Value)
                                Year = Convert.ToInt16(reader["Year"]);

                            if (reader["Seperator"] != DBNull.Value)
                                Seperator = Convert.ToString(reader["Seperator"]);

                            if (reader["Number"] != DBNull.Value)
                                Number = Convert.ToInt16(reader["Number"]);
                        }
                    }
                }

                Int16 currentYear = Convert.ToInt16(DateTime.Now.ToString("yy"));

                if (Year == currentYear)
                {
                    Number += 1;
                    return string.Format("{0}{1}{2}{3}", Characters, currentYear, Seperator, Number.ToString("0000"));
                }
                else if (currentYear > Year)
                {
                    return string.Format("{0}{1}{2}{3}", "CP", currentYear, Seperator, 1.ToString("0000"));
                }
                else
                {
                    return string.Format("{0}{1}{2}{3}", "CP", currentYear, Seperator, 1.ToString("0000"));
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLatestProuductTypeReference(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddProductTypeByTemplate_V2530(string MainServerConnectionString, UInt64 IdTemplate, UInt64 IdCPType)
        {
            try
            {
                if (IdTemplate > 0)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        // MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Insert", mySqlConnection);

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Insert_V2530", mySqlConnection);

                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                        mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeByTemplate_V2530(). IdTemplate- {0} ErrorMessage- {1}", IdTemplate, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddWaysByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Ways> WayList)
        {
            try
            {
                if (WayList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Ways wayList in WayList)
                        {
                            //MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", wayList.IdWays);
                            mySqlCommand.Parameters.AddWithValue("_OrderNumber", wayList.OrderNumber);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddWaysByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddDetectionsByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Detections> DetectionList)
        {
            try
            {
                if (DetectionList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Detections detectionList in DetectionList)
                        {
                            if (detectionList.IdDetections > 0)
                            {
                                // MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);

                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert_V2530", mySqlConnection);

                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionList.IdDetections);
                                mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionList.OrderNumber);

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionsByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddOptionsByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Options> OptionList)
        {
            try
            {
                if (OptionList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Options optionList in OptionList)
                        {
                            if (optionList.IdOptions > 0)
                            {
                                //  MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);

                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert_V2530", mySqlConnection);

                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", optionList.IdOptions);
                                mySqlCommand.Parameters.AddWithValue("_OrderNumber", optionList.OrderNumber);

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddOptionsByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddSparePartsByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<SpareParts> SparePartList)
        {
            try
            {
                if (SparePartList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (SpareParts sparePartsList in SparePartList)
                        {
                            // MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", sparePartsList.IdSpareParts);
                            mySqlCommand.Parameters.AddWithValue("_OrderNumber", sparePartsList.OrderNumber);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddSparePartsByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddConnectorFamiliesByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, List<Common.PCM.ConnectorFamilies> FamilyList)
        {
            try
            {
                if (FamilyList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Common.PCM.ConnectorFamilies familyList in FamilyList)
                        {
                            //MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCPType_Insert", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCPType_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdFamily", familyList.IdFamily);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddConnectorFamiliesByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddProductTypeAttachedDocByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeAttachedDoc> ProductTypeAttachedDocList, string ProductTypeAttachedDocPath)
        {
            try
            {
                if (ProductTypeAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeAttachedDoc productTypeAttachedDocList in ProductTypeAttachedDocList)
                        {
                            //  MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Insert_V2340", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeAttachedDocList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedDocList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeAttachedDocList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_IdAttachmentType", productTypeAttachedDocList.AttachmentType.IdLookupValue);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeAttachedDocList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            //mySqlCommand.Parameters.AddWithValue("_DocumentType", productTypeAttachedDocList.AttachmentType.Value);
                            // mySqlCommand.Parameters.AddWithValue("_IdDocType", productTypeAttachedDocList.IdDocType);
                            productTypeAttachedDocList.IdCPTypeAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (productTypeAttachedDocList.IdCPTypeAttachedDoc > 0)
                            {
                                AddProductTypeAttachedDocToPath_V2550(productTypeAttachedDocList, ProductTypeAttachedDocPath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeAttachedDocByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddProductTypeAttachedLinkByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeAttachedLink> ProductTypeAttachedLinkList)
        {
            try
            {
                if (ProductTypeAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeAttachedLink productTypeAttachedLinkList in ProductTypeAttachedLinkList)
                        {
                            // MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_AttachedLinks_Insert", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_AttachedLinks_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_Name", productTypeAttachedLinkList.Name);
                            mySqlCommand.Parameters.AddWithValue("_Address", productTypeAttachedLinkList.Address);
                            mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedLinkList.Description);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeAttachedLinkList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeAttachedLinkByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public bool AddProductTypeImageToPath_V2550(ProductTypeImage productTypeImage, string ProductTypeImagePath)
        {
            if (productTypeImage.ProductTypeImageInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", ProductTypeImagePath, productTypeImage.IdCPTypeImage);
                    string filePath = completePath + "\\" + productTypeImage.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, productTypeImage.ProductTypeImageInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeImageToPath()- Filename - {0}. ErrorMessage- {1}", productTypeImage.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }
        public bool AddProductTypeAttachedDocToPath_V2550(ProductTypeAttachedDoc productTypeAttachedDoc, string ProductTypeAttachedDocPath)
        {
            if (productTypeAttachedDoc.ProductTypeAttachedDocInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", ProductTypeAttachedDocPath, productTypeAttachedDoc.IdCPTypeAttachedDoc);
                    string filePath = completePath + "\\" + productTypeAttachedDoc.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }

                    File.WriteAllBytes(filePath, productTypeAttachedDoc.ProductTypeAttachedDocInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeAttachedDocToPath()- Filename - {0}. ErrorMessage- {1}", productTypeAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

         public void AddProductTypeImageByProductType_V2530(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeImage> ProductTypeImageList, string ProductTypeImagePath)
        {
            try
            {
                if (ProductTypeImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeImage productTypeImageList in ProductTypeImageList)
                        {
                            //  MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Insert", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeImageList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", productTypeImageList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeImageList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeImageList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_Position", productTypeImageList.Position);

                            productTypeImageList.IdCPTypeImage = Convert.ToUInt64(mySqlCommand.ExecuteScalar());

                            if (productTypeImageList.IdCPTypeImage > 0)
                            {
                                AddProductTypeImageToPath_V2550(productTypeImageList, ProductTypeImagePath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeImageByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddCustomersRegionsByProductType_V2550(string MainServerConnectionString, UInt64 IdCPType, List<CPLCustomer> CustomerList, UInt32 IdCreator)
        {
            try
            {
                if (CustomerList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CPLCustomer customerList in CustomerList)
                        {
                            //  MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_cptype_Insert_V2280", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_cptype_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            if (customerList.IdGroup == 0)
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", null);
                            }
                            else
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                            }

                            mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                            mySqlCommand.Parameters.AddWithValue("_IdCountry", customerList.IdCountry);
                            mySqlCommand.Parameters.AddWithValue("_IdSite", customerList.IdPlant);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);
                            mySqlCommand.Parameters.AddWithValue("_IsIncluded", customerList.IsIncluded);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCustomersRegionsByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        int count = 0;
        public void AddSitesByProductType_V2550(string MainServerConnectionString, UInt64 IdCPType, List<CPLCustomer> CustomersList, string PCMConnectionString)
        {
            count = count + 1;
            try
            {
                if (CustomersList != null)
                {
                    List<Site> SitesAddCustomerList = new List<Site>();

                    try
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetSitesByCustomerAndRegion_V2260", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            foreach (CPLCustomer customerList in CustomersList)
                            {
                                mySqlCommand.Parameters.Clear();
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdCountry", customerList.IdCountry);
                                mySqlCommand.Parameters.AddWithValue("_IdSite", customerList.IdPlant);
                                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        if (reader["IdSite"] != DBNull.Value)
                                        {
                                            Site site = new Site();
                                            site.IdSite = Convert.ToUInt32(reader["IdSite"]);
                                            SitesAddCustomerList.Add(site);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error AddSitesByProductType_V2530 - PCM_GetSitesByCustomerAndRegion. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }

                    if (SitesAddCustomerList != null && SitesAddCustomerList.Count > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();
                            //      MySqlCommand mySqlCommand = new MySqlCommand("PCM_SitesByCPType_Insert", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_SitesByCPType_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            SitesAddCustomerList = SitesAddCustomerList.Distinct().ToList();
                            foreach (Site site in SitesAddCustomerList)
                            {
                                try
                                {
                                    mySqlCommand.Parameters.Clear();
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdSite", site.IdSite);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error AddSitesByProductType_V2530 - PCM_SitesByCPType_Insert. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    //throw;
                                }
                            }
                            mySqlConnection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddSitesByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddProductTypeLogEntryNew_V2550(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeLogEntry> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByCptype_Insert_V2530", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 258);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeLogEntry_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        public void AddProductTypeCommentEntry_V2550(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeLogEntry> CommentList)
        {
            try
            {
                if (CommentList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeLogEntry commentList in CommentList)
                        {
                            //   MySqlCommand mySqlCommand = new MySqlCommand("PCM_CommentsEntriesByCptype_Insert_V2470", mySqlConnection);

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CommentsEntriesByCptype_Insert_V2530", mySqlConnection);

                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", commentList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", commentList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", commentList.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", 257);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeLogEntry_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public bool AddCompatibilitiesByProductType_V2550(string MainServerConnectionString, long IdCPType, List<ProductTypeCompatibility> CompatibilityList)
        {
            bool isSave = false;
            try
            {
                if (CompatibilityList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeCompatibility productTypeCompatibility in CompatibilityList)
                        {
                            //MySqlCommand mySqlCommand = new MySqlCommand("PCM_cptype_compatibilities_Insert", mySqlConnection);
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_cptype_compatibilities_Insert_V2530", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPtype", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdCPtypeCompatibility", productTypeCompatibility.IdCPtypeCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_IdArticleCompatibility", productTypeCompatibility.IdArticleCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_IdTypeCompatibility", productTypeCompatibility.IdTypeCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_MinimumElements", productTypeCompatibility.MinimumElements);
                            mySqlCommand.Parameters.AddWithValue("_MaximumElements", productTypeCompatibility.MaximumElements);
                            if (productTypeCompatibility.IdRelationshipType == 0)
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                            }
                            else
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", productTypeCompatibility.IdRelationshipType);
                            }
                            mySqlCommand.Parameters.AddWithValue("_Quantity", productTypeCompatibility.Quantity);
                            mySqlCommand.Parameters.AddWithValue("_Remarks", productTypeCompatibility.Remarks);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", productTypeCompatibility.IdCreator);
                            mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCompatibilitiesByProductType_V2530(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isSave;
        }
        public void AddUpdateDeletePLMModulePrices_V2550(string MainServerConnectionString, List<PLMModulePrice> PLMModulePriceList, UInt64 IdCPType, UInt32? IdModifier)
        {
            try
            {
                if (PLMModulePriceList != null)
                {
                    ProductTypes.UpdateAllMaxCostFromNullToZero(PLMModulePriceList);

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PLMModulePrice pLMModulePrice in PLMModulePriceList)
                        {
                            if (pLMModulePrice.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                if (pLMModulePrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_base_price_list_by_modules_Delete_V2530", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMModulePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMModulePrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_customer_price_list_by_modules_Delete_V2530", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMModulePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (pLMModulePrice.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (pLMModulePrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_base_price_list_by_modules_Update_V2530", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMModulePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMModulePrice.IdRule);
                                    pLMModulePrice.RuleValue = pLMModulePrice.RuleValue;
                                    if (pLMModulePrice.IdRule == 1518)
                                    {
                                        pLMModulePrice.RuleValue = 0;
                                    }
                                    else if (pLMModulePrice.IdRule == 308)
                                    {
                                        if (pLMModulePrice.RuleValue == 0)
                                        {
                                            pLMModulePrice.RuleValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMModulePrice.IdRule == 0)
                                        {
                                            pLMModulePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMModulePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_MaxCost", pLMModulePrice.MaxCost);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMModulePrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_customer_price_list_by_modules_Update_V2530", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMModulePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMModulePrice.IdRule);
                                    pLMModulePrice.RuleValue = pLMModulePrice.RuleValue;
                                    if (pLMModulePrice.IdRule == 1518)
                                    {
                                        pLMModulePrice.RuleValue = 0;
                                    }
                                    else if (pLMModulePrice.IdRule == 308)
                                    {
                                        if (pLMModulePrice.RuleValue == 0)
                                        {
                                            pLMModulePrice.RuleValue = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMModulePrice.IdRule == 0)
                                        {
                                            pLMModulePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMModulePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (pLMModulePrice.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                if (pLMModulePrice.Type == "BPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("geos.PLM_base_price_list_by_modules_Insert_V2530;", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdBasePriceList", pLMModulePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    pLMModulePrice.RuleValue = pLMModulePrice.RuleValue;
                                    if (pLMModulePrice.IdRule == 1518)
                                    {
                                        pLMModulePrice.RuleValue = 0;
                                    }
                                    else if (pLMModulePrice.IdRule == 308)
                                    {
                                        if (pLMModulePrice.RuleValue == 0)
                                        {
                                            pLMModulePrice.RuleValue = null;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMModulePrice.IdRule == 0)
                                        {
                                            pLMModulePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMModulePrice.IdRule);
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMModulePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_MaxCost", pLMModulePrice.MaxCost);
                                    mySqlCommand.Parameters.AddWithValue("_Royalty", 0);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                                else if (pLMModulePrice.Type == "CPL")
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PLM_customer_price_list_by_Modules_Insert_V2530", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCustomerPriceList", pLMModulePrice.IdCustomerOrBasePriceList);
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    pLMModulePrice.RuleValue = pLMModulePrice.RuleValue;
                                    if (pLMModulePrice.IdRule == 1518)
                                    {
                                        pLMModulePrice.RuleValue = 0;
                                    }
                                    else if (pLMModulePrice.IdRule == 308)
                                    {
                                        if (pLMModulePrice.RuleValue == 0)
                                        {
                                            pLMModulePrice.RuleValue = null;
                                        }
                                    }
                                    else
                                    {
                                        if (pLMModulePrice.IdRule == 0)
                                        {
                                            pLMModulePrice.RuleValue = null;
                                        }
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_IdRule", pLMModulePrice.IdRule);
                                    mySqlCommand.Parameters.AddWithValue("_RuleValue", pLMModulePrice.RuleValue);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedBy", IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeletePLMModulePrices(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public List<EmployeeAttendance> AddEmployeeImportAttendance_V2550(string mainServerWorkbenchConnectionString, List<EmployeeAttendance> employeeAttendanceList)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(mainServerWorkbenchConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("Hrm_AddEmployeeImportAttendance_V2350", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        foreach (EmployeeAttendance item in employeeAttendanceList)
                        {
                            mySqlCommand.Parameters.Clear();

                            mySqlCommand.Parameters.AddWithValue("_IdEmployee", item.IdEmployee);
                            mySqlCommand.Parameters.AddWithValue("_EmployeeDocumentNumber", item.ClockID);
                            mySqlCommand.Parameters.AddWithValue("_IdCompanyWork", item.IdCompanyWork);
                            mySqlCommand.Parameters.AddWithValue("_IdCompanyShift", item.IdCompanyShift);
                            mySqlCommand.Parameters.AddWithValue("_StartDate", item.StartDate);
                            mySqlCommand.Parameters.AddWithValue("_EndDate", item.EndDate);
                            mySqlCommand.Parameters.AddWithValue("_AccountingDate", item.AccountingDate);
                            mySqlCommand.Parameters.AddWithValue("_Creator", item.Creator);
                            mySqlCommand.Parameters.AddWithValue("_CreationDate", item.CreationDate);

                            item.IdEmployeeAttendance = Convert.ToInt64(mySqlCommand.ExecuteScalar());
                        }

                        mySqlConnection.Close();
                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddEmployeeImportAttendance_V2550(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }

            return employeeAttendanceList;
        }
    }
}
