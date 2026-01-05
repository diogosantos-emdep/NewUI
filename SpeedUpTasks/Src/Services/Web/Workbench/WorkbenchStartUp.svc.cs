using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Emdep.Geos.Data.BusinessLogic;
using System.IO;
using Emdep.Geos.Services.Contracts;
using System.Net;
using Emdep.Geos.Utility;
using System.ServiceModel.Activation;
using Emdep.Geos.Data.Common.Hrm;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.HarnessPart;
using System.Diagnostics;
using System.Configuration;
using System.Data.Entity.Core;
using Emdep.Geos.Data.Common.FileReplicator;

namespace Emdep.Geos.Services.Web.Workbench
{
    /// <summary>
    /// WorkbenchStartUp class use for getting information of Workbench
    /// </summary>
    public class WorkbenchStartUp : IWorkbenchStartUp
    {

        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="userLogin">Get login detail to check</param>
        /// <param name="password">Get password detail to check</param>
        /// <returns>Details of user from table User</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.GetUserByLogin("cpatil","123");
        ///     }
        /// }
        /// </code>
        /// </example>
        public User GetUserByLogin(string userLogin, string password)
        {
            User user = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                user = mgr.GetUserByLogin(userLogin, password, connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        public List<UserManagerDtl> GetManagerUsers(Int32 idManager)
        {
            List<UserManagerDtl> userManagerDtls = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                userManagerDtls = mgr.GetManagerUsers(idManager, connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return userManagerDtls;
        }

        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="userId">Get user id detail to check</param>
        /// <returns>Details of user from table User</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.GetUserById(3);
        ///     }
        /// }
        /// </code>
        /// </example>
        public User GetUserById(int userId)
        {

            User user = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                user = mgr.GetUserById(userId, connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to get workstation by WorkstationIP
        /// </summary>
        /// <param name="workstationIP">Get current user system IP</param>
        /// <returns>Details of workstation from class Workstation</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         Workstation workstation = control.GetWorkstationByIP("10.0.9.53");
        ///     }
        /// }
        /// </code>
        /// </example>
        public Workstation GetWorkstationByIP(string workstationIP)
        {
            Workstation workstation = null;
            try
            {
                UserManager mgr = new UserManager();
                workstation = mgr.GetWorkstationByIP(workstationIP);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return workstation;
        }

        /// <summary>
        /// This method is to get workstation by WorkstationID
        /// </summary>
        /// <param name="workstationId">Get workstation detail related to WorkstationID</param>
        /// <returns>Details of workstation from class Workstation</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         Workstation workstation = control.GetWorkstationById(2);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Workstation GetWorkstationById(int workstationId)
        {
            Workstation workstation = null;
            try
            {
                UserManager mgr = new UserManager();
                workstation = mgr.GetWorkstationById(workstationId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return workstation;
        }

        /// <summary>
        /// This method is to get stage by stageID
        /// </summary>
        /// <param name="stageId">Get stage detail related to stageID</param>
        /// <returns>Details of stage related to stageID  from class stage</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         Stage stage = control.GetStageById(2);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Stage GetStageById(int stageId)
        {
            Stage stage = null;
            try
            {
                UserManager mgr = new UserManager();
                stage = mgr.GetStageById(stageId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return stage;
        }

        /// <summary>
        /// This method is to add user logs in class UserLog
        /// </summary>
        /// <param name="userLog">To get details of UserLog from class UserLog</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         UserLog userlog = new UserLog();
        ///         userlog.IdUser = 1;
        ///         userlog.IdLogEntryType = 1;
        ///         userlog.Log = "";
        ///         control.AddUserLog(userlog);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void AddUserLog(UserLog userLog)
        {
            try
            {
                UserManager mgr = new UserManager();
                mgr.AddUserLog(userLog);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get latest version detail from class GeosWorkbenchVersion
        /// </summary>
        /// <returns>Details of latest version to check whether current version is updated or not from class GeosWorkbenchVersion</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosWorkbenchVersion geosWorkbenchVersion = control.GetLatestVersion();
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosWorkbenchVersion GetLatestVersion()
        {
            GeosWorkbenchVersion version = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                version = geosmodulemgr.GetCurrentVersion();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return version;
        }

        /// <summary>
        /// This method is to get current version beta wise
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>Geos workbench version details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosWorkbenchVersion version = control.GetCurrentVersionBetaWise(userId);
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosWorkbenchVersion GetCurrentVersionBetaWise(Int32 userId)
        {
            GeosWorkbenchVersion version = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                version = geosmodulemgr.GetCurrentVersionBetaWise(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return version;
        }

        /// <summary>
        /// This method is to get user isbeta current version
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>Geos workbench version details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosWorkbenchVersion version = control.GetUserIsBetaCurrentVersion(userId);
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosWorkbenchVersion GetUserIsBetaCurrentVersion(Int32 userId)
        {
            GeosWorkbenchVersion version = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                version = geosmodulemgr.GetUserIsBetaCurrentVersion(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return version;
        }

        /// <summary>
        /// This method is to get all version beta wise
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>List of geos workbench version details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GeosWorkbenchVersion&gt; version = control.GetAllVersionBetaWise(userId);
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosWorkbenchVersion> GetAllVersionBetaWise(Int32 userId)
        {
            List<GeosWorkbenchVersion> version = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                version = geosmodulemgr.GetAllVersionBetaWise(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return version;
        }

        /// <summary>
        /// This method is to get current publish version
        /// </summary>
        /// <returns>Geos workbench version details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosWorkbenchVersion version = control.GetCurrentPublishVersion();
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosWorkbenchVersion GetCurrentPublishVersion()
        {
            GeosWorkbenchVersion version = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                version = geosmodulemgr.GetCurrentPublishVersion();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return version;
        }


        /// <summary>
        /// This method is to get current version release notes from class GeosReleaseNote
        /// </summary>
        /// <param name="geosworkbenchversion">To get details of current version from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosReleaseNotes related to current version</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GeosReleaseNote&gt; geosReleaseNote = control.GetReleaseNotesByVersion(geosWorkbenchVersion);
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosReleaseNote> GetReleaseNotesByVersion(GeosWorkbenchVersion geosworkbenchversion)
        {
            List<GeosReleaseNote> releasenote = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                releasenote = geosmodulemgr.GetReleaseNotesByVersion(geosworkbenchversion);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return releasenote;
        }

        /// <summary>
        /// This method is to get all versions from class GeosWorkbenchVersion
        /// </summary>
        /// <returns>List of GeosWorkbenchVersion</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GeosWorkbenchVersion&gt; geosworkbenchversion = control.GetAllVersions();
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosWorkbenchVersion> GetAllVersions()
        {
            List<GeosWorkbenchVersion> geosworkbenchversion = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                geosworkbenchversion = geosmodulemgr.GetAllVersions();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosworkbenchversion;
        }

        /// <summary>
        /// This method is to add download version by IP in class GeosWorkbenchVersionDownload
        /// </summary>
        /// <param name="geosWorkbenchVersionDownload">To get details of current version from class GeosWorkbenchVersion</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          GeosWorkbenchVersionDownload geosWorkbenchVersionDownload = new GeosWorkbenchVersionDownload();
        ///          geosWorkbenchVersionDownload.IdGeosModuleVersion = 1;
        ///           geosWorkbenchVersionDownload.UserIP = ApplicationOperation.GetEmdepGroupIP("10.");
        ///          control.AddDownloadVersionByIP(geosWorkbenchVersionDownload);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void AddDownloadVersionByIP(GeosWorkbenchVersionDownload geosWorkbenchVersionDownload)
        {
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                geosWorkbenchVersionDownload.DownloadedIn = DateTime.Now;
                geosmodulemgr.AddDownloadVersionByIP(geosWorkbenchVersionDownload);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get current version files from class GeosWorkbenchVersionsFile
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get current version id from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosWorkbenchVersionsFiles related to current version id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GeosWorkbenchVersionsFile&gt; geosworkbenchversionsfile = control.GetWorkbenchVersionFiles(GeosWorkbenchVersion.idGeosWorkbenchVersion);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosWorkbenchVersionsFile> GetWorkbenchVersionFiles(Int32 idGeosWorkbenchVersion)
        {
            List<GeosWorkbenchVersionsFile> geosworkbenchversionsfile = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                geosworkbenchversionsfile = geosmodulemgr.GetWorkbenchVersionFiles(idGeosWorkbenchVersion);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return geosworkbenchversionsfile;
        }

        /// <summary>
        /// This method is to get current version documentations from class GeosModuleDocumentation
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get current version id from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosModuleDocumentations related to current version id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GeosModuleDocumentation&gt; geosmoduledocumentation = control.GetModuleDocumentations(GeosWorkbenchVersion.idGeosWorkbenchVersion);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosModuleDocumentation> GetModuleDocumentations(Int32 idGeosWorkbenchVersion)
        {
            List<GeosModuleDocumentation> geosmoduledocumentation = null;
            try
            {
                GeosWorkbenchVersionsManager GeosModulemgr = new GeosWorkbenchVersionsManager();
                geosmoduledocumentation = GeosModulemgr.GetModuleDocumentations(idGeosWorkbenchVersion);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return geosmoduledocumentation;
        }

        /// <summary>
        ///  This method is to download current version documentation filename and return it in bytes (Extra method)
        /// </summary>
        /// <param name="idGeosModuleDocumentation">Get current version document id from class GeosModuleDocumentation</param>
        /// <returns>FileTransferRequest:-File in bytes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         FileTransferRequest fileTransferRequest = control.DownloadModuleDocument(GeosModuleDocumentation.idGeosModuleDocumentation);
        ///     }
        /// }
        /// </code>
        /// </example>
        public FileTransferRequest DownloadModuleDocument(Int32 idGeosModuleDocumentation)
        {

            FileTransferRequest file = new FileTransferRequest();
            try
            {
                byte[] bytes = null;

                //GeosWorkbenchVersionsManager GeosModulemgr = new GeosWorkbenchVersionsManager();
                //string filepath = Properties.Settings.Default.WorkbenchVersionDocumentationFolder;
                //filepath += GeosModulemgr.GetModuleDocumentFileName(idGeosModuleDocumentation);

                //// open stream
                //using (System.IO.FileStream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                //{
                //    bytes = new byte[stream.Length];
                //    int numBytesToRead = (int)stream.Length;
                //    int numBytesRead = 0;
                //    while (numBytesToRead > 0)
                //    {
                //        // Read may return anything from 0 to numBytesToRead.
                //        int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                //        // Break when the end of the file is reached.
                //        if (n == 0)
                //            break;

                //        numBytesRead += n;
                //        numBytesToRead -= n;
                //    }
                //}

                //file.Content = bytes;
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return file;

        }

        /// <summary>
        /// This method is to send ForgetPasswordMail
        /// </summary>
        /// <param name="emailId">Get receiver EmailId</param>
        /// <param name="code">Get Code</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.SendForgetPasswordMail(user.companyEmail,GeneratePassword());
        ///     }
        ///     private String GeneratePassword()
        ///     {
        ///         int length = 10;
        ///         const string validCharactor = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        ///         StringBuilder strBuilder = new StringBuilder();
        ///         Random random = new Random();
        ///         while (0 &lt; length--)
        ///         {
        ///                 strBuilder.Append(validCharactor[random.Next(validCharactor.Length)]);
        ///         }
        ///
        ///          return strBuilder.ToString();
        ///      }
        ///  }
        /// </code>
        /// </example>
        public void SendForgetPasswordMail(string emailId, string code)
        {
            try
            {
                if (code != null && code != "")
                {
                    //Get user details from class user by emailid
                    UserManager usermgr = new UserManager();
                    User user = usermgr.GetUserByEmailId(emailId);
                    if (user != null)
                    {
                        ApplicationManager Appmgr = new ApplicationManager();

                        Appmgr.SendForgetPasswordMail(code, user, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.MailFrom, Properties.Settings.Default.EmailTemplate);
                    }
                    else
                    {
                        ServiceException exp = new ServiceException();
                        exp.ErrorCode = "000001";
                        throw new FaultException<ServiceException>(exp);
                    }
                }
                else
                {
                    ServiceException exp = new ServiceException();
                    exp.ErrorCode = "000002";
                    throw new FaultException<ServiceException>(exp);
                }
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
        }

        /// <summary>
        /// This method is to update new password
        /// </summary>
        /// <param name="userName">Get UserName or EmailId</param>
        /// <param name="newPassword">Get Password to update</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.UpdateUserPassword(user.companyEmail / user.Login,Encrypt.Encryption("1234"));
        ///     }
        /// }
        /// </code>
        /// </example>
        public void UpdateUserPassword(string userName, string newPassword)
        {
            try
            {
                UserManager usermgr = new UserManager();
                usermgr.UpdateUserPassword(userName, newPassword);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update new password
        /// </summary>
        /// <param name="newPassword">Get new password to update</param>
        /// <param name="oldPassword">Get old password to update</param>
        /// <param name="userId">Get user id</param>
        /// <returns>Detail of user related to userId</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.ChangeUserPassword(Encrypt.Encryption("1234"),Encrypt.Encryption("12"),user.userId);
        ///     }
        /// }
        /// </code>
        /// </example>
        public User ChangeUserPassword(string newPassword, string oldPassword, Int32 userId)
        {
            User user = null;
            try
            {
                UserManager usermgr = new UserManager();
                user = usermgr.ChangeUserPassword(newPassword, oldPassword, userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to get user related to userId and Password
        /// </summary>
        /// <param name="password">Get password</param>
        /// <param name="userId">Get userId</param>
        /// <returns>Details of user related to userId and Password from class user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.CheckUserPassword(user.Password,user.userId);
        ///     }
        /// }
        /// </code>
        /// </example>
        public User CheckUserPassword(string password, Int32 userId)
        {
            User user = null;
            try
            {
                UserManager usermgr = new UserManager();
                user = usermgr.CheckUserPassword(password, userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to get user detail by code
        /// </summary>
        /// <param name="code">Get Code</param>
        /// <returns>Details of user related to code  from class user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.GetUserByCode(user.CompanyCode);
        ///     }
        /// }
        /// </code>
        /// </example>
        public User GetUserByCode(string code)
        {
            User user = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                user = mgr.GetUserByCode(code, connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to get installer version
        /// </summary>
        /// <returns>Workbench installer version</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         String version = control.GetWorkbenchInstallerVersion();
        ///         
        ///         //OUTPUT : - Properties.Settings.Default.WorkbenchInstallerVersion( value = 0.0.0.1)
        ///     }
        /// }
        /// </code>
        /// </example>
        public String GetWorkbenchInstallerVersion()
        {
            String version = "";
            try
            {
                version = Properties.Settings.Default.WorkbenchInstallerVersion;
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return version;
        }

        /// <summary>
        /// This method is to get site detail by name
        /// </summary>
        /// <param name="companyName">Get company name</param>
        /// <returns>Details of company related to name from class company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         Company company = control.GetcompanyByName(company.Name);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Company GetCompanyByName(string companyName)
        {
            Company company = null;
            try
            {
                UserManager mgr = new UserManager();
                company = mgr.GetCompanyByName(companyName);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        ///// <summary>
        ///// This method is to get site information
        ///// </summary>
        ///// <param name="siteId">Get site id</param>
        ///// <returns>Site information</returns>
        ///// <example>
        ///// This sample shows how to call the method
        ///// <code>
        ///// class TestClass 
        ///// {
        /////     static void Main(string[] args)
        /////     {
        /////         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        /////         EmdepSite site = control.GetSiteByUserSiteId(user.IdSite);
        /////     }
        ///// }
        ///// </code>
        ///// </example>
        //public EmdepSite GetSiteByUserSiteId(Int32 siteId)
        //{
        //    EmdepSite site = null;
        //    try
        //    {
        //        UserManager mgr = new UserManager();
        //        site = mgr.GetSiteByUserSiteId(siteId);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return site;
        //}

        /// <summary>
        /// This method is to get company detail by alias
        /// </summary>
        /// <param name="companyAlias">Get company alias</param>
        /// <returns>Details of company related to alias from class company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///        Company company = control.GetCompanyByAlias(company.Alias);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Company GetCompanyByAlias(string companyAlias)
        {
            Company company = null;
            try
            {
                UserManager mgr = new UserManager();
                company = mgr.GetCompanyByAlias(companyAlias);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        /// <summary>
        /// This method is to get list of all company
        /// </summary>
        /// <returns>List of all company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Company&gt; company = control.GetCompanyList();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetCompanyList()
        {
            List<Company> company = null;
            try
            {
                UserManager mgr = new UserManager();
                company = mgr.GetCompanyList();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        /// <summary>
        /// This method is to get list of all departments
        /// </summary>
        /// <returns>List of all departments</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Department&gt; department = control.GetAllDepartment();
        ///     }
        /// }
        /// </code>
        /// </example>
        //public List<Department> GetAllDepartment()
        //{
        //    List<Department> department = null;
        //    try
        //    {
        //        HrmManager mgr = new HrmManager();
        //        department = mgr.GetAllDepartment();
        //    }
        //    catch (EntityException ee)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ee.InnerException.Message.ToString();
        //        exp.ErrorDetails = ee.ToString();

        //        throw new FaultException<ServiceException>(exp, ee.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return department;
        //}

        /// <summary>
        /// This method is to get list of all job description
        /// </summary>
        /// <returns>List of all job description</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;JobDescription&gt; jobdescription = control.GetAllJobDescription();
        ///     }
        /// }
        /// </code>
        /// </example>
        //public List<JobDescription> GetAllJobDescription()
        //{
        //    List<JobDescription> jobdescription = null;
        //    try
        //    {
        //        HrmManager mgr = new HrmManager();
        //        jobdescription = mgr.GetAllJobDescription();
        //    }
        //    catch (EntityException ee)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ee.InnerException.Message.ToString();
        //        exp.ErrorDetails = ee.ToString();

        //        throw new FaultException<ServiceException>(exp, ee.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return jobdescription;
        //}

        /// <summary>
        /// This method is to get data from two different database with stored procedure
        /// </summary>
        /// <returns>User Details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.GetDataFromDifferentDatabase();
        ///     }
        /// }
        /// </code>
        /// </example>
        public User GetDataFromDifferentDatabase()
        {
            User user;
            try
            {
                UserManager mgr = new UserManager();
                user = mgr.GetDataFromDifferentDatabase();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to get all user 
        /// </summary>
        /// <returns>List of all user </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;User&gt; user = control.GetImpersonateUser();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<User> GetImpersonateUser()
        {
            List<User> user = null;
            try
            {
                UserManager mgr = new UserManager();
                user = mgr.GetImpersonateUser();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to update user profile
        /// </summary>
        /// <param name="user">Get user detail to update</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.UpdateUserProfile(user);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void UpdateUserProfile(User user)
        {
            try
            {
                UserManager usermgr = new UserManager();
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                usermgr.UpdateUserProfile(user, mainServerWorkbenchConnectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="login">Get login detail for autentication</param>
        /// <returns>Details of user from class User</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.GetUserByLoginName(user.Login);
        ///     }
        /// }
        /// </code>
        /// </example>
        public User GetUserByLoginName(string login)
        {
            User user = null;
            try
            {
                UserManager usermgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                user = usermgr.GetUserByLoginName(login, connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to get permission by id
        /// </summary>
        /// <param name="idPermission">Get permission class</param>
        /// <returns>Details of permission from class permission</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         Permission permission = control.GetPermissionById(Permission.IdPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Permission GetPermissionById(int idPermission)
        {
            Permission permission = null;
            try
            {
                UserManager usrmgr = new UserManager();
                permission = usrmgr.GetPermissionById(idPermission);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return permission;
        }

        /// <summary>
        /// This method is to get server date time
        /// </summary>
        /// <returns>Server Date Time</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         DateTime serverdatetime = control.GetServerDateTime();
        ///     }
        /// }
        /// </code>
        /// </example>
        public DateTime GetServerDateTime()
        {
            DateTime serverdatetime;
            try
            {
                serverdatetime = DateTime.Now;
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serverdatetime;
        }

        /// <summary>
        /// This method is to get user detail by emailid or login
        /// </summary>
        /// <param name="emailId">Get EmailID</param>
        /// <returns>Details of user related to emailid or login  from class user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         User user = control.GetUserByEmailId(user.CompanyEmail/user.Login);
        ///     }
        /// }
        /// </code>
        /// </example>
        public User GetUserByEmailId(string emailId)
        {
            User user = null;
            try
            {
                UserManager mgr = new UserManager();
                user = mgr.GetUserByEmailId(emailId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// This method is to get list of modules related to user id permission
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of modules related to user id permission</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GeosModule&gt; geosModules = control.
        ///         (user.IdUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosModule> GetUserModulesPermissions(int idUser)
        {
            List<GeosModule> geosModules = null;
            try
            {
                GeosWorkbenchVersionsManager geosModulemgr = new GeosWorkbenchVersionsManager();
                geosModules = geosModulemgr.GetUserModulesPermissions(idUser);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosModules;
        }

        /// <summary>
        /// This method is to download current site image and return it in bytes 
        /// </summary>
        /// <param name="idCompany">To get current id site from class site</param>
        /// <returns>File in bytes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] bytes = control.GetCompanyImage(company.IdCompany);
        ///     }
        /// }
        /// </code>
        /// </example>
        public byte[] GetCompanyImage(Int32 idCompany)
        {
            byte[] bytes = null;
            try
            {
                var name = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString());
                string filepath = name + @"\Asset\Images\EmdepSites\" + idCompany + ".jpg";

                // open stream
                using (System.IO.FileStream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        /// <summary>
        /// This method is to get list of all notifications related to user id(Async)
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>list of all notifications related to user id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     public async static void GetAllNotificationAsync()
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         var query = await control.GetAllNotificationAsync(3);
        ///      }
        /// }     
        /// </code>
        /// </example>
        public async Task<List<Notification>> GetAllNotificationAsync(Int32 userId)
        {
            List<Notification> notifications = null;
            try
            {
                UserManager mgr = new UserManager();
                notifications = await mgr.GetAllNotificationAsync(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notifications;
        }

        /// <summary>
        /// This method is to get list of all notifications related to user id
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>list of all notifications related to user id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Notification&gt; notifications = control.GetAllNotificationTest(user.Iduser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Notification> GetAllNotificationTest(Int32 userId)
        {
            List<Notification> notifications = null;
            try
            {
                UserManager mgr = new UserManager();
                notifications = mgr.GetAllNotification(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notifications;
        }

        /// <summary>
        /// This method is to update notification status read
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.UpdateUserNotification(user.Iduser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void UpdateUserNotification(Int32 userId)
        {
            try
            {
                UserManager mgr = new UserManager();
                mgr.UpdateUserNotification(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        /// <summary>
        /// This method is to get list of all languages
        /// </summary>
        /// <returns>List of all languages</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Language&gt; languages = control.GetAllLanguage();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Language> GetAllLanguage()
        {
            List<Language> languages = null;
            try
            {
                LanguageManager languagemgr = new LanguageManager();
                languages = languagemgr.GetAllLanguage();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return languages;

        }

        /// <summary>
        /// This method is to update user isvalidate false
        /// </summary>
        /// <param name="user">Get user details to update</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.UpdateUserIsValidateFalse(user);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void UpdateUserIsValidateFalse(User user)
        {
            try
            {
                UserManager usermgr = new UserManager();
                usermgr.UpdateUserIsValidateFalse(user);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to set user profile image
        /// </summary>
        /// <param name="user">Get user details to set user profile image</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.SetUserProfileImage(user);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void SetUserProfileImage(User user)
        {
            try
            {
                string fileUploadPath = Properties.Settings.Default.UserProfileImage + "/_tmp/" + user.Login + "_temp.jpg";
                if (File.Exists(Properties.Settings.Default.UserProfileImage + user.Login + ".jpg"))
                {
                    File.Delete(Properties.Settings.Default.UserProfileImage + user.Login + ".jpg");
                }
                System.IO.File.Copy(fileUploadPath, Properties.Settings.Default.UserProfileImage + user.Login + ".jpg", true);
                UserManager usermgr = new UserManager();
                usermgr.UpdateUserIsValidateTrue(user);
                System.IO.File.Delete(fileUploadPath);
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        /// <summary>
        /// This method is to update isNotification by notification of Id
        /// </summary>
        /// <param name="notificationId">Get notification id to update isnotification</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.UpdateIsNotificationByID(Notification.notificationId);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void UpdateIsNotificationByID(Int32 notificationId)
        {
            try
            {
                UserManager mgr = new UserManager();
                mgr.UpdateIsNotificationByID(notificationId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        /// <summary>
        /// This method is to delete user profile image
        /// </summary>
        /// <param name="user">Get user login name</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.DeleteUserProfileImage(user);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void DeleteUserProfileImage(User user)
        {
            try
            {
                if (File.Exists(Properties.Settings.Default.UserProfileImage + user.Login + ".jpg"))
                {
                    File.Delete(Properties.Settings.Default.UserProfileImage + user.Login + ".jpg");
                }
                if (File.Exists(Properties.Settings.Default.UserProfileImage + "/_tmp/" + user.Login + "_temp.jpg"))
                {
                    File.Delete(Properties.Settings.Default.UserProfileImage + "/_tmp/" + user.Login + "_temp.jpg");
                }
                UserManager usermgr = new UserManager();
                usermgr.UpdateUserIsValidateNull(user);
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        /// <summary>
        /// This method is to get list of all unread notifications related to user id
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>list of all unread notifications related to user id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Notification&gt; notifications = control.GetAllUnreadNotificationTest(user.IdUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Notification> GetAllUnreadNotificationTest(Int32 userId)
        {
            List<Notification> notifications = null;
            try
            {
                UserManager mgr = new UserManager();
                notifications = mgr.GetAllUnreadNotification(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notifications;
        }


        /// <summary>
        /// This method is to get list of all unread notifications related to user id(Async)
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>list of all unread notifications related to user id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     public async static void GetAllNotificationAsync()
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Notification&gt; notifications = await control.GetAllUnreadNotificationAsync(user.IdUser);
        ///      }
        /// }     
        /// </code>
        /// </example>
        public async Task<List<Notification>> GetAllUnreadNotificationAsync(Int32 userId)
        {
            List<Notification> notifications = null;
            try
            {
                UserManager mgr = new UserManager();
                notifications = await mgr.GetAllUnreadNotificationAsync(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notifications;
        }

        /// <summary>
        /// This method is to update is new to 0
        /// </summary>
        /// <param name="notificationids">List of all unread notification list</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.UpdateListOfNotification(notificationList);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void UpdateListOfNotification(List<long> notificationids)
        {
            User user = null;
            try
            {
                UserManager mgr = new UserManager();
                mgr.UpdateListOfNotification(notificationids);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        /// <summary>
        /// This method is to get list of all notifications related to user id using paging
        /// </summary>
        /// <param name="startIndex">Get start index of notification list</param>
        /// <param name="pageCount">Get number of notification on page</param>
        /// <param name="userId">Get user id</param>
        /// <returns>list of all notifications related to user id using paging</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Notification&gt; notifications = control.GetNotification(0, 10, user.IdUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Notification> GetNotification(Int32 startIndex, Int32 pageCount, Int32 userId)
        {
            List<Notification> notifications = null;
            try
            {
                UserManager mgr = new UserManager();
                //GeosWorkbenchVersionsManager geosmgr=new GeosWorkbenchVersionsManager();
                //notifications=geosmgr.GetServiceProviderDetailByCompanyId()
                notifications = mgr.GetNotification(startIndex, pageCount, userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notifications;
        }

        /// <summary>
        /// This method is to get notification by notification id
        /// </summary>
        /// <param name="userId">Get userId for notification</param>
        /// <param name="notificationId">Get notificationId for notification</param>
        /// <param name="startIndex">Get startIndex of notification list</param>
        /// <param name="isLessThan">Get isLessThan of notification Id</param>
        /// <param name="pageCount">Get page limit</param>
        /// <param name="orderField">Get order by with field name</param>
        /// <param name="orderFormat">Get order by with format asc or desc</param>
        /// <returns>List of notification by notification id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Notification&gt; notifications = control.GetNotificationWithRecordId(3, 1, false, 0, 5, "Id", OrderBy.Ascending);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Notification> GetNotificationWithRecordId(Int32 userId, long notificationId, bool isLessThan, Int32 startIndex, Int32 pageCount, string orderField, OrderBy orderFormat)
        {
            List<Notification> notifications = null;
            try
            {
                UserManager mgr = new UserManager();
                notifications = mgr.GetNotification(userId, notificationId, isLessThan, startIndex, pageCount, orderField, orderFormat);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notifications;
        }

        /// <summary>
        /// This method is to get notification by notification id
        /// </summary>
        /// <param name="userId">Get userId for notification</param>
        /// <param name="notificationId">Get notificationId for notification</param>
        /// <param name="startIndex">Get startIndex of notification list</param>
        /// <param name="isLessThan">Get isLessThan of notification Id</param>
        /// <param name="pageCount">Get page limit</param>
        /// <param name="orderField">Get order by with field name</param>
        /// <param name="orderFormat">Get order by with format asc or desc</param>
        /// <returns>List of notification by notification id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     public async static void GetAllNotificationAsync()
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Notification&gt; notifications = await control.GetNotificationWithRecordIdAsync(3, 1, false, 0, 5, "Id", OrderBy.Ascending);
        ///      }
        /// }
        /// </code>
        /// </example>
        public async Task<List<Notification>> GetNotificationWithRecordIdTestAsync(Int32 userId, long notificationId, bool isLessThan, Int32 startIndex, Int32 pageCount, string orderField, OrderBy orderFormat)
        {
            List<Notification> notifications = null;
            try
            {
                UserManager mgr = new UserManager();
                notifications = await mgr.GetNotificationWithRecordIdTestAsync(userId, notificationId, isLessThan, startIndex, pageCount, orderField, orderFormat);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notifications;
        }

        /// <summary>
        /// This method is to delete all notification
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.DeleteAllNotification(user.IdUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void DeleteAllNotification(Int32 userId)
        {
            try
            {
                UserManager mgr = new UserManager();
                mgr.DeleteAllNotification(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        /// <summary>
        /// This method is to delete notification by id
        /// </summary>
        /// <param name="notificationId">Get notification id</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         control.DeleteNotificationById(Notification.Id);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void DeleteNotificationById(Int32 notificationId)
        {
            try
            {
                UserManager mgr = new UserManager();
                mgr.DeleteNotificationById(notificationId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all workbench version
        /// </summary>
        /// <returns>List of all geos workbench version</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GeosWorkbenchVersion&gt; geosAllWorkbenchVersion = control.GetAllWorkbenchVersion();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosWorkbenchVersion> GetAllWorkbenchVersion()
        {
            List<GeosWorkbenchVersion> geosAllWorkbenchVersion = null;
            try
            {
                GeosWorkbenchVersionsManager geosmgr = new GeosWorkbenchVersionsManager();
                geosAllWorkbenchVersion = geosmgr.GetAllWorkbenchVersion();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosAllWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get list of Document types
        /// </summary>
        /// <returns>List of Document types</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;DocumentType&gt; documentTypes = control.GetAllDocumentType();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<DocumentType> GetAllDocumentType()
        {
            List<DocumentType> documentTypes = null;
            try
            {
                UserManager mgr = new UserManager();
                documentTypes = mgr.GetAllDocumentType();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return documentTypes;
        }


        /// <summary>
        /// This method is to get total count of notification
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <returns>Total count of notification</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         Int32 totalNotification = control.GetAllNotificationCount(user.IdUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Int32 GetAllNotificationCount(Int32 userId)
        {
            Int32 totalNotification;
            try
            {
                UserManager mgr = new UserManager();
                totalNotification = mgr.GetAllNotificationCount(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return totalNotification;
        }

        /// <summary>
        /// This method is to get all sites having user permission
        /// </summary>
        /// <param name="userId">Get userId</param>
        /// <returns>List of sites having user permission</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Company&gt; companyList = control.GetAllCompanyByUserId(user.IdUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetAllCompanyByUserId(Int32 userId)
        {
            List<Company> companyList = null;
            try
            {
                UserManager mgr = new UserManager();
                companyList = mgr.GetAllCompanyByUserId(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companyList;
        }

        /// <summary>
        /// This method is to get all uithemes
        /// </summary>
        /// <returns>List of all UIThemes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;UITheme&gt; uiThemeList = control.GetAllThemes();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<UITheme> GetAllThemes()
        {
            List<UITheme> uiThemeList = null;
            try
            {
                UserManager mgr = new UserManager();
                uiThemeList = mgr.GetAllThemes();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return uiThemeList;
        }

        /// <summary>
        /// This method is to get workbench version by id
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get id of workbench version</param>
        /// <returns>Details of workbench version by id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosWorkbenchVersion geosWorkbenchVersion = control.GetWorkbenchVersionById(2);
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosWorkbenchVersion GetWorkbenchVersionById(Int32 idGeosWorkbenchVersion)
        {
            GeosWorkbenchVersion geosWorkbenchVersion = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                geosWorkbenchVersion = geosmodulemgr.GetWorkbenchVersionById(idGeosWorkbenchVersion);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get short name
        /// </summary>
        /// <param name="userId">Get userId</param>
        /// <returns>short name realted to id user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         String shortName = control.GetShortName(2);
        ///     }
        /// }
        /// </code>
        /// </example>
        public String GetShortName(Int32 userId)
        {
            String shortName = null;
            try
            {
                UserManager usrManager = new UserManager();
                shortName = usrManager.GetShortName(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return shortName;
        }

        /// <summary>
        /// This method is to get back version of geos workbench
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get latest version id</param>
        /// <returns>Get back version of geos workbench</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosWorkbenchVersion geosWorkbenchVersion = control.GetWorkbenchBackVersionToRestoreById(2);
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosWorkbenchVersion GetWorkbenchBackVersionToRestoreById(Int32 idGeosWorkbenchVersion)
        {
            GeosWorkbenchVersion geosWorkbenchVersion = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                geosWorkbenchVersion = geosmodulemgr.GetWorkbenchBackVersionToRestoreById(idGeosWorkbenchVersion);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get workbench version by version number
        /// </summary>
        /// <param name="versionNumber">Get version number of workbench version</param>
        /// <returns>Details of workbench version by version number</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosWorkbenchVersion geosWorkbenchVersion = control.GetWorkbenchVersionByVersionNumber("0.0.0.4");
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosWorkbenchVersion GetWorkbenchVersionByVersionNumber(string versionNumber)
        {
            GeosWorkbenchVersion geosWorkbenchVersion = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                geosWorkbenchVersion = geosmodulemgr.GetWorkbenchVersionByVersionNumber(versionNumber);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosWorkbenchVersion;
        }

        /// <summary>
        /// This method is to get geos version beta tester
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <param name="versionId">Get geos version id</param>
        /// <returns>Login user is Geos workbench version beta tester</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         bool isGeosWorkbenchVersionBetaTester = control.IsGeosWorkbenchVersionBetaTester(1,1);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool IsGeosWorkbenchVersionBetaTester(int versionId, int userId)
        {
            bool isGeosWorkbenchVersionBetaTester = false;
            try
            {
                UserManager mgr = new UserManager();
                isGeosWorkbenchVersionBetaTester = mgr.IsGeosWorkbenchVersionBetaTester(versionId, userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isGeosWorkbenchVersionBetaTester;
        }


        /// <summary>
        /// This method is to get Geos provider details
        /// </summary>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Geos provider details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GeosProvider geosProvider = control.GetServiceProviderDetailByCompanyId(8);
        ///     }
        /// }
        /// </code>
        /// </example>
        public GeosProvider GetServiceProviderDetailByCompanyId(Int32 idCompany)
        {
            GeosProvider geosProvider = null;
            try
            {
                GeosWorkbenchVersionsManager geosmodulemgr = new GeosWorkbenchVersionsManager();
                geosProvider = geosmodulemgr.GetServiceProviderDetailByCompanyId(idCompany);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosProvider;
        }


        /// <summary>
        /// This method is to get all users
        /// </summary>
        /// <returns>List of users</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;User&gt; users = control.GetAllUser();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<User> GetAllUser()
        {
            List<User> users = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                users = mgr.GetAllUser(connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return users;
        }


        /// <summary>
        /// This method is to get all permission
        /// </summary>
        /// <returns>List of permissions</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;Permission&gt; permissions = control.GetAllPermissions();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Permission> GetAllPermissions(Int32 userId)
        {
            List<Permission> permissions = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                permissions = mgr.GetAllPermissions(connectionString, userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return permissions;
        }


        public bool AddUserPermissions(List<UserPermission> userPermissions)
        {
            bool isAdded = false;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isAdded = mgr.AddUserPermissions(connectionString, userPermissions);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isAdded;
        }

        public bool AddUserSitePermissions(List<SiteUserPermission> siteUserPermissions)
        {
            bool isAdded = false;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isAdded = mgr.AddUserSitePermissions(connectionString, siteUserPermissions);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isAdded;
        }

        /// <summary>
        /// Sprint 24 [M024-09] Wrong permissions displayed in users section.
        /// This method is used to display user permission by iduser and by idGeosModule.
        /// </summary>
        /// <param name="userId">The id of active user</param>
        /// <param name="idGeosModule">The id of geos module like CRM - 5</param>
        /// <returns>List of Permissions to user.</returns>
        public List<Permission> GetUserPermissionsByGeosModule(Int32 userId, Byte idGeosModule)
        {
            List<Permission> permissions = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                permissions = mgr.GetUserPermissionsByGeosModule(connectionString, userId, idGeosModule);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return permissions;
        }

        /// <summary>
        /// Sprint 24 [M024-01] (#36880) Edit profile displaying always fpinas!
        /// This method is used to display user details by user Id
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The User details</returns>
        public User GetUserProfileDetailsById(Int32 userId)
        {
            User user = null;
            try
            {
                UserManager mgr = new UserManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                user = mgr.GetUserProfileDetailsById(workbenchConnectionString, userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        /// <summary>
        /// [CRM-M024-06] Automatic weekly report by email to sales users - Get all Automatic reports.
        /// </summary>
        /// <returns></returns>
        public List<AutomaticReport> GetAutomaticReports()
        {
            List<AutomaticReport> automaticReports = new List<AutomaticReport>();

            try
            {
                UserManager usrMgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                automaticReports = usrMgr.GetAutomaticReports(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return automaticReports;
        }

        /// <summary>
        /// [CRM-M024-06] Automatic weekly report by email to sales users - Update automatic report start date.
        /// </summary>
        /// <param name="automaticReport">The automatic report</param>
        public void UpdateAutomaticReport(AutomaticReport automaticReport)
        {
            try
            {
                UserManager usermgr = new UserManager();
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                usermgr.UpdateAutomaticReport(mainServerConnectionString, automaticReport);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public UserLoginEntry AddUserLoginEntry(UserLoginEntry userLoginEntry)
        {
            try
            {
                UserManager usermgr = new UserManager();
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return usermgr.AddUserLoginEntry(mainServerWorkbenchConnectionString, userLoginEntry);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public void UpdateUserLoginEntry(UserLoginEntry userLoginEntry)
        {
            try
            {
                UserManager usermgr = new UserManager();
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                usermgr.UpdateUserLoginEntry(mainServerWorkbenchConnectionString, userLoginEntry);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<GeosProvider> GetGeosProviderList()
        {
            List<GeosProvider> lstGeosProvider = new List<GeosProvider>();
            try
            {
                UserManager usermgr = new UserManager();
                lstGeosProvider = usermgr.GetGeosProviderList();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lstGeosProvider;
        }

        /// <summary>
        /// This method is used to get app setting data using in query
        /// </summary>
        /// <param name="IdAppSettings">comma seperated app settings like 2,3,4,5</param>
        /// <returns>The List of GeosAppSetting</returns>
        public List<GeosAppSetting> GetSelectedGeosAppSettings(string IdAppSettings)
        {
            List<GeosAppSetting> geosAppSettings = new List<GeosAppSetting>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                UserManager usermgr = new UserManager();
                geosAppSettings = usermgr.GetSelectedGeosAppSettings(workbenchConnectionString, IdAppSettings);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return geosAppSettings;
        }

        public List<Company> GetAllCompanyList()
        {
            List<Company> company = null;
            try
            {
                UserManager mgr = new UserManager();
                company = mgr.GetAllCompanyList();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        /// <summary>
        /// [001][cpatil][12-09-2018][CRM-M047-01] Sales managers hierarchy
        /// </summary>
        /// <param name="idManager">The manager id.</param>
        /// <returns>List of availble users under manager</returns>
        //public List<UserManagerDtl> GetManagerUsersHierarchy(Int32 idManager)
        //{
        //    List<UserManagerDtl> userManagerDtl = new List<UserManagerDtl>();
        //    try
        //    {
        //        string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
        //        UserManager usermgr = new UserManager();
        //        userManagerDtl = usermgr.GetManagerUsersHierarchy(idManager, workbenchConnectionString);
        //    }
        //    catch (EntityException ee)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ee.InnerException.Message.ToString();
        //        exp.ErrorDetails = ee.ToString();

        //        throw new FaultException<ServiceException>(exp, ee.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }

        //    return userManagerDtl;
        //}

        public List<DomainUser> GetUserDataFromTheActiveDirectory()
        {
            try
            {
                UserManager mgr = new UserManager();
                return mgr.GetUserDataFromTheActiveDirectory();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public void UpdateUserIsValidateTrue(User user)
        {
            try
            {
                UserManager usermgr = new UserManager();
                usermgr.UpdateUserIsValidateTrue(user);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // This Method Use for Upload File to Main Server
        public FileReturnMessage FileUpload(FileDetail fileDetail)
        {

            FileStream fileStream = null;
            FileReturnMessage fileReturnMessage = new FileReturnMessage();
            fileReturnMessage.IsFileActionPerformed = false;
            // string fileUploadPath = Path.Combine(Properties.Settings.Default.FilePath, fileDetail.FilePath);
            try
            {
                if (fileDetail.FileByte.Length > 0 && fileDetail.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                    {
                        FileInfo fileName = new FileInfo(fileDetail.FilePath);
                        DirectoryInfo dirInfo = new DirectoryInfo(fileName.DirectoryName);
                        if (!dirInfo.Exists)
                        {
                            dirInfo.Create();
                        }

                        fileStream = new FileStream(fileDetail.FilePath.ToString(), FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            // byte[] by = new byte[257399];
                            fs.Write(fileDetail.FileByte, 0, fileDetail.FileByte.Length);
                            fileReturnMessage.IsFileActionPerformed = true;
                        }

                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;
        }
        // perform operation 

        // This method use For check given Floder path exist or not on main Server
        public FileReturnMessage CheckDirectoryExistOrNot(FileDetail fileDetail)
        {
            FileReturnMessage fileReturnMessage = new FileReturnMessage();
            try
            {
                if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                {
                    if (Directory.Exists(fileDetail.FilePath.ToString()))
                        fileReturnMessage.IsFileActionPerformed = true;
                    else
                        fileReturnMessage.IsFileActionPerformed = false;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;

        }
        // This method use For check given File path exist or not on main Server
        public FileReturnMessage CheckFileExistOrNot(FileDetail fileDetail)
        {
            FileReturnMessage fileReturnMessage = new FileReturnMessage();

            try
            {
                if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                {
                    if (File.Exists(fileDetail.FilePath.ToString()))
                        fileReturnMessage.IsFileActionPerformed = true;
                    else
                        fileReturnMessage.IsFileActionPerformed = false;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;

        }
        // This method use For check given Floder path exist or not on main Server
        public FileReturnMessage CreateDirectory(FileDetail fileDetail)
        {
            FileReturnMessage fileReturnMessage = new FileReturnMessage();
            try
            {
                if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                {
                    Directory.CreateDirectory(fileDetail.FilePath.ToString());
                    fileReturnMessage.IsFileActionPerformed = true;
                }
                else
                    fileReturnMessage.IsFileActionPerformed = false;
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;

        }

        // Delete File From  Main Server
        public FileReturnMessage DeleteFile(FileDetail fileDetail)
        {
            FileReturnMessage fileReturnMessage = new FileReturnMessage();
            try
            {
                if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                {
                    File.Delete(fileDetail.FilePath.ToString());
                    fileReturnMessage.IsFileActionPerformed = true;
                }
            }

            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;
        }

        // This method use for Delete Folder From  Main Server
        public FileReturnMessage DeleteFolder(FileDetail fileDetail)
        {
            FileReturnMessage fileReturnMessage = new FileReturnMessage();
            //fileReturnMessage.IsFileActionPerformed = false;
            try
            {
                if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                {
                    Directory.Delete(fileDetail.FilePath.ToString(), true);
                    fileReturnMessage.IsFileActionPerformed = true;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;
        }
        // This Folder use for Raname folder Name on Main Server
        public FileReturnMessage RenameFolder(FileDetail fileDetail)
        {
            FileReturnMessage fileReturnMessage = new FileReturnMessage();
            // fileReturnMessage.IsFileActionPerformed = false;

            try
            {
                if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                {

                    Directory.Move(fileDetail.fileOldName.ToString(), fileDetail.FilePath.ToString());
                    fileReturnMessage.IsFileActionPerformed = true;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;
        }
        // This Folder use for Raname File Name on Main Server
        public FileReturnMessage RenameFile(FileDetail fileDetail)
        {
            FileReturnMessage fileReturnMessage = new FileReturnMessage();
            try
            {
                if (!string.IsNullOrEmpty(fileDetail.FilePath.ToString()))
                {
                    System.IO.File.Move(fileDetail.fileOldName.ToString(), fileDetail.FilePath.ToString());
                    fileReturnMessage.IsFileActionPerformed = true;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileReturnMessage;
        }


        public GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting)
        {
            GeosAppSetting geosAppSetting = new GeosAppSetting();

            try
            {
                UserManager mgr = new UserManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                geosAppSetting = mgr.GetGeosAppSettings(IdAppSetting, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosAppSetting;
        }

        public string GetUserNameByCompanyCode(string companyCode)
        {
            try
            {
                UserManager mgr = new UserManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUserNameByCompanyCode(companyCode, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
           
        }


        public User GetUserDetailsByEmailId(string userEmailId)
        {
            User user = null;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                user = mgr.GetUserDetailsByEmailId(userEmailId, connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        public bool UpdateUserNotification_V2043(Int32 userId)
        {
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateUserNotification_V2043(userId,connectionString);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public Int64 GetMaxNotificationId(Int32 userId)
        {
            try
            {
                UserManager mgr = new UserManager();
              
                return mgr.GetMaxNotificationId(userId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
    }
}
