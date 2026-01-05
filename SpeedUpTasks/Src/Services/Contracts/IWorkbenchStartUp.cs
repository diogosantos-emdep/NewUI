using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common.Hrm;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.HarnessPart;
using Emdep.Geos.Data.Common.FileReplicator;

namespace Emdep.Geos.Services.Contracts
{
    [ServiceContract]
    public interface IWorkbenchStartUp
    {
        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="userlogin">Get login detail to check</param>
        /// <param name="password">Get password detail to check</param>
        /// <returns>Details of user from table User</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserByLogin(string userLogin, string password);

        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="id">Get user id detail to check</param>
        /// <returns>Details of user from table User</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserById(int userId);

        /// <summary>
        /// This method is to get workstation by WorkstationIP
        /// </summary>
        /// <param name="WorkstationIP">Get current user system IP</param>
        /// <returns>Details of workstation from class Workstation</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Workstation GetWorkstationByIP(string workstationIP);

        /// <summary>
        /// This method is to get workstation by WorkstationID
        /// </summary>
        /// <param name="WorkstationID">Get workstation detail related to WorkstationID</param>
        /// <returns>Details of workstation from class Workstation</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Workstation GetWorkstationById(int workstationId);

        /// <summary>
        /// This method is to get stage by stageID
        /// </summary>
        /// <param name="StageID">Get stage detail related to stageID</param>
        /// <returns>Details of stage related to stageID  from class stage</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Stage GetStageById(int stageId);

        /// <summary>
        /// This method is to add user logs in class UserLog
        /// </summary>
        /// <param name="UserLog">To get details of UserLog from class UserLog</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddUserLog(UserLog userLog);

        /// <summary>
        /// This method is to get current version from class GeosWorkbenchVersion
        /// </summary>
        /// <returns>Details of current version from class GeosWorkbenchVersion</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosWorkbenchVersion GetLatestVersion();

        /// <summary>
        /// This method is to get current version release notes from class GeosReleaseNote
        /// </summary>
        /// <param name="geosworkbenchversion">To get details of current version from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosReleaseNotes related to current version</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosReleaseNote> GetReleaseNotesByVersion(GeosWorkbenchVersion geosWorkbenchVersion);

        /// <summary>
        /// This method is to add download version by IP in class GeosWorkbenchVersionDownload
        /// </summary>
        /// <param name="geosworkbenchversion">To get details of current version from class GeosWorkbenchVersion</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddDownloadVersionByIP(GeosWorkbenchVersionDownload geosWorkbenchVersionDownload);

        /// <summary>
        /// This method is to get current version files from class GeosWorkbenchVersionsFile
        /// </summary>
        /// <param name="IdGeosWorkbenchVersion">Get current version id from class GeosWorkbenchVersion</param>
        /// <returns>List of GeosWorkbenchVersionsFiles related to current version id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosWorkbenchVersionsFile> GetWorkbenchVersionFiles(Int32 idGeosWorkbenchVersion);

        /// <summary>
        /// This method is to get list of documentations related to current version id from class GeosModuleDocumentation
        /// </summary>
        /// <param name="IdGeosWorkbenchVersion">Get current version id from table GeosWorkbenchVersion</param>
        /// <returns>List of GeosModuleDocumentations related to current version id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosModuleDocumentation> GetModuleDocumentations(Int32 idGeosWorkbenchVersion);

        /// <summary>
        ///  This method is to download current version documentation filename and return it in bytes  
        /// </summary>
        /// <param name="IdGeosModuleDocumentation">Get current version document id from table GeosModuleDocumentation</param>
        /// <returns>FileTransferRequest:-File in bytes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FileTransferRequest DownloadModuleDocument(Int32 idGeosModuleDocumentation);


        /// <summary>
        /// This method is to send ForgetPasswordMail
        /// </summary>
        /// <param name="emailId">Get receiver EmailId</param>
        /// <param name="code">Get Code</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SendForgetPasswordMail(string emailId, string code);


