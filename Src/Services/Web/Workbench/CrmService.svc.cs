using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.ServiceModel;
//using Prism.Logging;

namespace Emdep.Geos.Services.Web.Workbench
{
    /// <summary>
    /// CrmService class use for getting information of Crm Service
    /// </summary>
    public class CrmService : ICrmService
    {
        /// <summary>
        /// This method is to get all list of company having user permission
        /// </summary>
        /// <param name="idUser">Get User id</param>
        /// <returns>List of companies</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Company&gt; companies = crmControl.GetAllCompaniesDetails(666);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetAllCompaniesDetails(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }


        /// <summary>
        /// This method is to get all list of people details
        /// </summary>
        /// <param name="idSite">Get idSite</param>
        /// <returns>List of people details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;People&gt; peoples = crmControl.GetSiteContacts(idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<People> GetSiteContacts(Int64 idSite)
        {
            List<People> peoples = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSiteContacts(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }


        /// <summary>
        /// This method is to add new customer
        /// </summary>
        /// <param name="customer">Get customer details to add</param>
        /// <returns>Added customer details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          Customer customer = crmControl.AddCustomer(customer);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Customer AddCustomer(Customer customer)
        {
            Customer Customer = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                Customer = mgr.AddCustomer(connectionString, customer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Customer;
        }


        /// <summary>
        /// This method is to get plant sales quota by user
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <returns>List of plant business unit sales quota</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;PlantBusinessUnitSalesQuota&gt; plantBusinessUnitSalesQuotas = crmControl.GetPlantBusinessUnitSalesQuotaByUser(idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<PlantBusinessUnitSalesQuota> GetPlantBusinessUnitSalesQuotaByUser(Int32 idUser)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuotas = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuotas = mgr.GetPlantBusinessUnitSalesQuotaByUser(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuotas;
        }


        /// <summary>
        /// This method is to get plant sales quota by selected users
        /// </summary>
        /// <param name="idUser">Get selected users</param>
        /// <returns>List of plant business unit sales quota</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;PlantBusinessUnitSalesQuota&gt; plantBusinessUnitSalesQuotas = crmControl.GetPlantBusinessUnitSalesQuotaBySelectedUsers(idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<PlantBusinessUnitSalesQuota> GetPlantBusinessUnitSalesQuotaBySelectedUsers(string idUser)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuotas = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuotas = mgr.GetPlantBusinessUnitSalesQuotaBySelectedUsers(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuotas;
        }

        /// <summary>
        /// This method is to get total plant quota of all selected users
        /// </summary>
        /// <param name="assignedPlant">Get assignedPlant</param>
        /// <param name="idCurrency">Get idCurrency</param>
        /// <returns>plant business unit sales quota details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = crmControl.GetTotalPlantQuotaSelectedUserWise(assignedPlant,idCurrency);
        ///     }
        /// }
        /// </code>
        /// </example>
        public PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedUserWise(string assignedPlant, byte idCurrency)
        {
            PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetTotalPlantQuotaSelectedUserWise(connectionString, assignedPlant, idCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }


        /// <summary>
        /// This method is to get sales user details
        /// </summary>
        /// <param name="userIds">Get user ids</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <returns>Sales user details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          SalesUser SalesUserQuota = crmControl.GetTotalSalesQuotaBySelectedUserId(userIds,idCurrency);
        ///     }
        /// }
        /// </code>
        /// </example>
        public SalesUser GetTotalSalesQuotaBySelectedUserId(string userIds, byte idCurrency)
        {
            SalesUser plantBusinessUnitSalesQuota = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetTotalSalesQuotaBySelectedUserId(connectionString, userIds, idCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }

        /// <summary>
        /// This method is to get list of offer without purchase order
        /// </summary>
        /// <param name="idCurrency">Get idCurrency</param>
        /// <param name="idUser">Get idUser</param>
        /// <param name="idZone">Get idZone</param>
        /// <param name="accountingYear">Get accountingYear</param>
        /// <param name="companyDetails">Get company details from which have to connect</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer without purchase order</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          IList&lt;Offer&gt; offers = crmControl.GetOffersPipeline(12,666,2,2016,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetOffersPipeline(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOffersPipeline(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get list of offer without purchase order
        /// </summary>
        /// <param name="idCurrency">Get idCurrency</param>
        /// <param name="idUser">Get idUser</param>
        /// <param name="idZone">Get idZone</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="companyDetails">Get company details from which have to connect</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer without purchase order</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          IList&lt;Offer&gt; offers = crmControl.GetOffersPipeline(12,666,2,2016,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetOffersPipeline(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOffersPipeline(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get list of offer model appointment
        /// </summary>
        /// <param name="idCurrency">Get id currency</param>
        /// <param name="idUser">Get id user</param>
        /// <param name="idZone">Get id zone</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="compdetails">Get company details from which have to connect</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer model appointment</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          IList&lt;ModelAppointment&gt; modelAppointments = crmControl.GetOffersModelAppointment(12,666,2,2016,compdetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<ModelAppointment> GetOffersModelAppointment(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company compdetails, Int32 idUserPermission)
        {
            List<ModelAppointment> modelAppointments = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetOffersModelAppointment(idCurrency, idUser, idZone, accountingYear, compdetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }


        /// <summary>
        /// This method is to get list of offer model appointment
        /// </summary>
        /// <param name="idCurrency">Get id currency</param>
        /// <param name="idUser">Get id user</param>
        /// <param name="idZone">Get id zone</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="compdetails">Get company details from which have to connect</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer model appointment</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          IList&lt;ModelAppointment&gt; modelAppointments = crmControl.GetOffersModelAppointment(12,666,2,2016,compdetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<ModelAppointment> GetOffersModelAppointment(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission)
        {
            List<ModelAppointment> modelAppointments = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetOffersModelAppointment(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, compdetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }

        public List<TempAppointment> GetDailyEvents(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<TempAppointment> modelAppointments = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetDailyEvents(idCurrency, idUser, accountingYearFrom, accountingYearTo, compdetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }


        /// <summary>
        /// This method is to add log entry by offer
        /// </summary>
        /// <param name="logEntryByOffer">Get logEntryByOffer details to add</param>
        /// <param name="offerPlantConnectStr">Get offer plant connection string</param>
        /// <returns>Is inserted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          bool isInserted = crmControl.AddLogEntryByOffer(logEntryByOffer,offerPlantConnectStr);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool AddLogEntryByOffer(LogEntryByOffer logEntryByOffer, string offerPlantConnectStr)
        {
            bool isInserted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isInserted = mgr.AddLogEntryByOffer(logEntryByOffer, connectionString, offerPlantConnectStr);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isInserted;
        }

        /// <summary>
        /// This method is to add log entry by site
        /// </summary>
        /// <param name="logEntryBySite">Get logEntryBySite details to add</param>
        /// <returns>Is inserted or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          bool isInserted = crmControl.AddLogEntryBySite(logEntryBySite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool AddLogEntryBySite(LogEntryBySite logEntryBySite)
        {
            bool isInserted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isInserted = mgr.AddLogEntryBySite(logEntryBySite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isInserted;
        }

        /// <summary>
        /// This method is to get list of company details
        /// </summary>
        /// <param name="idCurrency">Get id currency</param>
        /// <param name="accountingFromYear">Get accounting from year</param>
        /// <param name="accountingToYear">Get accounting to year</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <returns>list of company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Company&gt; companies = crmControl.GetSelectedUsersSitesTarget(1,2006,2016,"666,28,3088");
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetSelectedUsersSitesTarget(byte idCurrency, Int64 accountingFromYear, Int64 accountingToYear, string idUser)
        {
            List<Company> companies = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUsersSitesTarget(connectionString, idCurrency, accountingFromYear, accountingToYear, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get list of company target details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingFromYear">Get accounting from year</param>
        /// <param name="accountingToYear">Get accounting to year</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>list of company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Company&gt; companies = crmControl.GetSitesTarget(idCurrency,idUser,idZone,accountingFromYear,accountingToYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetSitesTarget(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSitesTarget(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }



        /// <summary>
        /// This method is to get offer quantity
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>list of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Offer&gt; offers = crmControl.GetOfferQuantitySalesStatusByMonthCompanyWise(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferQuantitySalesStatusByMonthCompanyWise(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferQuantitySalesStatusByMonthCompanyWise(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get offer quantity
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <returns>list of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Offer&gt; offers = crmControl.GetOfferQuantitySalesStatusByMonthCompanyWise(idCurrency,accountingYear,idUser,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int64 accountingYear, string idUser, Company companyDetails)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(idCurrency, accountingYear, idUser, companyDetails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<Offer> GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(idCurrency, accountingYearFrom, accountingYearTo, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get offer contacts details
        /// </summary>
        /// <param name="idOffer">Get offer id</param>
        /// <param name="offerPlantConnectStr">Get offer plant connection string</param>
        /// <returns>List of offer contacts</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          IList&lt;OfferContact&gt; offerContacts = crmControl.GetOfferContact(idOffer,offerPlantConnectStr);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<OfferContact> GetOfferContact(Int64 idOffer, string offerPlantConnectStr)
        {
            List<OfferContact> offerContacts = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offerContacts = mgr.GetOfferContact(idOffer, connectionString, offerPlantConnectStr);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerContacts;
        }

        /// <summary>
        /// This method is to get competitors
        /// </summary>
        /// <returns>List of competitors</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Competitor&gt; competitors = crmControl.GetCompetitors();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Competitor> GetCompetitors()
        {
            List<Competitor> competitors = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                competitors = mgr.GetCompetitors(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return competitors;
        }

        /// <summary>
        /// This method is to get countries
        /// </summary>
        /// <returns>List of country</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Country&gt; countries = crmControl.GetCountries();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Country> GetCountries()
        {
            List<Country> countries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetCountries(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return countries;
        }


        /// <summary>
        /// This method is to add company
        /// </summary>
        /// <param name="company">Get company details</param> 
        /// <returns>Added company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          Company addedCompany = crmControl.AddCompany(company);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Company AddCompany(Company company, Company emdepSite = null)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.AddCompany(company, mainconnectionString, crmconnectionString, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        /// <summary>
        /// This method is to update company
        /// </summary>
        /// <returns>updated company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          Company updatedCompany = crmControl.UpdateCompany(company);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Company UpdateCompany(Company company)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        /// <summary>
        /// This method is to get lookup values
        /// </summary>
        /// <param name="key">Get key</param>
        /// <returns>List of Lookup values</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          IList&lt;LookupValue&gt; list = crmControl.GetLookupValues(2);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;
            try
            {
                CrmManager mgr = new CrmManager();
                list = mgr.GetLookupValues(key);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }

        /// <summary>
        /// This method is to get company group details
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of customer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Customer&gt; customers = crmControl.GetCompanyGroup(idUser,idZone,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Customer> GetCompanyGroup(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetCompanyGroup(connectionString, idUser, idZone, idUserPermission, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        /// <summary>
        /// This method is to get all offer options
        /// </summary>
        /// <returns>List of offer option</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;OfferOption&gt; offerOptions = crmControl.GetAllOfferOptions();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<OfferOption> GetAllOfferOptions()
        {
            List<OfferOption> offerOptions = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offerOptions = mgr.GetAllOfferOptions(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerOptions;
        }

        /// <summary>
        /// This method is to get sales owner by customer id
        /// </summary>
        /// <param name="idSite">Get site id</param>
        /// <returns>list of people</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;People&gt; peoples = crmControl.GetSalesOwnerByCustomerId(idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<People> GetSalesOwnerBySiteId(Int32 idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSalesOwnerBySiteId(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        /// <summary>
        /// This method is to get company plant details by customer id
        /// </summary>
        /// <param name="idCustomer">Get customer id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Company&gt; companies = crmControl.GetCompanyPlantByCustomerId(idCustomer,idUser,idZone,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetCompanyPlantByCustomerId(Int32 idCustomer, Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCompanyPlantByCustomerId(connectionString, idCustomer, idUser, idZone, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get list of currency conversion
        /// </summary>
        /// <returns>List of currency conversion</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///        ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///         IList&lt;CurrencyConversion&gt; currencyConversions = crmControl.GetCurrencyConversions();
        ///     }
        /// }
        /// </code>
        /// </example>

        public IList<CurrencyConversion> GetCurrencyConversions()
        {
            IList<CurrencyConversion> currencyConversions = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                currencyConversions = mgr.GetCurrencyConversions(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return currencyConversions;
        }

        /// <summary>
        /// This method is to get list of sales status type
        /// </summary>
        /// <returns>List of sales status type</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///        ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///         IList&lt;GetAllSalesStatusType&gt; salesStatusTypes = crmControl.GetAllSalesStatusType();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<SalesStatusType> GetAllSalesStatusType()
        {
            IList<SalesStatusType> salesStatusTypes = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesStatusTypes = mgr.GetAllSalesStatusType(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesStatusTypes;
        }

        /// <summary>
        /// This method is to get top offers
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="offerLimit">Get offer limit</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get id user permission</param>
        /// <returns>List of top ten offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetTopTenOffers(idCurrency,idUser,idZone,offerLimit,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetTopOffers(byte idCurrency, Int32 idUser, Int32 idZone, Int32 offerLimit, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTopOffers(connectionString, idCurrency, idUser, idZone, offerLimit, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get all emdep sites details 
        /// </summary>
        /// <returns>List of company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Company&gt; companies = control.GetEmdepSitesCompanies();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Company> GetEmdepSitesCompanies()
        {
            IList<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetEmdepSitesCompanies(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get people details
        /// </summary>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <returns>List of people</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;People&gt; peoples = control.GetSelectedUserContactsBySalesOwnerId(idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<People> GetSelectedUserContactsBySalesOwnerId(string idUser)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSelectedUserContactsBySalesOwnerId(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        /// <summary>
        /// This method is to get companies details
        /// </summary>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <returns>List of companies details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Company&gt; companies = control.GetSelectedUserContactsBySalesOwnerId(idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetSelectedUserCustomersBySalesOwnerId(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get customer details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get zone id</param>
        /// <param name="customerLimit">Get customer limit</param>
        /// <param name="companyDetails">Get company details </param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of companies details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Customer&gt; customers = control.GetDashboardDetailsByCustomerCompanywise(idCurrency,idUser,idZone,accountingYear,customerLimit,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Customer> GetDashboardDetailsByCustomerCompanywise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetDashboardDetailsByCustomerCompanywiseDatatable(idCurrency, idUser, idZone, accountingYear, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }


        /// <summary>
        /// This method is to get customer details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="customerLimit">Get customer limit</param>
        /// <param name="companyDetails">Get company details </param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of companies details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Customer&gt; customers = control.GetDashboardDetailsByCustomerCompanywise(idCurrency,idUser,idZone,accountingYear,customerLimit,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Customer> GetDashboardDetailsByCustomerCompanywise(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetDashboardDetailsByCustomerCompanywiseDatatable(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }


        /// <summary>
        /// This method is to get dashboard offer details of forecasted,qualified,quoted,sales,rfq
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get zone id</param>
        /// <param name="companyDetails">Get company details </param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>Offer details of forecasted,qualified,quoted,sales,rfq</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Offer offers = control.GetDashboardDetailsCompanyWise(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Offer GetDashboardDetailsCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            Offer offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetDashboardDetailsCompanyWise(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get dashboard offer pipeline details log entry wise
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get zone id</param>
        /// <param name="companydetails">Get company details </param>
        /// <returns>List of offer pipeline details log entry wise</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers= control.GetSelectedOffersPipelineLogentryWise(idCurrency,idUser,accountingYear,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSelectedOffersPipelineLogentryWise(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0)
        {
            IList<Offer> offers = new List<Offer>();

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedOffersPipelineLogentryWise(idCurrency, idUser, accountingYear, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get dashboard offer pipeline details log entry wise
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer pipeline details log entry wise</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers= control.GetOffersPipelineLogEntryWise(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetOffersPipelineLogEntryWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOffersPipelineLogEntryWise(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to customer target details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>List of offer having customer target details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers= control.GetSelectedUsersTargetByCustomer(idCurrency,idUser,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSelectedUsersTargetByCustomer(byte idCurrency, string idUser, Int64 accountingYear)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersTargetByCustomer(connectionString, idCurrency, idUser, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get customer target details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer having customer target details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Offer&gt; offers= control.GetTargetByCustomer(idCurrency,idUser,idZone,accountingYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetTargetByCustomer(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetByCustomer(connectionString, idCurrency, idUser, idZone, accountingYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get dashboard details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="companyDetails">Get company details</param>
        /// <returns>Offer details of forecasted,qualified,quoted,sales,rfq </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          Offer offers = control.GetSelectedUsersDashboardDetailsCompanyWise(idCurrency,accountingYear,idUser,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Offer GetSelectedUsersDashboardDetailsCompanyWise(byte idCurrency, Int64 accountingYear, string idUser, Company companyDetails)
        {
            Offer offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersDashboardDetailsCompanyWise(idCurrency, accountingYear, idUser, companyDetails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get offer lead source
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <returns>List of offer having lead source details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Offer&gt; offers = control.GetSelectedUsersOfferLeadSourceCompanyWise(idCurrency,idUser,accountingYear,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise(byte idCurrency, string idUser, Int64 accountingYear, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOfferLeadSourceCompanyWise(idCurrency, idUser, accountingYear, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOfferLeadSourceCompanyWise(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get offer with lead source details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer with lead source details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Offer&gt; offers = control.GetOfferLeadSourceCompanyWise(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetOfferLeadSourceCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferLeadSourceCompanyWise(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<Offer> GetOfferLeadSourceCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferLeadSourceCompanyWise(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get dashboard details by customer
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="customerLimit">Get customer limit</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="companyDetails">Get company details</param>
        /// <returns>List of customer with dashboard details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Customer&gt; customers = control.GetSelectedUsersDashboardDetailsByCustomerCompanywise(idCurrency,accountingYear,customerLimit,idUser,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Customer> GetSelectedUsersDashboardDetailsByCustomerCompanywise(byte idCurrency, Int64 accountingYear, Int32 customerLimit, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            IList<Customer> customers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetSelectedUsersDashboardDetailsByCustomerCompanywiseDatatable(idCurrency, accountingYear, customerLimit, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }



        /// <summary>
        /// This method is to get dashboard details by customer
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="customerLimit">Get customer limit</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="companyDetails">Get company details</param>
        /// <returns>List of customer with dashboard details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Customer&gt; customers = control.GetSelectedUsersDashboardDetailsByCustomerCompanywise(idCurrency,accountingYear,customerLimit,idUser,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Customer> GetSelectedUsersDashboardDetailsByCustomerCompanywise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            IList<Customer> customers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetSelectedUsersDashboardDetailsByCustomerCompanywiseDatatable(idCurrency, accountingYearFrom, accountingYearTo, customerLimit, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        /// <summary>
        /// This method is to get offer option list with list of offer details and list offer options  details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="compdetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>offer option list with list of offer details and list offer options details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetOffersWithoutPurchaseOrderReturnListDatatable(idCurrency,idUser,idZone,accountingYear,compdetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company compdetails, Int32 idUserPermission)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetOffersWithoutPurchaseOrderReturnListDatatable(idCurrency, idUser, idZone, accountingYear, compdetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }


        /// <summary>
        /// This method is to get offer option list with list of offer details and list offer options  details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="compdetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>offer option list with list of offer details and list offer options details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission)
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetOffersWithoutPurchaseOrderReturnListDatatable(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, compdetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }
        /// <summary>
        /// This method is to get sales target by plant quotas
        /// </summary>
        /// <param name="accountingFromYear">Get accounting from year</param>
        /// <param name="accountingToYear">Get accounting to year</param>
        /// <returns>List of company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Company&gt; companies = control.GetSalesTargetByPlantQuotas(accountingFromYear,accountingToYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetSalesTargetByPlantQuotas(Int64 accountingFromYear, Int64 accountingToYear)
        {
            List<Company> companies = new List<Company>();
            try
            {
                CrmManager mgr = new CrmManager();

                string mainserverconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                companies = mgr.GetSalesTargetByPlantQuotas(mainserverconnectionString, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
        /// <summary>
        /// This method is to get offer option list with list of offer details and list offer options  details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="compdetails">Get company details</param>
        /// <returns>offer option list with list of offer details and list offer options details </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(idCurrency,idUser,accountingYear,compdetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, string idUser, Int64 accountingYear, Company compdetails, Int32 idCurrentUser = 0)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(idCurrency, idUser, accountingYear, compdetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }


        /// <summary>
        /// This method is to get offer option list with list of offer details and list offer options  details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="compdetails">Get company details</param>
        /// <returns>offer option list with list of offer details and list offer options details </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(idCurrency,idUser,accountingYear,compdetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idCurrentUser = 0)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(idCurrency, idUser, accountingYearFrom, accountingYearTo, compdetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }




        /// <summary>
        /// This method is to get list of order and list of offer options details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>OffersOptionsList with list of order and list of offer options details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(idCurrency,idUser,accountingYear,compdetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetSelectedUsersOrdersDatatable(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetSelectedUsersOrdersDatatable(idCurrency, idUser, accountingYear, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }



        /// <summary>
        /// This method is to get list of order and list of offer options details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="AccountingYearFrom">Get accounting year From</param>
        /// <param name="AccountingYearTo">Get accounting year To</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>OffersOptionsList with list of order and list of offer options details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetSelectedUsersOrdersDatatable(idCurrency,  idUser,  accountingYearFrom,  accountingYearTo,  companydetails,  idCurrentUser = 0)
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetSelectedUsersOrdersDatatable(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetSelectedUsersOrdersDatatable(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }

        /// <summary>
        /// This method is to get list of order and list of offer options details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>OffersOptionsList with list of order and list of offer options details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetOrdersDatatable(idCurrency,idUser,idZone,accountingYear,compdetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetOrdersDatatable(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetOrdersDatatable(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }


        /// <summary>
        /// This method is to get list of order and list of offer options details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>OffersOptionsList with list of order and list of offer options details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         OffersOptionsList offersOptionsList = control.GetOrdersDatatable(idCurrency,idUser,idZone,accountingYear,compdetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public OffersOptionsList GetOrdersDatatable(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            OffersOptionsList offersOptionsList = null;

            try
            {
                CrmManager mgr = new CrmManager();

                offersOptionsList = mgr.GetOrdersDatatable(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offersOptionsList;
        }

        /// <summary>
        /// This method is to get offer lost reason details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="companyDetails">Get company details</param>
        /// <returns>List of lost reason by offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;LostReasonsByOffer&gt; lostReasonsByOffers = control.GetSelectedUsersOfferLostReasonsDetailsCompanyWise(idCurrency,accountingYear,idUser,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise(byte idCurrency, Int64 accountingYear, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<LostReasonsByOffer> lostReasonsByOffers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lostReasonsByOffers = mgr.GetSelectedUsersOfferLostReasonsDetailsCompanyWise(connectionString, idCurrency, accountingYear, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lostReasonsByOffers;
        }

        public List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<LostReasonsByOffer> lostReasonsByOffers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lostReasonsByOffers = mgr.GetSelectedUsersOfferLostReasonsDetailsCompanyWise(connectionString, idCurrency, accountingYearFrom, accountingYearTo, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lostReasonsByOffers;
        }

        /// <summary>
        /// This method is to get list of sales target by site
        /// </summary>
        /// <param name="idCurrency">Get idCurrency</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of sales target by site</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;SalesTargetBySite&gt; salesTargetBySites = control.GetSalesStatusTargetDashboard(12,666,2,2016);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<SalesTargetBySite> GetSalesStatusTargetDashboard(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 idUserPermission)
        {
            IList<SalesTargetBySite> salesTargetBySites = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesTargetBySites = mgr.GetSalesStatusTargetDashboard(connectionString, idCurrency, idUser, idZone, accountingYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesTargetBySites;
        }

        /// <summary>
        /// This method is to get list of offer
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSalesStatus(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSalesStatus(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatus(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }




        /// <summary>
        /// This method is to get list of offer
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSalesStatus(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSalesStatus(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatus(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get list of offer status details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of offer status details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSelectedUserSalesStatus(idCurrency,idUser,accountingYear,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSelectedUserSalesStatus(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSelectedUserSalesStatus(idCurrency, idUser, accountingYear, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get list of offer status details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of offer status details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSelectedUserSalesStatus(idCurrency,idUser,accountingYear,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSelectedUserSalesStatus(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSelectedUserSalesStatus(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }



        /// <summary>
        /// This method is to get list of offer status details by month
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of offer status details by month</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Offer&gt; offers = control.GetSelectedUsersSalesStatusByMonth(idCurrency,idUser,accountingYear,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSelectedUsersSalesStatusByMonth(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSelectedUsersSalesStatusByMonth(idCurrency, idUser, accountingYear, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        /// <summary>
        /// This method is to get list of offer status details by month
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of offer status details by month</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Offer&gt; offers = control.GetSelectedUsersSalesStatusByMonth(idCurrency,idUser,accountingYear,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSelectedUsersSalesStatusByMonth(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSelectedUsersSalesStatusByMonth(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get sales staus by week
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Offer&gt; offers = control.GetSalesStatusByWeek(idCurrency,idUser,idZone,accountingYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSalesStatusByWeek(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSalesStatusByWeek(connectionString, idCurrency, idUser, idZone, accountingYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get top offers 
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="offerLimit">Get offer limit</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="companyDetails">Get company details</param>
        /// <returns>List of top offer details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSelectedUsersTopOffers(idCurrency,offerLimit,accountingYear,idUser,companyDetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSelectedUsersTopOffers(byte idCurrency, Int32 offerLimit, Int64 accountingYear, string idUser, Company companyDetails)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersTopOffers(connectionString, idCurrency, offerLimit, accountingYear, idUser, companyDetails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        /// <summary>
        /// This method is to get sales staus by month
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSalesStatusByMonth(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSalesStatusByMonth(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatusByMonth(idCurrency, idUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }




        /// <summary>
        /// This method is to get sales staus by month
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSalesStatusByMonth(idCurrency,idUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetSalesStatusByMonth(byte idCurrency, Int32 idUser, Int32 idZone,
DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatusByMonth(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        /// <summary>
        /// This method is to get people contacts
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of people</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;People&gt; peopleContacts = control.GetPeopleContacts(idUser,idZone,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<People> GetPeopleContacts(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            IList<People> peopleContacts = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peopleContacts = mgr.GetPeopleContacts(connectionString, idUser, idZone, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peopleContacts;
        }

        /// <summary>
        /// This method to get list of people
        /// </summary>
        /// <param name="idSite">Get site id</param>
        /// <returns>List of people</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;People&gt; peopleContacts = control.GetContactsOfSiteByOfferId(idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<People> GetContactsOfSiteByOfferId(Int32 idSite)
        {
            IList<People> peopleContacts = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peopleContacts = mgr.GetContactsOfSiteByOfferId(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peopleContacts;
        }



        //public bool IsDeletedOfferContact(OfferContact offerContact)
        //{
        //    bool isDeleted = false;

        //    try
        //    {
        //        CrmManager mgr = new CrmManager();
        //        string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
        //        isDeleted = mgr.IsDeletedOfferContact(offerContact, connectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return isDeleted;
        //}

        /// <summary>
        /// This method is to get is set primary or not
        /// </summary>
        /// <param name="offerContact">Get offer contact details</param>
        /// <returns>is set primary or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isSetPrimary = control.IsSetPrimaryOfferContact(offerContact);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool IsSetPrimaryOfferContact(OfferContact offerContact)
        {
            bool isSetPrimary = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isSetPrimary = mgr.IsSetPrimaryOfferContact(offerContact, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isSetPrimary;
        }

        /// <summary>
        /// This method is to get site info
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of companies</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Company&gt; Companies = control.GetSiteInfo(idUser,idZone,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Company> GetSiteInfo(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            IList<Company> Companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Companies = mgr.GetSiteInfo(connectionString, idUser, idZone, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Companies;
        }

        /// <summary>
        /// This method is to get list of log entries related to id offer
        /// </summary>
        /// <param name="idOffer">Get id offer</param>
        /// <param name="offerConnectstr">Get offer plant connection string</param>
        /// <returns>List of log entry</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;LogEntryByOffer&gt; logEntries = control.GetAllLogEntriesByIdOffer(idOffer,offerConnectstr);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LogEntryByOffer> GetAllLogEntriesByIdOffer(Int64 idOffer, string offerConnectstr)
        {
            List<LogEntryByOffer> logEntries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntries = mgr.GetAllLogEntriesByIdOffer(idOffer, offerConnectstr);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntries;
        }


        /// <summary>
        /// This method is to get list of log entries related to id site
        /// </summary>
        /// <param name="idSite">Get id site</param>
        /// <returns>List of log entry</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;LogEntryBySite&gt; logEntries = control.GetAllLogEntriesByIdSite(idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LogEntryBySite> GetAllLogEntriesByIdSite(Int64 idSite)
        {
            List<LogEntryBySite> logEntries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntries = mgr.GetAllLogEntriesByIdSite(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntries;
        }

        /// <summary>
        /// This method is to get list of comments related by id offer
        /// </summary>
        /// <param name="idOffer">Get id offer</param>
        /// <param name="offerConnectstr">Get offer plant connection string</param>
        /// <returns>List of log entry</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;LogEntryByOffer&gt; logEntries = control.GetAllCommentsByIdOffer(idOffer,offerConnectstr);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LogEntryByOffer> GetAllCommentsByIdOffer(Int64 idOffer, string offerConnectstr)
        {
            List<LogEntryByOffer> logEntries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntries = mgr.GetAllCommentsByIdOffer(idOffer, offerConnectstr);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntries;
        }

        /// <summary>
        /// This method is to get list of sales target by site details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>List of sales target by site details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;SalesTargetBySite&gt; salesTargetBySites = control.GetSelectedUsersSalesStatusTargetDashboard(idCurrency,idUser,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<SalesTargetBySite> GetSelectedUsersSalesStatusTargetDashboard(byte idCurrency, string idUser, Int64 accountingYear)
        {
            IList<SalesTargetBySite> salesTargetBySites = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesTargetBySites = mgr.GetSelectedUsersSalesStatusTargetDashboard(connectionString, idCurrency, idUser, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesTargetBySites;
        }


        /// <summary>
        /// This method is to get list of offer model appointment details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="compdetails">Get company details</param>
        /// <returns>List of offer model appointment details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;ModelAppointment&gt; offers = control.GetSelectedUsersOffersModelAppointment(idCurrency,idUser,accountingYear,compdetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<ModelAppointment> GetSelectedUsersOffersModelAppointment(byte idCurrency, string idUser, Int64 accountingYear, Company compdetails, Int32 idCurrentUser = 0)
        {
            List<ModelAppointment> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOffersModelAppointment(idCurrency, idUser, accountingYear, compdetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }




        /// <summary>
        /// This method is to get list of offer model appointment details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="compdetails">Get company details</param>
        /// <returns>List of offer model appointment details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;ModelAppointment&gt; offers = control.GetSelectedUsersOffersModelAppointment(idCurrency,idUser,accountingYear,compdetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<ModelAppointment> GetSelectedUsersOffersModelAppointment(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idCurrentUser = 0)
        {
            List<ModelAppointment> modelAppointments = null;
            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetSelectedUsersOffersModelAppointment(idCurrency, idUser, accountingYearFrom, accountingYearTo, compdetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }


        /// <summary>
        /// This method is to get list of offer pipeline details 
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of offer pipeline details </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSelectedOffersPipeline(idCurrency,idUser,accountingYear,compdetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSelectedOffersPipeline(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSelectedOffersPipeline(idCurrency, idUser, accountingYear, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }



        /// <summary>
        /// This method is to get list of offer pipeline details 
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of offer pipeline details </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSelectedOffersPipeline(idCurrency,idUser,accountingYear,compdetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSelectedOffersPipeline(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSelectedOffersPipeline(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// This method is to get list of country
        /// </summary>
        /// <returns>List of country</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Country&gt; countries = control.GetAllCountries();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Country> GetAllCountries()
        {
            List<Country> countries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetAllCountries(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return countries;
        }


        /// <summary>
        /// This method is to get offer details related to offer id
        /// </summary>
        /// <param name="idOffer">Get offer id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="company">Get company</param>
        /// <returns>Get offer details related to offer id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Offer offer = control.GetOfferDetailsById(idOffer,idUser,idCurrency,accountingYear,company);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Offer GetOfferDetailsById(Int64 idOffer, Int32 idUser, byte idCurrency, Int64 accountingYear, Company company)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById(idOffer, idUser, idCurrency, accountingYear, company, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        /// <summary>
        /// This method is to get offer details related to offer id
        /// </summary>
        /// <param name="idOffer">Get offer id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="company">Get company</param>
        /// <returns>Get offer details related to offer id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Offer offer = control.GetOfferDetailsById(idOffer,idUser,idCurrency,accountingYear,company);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Offer GetOfferDetailsById(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, company, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        /// <summary>
        /// This method is to get list of offer lost reason details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idActiveUser">Get login user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="companyDetails">Get company details</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer lost reason details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;LostReasonsByOffer&gt; LstLostReasonsByOffer = control.GetOfferLostReasonsDetailsCompanyWise(idCurrency,idActiveUser,idZone,accountingYear,companyDetails,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise(byte idCurrency, Int32 idActiveUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission)
        {
            List<LostReasonsByOffer> LstLostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                LstLostReasonsByOffer = mgr.GetOfferLostReasonsDetailsCompanyWise(connectionString, idCurrency, idActiveUser, idZone, accountingYear, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstLostReasonsByOffer;
        }

        public List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<LostReasonsByOffer> LstLostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                LstLostReasonsByOffer = mgr.GetOfferLostReasonsDetailsCompanyWise(connectionString, idCurrency, idActiveUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstLostReasonsByOffer;
        }

        /// <summary>
        /// This method is to get list of company
        /// </summary>
        /// <returns>List of all company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Company&gt; companies = control.GetCompanies();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Company> GetCompanies()
        {
            IList<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCompanies(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get contacts details related by sales owner id
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of people details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;People&gt; peoples = control.GetContactsBySalesOwnerId(idUser,idZone,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<People> GetContactsBySalesOwnerId(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsBySalesOwnerId(connectionString, idUser, idZone, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        /// <summary>
        /// This method is to get customers details related to sales owner id
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of company</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Company&gt; companies = control.GetCustomersBySalesOwnerId(idUser,idZone,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetCustomersBySalesOwnerId(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId(connectionString, idUser, idZone, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to make offer code
        /// </summary>
        /// <param name="idOfferType">Get offer type id</param>
        /// <param name="idCustomer">Get customer id</param>
        /// <param name="connectPlantstr">Get connect plant string</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>Get offer code</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         string code =  control.MakeOfferCode(idOfferType,idCustomer,connectPlantstr,idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public string MakeOfferCode(byte? idOfferType, Int32 idCustomer, string connectPlantstr, Int32 idUser)
        {
            string code = "";
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                code = mgr.MakeOfferCode(idOfferType, idCustomer, connectionString, offerCodeConnectionString, connectPlantstr, idUser, mainServerConnectionString, UseSQLCounter);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return code;
        }


        /// <summary>
        /// This method is to get list of offer types
        /// </summary>
        /// <returns>List of offer types</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;OfferType&gt; offerTypes = control.GetOfferType();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<OfferType> GetOfferType()
        {

            IList<OfferType> offerTypes = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offerTypes = mgr.GetOfferType();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerTypes;
        }

        /// <summary>
        /// This method is to get next number of offer from counters
        /// </summary>
        /// <param name="idOfferType">Get offer type id</param>
        /// <param name="connectPlantstr">Get connect plant string</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>Get next number of offer from counters</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Int64 nextNumber = control.GetNextNumberOfOfferFromCounters(idOfferType,connectPlantstr,idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Int64 GetNextNumberOfOfferFromCounters(byte? idOfferType, string connectPlantstr, Int32 idUser)
        {
            Int64 nextNumber = 0;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainserverconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                nextNumber = mgr.GetNextNumberOfOfferFromCounters(idOfferType, connectionString, connectPlantstr, idUser, mainserverconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return nextNumber;
        }

        /// <summary>
        /// This method is to get next number of supplies from GCM
        /// </summary>
        /// <param name="idOfferType">Get type</param>
        /// <returns>Get next number of supplies from GCM</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Int64 nextNumber = control.GetNextNumberOfSuppliesFromGCM(idOfferType);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Int64 GetNextNumberOfSuppliesFromGCM(byte? idOfferType)
        {
            Int64 nextNumber = 0;
            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                nextNumber = mgr.GetNextNumberOfSuppliesFromGCM(idOfferType, offerCodeConnectionString, mainServerConnectionString, UseSQLCounter);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return nextNumber;
        }

        /// <summary>
        /// This method is to create offer folder structure 
        /// </summary>
        /// <param name="offer">Get offer details to create folder structure</param>
        /// <param name="isNewFolderForOffer">This is create new folder structure</param>
        /// <returns>IsCreated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isCreated = control.CreateFolderOffer(offer);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool CreateFolderOffer(Offer offer, bool? isNewFolderForOffer = null)
        {
            bool isCreated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                isCreated = mgr.CreateFolderOffer(offer, connectionString, offer.Site.ConnectPlantId, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, workbenchConnectionString, isNewFolderForOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isCreated;
        }

        /// <summary>
        /// This method is to get emdep site details by site id
        /// </summary>
        /// <param name="idSite">Get id site</param>
        /// <returns>Site details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         EmdepSite emdepSite = control.GetEmdepSiteById(idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public EmdepSite GetEmdepSiteById(Int32 idSite)
        {
            EmdepSite emdepSite = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                emdepSite = mgr.GetEmdepSiteById(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return emdepSite;
        }

        /// <summary>
        /// This method is to add offer
        /// </summary>
        /// <param name="offer">Get offer details</param>
        /// <param name="idSite">Get site id</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>Get offer id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///            ICrmService control = new CrmServiceController(ServiceUrl);
        ///            Offer offer = new Offer();
        ///            offer.Code = OfferCode;
        ///            offer.IdCustomer = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
        ///            offer.IdProject = 4;
        ///            offer.Number = OfferNumber;
        ///            offer.IdOfferType = OfferTypeList[SelectedIndexOfferType].IdOfferType;
        ///            offer.Description = Description;
        ///            offer.IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
        ///            offer.Currency = new Currency { IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency, Name = Currencies[SelectedIndexCurrency].Name
        /// };
        ///            offer.IdStatus = GeosStatusList[SelectedIndexStatus].IdOfferStatusType;
        ///            offer.Value = OfferAmount;
        ///            offer.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
        ///            offer.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
        ///            offer.IdBusinessUnit = Convert.ToByte(BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue);
        ///            offer.DeliveryDate = OfferCloseDate;
        ///            offer.ProbabilityOfSuccess = Convert.ToSByte(SelectedIndexConfidentialLevel.ToString());
        ///            offer.OfferExpectedDate = OfferCloseDate;
        ///            List&lt;LogEntryByOffer&gt; logEntryByOffers = new List&lt;LogEntryByOffer&gt;();
        ///            LogEntryByOffer logEntryByOffer = new LogEntryByOffer();
        ///            logEntryByOffer.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
        ///            logEntryByOffer.Comments = "Test";
        ///            logEntryByOffer.IdLogEntryType = 2;
        ///            logEntryByOffers.Add(logEntryByOffer);
        ///            offer.LogEntryByOffers = logEntryByOffers;
        ///            offer.Quotations = new System.Collections.ObjectModel.ObservableCollection&lt;Quotation&gt;(TemplateDetailList.Where(i => i.QuotQuantity != null).ToList());
        ///
        ///            OptionsByOfferList = new List&lt;OptionsByOffer&gt;();
        ///            foreach (var item in offer.Quotations)
        ///            {
        ///                OptionsByOffer optionsByOffer = new OptionsByOffer();
        ///                 optionsByOffer.IdOption = item.IdDetectionsTemplate;
        ///                optionsByOffer.Quantity = Convert.ToInt32(item.QuotQuantity);
        ///                OptionsByOfferList.Add(optionsByOffer);
        ///            }
        ///
        ///                 offer.OptionsByOffers = OptionsByOfferList;
        ///
        ///             Offer offerReturnValue = CrmStartUp.AddOffer(offer,idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Offer AddOffer(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer updatedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                updatedOffer = mgr.AddOffer(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedOffer;
        }

        /// <summary>
        /// This method is to get business unit status wise
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idPermission">Get permission id</param>
        /// <param name="company">Get company details</param>
        /// <returns>List of business unit details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;LookupValue&gt; lookupValues = control.GetBusinessUnitStatusWise(idCurrency,idUser,accountingYear,idPermission,company);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LookupValue> GetBusinessUnitStatusWise(byte idCurrency, Int32 idUser, Int64 accountingYear, Int32 idPermission, Company company)
        {

            List<LookupValue> lookupValues = null;

            try
            {
                CrmManager mgr = new CrmManager();

                lookupValues = mgr.GetBusinessUnitStatusWise(idCurrency, idUser, accountingYear, idPermission, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lookupValues;
        }


        /// <summary>
        /// This method is to get business unit status wise
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="idPermission">Get permission id</param>
        /// <param name="company">Get company details</param>
        /// <returns>List of business unit details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;LookupValue&gt; lookupValues = control.GetBusinessUnitStatusWise(idCurrency,idUser,accountingYear,idPermission,company);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LookupValue> GetBusinessUnitStatusWise(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo
, Int32 idPermission, Company company)
        {

            List<LookupValue> lookupValues = null;

            try
            {
                CrmManager mgr = new CrmManager();

                lookupValues = mgr.GetBusinessUnitStatusWise(idCurrency, idUser, accountingYearFrom, accountingYearTo, idPermission, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lookupValues;
        }


        /// <summary>
        /// This method is to get plant quota amount business unit wise
        /// </summary>
        /// <param name="assignedPlant">Get assigned plants</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>List of business unit details as per plant</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;LookupValue&gt; lookupValues = control.GetPlantQuotaAmountBUWise(assignedPlant,idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LookupValue> GetPlantQuotaAmountBUWise(string assignedPlant, byte idCurrency, Int64 accountingYear)
        {
            List<LookupValue> lookupValues = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lookupValues = mgr.GetPlantQuotaAmountBUWise(assignedPlant, idCurrency, accountingYear, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lookupValues;
        }


        /// <summary>
        /// This method is to get plant quota amount business unit wise
        /// </summary>
        /// <param name="assignedPlant">Get assigned plants</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <returns>List of business unit details as per plant</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;LookupValue&gt; lookupValues = control.GetPlantQuotaAmountBUWise(assignedPlant,idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<LookupValue> GetPlantQuotaAmountBUWise(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            List<LookupValue> lookupValues = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lookupValues = mgr.GetPlantQuotaAmountBUWise(assignedPlant, idCurrency, accountingYearFrom, accountingYearTo, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lookupValues;
        }
        /// <summary>
        /// This method is to update offer details
        /// </summary>
        /// <param name="offer">Get offer details to update</param>
        /// <param name="idSite">Get site id details</param>
        /// <param name="idUser">Get user id </param>
        /// <returns>is updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Offer offer = new Offer();
        ///         offer = CrmStartUp.GetOfferDetailsById(selectedLeadList[0].IdOffer);
        ///         offer.IdCustomer = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
        ///         offer.IdOfferType = OfferTypeList[SelectedIndexOfferType].IdOfferType;
        ///         offer.Number = SelectedLeadList[0].NumberOfOffers;
        ///         offer.Code = OfferCode;
        ///         offer.IdBusinessUnit = Convert.ToByte(BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue);
        ///         offer.IsUpdateLeadToOT = SelectedLeadList[0].IsUpdateLeadToOT;
        ///         offer.ProbabilityOfSuccess = Convert.ToSByte(SelectedIndexConfidentialLevel);
        ///         offer.OfferExpectedDate = SelectedLeadList[0].DeliveryDate;
        ///         offer.Description = Description;
        ///         offer.DeliveryDate = SelectedLeadList[0].DeliveryDate;
        ///         offer.IdStatus = GeosStatusList[SelectedIndexStatus].IdOfferStatusType;
        ///         offer.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
        ///         offer.Quotations = new System.Collections.ObjectModel.ObservableCollection&lt;Quotation&gt;(TemplateDetailList.Where(i => i.QuotQuantity != null).ToList());
        ///         offer.LostReasonsByOffer = SelectedLeadList[0].LostReasonsByOffer;
        ///         OptionsByOfferList = new List&lt;OptionsByOffer&gt;();
        ///         foreach (var item in offer.Quotations)
        ///         {
        ///           OptionsByOffer optionsByOffer = new OptionsByOffer();
        ///           optionsByOffer.IdOffer = selectedLeadList[0].IdOffer;
        ///           optionsByOffer.IdOption = item.IdDetectionsTemplate;
        ///           optionsByOffer.Quantity = Convert.ToInt32(item.QuotQuantity);
        ///            OptionsByOfferList.Add(optionsByOffer);
        ///         }
        ///        offer.OptionsByOffers = OptionsByOfferList;
        ///        bool isoffersave = CrmStartUp.UpdateOffer(offer, idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateOffer(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOffer(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        /// <summary>
        /// This method is to update offer for particular columns
        /// </summary>
        /// <param name="offer">Get offer details to update</param>
        /// <param name="idSite">Get site id</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>Is updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isUpdated = control.UpdateOfferForParticularColumn(offer,idSite,idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateOfferForParticularColumn(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }
        /// <summary>
        /// This method is to update offer status
        /// </summary>
        /// <param name="idOffer">Get offer id</param>
        /// <param name="idStatus">Get status id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="statusDate">Get status date</param>
        /// <param name="idSalesStatusType">Get sales status type id</param>
        /// <param name="isCodeUpdate">Get is code update or not</param>
        /// <param name="lostReasonsByOffer">Get lost reason by offer</param>
        /// <param name="connectPlantId">Get offer connect plant id</param>
        /// <param name="offer">Get offer details</param>
        /// <param name="idSite">Get site id</param>
        /// <returns>updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          bool isUpdated = control.UpdateOfferStatus(idOffer,idStatus,idUser,statusDate,idSalesStatusType,isCodeUpdate,lostReasonsByOffer,connectPlantId,offer,idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateOfferStatus(Int64 idOffer, Int64 idStatus, Int32 idUser, DateTime statusDate, Int64 idSalesStatusType, bool isCodeUpdate, LostReasonsByOffer lostReasonsByOffer, string connectPlantConstr, Offer offer = null, Int32 idSite = 0)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainserverConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                if (isCodeUpdate)
                    isUpdated = mgr.UpdateOfferStatus(idOffer, idStatus, idUser, statusDate, idSalesStatusType, isCodeUpdate, lostReasonsByOffer, connectionString, connectPlantConstr, mainserverConnectionString, offer, offerCodeConnectionString, idSite, UseSQLCounter);
                else
                    isUpdated = mgr.UpdateOfferStatus(idOffer, idStatus, idUser, statusDate, idSalesStatusType, isCodeUpdate, lostReasonsByOffer, connectionString, connectPlantConstr, mainserverConnectionString, offer, null, 0, UseSQLCounter);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to update contact
        /// </summary>
        /// <param name="people">Get people details to update</param>
        /// <returns>Is updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          bool isUpdated = control.UpdateContact(people);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateContact(People people)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateContact(connectionString, people);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to add contact
        /// </summary>
        /// <param name="people">Get people deatils to add contact</param>
        /// <returns>Added people details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          People addedPeople = control.AddContact(people);
        ///     }
        /// }
        /// </code>
        /// </example>
        public People AddContact(People people)
        {
            People addedPeople = new People();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                addedPeople = mgr.AddContact(connectionString, people);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedPeople;
        }



        /// <summary>
        /// This method is to add car project
        /// </summary>
        /// <param name="carProject">Get car project details</param>
        /// <returns>Added car project details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          CarProject addedCarProject = control.AddCarProject(carProject);
        ///     }
        /// }
        /// </code>
        /// </example>
        public CarProject AddCarProject(CarProject carProject)
        {
            CarProject addedCarProject = new CarProject();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                addedCarProject = mgr.AddCarProject(carProject, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCarProject;
        }

        /// <summary>
        /// This method is to get all car projects
        /// </summary>
        /// <param name="idCustomer">Get customer id</param>
        /// <returns>List of car project</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          List&lt;CarProject&gt; carProjects = control.GetCarProject(idCustomer);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<CarProject> GetCarProject(Int32 idCustomer)
        {
            List<CarProject> carProjects = new List<CarProject>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                carProjects = mgr.GetCarProject(idCustomer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return carProjects;
        }


        /// <summary>
        /// This method is to get car project exist or not
        /// </summary>
        /// <param name="Name">Get car project name</param>
        /// <returns>Is exist or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          bool isExist =  control.IsExistCarProject(Name);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool IsExistCarProject(string Name)
        {
            bool isExist = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isExist = mgr.IsExistCarProject(connectionString, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isExist;
        }

        /// <summary>
        /// This method is to update sales target by site
        /// </summary>
        /// <param name="salesTargetBySite">Get sale target by site details to update</param>
        /// <returns>Is updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isUpdated = control.UpdateSalesTargetBySite(salesTargetBySite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateSalesTargetBySite(SalesTargetBySite salesTargetBySite)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateSalesTargetBySite(salesTargetBySite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to update plant business unit sales quota
        /// </summary>
        /// <param name="plantBusinessUnitSalesQuota">Get plant business unit sales quota details to update</param>
        /// <returns>Isupdated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isUpdated = control.UpdatePlantBusinessUnitSalesQuota(plantBusinessUnitSalesQuota);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdatePlantBusinessUnitSalesQuota(PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdatePlantBusinessUnitSalesQuota(plantBusinessUnitSalesQuota, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        /// <summary>
        /// This method is to check customer is exist in customers table or not
        /// </summary>
        /// <param name="customerName">Get customer name</param>
        /// <returns>Is Exist or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isExist = control.IsExistCustomer(customerName);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool IsExistCustomer(string customerName)
        {
            bool isExist = false;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isExist = mgr.IsExistCustomer(connectionString, customerName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isExist;
        }


        /// <summary>
        /// This method is to get lost reasons by offer
        /// </summary>
        /// <param name="idOffer">Get offer id</param>
        /// <param name="offerConnectStr">Get offer connection string</param>
        /// <returns>Lost reasons by offer details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         LostReasonsByOffer lostReasonsByOffer = control.GetLostReasonsByOffer(idOffer,offerConnectStr);
        ///     }
        /// }
        /// </code>
        /// </example>
        public LostReasonsByOffer GetLostReasonsByOffer(Int64 idOffer, string offerConnectStr)
        {
            LostReasonsByOffer lostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lostReasonsByOffer = mgr.GetLostReasonsByOffer(idOffer, offerConnectStr);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lostReasonsByOffer;
        }

        /// <summary>
        /// This method is to get current plant id
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <returns>Company details of current plant id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Company site = control.GetCurrentPlantId(idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Company GetCurrentPlantId(Int32 idUser)
        {
            Company site = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                site = mgr.GetCurrentPlantId(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return site;
        }


        /// <summary>
        /// This method is to get plant sales quota year and user wise
        /// </summary>
        /// <param name="idUser">Get user id</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>List of plant business unit sales quota details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;PlantBusinessUnitSalesQuota&gt; plantBusinessUnitSalesQuota = control.GetPlantSalesQuotaWithYearByIdUser(idUser,idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUser(Int32 idUser, byte idCurrency, Int32 accountingYear)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetPlantSalesQuotaWithYearByIdUser(connectionString, idUser, idCurrency, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }



        /// <summary>
        /// This method is to get all shipment details by offer id
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="idOffer">Get offer id</param>
        /// <returns>List of shipment</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Shipment&gt; shipments = control.GetAllShipmentsByOfferId(company,idOffer);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Shipment> GetAllShipmentsByOfferId(Company company, Int64 idOffer)
        {
            List<Shipment> shipments = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                shipments = mgr.GetAllShipmentsByOfferId(company, idOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return shipments;
        }


        /// <summary>
        /// This method is to get all packing boxes details by shipment id
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="idShipment">Get shipment id</param>
        /// <returns>List of packing box</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;PackingBox&gt; packingBoxes = control.GetAllPackingBoxesByShipmentId(company,idShipment);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<PackingBox> GetAllPackingBoxesByShipmentId(Company company, Int64 idShipment)
        {
            List<PackingBox> packingBoxes = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                packingBoxes = mgr.GetAllPackingBoxesByShipmentId(company, idShipment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return packingBoxes;
        }

        /// <summary>
        /// This method is to get list of currency
        /// </summary>
        /// <returns>List of currency</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Currency&gt; currencies = control.GetCurrencies();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Currency> GetCurrencies()
        {
            IList<Currency> currencies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                currencies = mgr.GetCurrencies();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return currencies;
        }


        /// <summary>
        /// This method is to get currencies with exchange rates
        /// </summary>
        /// <returns>List of currency details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Currency&gt; currencies = control.GetCurrencyByExchangeRate();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Currency> GetCurrencyByExchangeRate()
        {
            IList<Currency> currencies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                currencies = mgr.GetCurrencyByExchangeRate(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return currencies;
        }


        /// <summary>
        /// This method is to get offer max value
        /// </summary>
        /// <returns>Offer max value</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Double max_value = control.GetOfferMaxValue();
        ///     }
        /// }
        /// </code>
        /// </example>
        public Double GetOfferMaxValue()
        {
            Double max_value = 0;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                max_value = mgr.GetOfferMaxValue(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return max_value;
        }

        /// <summary>
        /// This method is to get list of Project details
        /// </summary>
        /// <param name="idCustomer">Get customer id</param>
        /// <returns>List of Projects</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;GeosProject&gt; geosProjects = control.GetProjectByCustomerId(idCustomer);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GeosProject> GetProjectByCustomerId(Int64 idCustomer)
        {
            List<GeosProject> geosProjects = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                geosProjects = mgr.GetProjectByCustomerId(connectionString, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosProjects;
        }

        /// <summary>
        /// This method is to get list of caroem
        /// </summary>
        /// <returns>List of caroem</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;CarOEM&gt; carOEMs = control.GetCarOEM();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<CarOEM> GetCarOEM()
        {
            List<CarOEM> carOEMs = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                carOEMs = mgr.GetCarOEM(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return carOEMs;
        }

        /// <summary>
        /// This method is to get list of templates
        /// </summary>
        /// <returns>List of templates</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Template&gt; currencies = control.GetTemplates();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Template> GetTemplates()
        {
            IList<Template> templates = null;

            try
            {
                CrmManager mgr = new CrmManager();
                templates = mgr.GetTemplates();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return templates;
        }

        public List<Template> GetTemplatesByIdTemplateType(Int64 idTemplateType)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetTemplatesByIdTemplateType(connectionString, idTemplateType);
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
        /// This method is to get target forecast details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingFromYear">Get accounting from year</param>
        /// <param name="accountingToYear">Get accounting to year</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>List of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetTargetForecast(idCurrency,idUser,idZone,accountingFromYear,accountingToYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetTargetForecast(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecast(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        /// <summary>
        /// This method is to get top car project offers
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="projectLimit">Get project limit</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of car projects</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;CarProject&gt; carProjects = control.GetTopCarProjectOffers(idCurrency,idUser,accountingYear,projectLimit,idUserPermission,companydetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<CarProject> GetTopCarProjectOffers(byte idCurrency, string idUser, Int64 accountingYear, Int32 projectLimit, Int32 idUserPermission, Company companydetails, Int32 idCurrentUser = 0)
        {
            List<CarProject> carProjects = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                carProjects = mgr.GetTopCarProjectOffers(idCurrency, idUser, accountingYear, projectLimit, idUserPermission, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return carProjects;
        }

        /// <summary>
        /// This method is to get top car project offers
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="projectLimit">Get project limit</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <param name="companydetails">Get company details</param>
        /// <returns>List of car projects</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;CarProject&gt; carProjects = control.GetTopCarProjectOffers(idCurrency,idUser,accountingYear,projectLimit,idUserPermission,companydetails);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<CarProject> GetTopCarProjectOffers(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 projectLimit, Int32 idUserPermission, Company companydetails, Int32 idCurrentUser = 0)
        {
            List<CarProject> carProjects = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                carProjects = mgr.GetTopCarProjectOffers(idCurrency, idUser, accountingYearFrom, accountingYearTo, projectLimit, idUserPermission, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return carProjects;
        }

        /// <summary>
        /// This method is to get sales user details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>List of sales user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;SalesUser&gt; salesUsers = control.GetAllSalesUserPeopleDetails(idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<SalesUser> GetAllSalesUserPeopleDetails(byte idCurrency, Int32 accountingYear)
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserPeopleDetails(idCurrency, connectionString, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }



        /// <summary>
        /// This method is to get total sales quota by selected user id and year
        /// </summary>
        /// <param name="userIds">Get user ids</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>Sales user quota details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         SalesUserQuota salesUserQuota = control.GetTotalSalesQuotaBySelectedUserIdAndYear(userIds,idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYear(string userIds, byte idCurrency, Int32 accountingYear)
        {
            SalesUserQuota salesUserQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuota = mgr.GetTotalSalesQuotaBySelectedUserIdAndYear(connectionString, userIds, idCurrency, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuota;
        }



        /// <summary>
        /// This method is to get total sales quota by selected user id and year
        /// </summary>
        /// <param name="userIds">Get user ids</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <returns>Sales user quota details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         SalesUserQuota salesUserQuota = control.GetTotalSalesQuotaBySelectedUserIdAndYear(userIds,idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYear(string userIds, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            SalesUserQuota salesUserQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuota = mgr.GetTotalSalesQuotaBySelectedUserIdAndYear(connectionString, userIds, idCurrency, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuota;
        }


        /// <summary>
        /// This method is to get total plant quota of selected plants year wise
        /// </summary>
        /// <param name="assignedPlant">Get assigned plants</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>Get plant business unit sales quota details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = control.GetTotalPlantQuotaSelectedPlantWiseAndYear(assignedPlant,idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedPlantWiseAndYear(string assignedPlant, byte idCurrency, Int32 accountingYear, Int32 idCurrentUser = 0)
        {
            PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetTotalPlantQuotaSelectedPlantWiseAndYear(connectionString, assignedPlant, idCurrency, accountingYear, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }


        /// <summary>
        /// This method is to get total plant quota of selected plants year wise
        /// </summary>
        /// <param name="assignedPlant">Get assigned plants</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <returns>Get plant business unit sales quota details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = control.GetTotalPlantQuotaSelectedPlantWiseAndYear(assignedPlant,idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedPlantWiseAndYear(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser = 0)
        {
            PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetTotalPlantQuotaSelectedPlantWiseAndYear(connectionString, assignedPlant, idCurrency, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }





        /// <summary>
        /// This method is to update plant business unit sales quota with year
        /// </summary>
        /// <param name="plantBusinessUnitSalesQuota">Get plant business unit sales quota details to update</param>
        /// <returns>Isupdated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isUpdatedPlantquota = control.UpdatePlantBusinessUnitSalesQuotaWithYear(plantBusinessUnitSalesQuota);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdatePlantBusinessUnitSalesQuotaWithYear(PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota)
        {
            bool isUpdatedPlantquota = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdatedPlantquota = mgr.UpdatePlantBusinessUnitSalesQuotaWithYear(plantBusinessUnitSalesQuota, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdatedPlantquota;
        }


        /// <summary>
        /// This method is to add sales user
        /// </summary>
        /// <param name="salesUser">Get sales user details to add</param>
        /// <returns>IsAdded or not </returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isAddedSalesUsers = control.AddSaleUser(salesUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool AddSaleUser(SalesUser salesUser)
        {
            bool isAddedSalesUsers = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isAddedSalesUsers = mgr.AddSaleUser(salesUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isAddedSalesUsers;
        }

        /// <summary>
        /// This method is to get sales user details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>List of sales user quota</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;SalesUserQuota&gt; salesUsers = control.GetAllSalesUserDetails(idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<SalesUserQuota> GetAllSalesUserDetails(byte idCurrency, Int32 accountingYear)
        {
            List<SalesUserQuota> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserDetails(connectionString, idCurrency, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }

        /// <summary>
        /// This method is to get won value by sales user id
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>Get won value by sales user id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         double amount = control.GetWonValueByIdSaleUser(company,idCurrency,idUser,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public double GetWonValueByIdSaleUser(Company company, byte idCurrency, Int32 idUser, Int64 accountingYear)
        {
            double amount = 0.00;

            try
            {
                CrmManager mgr = new CrmManager();
                amount = mgr.GetWonValueByIdSaleUser(company, idCurrency, idUser, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return amount;
        }



        /// <summary>
        /// This method is to get won value by sales user id
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <returns>Get won value by sales user id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         double amount = control.GetWonValueByIdSaleUser(company,idCurrency,idUser,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public double GetWonValueByIdSaleUser(Company company, byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            double amount = 0.00;

            try
            {
                CrmManager mgr = new CrmManager();
                amount = mgr.GetWonValueByIdSaleUser(company, idCurrency, idUser, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return amount;
        }

        /// <summary>
        /// This method is to get target forecast details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <param name="accountingFromYear">Get accounting from year</param>
        /// <param name="accountingToYear">Get accounting to year</param>
        /// <returns>List of all offer details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetSelectedUsersTargetForecast(idCurrency,idUser,idZone,accountingFromYear,accountingToYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetSelectedUsersTargetForecast(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersTargetForecast(connectionString, idCurrency, idUser, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        /// <summary>
        /// This method is to get offer lost reason
        /// </summary>
        /// <returns>List of offer lost reason</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;OfferLostReason&gt; offerLostReasons = control.GetOfferLostReason();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<OfferLostReason> GetOfferLostReason()
        {
            List<OfferLostReason> offerLostReasons = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offerLostReasons = mgr.GetOfferLostReason();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerLostReasons;
        }


        /// <summary>
        /// This method is to update list of currency conversion
        /// </summary>
        /// <param name="currencyConversions">Get list of currency conversion to update</param>
        /// <returns>Is updated or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         bool isUpdated = control.UpdateCurrencyConversion(currencyConversions);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool UpdateCurrencyConversion(List<CurrencyConversion> currencyConversions)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                isUpdated = mgr.UpdateCurrencyConversion(currencyConversions);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
                isUpdated = false;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to get company details by id
        /// </summary>
        /// <param name="idSite">Get site id</param>
        /// <returns>Company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         Company company = control.GetCompanyDetailsById(idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Company GetCompanyDetailsById(Int32 idSite)
        {
            Company company = new Company();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                company = mgr.GetCompanyDetailsById(connectionString, idSite);
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
        /// This method is to get GeosStatus
        /// </summary>
        /// <returns>List of GeosStatus</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;GeosStatus&gt; geosStatus = control.GetGeosOfferStatus();
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<GeosStatus> GetGeosOfferStatus()
        {
            IList<GeosStatus> geosStatus = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                geosStatus = mgr.GetGeosOfferStatus(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return geosStatus;
        }

        /// <summary>
        /// This method is to get company plant by customer id
        /// </summary>
        /// <param name="idCustomer">Get customer id</param>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <returns>List od company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Company&gt; companies = control.GetSelectedUserCompanyPlantByCustomerId(idCustomer,idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetSelectedUserCompanyPlantByCustomerId(Int32 idCustomer, string idUser)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCompanyPlantByCustomerId(connectionString, idCustomer, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get company group details
        /// </summary>
        /// <param name="idUser">Get all user ids separeted by , having active user rights to view this users offers</param>
        /// <returns>List of customer details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Customer&gt; customers = control.GetSelectedUserCompanyGroup(idUser);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Customer> GetSelectedUserCompanyGroup(string idUser, bool isIncludeDefault = false)
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetSelectedUserCompanyGroup(connectionString, idUser, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        /// <summary>
        /// This method is to get list of people details
        /// </summary>
        /// <returns>List of people details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;People&gt; peoples = control.GetPeoples();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<People> GetPeoples()
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetPeoples(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        /// <summary>
        /// This method is to all companies
        /// </summary>
        /// <returns>List of all companies</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Company&gt; companies = control.GetAllCompanies();
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetAllCompanies(Int32 idUser)
        {
            List<Company> companies = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetAllCompanies(connectionString, idUser);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public List<EmdepSite> GetAllEmdepSites(Int32 idUser)
        {
            List<EmdepSite> emdepSites = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                emdepSites = mgr.GetAllEmdepSites(connectionString, idUser);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return emdepSites;
        }

        public SalesUser GetUserAllSalesQuota(Int32 idUser, byte idCurrency)
        {
            SalesUser salesUser = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUser = mgr.GetUserAllSalesQuota(connectionString, idUser, idCurrency);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUser;
        }


        public bool AddSaleUserQuota(List<SalesUserQuota> salesUserQuotas)
        {
            bool isAdded = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isAdded = mgr.AddSaleUserQuota(salesUserQuotas, connectionString);
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


        public List<SalesUserQuota> GetAllSalesUserDetailsPlantWise(byte idCurrency, Int32 accountingYear, string assignedPlants)
        {
            List<SalesUserQuota> salesUserQuotas = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuotas = mgr.GetAllSalesUserDetailsPlantWise(connectionString, idCurrency, accountingYear, assignedPlants);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuotas;
        }

        public List<SalesUserQuota> GetAllSalesUserDetailsPlantWise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string assignedPlants)
        {
            List<SalesUserQuota> salesUserQuotas = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuotas = mgr.GetAllSalesUserDetailsPlantWise(connectionString, idCurrency, accountingYearFrom, accountingYearTo, assignedPlants);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuotas;
        }


        public SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYearPlantWise(string userIds, byte idCurrency, Int32 accountingYear, string assignedPlants)
        {
            SalesUserQuota salesUserQuota = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuota = mgr.GetTotalSalesQuotaBySelectedUserIdAndYearPlantWise(connectionString, userIds, idCurrency, accountingYear, assignedPlants);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuota;
        }


        public bool UpdateSaleUser(SalesUser salesUser)
        {
            bool isUpdatedSalesUsers = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdatedSalesUsers = mgr.UpdateSaleUser(salesUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdatedSalesUsers;
        }


        public bool UpdateOrderForParticularColumn(Offer offer)
        {

            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderForParticularColumn(offer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public CarProject AddCarProjectWithCreatedBy(CarProject carProject)
        {
            CarProject addedCarProject = new CarProject();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                addedCarProject = mgr.AddCarProjectWithCreatedBy(carProject, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCarProject;
        }


        /// <summary>
        /// This method is to add offer
        /// </summary>
        /// <param name="offer">Get offer details</param>
        /// <param name="idSite">Get site id</param>
        /// <param name="idUser">Get user id</param>
        /// <returns>Get offer id</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///            ICrmService control = new CrmServiceController(ServiceUrl);
        ///            Offer offer = new Offer();
        ///            offer.Code = OfferCode;
        ///            offer.IdCustomer = CompanyPlantList[SelectedIndexCompanyPlant].IdCompany;
        ///            offer.IdProject = 4;
        ///            offer.Number = OfferNumber;
        ///            offer.IdOfferType = OfferTypeList[SelectedIndexOfferType].IdOfferType;
        ///            offer.Description = Description;
        ///            offer.IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency;
        ///            offer.Currency = new Currency { IdCurrency = Currencies[SelectedIndexCurrency].IdCurrency, Name = Currencies[SelectedIndexCurrency].Name
        /// };
        ///            offer.IdStatus = GeosStatusList[SelectedIndexStatus].IdOfferStatusType;
        ///            offer.Value = OfferAmount;
        ///            offer.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
        ///            offer.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
        ///            offer.IdBusinessUnit = Convert.ToByte(BusinessUnitList[SelectedIndexBusinessUnit].IdLookupValue);
        ///            offer.DeliveryDate = OfferCloseDate;
        ///            offer.ProbabilityOfSuccess = Convert.ToSByte(SelectedIndexConfidentialLevel.ToString());
        ///            offer.OfferExpectedDate = OfferCloseDate;
        ///            List&lt;LogEntryByOffer&gt; logEntryByOffers = new List&lt;LogEntryByOffer&gt;();
        ///            LogEntryByOffer logEntryByOffer = new LogEntryByOffer();
        ///            logEntryByOffer.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
        ///            logEntryByOffer.Comments = "Test";
        ///            logEntryByOffer.IdLogEntryType = 2;
        ///            logEntryByOffers.Add(logEntryByOffer);
        ///            offer.LogEntryByOffers = logEntryByOffers;
        ///            offer.Quotations = new System.Collections.ObjectModel.ObservableCollection&lt;Quotation&gt;(TemplateDetailList.Where(i => i.QuotQuantity != null).ToList());
        ///
        ///            OptionsByOfferList = new List&lt;OptionsByOffer&gt;();
        ///            foreach (var item in offer.Quotations)
        ///            {
        ///                OptionsByOffer optionsByOffer = new OptionsByOffer();
        ///                 optionsByOffer.IdOption = item.IdDetectionsTemplate;
        ///                optionsByOffer.Quantity = Convert.ToInt32(item.QuotQuantity);
        ///                OptionsByOfferList.Add(optionsByOffer);
        ///            }
        ///
        ///                 offer.OptionsByOffers = OptionsByOfferList;
        ///
        ///             Offer offerReturnValue = CrmStartUp.AddOffer(offer,idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Offer AddOfferWithIdSourceOffer(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer updatedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                updatedOffer = mgr.AddOfferWithIdSourceOffer(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return updatedOffer;
        }


        public List<CustomerPurchaseOrder> GetOfferPurchaseOrders(Company company, byte idCurrency, Int64 idOffer, Int64 accountingYear)
        {
            List<CustomerPurchaseOrder> customerPurchaseOrder = null;

            try
            {
                CrmManager mgr = new CrmManager();

                customerPurchaseOrder = mgr.GetOfferPurchaseOrders(company, idCurrency, idOffer, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customerPurchaseOrder;
        }

        /// <summary>
        /// This method is to get sales user details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <returns>List of sales user</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;SalesUser&gt; salesUsers = control.GetAllSalesUserPeopleDetails(idCurrency,accountingYear);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<SalesUser> GetAllSalesUserPeopleDetailsWithPlantAndPermission(byte idCurrency, Int32 accountingYear)
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserPeopleDetailsWithPlantAndPermission(idCurrency, connectionString, connectionWorkbenchString, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }


        public People GetPeopleDetailByIdPerson(Int32 idPerson)
        {
            People people = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                people = mgr.GetPeopleDetailByIdPerson(connectionString, idPerson);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return people;
        }

        public Activity AddActivity(Activity activity)
        {
            Activity addedActivity = new Activity();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                addedActivity = mgr.AddActivity(activity, connectionString, Properties.Settings.Default.ActivityAttachmentsPath, WorkbenchConnectionString, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.MailFrom, Properties.Settings.Default.EmailTemplate);
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

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedActivity;
        }

        public bool AddActivityAttendees(Int64 idActivity, List<ActivityAttendees> activityAttendees)
        {
            bool isinserted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isinserted = mgr.AddActivityAttendees(idActivity, activityAttendees, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isinserted;
        }

        public bool AddLogEntriesByActivity(Int64 idActivity, List<LogEntriesByActivity> logEntriesByActivity)
        {
            bool isinserted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isinserted = mgr.AddLogEntriesByActivity(idActivity, logEntriesByActivity, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isinserted;
        }


        public bool UpdateActivity(Activity activity)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateActivity(activity, connectionString, Properties.Settings.Default.ActivityAttachmentsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public bool DeleteActivityAttendees(ActivityAttendees activityAttendees)
        {
            bool isDeleted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isDeleted = mgr.DeleteActivityAttendees(activityAttendees, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }

        public List<Activity> GetActivities(Int32 idOwner)
        {
            List<Activity> lstActivity = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lstActivity = mgr.GetActivities(idOwner, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lstActivity;
        }

        public List<LogEntriesByActivity> GetlogEntriesByActivity(Int64 idActivity, byte idLogEntryType)
        {
            List<LogEntriesByActivity> lstLogEntriesByActivity = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lstLogEntriesByActivity = mgr.GetlogEntriesByActivity(idActivity, idLogEntryType, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lstLogEntriesByActivity;
        }

        public Activity GetActivityByIdActivity(Int64 idActivity)
        {
            Activity Activity = new Activity();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Activity = mgr.GetActivityByIdActivity(idActivity, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Activity;
        }

        public List<Company> GetAllCustomerCompanies()
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetAllCustomerCompanies(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public bool UpdateCommentsByActivity(List<LogEntriesByActivity> logEntriesByActivity)
        {
            bool isinserted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isinserted = mgr.UpdateCommentsByActivity(logEntriesByActivity, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isinserted;
        }

        public List<People> GetAllActivePeoples()
        {
            List<People> ActivePeoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ActivePeoples = mgr.GetAllActivePeoples(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ActivePeoples;
        }

        public List<Customer> GetAllCustomer()
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetAllCustomer(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        public List<Activity> GetActivitiesByIdPermission(string idOwner, Int32 idPermission, string idPlant)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdPermission(idOwner, idPermission, idPlant, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public Activity GetPlannedAndActualActivity(Int32 idOwner)
        {
            Activity activity = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activity = mgr.GetPlannedAndActualActivity(idOwner, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activity;
        }

        public Activity GetPlannedAndActualActivity(Int32 idOwner, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            Activity activity = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activity = mgr.GetPlannedAndActualActivity(idOwner, connectionString, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activity;
        }

        //public List<ActivityAttachment> UploadActivityAttachment(List<ActivityAttachment> activityAttachments)
        //{
        //    List<ActivityAttachment> uploadedActivityAttachments = null;

        //    try
        //    {
        //        CrmManager mgr = new CrmManager();

        //        uploadedActivityAttachments = mgr.UploadActivityAttachment(activityAttachments, Properties.Settings.Default.ActivityAttachmentsPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return uploadedActivityAttachments;
        //}

        public ActivityAttachment DownloadActivityAttachment(ActivityAttachment activityAttachment)
        {
            ActivityAttachment downloadedActivityAttachment = null;

            try
            {
                CrmManager mgr = new CrmManager();

                downloadedActivityAttachment = mgr.DownloadActivityAttachment(activityAttachment, Properties.Settings.Default.ActivityAttachmentsPath);
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

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return downloadedActivityAttachment;
        }

        public List<UserManagerDtl> GetSalesUserByPlant(string idPlants)
        {
            List<UserManagerDtl> userManagers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                userManagers = mgr.GetSalesUserByPlant(idPlants, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return userManagers;
        }

        public List<Activity> GetActivityReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivityReportDetails(idSalesOwner, fromDate, toDate, idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public List<Offer> GetOfferReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idbusinessUnit, string idSite, byte idCurrency, Company company)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferReportDetails(idSalesOwner, fromDate, toDate, idbusinessUnit, idSite, idCurrency, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<People> GetContactsByLinkedItemAccount(string idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByLinkedItemAccount(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }


        public List<People> GetAllAttendesList(string idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetAllAttendesList(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }


        public List<Company> GetSelectedSalesOwnerSites(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedSalesOwnerSites(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }



        public List<Company> GetSelectedUserCustomerPlant(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomerPlant(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }


        public List<Company> GetCustomerPlant(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomerPlant(connectionString, idUser, idZone, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public bool DeleteActivity(Activity activity)
        {
            bool isDeleted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isDeleted = mgr.DeleteActivity(activity, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }

        public bool DeleteMultipleActivities(List<Activity> activities, LogEntriesByActivity deleteLogEntry)
        {
            bool isDeleted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isDeleted = mgr.DeleteMultipleActivities(MainServerConnectionString, activities, deleteLogEntry);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is used to get all selected users by idsite 
        /// </summary>
        /// <param name="idSite"></param>
        /// <returns></returns>
        public List<Company> GetSelectedUserCustomersByPlant(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is used to get all selected users by idsite
        /// </summary>
        /// <param name="idSite">The id Site</param>
        /// <returns>The list of selected users</returns>
        public List<People> GetSelectedUserContactsByPlant(string idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSelectedUserContactsByPlant(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }


        public List<People> GetContactsByIdPermission(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission(connectionString, idActiveUser, idUser, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        /// <summary>
        /// This method is used to get all activities related to idSite.
        /// </summary>
        /// <param name="idSite">The Emdep Site</param>
        /// <returns>The list of activitites.</returns>
        public List<Activity> GetActivitiesByIdSite(Int32 idSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdSite(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        /// <summary>
        /// This method is used to get all activities linked to Contact.
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns></returns>
        public List<Activity> GetActivitiesByIdContact(Int32 idPerson)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdContact(idPerson, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }


        /// <summary>
        /// This method is used to get all tags from crmConext.
        /// </summary>
        /// <returns>The list of all tags.</returns>
        public List<Tag> GetAllTags()
        {
            List<Tag> tags = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                tags = mgr.GetAllTags(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return tags;
        }

        /// <summary>
        /// This method is to get car tag exist or not
        /// </summary>
        /// <param name="Name">Get tag name</param>
        /// <returns>Is exist or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///          bool isExist =  control.IsExistTagName(Name);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool IsExistTagName(string Name)
        {
            bool isExist = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isExist = mgr.IsExistTagName(connectionString, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isExist;
        }

        /// <summary>
        /// This method is to save tag to main server.
        /// </summary>
        /// <param name="tag">The tag to save.</param>
        /// <returns>The tag with idtag.</returns>
        /// <example>
        /// <code>
        /// </code>
        /// </example>
        public Tag AddTag(Tag tag)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                tag = mgr.AddTag(tag, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return tag;
        }

        /// <summary>
        /// This method is to add notification to main Server. (as notifications is replicated table)
        /// and Send email.
        /// </summary>
        /// <param name="notification">The notification</param>
        /// <returns>notification with idNotification</returns>
        public Notification AddCommonNotification(Notification notification)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                notification = mgr.AddCommonNotification(notification, connectionString, connectionWorkbenchString, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.MailFrom, Properties.Settings.Default.EmailTemplate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notification;
        }

        public List<Activity> GetActivitiesByIdOffer(Int64 idOffer, Int32 idEmdepSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdOffer(idOffer, idEmdepSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public Offer GetOfferByIdOfferAndEmdepSite(Int64 idOffer, string offerConnectStr)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferByIdOfferAndEmdepSite(idOffer, offerConnectStr);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offer;
        }

        public People GetContactsByIdPerson(Int32 idPerson)
        {
            People peopleContact = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peopleContact = mgr.GetContactsByIdPerson(idPerson, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peopleContact;
        }

        public IList<LookupValue> GetBusinessUnitsWithoutRestrictedBU(Int32 idCurrentUser)
        {
            IList<LookupValue> list = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                list = mgr.GetBusinessUnitsWithoutRestrictedBU(idCurrentUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }

        /// <summary>
        /// Activity Reminder in the application when the date will be reached, we will send some email to the user.
        /// </summary>
        /// <returns></returns>
        public List<Activity> GetActivitiesForActivityReminder()
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                activities = mgr.GetActivitiesForActivityReminder(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        /// <summary>
        /// This method is used to send mails for activity reminder.
        /// </summary>
        /// <param name="activityMail"></param>
        public void SendActivityReminderMail(ActivityMail activityMail)
        {
            try
            {
                if (activityMail != null)
                {
                    ApplicationManager Appmgr = new ApplicationManager();
                    Appmgr.SendActivityReminderMail(activityMail, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.MailFrom, Properties.Settings.Default.EmailTemplate);
                }
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
        /// After send mail activity reminder then update isSentMail to 1
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public bool UpdateActivityReminder(Activity activity)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateActivityReminder(activity, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public List<Offer> GetOffersEngineeringAnalysis(byte idCurrency, Int32 idCurrentUser, Int32 idZone, Int64 accountingYear, Company compdetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersEngineeringAnalysis(idCurrency, idCurrentUser, idZone, accountingYear, compdetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        public List<Offer> GetOffersEngineeringAnalysis(byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersEngineeringAnalysis(idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, compdetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public Double GetOfferMaxValueById(Int16 idMaxValue)
        {
            Double max_value = 0;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                max_value = mgr.GetOfferMaxValueById(connectionString, idMaxValue);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return max_value;
        }

        public List<Activity> GetAllActivityReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idSite, string idContacts)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetAllActivityReportDetails(idSalesOwner, fromDate, toDate, idSite, idContacts, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public List<Offer> GetAllOfferReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idbusinessUnit, string idSite, byte idCurrency, Company company, string idContacts)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetAllOfferReportDetails(idSalesOwner, fromDate, toDate, idbusinessUnit, idSite, idCurrency, company, idContacts);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        /// <summary>
        /// Sprint 20 - This method is used to check if Offer or Order by CustomerPurchaseOrder Table 
        /// (Connection string - emdepSite).
        /// </summary>
        /// <param name="idOffer">The Id Offer</param>
        /// <param name="company">The Company</param>
        /// <returns>True (Order) if Has purchase order else False (Offer)</returns>
        public bool IsPurchaseOrderDoneByIdOffer(Int64 idOffer, Company company)
        {
            bool IsPurchaseOrderDone = false;

            try
            {
                CrmManager mgr = new CrmManager();
                IsPurchaseOrderDone = mgr.IsPurchaseOrderDoneByIdOffer(idOffer, company);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return IsPurchaseOrderDone;
        }

        /// <summary>
        /// Sprint 21 - This method is used to Disable Contact.
        /// </summary>
        /// <param name="people">The people</param>
        /// <returns>True if Disabled account else False</returns>
        public bool DisableContact(People people)
        {
            bool isDeleted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isDeleted = mgr.DisableContact(people, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }

        /// <summary>
        /// Sprint 21 - This method is used to Disable account.
        /// </summary>
        /// <param name="company">The Company with idCompany</param>
        /// <returns>True if Disabled account else False.</returns>
        public bool DisableAccount(Company company)
        {
            bool isDeleted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isDeleted = mgr.DisableAccount(company, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }


        /// <summary>
        /// Sprint 21 - This method is to get list of company target details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingFromYear">Get accounting from year</param>
        /// <param name="accountingToYear">Get accounting to year</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>list of company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Company&gt; companies = crmControl.GetSitesTarget(idCurrency,idUser,idZone,accountingFromYear,accountingToYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetSitesTargetByPlant(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSitesTargetByPlant(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }


        /// <summary>
        /// Sprint 21 - This method is to get list of company target details
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYearFrom">Get accounting year From</param>
        /// <param name="accountingYearTo">Get accounting year To</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <returns>list of company details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          ICrmService crmControl = new CrmServiceController(ServiceUrl);
        ///          List&lt;Company&gt; companies = crmControl.GetSitesTarget(idCurrency,idUser,idZone,accountingFromYear,accountingToYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Company> GetSitesTargetByPlant(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSitesTargetByPlant(connectionString, idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }


        /// <summary>
        /// Sprint 21 - This method is to get target forecast details for Role 22.
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingFromYear">Get accounting from year</param>
        /// <param name="accountingToYear">Get accounting to year</param>
        /// <param name="idUserPermission">Get user permission id</param>
        /// <param name="idSite">Get idSite for Role 22</param>
        /// <returns>List of offer</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         IList&lt;Offer&gt; offers = control.GetTargetForecastByPlant(idCurrency,idUser,idZone,accountingFromYear,accountingToYear,idUserPermission);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<Offer> GetTargetForecastByPlant(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, Int32 idUserPermission, string idSite)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecastByPlant(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        ///// <summary>
        ///// Sprint 21 - This method is to get target forecast details for Role 22.
        ///// </summary>
        ///// <param name="idCurrency">Get currency id</param>
        ///// <param name="idUser">Get user id</param>
        ///// <param name="idZone">Get zone id</param>
        ///// <param name="accountingYearFrom">Get accounting year From</param>
        ///// <param name="accountingYearTo">Get accounting year To</param>
        ///// <param name="idUserPermission">Get user permission id</param>
        ///// <param name="idSite">Get idSite for Role 22</param>
        ///// <returns>List of offer</returns>
        ///// <example>
        ///// This sample shows how to call the method
        ///// <code>
        ///// class TestClass 
        ///// {
        /////     static void Main(string[] args)
        /////     {
        /////         ICrmService control = new CrmServiceController(ServiceUrl);
        /////         IList&lt;Offer&gt; offers = control.GetTargetForecastByPlant(idCurrency,idUser,idZone,accountingFromYear,accountingToYear,idUserPermission);
        /////     }
        ///// }
        ///// </code>
        ///// </example>
        //public IList<Offer> GetTargetForecastByPlant(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission, string idSite)
        //{
        //    IList<Offer> offers = null;

        //    try
        //    {
        //        CrmManager mgr = new CrmManager();
        //        string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
        //        offers = mgr.GetTargetForecastByPlant(connectionString, idCurrency, idUser, idZone,  accountingYearFrom,  accountingYearTo, idUserPermission, idSite);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return offers;
        //}

        /// <summary>
        /// Sprint 21 - This method is to get customer target details
        /// Report Dashboard 2 - GetTargetByCustomerByPlant - Role 22
        /// </summary>
        /// <param name="idCurrency">Get currency id</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <param name="accountingYear">Get accounting year</param>
        /// <param name="idSite">Get idSite</param>
        /// <returns>List of offer having customer target details</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ICrmService control = new CrmServiceController(ServiceUrl);
        ///         List&lt;Offer&gt; offers= control.GetTargetByCustomer(idCurrency,idUser,idZone,accountingYear,idSite);
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Offer> GetTargetByCustomerByPlant(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, string idSite)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetByCustomerByPlant(connectionString, idCurrency, idUser, idZone, accountingYear, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        public List<Activity> GetSharedActivitiesByIdPermission(Int32 idActiveUser, string idOwner, Int32 idPermission, string idPlant)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetSharedActivitiesByIdPermission(idActiveUser, idOwner, idPermission, idPlant, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public List<Activity> GetSharedActivitiesByIdPermission(Int32 idActiveUser, string idOwner, Int32 idPermission, string idPlant, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetSharedActivitiesByIdPermission(idActiveUser, idOwner, idPermission, idPlant, connectionString, accountingYearFrom, accountingYearTo, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public bool UpdateOrderByIdOffer(Offer offer)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderByIdOffer(offer, connectionString, mainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public List<Activity> GetActivitiesLinkedToAccount(string idOwner, Int32 idPermission, string idPlant, Int32 idOfferAccount, Int64 idOffer, Int32 idEmdepSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesLinkedToAccount(idOwner, idPermission, idPlant, idOfferAccount, idOffer, idEmdepSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return activities;
        }


        /// <summary>
        /// Sprint 23 - [CRM-M023-04]
        /// Accounts not displayed properly when adding new activity for role22  (same like role 22 in account section).
        /// </summary>
        /// <param name="idActiveUser"></param>
        /// <param name="idSite"></param>
        /// <returns></returns>
        public List<Company> GetAccountBySelectedPlant(Int32 idActiveUser, string idSite)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetAccountBySelectedPlant(connectionString, idActiveUser, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// Sprint 24 - [CRM-M024-06] Automatic weekly report by email to sales users.
        /// </summary>
        /// <param name="idCurrency">The id currency</param>
        /// <param name="idUser">The sales user</param>
        /// <param name="idZone">The idzone</param>
        /// <param name="accountingYear">The accounting year</param>
        /// <param name="company">The Company with connection string to connect to asigned plant</param>
        /// <param name="Interval">The Interval - (daily, weekly, monthly, quarterly, yearly,...)</param>
        /// <returns></returns>
        public List<Offer> GetReportOffersPerSalesUser(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company company, string Interval)
        {
            List<Offer> Offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                Offer = mgr.GetReportOffersPerSalesUser(idCurrency, idUser, idZone, accountingYear, company, Interval);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return Offer;
        }

        /// <summary>
        /// [CRM-M024-06] Automatic weekly report by email to sales users.
        /// Activities are going to due in Inverval.
        /// </summary>
        /// <param name="Interval"></param>
        /// <returns></returns>
        public List<Activity> GetActivitiesGoingToDueInInverval(string Interval)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                activities = mgr.GetActivitiesGoingToDueInInverval(connectionString, Interval);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return activities;
        }

        /// <summary>
        /// Get all sales users for automatic weekly report.
        /// </summary>
        /// <returns></returns>
        public List<SalesUser> GetAllSalesUsersForReport()
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUsersForReport(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return salesUsers;
        }

        /// <summary>
        /// [CRM-M025-03] Add offer to an activity from the activity popup
        /// </summary>
        /// <param name="idCurrency"></param>
        /// <param name="idUser"></param>
        /// <param name="accountingYear"></param>
        /// <param name="company"></param>
        /// <param name="idUserPermission"></param>
        /// <param name="idSite"></param>
        /// <returns></returns>
        public List<Offer> GetOffersByIdSiteToLinkedWithActivities(byte idCurrency, Int32 idUser, Int64 accountingYear, Company company, Int32 idUserPermission, Int32 idSite)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersByIdSiteToLinkedWithActivities(idCurrency, idUser, accountingYear, company, idUserPermission, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        public List<Offer> GetOffersByIdSiteToLinkedWithActivities(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company company, Int32 idUserPermission, Int32 idSite)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersByIdSiteToLinkedWithActivities(idCurrency, idUser, accountingYearFrom, accountingYearTo, company, idUserPermission, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        /// <summary>
        /// [CRM-M025-04] Automatic task suggestion wizzard.
        /// Added Values from database.
        /// </summary>
        /// <returns></returns>
        public List<ActivityTemplateTrigger> GetActivityTemplateTriggers()
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActivityTemplateTriggers(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LogEntriesByContact> GetLogEntriesByContact(Int32 idContact, byte idLogEntryType)
        {
            List<LogEntriesByContact> logEntriesByContact = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntriesByContact = mgr.GetLogEntriesByContact(idContact, idLogEntryType, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntriesByContact;
        }

        public List<People> GetAllSalesUserByIdSalesTeam(Int32 idSalesTeam)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetAllSalesUserByIdSalesTeam(idSalesTeam, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }


        public List<Competitor> GetAllCompetitor()
        {
            List<Competitor> competitors = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                competitors = mgr.GetAllCompetitor(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return competitors;
        }

        public List<People> GetAllSalesUser()
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetAllSalesUser(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        public People GetIdSalesTeamByIdSalesUser(Int32 idSalesUser)
        {
            People peoplesalesUser = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoplesalesUser = mgr.GetIdSalesTeamByIdSalesUser(idSalesUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoplesalesUser;
        }

        /// <summary>
        /// [001][2018-07-06][skhade][CRM-M042-05] Replacement of current exchange rates section.
        /// This method is used to get all daily currency conversions by date.
        /// </summary>
        /// <param name="fromDate">The from date from custom period.</param>
        /// <param name="toDate">The to date from custom period</param>
        /// <returns>The list of daily currency Conversions.</returns>
        public List<DailyCurrencyConversion> GetAllDailyCurrencyConversionsByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllDailyCurrencyConversionsByDate(connectionString, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public DailyCurrencyConversion AddCurrencyConversion(DailyCurrencyConversion dailyCurrencyConversion)
        {
            DailyCurrencyConversion addedDailyCurrencyConversion = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                addedDailyCurrencyConversion = mgr.AddCurrencyConversion(connectionString, dailyCurrencyConversion);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedDailyCurrencyConversion;
        }


        public DailyCurrencyConversion UpdateCurrencyConversionDaily(DailyCurrencyConversion dailyCurrencyConversion)
        {
            DailyCurrencyConversion addedDailyCurrencyConversion = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                addedDailyCurrencyConversion = mgr.UpdateCurrencyConversionDaily(connectionString, dailyCurrencyConversion);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedDailyCurrencyConversion;
        }

        public bool UpdateCurrencyConversionListDaily(List<DailyCurrencyConversion> dailyCurrencyConversionList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCurrencyConversionListDaily(connectionString, dailyCurrencyConversionList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateReadOnlyActivityLogs(Activity activity)
        {
            bool isUpdate = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;

                isUpdate = mgr.UpdateReadOnlyActivityLogs(activity, connectionString, Properties.Settings.Default.ActivityAttachmentsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdate;
        }

        public bool IsUserManager(Int32 idUser)
        {
            bool isLoginUserManager = false;
            try
            {
                UserManager mgr = new UserManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                isLoginUserManager = mgr.IsUserManager(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isLoginUserManager;
        }

        public List<Activity> GetPlannedAppointmentAndAppointment(Int32 idOwner, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            List<Activity> LstActivities = new List<Activity>();
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                LstActivities = mgr.GetPlannedAppointmentAndAppointment(connectionString, idOwner, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstActivities;
        }

        public List<Activity> GetPlannedAppiontmentByUserId(Int32 idActiveUser, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            List<Activity> LstActivities = new List<Activity>();
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                LstActivities = mgr.GetPlannedAppiontmentByUserId(connectionString, idActiveUser, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstActivities;
        }

        public List<Offer> GetSelectedOffersEngAnalysisDateWise(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetSelectedOffersEngAnalysisDateWise(idUser, idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        public List<Offer> GetOffersEngineeringAnalysisByPermission(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }


        public List<WarehouseCategory> GetWarehouseCategories()
        {
            List<WarehouseCategory> articleCategories = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                articleCategories = mgr.GetWarehouseCategories(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return articleCategories;
        }

        public List<ArticleBySupplier> GetArticlesBySupplier(string idCompanies = null)
        {
            List<ArticleBySupplier> articlesBySupplier = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                articlesBySupplier = mgr.GetArticlesBySupplier(connectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return articlesBySupplier;
        }

        public List<Offer> GetArticlesReportDetails(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails(connectionString, idActiveUser, company, fromDate, toDate, idCurrency, idArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        public List<Offer> GetOffersByIdCustomerToLinkedWithActivities(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company company, Int32 idUserPermission, Int32 idCustomer)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersByIdCustomerToLinkedWithActivities(idCurrency, idUser, accountingYearFrom, accountingYearTo, company, idUserPermission, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        public IList<LookupValue> GetLookupvaluesWithoutRestrictedBU(Int32 idUser, Int32 idUserPermission)
        {
            IList<LookupValue> LstlookupValue = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                LstlookupValue = mgr.GetLookupvaluesWithoutRestrictedBU(idUser, idUserPermission, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return LstlookupValue;
        }

        public List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionwise(Int32 idUser, byte idCurrency, Int32 accountingYear, Int32 idUserPermission)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetPlantSalesQuotaWithYearByIdUserPermissionwise(connectionString, idUser, idCurrency, accountingYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }

        public List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionDatewise(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise(connectionString, idUser, idCurrency, accountingYearFrom, accountingYearTo, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }

        public List<CpType> GetAllCpTypes()
        {
            List<CpType> cpTypes = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                cpTypes = mgr.GetAllCpTypes(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return cpTypes;
        }

        public List<CpType> GetCpTypesByTemplate(byte idTemplate)
        {
            List<CpType> cpTypes = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                cpTypes = mgr.GetCpTypesByTemplate(connectionString, idTemplate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return cpTypes;
        }

        public List<Detection> GetDetectionByCpTypeAndTemplate(byte idTemplate, byte idCPType)
        {
            List<Detection> detections = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                detections = mgr.GetDetectionByCpTypeAndTemplate(connectionString, idTemplate, idCPType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return detections;
        }

        public List<Offer> GetModulesReportDetails(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetModulesReportDetails(idActiveUser, company, fromDate, toDate, idCompany, idTemplate, idCPType, idDetections, idCurrency, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        public List<Activity> GetActivityPerformanceTest(Int32 idActiveUser, string idOwner, Int32 idPermission, string idPlant, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivityPerformanceTest(idActiveUser, idOwner, idPermission, idPlant, connectionString, accountingYearFrom, accountingYearTo, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public List<Company> GetCompanyPlantByUserId(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCompanyPlantByUserId(connectionString, idUser, idZone, idUserPermission, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public List<Company> GetSelectedUserCompanyPlantByIdUser(string idUser, bool isIncludeDefault = false)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCompanyPlantByIdUser(connectionString, idUser, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public SalesUser GetUserAllSalesQuotaByDate(Int32 idUser, byte idCurrency)
        {
            SalesUser salesUser = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUser = mgr.GetUserAllSalesQuotaByDate(connectionString, idUser, idCurrency);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUser;
        }

        public List<SalesUser> GetAllSalesUserPeopleDetailsWithPlantAndPermissionByDate(byte idCurrency, Int32 accountingYear)
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserPeopleDetailsWithPlantAndPermissionByDate(idCurrency, connectionString, connectionWorkbenchString, accountingYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }



        public List<DateTime> GetAllCurrencyConversionDates()
        {
            List<DateTime> dateTimes = null;

            try
            {
                CrmManager mgr = new CrmManager();

                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                dateTimes = mgr.GetAllCurrencyConversionDates(connectionWorkbenchString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return dateTimes;
        }



        public DailyCurrencyConversion GetCurrencyRateByDateAndId(DailyCurrencyConversion dailyCurrencyConversion)
        {
            try
            {
                CrmManager mgr = new CrmManager();

                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                dailyCurrencyConversion = mgr.GetCurrencyRateByDateAndId(connectionWorkbenchString, dailyCurrencyConversion);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return dailyCurrencyConversion;
        }

        public bool AddSaleUserQuotaWithDate(List<SalesUserQuota> salesUserQuotas)
        {
            bool isAdded = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isAdded = mgr.AddSaleUserQuotaWithDate(salesUserQuotas, connectionString);
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

        public bool UpdatePlantBusinessUnitSalesQuotaWithYearAndDate(PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota)
        {
            bool isUpdatedPlantquota = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdatedPlantquota = mgr.UpdatePlantBusinessUnitSalesQuotaWithYearAndDate(plantBusinessUnitSalesQuota, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdatedPlantquota;
        }



        public List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionwiseByDate(Int32 idUser, byte idCurrency, Int32 accountingYear, Int32 idUserPermission)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetPlantSalesQuotaWithYearByIdUserPermissionwiseByDate(connectionString, idUser, idCurrency, accountingYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }

        public IList<Offer> GetSelectedUsersTargetForecastDate(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersTargetForecastDate(connectionString, idCurrency, idUser, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public IList<Offer> GetTargetForecastByPlantDate(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission, string idSite)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecastByPlantDate(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public bool UpdateSalesTargetBySiteDate(SalesTargetBySite salesTargetBySite)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateSalesTargetBySiteDate(salesTargetBySite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public List<DailyCurrencyConversion> GetLatestCurrencyConversion()
        {
            List<DailyCurrencyConversion> dailyCurrencyConversions = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                dailyCurrencyConversions = mgr.GetLatestCurrencyConversion(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return dailyCurrencyConversions;
        }

        public List<DailyCurrencyConversion> GetLatestCurrencyConversion_V2190()
        {
            List<DailyCurrencyConversion> dailyCurrencyConversions = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                dailyCurrencyConversions = mgr.GetLatestCurrencyConversion_V2190(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return dailyCurrencyConversions;
        }

        public PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser = 0)
        {
            PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(connectionString, assignedPlant, idCurrency, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }

        public SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(string userIds, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            SalesUserQuota salesUserQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuota = mgr.GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(connectionString, userIds, idCurrency, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuota;
        }


        public List<Company> GetSitesTargetNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSitesTargetNewCurrencyConversion(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }


        public List<Company> GetSitesTargetByPlantNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSitesTargetByPlantNewCurrencyConversion(connectionString, idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }


        public List<Company> GetSelectedUsersSitesTargetNewCurrencyConversion(byte idCurrency, Int64 accountingFromYear, Int64 accountingToYear, string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUsersSitesTargetNewCurrencyConversion(connectionString, idCurrency, accountingFromYear, accountingToYear, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }


        public IList<Offer> GetTargetForecastNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecastNewCurrencyConversion(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<SalesTargetBySite> GetSalesTargetBySiteDetailByIdSite(Int32 idSite)
        {
            List<SalesTargetBySite> salesTargetBySites = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesTargetBySites = mgr.GetSalesTargetBySiteDetailByIdSite(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesTargetBySites;
        }

        public List<SalesUserQuota> GetAllSalesUserDetailsNewCurrencyConversion(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string assignedPlants)
        {
            List<SalesUserQuota> salesUserQuotas = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuotas = mgr.GetAllSalesUserDetailsNewCurrencyConversion(connectionString, idCurrency, accountingYearFrom, accountingYearTo, assignedPlants);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuotas;
        }


        public List<LookupValue> GetPlantQuotaAmountBUWiseNewCurrencyConversion(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            List<LookupValue> lookupValues = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lookupValues = mgr.GetPlantQuotaAmountBUWiseNewCurrencyConversion(assignedPlant, idCurrency, accountingYearFrom, accountingYearTo, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lookupValues;
        }

        public List<CarProjectDetail> GetTopCarProjectOffersOptimization(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 projectLimit, Int32 idUserPermission, Company companydetails, Int32 idCurrentUser = 0)
        {
            List<CarProjectDetail> carProjects = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                carProjects = mgr.GetTopCarProjectOffersOptimization(idCurrency, idUser, accountingYearFrom, accountingYearTo, projectLimit, idUserPermission, companydetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return carProjects;
        }


        public List<OfferDetail> GetSalesStatusWithTarget(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusWithTarget(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }


        public List<OfferDetail> GetSalesStatusByMonthAllPermission(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthAllPermission(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }


        //public List<ProductCategory> GetAllProductCategory()
        //{
        //    List<ProductCategory> productCategories = null;

        //    try
        //    {
        //        CrmManager mgr = new CrmManager();
        //        string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
        //        productCategories = mgr.GetAllProductCategory(connectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return productCategories;
        //}


        public List<DailyEventActivity> GetDailyEventActivities(string idOwner, Int32 idPermission, string idPlant)
        {
            List<DailyEventActivity> dailyEventActivities = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                dailyEventActivities = mgr.GetDailyEventActivities(idOwner, idPermission, idPlant, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return dailyEventActivities;
        }

        //public List<ProductCategory> GetAllProductCategories()
        //{
        //    List<ProductCategory> productCategories = null;

        //    try
        //    {
        //        CrmManager mgr = new CrmManager();
        //        string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
        //        productCategories = mgr.GetAllProductCategories(connectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return productCategories;
        //}

        public List<ProductCategory> GetAllCategory()
        {
            List<ProductCategory> productCategories = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                productCategories = mgr.GetAllCategory(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return productCategories;
        }


        public List<ProductCategoryOfferOption> GetAllCategoryOfferOption()
        {
            List<ProductCategoryOfferOption> productCategories = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                productCategories = mgr.GetAllCategoryOfferOption(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return productCategories;
        }

        public Int64? GetIdProductCategoryByIdOffer(Int64 idOffer, string connectPlant)
        {
            Int64? idProductCategory = null;

            try
            {
                CrmManager mgr = new CrmManager();

                idProductCategory = mgr.GetIdProductCategoryByIdOffer(idOffer, connectPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return idProductCategory;
        }

        public List<SalesUser> GetAllSalesUserQuotaPeopleDetailsByDatewise(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear)
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserQuotaPeopleDetailsByDatewise(idCurrency, connectionString, connectionWorkbenchString, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }

        public List<Activity> GetPlannedAppiontmentByUserIdIsInternal(Int32 idActiveUser, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            List<Activity> LstActivities = new List<Activity>();
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                LstActivities = mgr.GetPlannedAppiontmentByUserIdIsInternal(connectionString, idActiveUser, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstActivities;
        }

        public List<ActivitiesRecurrence> GetActivitiesRecurrence(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActivitiesRecurrence(connectionString, idUser, accountingYearFrom, accountingYearTo, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ActivitiesRecurrence AddActivitiesRecurrence(ActivitiesRecurrence activitiesRecurrence)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActivitiesRecurrence(connectionString, activitiesRecurrence);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ActivitiesRecurrence UpdateActivitiesRecurrence(ActivitiesRecurrence activitiesRecurrence)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActivitiesRecurrence(connectionString, activitiesRecurrence);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool CreateAutomaticPlannedActivity(DateTime startDate, DateTime endDate, Int32 idUser)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.CreateAutomaticPlannedActivity(connectionString, mainConnectionString, startDate, endDate, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public CustomerTargetDetail GetTop5CustomersDashboardDetails(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false)
        {
            CustomerTargetDetail customerTargetDetail = new CustomerTargetDetail();
            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customerTargetDetail = mgr.GetTop5CustomersDashboardDetails(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, assignedPlant, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customerTargetDetail;
        }

        public List<SalesUserQuota> GetAllSalesTeamUserDetail(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string assignedPlants)
        {
            List<SalesUserQuota> salesUserQuotas = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUserQuotas = mgr.GetAllSalesTeamUserDetail(connectionString, idCurrency, accountingYearFrom, accountingYearTo, assignedPlants);
            }

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserQuotas;
        }


        public List<SalesUserWon> GetAllSalesTeamUserWonDetail(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser)
        {
            List<SalesUserWon> salesUserWons = new List<SalesUserWon>();

            try
            {
                CrmManager mgr = new CrmManager();
                salesUserWons = mgr.GetAllSalesTeamUserWonDetail(company, idCurrency, idUser, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserWons;
        }

        public List<OfferMonthDetail> GetSalesStatusByMonthWithTarget(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false)
        {
            List<OfferMonthDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthWithTarget(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser, isSiteTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        public BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false)
        {

            BusinessUnitTargetDetail businessUnitTargetDetail = null;

            try
            {
                CrmManager mgr = new CrmManager();

                businessUnitTargetDetail = mgr.GetBusinessUnitStatusWithTarget(idCurrency, assignedPlant, accountingYearFrom, accountingYearTo, idCurrentUser, company, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return businessUnitTargetDetail;
        }

        public List<BusinessUnitDetail> GetBusinessUnitsDetails(Int32 idCurrentUser)
        {
            List<BusinessUnitDetail> list = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                list = mgr.GetBusinessUnitsDetails(idCurrentUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }

        public List<SalesStatusTypeDetail> GetSalesStatusTypeDetail()
        {
            List<SalesStatusTypeDetail> salesStatusTypes = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesStatusTypes = mgr.GetSalesStatusTypeDetail(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesStatusTypes;
        }

        public List<PlantBusinessUnitSalesQuota> GetPlantQuotaDetailsById(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission, Int32 idCompany)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string workbenchconnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetPlantQuotaDetailsById(connectionString, idUser, idCurrency, accountingYearFrom, accountingYearTo, idUserPermission, idCompany, workbenchconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }


        public bool UpdatePlantQuota(List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuotas)
        {
            bool isUpdatedPlantquota = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdatedPlantquota = mgr.UpdatePlantQuota(plantBusinessUnitSalesQuotas, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdatedPlantquota;
        }

        public PlannedVisitDetail GetPlannedVisitDetail(DateTime accountingYearFrom, DateTime accountingYearTo, string idOwner = null, Int32 idPermission = 0, string idPlant = null)
        {
            PlannedVisitDetail plannedVisitDetail = new PlannedVisitDetail();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plannedVisitDetail = mgr.GetPlannedVisitDetail(connectionString, accountingYearFrom, accountingYearTo, idOwner, idPermission, idPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plannedVisitDetail;
        }


        public GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting)
        {
            GeosAppSetting geosAppSetting = new GeosAppSetting();

            try
            {
                CrmManager mgr = new CrmManager();
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


        public bool CreateAutomaticPlannedActivitySprint60(DateTime startDate, DateTime endDate, Int32 idUser, List<ActivitiesRecurrence> LstActivitiesRecurrence)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.CreateAutomaticPlannedActivitySprint60(connectionString, mainConnectionString, startDate, endDate, idUser, LstActivitiesRecurrence);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public double GetOfferAmountByCurrencyConversion(Int64 idOffer, double offerAmount, byte formIdCurrency, byte toIdCurrency, DateTime? deliveryDate, DateTime? sendIn, DateTime? RFQReception, DateTime? CreatedIn, DateTime? ReceivedIn)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetOfferAmountByCurrencyConversion(connectionString, idOffer, offerAmount, formIdCurrency, toIdCurrency, deliveryDate, sendIn, RFQReception, CreatedIn, ReceivedIn);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<People> GetAllAttendesList_V2030(string idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetAllAttendesList_V2030(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        public List<People> GetContactsByLinkedItemAccount_V2030(string idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByLinkedItemAccount_V2030(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        public Activity AddActivity_V2031(Activity activity)
        {
            Activity addedActivity = new Activity();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                addedActivity = mgr.AddActivity_V2031(activity, connectionString, Properties.Settings.Default.ActivityAttachmentsPath, WorkbenchConnectionString, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.MailFrom, Properties.Settings.Default.EmailTemplate);
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

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedActivity;
        }

        public bool UpdateActivity_V2031(Activity activity)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateActivity_V2031(activity, connectionString, Properties.Settings.Default.ActivityAttachmentsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public List<ActivitiesRecurrence> GetActivitiesRecurrence_V2031(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActivitiesRecurrence_V2031(connectionString, idUser, accountingYearFrom, accountingYearTo, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public ActivitiesRecurrence UpdateActivitiesRecurrence_V2031(ActivitiesRecurrence activitiesRecurrence)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActivitiesRecurrence_V2031(connectionString, activitiesRecurrence);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool CreateAutomaticPlannedActivity_V2031(DateTime startDate, DateTime endDate, Int32 idUser, List<ActivitiesRecurrence> LstActivitiesRecurrence)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.CreateAutomaticPlannedActivity_V2031(connectionString, mainConnectionString, startDate, endDate, idUser, LstActivitiesRecurrence);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Activity> GetActivitiesByIdOffer_V2031(Int64 idOffer, Int32 idEmdepSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdOffer_V2031(idOffer, idEmdepSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public bool UpdateOffer_V2031(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOffer_V2031(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public bool UpdateOrderByIdOffer_V2031(Offer offer)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderByIdOffer_V2031(offer, connectionString, mainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public People AddContact_V2033(People people)
        {
            People addedPeople = new People();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                addedPeople = mgr.AddContact_V2033(connectionString, people);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedPeople;
        }





        public List<People> GetContactsByIdPermission_V2033(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2033(connectionString, idActiveUser, idUser, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }


        public Offer AddOffer_V2033(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2033(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public Offer AddOfferWithIdSourceOffer_V2033(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOfferWithIdSourceOffer_V2033(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public bool UpdateOffer_V2033(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOffer_V2033(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public bool UpdateOfferForParticularColumn_V2033(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn_V2033(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public Offer GetOfferDetailsById_V2033(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2033(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, company, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public Offer AddOffer_V2034(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2034(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        //public Offer AddOfferWithIdSourceOffer_V2034(Offer offer, Int32 idSite, Int32 idUser)
        //{
        //    Offer addedOffer = null;

        //    try
        //    {
        //        CrmManager mgr = new CrmManager();
        //        string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
        //        string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
        //        string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
        //        bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
        //        string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
        //        string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

        //        MailServer mailServer = new MailServer();
        //        mailServer.MailServerName = Properties.Settings.Default.MailServerName;
        //        mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
        //        mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
        //        mailServer.MailFrom = Properties.Settings.Default.MailFrom;

        //        addedOffer = mgr.AddOfferWithIdSourceOffer_V2034(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString, Properties.Settings.Default.JiraCredential);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return addedOffer;
        //}


        public Offer UpdateOffer_V2034(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2034(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        //public Offer UpdateOfferForParticularColumn_V2034(Offer offer, Int32 idSite, Int32 idUser)
        //{

        //    try
        //    {
        //        CrmManager mgr = new CrmManager();
        //        string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
        //        string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
        //        string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
        //        bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
        //        string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
        //        string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
        //        MailServer mailServer = new MailServer();
        //        mailServer.MailServerName = Properties.Settings.Default.MailServerName;
        //        mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
        //        mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
        //        mailServer.MailFrom = Properties.Settings.Default.MailFrom;

        //        offer = mgr.UpdateOfferForParticularColumn_V2034(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString, Properties.Settings.Default.JiraCredential);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //    return offer;
        //}

        public List<Offer> GetSelectedOffersEngAnalysisDateWise_V2034(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                LstOffer = mgr.GetSelectedOffersEngAnalysisDateWise_V2034(idUser, idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        public List<Offer> GetOffersEngineeringAnalysisByPermission_V2034(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission_V2034(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }


        public bool CreateAutomaticPlannedActivity_V2035(DateTime startDate, DateTime endDate, Int32 idUser, List<ActivitiesRecurrence> LstActivitiesRecurrence)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.CreateAutomaticPlannedActivity_V2035(connectionString, mainConnectionString, startDate, endDate, idUser, LstActivitiesRecurrence);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Activity GetActivityByIdActivity_V2035(Int64 idActivity)
        {
            Activity Activity = new Activity();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Activity = mgr.GetActivityByIdActivity_V2035(idActivity, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Activity;
        }

        public List<Offer> GetOffersByIdCustomerToLinkedWithActivities_V2035(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company company, Int32 idUserPermission, Int32 idCustomer)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersByIdCustomerToLinkedWithActivities_V2035(idCurrency, idUser, accountingYearFrom, accountingYearTo, company, idUserPermission, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        public IList<Offer> GetOffersPipeline_V2035(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOffersPipeline_V2035(idCurrency, userids, idCurrentUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<OfferDetail> GetSalesStatusWithTarget_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusWithTarget_V2035(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }


        public List<OfferDetail> GetSalesStatusByMonthAllPermission_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthAllPermission_V2035(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }


        public List<TempAppointment> GetDailyEvents_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<TempAppointment> modelAppointments = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetDailyEvents_V2035(idCurrency, idUser, accountingYearFrom, accountingYearTo, compdetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }


        public IList<Customer> GetDashboard2Details_V2035(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetDashboard2Details_V2035(idCurrency, idCurrentUser, idsUser, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        public IList<Offer> GetSalesStatus_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatus_V2035(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false)
        {
            List<OfferMonthDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthWithTarget_V2035(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser, isSiteTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }


        public CustomerTargetDetail GetTop5CustomersDashboardDetails_V2035(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false)
        {
            CustomerTargetDetail customerTargetDetail = new CustomerTargetDetail();
            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customerTargetDetail = mgr.GetTop5CustomersDashboardDetails_V2035(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, assignedPlant, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customerTargetDetail;
        }


        public List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2035(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser)
        {
            List<SalesUserWon> salesUserWons = new List<SalesUserWon>();

            try
            {
                CrmManager mgr = new CrmManager();
                salesUserWons = mgr.GetAllSalesTeamUserWonDetail_V2035(company, idCurrency, idUser, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserWons;
        }


        public BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_V2035(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false)
        {

            BusinessUnitTargetDetail businessUnitTargetDetail = null;

            try
            {
                CrmManager mgr = new CrmManager();

                businessUnitTargetDetail = mgr.GetBusinessUnitStatusWithTarget_V2035(idCurrency, assignedPlant, accountingYearFrom, accountingYearTo, idCurrentUser, company, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return businessUnitTargetDetail;
        }


        public List<Offer> GetArticlesReportDetails_V2035(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails_V2035(connectionString, idActiveUser, company, fromDate, toDate, idCurrency, idArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }


        public List<Offer> GetAllOfferReportDetails_V2035(string idSalesOwner, DateTime fromDate, DateTime toDate, string idbusinessUnit, string idSite, byte idCurrency, Company company, string idContacts)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetAllOfferReportDetails_V2035(idSalesOwner, fromDate, toDate, idbusinessUnit, idSite, idCurrency, company, idContacts);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public Offer GetOfferDetailsById_V2035(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2035(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, company, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public List<EngineeringAnalysisType> GetEngAnalysisArticles()
        {

            try
            {
                CrmManager mgr = new CrmManager();

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetEngAnalysisArticles(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public Offer AddOffer_V2037(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2037(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public Offer UpdateOffer_V2037(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2037(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public Offer GetOfferDetailsById_V2037(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2037(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, company, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        public List<User> GetSalesAndCommericalUsers()
        {
            List<User> users = new List<User>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                users = mgr.GetSalesAndCommericalUsers(connectionString);
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

        public List<OfferContact> GetContactsOfCustomerGroupByOfferId(Int32 idCustomer)
        {
            List<OfferContact> offerContacts = new List<OfferContact>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offerContacts = mgr.GetContactsOfCustomerGroupByOfferId(connectionString, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerContacts;
        }


        public List<Offer> GetArticlesReportDetails_V2037(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails_V2037(connectionString, idActiveUser, company, fromDate, toDate, idCurrency, idArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }


        public bool UpdateOrderByIdOffer_V2037(Offer offer)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderByIdOffer_V2037(offer, connectionString, mainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public bool IsExistCustomer_V2040(string customerName, byte idCustomerType)
        {
            bool isExist = false;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isExist = mgr.IsExistCustomer_V2040(connectionString, customerName, idCustomerType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isExist;
        }

        public List<UserSiteGeosServiceProvider> GetAllCompaniesWithServiceProvider(Int32 idUser)
        {


            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllCompaniesWithServiceProvider(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public Int64 GetNextNumberOfOfferFromCounters_V2040(byte? idOfferType, Int32 idUser)
        {
            Int64 nextNumber = 0;
            try
            {
                CrmManager mgr = new CrmManager();
                string mainserverconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;

                nextNumber = mgr.GetNextNumberOfOfferFromCounters_V2040(idOfferType, mainserverconnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return nextNumber;
        }


        public string MakeOfferCode_V2040(byte? idOfferType, Int32 idCustomer, Int32 idUser)
        {
            string code = "";
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                code = mgr.MakeOfferCode_V2040(idOfferType, idCustomer, connectionString, offerCodeConnectionString, mainServerConnectionString, idUser, UseSQLCounter);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return code;
        }


        public List<Company> GetAllCompaniesDetails_V2040(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2040(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }


        public Offer GetOfferDetailsById_V2040(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2040(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, activeSite, Properties.Settings.Default.WorkingOrdersPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public LostReasonsByOffer GetLostReasonsByOffer_V2040(Int64 idOffer)
        {
            LostReasonsByOffer lostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lostReasonsByOffer = mgr.GetLostReasonsByOffer_V2040(idOffer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lostReasonsByOffer;
        }


        public Offer AddOffer_V2040(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2040(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public Offer UpdateOffer_V2040(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2040(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        public Offer AddOfferWithIdSourceOffer_V2040(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOfferWithIdSourceOffer_V2040(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public bool AddLogEntryByOffer_V2040(LogEntryByOffer logEntryByOffer)
        {
            bool isInserted = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isInserted = mgr.AddLogEntryByOffer_V2040(logEntryByOffer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isInserted;
        }


        public List<LogEntryByOffer> GetAllCommentsByIdOffer_V2040(Int64 idOffer)
        {
            List<LogEntryByOffer> logEntries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntries = mgr.GetAllCommentsByIdOffer(idOffer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntries;
        }

        public List<LogEntryByOffer> GetAllLogEntriesByIdOffer_V2040(Int64 idOffer)
        {
            List<LogEntryByOffer> logEntries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntries = mgr.GetAllLogEntriesByIdOffer(idOffer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntries;
        }


        public List<OfferContact> GetOfferContact_V2040(Int64 idOffer)
        {
            List<OfferContact> offerContacts = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offerContacts = mgr.GetOfferContact_V2040(idOffer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerContacts;
        }


        public bool UpdateOfferForParticularColumn_V2040(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn_V2040(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public List<Shipment> GetAllShipmentsByOfferId_V2040(Int64 idOffer)
        {
            List<Shipment> shipments = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                shipments = mgr.GetAllShipmentsByOfferId_V2040(connectionString, idOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return shipments;
        }

        public List<PackingBox> GetAllPackingBoxesByShipmentId_V2040(Int64 idShipment)
        {
            List<PackingBox> packingBoxes = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                packingBoxes = mgr.GetAllPackingBoxesByShipmentId_V2040(connectionString, idShipment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return packingBoxes;
        }

        public ActionPlan GetActionPlan(Int64 IdActionPlan)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlan(IdActionPlan, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<ActionPlanItem> GetSalesActivityRegister(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public ActionPlanItem GetActionPlanItem(Int64 IdActionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItem(IdActionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ActionPlanItem AddActionPlanItem(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActionPlanItem(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool UpdateActionPlanItem(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItem(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool DeleteActionPlanItem(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteActionPlanItem(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        public List<LogEntriesByActionItem> AddUpdateDeleteLogEntriesByActionItem(Int64 idActionPlanItem, List<LogEntriesByActionItem> logEntriesByActionItems)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdateDeleteLogEntriesByActionItem(idActionPlanItem, logEntriesByActionItems, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<People> GetActionPlanItemResponsible(Int32 idSite)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItemResponsible(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public bool UpdateParticluarColumnActionPlanItem(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateParticluarColumnActionPlanItem(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public IList<LookupValue> GetActionsCountOfStatus(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionsCountOfStatus(connectionString, idsSelectedUser, idsSelectedPlant, idUser, idPermission, accountingYearFrom, accountingYearTo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<TimelineGrid> GetOfferAndOrderDetailsByOfferCode(string offerCode, TimelineParams timelineParams)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetOfferAndOrderDetailsByOfferCode(offerCode, timelineParams, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public Offer AddOffer_V2041(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2041(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public Offer UpdateOffer_V2041(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2041(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public List<EngineeringAnalysisType> GetEngAnalysisArticles_V2041()
        {

            try
            {
                CrmManager mgr = new CrmManager();

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetEngAnalysisArticles_V2041(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<ActivityGrid> GetActivitiesDetail(ActivityParams objActivityParams)
        {

            try
            {
                CrmManager mgr = new CrmManager();

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActivitiesDetail(connectionString, objActivityParams);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool SaveCopyOfEMail(Offer offer, FileDetail emailFileDetail)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                return mgr.SaveCopyOfEMail(offer, emailFileDetail, Properties.Settings.Default.CommercialRfqPath);
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

            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<TimelineGrid> GetOpportunities(TimelineParams timelineParams)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetOpportunities(timelineParams, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public PeopleDetails GetGroupPlantByMailId(string mailId)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetGroupPlantByMailId(connectionString, mailId);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public Offer GetOfferDetailsById_V2042(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2042(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, activeSite, Properties.Settings.Default.WorkingOrdersPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        public Offer UpdateOffer_V2042(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2042(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        public EngineeringAnalysis GetEngAnalysisByOfferCode(Offer offer)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetEngAnalysisByOfferCode(offer, connectionString, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }



        public bool UpdateAttachActivity(Activity activity)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateAttachActivity(activity, connectionString, Properties.Settings.Default.ActivityAttachmentsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public List<EngineeringAnalysisArticleDetail> GetEngineeringAnalysisArticleDetail(string offerCode)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetEngineeringAnalysisArticleDetail(offerCode, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool UpdateAttachOffer(Offer offer)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateAttachOffer(offer, connectionString, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<ActionPlanItem> GetSalesActivityRegister_V2043(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister_V2043(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<ActionPlanItem> GetSalesActivityRegister_V2051(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister_V2051(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public ActionPlanItem GetActionPlanItem_V2043(Int64 IdActionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItem_V2043(IdActionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ActionPlanItem AddActionPlanItem_V2043(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActionPlanItem_V2043(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool UpdateActionPlanItem_V2043(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItem_V2043(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool UpdateActionPlanItemForGrid(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItemForGrid(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public Company AddCompany_V2043(Company company, Company emdepSite = null)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.AddCompany_V2043(company, mainconnectionString, crmconnectionString, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }
        public List<ActionPlanItem> GetAppointmentActivities(ActivityParams objActivityParams)
        {
            List<ActionPlanItem> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetAppointmentActivities(connectionString, objActivityParams);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public Company UpdateCompany_V2043(Company company)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany_V2043(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        public List<SalesUser> GetAllSalesUserQuotaPeopleDetails_V2045(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear)
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserQuotaPeopleDetails_V2045(idCurrency, connectionString, connectionWorkbenchString, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }

        public Offer AddOffer_V2045(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2045(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public ActionPlanItem AddActionPlanItem_V2080(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActionPlanItem_V2080(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool UpdateActionPlanItem_V2080(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItem_V2080(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<ActionPlanItem> GetSalesActivityRegister_V2080(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister_V2080(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public ActionPlanItem GetActionPlanItem_V2080(Int64 IdActionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItem_V2080(IdActionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public Offer GetOfferDetailsById_V2090(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2090(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, activeSite, Properties.Settings.Default.WorkingOrdersPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public Offer AddOffer_V2090(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2090(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public Offer UpdateOffer_V2090(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2090(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public bool UpdateOfferForParticularColumn_V2090(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn_V2090(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public Offer AddOfferWithIdSourceOffer_V2090(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOfferWithIdSourceOffer_V2090(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }

        public Offer AddOfferWithIdSourceOffer_V2170(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOfferWithIdSourceOffer_V2170(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public bool UpdateOrderByIdOffer_V2090(Offer offer)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderByIdOffer_V2090(offer, connectionString, mainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public bool UpdateOrderForParticularColumn_V2090(Offer offer)
        {

            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderForParticularColumn_V2090(offer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public List<SalesUser> GetAllSalesUserQuotaPeopleDetails_V2110(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear)
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserQuotaPeopleDetails_V2110(idCurrency, connectionString, connectionWorkbenchString, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }

        public bool AddSaleUser_V2110(SalesUser salesUser)
        {
            bool isAddedSalesUsers = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isAddedSalesUsers = mgr.AddSaleUser_V2110(salesUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isAddedSalesUsers;
        }

        public bool UpdateSaleUser_V2110(SalesUser salesUser)
        {
            bool isUpdatedSalesUsers = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdatedSalesUsers = mgr.UpdateSaleUser_V2110(salesUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdatedSalesUsers;
        }


        public IList<Offer> GetSalesStatus_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatus_V2110(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<Offer> GetOfferLeadSourceCompanyWise_V2110(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferLeadSourceCompanyWise_V2110(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_V2110(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<LostReasonsByOffer> LstLostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                LstLostReasonsByOffer = mgr.GetOfferLostReasonsDetailsCompanyWise_V2110(connectionString, idCurrency, idActiveUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstLostReasonsByOffer;
        }

        public List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false)
        {
            List<OfferMonthDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthWithTarget_V2110(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser, isSiteTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        public CustomerTargetDetail GetTop5CustomersDashboardDetails_V2110(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false)
        {
            CustomerTargetDetail customerTargetDetail = new CustomerTargetDetail();
            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customerTargetDetail = mgr.GetTop5CustomersDashboardDetails_V2110(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, assignedPlant, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customerTargetDetail;
        }


        public List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2110(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser)
        {
            List<SalesUserWon> salesUserWons = new List<SalesUserWon>();

            try
            {
                CrmManager mgr = new CrmManager();
                salesUserWons = mgr.GetAllSalesTeamUserWonDetail_V2110(company, idCurrency, idUser, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserWons;
        }


        public BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_V2110(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false)
        {

            BusinessUnitTargetDetail businessUnitTargetDetail = null;

            try
            {
                CrmManager mgr = new CrmManager();

                businessUnitTargetDetail = mgr.GetBusinessUnitStatusWithTarget_V2110(idCurrency, assignedPlant, accountingYearFrom, accountingYearTo, idCurrentUser, company, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return businessUnitTargetDetail;
        }

        public List<Offer> GetSelectedOffersEngAnalysisDateWise_V2110(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetSelectedOffersEngAnalysisDateWise_V2110(idUser, idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        public List<Offer> GetOffersEngineeringAnalysis_V2110(byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersEngineeringAnalysis_V2110(idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, compdetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public IList<Offer> GetOffersPipeline_V2110(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOffersPipeline_V2110(idCurrency, userids, idCurrentUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOfferLeadSourceCompanyWise_V2110(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise_V2110(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<LostReasonsByOffer> lostReasonsByOffers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lostReasonsByOffers = mgr.GetSelectedUsersOfferLostReasonsDetailsCompanyWise_V2110(connectionString, idCurrency, accountingYearFrom, accountingYearTo, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lostReasonsByOffers;
        }

        public List<Offer> GetOffersEngineeringAnalysisByPermission_V2110(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission_V2110(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        public List<OfferDetail> GetSalesStatusWithTarget_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusWithTarget_V2110(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        public List<OfferDetail> GetSalesStatusByMonthAllPermission_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthAllPermission_V2110(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        public List<TempAppointment> GetDailyEvents_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<TempAppointment> modelAppointments = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetDailyEvents_V2110(idCurrency, idUser, accountingYearFrom, accountingYearTo, compdetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }

        public IList<Customer> GetDashboard2Details_V2110(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetDashboard2Details_V2110(idCurrency, idCurrentUser, idsUser, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        public List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise_V2110(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferQuantitySalesStatusByMonthCompanyWise_V2110(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<Offer> GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise_V2110(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise_V2110(idCurrency, accountingYearFrom, accountingYearTo, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public ActionPlanItem AddActionPlanItem_V2120(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActionPlanItem_V2120(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public List<ActionPlanItem> GetSalesActivityRegister_V2120(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister_V2120(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ActionPlanItem GetActionPlanItem_V2120(Int64 IdActionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItem_V2120(IdActionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateActionPlanItem_V2120(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItem_V2120(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }



        public bool UpdateOrderForParticularColumn_V2120(Offer offer)
        {

            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderForParticularColumn_V2120(offer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public bool UpdateOfferForParticularColumn_V2120(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn_V2120(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public Offer GetOfferDetailsById_V2120(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2120(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, activeSite, Properties.Settings.Default.WorkingOrdersPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        public List<Detection> GetAllDetectionForCreateColumn()
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllDetectionForCreateColumn(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public List<Company> GetAllCompaniesDetailsWithServiceProvider()
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompaniesDetailsWithServiceProvider(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool IsDeletedOfferDetails(UInt64 idOffer)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsDeletedOfferDetails(idOffer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130(connectionString, idUser, idCurrency, accountingYearFrom, accountingYearTo, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }

        public List<Company> GetAllCompaniesDetails_V2130(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2130(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }

        public List<Offer> GetModulesReportDetails_V2130(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetModulesReportDetails_V2130(idActiveUser, company, fromDate, toDate, idCompany, idTemplate, idCPType, idDetections, idCurrency, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }


        public List<Offer> GetArticlesReportDetails_V2130(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails_V2130(connectionString, idActiveUser, company, fromDate, toDate, idCurrency, idArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }


        public bool UpdateOfferForParticularColumn_V2130(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn_V2130(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public bool UpdateCurrencyConversionListDaily_V2140(List<DailyCurrencyConversion> dailyCurrencyConversionList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCurrencyConversionListDaily_V2140(connectionString, dailyCurrencyConversionList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Offer AddOffer_V2140(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2140(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }

        public Offer AddOffer_V2170(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2170(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public Offer UpdateOffer_V2140(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2140(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }






        public bool CreateFolderOffer_V2140(Offer offer, bool? isNewFolderForOffer = null)
        {
            bool isCreated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                isCreated = mgr.CreateFolderOffer_V2140(offer, connectionString, offer.Site.ConnectPlantId, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, workbenchConnectionString, isNewFolderForOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isCreated;
        }


        public List<EngineeringAnalysis> GetEngAnalysisAllRevisionsByOfferCode(Offer offer)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetEngAnalysisAllRevisionsByOfferCode(offer, connectionString, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public List<Offer> GetOffersByIdCustomerToLinkedWithActivities_V2150(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company company, Int32 idUserPermission, Int32 idCustomer)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersByIdCustomerToLinkedWithActivities_V2150(idCurrency, idUser, accountingYearFrom, accountingYearTo, company, idUserPermission, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }


        //GEOS2-3144 IESD-24539 CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM
        public List<QuotationMatrixTemplate> GetAllQuotationMatrixTemplates_V2160()
        {
            List<QuotationMatrixTemplate> list = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                CrmManager mgr = new CrmManager();
                list = mgr.GetAllQuotationMatrixTemplates_V2160(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return list;
        }

        //GEOS2-3145 IESD-24539 CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM
        public QuotationMatrixTemplate AddQuotationMatrix_V2160(QuotationMatrixTemplate quotationMatrixTemplate)
        {
            // bool isAdded = false;
            // QuotationMatrixTemplate quotationMatrixTemplate;
            try
            {
                CrmManager crmManager = new CrmManager();
                string connectionString =
                    ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                quotationMatrixTemplate = crmManager.AddQuotationMatrix_V2160(quotationMatrixTemplate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return quotationMatrixTemplate;
        }

        //GEOS2-3145 IESD-24539 CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM
        public bool UpdateQuotationMatrix_V2160(QuotationMatrixTemplate quotationMatrixTemplate)
        {
            bool isUpdated = false;
            try
            {
                CrmManager crmManager = new CrmManager();
                string connectionString =
                    ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = crmManager.UpdateQuotationMatrix_V2160(quotationMatrixTemplate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }



        //[GEOS2-3144] [GEOS2-3145] [GEOS2-3185] [YJoshi]
        //[CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM - Add MartixList And load the grid details in Configuration section]

        /// <summary>
        /// Get All Customers Details 
        /// </summary>
        /// <returns>All Customers Details</returns>
        public List<Customer> GetAllCustomersDetails_V2160()
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                var connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetAllCustomersDetails_V2160(connectionString);
            }
            catch (Exception ex)
            {
                var exp = new ServiceException
                {
                    ErrorMessage = ex.Message,
                    ErrorDetails = ex.ToString()
                };
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        public bool UpdateCurrencyConversionListDaily_V2190(List<DailyCurrencyConversion> dailyCurrencyConversionList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCurrencyConversionListDaily_V2190(connectionString, dailyCurrencyConversionList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Offer GetOfferDetailsById_V2200(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2200(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, activeSite, Properties.Settings.Default.WorkingOrdersPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public bool UpdateOrderForParticularColumn_V2200(Offer offer)
        {

            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderForParticularColumn_V2200(offer, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public bool UpdateOfferForParticularColumn_V2200(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn_V2200(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        //GEOS2-1900
        public List<Offer> GetArticlesReportDetails_V2220(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails_V2220(connectionString, idActiveUser, company, fromDate, toDate, idCurrency, idArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        //GEOS2-1900
        public List<Offer> GetModulesReportDetails_V2220(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetModulesReportDetails_V2220(idActiveUser, company, fromDate, toDate, idCompany, idTemplate, idCPType, idDetections, idCurrency, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        public Offer UpdateOffer_V2270(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2270(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        //GEOS2-243
        public List<CarOEM> GetAllCarOEM()
        {
            List<CarOEM> CarOEMs = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                CarOEMs = mgr.GetAllCarOEM(connectionString, Properties.Settings.Default.CaroemFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return CarOEMs;
        }

        //GEOS2-244
        public bool InsertCarOEM(byte[] ImageBytes, string Name)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.InsertCarOEM(ImageBytes, Name, connectionString, Properties.Settings.Default.CaroemFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //GEOS2-245
        public bool UpdateCarOEM(byte[] CarByteImage, int CarId, string PrevCarOEMName, string CarOEMName)
        {
            bool isupdated = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isupdated = mgr.UpdateCarOEM(CarByteImage, CarId, PrevCarOEMName, CarOEMName, connectionString, Properties.Settings.Default.CaroemFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isupdated;
        }

        //[rdixit][29.06.2022][GEOS2-3514]
        public IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferPipelineDatewise_IgnoreOfferCloseDate(idCurrency, userids, idCurrentUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        //[rdixit][29.06.2022][GEOS2-3515]
        public List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusWithTarget_IgnoreOfferCloseDate(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        //[rdixit][30.06.2022][GEOS2-3515]
        public List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        //[rdixit][30.06.2022][GEOS2-3515]
        public List<TempAppointment> GetDailyEvents_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<TempAppointment> modelAppointments = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetDailyEvents_IgnoreOfferCloseDate(idCurrency, idUser, accountingYearFrom, accountingYearTo, compdetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }

        //[rdixit][30.06.2022][GEOS2-3517] 
        public IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetDashboard2Details_IgnoreOfferCloseDate(idCurrency, idCurrentUser, idsUser, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        //[rdixit][30.06.2022][GEOS2-3518]
        public List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false)
        {
            List<OfferMonthDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser, isSiteTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        //[rdixit][30.06.2022][GEOS2-3518]
        public CustomerTargetDetail GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false)
        {
            CustomerTargetDetail customerTargetDetail = new CustomerTargetDetail();
            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customerTargetDetail = mgr.GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, assignedPlant, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customerTargetDetail;
        }

        //[rdixit][30.06.2022][GEOS2-3518]
        public BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false)
        {

            BusinessUnitTargetDetail businessUnitTargetDetail = null;

            try
            {
                CrmManager mgr = new CrmManager();

                businessUnitTargetDetail = mgr.GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate(idCurrency, assignedPlant, accountingYearFrom, accountingYearTo, idCurrentUser, company, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return businessUnitTargetDetail;
        }



        //[rdixit][GEOS2-3519][01.07.2022]
        public List<Offer> GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate(idUser, idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        //[rdixit][GEOS2-3519][01.07.2022]
        public List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }


        //[rdixit][Ignore the �To� date in the �DASHBOARD-> DASHBOARD3� section for Opportunities info][01.07.2022]
        public IList<Offer> GetSalesStatus_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatus_IgnoreOfferCloseDate(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        //[rdixit][Ignore the �To� date in the �DASHBOARD-> DASHBOARD3� section for Opportunities info][01.07.2022]
        public List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersOfferLeadSourceCompanyWise_IgnoreOfferCloseDate(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        //[rdixit][Ignore the �To� date in the �DASHBOARD-> DASHBOARD3� section for Opportunities info][01.07.2022]
        public List<Offer> GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        //[rdixit][Ignore the �To� date in the �DASHBOARD-> DASHBOARD3� section for Opportunities info][01.07.2022]
        public List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<LostReasonsByOffer> LstLostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                LstLostReasonsByOffer = mgr.GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate(connectionString, idCurrency, idActiveUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstLostReasonsByOffer;
        }

        //[rdixit][Ignore the �To� date in the �DASHBOARD-> DASHBOARD3� section for Opportunities info][01.07.2022]
        public List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<LostReasonsByOffer> lostReasonsByOffers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lostReasonsByOffers = mgr.GetSelectedUsersOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate(connectionString, idCurrency, accountingYearFrom, accountingYearTo, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lostReasonsByOffers;
        }

        //[rdixit][GEOS2-2911][04.07.2022]
        public bool UpdateOrderByIdOffer_V2290(Offer offer)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderByIdOffer_V2290(offer, connectionString, mainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        //[rdixit][GEOS2-2909][05.07.2022]
        public Offer AddOffer_V2290(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2290(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }

        //[001][kshinde][GEOS2-2910][05.07.2022]
        public Offer UpdateOffer_V2290(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2290(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        //[001][kshinde][GEOS2-2910][05.07.2022]

        public Offer GetOfferDetailsById_V2290(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite)
        {
            Offer offer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offer = mgr.GetOfferDetailsById_V2290(idOffer, idUser, idCurrency, accountingYearFrom, accountingYearTo, activeSite, Properties.Settings.Default.WorkingOrdersPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        //[GEOS2-2312][12.08.2022][rdixit]
        public List<People> GetContactsByIdPermission_V2300(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2300(connectionString, idActiveUser, idUser, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  02 Sep 2022
        public IList<Offer> GetSelectedUsersTargetForecastDate_V2301(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersTargetForecastDate_V2301(connectionString, idCurrency, idUser, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  02 Sep 2022
        public IList<Offer> GetTargetForecastByPlantDate_V2301(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission, string idSite)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecastByPlantDate_V2301(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        // shubham[skadam] GEOS2-3158 Improvement in Target & forecast  (better visibility) - 1  02 Sep 2022
        public IList<Offer> GetTargetForecastNewCurrencyConversion_V2301(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecastNewCurrencyConversion_V2301(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<SalesUser> GetAllSalesUserQuotaPeopleDetails_V2301(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear)
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUserQuotaPeopleDetails_V2301(idCurrency, connectionString, connectionWorkbenchString, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUsers;
        }

        public IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2320(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;
            try
            {
                CrmManager mgr = new CrmManager();
                customers = mgr.GetDashboard2Details_IgnoreOfferCloseDate_V2320(idCurrency, idCurrentUser, idsUser, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }


        //[GEOS2-3994][rdixit][17.11.2022]
        public Company UpdateCompany_V2340(Company company)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany_V2340(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }
        //[GEOS2-3994][rdixit][17.11.2022]
        public Company AddCompany_V2340(Company company, Company emdepSite = null)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.AddCompany_V2340(company, mainconnectionString, crmconnectionString, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        // shubham[skadam]GEOS2-4052 Add a new column �PO Date� in Articles Report 30 11 2022
        public List<Offer> GetArticlesReportDetails_V2340(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails_V2340(connectionString, idActiveUser, company, fromDate, toDate, idCurrency, idArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        //[rdixit][GEOS2-3871][09.01.2023]
        public string GetCurrentSiteTimeZone(string siteName)
        {
            string CurrentSiteTimeZone = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                CurrentSiteTimeZone = mgr.GetCurrentSiteTimeZone(siteName, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return CurrentSiteTimeZone;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public bool IsExistCarProject_V2350(string Name)
        {
            bool isExist = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isExist = mgr.IsExistCarProject_V2350(connectionString, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isExist;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public Double GetOfferMaxValue_V2350()
        {
            Double max_value = 0;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                max_value = mgr.GetOfferMaxValue_V2350(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return max_value;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public bool IsExistTagName_V2350(string Name)
        {
            bool isExist = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isExist = mgr.IsExistTagName_V2350(connectionString, Name);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isExist;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public bool IsExistCustomer_V2350(string customerName, byte idCustomerType)
        {
            bool isExist = false;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isExist = mgr.IsExistCustomer_V2350(connectionString, customerName, idCustomerType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isExist;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public Int64 GetNextNumberOfOfferFromCounters_V2350(byte? idOfferType, Int32 idUser)
        {
            Int64 nextNumber = 0;
            try
            {
                CrmManager mgr = new CrmManager();
                string mainserverconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                nextNumber = mgr.GetNextNumberOfOfferFromCounters_V2350(idOfferType, mainserverconnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return nextNumber;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public Double GetOfferMaxValueById_V2350(Int16 idMaxValue)
        {
            Double max_value = 0;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                max_value = mgr.GetOfferMaxValueById_V2350(connectionString, idMaxValue);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return max_value;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public string MakeOfferCode_V2350(byte? idOfferType, Int32 idCustomer, Int32 idUser)
        {
            string code = "";
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                code = mgr.MakeOfferCode_V2350(idOfferType, idCustomer, connectionString, offerCodeConnectionString, mainServerConnectionString, idUser, UseSQLCounter);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return code;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public List<Activity> GetActivitiesForActivityReminder_V2350()
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesForActivityReminder_V2350(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public List<Activity> GetActivitiesGoingToDueInInverval_V2350(string Interval)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesGoingToDueInInverval_V2350(connectionString, Interval);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return activities;
        }

        //[GEOS2-4120][rdixit][10.01.2023]
        public List<SalesUser> GetAllSalesUsersForReport_V2350()
        {
            List<SalesUser> salesUsers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                salesUsers = mgr.GetAllSalesUsersForReport_V2350(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return salesUsers;
        }

        //[GEOS2-4128][rdixit][11.01.2023]
        public List<Offer> GetModulesReportDetails_V2350(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetModulesReportDetails_V2350(idActiveUser, company, fromDate, toDate, idCompany, idTemplate, idCPType, idDetections, idCurrency, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        //[plahange][7/2/2023] GEOS2-4146
        public List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2360(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission)
        {
            List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuota = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                plantBusinessUnitSalesQuota = mgr.GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2360(connectionString, idUser, idCurrency, accountingYearFrom, accountingYearTo, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return plantBusinessUnitSalesQuota;
        }

        //[rdixit][GEOS2-2358][09.02.2023]
        public bool CreateFolderOffer_V2360(Offer offer, bool? isNewFolderForOffer = null)
        {
            bool isCreated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                isCreated = mgr.CreateFolderOffer_V2360(offer, connectionString, offer.Site.ConnectPlantId, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, workbenchConnectionString, isNewFolderForOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isCreated;
        }

        //[rdixit][GEOS2-4127][10.02.2023]
        public List<ModuleFamily> GetModuleFamilies()
        {
            List<ModuleFamily> families = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                families = mgr.GetModuleFamilies(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return families;
        }

        //[rdixit][GEOS2-4127][10.02.2023]
        public List<Offer> GetModulesReportDetails_V2360(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany,
            byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, byte? idfamily, byte? idsubFamily, Int32? idCustomer = null)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetModulesReportDetails_V2360(idActiveUser, company, fromDate, toDate, idCompany, idTemplate, idCPType, idDetections, idCurrency, idfamily, idsubFamily, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        //[rdixit][01.03.2023][GEOS2-4219]
        public List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2360(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2360(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        //[rdixit][01.03.2023][GEOS2-4219]
        public List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2360(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2360(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        //[rdixit][GEOS2-4222][01.03.2023]
        public IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2360(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2360(idCurrency, userids, idCurrentUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        //[rdixit][GEOS2-4224][01.03.2023]
        public CustomerTargetDetail GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate_V2360(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false)
        {
            CustomerTargetDetail customerTargetDetail = new CustomerTargetDetail();
            try
            {
                CrmManager mgr = new CrmManager();
                customerTargetDetail = mgr.GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate_V2360(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, assignedPlant, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customerTargetDetail;
        }

        //[rdixit][GEOS2-4224][01.03.2023]
        public BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate_V2360(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false)
        {

            BusinessUnitTargetDetail businessUnitTargetDetail = null;

            try
            {
                CrmManager mgr = new CrmManager();

                businessUnitTargetDetail = mgr.GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate_V2360(idCurrency, assignedPlant, accountingYearFrom, accountingYearTo, idCurrentUser, company, isTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return businessUnitTargetDetail;
        }

        //[rdixit][GEOS2-4224][01.03.2023]
        public List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2360(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser)
        {
            List<SalesUserWon> salesUserWons = new List<SalesUserWon>();

            try
            {
                CrmManager mgr = new CrmManager();
                salesUserWons = mgr.GetAllSalesTeamUserWonDetail_V2360(company, idCurrency, idUser, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserWons;
        }

        //[rdixit][GEOS2-4224][01.03.2023]
        public List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate_V2360(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false)
        {
            List<OfferMonthDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate_V2360(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser, isSiteTarget);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        //[rdixit][GEOS2-4223][03.03.2023]
        public IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2360(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;
            try
            {
                CrmManager mgr = new CrmManager();
                customers = mgr.GetDashboard2Details_IgnoreOfferCloseDate_V2360(idCurrency, idCurrentUser, idsUser, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        //[pmisal][GEOS2-4323][10.04.2023]        
        public List<Company> GetSelectedUserCustomersByPlant_V2380(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2380(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[rdixit][GEOS2-4324][11.04.2023]
        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2380(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2380(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[rdixit][GEOS2-4324][11.04.2023]
        public List<Company> GetCustomersBySalesOwnerId_V2380(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2380(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[rdixit][GEOS2-4324][17.04.2023]
        public List<Country> GetAllCountriesDetails()
        {
            List<Country> Countries = new List<Country>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Countries = mgr.GetAllCountriesDetails(connectionString, Properties.Settings.Default.CountryFilePath);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Countries;
        }

        //[GEOS2-4327][rdixit][18.04.2023]
        public List<ActivitiesRecurrence> GetActivitiesRecurrence_V2380(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActivitiesRecurrence_V2380(connectionString, idUser, accountingYearFrom, accountingYearTo, idSite, idPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4327][rdixit][26.04.2023]
        public List<People> GetContactsByIdPermission_V2380(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2380(connectionString, idActiveUser, idUser, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }
        //[GEOS2-4327][rdixit][26.04.2023]
        public IList<Offer> GetSelectedUsersTargetForecastDate_V2380(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetSelectedUsersTargetForecastDate_V2380(connectionString, idCurrency, idUser, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        //[GEOS2-4327][rdixit][26.04.2023]
        public IList<Offer> GetTargetForecastByPlantDate_V2380(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission, string idSite)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecastByPlantDate_V2380(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        //[GEOS2-4327][rdixit][26.04.2023]
        public IList<Offer> GetTargetForecastNewCurrencyConversion_V2380(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetTargetForecastNewCurrencyConversion_V2380(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }
        //[rdixit][GEOS2-4372][27.04.2023]
        public ActionPlanItem AddActionPlanItem_V2380(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActionPlanItem_V2380(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        //[rdixit][GEOS2-4372][27.04.2023]
        public bool UpdateActionPlanItem_V2380(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItem_V2380(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][GEOS2-4372][27.04.2023]
        public bool UpdateActionPlanItemForGrid_V2380(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItemForGrid_V2380(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        #region V2390
        //[GEOS2-4278][rdixit][04.05.2023]
        public List<People> GetContactsByIdPermission_V2390(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2390(connectionString, idActiveUser, idUser, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }
        //[GEOS2-4278][rdixit][04.05.2023]
        public List<People> GetSalesOwnerBySiteId_V2390(Int32 idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSalesOwnerBySiteId_V2390(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }
        //[GEOS2-4278][rdixit][04.05.2023]
        public Company UpdateCompany_V2390(Company company)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany_V2390(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        //[GEOS2-4279][rdixit][05.05.2023]
        public List<Company> GetSelectedUserCustomersByPlant_V2390(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2390(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[GEOS2-4279][rdixit][05.05.2023]
        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2390(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2390(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[GEOS2-4279][rdixit][05.05.2023]
        public List<Company> GetCustomersBySalesOwnerId_V2390(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2390(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
        //[GEOS2-4279][rdixit][05.05.2023]
        public Company AddCompany_V2390(Company company, Company emdepSite = null)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.AddCompany_V2390(company, mainconnectionString, crmconnectionString, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        //[GEOS2-4283][rdixit][10.05.2023]
        public List<ActivitiesRecurrence> GetActivitiesRecurrence_V2390(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActivitiesRecurrence_V2390(connectionString, idUser, accountingYearFrom, accountingYearTo, idSite, idPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4274][22.05.2023]
        public List<Regions> GetRegionsByGroupAndCountryAndSites(int IdGroup, string CountryNames, string SiteNames)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRegionsByGroupAndCountryAndSites(connectionString, IdGroup, CountryNames, SiteNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4274][22.05.2023]
        public List<Country> GetCountriesByGroupAndRegionAndSites(int IdGroup, string RegionNames, string SiteNames)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetCountriesByGroupAndRegionAndSites(connectionString, IdGroup, RegionNames, SiteNames);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4274][22.05.2023]
        public List<SitesWithCustomer> GetCustomerWithSites()
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetCustomerWithSites(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4274][22.05.2023]
        public ActionPlanItem ActionLauncherAdd(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.ActionLauncherAdd(actionPlanItem, customerSiteList, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4274][22.05.2023]
        public List<SitesWithCustomer> GetSitesBySalesOwner(int idSalesOwner)
        {
            List<SitesWithCustomer> sites = new List<SitesWithCustomer>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                sites = mgr.GetSitesBySalesOwner(connectionString, idSalesOwner);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return sites;
        }

        //[rdixit][GEOS2-4274][25.05.2023]
        public List<People> GetActivePeoplesBySiteList(string idSiteList)
        {
            List<People> ActivePeoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ActivePeoples = mgr.GetActivePeoplesBySiteList(connectionString, idSiteList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ActivePeoples;
        }

        //TO get only IsStillActive and IsEnabled Users by rdixit 31.05.2023
        public List<People> GetAllActivePeoples_V2390()
        {
            List<People> ActivePeoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ActivePeoples = mgr.GetAllActivePeoples_V2390(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ActivePeoples;
        }
        #endregion

        //[rdixit][GEOS2-4652][11.07.2023]
        public List<SitesWithCustomer> GetCustomerWithSites_V2410()
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetCustomerWithSites_V2410(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4655][18.07.2023]
        public List<People> GetAllActivePeoplesWithSites()
        {
            List<People> ActivePeoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ActivePeoples = mgr.GetAllActivePeoplesWithSites(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ActivePeoples;
        }

        //[rdixit][GEOS2-4655][18.07.2023]
        public List<SitesWithCustomer> GetWithSiteswithSalesUsers()
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWithSiteswithSalesUsers(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<People> GetActivePeoplesBySiteList_V2410(string idSiteList)
        {
            List<People> ActivePeoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ActivePeoples = mgr.GetActivePeoplesBySiteList_V2410(connectionString, idSiteList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ActivePeoples;
        }

        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2420(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2420(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[GEOS2-4279][rdixit][05.05.2023]
        public List<Company> GetCustomersBySalesOwnerId_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2420(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2420(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2420(idCurrency, userids, idCurrentUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<ActivitiesRecurrence> GetActivitiesRecurrence_V2420(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActivitiesRecurrence_V2420(connectionString, idUser, accountingYearFrom, accountingYearTo, idSite, idPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-4682][07-08-2023]
        public List<Customer> GetCompanyGroup_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetCompanyGroup_V2420(connectionString, idUser, idZone, idUserPermission, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        //[pramod.misal][GEOS2-4682][08-08-2023]
        public List<Company> GetSelectedUserCompanyPlantByIdUser_V2420(string idUser, bool isIncludeDefault = false)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCompanyPlantByIdUser_V2420(connectionString, idUser, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[pramod.misal][GEOS2-4682][08-08-2023]
        public List<Company> GetCompanyPlantByUserId_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Company> companies = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCompanyPlantByUserId_V2420(connectionString, idUser, idZone, idUserPermission, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public List<Company> GetSelectedUserCustomersByPlant_V2420(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2420(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public List<Customer> GetSelectedUserCompanyGroup_V2420(string idUser, bool isIncludeDefault = false)
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetSelectedUserCompanyGroup_V2420(connectionString, idUser, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        //AnnualSalesPerformanceViewModel
        public List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2420(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        public List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2420(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }

        public List<TempAppointment> GetDailyEvents_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<TempAppointment> modelAppointments = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                modelAppointments = mgr.GetDailyEvents_IgnoreOfferCloseDate_V2420(idCurrency, idUser, accountingYearFrom, accountingYearTo, compdetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return modelAppointments;
        }

        //DashboardSaleViewModel
        public IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2420(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;
            try
            {
                CrmManager mgr = new CrmManager();
                customers = mgr.GetDashboard2Details_IgnoreOfferCloseDate_V2420(idCurrency, idCurrentUser, idsUser, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        public List<Company> GetSitesTargetNewCurrencyConversion_V2420(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSitesTargetNewCurrencyConversion_V2420(connectionString, idCurrency, idUser, idZone, accountingFromYear, accountingToYear, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public List<Company> GetSelectedUsersSitesTargetNewCurrencyConversion_V2420(byte idCurrency, Int64 accountingFromYear, Int64 accountingToYear, string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUsersSitesTargetNewCurrencyConversion_V2420(connectionString, idCurrency, accountingFromYear, accountingToYear, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //DahsBoardPerformanceViewModel
        public IList<Offer> GetSalesStatus_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatus_IgnoreOfferCloseDate_V2420(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<Offer> GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<LostReasonsByOffer> LstLostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                LstLostReasonsByOffer = mgr.GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420(connectionString, idCurrency, idActiveUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstLostReasonsByOffer;
        }

        public List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSelectedUsersOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0)
        {
            List<LostReasonsByOffer> lostReasonsByOffers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                lostReasonsByOffers = mgr.GetSelectedUsersOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420(connectionString, idCurrency, accountingYearFrom, accountingYearTo, idUser, companyDetails, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lostReasonsByOffers;
        }

        //DashboardEngineeringAnalysisViewModel
        public List<Offer> GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2420(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2420(idUser, idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        public List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2420(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2420(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }


        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        public Company UpdateCompany_V2430(Company company)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany_V2430(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }


        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        public Company GetCompanyDetailsById_V2340(Int32 idSite)
        {
            Company company = new Company();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                company = mgr.GetCompanyDetailsById_V2340(connectionString, idSite);
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


        //[Sudhir.Jangra][GEOs2-4663][28/08/2023]
        public Company AddCompany_V2430(Company company, Company emdepSite = null)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.AddCompany_V2430(company, mainconnectionString, crmconnectionString, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        //[Sudhir.jangra][GEOS2-4664]

        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2430(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2430(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[Sudhir.Jangra][GEOS2-4664]
        public List<Company> GetSelectedUserCustomersByPlant_V2430(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2430(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[Sudhir.Jangra][GEOS2-4664]
        public List<Company> GetCustomersBySalesOwnerId_V2430(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2430(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[pramod.misal][GEOS2-4688][27.09.2023]
        public List<UserManagerDtl> GetSalesUserByPlant_V2440(string idPlants)
        {
            List<UserManagerDtl> userManagers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                userManagers = mgr.GetSalesUserByPlant_V2440(idPlants, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return userManagers;
        }
        //[rajashri.telvekar][GEOS2-4689]
        public List<People> GetAllAttendesList_V2440(string idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetAllAttendesList_V2440(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }

        //[pramod.misal][GEOS2-4690]][29-09-2023]
        public List<User> GetSalesAndCommericalUsers_V2440()
        {
            List<User> users = new List<User>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                users = mgr.GetSalesAndCommericalUsers_V2440(connectionString);
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

        //[19.10.2023][GEOS2-4903][rdixit] 
        public List<Customer> GetCompanyGroup_V2450(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetCompanyGroup_V2450(connectionString, idUser, idZone, idUserPermission, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        //[rdixit][GEOS2-4903][27.10.2023]
        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2450(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2450(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
        //[rdixit][GEOS2-4903][27.10.2023]
        public List<People> GetSalesOwnerBySiteId_V2450(Int32 idSite)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSalesOwnerBySiteId_V2450(connectionString, idSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }
        //[rdixit][GEOS2-4903][27.10.2023]
        public List<Company> GetSelectedUserCustomersByPlant_V2450(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2450(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
        //[rdixit][GEOS2-4903][27.10.2023]
        public List<Company> GetCustomersBySalesOwnerId_V2450(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2450(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
        //rajashri [GEOS2-5014][6-12-2023]
        public List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2460(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser)
        {
            List<SalesUserWon> salesUserWons = new List<SalesUserWon>();

            try
            {
                CrmManager mgr = new CrmManager();
                salesUserWons = mgr.GetAllSalesTeamUserWonDetail_V2460(company, idCurrency, idUser, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserWons;
        }

        //Shubham[skadam] GEOS2-5041 Error when trying to close an analysis from CRM analysis window 13 12 2023 
        public Offer UpdateOffer_V2460(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2460(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        //[cpatil][11-01-2024][GEOS2-5064]
        public Offer UpdateOffer_V2480(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2480(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        #region chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        public bool UpdateActionPlanItem_V2480(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItem_V2480(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ActionPlanItem GetActionPlanItem_V2480(Int64 IdActionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItem_V2480(IdActionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ActionPlanItem> GetSalesActivityRegister_V2480(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister_V2480(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        #region [rgadhave][GEOS2-3801][17.01.2024]
        public ActionPlanItem AddActionPlanItem_V2480(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActionPlanItem_V2480(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        #endregion

        //[Sudhir.JAngra][GEOS2-5170]
        public int GetSourcePositionForLookupValue_V2480(int IdLookupKey)
        {
            int position = new int();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                position = mgr.GetSourcePositionForLookupValue_V2480(connectionString, IdLookupKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return position;
        }

        //[Sudhir.Jangra][GEOS2-5170]
        public LookupValue AddNewSourceInLookupValue_V2480(LookupValue lookupValue)
        {
            LookupValue position = new LookupValue();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                position = mgr.AddNewSourceInLookupValue_V2480(connectionString, lookupValue);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return position;
        }

        //[pramod.misal][GEOS2-5261][13.02.2024]
        public IList<Currency> GetCurrencies_V2490()
        {
            IList<Currency> currencies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                //string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                currencies = mgr.GetCurrencies_V2490(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return currencies;
        }

        //[pramod.misal][GEOS2-5347][16.02.2024]
        public List<Offer> GetOffersByIdCustomerToLinkedWithActivities_V2490(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company company, Int32 idUserPermission, Int32 idCustomer)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOffersByIdCustomerToLinkedWithActivities_V2490(idCurrency, idUser, accountingYearFrom, accountingYearTo, company, idUserPermission, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        //[cpatil][26-02-2024][GEOS2-5229]
        public List<Company> GetAllCompaniesDetails_V2490(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2490(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }

        //[cpatil][26-02-2024][GEOS2-5229]
        public Company GetCurrentPlantId_V2490(Int32 idUser)
        {
            Company site = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                site = mgr.GetCurrentPlantId_V2490(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return site;
        }


        //[cpatil][GEOS2-5348][28.02.2024]
        public Offer AddOffer_V2490(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;

                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2490(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        //[cpatil][28-02-2024][GEOS2-5348]
        public Offer UpdateOffer_V2490(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2490(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        public bool UpdateOfferForParticularColumn_V2490(Offer offer, Int32 idSite, Int32 idUser)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                // string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                isUpdated = mgr.UpdateOfferForParticularColumn_V2490(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public Offer AddOfferWithIdSourceOffer_V2490(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                //  string dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOfferWithIdSourceOffer_V2490(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        public List<Offer> GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2490(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();
                string rdDotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    rdDotProjectConnectionString = null;
                }

                LstOffer = mgr.GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2490(idUser, idCurrency, idCurrentUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        public List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2490(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();

                string rdDotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    rdDotProjectConnectionString = null;
                }
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2490(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }

        #region GEOS2-5310 Sudhir.Jangra
        public IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2490(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2490(idCurrency, userids, idCurrentUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }

        public List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2490(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2490(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }


        public List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2490(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser)
        {
            List<OfferDetail> offerDetails = null;

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();

                offerDetails = mgr.GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2490(idCurrency, idUser, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerDetails;
        }


        public IList<Offer> GetSalesStatus_IgnoreOfferCloseDate_V2490(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission)
        {
            IList<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetSalesStatus_IgnoreOfferCloseDate_V2490(idCurrency, idUser, accountingYearFrom, accountingYearTo, companydetails, idCurrentUser, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<Offer> GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2490(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2490(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offers;
        }


        public List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2490(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<LostReasonsByOffer> LstLostReasonsByOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                LstLostReasonsByOffer = mgr.GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2490(connectionString, idCurrency, idActiveUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstLostReasonsByOffer;
        }


        public IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2490(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission)
        {
            IList<Customer> customers = null;
            try
            {
                CrmManager mgr = new CrmManager();
                customers = mgr.GetDashboard2Details_IgnoreOfferCloseDate_V2490(idCurrency, idCurrentUser, idsUser, accountingYearFrom, accountingYearTo, customerLimit, companyDetails, idUserPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        #endregion

        #region chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD Linked Items section in ACTIONS:
        public ActionPlanItem AddActionPlanItem_V2500(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddActionPlanItem_V2500(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        #endregion

        #region chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        public bool UpdateActionPlanItem_V2500(ActionPlanItem actionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateActionPlanItem_V2500(actionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ActionPlanItem GetActionPlanItem_V2500(Int64 IdActionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItem_V2500(IdActionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        //[GEOS2-5445][rdixit][14.03.2024]
        public Offer AddOfferWithIdSourceOffer_V2500(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOfferWithIdSourceOffer_V2500(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }

        #region chitra.girigosavi[28/02/2024][GEOS2-3803][ACTIONS_REVIEW] Display “ACTIONS” in the “Opportunities
        public List<Activity> GetActivitiesByIdOffer_V2500(Int64 idOffer, Int32 idEmdepSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdOffer_V2500(idOffer, idEmdepSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }
        #endregion

        #region chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        public List<Activity> GetActivitiesLinkedToAccount_V2500(string idOwner, Int32 idPermission, string idPlant, Int32 idOfferAccount, Int64 idOffer, Int32 idEmdepSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesLinkedToAccount_V2500(idOwner, idPermission, idPlant, idOfferAccount, idOffer, idEmdepSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return activities;
        }

        public Offer UpdateOffer_V2500(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2500(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        #endregion

        //[GEOS2-5490][rdixit][15.03.2024]
        public ActionPlanItem ActionLauncherAdd_V2500(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.ActionLauncherAdd_V2500(actionPlanItem, customerSiteList, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-5556][27.03.2024][rdixit]
        public List<Company> GetAllCompaniesDetails_V2500(Int32 idUser)
        {
            List<Company> connectionstrings = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompaniesDetails_V2500(idUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                //exp.Logger.Log("Get an error in GetAllCompaniesDetails() Method " + exp.ErrorMessage, category: Category.Info, priority: Priority.Low);
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return connectionstrings;
        }

        #region chitra.girigosavi[20/03/2024][GEOS2-3804][ACTIONS_REVIEW] Display “ACTIONS” in the “Orders"
        public bool UpdateOrderByIdOffer_V2520(Offer offer)
        {
            bool isUpdated = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateOrderByIdOffer_V2520(offer, connectionString, mainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }
        public ActionPlanItem GetActionPlanItem_V2520(Int64 IdActionPlanItem)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItem_V2520(IdActionPlanItem, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        public List<Activity> GetActivitiesByIdOffer_V2520(Int64 idOffer, Int32 idEmdepSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdOffer_V2520(idOffer, idEmdepSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        #region chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        public List<Activity> GetActivitiesLinkedToAccount_V2520(string idOwner, Int32 idPermission, string idPlant, Int32 idOfferAccount, Int64 idOffer, Int32 idEmdepSite)
        {
            List<Activity> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesLinkedToAccount_V2520(idOwner, idPermission, idPlant, idOfferAccount, idOffer, idEmdepSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return activities;
        }

        public Offer UpdateOffer_V2520(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2520(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }

        #endregion


        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]
        public List<CpType> GetAllCpTypes_V2530()
        {
            List<CpType> cpTypes = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                cpTypes = mgr.GetAllCpTypes_V2530(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return cpTypes;
        }

        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]

        public List<CpType> GetCpTypesByTemplate_V2530(byte idTemplate)
        {
            List<CpType> cpTypes = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                cpTypes = mgr.GetCpTypesByTemplate_V2530(connectionString, idTemplate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return cpTypes;
        }

        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]

        public List<Detection> GetDetectionByCpTypeAndTemplate_V2530(byte idTemplate, long idCPType)
        {
            List<Detection> detections = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                detections = mgr.GetDetectionByCpTypeAndTemplate_V2530(connectionString, idTemplate, idCPType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return detections;
        }

        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]
        public List<Offer> GetModulesReportDetails_V2530(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany,
          byte? idTemplate, long? idCPType, string idDetections, byte idCurrency, byte? idfamily, byte? idsubFamily, Int32? idCustomer = null)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetModulesReportDetails_V2530(idActiveUser, company, fromDate, toDate, idCompany, idTemplate, idCPType, idDetections, idCurrency, idfamily, idsubFamily, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }
        // [nsatpute][23-06-2024] GEOS2-5701
        public bool InsertImportedAccounts(List<Company> lstCompanies, Company emdepSite)
        {
            bool result = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                result = mgr.InsertImportedAccounts(connectionString, lstCompanies, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return result;
        }
        // [nsatpute][23-06-2024] GEOS2-5701
        public List<People> GetAllActivePeoples_V2530()
        {
            List<People> result = new List<People>();
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                result = mgr.GetAllActivePeoples_V2530(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return result;
        }
        // [nsatpute][27-06-2024] GEOS2-5702
        public List<Emdep.Geos.Data.Common.PLM.Group> GetAllActiveCompanyGroups()
        {
            List<Emdep.Geos.Data.Common.PLM.Group> result = new List<Emdep.Geos.Data.Common.PLM.Group>();
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                result = mgr.GetAllActiveCompanyGroups(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return result;
        }
        // [nsatpute][27-06-2024] GEOS2-5702
        public List<Company> GetCompanyListByIdGroup(int idGroup)
        {
            List<Company> result = new List<Company>();
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                result = mgr.GetCompanyListByIdGroup(connectionString, idGroup);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return result;
        }
        // [nsatpute][27-06-2024] GEOS2-5702
        public bool InsertImportedContacts(List<People> lstContacts)
        {
            bool result = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                result = mgr.InsertImportedContacts(connectionString, lstContacts);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return result;
        }

        #region //chitra.girigosavi 24/07/2024 GEOS2-5645 Different styles applied in combobox selection

        public List<Country> GetCountries_V2550()
        {
            List<Country> countries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetCountries_V2550(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return countries;
        }
        #endregion
        //[ashish.malkhede][30.07.2024] [GEOS2-5897] https://helpdesk.emdep.com/browse/GEOS2-5897
        public List<Offer> GetModulesReportDetails_V2550(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany,
             byte? idTemplate, long? idCPType, string idDetections, byte idCurrency, byte? idfamily, byte? idsubFamily, Int32? idCustomer = null)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                offers = mgr.GetModulesReportDetails_V2550(idActiveUser, company, fromDate, toDate, idCompany, idTemplate, idCPType, idDetections, idCurrency, idfamily, idsubFamily, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }
        //[Rahul.Gadhave][GEOS2-6084][Date:04/09/2024]
        public List<User> GetSalesAndCommericalUsers_V2560()
        {
            List<User> users = new List<User>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                users = mgr.GetSalesAndCommericalUsers_V2560(connectionString);
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
        //[Rahul.Gadhave][GEOS2-6084][Date:12/09/2024]
        public User GetUserById_V2560(int userId)
        {
            User user = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                user = mgr.GetUserById_V2560(userId, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException
                {
                    ErrorMessage = ex.Message,
                    ErrorDetails = ex.ToString()
                };
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return user;
        }

        //chitra.girigosavi GEOS2-6498 When add/Edit customer taking to much time to load form
        public List<Country> GetCountries_V2570()
        {
            List<Country> countries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetCountries_V2570(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return countries;
        }


        // [pramod.misal][25-10-2024][GEOS2-6461]
        public IList<Currency> GetCurrencies_V2580()
        {
            IList<Currency> currencies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                currencies = mgr.GetCurrencies_V2580(connectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return currencies;
        }
        //[GEOS2-6446][05.11.2024][rdixit]
        public List<Site> GetPlantsByIdSourceList_V2580(string idSourceList, string idRegionList, string idCountryList)
        {
            List<Site> countries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetPlantsByIdSourceList_V2580(connectionString, idSourceList, idRegionList, idCountryList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return countries;
        }

        //[pramod.misal][GEOS2-6462][15.11.2024]
        public List<Emdep.Geos.Data.Common.Crm.ShippingAddress> GetShippingAddressByIdPlant(IList<Company> CompanyList)
        {
            List<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddress = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                shippingAddress = mgr.GetShippingAddressByIdPlant(connectionString, CompanyList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return shippingAddress;
        }
        //[RGadhave][12.11.2024][GEOS2-6462]
        public IList<Data.Common.OTM.CustomerDetail> GetIncotermsList_V2580()
        {
            IList<Data.Common.OTM.CustomerDetail> incoterms = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                incoterms = mgr.GetIncotermsList_V2580(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return incoterms;
        }

        //[RGadhave][12.11.2024][GEOS2-6462]
        public IList<Data.Common.OTM.CustomerDetail> GetPaymentTermsList_V2580()
        {
            IList<Data.Common.OTM.CustomerDetail> incoterms = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                incoterms = mgr.GetPaymentTermsList_V2580(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return incoterms;
        }
        public Company AddCompany_V2580(Company company, int SelectedIncotermId, string SelectedPaymentTermsName, ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddressList, Company emdepSite = null)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.AddCompany_V2580(company, SelectedIncotermId, SelectedPaymentTermsName, shippingAddressList, mainconnectionString, crmconnectionString, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }


        //[pramod.misal][GEOS2-6462][18-11-2024]
        public Company GetCompanyDetailsById_V2580(Int32 idSite)
        {
            Company company = new Company();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                company = mgr.GetCompanyDetailsById_V2580(connectionString, idSite);
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

        public List<Emdep.Geos.Data.Common.Crm.ShippingAddress> GetContentcountryimgurl_V2580(Int64 idcountry)
        {
            List<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddress = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                shippingAddress = mgr.GetContentcountryimgurl_V2580(connectionString, idcountry);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return shippingAddress;
        }

        //[pooja.jadhav][22-11-2024][GEOS2-6462]
        public Company UpdateCompany_V2580(Company company)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany_V2580(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        //[rdixit][GEOS2-6602][26.11.2024]
        public List<People> GetActionPlanItemResponsible_V2590(string idSiteList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetActionPlanItemResponsible_V2590(connectionString, idSiteList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        // [Rahul.Gadhave][GEOS2-6011][Date:10-01-2025]
        public IList<Currency> GetCurrencyByExchangeRate_V2600()
        {
            IList<Currency> currencies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                currencies = mgr.GetCurrencyByExchangeRate_V2600(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return currencies;
        }
        //[Rahul.Gadhave][GEOS2-6817 - Q quotation generated without Date] [Date:14-01-2025] 
        public Offer UpdateOffer_V2600(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2600(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }
        //[Rahul.Gadhave][GEOS2-6817 - Q quotation generated without Date] [Date:14-01-2025] 
        public Offer AddOffer_V2600(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;

                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOffer_V2600(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }
        //[Rahul.Gadhave][GEOS2-6817 - Q quotation generated without Date] [Date:14-01-2025]
        public Offer AddOfferWithIdSourceOffer_V2600(Offer offer, Int32 idSite, Int32 idUser)
        {
            Offer addedOffer = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                addedOffer = mgr.AddOfferWithIdSourceOffer_V2600(offer, connectionString, offerCodeConnectionString, idSite, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedOffer;
        }


        //[cpatil][GEOS2-6664][05 - 02 - 2025]
        public List<ActionPlanItem> GetSalesActivityRegister_V2610(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister_V2610(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-6605][cpatil][05.03.2025]
        public Tuple<string, Int64> MakeOfferCode_V2620(byte? idOfferType, Int32 idCustomer, Int32 idUser)
        {

            Tuple<string, Int64> codeAndOfferNumber;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                codeAndOfferNumber = mgr.MakeOfferCode_V2620(idOfferType, idCustomer, connectionString, offerCodeConnectionString, mainServerConnectionString, idUser, UseSQLCounter);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return codeAndOfferNumber;
        }
        //[nsatpute][19-03-2025][GEOS2-6991]
        public List<Offer> GetArticlesReportDetails_V2620(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles)
        {
            List<Offer> offers = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails_V2620(connectionString, idActiveUser, company, fromDate, toDate, idCurrency, idArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return offers;
        }

        //[pooja.jadhav][GEOS2-6809][20-03-2025]
        public bool CreateFolderOffer_V2620(Offer offer, EmdepSite site, bool? isNewFolderForOffer = null)
        {
            bool isCreated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                isCreated = mgr.CreateFolderOffer_V2620(offer, connectionString, offer.Site.ConnectPlantId, Properties.Settings.Default.WorkingOrdersPath, workbenchConnectionString, site, isNewFolderForOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isCreated;
        }
        //chitra.girigosavi[GEOS2-7207][25/03/2025]
        public List<Customer> GetAllCustomer_V2630()
        {
            List<Customer> customers = null;

            try
            {
                CustomerManager mgr = new CustomerManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetAllCustomer_V2630(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }
        // [Rahul.Gadhave][GEOS2-7650][Date:04-01-2025]
        public List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2630(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser)
        {
            List<SalesUserWon> salesUserWons = new List<SalesUserWon>();

            try
            {
                CrmManager mgr = new CrmManager();
                salesUserWons = mgr.GetAllSalesTeamUserWonDetail_V2630(company, idCurrency, idUser, accountingYearFrom, accountingYearTo, idCurrentUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return salesUserWons;
        }

        //chitra.girigosavi GEOS2-6695 04/04/2025
        public List<Site> GetPlantsByIdSourceList_V2630(string idSourceList, string idRegionList, string idCountryList, string SalesOwnerForPlant, string IdStatus)
        {
            List<Site> countries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetPlantsByIdSourceList_V2630(connectionString, idSourceList, idRegionList, idCountryList, SalesOwnerForPlant, IdStatus);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return countries;
        }

        //chitra.girigosavi GEOS2-7242 02/04/2025
        public Company UpdateCompany_V2630(Company company)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany_V2630(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2630(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2630(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        public List<Company> GetSelectedUserCustomersByPlant_V2630(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2630(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        public List<Company> GetCustomersBySalesOwnerId_V2630(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2630(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        public Company GetCompanyDetailsById_V2630(Int32 idSite)
        {
            Company company = new Company();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                company = mgr.GetCompanyDetailsById_V2630(connectionString, idSite);
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

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        public Company AddCompany_V2630(Company company, int SelectedIncotermId, string SelectedPaymentTermsName, ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddressList, Company emdepSite = null)
        {
            Company addedCompany = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.AddCompany_V2630(company, SelectedIncotermId, SelectedPaymentTermsName, shippingAddressList, mainconnectionString, crmconnectionString, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }


        public ActionPlanItem ActionLauncherAdd_V2630(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.ActionLauncherAdd_V2630(actionPlanItem, customerSiteList, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //chitra.girigosavi[GEOS2-8129][19/05/2025]
        public ActionPlanItem ActionLauncherAdd_V2640(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.ActionLauncherAdd_V2640(actionPlanItem, customerSiteList, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][19.06.2025][GEOS2-7032]
        public List<ActionPlanItem> GetSalesActivityRegister_V2650(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate)
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSalesActivityRegister_V2650(idsSelectedUser, idsSelectedPlant, idUser, idPermission, closedDate, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [pramod.misal][GEOS2-8169][17.07.2025]
        public List<SitesWithCustomer> GetWithSiteswithSalesUsers_V2660()
        {
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWithSiteswithSalesUsers_V2660(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-8169][17.07.2025]
        public List<Site> GetPlantsByIdSourceList_V2660(string idSourceList, string idRegionList, string idCountryList, string SalesOwnerForPlant, string IdStatus)
        {
            List<Site> countries = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetPlantsByIdSourceList_V2660(connectionString, idSourceList, idRegionList, idCountryList, SalesOwnerForPlant, IdStatus);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return countries;
        }
 
        // [pallavi.kale][04-09-2025] [GEOS2-8949]
        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2670(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2670(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
 
        // [pallavi.kale][04-09-2025] [GEOS2-8949]
        public List<Company> GetCustomersBySalesOwnerId_V2670(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2670(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
   
        // [pallavi.kale][04-09-2025] [GEOS2-8949]
        public List<Company> GetSelectedUserCustomersByPlant_V2670(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2670(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[Rahul.Gadhave][Date:08/10/2025][https://helpdesk.emdep.com/browse/GEOS2-8440]
        public List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2680(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();

                string rdDotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    rdDotProjectConnectionString = null;
                }
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2680(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }
       
        //[pallavi.kale][GEOS2-9792][09.10.2025]]
        public List<Company> GetSelectedUserCustomersByPlant_V2680(string idSite)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersByPlant_V2680(connectionString, idSite, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[pallavi.kale][GEOS2-9792][09.10.2025]
        public List<Company> GetSelectedUserCustomersBySalesOwnerId_V2680(string idUser)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCustomersBySalesOwnerId_V2680(connectionString, idUser, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[pallavi.kale][GEOS2-9792][09.10.2025]
        public List<Company> GetCustomersBySalesOwnerId_V2680(Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCustomersBySalesOwnerId_V2680(connectionString, idUser, idZone, idUserPermission, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
        
        //[pallavi.kale][GEOS2-8955][09.10.2025]
        public bool UpdateContact_V2680(People people)
        {
            bool isUpdated = false;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateContact_V2680(connectionString, people);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        //[pallavi.kale][GEOS2-8955][09.10.2025]
        public List<People> GetContactsByIdPermission_V2680(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2680(connectionString, idActiveUser, idUser, idSite, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        public ObservableCollection<User> GetAssigneeUserList_V2680()
        {
            ObservableCollection<User> list = null;
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                CrmManager mgr = new CrmManager();
                list = mgr.GetAssigneeUserList_V2680(ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        public List<EngineeringAnalysis> GetEngAnalysisAllRevisionsByOfferCode_V2680(Offer offer)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetEngAnalysisAllRevisionsByOfferCode_V2680(offer, connectionString, Properties.Settings.Default.WorkingOrdersPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        public EngineeringAnalysisType CRM_GetAssignedToFromOtitems_V2680(Int64 IdRevisionItem)
        {
            EngineeringAnalysisType engineeringAnalysis = null;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                engineeringAnalysis = mgr.CRM_GetAssignedToFromOtitems_V2680(connectionString, IdRevisionItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return engineeringAnalysis;
        }
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        public User GetAssigneeUserIdUserwise_V2680(Int64 AssignedToUser)
        {
            User list = new User();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                CrmManager mgr = new CrmManager();
                list = mgr.GetAssigneeUserIdUserwise_V2680(AssignedToUser,ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        public Offer UpdateOffer_V2680(Offer offer, Int32 idSite, Int32 idUser)
        {

            try
            {
                CrmManager mgr = new CrmManager();
                string dotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    dotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    dotProjectConnectionString = null;
                }
                string offerCodeConnectionString = ConfigurationManager.ConnectionStrings["OfferCodeContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                bool UseSQLCounter = Properties.Settings.Default.UseSQLCounter;

                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MailServer mailServer = new MailServer();
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;

                offer = mgr.UpdateOffer_V2680(offer, connectionString, offerCodeConnectionString, idSite, idUser, Properties.Settings.Default.CommericalPath, Properties.Settings.Default.WorkingOrdersPath, mainServerConnectionString, UseSQLCounter, dotProjectConnectionString, mailServer, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offer;
        }


        //[pramod.misal][19-11-2025][GEOS2-8444] https://helpdesk.emdep.com/browse/GEOS2-8444
        public List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2690(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission)
        {
            List<Offer> LstOffer = new List<Offer>();
            try
            {
                CrmManager mgr = new CrmManager();

                string rdDotProjectConnectionString;
                if (mgr.IsConnectionStringNameExist("DotProjectContext") == true)
                {
                    rdDotProjectConnectionString = ConfigurationManager.ConnectionStrings["DotProjectContext"].ConnectionString;
                }
                else
                {
                    rdDotProjectConnectionString = null;
                }
                LstOffer = mgr.GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2690(idCurrency, idUser, idZone, accountingYearFrom, accountingYearTo, companyDetails, idUserPermission, rdDotProjectConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return LstOffer;
        }
        //[nsatpute][03.12.2025][GEOS2-10361]
        public bool InsertImportedAccounts_V2690(List<Company> lstCompanies, Company emdepSite)
        {
            bool result = false;
            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                result = mgr.InsertImportedAccounts_V2690(connectionString, lstCompanies, emdepSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return result;
        }
    }
}
