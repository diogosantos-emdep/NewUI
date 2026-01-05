using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.DataAccess;
using Emdep.Geos.Data.Common;
using System.Net;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure;
using System.Xml;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.HarnessPart;
using MySql.Data.MySqlClient;
using System.Data;
using System.Transactions;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class UserManager
    {
        DataTable DataTableUserManager;

        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="userlogin">Get login detail for autentication</param>
        /// <param name="password">Get password detail for autentication</param>
        /// <returns>Details of user from class User</returns>
        public User GetUserByLogin(string login, string password, string connectionString)
        {
            User user = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users.Include("UserPermissions.Permission") where records.Login == login && records.Password == password select records).SingleOrDefault();

                if (user != null)
                {
                    Company company = new Company();

                    if (user.IdCompany != null)
                    {
                        using (MySqlConnection myConnection = new MySqlConnection(connectionString))
                        {
                            myConnection.Open();
                            //MySqlCommand myCommand = new MySqlCommand("select s.IdSite, s.Name, s.IdCountry, cou.name as countryName, cou.IdZone, zones.Name as zoneName from sites s inner join countries cou on s.IdCountry = cou.idCountry inner join zones ON zones.IdZone = cou.IdZone where s.IdSite=" + user.IdCompany, myConnection);
                            MySqlCommand concommand = new MySqlCommand("sites_GetLoginUserSiteDetails", myConnection);
                            concommand.CommandType = CommandType.StoredProcedure;
                            concommand.Parameters.AddWithValue("_IdCompany", user.IdCompany);

                            using (MySqlDataReader reader = concommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                                    company.Name = reader["Name"].ToString();
                                    company.ShortName = reader["ShortName"].ToString();
                                    company.IdCountry = Convert.ToByte(reader["IdCountry"].ToString());
                                    company.Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["countryName"].ToString(), IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["zoneName"].ToString() } };
                                }
                            }
                        }
                    }

                    user.Company = company;
                }
            }

            return user;
        }


        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="id">Get user id detail to check</param>
        /// <returns>Details of user from class User</returns>
        public User GetUserById(int userId, string connectionString)
        {
            User user = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users.Include("UserPermissions.Permission") where records.IdUser == userId select records).SingleOrDefault();
            }
            Company company = new Company();
            if (user != null)
            {
                if (user.IdCompany != null)
                {
                    using (MySqlConnection myConnection = new MySqlConnection(connectionString))
                    {
                        myConnection.Open();
                        MySqlCommand concommand = new MySqlCommand("sites_GetLoginUserSiteDetails", myConnection);
                        concommand.CommandType = CommandType.StoredProcedure;
                        concommand.Parameters.AddWithValue("_IdCompany", user.IdCompany);

                        using (MySqlDataReader reader = concommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                                company.Name = reader["Name"].ToString();
                                company.ShortName = reader["ShortName"].ToString();
                                company.IdCountry = Convert.ToByte(reader["IdCountry"].ToString());
                                company.Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["countryName"].ToString(), IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["zoneName"].ToString() } };
                            }
                        }

                    }
                }
                user.Company = company;
            }
            return user;
        }

        /// <summary>
        /// This method is to get workstation by WorkstationIP
        /// </summary>
        /// <param name="WorkstationIP">Get current user system IP</param>
        /// <returns>Details of workstation from class Workstation</returns>
        public Workstation GetWorkstationByIP(string workstationIp)
        {
            Workstation workstation = null;
            using (var db = new WorkbenchContext())
            {

                workstation = (from records in db.Workstations.Include("Stage")
                               where records.IP == workstationIp
                               select records).SingleOrDefault();
            }
            return workstation;
        }

        public List<UserManagerDtl> GetManagerUsers(Int32 idManager, string connectionString)
        {
            List<UserManagerDtl> userManagers = new List<UserManagerDtl>();
            #region
            //using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            //{
            //    myConnection.Open();
            //    MySqlCommand concommand = new MySqlCommand("GetSalesUserByManager", myConnection);
            //    concommand.CommandType = CommandType.StoredProcedure;
            //    concommand.Parameters.AddWithValue("_IdManager", idManager);

            //    //MySqlCommand myCommand = new MySqlCommand("SELECT users.IdUser, users.FirstName, users.LastName FROM user_managers INNER JOIN users ON user_managers.IdUser = users.IdUser WHERE IdManager =" + idManager, myConnection);

            //    using (MySqlDataReader reader = concommand.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            UserManagerDtl userManagerDtl = new UserManagerDtl();
            //            userManagerDtl.IdUser = Convert.ToInt32(reader["IdUser"].ToString());
            //            userManagerDtl.IdManager = idManager;

            //            User user = new User();
            //            user.IdUser = Convert.ToInt32(reader["IdUser"].ToString());
            //            user.FirstName = reader["FirstName"].ToString();
            //            user.LastName = reader["LastName"].ToString();

            //            if (reader["CompanyEmail"] != DBNull.Value)
            //                user.CompanyEmail = reader["CompanyEmail"].ToString();

            //            if (reader["IsEnabled"] != DBNull.Value)
            //                user.IsEnabled = Convert.ToByte(reader["IsEnabled"]);

            //            userManagerDtl.User = user;
            //            userManagers.Add(userManagerDtl);
            //        }
            //    }
            //}
            #endregion
            string managerList = idManager.ToString();
            DataTable dtfinal = new DataTable();

            GetSalesUserByListofUserManager(connectionString, idManager.ToString(), idManager, dtfinal);

            DataRow[] datarow = DataTableUserManager.Select().OrderBy(um => um["FirstName"]).ThenBy(um => um["LastName"]).ToArray();

            foreach (DataRow item in datarow)
            {
                if (!userManagers.Any(um => um.IdUser == Convert.ToInt32(item["IdUser"])))
                {
                    UserManagerDtl userManagerDtl = new UserManagerDtl();
                    userManagerDtl.IdUser = Convert.ToInt32(item["IdUser"]);
                    userManagerDtl.IdManager = idManager;

                    User user = new User();
                    user.IdUser = Convert.ToInt32(item["IdUser"]);
                    user.FirstName = Convert.ToString(item["FirstName"]);
                    user.LastName = Convert.ToString(item["LastName"]);

                    if (item["CompanyEmail"] != DBNull.Value)
                        user.CompanyEmail = Convert.ToString(item["CompanyEmail"]);

                    if (item["IsEnabled"] != DBNull.Value)
                        user.IsEnabled = Convert.ToByte(item["IsEnabled"]);

                    userManagerDtl.User = user;
                    userManagers.Add(userManagerDtl);
                }
            }
            return userManagers;
        }

        /// <summary>
        /// This method is to get workstation by WorkstationID
        /// </summary>
        /// <param name="WorkstationID">Get workstation detail related to WorkstationID</param>
        /// <returns>Details of workstation from class Workstation</returns>
        public Workstation GetWorkstationById(int workstationId)
        {
            Workstation workstation = null;
            using (var db = new WorkbenchContext())
            {

                workstation = (from records in db.Workstations
                               where records.IdWorkStation == workstationId
                               select records).SingleOrDefault();
            }
            return workstation;
        }

        /// <summary>
        /// This method is to get stage by stageID
        /// </summary>
        /// <param name="StageID">Get stage detail related to stageID</param>
        /// <returns>Details of stage related to stageID  from class stage</returns>
        public Stage GetStageById(int stageId)
        {
            Stage stage = null;
            using (var db = new WorkbenchContext())
            {
                stage = (from records in db.Stages
                         where records.IdStage == stageId
                         select records).SingleOrDefault();
            }
            return stage;
        }

        /// <summary>
        /// This method is to add user logs in class UserLog
        /// </summary>
        /// <param name="UserLog">To get details of UserLog from class UserLog</param>
        public void AddUserLog(UserLog userLog)
        {
            using (var db = new WorkbenchContext())
            {
                db.UserLogs.Add(userLog);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to update new password (Extra required in Future)
        /// </summary>
        /// <param name="userName">Get UserName or EmailId</param>
        /// <param name="NewPassword">Get Password to update</param>
        public void UpdateUserPassword(string userName, string newPassword)
        {
            User user = null;
            using (var db = new MainServerWorkbenchContext())
            {
                user = (from records in db.Users
                        where records.CompanyEmail == userName || records.Login == userName
                        select records).SingleOrDefault();
                user.Password = newPassword;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to update new password
        /// </summary>
        /// <param name="newPassword">Get new password to update</param>
        /// <param name="oldPassword">Get old password to update</param>
        /// <param name="userId">Get user id</param>
        /// <returns>Detail of user related to userId</returns>
        public User ChangeUserPassword(string newPassword, string oldPassword, Int32 userId)
        {
            User user = null;
            using (var db = new MainServerWorkbenchContext())
            {
                user = (from records in db.Users
                        where records.IdUser == userId && records.Password == oldPassword
                        select records).SingleOrDefault();
                if (user != null)
                {
                    user.Password = newPassword;
                    db.SaveChanges();
                }
            }
            return user;
        }

        /// <summary>
        /// This method is to get user detail by emailid or login
        /// </summary>
        /// <param name="emailid">Get EmailID</param>
        /// <returns>Details of user related to emailid or login  from class user</returns>
        public User GetUserByEmailId(string emailId)
        {
            User user = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users
                        where records.CompanyEmail == emailId || records.Login == emailId
                        select records).FirstOrDefault(); ;
            }

            return user;
        }

        /// <summary>
        /// This method is to get user detail by code
        /// </summary>
        /// <param name="code">Get Code</param>
        /// <returns>Details of user related to code  from class user</returns>
        public User GetUserByCode(string code, string connectionString)
        {
            User user = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users.Include("UserPermissions.Permission")
                        where records.CompanyCode == code
                        select records).SingleOrDefault();

                if (user != null)
                {
                    Company company = new Company();

                    if (user.IdCompany != null)
                    {
                        using (MySqlConnection myConnection = new MySqlConnection(connectionString))
                        {
                            myConnection.Open();
                            //MySqlCommand myCommand = new MySqlCommand("select s.IdSite, s.Name, s.IdCountry, cou.name as countryName, cou.IdZone, zones.Name as zoneName from sites s inner join countries cou on s.IdCountry = cou.idCountry inner join zones ON zones.IdZone = cou.IdZone where s.IdSite=" + user.IdCompany, myConnection);
                            MySqlCommand concommand = new MySqlCommand("sites_GetLoginUserSiteDetails", myConnection);
                            concommand.CommandType = CommandType.StoredProcedure;
                            concommand.Parameters.AddWithValue("_IdCompany", user.IdCompany);

                            using (MySqlDataReader reader = concommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                                    company.Name = reader["Name"].ToString();
                                    company.ShortName = reader["ShortName"].ToString();
                                    company.IdCountry = Convert.ToByte(reader["IdCountry"].ToString());
                                    company.Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["countryName"].ToString(), IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["zoneName"].ToString() } };
                                }
                            }
                        }
                    }

                    user.Company = company;
                }

                return user;
            }
        }

        /// <summary>
        /// This method is to get company detail by name
        /// </summary>
        /// <param name="sitename">Get company name</param>
        /// <returns>Details of company related to name from class company</returns>
        public Company GetCompanyByName(string companyName)
        {
            Company company = null;
            using (var db = new WorkbenchContext())
            {
                company = (from records in db.Companies join geosprovider in db.GeosProviders on records.IdCompany equals geosprovider.IdCompany where records.Name == companyName select records).SingleOrDefault();
            }

            return company;
        }

        /// <summary>
        /// This method is to get company detail by alias
        /// </summary>
        /// <param name="sitename">Get company alias</param>
        /// <returns>Details of company related to alias from class company</returns>
        public Company GetCompanyByAlias(string companyAlias)
        {
            Company company = null;
            using (var db = new WorkbenchContext())
            {
                company = (from records in db.Companies join geosprovider in db.GeosProviders on records.IdCompany equals geosprovider.IdCompany where records.Alias == companyAlias select records).SingleOrDefault();
            }

            return company;
        }

        /// <summary>
        /// This method is to get list of all companies
        /// </summary>
        /// <returns>List of all companies</returns>
        public List<Company> GetCompanyList()
        {
            List<Company> companyList = null;
            using (var db = new WorkbenchContext())
            {
                companyList = (from records in db.Companies
                               join geosprovider in db.GeosProviders on records.IdCompany equals geosprovider.IdCompany
                               select records).ToList();
            }

            return companyList;
        }

        /// <summary>
        /// This method is to get all companies having user permission
        /// </summary>
        /// <param name="userId">Get userId</param>
        /// <returns>List of companies having user permission</returns>
        public List<Company> GetAllCompanyByUserId(Int32 userId)
        {
            List<Company> companyList = null;

            using (var db = new WorkbenchContext())
            {
                companyList = (from records in db.Companies
                               join siteuserpermission in db.SiteUserPermissions on records.IdCompany equals siteuserpermission.IdCompany
                               join geosprovider in db.GeosProviders on records.IdCompany equals geosprovider.IdCompany
                               where siteuserpermission.IdUser == userId
                               select records).ToList();
            }
            return companyList;
        }

        /// <summary>
        /// This method is to get company information
        /// </summary>
        /// <param name="companyId">Get site id</param>
        /// <returns>Company information</returns>
        public GeosProvider GetCompanyByUserCompanyId(Int32 companyId)
        {
            GeosProvider site = null;
            using (var db = new WorkbenchContext())
            {
                site = (from records in db.Companies join geosprovider in db.GeosProviders on records.IdCompany equals geosprovider.IdCompany where geosprovider.IdCompany == companyId select geosprovider).SingleOrDefault();
            }

            return site;
        }

        /// <summary>
        /// This method is to get data from two different database
        /// </summary>
        /// <returns>User detail</returns>
        public User GetDataFromDifferentDatabase()
        {
            User user = null;
            using (var db = new WorkbenchContext())
            {
                using (var db1 = new HrmContext())
                {
                    // user = (from records in db.Users join depart in db1.Departments on records.IdDepartment equals depart.IdDepartment select new { Department=from departm in db1.Departments where departm.IdDepartment== depart.IdDepartment }).SingleOrDefault();
                    user = db.Users.SqlQuery("DifferentMapping").SingleOrDefault();
                    System.Data.Entity.DbSet per = db1.Set<User>();
                    per.Attach(user);
                    db1.Entry(user).Reference("Department").Load();
                }
            }
            return user;
        }

        /// <summary>
        /// This method is to get all user
        /// </summary>
        /// <returns>List of all user</returns>
        public List<User> GetImpersonateUser()
        {
            List<User> user = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users where records.IsEnabled == 1 select records).ToList();
            }

            return user;
        }

        /// <summary>
        /// This method is to update user profile
        /// </summary>
        /// <param name="user">Get user detail to update</param>
        public void UpdateUserProfile(User user, string mainServerWorkbenchConnectionString)
        {
            //using (var db = new WorkbenchContext())
            //{
            //    User usr = (from records in db.Users
            //                where records.Login == user.Login
            //                select records).SingleOrDefault();

            //    usr.IdDepartment = user.IdDepartment;
            //    usr.IdJobDescription = user.IdJobDescription;
            //    usr.IdCompany = user.IdCompany;

            //    db.SaveChanges();
            //}
            //using (var dbhrm = new HrmContext())
            //{
            //}

            TransactionScope transactionScope = null;

            using (transactionScope = new System.Transactions.TransactionScope())
            {
                try
                {
                    using (MySqlConnection connUsers = new MySqlConnection(mainServerWorkbenchConnectionString))
                    {
                        connUsers.Open();

                        MySqlCommand updateUsersCommand = new MySqlCommand("users_UpdateUserProfile", connUsers);
                        updateUsersCommand.CommandType = CommandType.StoredProcedure;

                        updateUsersCommand.Parameters.AddWithValue("_IdUser", user.IdUser);
                        updateUsersCommand.Parameters.AddWithValue("_FirstName", user.FirstName);
                        updateUsersCommand.Parameters.AddWithValue("_LastName", user.LastName);
                        updateUsersCommand.Parameters.AddWithValue("_CompanyEmail", user.CompanyEmail);
                        updateUsersCommand.Parameters.AddWithValue("_Phone", user.Phone);

                        updateUsersCommand.ExecuteNonQuery();
                        connUsers.Close();

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
            }
        }

        /// <summary>
        /// This method is to check user is valid or not from class User
        /// </summary>
        /// <param name="userlogin">Get login detail for autentication</param>
        /// <returns>Details of user from class User</returns>
        public User GetUserByLoginName(string login, string connectionString)
        {
            User user = null;
            bool fHasSpace = login.Contains(" ");
            bool fHasDot = login.Contains(".emdep");
            using (var db = new WorkbenchContext())
            {
                if (fHasSpace)
                {
                    string[] loginstr = login.Split(' ');
                    string firstname = loginstr[0];
                    string lastname = loginstr[1];

                    user = (from records in db.Users.Include("UserPermissions.Permission") where records.FirstName == firstname && records.LastName == lastname select records).SingleOrDefault();
                }
                else if (fHasDot)
                {
                    string[] loginstr = login.Split('.');
                    string loginname = loginstr[0];
                    user = (from records in db.Users.Include("UserPermissions.Permission") where records.Login == loginname select records).SingleOrDefault();
                }
                else
                    user = (from records in db.Users.Include("UserPermissions.Permission") where records.Login == login select records).SingleOrDefault();


                if (user != null)
                {
                    Company company = new Company();
                    if (user.IdCompany != null)
                    {
                        using (MySqlConnection myConnection = new MySqlConnection(connectionString))
                        {
                            myConnection.Open();
                            MySqlCommand concommand = new MySqlCommand("sites_GetLoginUserSiteDetails", myConnection);
                            concommand.CommandType = CommandType.StoredProcedure;
                            concommand.Parameters.AddWithValue("_IdCompany", user.IdCompany);

                            using (MySqlDataReader reader = concommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                                    company.Name = reader["Name"].ToString();
                                    company.ShortName = reader["ShortName"].ToString();
                                    company.IdCountry = Convert.ToByte(reader["IdCountry"].ToString());
                                    company.Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["countryName"].ToString(), IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["zoneName"].ToString() } };
                                }
                            }
                        }
                    }
                    user.Company = company;
                }
            }
            return user;
        }

        /// <summary>
        /// This method is to get permission by id
        /// </summary>
        /// <param name="permission">Get permission class</param>
        /// <returns>Details of permission from class permission</returns>
        public Permission GetPermissionById(int idPermission)
        {
            Permission permission = null;
            using (var db = new WorkbenchContext())
            {
                permission = (from records in db.Permissions.Include("UserPermissions").Include("PermissionTemplates") where records.IdPermission == idPermission select records).SingleOrDefault();
            }
            return permission;
        }

        /// <summary>
        /// This method is to get user related to userId and Password
        /// </summary>
        /// <param name="password">Get password</param>
        /// <param name="userId">Get userId</param>
        /// <returns>Details of user related to userId and Password from class user</returns>
        public User CheckUserPassword(string password, Int32 userId)
        {
            User user = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users where records.IdUser == userId && records.Password == password select records).SingleOrDefault();
            }
            return user;
        }



        /// <summary>
        /// This method is to get list of all notifications related to user id
        /// </summary>
        /// <returns>list of all notifications related to user id</returns>
        public List<Notification> GetAllNotification(Int32 userId)
        {
            List<Notification> notifications = null;
            using (var db = new WorkbenchContext())
            {
                notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId select records).ToList();
            }
            return notifications;
        }

        /// <summary>
        /// This method is to get list of all notifications related to user id(Async)
        /// </summary>
        /// <returns>list of all notifications related to user id</returns>
        public async Task<List<Notification>> GetAllNotificationAsync(Int32 userId)
        {
            return await Task.FromResult<List<Notification>>(GetAllNotification(userId));
        }

        /// <summary>
        /// This method is to update notification status read
        /// </summary>
        /// <param name="userId">Get user id</param>
        public void UpdateUserNotification(Int32 userId)
        {
            List<Notification> notifications = null;

            using (var db = new MainServerWorkbenchContext())
            {
                notifications = (from records in db.Notifications
                                 where records.Status == "Unread" && records.ToUser == userId
                                 select records).ToList();
                foreach (Notification notification in notifications)
                {
                    notification.Status = "Read";
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to update isNotification by notification of Id
        /// </summary>
        /// <param name="notificationId">Get notification id to update isnotification</param>
        public void UpdateIsNotificationByID(Int32 notificationId)
        {
            Notification notification = null;

            using (var db = new MainServerWorkbenchContext())
            {
                notification = (from records in db.Notifications
                                where records.Id == notificationId
                                select records).SingleOrDefault();
                notification.IsNew = 0;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to update user isvalidate false
        /// </summary>
        /// <param name="user">Get user details to update user isvalidate false</param>
        public void UpdateUserIsValidateFalse(User user)
        {
            User usr = null;
            using (var db = new WorkbenchContext())
            {
                usr = (from records in db.Users
                       where records.IdUser == user.IdUser
                       select records).SingleOrDefault();
                usr.IsValidated = 1;
                db.SaveChanges();

            }
        }

        /// <summary>
        /// This method is to update user isvalidate true
        /// </summary>
        /// <param name="user">Get user details to update user isvalidate true</param>
        public void UpdateUserIsValidateTrue(User user)
        {
            User usr = null;
            using (var db = new WorkbenchContext())
            {
                usr = (from records in db.Users
                       where records.IdUser == user.IdUser
                       select records).SingleOrDefault();
                usr.IsValidated = 1;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to update isvalidate null
        /// </summary>
        /// <param name="user">Get user detail</param>
        public void UpdateUserIsValidateNull(User user)
        {
            User usr = null;
            using (var db = new WorkbenchContext())
            {
                usr = (from records in db.Users
                       where records.IdUser == user.IdUser
                       select records).SingleOrDefault();
                usr.IsValidated = null;
                db.SaveChanges();

            }
        }

        public String GetShortName(Int32 userId)
        {

            User user = null;
            string shortName = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users where records.IdUser == userId select records).SingleOrDefault();
            }
            if (user != null)
                shortName = (user.FirstName.Substring(0, 1) + user.LastName.Substring(0, 1)).ToString().ToUpper();
            return shortName;
        }

        /// <summary>
        /// This method is to get list of all unread notifications related to user id
        /// </summary>
        /// <returns>list of all unread notifications related to user id</returns>
        public List<Notification> GetAllUnreadNotification(Int32 userId)
        {
            List<Notification> notifications = null;
            using (var db = new WorkbenchContext())
            {
                notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Status == "Unread" select records).ToList();
            }
            return notifications;
        }

        /// <summary>
        /// This method is to get list of all unread notifications related to user id (Async)
        /// </summary>
        /// <returns>list of all unread notifications related to user id</returns>
        public async Task<List<Notification>> GetAllUnreadNotificationAsync(Int32 userId)
        {
            return await Task.FromResult<List<Notification>>(GetAllUnreadNotification(userId));
        }

        /// <summary>
        /// This method is to get notification by userId and pagesize
        /// </summary>
        /// <param name="startIndex">Get startIndex of notification list</param>
        /// <param name="pageCount">Get page limit</param>
        /// <param name="userId">Get userId for notification</param>
        /// <returns>List of notification by userid and page count</returns>
        public List<Notification> GetNotification(Int32 startIndex, Int32 pageCount, Int32 userId)
        {
            List<Notification> notifications = null;
            using (var db = new WorkbenchContext())
            {
                notifications = (from records in db.Notifications where records.ToUser == userId orderby records.Id ascending select records).Skip(startIndex).Take(pageCount).ToList();
            }
            return notifications;
        }

        /// <summary>
        /// This method is to get notification by notification id(Async)
        /// </summary>
        /// <param name="userId">Get userId for notification</param>
        /// <param name="notificationId">Get notificationId for notification</param>
        /// <param name="startIndex">Get startIndex of notification list</param>
        /// <param name="isLessThan">Get isLessThan of notification Id</param>
        /// <param name="pageCount">Get page limit</param>
        /// <param name="orderField">Get order by with field name</param>
        /// <param name="orderFormat">Get order by with format asc or desc</param>
        /// <returns>List of notification by notification id</returns>
        public async Task<List<Notification>> GetNotificationWithRecordIdTestAsync(Int32 userId, long notificationId, bool isLessThan, Int32 startIndex, Int32 pageCount, string orderField, OrderBy orderFormat)
        {
            return await Task.FromResult<List<Notification>>(GetNotification(userId, notificationId, isLessThan, startIndex, pageCount, orderField, orderFormat));
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
        public List<Notification> GetNotification(Int32 userId, long notificationId, bool isLessThan, Int32 startIndex, Int32 pageCount, string orderField, OrderBy orderFormat)
        {
            List<Notification> notifications = null;
            using (var db = new WorkbenchContext())
            {

                if (isLessThan)
                {
                    if (orderField == "Id")
                    {
                        if (orderFormat == OrderBy.Ascending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id < notificationId && records.Status == "Read" orderby records.Id ascending select records).Skip(startIndex).Take(pageCount).ToList();
                        else if (orderFormat == OrderBy.Descending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id < notificationId && records.Status == "Read" orderby records.Id descending select records).Skip(startIndex).Take(pageCount).ToList();
                        else
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id < notificationId && records.Status == "Read" select records).Skip(startIndex).Take(pageCount).ToList();

                    }
                    else
                    {
                        if (orderFormat == OrderBy.Ascending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id < notificationId && records.Status == "Read" orderby records.Time ascending select records).Skip(startIndex).Take(pageCount).ToList();
                        else if (orderFormat == OrderBy.Descending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id < notificationId && records.Status == "Read" orderby records.Time descending select records).Skip(startIndex).Take(pageCount).ToList();
                        else
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id < notificationId && records.Status == "Read" select records).Skip(startIndex).Take(pageCount).ToList();
                    }
                }
                else
                {
                    if (orderField == "Id")
                    {
                        if (orderFormat == OrderBy.Ascending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id > notificationId && records.Status == "Read" orderby records.Id ascending select records).Skip(startIndex).Take(pageCount).ToList();
                        else if (orderFormat == OrderBy.Descending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id > notificationId && records.Status == "Read" orderby records.Id descending select records).Skip(startIndex).Take(pageCount).ToList();
                        else
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id > notificationId && records.Status == "Read" select records).Skip(startIndex).Take(pageCount).ToList();
                    }
                    else
                    {
                        if (orderFormat == OrderBy.Ascending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id > notificationId && records.Status == "Read" orderby records.Time ascending select records).Skip(startIndex).Take(pageCount).ToList();
                        else if (orderFormat == OrderBy.Descending)
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id > notificationId && records.Status == "Read" orderby records.Time descending select records).Skip(startIndex).Take(pageCount).ToList();
                        else
                            notifications = (from records in db.Notifications.Include("FromUserName") where records.ToUser == userId && records.Id > notificationId && records.Status == "Read" select records).Skip(startIndex).Take(pageCount).ToList();

                    }

                }

            }
            return notifications;
        }

        /// <summary>
        /// This method is to delete notification by id
        /// </summary>
        /// <param name="notificationId">Get notification id</param>
        public void DeleteNotificationById(Int32 notificationId)
        {
            Notification notification = null;
            using (var db = new MainServerWorkbenchContext())
            {
                notification = (from records in db.Notifications where records.Id == notificationId select records).SingleOrDefault();
                db.Notifications.Remove(notification);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to delete all notification
        /// </summary>
        public void DeleteAllNotification(Int32 userId)
        {
            using (var db = new MainServerWorkbenchContext())
            {
                db.Notifications.RemoveRange((from records in db.Notifications where records.ToUser == userId select records).ToList());
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method is to get list of Document types
        /// </summary>
        /// <returns>List of Document types</returns>
        public List<DocumentType> GetAllDocumentType()
        {
            List<DocumentType> documentTypes = null;
            using (var db = new WorkbenchContext())
            {
                documentTypes = (from records in db.DocumentTypes select records).ToList();
            }
            return documentTypes;
        }

        /// <summary>
        /// This method is to get total count of notification
        /// </summary>
        /// <returns>Total count of notification</returns>
        public Int32 GetAllNotificationCount(Int32 userId)
        {
            Int32 totalNotification;
            using (var db = new WorkbenchContext())
            {
                totalNotification = (from records in db.Notifications where records.ToUser == userId select records).Count();
            }
            return totalNotification;
        }


        /// <summary>
        /// This method is to get all uithemes
        /// </summary>
        /// <returns>List of all UIThemes</returns>
        public List<UITheme> GetAllThemes()
        {
            List<UITheme> uiThemes;
            using (var db = new WorkbenchContext())
            {
                uiThemes = (from records in db.UIThemes select records).ToList();
            }
            return uiThemes;
        }

        /// <summary>
        /// This method is to get all users
        /// </summary>
        /// <returns>List of user</returns>
        public IList<User> GetUsers()
        {
            IList<User> users = null;
            using (var db = new WorkbenchContext())
            {
                users = (from records in db.Users select records).ToList();
            }
            return users;
        }

        /// <summary>
        /// This method is to get geos version beta tester
        /// </summary>
        /// <param name="userId">Get user id</param>
        /// <param name="versionId">Get geos version id</param>
        /// <returns>Login user is Geos workbench version beta tester</returns>
        public bool IsGeosWorkbenchVersionBetaTester(int versionId, int userId)
        {
            bool isCheckBetaTester = false;
            using (var db = new WorkbenchContext())
            {
                isCheckBetaTester = (db.GeosVersionBetaTesters.Any(userRecord => userRecord.IdUser == userId && userRecord.IdGeosVersion == versionId));
            }
            return isCheckBetaTester;
        }

        /// <summary>
        /// This method is to update is new to 0
        /// </summary>
        /// <param name="notificationList">List of all unread notification list</param>
        public void UpdateListOfNotification(List<long> notificationids)
        {
            using (var db = new MainServerWorkbenchContext())
            {
                db.Notifications.Where(x => notificationids.Contains(x.Id)).ToList().ForEach(a => a.IsNew = 0);
                db.SaveChanges();
            }
        }


        public List<User> GetAllUser(string connectionString)
        {
            List<User> users = new List<User>();
            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("users_GetUsers", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User();
                        if (reader["IdUser"] != DBNull.Value)
                            user.IdUser = Convert.ToInt32(reader["IdUser"].ToString());
                        user.Login = reader["Login"].ToString();
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.CompanyEmail = reader["CompanyEmail"].ToString();
                        if (reader["IdUserGender"] != DBNull.Value)
                            user.IdUserGender = Convert.ToByte(reader["IdUserGender"].ToString());
                        if (reader["IdCompany"] != DBNull.Value)
                        {
                            user.IdCompany = Convert.ToInt32(reader["IdCompany"].ToString());
                            user.Company = new Company();
                            user.Company.IdCompany = Convert.ToInt32(reader["IdCompany"].ToString());
                            user.Company.Name = reader["SiteName"].ToString();
                            user.Company.ShortName = reader["ShortName"].ToString();
                            user.Company.Country = new Country();
                            if (reader["IdCountry"] != DBNull.Value)
                            {
                                user.Company.Country.IdCountry = Convert.ToByte(reader["IdCountry"].ToString());
                                user.Company.Country.Name = reader["Country"].ToString();
                            }
                            user.Company.Country.Zone = new Zone();
                            if (reader["IdZone"] != DBNull.Value)
                            {
                                user.Company.Country.Zone.IdZone = Convert.ToInt32(reader["IdZone"].ToString());
                                user.Company.Country.Zone.Name = reader["Zone"].ToString();
                            }
                        }

                        users.Add(user);
                    }
                }

            }
            return users;
        }


        public List<Permission> GetAllPermissions(string connectionString, Int32 idUser)
        {
            List<Permission> permissions = new List<Permission>();
            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                //MySqlCommand myCommand = new MySqlCommand("SELECT PER.IdPermission, PER.PermissionName, PER.IdGeosModule,ifnull(UP.IdUser,0) As IsUserPermission FROM permissions PER LEFT JOIN user_permissions UP ON PER.IdPermission = UP.IdPermission AND UP.IdUser = " + idUser + " group by PER.IdPermission;", myConnection);

                MySqlCommand myCommand = new MySqlCommand("permissions_GetAllUserPermission", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_IdUser", idUser);
                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Permission permission = new Permission();
                        if (reader["IdPermission"] != DBNull.Value)
                            permission.IdPermission = Convert.ToInt32(reader["IdPermission"].ToString());
                        permission.PermissionName = reader["PermissionName"].ToString();
                        if (reader["IdGeosModule"] != DBNull.Value)
                            permission.IdGeosModule = Convert.ToByte(reader["IdGeosModule"].ToString());
                        if (Convert.ToInt32(reader["IsUserPermission"].ToString()) == 0)
                            permission.IsUserPermission = false;
                        else
                            permission.IsUserPermission = true;
                        permissions.Add(permission);
                    }
                }

            }
            return permissions;
        }


        public bool AddUserPermissions(string connectionString, List<UserPermission> userPermissions)
        {
            bool isAdded = false;
            foreach (UserPermission permission in userPermissions)
            {
                if (permission.IsDeleted == true)
                {
                    using (MySqlConnection concompany = new MySqlConnection(connectionString))
                    {
                        concompany.Open();
                        using (MySqlTransaction trans = concompany.BeginTransaction())
                        {
                            try
                            {
                                //MySqlCommand concompanycommand = new MySqlCommand("DELETE FROM user_permissions WHERE IdUser = " + permission.IdUser + " AND IdPermission = " + permission.IdPermission + ";", concompany);
                                MySqlCommand concompanycommand = new MySqlCommand("userpermissions_Delete", concompany);
                                concompanycommand.CommandType = CommandType.StoredProcedure;
                                concompanycommand.Parameters.AddWithValue("_IdUser", permission.IdUser);
                                concompanycommand.Parameters.AddWithValue("_IdPermission", permission.IdPermission);
                                concompanycommand.ExecuteNonQuery();
                                isAdded = true;
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                isAdded = false;
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                }
                else
                {
                    using (MySqlConnection concompany = new MySqlConnection(connectionString))
                    {
                        concompany.Open();
                        using (MySqlTransaction trans = concompany.BeginTransaction())
                        {
                            try
                            {
                                MySqlCommand concompanycommand = new MySqlCommand("userpermissions_SetUserPermission", concompany);
                                concompanycommand.CommandType = CommandType.StoredProcedure;

                                concompanycommand.Parameters.AddWithValue("_IdUser", permission.IdUser);
                                concompanycommand.Parameters.AddWithValue("_IdPermission", permission.IdPermission);

                                permission.Id = Convert.ToInt32(concompanycommand.ExecuteScalar());
                                if (permission.Id > 0)
                                    isAdded = true;
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                isAdded = false;
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                }

            }
            return isAdded;
        }


        public bool AddUserSitePermissions(string connectionString, List<SiteUserPermission> siteUserPermissions)
        {
            bool isAdded = false;
            foreach (SiteUserPermission permission in siteUserPermissions)
            {
                if (permission.IsDeleted == true)
                {
                    using (MySqlConnection concompany = new MySqlConnection(connectionString))
                    {
                        concompany.Open();
                        using (MySqlTransaction trans = concompany.BeginTransaction())
                        {
                            try
                            {
                                //MySqlCommand concompanycommand = new MySqlCommand("DELETE FROM site_user_permission WHERE IdCompany = " + permission.IdCompany + " AND IdUser = " + permission.IdUser + ";", concompany);

                                MySqlCommand concompanycommand = new MySqlCommand("siteuserpermission_Delete", concompany);
                                concompanycommand.CommandType = CommandType.StoredProcedure;
                                concompanycommand.Parameters.AddWithValue("_IdCompany", permission.IdCompany);
                                concompanycommand.Parameters.AddWithValue("_IdUser", permission.IdUser);
                                concompanycommand.ExecuteNonQuery();
                                isAdded = true;
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                isAdded = false;
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                }
                else
                {
                    using (MySqlConnection concompany = new MySqlConnection(connectionString))
                    {
                        concompany.Open();
                        using (MySqlTransaction trans = concompany.BeginTransaction())
                        {
                            try
                            {
                                MySqlCommand concompanycommand = new MySqlCommand("siteuserpermission_SetUserSitePermission", concompany);
                                concompanycommand.CommandType = CommandType.StoredProcedure;

                                concompanycommand.Parameters.AddWithValue("_IdCompany", permission.IdCompany);
                                concompanycommand.Parameters.AddWithValue("_IdUser", permission.IdUser);

                                permission.Id = Convert.ToInt32(concompanycommand.ExecuteScalar());
                                if (permission.Id > 0)
                                    isAdded = true;
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                isAdded = false;
                                trans.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            return isAdded;
        }

        /// <summary>
        /// Sprint 24 [M024-09] Wrong permissions displayed in users section.
        /// This method is used to display user permission by iduser and by idGeosModule.
        /// </summary>
        /// <param name="connectionString">WorkbenchContext Connection String</param>
        /// <param name="idUser">The id of active user</param>
        /// <param name="idGeosModule">The id of geos module like CRM - 5</param>
        /// <returns>List of Permissions to user.</returns>
        public List<Permission> GetUserPermissionsByGeosModule(string connectionString, Int32 idUser, Byte idGeosModule)
        {
            List<Permission> permissions = new List<Permission>();
            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                //MySqlCommand myCommand = new MySqlCommand("SELECT PER.IdPermission, PER.PermissionName, PER.IdGeosModule,ifnull(UP.IdUser,0) As IsUserPermission FROM permissions PER LEFT JOIN user_permissions UP ON PER.IdPermission = UP.IdPermission AND UP.IdUser = " + idUser + " group by PER.IdPermission;", myConnection);

                MySqlCommand myCommand = new MySqlCommand("permissions_GetUserPermissionByGeosModule", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_IdUser", idUser);
                myCommand.Parameters.AddWithValue("_IdGeosModule", idGeosModule);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Permission permission = new Permission();

                        if (reader["IdPermission"] != DBNull.Value)
                            permission.IdPermission = Convert.ToInt32(reader["IdPermission"].ToString());

                        permission.PermissionName = reader["PermissionName"].ToString();

                        if (reader["IdGeosModule"] != DBNull.Value)
                            permission.IdGeosModule = Convert.ToByte(reader["IdGeosModule"].ToString());

                        if (Convert.ToInt32(reader["IsUserPermission"].ToString()) == 0)
                            permission.IsUserPermission = false;
                        else
                            permission.IsUserPermission = true;

                        permissions.Add(permission);
                    }
                }

            }
            return permissions;
        }

        /// <summary>
        /// Sprint 24 [M024-01] (#36880) Edit profile displaying always fpinas!
        /// This method is used to display user details by user Id
        /// </summary>
        /// <param name="workbenchConnectionString">The WorkbenchContext ConnectionString</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The User details</returns>
        public User GetUserProfileDetailsById(string workbenchConnectionString, int userId)
        {
            User user = new User();

            using (MySqlConnection myConnection = new MySqlConnection(workbenchConnectionString))
            {
                myConnection.Open();
                MySqlCommand connCommand = new MySqlCommand("users_GetUserProfileDetailsById", myConnection);
                connCommand.CommandType = CommandType.StoredProcedure;
                connCommand.Parameters.AddWithValue("_IdUser", userId);

                using (MySqlDataReader reader = connCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user.IdUser = Convert.ToInt32(reader["IdUser"].ToString());
                        if (reader["Login"] != DBNull.Value)
                            user.Login = reader["Login"].ToString();

                        if (reader["FirstName"] != DBNull.Value)
                            user.FirstName = reader["FirstName"].ToString();

                        if (reader["LastName"] != DBNull.Value)
                            user.LastName = reader["LastName"].ToString();

                        if (reader["IdUserGender"] != DBNull.Value)
                            user.IdUserGender = Convert.ToByte(reader["IdUserGender"].ToString());

                        if (reader["CompanyEmail"] != DBNull.Value)
                            user.CompanyEmail = reader["CompanyEmail"].ToString();

                        if (reader["Phone"] != DBNull.Value)
                            user.Phone = reader["Phone"].ToString();

                        if (reader["IdCompany"] != DBNull.Value)
                        {
                            user.IdCompany = Convert.ToInt32(reader["IdCompany"].ToString());

                            Company company = new Company();
                            company.IdCompany = Convert.ToInt32(reader["IdCompany"].ToString());
                            company.Name = reader["SiteName"].ToString();
                            company.ShortName = reader["ShortName"].ToString();

                            user.Company = company;
                            //company.Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["countryName"].ToString(), IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["zoneName"].ToString() } };
                        }
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// [CRM-M024-06] Automatic weekly report by email to sales users - Get all Automatic reports.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<AutomaticReport> GetAutomaticReports(string connectionString)
        {
            List<AutomaticReport> automaticReports = new List<AutomaticReport>();

            try
            {
                using (MySqlConnection connReports = new MySqlConnection(connectionString))
                {
                    connReports.Open();

                    MySqlCommand reportCommand = new MySqlCommand("automatic_reports_GetAutomaticReports", connReports);
                    reportCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reportReader = reportCommand.ExecuteReader())
                    {
                        while (reportReader.Read())
                        {
                            AutomaticReport automaticReport = new AutomaticReport();

                            automaticReport.IdAutomaticReport = Convert.ToInt16(reportReader["Id"].ToString());
                            automaticReport.Name = reportReader["Name"].ToString();
                            automaticReport.StartDate = Convert.ToDateTime(reportReader["StartDate"].ToString());
                            automaticReport.Interval = reportReader["Interval"].ToString();
                            automaticReport.IsEnabled = Convert.ToByte(reportReader["IsEnabled"]);

                            automaticReports.Add(automaticReport);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return automaticReports;
        }

        /// <summary>
        /// [CRM-M024-06] Automatic weekly report by email to sales users - Update automatic report start date.
        /// </summary>
        /// <param name="mainServerConnectionString"></param>
        /// <param name="automaticReport"></param>
        public void UpdateAutomaticReport(string mainServerConnectionString, AutomaticReport automaticReport)
        {
            TransactionScope transactionScope = null;

            using (transactionScope = new System.Transactions.TransactionScope())
            {
                try
                {
                    using (MySqlConnection connAutomaticReport = new MySqlConnection(mainServerConnectionString))
                    {
                        connAutomaticReport.Open();

                        MySqlCommand automaticReportCommand = new MySqlCommand("automatic_reports_UpdateAutomaticReport", connAutomaticReport);
                        automaticReportCommand.CommandType = CommandType.StoredProcedure;

                        automaticReportCommand.Parameters.AddWithValue("_IdAutomaticReport", automaticReport.IdAutomaticReport);
                        automaticReportCommand.Parameters.AddWithValue("_Name", automaticReport.Name);
                        automaticReportCommand.Parameters.AddWithValue("_StartDate", automaticReport.StartDate);
                        automaticReportCommand.Parameters.AddWithValue("_Interval", automaticReport.Interval);
                        automaticReportCommand.Parameters.AddWithValue("_IsEnabled", automaticReport.IsEnabled);

                        automaticReportCommand.ExecuteNonQuery();
                        connAutomaticReport.Close();

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
            }
        }


        /// <summary>
        /// Register user access in db with current version
        /// </summary>
        /// <param name="mainServerWorkbenchConnString"></param>
        /// <param name="userLoginEntry"></param>
        /// <returns></returns>
        public UserLoginEntry AddUserLoginEntry(string mainServerWorkbenchConnString, UserLoginEntry userLoginEntry)
        {
            TransactionScope transactionScope = null;

            using (transactionScope = new System.Transactions.TransactionScope())
            {
                try
                {
                    using (MySqlConnection connActivity = new MySqlConnection(mainServerWorkbenchConnString))
                    {
                        connActivity.Open();

                        MySqlCommand userLoginEntryCommand = new MySqlCommand("user_login_entries_Insert", connActivity);
                        userLoginEntryCommand.CommandType = CommandType.StoredProcedure;

                        userLoginEntryCommand.Parameters.AddWithValue("_IdUser", userLoginEntry.IdUser);
                        userLoginEntryCommand.Parameters.AddWithValue("_LoginTime", userLoginEntry.LoginTime);
                        userLoginEntryCommand.Parameters.AddWithValue("_LogoutTime", userLoginEntry.LogoutTime);
                        userLoginEntryCommand.Parameters.AddWithValue("_IpAddress", userLoginEntry.IpAddress);
                        userLoginEntryCommand.Parameters.AddWithValue("_IdCurrentGeosVersion", userLoginEntry.IdCurrentGeosVersion);

                        userLoginEntry.IdUserLoginEntry = Convert.ToInt32(userLoginEntryCommand.ExecuteScalar());

                        connActivity.Close();
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

                return userLoginEntry;
            }
        }

        /// <summary>
        /// Register user access in db with current version
        /// </summary>
        /// <param name="mainServerWorkbenchConnString"></param>
        /// <param name="userLoginEntry"></param>
        public void UpdateUserLoginEntry(string mainServerWorkbenchConnString, UserLoginEntry userLoginEntry)
        {
            TransactionScope transactionScope = null;

            using (transactionScope = new System.Transactions.TransactionScope())
            {
                try
                {
                    using (MySqlConnection connUserLoginEntry = new MySqlConnection(mainServerWorkbenchConnString))
                    {
                        connUserLoginEntry.Open();

                        MySqlCommand userLoginEntryCommand = new MySqlCommand("user_login_entries_Update", connUserLoginEntry);
                        userLoginEntryCommand.CommandType = CommandType.StoredProcedure;

                        userLoginEntryCommand.Parameters.AddWithValue("_IdUserLoginEntry", userLoginEntry.IdUserLoginEntry);
                        userLoginEntryCommand.Parameters.AddWithValue("_IdUser", userLoginEntry.IdUser);
                        userLoginEntryCommand.Parameters.AddWithValue("_LoginTime", userLoginEntry.LoginTime);
                        userLoginEntryCommand.Parameters.AddWithValue("_LogoutTime", userLoginEntry.LogoutTime);
                        userLoginEntryCommand.Parameters.AddWithValue("_IpAddress", userLoginEntry.IpAddress);
                        userLoginEntryCommand.Parameters.AddWithValue("_IdCurrentGeosVersion", userLoginEntry.IdCurrentGeosVersion);

                        userLoginEntryCommand.ExecuteNonQuery();
                        connUserLoginEntry.Close();

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
            }
        }

        public bool IsUserManager(Int32 idUser, string connectionString)
        {
            bool isLoginUserManager = false;
            try
            {
                using (MySqlConnection connReports = new MySqlConnection(connectionString))
                {
                    connReports.Open();

                    MySqlCommand reportCommand = new MySqlCommand("usermanager_IsUserManager", connReports);
                    reportCommand.CommandType = CommandType.StoredProcedure;
                    reportCommand.Parameters.AddWithValue("_idUser", idUser);
                    using (MySqlDataReader reportReader = reportCommand.ExecuteReader())
                    {
                        if (reportReader.Read())
                        {
                            isLoginUserManager = true;
                        }
                        else
                        {
                            isLoginUserManager = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                isLoginUserManager = false;
                throw;

            }

            return isLoginUserManager;
        }

        /// <summary>
        /// This method is to get list of all Geos Provider
        /// </summary>
        /// <returns>List of all GeosProvider</returns>
        public List<GeosProvider> GetGeosProviderList()
        {
            List<GeosProvider> geosProviderList = null;
            using (var db = new WorkbenchContext())
            {
                //geosProviderList = (from records in db.GeosProviders.Include("Company")
                //                    join comp in db.Companies on records.IdCompany equals comp.IdCompany
                //                    select records).ToList();

                geosProviderList = (from records in db.GeosProviders.Include("Company")
                                    select records).ToList();
            }

            return geosProviderList;
        }

        public List<GeosAppSetting> GetSelectedGeosAppSettings(string connectionString, string IdAppSettings)
        {
            List<GeosAppSetting> geosAppSettings = new List<GeosAppSetting>();

            using (MySqlConnection MyConnection = new MySqlConnection(connectionString))
            {
                MyConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("geosappsettings_GetSelectedGeosAppSettings", MyConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_IdAppSettings", IdAppSettings);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GeosAppSetting geosAppSetting = new GeosAppSetting();
                        geosAppSetting.IdAppSetting = Convert.ToInt16(reader["IdAppSetting"]);

                        if (reader["AppSettingName"] != DBNull.Value)
                            geosAppSetting.AppSettingName = Convert.ToString(reader["AppSettingName"]);

                        if (reader["IsUserModify"] != DBNull.Value)
                            geosAppSetting.IsUserModify = Convert.ToByte(reader["IsUserModify"]);

                        if (reader["DefaultValue"] != DBNull.Value)
                            geosAppSetting.DefaultValue = Convert.ToString(reader["DefaultValue"]);

                        geosAppSettings.Add(geosAppSetting);
                    }

                    reader.Close();
                }

                myCommand.Dispose();
                MyConnection.Close();
            }

            return geosAppSettings;
        }

        public List<Company> GetAllCompanyList()
        {
            List<Company> companyList = null;
            using (var db = new WorkbenchContext())
            {
                companyList = (from records in db.Companies.Include("Country")
                               where records.IsStillActive == 1

                               select records).ToList();
            }

            return companyList;
        }

        //public List<UserManagerDtl> GetManagerUsersHierarchy(Int32 idManager, string connectionString)
        //{
        //    List<UserManagerDtl> userManagers = new List<UserManagerDtl>();


        //    return userManagers;
        //}

        public void GetSalesUserByListofUserManager(string connectionString, string managerList, Int32? userId, DataTable dt)
        {
            DataTable dttemp = new DataTable();

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("GetSalesUserByManagerHierarchy", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdManager", managerList);
                mySqlCommand.Parameters.AddWithValue("_IdLoginUser", userId);

                MySqlDataAdapter da = new MySqlDataAdapter(mySqlCommand);
                da.Fill(dttemp);
            }

            if (dttemp == null || dttemp.Rows.Count == 0)
            {
                if (dt == null)
                {
                    dt = dttemp;
                }

                DataTableUserManager = new DataTable();
                DataTableUserManager = dt;

                return;
            }
            else
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = dttemp;
                    List<string> st = new List<string>();
                    foreach (DataRow item in dttemp.Rows)
                    {
                        st.Add(item[0].ToString());
                    }
                    managerList = string.Join(",", st);
                }
                else
                {
                    List<string> st = new List<string>();
                    foreach (DataRow item in dttemp.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = item[0];
                        dr[1] = item[1];
                        dr[2] = item[2];
                        dr[3] = item[3];
                        dr[4] = item[4];

                        dt.Rows.Add(dr);
                        st.Add(item[0].ToString());
                    }

                    managerList = string.Join(",", st);
                }

                GetSalesUserByListofUserManager(connectionString, managerList, null, dt);
            }
        }

        public List<DomainUser> GetUserDataFromTheActiveDirectory()
        {
            List<DomainUser> domainUsersData = new List<DomainUser>();

            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "emdep"))
            {
                using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (Principal result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                        DomainUser domainUser = new DomainUser();
                        domainUser.FirstName = Convert.ToString(de.Properties["givenName"].Value);
                        domainUser.LastName = Convert.ToString(de.Properties["sn"].Value);
                        domainUser.Email = Convert.ToString(de.Properties["mail"].Value);

                        //Console.WriteLine("SAM Account name : " + de.Properties["samAccountName"].Value);
                        //Console.WriteLine("User principle name: " + de.Properties["userPrincipleName"].Value);

                        domainUsersData.Add(domainUser);
                    }
                }
            }

            return domainUsersData.OrderBy(q => q.FirstName).ThenBy(r => r.LastName).ToList();
        }


        public GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting, string connectionString)
        {
            GeosAppSetting geosAppSetting = new GeosAppSetting();

            using (MySqlConnection MyConnection = new MySqlConnection(connectionString))
            {
                MyConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("geosappsettings_GetGeosAppSettings", MyConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_IdAppSetting", IdAppSetting);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        geosAppSetting.IdAppSetting = IdAppSetting;
                        geosAppSetting.DefaultValue = reader["DefaultValue"].ToString();
                        geosAppSetting.AppSettingName = reader["AppSettingName"].ToString();
                    }

                    reader.Close();
                }

                myCommand.Dispose();
                MyConnection.Close();
            }

            return geosAppSetting;
        }


        public string GetUserNameByCompanyCode(string companyCode, string connectionString)
        {
            string username = null;

            using (MySqlConnection MyConnection = new MySqlConnection(connectionString))
            {
                MyConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("GetUserNameByCompanyCode", MyConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_CompanyCode", companyCode);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        username = reader["UserName"].ToString();
                    }

                    reader.Close();
                }

                myCommand.Dispose();
                MyConnection.Close();
            }

            return username;
        }


        public User GetUserDetailsByEmailId(string userEmailId, string connectionString)
        {
            User user = null;
            using (var db = new WorkbenchContext())
            {
                user = (from records in db.Users.Include("UserPermissions.Permission") where records.CompanyEmail == userEmailId select records).FirstOrDefault();

                if (user != null)
                {
                    Company company = new Company();

                    if (user.IdCompany != null)
                    {
                        using (MySqlConnection myConnection = new MySqlConnection(connectionString))
                        {
                            myConnection.Open();
                            //MySqlCommand myCommand = new MySqlCommand("select s.IdSite, s.Name, s.IdCountry, cou.name as countryName, cou.IdZone, zones.Name as zoneName from sites s inner join countries cou on s.IdCountry = cou.idCountry inner join zones ON zones.IdZone = cou.IdZone where s.IdSite=" + user.IdCompany, myConnection);
                            MySqlCommand concommand = new MySqlCommand("sites_GetLoginUserSiteDetails", myConnection);
                            concommand.CommandType = CommandType.StoredProcedure;
                            concommand.Parameters.AddWithValue("_IdCompany", user.IdCompany);

                            using (MySqlDataReader reader = concommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                                    company.Name = reader["Name"].ToString();
                                    company.ShortName = reader["ShortName"].ToString();
                                    company.IdCountry = Convert.ToByte(reader["IdCountry"].ToString());
                                    company.Country = new Country { IdCountry = Convert.ToByte(reader["IdCountry"].ToString()), Name = reader["countryName"].ToString(), IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["zoneName"].ToString() } };
                                }
                            }
                        }
                    }

                    user.Company = company;
                }
            }

            return user;
        }

        public bool UpdateUserNotification_V2043(Int32 userId, string connectionString)
        {
            bool isUpdated = false;
            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand concommand = new MySqlCommand("Workbench_UpdateNotification", myConnection);
                concommand.CommandType = CommandType.StoredProcedure;
                concommand.Parameters.AddWithValue("_idUser", userId);
                concommand.ExecuteNonQuery();
                isUpdated = true;
            }
            return isUpdated;
        }

       

        public Int64 GetMaxNotificationId(Int32 userId)
        {
            Int64 notificationId = 0;
            using (var db = new WorkbenchContext())
            {
                notificationId = (from records in db.Notifications where records.ToUser == userId && records.Status == "Read" orderby records.Id descending select records.Id).FirstOrDefault();
            }
            return notificationId;
        }
    }
}