        /// <summary>
        /// This method is to update new password
        /// </summary>
        /// <param name="userName">Get UserName or EmailId</param>
        /// <param name="NewPassword">Get Password to update</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserPassword(string userName, string newPassword);

        /// <summary>
        /// This method is to get installer version
        /// </summary>
        /// <returns>Workbench installer version</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        String GetWorkbenchInstallerVersion();

        /// <summary>
        /// This method is to get user detail by code
        /// </summary>
        /// <param name="code">Get Code</param>
        /// <returns>Details of user related to code  from class user</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserByCode(string code);

        /// <summary>
        /// This method is to get company detail by name
        /// </summary>
        /// <param name="sitename">Get company name</param>
        /// <returns>Details of company related to code from class company</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyByName(string companyName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserManagerDtl> GetManagerUsers(Int32 idManager);

        // /// <summary>
        ///// This method is to get company information
        ///// </summary>
        ///// <param name="companyId">Get company id</param>
        ///// <returns>company information</returns>
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //EmdepSite GetCompanyByUserCompanyId(Int32 siteId);

        /// <summary>
        /// This method is to get company detail by alias
        /// </summary>
        /// <param name="companyAlias">Get company alias</param>
        /// <returns>Details of company related to alias from class company</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyByAlias(string companyAlias);

        /// <summary>
        /// This method is to get list of all companies
        /// </summary>
        /// <returns>List of all companies</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanyList();

        /// <summary>
        /// This method is to get all versions from class GeosWorkbenchVersion
        /// </summary>
        /// <returns>List of GeosWorkbenchVersion</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosWorkbenchVersion> GetAllVersions();


        ///// <summary>
        ///// This method is to get list of all departments
        ///// </summary>
        ///// <returns>List of all departments</returns>
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<Department> GetAllDepartment();


        ///// <summary>
        ///// This method is to get list of all job description
        ///// </summary>
        ///// <returns>List of all job description</returns>
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<JobDescription> GetAllJobDescription();

        /// <summary>
        /// This method is to get data from two different database
        /// </summary>
        /// <returns>User details</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetDataFromDifferentDatabase();


        /// <summary>
        /// This method is to get all user 
        /// </summary>
        /// <returns>List of all user</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<User> GetImpersonateUser();

        /// <summary>
        /// This method is to update user profile
        /// </summary>
        /// <param name="user">Get user detail to update</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserProfile(User user);


        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="userlogin">Get login detail for autentication</param>
        /// <returns>Details of user from class User</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserByLoginName(string login);

        /// <summary>
        /// This method is to get permission by id
        /// </summary>
        /// <param name="permission">Get permission class</param>
        /// <returns>Details of permission from class permission</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Permission GetPermissionById(int idPermission);


        /// <summary>
        /// This method is to get server date time
        /// </summary>
        /// <returns>Server Date Time</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DateTime GetServerDateTime();

        /// <summary>
        /// This method is to get user detail by emailid or login
        /// </summary>
        /// <param name="emailid">Get EmailID</param>
        /// <returns>Details of user related to emailid or login  from class user</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserByEmailId(string emailId);

        /// <summary>
        /// This method is to get list of modules related to user id permission
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of modules related to user id permission</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosModule> GetUserModulesPermissions(int idUser);

        /// <summary>
        /// This method is to download current company image and return it in bytes 
        /// </summary>
        /// <param name="mgr">To get current id company from class company</param>
        /// <returns>File in bytes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetCompanyImage(Int32 idCompany);

        /// <summary>
        /// This method is to get list of all notifications related to user id(Async)
        /// </summary>
        /// <returns>list of all notifications related to user id</returns>
        [OperationContract(AsyncPattern = true)]
        [FaultContract(typeof(ServiceException))]
        Task<List<Notification>> GetAllNotificationAsync(Int32 userId);

        /// <summary>
        /// This method is to update notification status read
        /// </summary>
        /// <param name="userId">Get user id</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use UpdateUserNotification_V2043 instead.")]
        void UpdateUserNotification(Int32 userId);

