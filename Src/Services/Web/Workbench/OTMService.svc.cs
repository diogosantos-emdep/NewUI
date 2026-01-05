using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.OTMDataModel;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ShippingAddress = Emdep.Geos.Data.Common.OTM.ShippingAddress;
using System.Collections.ObjectModel;
using System.Net.Mail;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.APM;
using Prism.Logging;
using System.IO;
using SelectPdf;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OTMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OTMService.svc or OTMService.svc.cs at the Solution Explorer and start debugging.
    public class OTMService : IOTMService
    {
        OTMManager mgr = new OTMManager();
   
        public bool AddOTMEmails(email emailDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                return mgr.AddEmails(emailDetails, Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("OtmContext") == false)
                {
                    exp.ErrorMessage = "OtmContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddEmails(Email emailDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddEmails(emailDetails, MainServerConnectionString, Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("OtmContext") == false)
                {
                    exp.ErrorMessage = "OtmContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][30.07.2024][GEOS2-6005]
        public bool AddEmails_V2550(Email emailDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddEmails_V2550(emailDetails, MainServerConnectionString, Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("OtmContext") == false)
                {
                    exp.ErrorMessage = "OtmContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region [GEOS2-5867][rdixit][25.07.2024]
        public List<Email> GetAllUnprocessedEmails()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionStringGEOS = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string ConnectionStringemdep_geos = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllUnprocessedEmails(ConnectionStringGEOS, ConnectionStringemdep_geos, Properties.Settings.Default.PoAnalyzerEmailToCheck, 
                    Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        public List<BlackListEmail> GetAllBlackListEmails()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllBlackListEmails(ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                OTMManager mgr = new OTMManager();
                list = mgr.GetLookupValues(ConnectionString, key);
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

        public bool AddPORequest(PORequest poRequest)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddPORequest(poRequest, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TemplateSetting> GetAllTags()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllTags(MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddPODetails(List<CustomerDetail> poDetailList)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddPODetails(poDetailList, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Currency> GetAllCurrencies()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllCurrencies(ConnectionString);
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
        /// [001][ashish.malkhede][03102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
        /// </summary>
        /// <returns></returns>
        public List<PORequestDetails> GetPORequestDetails(DateTime FromDate, DateTime Todate, long plantId, string plantConnection)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails(ConnectionString,FromDate,Todate,plantId,plantConnection, Properties.Settings.Default.CurrenciesImages);
            }
            catch(Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        /// <summary>
        /// [001] [ashish.malkhede][08-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6460
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="Todate"></param>
        /// <param name="plantId"></param>
        /// <param name="plantConnection"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<PORegisteredDetails> GetPORegisteredDetails(DateTime FromDate, DateTime Todate,int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter Filter)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORegisteredDetails(ConnectionString, FromDate, Todate, IdCurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, Filter);
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
        /// [001][ashish.malkhede][04102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public List<Company> GetAllSitesWithImagesByIdUser(Int32 idUser)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllSitesWithImagesByIdUser(connectionString, idUser);
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

        // [pramod.misal][04-10-2024][GEOS2-6520]
        public List<POStatus> OTM_GetAllPOWorkflowStatus()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.OTM_GetAllPOWorkflowStatus(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        //[001] [ashish.malkhede][13-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6460
        public List<POType> OTM_GetAllPOTypeStatus()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetAllPOTypeStatus(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //[rdixit][GEOS2-5868][14.10.2024]
        public List<Customer> GetAllCustomers()
        {
            List<Customer> CustomerList = new List<Customer>();

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerList = mgr.GetAllCustomers(connectionString);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CustomerList;
        }

        //[rdixit][GEOS2-5868][14.10.2024]
        public List<TemplateSetting> GetTemplateByCustomer(int idCustomer)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetTemplateByCustomer(MainServerConnectionString, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-5868][14.10.2024]
        public bool AddPODetails_V2570(List<CustomerDetail> poDetailList)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddPODetails_V2570(poDetailList, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-5868][14.10.2024]
        public List<Email> GetAllUnprocessedEmails_V2570()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionStringGEOS = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string ConnectionStringemdep_geos = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllUnprocessedEmails_V2570(ConnectionStringGEOS, ConnectionStringemdep_geos, Properties.Settings.Default.PoAnalyzerEmailToCheck,
                    Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-5868][14.10.2024]
        public bool UpdatePORequestStatus(PORequest poRequest)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.UpdatePORequestStatus(poRequest, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-5868][14.10.2024]
        public List<Email> GetEmailCreatedIn_V2570()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionStringGEOS = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmailCreatedIn_V2570(ConnectionStringGEOS, Properties.Settings.Default.PoAnalyzerEmailToCheck);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-5868][14.10.2024]
        public bool AddEmails_V2570(Email emailDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddEmails_V2570(emailDetails, MainServerConnectionString, Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("OtmContext") == false)
                {
                    exp.ErrorMessage = "OtmContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][07.11.2024][GEOS2-6600]
        //public List<PORequestDetails> GetPORequestDetails_V2580(DateTime FromDate, DateTime Todate, long plantId, string plantConnection)
        //{
        //    OTMManager mgr = new OTMManager();
        //    try
        //    {
        //        string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
        //        return mgr.GetPORequestDetails_V2580(ConnectionString, FromDate, Todate, plantId, plantConnection, Properties.Settings.Default.CurrenciesImages);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        //[RGadhave][12.11.2024][GEOS2-6461]
        public List<PORequestDetails> GetPORequestDetails_V2580(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId, string plantConnection)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2580(ConnectionString, FromDate, Todate, Idcurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        public List<Company> GetAllCompaniesDetails_V2580(Int32 idUser)
        {
            List<Company> companyList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                companyList = mgr.GetAllCompaniesDetails_V2580(idUser, connectionString);
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

        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        public List<POType> OTM_GetPOTypes_V2580()
        {
            List<POType> POTypeList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                POTypeList = mgr.OTM_GetPOTypes_V2580(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return POTypeList;
        }

        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        public List<CustomerPlant> OTM_GetCustomerPlant_V2580()
        {
            List<CustomerPlant> CustomerPlantList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerPlantList = mgr.OTM_GetCustomerPlant_V2580(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CustomerPlantList;
        }

        public List<string> OTM_GetPOSender_V2580()
        {
            List<string> POSenderList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                POSenderList = mgr.OTM_GetPOSender_V2580(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return POSenderList;
        }

        //[pramod.misal][GEOS2-6460][28-11-2024]
        public List<Currency> GetAllCurrencies_V2590()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllCurrencies_V2590(ConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-6460][28-11-2024]
        public List<Company> GetAllSitesWithImagesByIdUser_V2590(Int32 idUser)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllSitesWithImagesByIdUser_V2590(connectionString, idUser);
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
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<ShippingAddress> OTM_GetShippingAddress_V2590(int IdCustomerPlant)
        {
            List<ShippingAddress> ShippingAddressList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                ShippingAddressList = mgr.OTM_GetShippingAddress_V2590(connectionString,IdCustomerPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ShippingAddressList;
        }
        /// <summary>
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="Todate"></param>
        /// <param name="plantId"></param>
        /// <param name="plantConnection"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<PORegisteredDetails> GetPORegisteredDetails_V2590(DateTime FromDate, DateTime Todate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter Filter)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORegisteredDetails_V2590(ConnectionString, FromDate, Todate, IdCurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, Filter);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public bool IsProfileUpdate(string EmployeeCode)
        {
            bool isUpdated = false;
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                isUpdated = mgr.IsProfileUpdate(EmployeeCode, workbenchConnectionString);
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

        //[pramod.misal][06-12-2024][GEOS2-6463]
        public List<LinkedOffers> OTM_GetLinkedofferByIdCustomerPlant(Int32 SelectedIdCustomerPlant,Int64 SelectedIdPO, GeosAppSetting geosAppSetting)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetLinkedofferByIdCustomerPlant_V2630(ConnectionString, SelectedIdCustomerPlant, SelectedIdPO, Properties.Settings.Default.CurrenciesImages, geosAppSetting,Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<Customer> GetAllCustomers_V2590()
        {
            List<Customer> CustomerList = new List<Customer>();

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerList = mgr.GetAllCustomers_V2590(connectionString);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CustomerList;
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<CustomerPlant> OTM_GetCustomerPlant_V2590()
        {
            List<CustomerPlant> CustomerPlantList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerPlantList = mgr.OTM_GetCustomerPlant_V2590(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CustomerPlantList;
        }
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<Currency> GetAllPOCurrencies_V2590()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllPOCurrencies_V2590(ConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<PORegisteredDetails> OTM_GetPOSender_V2590()
        {
            List<PORegisteredDetails> POSenderList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                POSenderList = mgr.OTM_GetPOSender_V2590(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return POSenderList;
        }

        //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2590(PORegisteredDetails poRegisteredDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetOTM_GetLinkedOffers_V2590(ConnectionString, poRegisteredDetails,Properties.Settings.Default.CommericalPath);
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
        /// [001][ashish.malkhede][07-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6464
        /// </summary>
        /// <param name="idPO"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<LogEntryByPOOffer> GetAllPOChangeLog_V2590(long idPO)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllPOChangeLog_V2590(ConnectionString, idPO);
            }
            catch(Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
      
        /// <summary>
        /// [001][ashish.malkhede][10-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="po"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool UpdatePurchaseOrder_V2590(PORegisteredDetails po)
        {
            bool isSave = false;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                isSave = mgr.UpdatePurchaseOrder_V2590(po, connectionString, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isSave;
        }
        /// <summary>
        /// [001][ashish.malkhede][11-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="IdAppSetting"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting)
        {
            GeosAppSetting geosAppSetting = new GeosAppSetting();
            try
            {
                OTMManager mgr = new OTMManager();
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

        //[pramod.misal][GEOS2-6462][18-11-2024]
        public Company GetCompanyDetailsById_V2580(Int32 idSite)
        {
            Company company = new Company();

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
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
        public List<People> GetContactsByIdPermission_V2590(Int32 Idperson)
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2590(connectionString, Idperson);
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

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<People> GetPOReceptionEmailToFeilds_V2590(Int64 IdPO)
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetPOReceptionEmailToFeilds_V2590(connectionString, IdPO);
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

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<People> GetPOReceptionEmailCCFeilds_V2590(Int64 IdPO)
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetPOReceptionEmailCCFeilds_V2590(connectionString, IdPO);
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

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<People> GetPeopleByEMDEPcustomer_V2590()
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetPeopleByEMDEPcustomer_V2590(connectionString);
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

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        public List<Language> GetAllLanguages_V2590()
        {
            List<Language> languages = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                languages = mgr.GetAllLanguages_V2590(connectionString);
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
        //[ashish.malkhede][GEOS2-6463][19-12-2024]
        public string GetEmployeeCodeByUserID(Int64 IdUser)
        {
            string EmployeeCode = "";

            try
            {
                OTMManager mgr = new OTMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeCode = mgr.GetEmployeeCodeByUserID(IdUser, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeCode;
        }
        //[ashish.malkhede][GEOS2-6463][19-12-2024]
        public string GetCommercialPath()
        {
            string Path = "";
            try
            {
                Path = Properties.Settings.Default.CommericalPath;
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Path;
        }
        public double GetOfferAmountByCurrencyConversion_V2590(int PreIdCurrency, int idCurrency, long IdPO)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetOfferAmountByCurrencyConversion_V2590(connectionString,PreIdCurrency,idCurrency,IdPO);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-6465][16.12.2024]
        public string ReadMailTemplate(string templateName)
        {
            try
            {
                return System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, templateName));
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-6465][16.12.2024]
        public bool  POEmailSend_V2590(string EmailSubject,string htmlEmailtemplate, ObservableCollection<People> toContactList, ObservableCollection<People> CcContactList,string fromMail, List<LinkedResource> imageList)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.POEmailSend_V2590(EmailSubject,htmlEmailtemplate, toContactList, CcContactList, fromMail, workbenchConnectionString, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, imageList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        public List<OtRequestTemplates> GetAllOtImportTemplate_V2600()
        {
            List<OtRequestTemplates> Otrequesttemplates = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Otrequesttemplates = mgr.GetAllOtImportTemplate_V2600(connectionString, Properties.Settings.Default.OtAttachmentPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return Otrequesttemplates;
        }
        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        public bool DeletetImportTemplate_V2600(Int32 IdOTRequestTemplate)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeletetImportTemplate_V2600(connectionString, IdOTRequestTemplate);
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
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<Customer> GetCustomerDetails_V2600()
        {
            List<Customer> Customers = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Customers = mgr.GetCustomerDetails_V2600(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return Customers;
        }

        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<Regions> GetRegions_V2600()
        {
            List<Regions> RegionsList = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                RegionsList = mgr.GetRegions_V2600(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return RegionsList;
        }

        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<Country> GetCountriesDetails_V2600()
        {
            List<Country> Countries = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Countries = mgr.GetCountriesDetails_V2600(connectionString);
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

        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool AddUpdateOTRequestTemplates(OtRequestTemplates template)
        {
            bool flag;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                flag = mgr.AddUpdateOTRequestTemplates(template, connectionString, Properties.Settings.Default.OtAttachmentPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return flag;

        }

        //[pooja.jadhav][09-01-2025][GEOS2-6831]
        public string GetCode()
        {
            string Code;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Code = mgr.GetCode(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return Code;
        }

        //[pooja.jadhav][20-01-2025][GEOS2-6734]
        public Dictionary<int, string> GetPoAnalizerTag_V2600()
        {
            Dictionary<int, string> PoAnalizerTag;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                PoAnalizerTag = mgr.GetPoAnalizerTag_V2600(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return PoAnalizerTag;
        }

        //[pooja.jadhav][20-01-2025][GEOS2-6734]
        public Dictionary<int, string> GetPORequestTemplateFieldType_V2600()
        {
            Dictionary<int, string> PORequestTemplateFieldType;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                PORequestTemplateFieldType = mgr.GetPORequestTemplateFieldType_V2600(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return PORequestTemplateFieldType;
        }

        //[pooja.jadhav][21-01-2025][GEOS2-6734]
        public Dictionary<int, string> GetPORequestTemplateFieldTypeForPDF_V2600()
        {
            Dictionary<int, string> PORequestTemplateFieldType;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                PORequestTemplateFieldType = mgr.GetPORequestTemplateFieldTypeForPDF_V2600(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return PORequestTemplateFieldType;
        }
        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        public List<CustomerCountriesDetails> GetAllCustomersAndCountries_V2600()
        {
            List<CustomerCountriesDetails> customerDetails;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                customerDetails = mgr.GetAllCustomersAndCountries_V2600(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return customerDetails;
        }
        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        public List<POAnalyzerOTTemplate> GetOTRequestTemplateByCustomer_V2600(int idCustomer)
        {
            List<POAnalyzerOTTemplate> templates;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                templates = mgr.GetOTRequestTemplateByCustomer_V2600(connectionString, idCustomer);
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

        public string GetPdfFilePath(Emailattachment attachFile)
        {
            string Path = "";
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Path = Properties.Settings.Default.OTMMailAttachmentSavePath;
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return Path;
        }

        //[pooja.jadhav][24-01-2025][GEOS2-6734]
        public List<Country> GetCountriesByCustomerAndRegion(int IdCustomer,int IdRegion)
        {
            List<Country> Countries = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Countries = mgr.GetCountriesByCustomerAndRegion(connectionString,IdCustomer, IdRegion);
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

        //[pooja.jadhav][24-01-2025][GEOS2-6734]
        public List<CustomerPlant> GetPlantByCustomerAndCountry(int IdCustomer, int IdCountry)
        {
            List<CustomerPlant> CustomerPlant = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerPlant = mgr.GetPlantByCustomerAndCountry(connectionString,IdCustomer, IdCountry);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return CustomerPlant;
        }

        //[pramod.misal][GEOS2-6735][27-01-2025]
        public List<Email> GetAllUnprocessedEmails_V2600()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionStringGEOS = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string ConnectionStringemdep_geos = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllUnprocessedEmails_V2600(ConnectionStringGEOS, ConnectionStringemdep_geos, Properties.Settings.Default.PoAnalyzerEmailToCheck,
                    Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-6735][23.01.2025]
        public List<POAnalyzerOTTemplate> GetOTRequestExcelTemplateByCustomer_V2600(int idCustomer)
        {
            List<POAnalyzerOTTemplate> templates;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                templates = mgr.GetOTRequestExcelTemplateByCustomer_V2600(connectionString, idCustomer);
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
        //[rahul.gadhave][GEOS2-6720][29.01.2025]
        public List<PORequestDetails> GetPORequestDetails_V2610(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId, string plantConnection)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2610(ConnectionString, FromDate, Todate, Idcurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OtRequestTemplates> GetAllOtImportTemplate_V2610()
        {
            List<OtRequestTemplates> Otrequesttemplates = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Otrequesttemplates = mgr.GetAllOtImportTemplate_V2610(connectionString, Properties.Settings.Default.POTemplateFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return Otrequesttemplates;
        }

        public List<OtRequestTemplates> OTM_GetAllMappingFieldsData_V2610(OtRequestTemplates ObjOtRequestTemplates)
        {
            List<OtRequestTemplates> Otrequesttemplates = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Otrequesttemplates = mgr.OTM_GetAllMappingFieldsData_V2610(connectionString, ObjOtRequestTemplates);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return Otrequesttemplates;
        }

        public OtRequestTemplates AddUpdateOTRequestTemplates_V2610(OtRequestTemplates template)
        {
            OtRequestTemplates temp = new OtRequestTemplates();
            try
            {
               
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                temp = mgr.AddUpdateOTRequestTemplates_V2610(template, connectionString, Properties.Settings.Default.POTemplateFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return temp;

        }

        /// <summary>
        /// [pooja.jadhav][08-01-2025][GEOS2-6831]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<Regions> GetRegions_V2610(int idCustomer)
        {
            List<Regions> RegionsList = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                RegionsList = mgr.GetRegions_V2610(connectionString, idCustomer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return RegionsList;
        }

        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        public bool DeletetTextAndLocation_V2610(long IdOTRequestTemplate)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeletetTextAndLocation_V2610(connectionString, IdOTRequestTemplate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        public bool DeletetCellFields_V2610(long IdOTRequestTemplate)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeletetCellFields_V2610(connectionString, IdOTRequestTemplate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        public bool AddEmails_V2610(Email emailDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddEmails_V2610(emailDetails, MainServerConnectionString, Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("OtmContext") == false)
                {
                    exp.ErrorMessage = "OtmContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        public bool AddPODetails_V2610(List<CustomerDetail> poDetailList)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddPODetails_V2610(poDetailList, MainServerConnectionString);
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
        /// //[pramod.misal][04.02.2025][GEOS2 - 6726]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<POEmployeeInfo> GetPOEmployeeInfoList_V2610()
        {
            List<POEmployeeInfo> Countries = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Countries = mgr.GetPOEmployeeInfoList_V2610(connectionString);
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


        //[pramod.misal][GEOS2-6720][19.02.2025]
        public List<PORequestDetails> GetPORequestDetails_V2610_V1(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId, string plantConnection)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2610_V1(ConnectionString, FromDate, Todate, Idcurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave][GEOS2-6799][Date:19/02/2025]
        public List<Email> GetEmailCreatedIn_V2610()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionStringGEOS = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmailCreatedIn_V2610(ConnectionStringGEOS, Properties.Settings.Default.PoAnalyzerEmailToCheck);
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
        /// //[pramod.misal][19.02.2025][GEOS2 - 6719]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<CustomerContacts> GetAllCustomerContactsList_V2620()
        {
            List<CustomerContacts> CustomerContacts = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerContacts = mgr.GetAllCustomerContactsList_V2620(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return CustomerContacts;
        }

        //[pramod.misal][GEOS2-7036][27-02-2025] https://helpdesk.emdep.com/browse/GEOS2-7036
        public List<PORegisteredDetails> GetPORegisteredDetails_V2620(DateTime FromDate, DateTime Todate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter Filter)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORegisteredDetails_V2620(ConnectionString, FromDate, Todate, IdCurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, Filter);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[ashish.malkhede][GEOS2-7042][12-03-2025]
        public List<PORegisteredDetails> GetPORegisteredDetails_V2620_V1(DateTime FromDate, DateTime Todate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter Filter)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                var temp = mgr.GetPORegisteredDetails_V2620_V1(ConnectionString, FromDate, Todate, IdCurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, Filter);
                return temp.Result.Cast<PORegisteredDetails>().ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<CustomerPlant> OTM_GetCustomerPlant_V2620()
        {
            List<CustomerPlant> CustomerPlantList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerPlantList = mgr.OTM_GetCustomerPlant_V2620(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CustomerPlantList;
        }
        //[Rahul.Gadhave][GEOS-7040][Date:28-02-2025][https://helpdesk.emdep.com/browse/GEOS2-7040]
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2620(PORegisteredDetails poRegisteredDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetOTM_GetLinkedOffers_V2620(ConnectionString, poRegisteredDetails, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][04.03.2025][GEOS2-6724]
        public byte[] GetPoAttachmentByte(string attachmentPath)
        {
            byte[] bytes = null;
            try
            {
                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    if (File.Exists(attachmentPath))
                    {
                        using (System.IO.FileStream stream = new System.IO.FileStream(attachmentPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                }
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return bytes;
        }
        //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
        public int? GetOfferDetails_V2620(string OfferCode)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetOfferDetails_V2620(ConnectionString, OfferCode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
        public void  OTM_InsertPORequestLinkedOffer_V2620(Int64 IdPORequest, int? IdOffer)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.OTM_InsertPORequestLinkedOffer_V2620(IdPORequest,IdOffer,ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<PORequestDetails> GetPORequestDetails_V2620(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId, string plantConnection)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2620(ConnectionString, FromDate, Todate, Idcurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-7056][Date:07-03-2025]
        public List<ShippingAddress> OTM_GetShippingAddress_V2620(int IdSite)
        {
            List<ShippingAddress> ShippingAddressList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                ShippingAddressList = mgr.OTM_GetShippingAddress_V2620(connectionString, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ShippingAddressList;
        }
        //[pooja.jadhav][GEOS2-7054][10-03-2025]
        public List<PORegisteredDetails> OTM_GetPOSender_V2620()
        {
            List<PORegisteredDetails> POSenderList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                POSenderList = mgr.OTM_GetPOSender_V2620(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return POSenderList;
        }

        /// <summary>
        /// [001]  //[Rahul.Gadhave][GEOS2-7226][Date:24-03-2025] https://helpdesk.emdep.com/browse/GEOS2-7226
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<ShippingAddress> OTM_GetShippingAddressForShowAll_V2630(int IdCustomerPlant)
        {
            List<ShippingAddress> ShippingAddressList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                ShippingAddressList = mgr.OTM_GetShippingAddressForShowAll_V2630(connectionString, IdCustomerPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ShippingAddressList;
        }
        //[ashish.malkhede][GEOS2-7049][25-03-2025] https://helpdesk.emdep.com/browse/GEOS2-7049
        public List<PORegisteredDetails> GetPORegisteredDetails_V2630(DateTime FromDate, DateTime Todate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter Filter)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORegisteredDetails_V2630(ConnectionString, FromDate, Todate, IdCurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages, Properties.Settings.Default.CountryFilePath, Filter);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        public bool POEmailSend_V2630(string EmailSubject, string htmlEmailtemplate, List<People> toContactList, List<People> CcContactList, string fromMail, List<LinkedResource> imageList)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.POEmailSend_V2630(EmailSubject, htmlEmailtemplate, toContactList, CcContactList, fromMail, workbenchConnectionString, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, imageList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        public List<People> GetPeopleByJobDescriptions_V2630(GeosAppSetting CustomerPOConfirmationJD, long plantIds)
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetPeopleByJobDescriptions_V2630(connectionString,CustomerPOConfirmationJD,plantIds);
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
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        public List<People> GetPeopleByEMDEPcustomer_V2630(PORegisteredDetails poregistereddetailsforemail)
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetPeopleByEMDEPcustomer_V2630(connectionString,poregistereddetailsforemail);
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
        public List<Email> GetAllEmailsBlankColumns()
        {
            List<Email> emails = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                emails = mgr.GetAllEmailsBlankColumns(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return emails;
        }
        public bool UpdatePORequestGroupPlant_V2630(Email poRequest)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.UpdatePORequestGroupPlant_V2630(poRequest, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public void UpdateSenderIdPerson_V2630(Email email)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.UpdateSenderIdPerson_V2630(email, ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public void UpdateCCIdPerson_V2630(Email email)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.UpdateCCIdPerson_V2630(email, ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public void UpdateToIdPerson_V2630(Email email)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.UpdateToIdPerson_V2630(email, ConnectionString);
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
        /// //[pramod.misal][04.02.2025][GEOS2 - 6726]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<POEmployeeInfo> GetPOEmployeeInfoList_V2630()
        {
            List<POEmployeeInfo> Countries = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Countries = mgr.GetPOEmployeeInfoList_V2630(connectionString);
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

        //[pramod.misal][GEOS2-7720][01-04-2025]
        public List<Email> GetAllEmailsBlankColumns_V2630()
        {
            List<Email> emails = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                emails = mgr.GetAllEmailsBlankColumns_V2630(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return emails;
        }
        //[ashish.malkhede][GEOS2-7724][02-04-2025]
        public List<PORequestDetails> GetPORequestDetails_V2630(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId, string plantConnection)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2630(ConnectionString, FromDate, Todate, Idcurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages);
                //return temp.Result.Cast<PORequestDetails>().ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[ashish.malkhede][GEOS-7049][03-04-2025][https://helpdesk.emdep.com/browse/GEOS2-7049]
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2630(PORegisteredDetails poRegisteredDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetOTM_GetLinkedOffers_V2630(ConnectionString, poRegisteredDetails, Properties.Settings.Default.CommericalPath);
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
        /// [ashish.malkhede][GEOS-9194][08-12-2025]https://helpdesk.emdep.com/browse/GEOS2-9194
        /// </summary>
        /// <param name="poRegisteredDetails"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<LinkedOffers> GetOTM_GetLinkedOffers_V2660(PORegisteredDetails poRegisteredDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetOTM_GetLinkedOffers_V2660(ConnectionString, poRegisteredDetails, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-7724][07/04/2025]
        public bool AddEmails_V2630(Email emailDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddEmails_V2630(emailDetails, MainServerConnectionString, Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("OtmContext") == false)
                {
                    exp.ErrorMessage = "OtmContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pooja.jadhav][GEOS2-7052][11-04-2025]
        public List<Customer> GetAllCustomers_V2630()
        {
            List<Customer> CustomerList = new List<Customer>();

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerList = mgr.GetAllCustomers_V2630(connectionString);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CustomerList;
        }
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        public ObservableCollection<LookupValue> GetLookupValues_V2640()
        {
            ObservableCollection<LookupValue> list = null;
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                OTMManager mgr = new OTMManager();
                list = mgr.GetLookupValues_V2640(ConnectionString);
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
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        public List<LinkedOffers> OTM_GetPoRequestLinkedOffers_V2640(string Offers)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetPoRequestLinkedOffers_V2640(ConnectionString,Offers, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        public List<LinkedOffers> OTM_GetPoRequestLinkedPO_V2640(string OfferCode)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetPoRequestLinkedPO_V2640(connectionString,OfferCode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<LinkedOffers> GetPoRequestOfferTo_V2640(LinkedOffers Obj)
        {
            List<LinkedOffers> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetPoRequestOfferTo_V2640(connectionString,Obj);
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

        public ObservableCollection<LookupValue> GetCarriageMethod_V2640()
        {
            ObservableCollection<LookupValue> list = null;
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                OTMManager mgr = new OTMManager();
                list = mgr.GetCarriageMethod_V2640(ConnectionString);
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

        public List<LinkedOffers> OTM_GetPoRequestOfferType_V2640()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetPoRequestOfferType_V2640(ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[pramod.misal][06-12-2024][GEOS2-6463]
        public List<LinkedOffers> OTM_GetLinkedofferByIdCustomerPlant_V2630(Int32 SelectedIdCustomerPlant, Int64 SelectedIdPO, GeosAppSetting geosAppSetting)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetLinkedofferByIdCustomerPlant_V2630(ConnectionString, SelectedIdCustomerPlant, SelectedIdPO, Properties.Settings.Default.CurrenciesImages, geosAppSetting, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-7724][02-04-2025]
        public List<PORequestDetails> GetPORequestDetails_V2640(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId, string plantConnection)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2640(ConnectionString, FromDate, Todate, Idcurrency, plantId, ConnectionString, Properties.Settings.Default.CurrenciesImages);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][06-12-2024][GEOS2-6463]
        public List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2640(string SelectedIdPlant, string SelectedIdGroup, GeosAppSetting geosAppSetting)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetLinkedofferByIdPlantAndGroup_V2640(ConnectionString, SelectedIdPlant, SelectedIdGroup, Properties.Settings.Default.CurrenciesImages, geosAppSetting, Properties.Settings.Default.CommericalPath);
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
        /// [001][ashish.malkhede] https://helpdesk.emdep.com/browse/GEOS2-7251
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool UpdateOffer(LinkedOffers offer, List<Emailattachment> POAttachementsList)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.UpdateOffer(offer, ConnectionString, POAttachementsList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][28-04-2024][GEOS2-7247]
        public string OTM_GetEmailBodyByIdEmail_V2640(Int64 IdEmail)
        {
            string EmailBody = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                EmailBody = mgr.OTM_GetEmailBodyByIdEmail_V2640(connectionString, IdEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmailBody;
        }
        //[Rahul.gadhave][GEOS2-7246][Date:03-06-2025]
        public bool UpdatePoRequestStatus_V2640(LinkedOffers offer, List<Emailattachment> POAttachementsList)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.UpdatePoRequestStatus_V2640(offer, ConnectionString, POAttachementsList);
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
        /// [001][ashish.malkhede][07-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7254
        /// </summary>
        /// <param name="idPORequest"></param>
        /// <param name="idOffer"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<LogEntryByPORequest> GetAllPORequestChangeLog_V2640(long idPORequest, string idOffer)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllPORequestChangeLog_V2640(ConnectionString, idPORequest, idOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][07-05-2025][GEOS2-7248]
        public List<Emailattachment> OTM_GetEmailAttachementByIdEmail_V2640(Int64 IdEmail)
        {
            List<Emailattachment> EmailAttachementList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                EmailAttachementList = mgr.OTM_GetEmailAttachementByIdEmail_V2640(connectionString, IdEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmailAttachementList;
        }
       
        //[Rahul.Gadhave][GEOS2-7253][Date:05/06/2025]
        public string GetCommercialOffersPath_V2640()
        {
            string EmailBody = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                EmailBody = mgr.GetCommercialOffersPath_V2640(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmailBody;
        }

        //[pooja.jadhav][GEOS2-7252][09-05-2025]
        public PORegisteredDetails GetPORegisteredDetailsByIdPo(int IdPo, int IdCurrency)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORegisteredDetailsByIdPo(ConnectionString, IdPo, IdCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pooja.jadhav][GEOS2-7252][09-05-2025]
        public bool IsPoNumberExist(string code)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.IsPoNumberExist(ConnectionString, code);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public PORequestDetails GetPODetailsbyAttachment(int IdAttachment)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPODetailsbyAttachment(ConnectionString, IdAttachment);
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
        /// [001][rahul.gadhave][13-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7252
        /// </summary>
        /// <param name="po"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool InsertPurchaseOrder_V2640(PORegisteredDetails po)
        {
            bool isSave = false;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                isSave = mgr.InsertPurchaseOrder_V2640(po, connectionString, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isSave;
        }
        //[Rahul.Gadhave][GEOS2-8339][Date:06-06-2025]
        public List<PORequestDetails> GetPORequestDetails_V2650(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2650(ConnectionString, FromDate, Todate, Idcurrency, plantId, Properties.Settings.Default.CurrenciesImages);

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
        /// [pooja.jadhav][GEOS2-8342][11-06-2025]
        /// </summary>
        /// <param name="logEntries"></param>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public void AddChangeLogByPORequest(ObservableCollection<LogEntryByPORequest> logEntries)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.AddChangeLogByPORequest(logEntries, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-8772][Date:30-06-2025]https://helpdesk.emdep.com/browse/GEOS2-8772
        public List<PORequestDetails> GetPORequestDetails_V2660(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2660(ConnectionString, FromDate, Todate, Idcurrency, plantId, Properties.Settings.Default.CurrenciesImages);

            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

         /// [Rahul.Gadhave][GEOS2-8655][Date:08-07-2025]
        public ObservableCollection<LookupValue> GetCarriageMethod_V2660()
        {
            ObservableCollection<LookupValue> list = null;
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                OTMManager mgr = new OTMManager();
                list = mgr.GetCarriageMethod_V2660(ConnectionString);
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

        public List<LinkedOffers> OTM_GetPoRequestLinkedOffers_V2660(string Offers)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetPoRequestLinkedOffers_V2660(ConnectionString, Offers, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        public List<Email> GetAllUnprocessedEmails_V2660()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionStringGEOS = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string ConnectionStringemdep_geos = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllUnprocessedEmails_V2660(ConnectionStringGEOS, ConnectionStringemdep_geos, Properties.Settings.Default.PoAnalyzerEmailToCheck,
                    Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-9020][23.07.2025]
        public bool AddEmails_V2660(Email emailDetails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.AddEmails_V2660(emailDetails, MainServerConnectionString, Properties.Settings.Default.OTMMailAttachmentSavePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("OtmContext") == false)
                {
                    exp.ErrorMessage = "OtmContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-9020][23.07.2025]
        public void AddUniqueOffersToPORequest_V2660(Int64 idPORequest, string quotationCode)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.AddUniqueOffersToPORequest_V2660(idPORequest, quotationCode, ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-9020][23.07.2025]
        public bool UpdatePORequestGroupPlant_V2660(Email poRequest)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.UpdatePORequestGroupPlant_V2660(poRequest, MainServerConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-9080][Date:30-07-2025]
        public List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2660(string SelectedIdPlant, string SelectedIdGroup, GeosAppSetting geosAppSetting)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetLinkedofferByIdPlantAndGroup_V2660(ConnectionString, SelectedIdPlant, SelectedIdGroup, Properties.Settings.Default.CurrenciesImages, geosAppSetting, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[ashish.malkhede][08-07-2025] https://helpdesk.emdep.com/browse/GEOS2-9105
        public Int64 InsertPurchaseOrder_V2660(PORegisteredDetails po)
        {
            Int64 isSave = 0;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                isSave = mgr.InsertPurchaseOrder_V2660(po, connectionString, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isSave;
        }

        //[Rahul.Gadhave][GEOS2-9113][Date:01-08-2025]
        public List<People> GetPOReceptionEmailCCFeilds_V2660(Int64 IdPO)
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                peoples = mgr.GetPOReceptionEmailCCFeilds_V2660(connectionString, IdPO);
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
        /// [001][pramod.misal][10-12-2024] https://helpdesk.emdep.com/browse/GEOS2-9109
        /// </summary>
        /// <param name="po"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool UpdatePurchaseOrder_V2660(PORegisteredDetails po)
        {
            bool isSave = false;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                isSave = mgr.UpdatePurchaseOrder_V2660(po, connectionString, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isSave;
        }


        //[rdixit][GEOS2-9141][02.08.2025]
        public int GetUserPlant_V2660(int idperson)
        {
            bool isSave = false;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetUserPlant_V2660(connectionString,idperson);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }        
        }

        //[rdixit][GEOS2-9141][02.08.2025]
        public List<Company> GetAllCompanyDetailsList_V2660()
        {
            List<Company> connectionstrings = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                connectionstrings = mgr.GetAllCompanyDetailsList_V2660(connectionString);
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

        public bool OTM_CheckOfferExistsInGeos_V2660(int year, int number)
        {
            bool isUpdated = false;
            try
            {
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                isUpdated = mgr.OTM_CheckOfferExistsInGeos_V2660(year, number, LocalGeosContext);
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

        //[rdixit][GEOS2-9141][02.08.2025]
        public Offer GetOfferDetailsByYearAndNumber_V2660(int year, int number)
        {
            Offer Offer = null;
            try
            {
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                Offer = mgr.GetOfferDetailsByYearAndNumber_V2660(year, number, LocalGeosContext);
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
        //[rdixit][GEOS2-9141][02.08.2025]
        public Int64 InsertOffer_V2660(Quotation q)
        {
            Int64 idoffer = 0;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                idoffer = mgr.InsertOffer_V2660(q, connectionString, Properties.Settings.Default.WorkingOrdersPath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return idoffer;
        }

        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        public Quotation OTM_GetQuotationByCode_V2660(string Code, int numOT, string otcode, Quotation quo)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetQuotationByCode_V2660(connectionString, Code, numOT, otcode, quo);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool OTM_CheckQuotationExistsInGeos_V2660(string Code)
        {
            bool isUpdated = false;
            try
            {
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                isUpdated = mgr.OTM_CheckQuotationExistsInGeos_V2660(Code, LocalGeosContext);
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

        //[rdixit][GEOS2-9141][02.08.2025]
        public Int64 InsertQuotations_V2660(Quotation quotation)
        {
            Int64 idquotation = 0;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                idquotation = mgr.InsertQuotations_V2660(quotation, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return idquotation;
        }
        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        public void InsertCPDetections_V2660(List<CPDetection> detections)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                //mgr.InsertCPDetections_V2660(detections,1, connectionString);
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
        }

        /// <summary>
        /// [pramod.misal]][05-08-2025][GEOS2-9167]https://helpdesk.emdep.com/browse/GEOS2-9167
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool UpdateOffer_V2660(LinkedOffers offer, List<Emailattachment> POAttachementsList)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.UpdateOffer_V2660(offer, ConnectionString, POAttachementsList);
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
        /// [001][pramod.misal][07-08-2025] https://helpdesk.emdep.com/browse/GEOS2-9049
        /// </summary>
        /// <param name="idPORequest"></param>
        /// <param name="idOffer"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<LogEntryByPORequest> GetAllPORequestChangeLog_V2660(long idPORequest, string idOffer)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetAllPORequestChangeLog_V2660(ConnectionString, idPORequest, idOffer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.gadhave][GEOS2-9191][Date:11-08-2025]
        public ObservableCollection<LookupValue> GetLookupValues_V2660()
        {
            ObservableCollection<LookupValue> list = null;
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                OTMManager mgr = new OTMManager();
                list = mgr.GetLookupValues_V2660(ConnectionString);
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

        //[rdixit][GEOS2-9137][12.08.2025]
        public void AddChangeLogByPORequest_V2660(ObservableCollection<LogEntryByPORequest> logEntries)
        {
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.AddChangeLogByPORequest_V2660(logEntries, Properties.Settings.Default.OTMMailAttachmentSavePath, connectionString);
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
        /// [pooja.jadhav][GEOS2-9179][13-08-2025]
        /// </summary>
        /// <param name="IdCustomer"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<Customer> GetAllCustomers_V2660(int IdCustomer)
        {
            List<Customer> CustomerList = new List<Customer>();

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CustomerList = mgr.GetAllCustomers_V2660(IdCustomer, connectionString);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CustomerList;
        }

        public List<LinkedOffers> OTM_GetPoRequestLinkedPO_V2660(string OfferCode)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetPoRequestLinkedPO_V2660(connectionString, OfferCode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Emailattachment> OTM_GetEmailAttachementByIdEmail_V2660(Int64 IdEmail)
        {
            List<Emailattachment> EmailAttachementList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string Emdep_geos_ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                EmailAttachementList = mgr.OTM_GetEmailAttachementByIdEmail_V2660(connectionString, Emdep_geos_ConnectionString, IdEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmailAttachementList;
        }
        //[Rahul.Gadhave][GEOS2-9041][Date:02-09-2025] 
        public List<PORequestDetails> GetPORequestDetails_V2670(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2670(ConnectionString, FromDate, Todate, Idcurrency, plantId, Properties.Settings.Default.CurrenciesImages);

            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public string InsertFileOnPath_V6(PORegisteredDetails po)
        {
            string CompletePath = "";

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                CompletePath = mgr.InsertFileOnPath_V6(po, connectionString, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CompletePath;
        }

        /// <summary>
        /// [pramod.misal][04-09-2025][GEOS2-9041] https://helpdesk.emdep.com/browse/GEOS2-9041
        /// <param name="offer"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool UpdateOffer_V2670(Int64 IdEmail,LinkedOffers offer, List<Emailattachment> POAttachementsList, Int64 IdCustomerGroup, Int64 IdCustomerPlant)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.UpdateOffer_V2770(IdEmail,offer, ConnectionString, POAttachementsList, IdCustomerGroup, IdCustomerPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<POType> OTM_GetAllPOTypeStatus_V2670()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetAllPOTypeStatus_V2670(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //[rdixit][04.09.2025][GEOS2-9416] To get timezone as per service connected plant region
        public string GetTimezoneByServiceUrl_V2670(string serviceUrl)
        {
            string TimeZoneIdentifier = string.Empty;
            try
            {
                OTMManager mgr = new OTMManager();
                string Emdep_geos_ConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                TimeZoneIdentifier = mgr.GetTimezoneByServiceUrl_V2670(Emdep_geos_ConnectionString, serviceUrl);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return TimeZoneIdentifier;
        }

        //[pooja.jadhav][04-09-2025][GEOS2-9322] OTM - Limit the Sender list in Edit PO
        public List<PORegisteredDetails> OTM_GetPOSender_V2670()
        {
            List<PORegisteredDetails> POSenderList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                POSenderList = mgr.OTM_GetPOSender_V2670(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return POSenderList;
        }

        /// <summary>
        /// [Rahul.Gadhave][09.09.2025][GEOS2-9281]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<POEmployeeInfo> GetJob_DescriptionsList_V2670()
        {
            List<POEmployeeInfo> Countries = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Countries = mgr.GetJob_DescriptionsList_V2670(connectionString);
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
        /// <summary>
       //[Rahul.Gadhave][GEOS2-9437][10 - 09 - 2025]
        /// </summary>
        /// <param name="po"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public bool ChectCustomerpurchaseOrderExist_V2670(PORegisteredDetails po)
        {
            bool isSave = false;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                isSave = mgr.ChectCustomerpurchaseOrderExist_V2670(po, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isSave;
        }


        /// <summary>
        /// [Rahul.Gadhave][09.09.2025][GEOS2-9281]
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<POEmployeeInfo> GetAddedJob_DescriptionsList_V2670()
        {
            List<POEmployeeInfo> Countries = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Countries = mgr.GetAddedJob_DescriptionsList_V2670(connectionString);
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

        public bool UpdateConfirmationJDSetting_V2670(List<POEmployeeInfo> po)
        {
            bool isSave = false;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isSave = mgr.UpdateConfirmationJDSetting_V2670(po, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isSave;
        }

        //[Rahul.Gadhave][GEOS2-9150][Date:15-09-2025]
        public List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2670(string SelectedIdPlant, string SelectedIdGroup, GeosAppSetting geosAppSetting)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.OTM_GetLinkedofferByIdPlantAndGroup_V2670(ConnectionString, SelectedIdPlant, SelectedIdGroup, Properties.Settings.Default.CurrenciesImages, geosAppSetting, Properties.Settings.Default.CommericalPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][24.09.2025][GEOS2-9576]https://helpdesk.emdep.com/browse/GEOS2-9576
        public Int64 InsertOffer_V2670(Quotation q)
        {
            Int64 idoffer = 0;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                idoffer = mgr.InsertOffer_V2670(q, connectionString, Properties.Settings.Default.WorkingOrdersPath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return idoffer;
        }

        //[rdixit][GEOS2-9624][03.10.2025]
        public List<Email> GetEmailCreatedIn_V2670()
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionStringGEOS = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetEmailCreatedIn_V2670(ConnectionStringGEOS, Properties.Settings.Default.PoAnalyzerEmailToCheck);
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
        /// [001]  ///[Rahul.Gadhave][GEOS2-9517][Date:07-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9517
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public List<ShippingAddress> OTM_GetShippingAddressForShowAll_V2680(int IdCustomerPlant)
        {
            List<ShippingAddress> ShippingAddressList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                ShippingAddressList = mgr.OTM_GetShippingAddressForShowAll_V2680(connectionString, IdCustomerPlant);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ShippingAddressList;
        }

        //[Rahul.Gadhave][GEOS2-9517][Date:07-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9517
        public List<ShippingAddress> OTM_GetShippingAddress_V2680(int IdSite)
        {
            List<ShippingAddress> ShippingAddressList = null;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                ShippingAddressList = mgr.OTM_GetShippingAddress_V2680(connectionString, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ShippingAddressList;
        }

        public List<People> GetPeoples()
        {
            List<People> peoples = null;

            try
            {
                OTMManager mgr = new OTMManager();
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

        public int GetPeopleDetailsbyEmpCode_V2680(string employeeCodes)
        {
            bool isSave = false;
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPeopleDetailsbyEmpCode_V2680(connectionString, employeeCodes);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public People GetContactsByIdPermission_V2680(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission, int idperson)
        {
            People people = null;

            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                people = mgr.GetContactsByIdPermission_V2680(connectionString, idActiveUser, idUser, idSite, idPermission, idperson);
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
        /// <summary>
        /// [ashish.malkhede][GEOS2-9207] https://helpdesk.emdep.com/browse/GEOS2-9207
        /// </summary>
        /// <param name="IdAttachment"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public PORequestDetails GetPODetailsbyAttachment_V2680(int IdAttachment)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPODetailsbyAttachment_V2680(ConnectionString, IdAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[pramod.misal][GEOS2-9601][29-10-2025]
        public int GetPODetails_V2680(string OfferCode)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPODetails_V2680(ConnectionString, OfferCode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<int> GetIdOffersByCustomerPurchaseOrder_V2680(int idCustomerPurchaseOrder)
        {
            List<int> offerIds = new List<int>();
            try
            {
                OTMManager mgr = new OTMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                offerIds = mgr.GetIdOffersByCustomerPurchaseOrder_V2680(connectionString, idCustomerPurchaseOrder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return offerIds;
        }

        public void OTM_InsertPORequestLinkedOffer_V2680(Int64 IdPORequest, List<int> IdOfferList)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                mgr.OTM_InsertPORequestLinkedOffer_V2680(IdPORequest, IdOfferList, ConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LinkedOffers> GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680(Int32 IdCustomer, int IdPlant, string amount)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680(ConnectionString, IdCustomer,IdPlant,amount);
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
        /// [rahul.gadhave][GEOS2-9878] https://helpdesk.emdep.com/browse/GEOS2-9878
        /// </summary>
        /// <param name="IdEmail"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public PORequestDetails GetPODetailsbyIdEmail_V2690(Int64 IdEmail)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPODetailsbyIdEmail_V2690(ConnectionString, IdEmail);
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
        /// [rahul.gadhave][GEOS2-9878] https://helpdesk.emdep.com/browse/GEOS2-9878
        /// </summary>
        /// <param name="IdEmail"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{ServiceException}"></exception>
        public PORequestDetails GetPODetailsbyEmail_V2690(List<string> allEmails)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPODetailsbyEmail_V2690(ConnectionString, allEmails);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][20.11.2025][PredefinedGeometryStock2DModel-9429] https://helpdesk.emdep.com/browse/GEOS2-9429
        public List<PORequestDetails> GetPORequestDetails_V2690(DateTime FromDate, DateTime Todate, int Idcurrency, long plantId)
        {
            OTMManager mgr = new OTMManager();
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.GetPORequestDetails_V2690(ConnectionString, FromDate, Todate, Idcurrency, plantId, Properties.Settings.Default.CurrenciesImages);

            }
            catch (Exception ex)
            {

                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-9880][Date:26-11-2025]
        public PdfResultDto GenerateEmailPdf_V2690(Int64 IdEmail, string Emailbody, string Year, string CustomerGroup, string Name, string Code, string html)
        {
         
            try
            {
                string CommericalPath = Properties.Settings.Default.CommericalPath;
                string baseFolder = Path.Combine(CommericalPath, Year.ToString(), $"{CustomerGroup} - {Name}", Code, "03 - PO");
                if (!Directory.Exists(baseFolder))
                    Directory.CreateDirectory(baseFolder);

                string emailId = IdEmail.ToString();
                //string htmlFileName = $"GoAheadPOEmail_{emailId}.html";
                //string htmlSavePath = Path.Combine(baseFolder, htmlFileName);
                //File.WriteAllText(htmlSavePath, html);
                HtmlToPdf converter = new HtmlToPdf
                {
                    Options =
                    {
                        PdfPageSize = PdfPageSize.A4,
                        PdfPageOrientation = PdfPageOrientation.Portrait,
                        MarginTop = 20,
                        MarginBottom = 20,
                        MarginLeft = 20,
                        MarginRight = 20
                    }
                };
                PdfDocument pdfDoc = converter.ConvertHtmlString(html);

                
                if (!Directory.Exists(baseFolder))
                    Directory.CreateDirectory(baseFolder);

                string fileName = $"GoAheadPOEmail_{emailId}.pdf";
                string savePath = Path.Combine(baseFolder, fileName);
                // 6. Save PDF
                pdfDoc.Save(savePath);
                pdfDoc.Close();
                return new PdfResultDto
                {
                    FileBytes = File.ReadAllBytes(savePath),
                    FileName = fileName,
                    FileFullPath= savePath
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate PDF from email.", ex);
            }
        }
    }
}