        /// <summary>
        /// This method is to update new password
        /// </summary>
        /// <param name="newPassword">Get new password to update</param>
        /// <param name="oldPassword">Get old password to update</param>
        /// <param name="userId">Get user id</param>
        /// <returns>Detail of user related to userId</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User ChangeUserPassword(string newPassword, string oldPassword, Int32 userId);

        /// <summary>
        /// This method is to get user related to userId and Password
        /// </summary>
        /// <param name="password">Get password</param>
        /// <param name="userId">Get userId</param>
        /// <returns>Details of user related to userId and Password from class user</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User CheckUserPassword(string password, Int32 userId);

        /// <summary>
        /// This method is to get list of all languages
        /// </summary>
        /// <returns>List of all languages</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Language> GetAllLanguage();

        /// <summary>
        /// This method is to update user isvalidate false
        /// </summary>
        /// <param name="user">Get user details to update</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserIsValidateFalse(User user);

        /// <summary>
        /// This method is to set user profile image
        /// </summary>
        /// <param name="user">Get user details to set user profile image</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SetUserProfileImage(User user);

        /// <summary>
        /// This method is to update isNotification by notification of Id
        /// </summary>
        /// <param name="notificationId">Get notification id to update isnotification</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateIsNotificationByID(Int32 notificationId);

        /// <summary>
        /// This method is to delete user profile image
        /// </summary>
        /// <param name="userName">Get user login name</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteUserProfileImage(User user);

        /// <summary>
        /// This method is to get list of all unread notifications related to user id
        /// </summary>
        /// <returns>list of all unread notifications related to user id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Notification> GetAllUnreadNotificationTest(Int32 userId);

        /// <summary>
        /// This method is to get list of all notifications related to user id
        /// </summary>
        /// <returns>list of all notifications related to user id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Notification> GetAllNotificationTest(Int32 userId);

        /// <summary>
        /// This method is to get list of all unread notifications related to user id(Async)
        /// </summary>
        /// <returns>list of all unread notifications related to user id</returns>
        [OperationContract(AsyncPattern = true)]
        [FaultContract(typeof(ServiceException))]
        Task<List<Notification>> GetAllUnreadNotificationAsync(Int32 userId);

        /// <summary>
        /// This method is to get list of all notifications related to user id using paging
        /// </summary>
        /// <returns>list of all notifications related to user id using paging</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Notification> GetNotification(Int32 startIndex, Int32 pageCount, Int32 userId);

        /// <summary>
        /// This method is to delete notification by id
        /// </summary>
        /// <param name="notificationId">Get notification id</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteNotificationById(Int32 notificationId);

        /// <summary>
        /// This method is to delete all notification
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteAllNotification(Int32 userId);

        /// <summary>
        /// This method is to get all workbench version
        /// </summary>
        /// <returns>List of all geos workbench version</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosWorkbenchVersion> GetAllWorkbenchVersion();

        /// <summary>
        /// This method is to get list of Document types
        /// </summary>
        /// <returns>List of Document types</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DocumentType> GetAllDocumentType();

        /// <summary>
        /// This method is to get total count of notification
        /// </summary>
        /// <returns>Total count of notification</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int32 GetAllNotificationCount(Int32 userId);

        /// <summary>
        /// This method is to get all companies having user permission
        /// </summary>
        /// <param name="userId">Get userId</param>
        /// <returns>List of companies having user permission</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompanyByUserId(Int32 userId);

        /// <summary>
        /// This method is to get all uithemes
        /// </summary>
        /// <returns>List of all UIThemes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UITheme> GetAllThemes();

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
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Notification> GetNotificationWithRecordId(Int32 userId, long notificationId, bool isLessThan, Int32 startIndex, Int32 pageCount, string orderField, OrderBy orderFormat);

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
        ///         List&lt;Notification&gt; notifications = await control.GetNotificationWithRecordIdTestAsync(3, 1, false, 0, 5, "Id", OrderBy.Ascending);
        ///      }
        /// }
        /// </code>
        /// </example>
        [OperationContract(AsyncPattern = true)]
        [FaultContract(typeof(ServiceException))]
        Task<List<Notification>> GetNotificationWithRecordIdTestAsync(Int32 userId, long notificationId, bool isLessThan, Int32 startIndex, Int32 pageCount, string orderField, OrderBy orderFormat);

        /// <summary>
        /// This method is to get workbench version by id
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get id of workbench version</param>
        /// <returns>Details of workbench version by id</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosWorkbenchVersion GetWorkbenchVersionById(Int32 idGeosWorkbenchVersion);

        /// <summary>
        /// This method is to get back version of geos workbench
        /// </summary>
        /// <param name="idGeosWorkbenchVersion">Get latest version id</param>
        /// <returns>Get back version of geos workbench</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosWorkbenchVersion GetWorkbenchBackVersionToRestoreById(Int32 idGeosWorkbenchVersion);

        /// <summary>
        /// This method is to get workbench version by version number
        /// </summary>
        /// <param name="versionNumber">Get version number of workbench version</param>
        /// <returns>Details of workbench version by version number</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosWorkbenchVersion GetWorkbenchVersionByVersionNumber(string versionNumber);

        /// <summary>
        /// This method is to get geos version beta tester
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <param name="versionId">Get geos version id</param>
        /// <returns>Geos version beta tester details</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsGeosWorkbenchVersionBetaTester(int versionId, int userId);

        /// <summary>
        /// This method is to get Geos provider details
        /// </summary>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Geos provider details</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosProvider GetServiceProviderDetailByCompanyId(Int32 idCompany);

        /// <summary>
        /// This method is to update is new to 0
        /// </summary>
        /// <param name="notificationList">List of all unread notification list</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateListOfNotification(List<long> notificationids);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        String GetShortName(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosWorkbenchVersion GetCurrentVersionBetaWise(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosWorkbenchVersion> GetAllVersionBetaWise(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosWorkbenchVersion GetCurrentPublishVersion();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosWorkbenchVersion GetUserIsBetaCurrentVersion(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<User> GetAllUser();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Permission> GetAllPermissions(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUserPermissions(List<UserPermission> userPermissions);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUserSitePermissions(List<SiteUserPermission> siteUserPermissions);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Permission> GetUserPermissionsByGeosModule(Int32 userId, Byte idGeosModule);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserProfileDetailsById(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AutomaticReport> GetAutomaticReports();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateAutomaticReport(AutomaticReport automaticReport);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        UserLoginEntry AddUserLoginEntry(UserLoginEntry userLoginEntry);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserLoginEntry(UserLoginEntry userLoginEntry);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosProvider> GetGeosProviderList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosAppSetting> GetSelectedGeosAppSettings(string IdAppSettings);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompanyList();

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<UserManagerDtl> GetManagerUsersHierarchy(Int32 idManager);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DomainUser> GetUserDataFromTheActiveDirectory();


        /// <summary>
        /// This method is to update user isvalidate true
        /// </summary>
        /// <param name="user">Get user details to update</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserIsValidateTrue(User user);


        [OperationContract]
        FileReturnMessage FileUpload(FileDetail fileDetail);

        [OperationContract]
        FileReturnMessage DeleteFile(FileDetail fileDetail);

        [OperationContract]
        FileReturnMessage DeleteFolder(FileDetail fileDetail);

        [OperationContract]
        FileReturnMessage CheckDirectoryExistOrNot(FileDetail fileDetail);

        [OperationContract]
        FileReturnMessage CreateDirectory(FileDetail fileDetail);

        [OperationContract]
        FileReturnMessage CheckFileExistOrNot(FileDetail fileDetail);

        [OperationContract]
        FileReturnMessage RenameFolder(FileDetail fileDetail);

        [OperationContract]
        FileReturnMessage RenameFile(FileDetail fileDetail);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetUserNameByCompanyCode(string companyCode);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserDetailsByEmailId(string userEmailId);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateUserNotification_V2043(Int32 userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetMaxNotificationId(Int32 userId);
    }
}
