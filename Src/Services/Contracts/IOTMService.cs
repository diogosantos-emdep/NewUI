using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.OTMDataModel;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ShippingAddress = Emdep.Geos.Data.Common.OTM.ShippingAddress;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOTMService" in both code and config file together.
    [ServiceContract]
    public interface IOTMService
    {

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddOTMEmails(email emailDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2550. Use AddEmails_V2550 instead.")]
        bool AddEmails(Email emailDetails);

        //[rdixit][30.07.2024][GEOS2-6005]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmails_V2550(Email emailDetails);

        #region [GEOS2-5867][rdixit][25.07.2024]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Email> GetAllUnprocessedEmails();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BlackListEmail> GetAllBlackListEmails();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues(byte key);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddPORequest(PORequest poRequest);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddPODetails(List<CustomerDetail> poDetailList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TemplateSetting> GetAllTags();
        #endregion

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetPORequestDetails_V2580 instead.")]
        List<Currency> GetAllCurrencies();
        /// <summary>
        /// [001][ashish.malkhede][03102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetPORequestDetails_V2580 instead.")]
        List<PORequestDetails> GetPORequestDetails(DateTime FromDate, DateTime ToDate, long plantId, string plantConnection);

        /// <summary>
        /// [001] [ashish.malkhede][08-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6460
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="plantId"></param>
        /// <param name="plantConnection"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetPORegisteredDetails_V2590 instead.")]
        List<PORegisteredDetails> GetPORegisteredDetails(DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter filter);
        /// <summary>
        /// [001][ashish.malkhede][04102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetPORequestDetails_V2580 instead.")]
        List<Company> GetAllSitesWithImagesByIdUser(Int32 idUser);

        // [pramod.misal][04-10-2024][GEOS2-6520]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POStatus> OTM_GetAllPOWorkflowStatus();

        // [ashish.malkhede][13-11-2024][GEOS2-6460]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POType> OTM_GetAllPOTypeStatus();

        //[rdixit][GEOS2-5868][14.10.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TemplateSetting> GetTemplateByCustomer(int idCustomer);

        //[rdixit][GEOS2-5868][14.10.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetAllCustomers_V2590 instead.")]
        List<Customer> GetAllCustomers();

        //[rdixit][GEOS2-5868][14.10.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use AddPODetails_V2610 instead.")]
        bool AddPODetails_V2570(List<CustomerDetail> poDetailList);

        //[rdixit][GEOS2-5868][14.10.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Email> GetAllUnprocessedEmails_V2570();

        //[rdixit][GEOS2-5868][14.10.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePORequestStatus(PORequest poRequest);

        //[rdixit][GEOS2-5868][14.10.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use GetEmailCreatedIn_V2610 instead.")]
        List<Email> GetEmailCreatedIn_V2570();

        //[rdixit][GEOS2-5868][14.10.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use AddEmails_V2610 instead.")]
        bool AddEmails_V2570(Email emailDetails);

        //[rdixit][07.11.2024][GEOS2-6600]
        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<PORequestDetails> GetPORequestDetails_V2580(DateTime FromDate, DateTime ToDate, long plantId, string plantConnection);

        //[RGadhave][12.11.2024][GEOS2-6461]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetAllCustomers_V2610 instead.")]
        List<PORequestDetails> GetPORequestDetails_V2580(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId, string plantConnection);

        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetails_V2580(Int32 idUser);

        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POType> OTM_GetPOTypes_V2580();

        //[pooja.jadhav][14-11-2024][GEOS2-GEOS2-6460]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use OTM_GetCustomerPlant_V2590 instead.")]
        List<CustomerPlant> OTM_GetCustomerPlant_V2580();

        //[pooja.jadhav][18-11-2024][GEOS2-GEOS2-6460]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use OTM_GetPOSender_V2590 instead.")]
        List<string> OTM_GetPOSender_V2580();

        //[pramod.misal][GEOS2-6460][28-11-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Currency> GetAllCurrencies_V2590();

        //[pramod.misal][GEOS2-6460][28-11-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllSitesWithImagesByIdUser_V2590(Int32 idUser);

        // [Rahul.Gadhave][25-11-6463][GEOS2-6460]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use OTM_GetShippingAddress_V2620 instead.")]
        List<ShippingAddress> OTM_GetShippingAddress_V2590(int IdCustomerPlant);

        /// <summary>
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="plantId"></param>
        /// <param name="plantConnection"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetPORegisteredDetails_V2620 instead.")]
        List<PORegisteredDetails> GetPORegisteredDetails_V2590(DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter filter);
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsProfileUpdate(string EmployeeCode);

        //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetPORegisteredDetails_V2620 instead.")]
        List<LinkedOffers> OTM_GetLinkedofferByIdCustomerPlant(Int32 SelectedIdCustomerPlant, Int64 SelectedIdPO, GeosAppSetting geosAppSetting);
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetAllCustomers_V2590();
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPlant> OTM_GetCustomerPlant_V2590();
        /// [001] [Rahul.Gadhave][25-11-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Currency> GetAllPOCurrencies_V2590();

        //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORegisteredDetails> OTM_GetPOSender_V2590();

        //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetOTM_GetLinkedOffers_V2620 instead.")]
        List<LinkedOffers> GetOTM_GetLinkedOffers_V2590(PORegisteredDetails poRegisteredDetails);

        // [001][ashish.malkhede][07-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6464
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntryByPOOffer> GetAllPOChangeLog_V2590(long idPO);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePurchaseOrder_V2590(PORegisteredDetails po);

        /// <summary>
        /// [001][ashish.malkhede][11-12-2024] https://helpdesk.emdep.com/browse/GEOS2-6463
        /// </summary>
        /// <param name="IdAppSetting"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting);
        //[pramod.misal][GEOS2-6462][18-11-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyDetailsById_V2580(Int32 idSite);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsByIdPermission_V2590(Int32 IdPerson);

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetPOReceptionEmailToFeilds_V2590(Int64 IdPO);

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetPOReceptionEmailCCFeilds_V2660 instead.")]
        List<People> GetPOReceptionEmailCCFeilds_V2590(Int64 IdPO);

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetPeopleByEMDEPcustomer_V2630 instead.")]
        List<People> GetPeopleByEMDEPcustomer_V2590();

        //[pooja.jadhav][GEOS2-6465][18-12-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Language> GetAllLanguages_V2590();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetEmployeeCodeByUserID(Int64 IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetCommercialPath();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetOfferAmountByCurrencyConversion_V2590(int PreIdCurrency, int idCurrency, long IdPO);

        //[pramod.misal][GEOS2-6465][16.12.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string ReadMailTemplate(string templateName);

        //[pramod.misal][GEOS2-6465][16.12.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use POEmailSend_V2630 instead.")]
        bool POEmailSend_V2590(string EmailSubject, string htmlEmailtemplate, ObservableCollection<People> toContactList, ObservableCollection<People> CcContactList,string fromMail,List<LinkedResource> imageList);

        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetAllOtImportTemplate_V2610 instead.")]
        List<OtRequestTemplates> GetAllOtImportTemplate_V2600();
        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeletetImportTemplate_V2600(Int32 IdOTRequestTemplate);

        //[pooja.jadhav][08-01-2025][GEOS2-6831]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetCustomerDetails_V2600();

        //[pooja.jadhav][08-01-2025][GEOS2-6831]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Regions> GetRegions_V2600();

        //[pooja.jadhav][08-01-2025][GEOS2-6831]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountriesDetails_V2600();

        //[pooja.jadhav][08-01-2025][GEOS2-6831]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version. Use AddUpdateOTRequestTemplates_V2610 instead.")]
        bool AddUpdateOTRequestTemplates(OtRequestTemplates template);

        //[pooja.jadhav][09-01-2025][GEOS2-6831]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetCode();

        //[pooja.jadhav][20-01-2025][GEOS2-6734]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<int, string> GetPoAnalizerTag_V2600();

        //[pooja.jadhav][20-01-2025][GEOS2-6734]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<int, string> GetPORequestTemplateFieldType_V2600();

        //[pooja.jadhav][21-01-2025][GEOS2-6734]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<int, string> GetPORequestTemplateFieldTypeForPDF_V2600();

        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerCountriesDetails> GetAllCustomersAndCountries_V2600();

        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POAnalyzerOTTemplate> GetOTRequestTemplateByCustomer_V2600(int isCustomer);

        //[ashish.malkhede][23-01-2025][GEOS2-6735]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetPdfFilePath(Emailattachment attchedFile);

        //[pooja.jadhav][24-01-2025][GEOS2-6734]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountriesByCustomerAndRegion(int IdCustomer, int IdRegion);

        //[pooja.jadhav][24-01-2025][GEOS2-6734]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
   
        List<CustomerPlant> GetPlantByCustomerAndCountry(int IdCustomer, int IdCountry);

        //[pramod.misal][GEOS2-6735][27-01-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version. Use AddUpdateOTRequestTemplates_V2610 instead.")]
        List<Email> GetAllUnprocessedEmails_V2600();

        //[pramod.misal][GEOS2-6735][23.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POAnalyzerOTTemplate> GetOTRequestExcelTemplateByCustomer_V2600(int isCustomer);

        //[rahul.gadhave][GEOS2-6720][29.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORequestDetails> GetPORequestDetails_V2610(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId, string plantConnection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtRequestTemplates> GetAllOtImportTemplate_V2610();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtRequestTemplates> OTM_GetAllMappingFieldsData_V2610(OtRequestTemplates ObjOtRequestTemplates);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OtRequestTemplates AddUpdateOTRequestTemplates_V2610(OtRequestTemplates template);

        //[pramod.misal][03-02-2025][GEOS2-6831]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Regions> GetRegions_V2610(int idCustomer);
        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeletetTextAndLocation_V2610(long IdOTRequestTemplate);
        //  [Rahul.Gadhave][GEOS2-6910][Date:03/02/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeletetCellFields_V2610(long IdOTRequestTemplate);

        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmails_V2610(Email emailDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddPODetails_V2610(List<CustomerDetail> poDetailList);

        //[pramod.misal][04.02.2025][GEOS2 - 6726]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POEmployeeInfo> GetPOEmployeeInfoList_V2610();

        //[pramod.misal][GEOS2-6720][19.02.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610_V. Use GetPORequestDetails_V2620 instead.")]
        List<PORequestDetails> GetPORequestDetails_V2610_V1(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId, string plantConnection);
       
        //[rahul.gadhave][GEOS2-6799][Date:19/02/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetEmailCreatedIn_V2670 instead.")]
        List<Email> GetEmailCreatedIn_V2610();

        //[pramod.misal][19.02.2025][GEOS2 - 6719]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerContacts> GetAllCustomerContactsList_V2620();

        //[pramod.misal][GEOS2-7036][27-02-2025] https://helpdesk.emdep.com/browse/GEOS2-7036
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORegisteredDetails> GetPORegisteredDetails_V2620(DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter filter);

        //[ashish.malkhede][GEOS2-7042][12-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORegisteredDetails> GetPORegisteredDetails_V2620_V1(DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter filter);

        //[pramod.misal][GEOS2-7036][27-02-2025] https://helpdesk.emdep.com/browse/GEOS2-7036
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPlant> OTM_GetCustomerPlant_V2620();

        //[Rahul.Gadhave][GEOS-7040][Date:28-02-2025][https://helpdesk.emdep.com/browse/GEOS2-7040]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> GetOTM_GetLinkedOffers_V2620(PORegisteredDetails poRegisteredDetails);

        //[rdixit][04.03.2025][GEOS2-6724]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetPoAttachmentByte(string attachmentPath);
        //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int? GetOfferDetails_V2620(string OfferCode);
        //[Rahul.Gadhave][GEOS2-6723][Date:05-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void OTM_InsertPORequestLinkedOffer_V2620(Int64 IdPORequest, int? IdOffer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORequestDetails> GetPORequestDetails_V2620(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId, string plantConnection);

        //[Rahul.Gadhave][GEOS2-7056][Date:07-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use OTM_GetShippingAddress_V2620 instead.")]
        List<ShippingAddress> OTM_GetShippingAddress_V2620(int IdSite);

        //[pooja.jadhav][GEOS2-7054][10-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORegisteredDetails> OTM_GetPOSender_V2620();

        //[Rahul.Gadhave][GEOS2-7226][Date:24-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use OTM_GetShippingAddressForShowAll_V2680 instead.")]
        List<ShippingAddress> OTM_GetShippingAddressForShowAll_V2630(int IdCustomerPlant);

        //[ashish.malkhede][GEOS2-7049][25-03-2025] https://helpdesk.emdep.com/browse/GEOS2-7049
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORegisteredDetails> GetPORegisteredDetails_V2630(DateTime FromDate, DateTime ToDate, int IdCurrency, long plantId, string plantConnection, PORegisteredDetailFilter filter);
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool POEmailSend_V2630(string EmailSubject, string htmlEmailtemplate, List<People> toContactList, List<People> CcContactList, string fromMail, List<LinkedResource> imageList);
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetPeopleByJobDescriptions_V2630(GeosAppSetting CustomerPOConfirmationJD, long plantIds);
        //[Rahul.Gadhave][GEOS2-7079][Date:25-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetPeopleByEMDEPcustomer_V2630(PORegisteredDetails poregistereddetailsforemail);

        //[ashish.malkhede][GEOS2-6735][27-01-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetPORequestDetails_V2620 instead.")]
        List<Email> GetAllEmailsBlankColumns();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePORequestGroupPlant_V2630(Email poRequest);
        //[pramod.misal][28.03.2025][GEOS2 - 7718]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateSenderIdPerson_V2630(Email email);

        //[pramod.misal][28.03.2025][GEOS2 - 7718]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateCCIdPerson_V2630(Email email);

        //[pramod.misal][28.03.2025][GEOS2 -7718 ]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateToIdPerson_V2630(Email email);

        //[pramod.misal][04.02.2025][GEOS2 - 7718]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POEmployeeInfo> GetPOEmployeeInfoList_V2630();

        //[pramod.misal][GEOS2-7720][01-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Email> GetAllEmailsBlankColumns_V2630();

        //[ashish.malkhede][GEOS2-7724][02-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORequestDetails> GetPORequestDetails_V2630(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId, string plantConnection);

        //[ashish.malkhede][GEOS-7049][03-04-2025][https://helpdesk.emdep.com/browse/GEOS2-7049]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> GetOTM_GetLinkedOffers_V2630(PORegisteredDetails poRegisteredDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> GetOTM_GetLinkedOffers_V2660(PORegisteredDetails poRegisteredDetails);

        //[pramod.misal][GEOS2-7724][07/04/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmails_V2630(Email emailDetails);

        //[pooja.jadhav][GEOS2-7052][11-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetAllCustomers_V2630();
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetLookupValues_V2660 instead.")]
        ObservableCollection<LookupValue> GetLookupValues_V2640();

        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use OTM_GetPoRequestLinkedOffers_V2660 instead.")]
        List<LinkedOffers> OTM_GetPoRequestLinkedOffers_V2640(string Offers);
        // [Rahul.Gadhave][GEOS2-7246] [Date:15-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use OTM_GetPoRequestLinkedPO_V2660 instead.")]
        List<LinkedOffers> OTM_GetPoRequestLinkedPO_V2640(string OfferCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> GetPoRequestOfferTo_V2640(LinkedOffers Obj);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetCarriageMethod_V2660 instead.")]
        ObservableCollection<LookupValue> GetCarriageMethod_V2640();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> OTM_GetPoRequestOfferType_V2640();

        ///[pramod.misal][23-04-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> OTM_GetLinkedofferByIdCustomerPlant_V2630(Int32 SelectedIdCustomerPlant, Int64 SelectedIdPO, GeosAppSetting geosAppSetting);

        //[pramod.misal][GEOS2-7724][23-04-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetPORequestDetails_V2650 instead.")]
        List<PORequestDetails> GetPORequestDetails_V2640(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId, string plantConnection);

        //[pramod.misal][23-04-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2640. Use GetPORequestDetails_V2650 instead.")]
        List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2640(string SelectedIdPlant, string SelectedIdGroup, GeosAppSetting geosAppSetting);

        // [001][ashish.malkhede] https://helpdesk.emdep.com/browse/GEOS2-7251
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use UpdateOffer_V2670 instead.")]
        bool UpdateOffer(LinkedOffers offer, List<Emailattachment> POAttachementsList);


        //[pramod.misal][28-04-2024][GEOS2-7247]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string OTM_GetEmailBodyByIdEmail_V2640(Int64 IdEmail);

        //[Rahul.gadhave][GEOS2-7246][Date:03-06-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePoRequestStatus_V2640(LinkedOffers offer, List<Emailattachment> POAttachementsList);

        // [001][ashish.malkhede][06-05-2024] https://helpdesk.emdep.com/browse/GEOS2-7254
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntryByPORequest> GetAllPORequestChangeLog_V2640(long idPORequest, string idOffer);

        //[pramod.misal][07-05-2025][GEOS2-7248]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use OTM_GetEmailAttachementByIdEmail_V2660 instead.")]
        List<Emailattachment> OTM_GetEmailAttachementByIdEmail_V2640(Int64 IdEmail);

        //[Rahul.Gadhave][GEOS2-7253][Date:07/05/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetCommercialOffersPath_V2640();

        //[pooja.jadhav][GEOS2-7252][09-05-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PORegisteredDetails GetPORegisteredDetailsByIdPo(int IdPo, int IdCurrency);

        //[pooja.jadhav][GEOS2-7252][09-05-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsPoNumberExist(string code);

        //[pooja.jadhav][GEOS2-7252][09-05-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PORequestDetails GetPODetailsbyAttachment(int IdAttachment);

        /// [001][rahul.gadhave][13-05-2025] https://helpdesk.emdep.com/browse/GEOS2-7252
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertPurchaseOrder_V2640(PORegisteredDetails po);
        //[Rahul.Gadhave][GEOS2-8339][Date:06-06-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORequestDetails> GetPORequestDetails_V2650(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId);

        //[pooja.jadhav][GEOS2-8342][11-06-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use AddChangeLogByPORequest_V2660 instead.")]
        void AddChangeLogByPORequest(ObservableCollection<LogEntryByPORequest> logEntries);

        //[pramod.misal][GEOS2-8772][Date:30-06-2025]https://helpdesk.emdep.com/browse/GEOS2-8772
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use GetPORequestDetails_V2670 instead.")]
        List<PORequestDetails> GetPORequestDetails_V2660(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId);

        // [Rahul.Gadhave][GEOS2-8655][Date:08-07-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ObservableCollection<LookupValue> GetCarriageMethod_V2660();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> OTM_GetPoRequestLinkedOffers_V2660(string Offers);

        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Email> GetAllUnprocessedEmails_V2660();

        //[rdixit][GEOS2-9020][23.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmails_V2660(Email emailDetails);

        //[rdixit][GEOS2-9020][23.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddUniqueOffersToPORequest_V2660(Int64 IdPORequest, string quotationCode);

        //[rdixit][GEOS2-9020][23.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePORequestGroupPlant_V2660(Email poRequest);

        //[Rahul.Gadhave][GEOS2-9080][Date:30-07-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2660(string SelectedIdPlant, string SelectedIdGroup, GeosAppSetting geosAppSetting);

        //[ashish.malkhede][08-07-2025] https://helpdesk.emdep.com/browse/GEOS2-9105
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 InsertPurchaseOrder_V2660(PORegisteredDetails po);

        //[Rahul.Gadhave][GEOS2-9113][Date:01-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetPOReceptionEmailCCFeilds_V2660(Int64 IdPO);

 
        /// [001][pramod.misal][10-12-2024] https://helpdesk.emdep.com/browse/GEOS2-9109
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePurchaseOrder_V2660(PORegisteredDetails po);

        //[rdixit][GEOS2-9141][02.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetUserPlant_V2660(int idperson);

        //[rdixit][GEOS2-9141][02.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompanyDetailsList_V2660();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool OTM_CheckOfferExistsInGeos_V2660(int year, int number);

        //[rdixit][GEOS2-9141][02.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer GetOfferDetailsByYearAndNumber_V2660(int year, int number);

        //[rdixit][GEOS2-9141][02.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 InsertOffer_V2660(Quotation q);

        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Quotation OTM_GetQuotationByCode_V2660(string Code, int numOT, string otcode, Quotation quo);

        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool OTM_CheckQuotationExistsInGeos_V2660(string Code);

        //[rdixit][GEOS2-9141][02.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 InsertQuotations_V2660(Quotation quotation);
        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertCPDetections_V2660(List<CPDetection> detections);

        /// [pramod.misal]][05-08-2025][GEOS2-9167]https://helpdesk.emdep.com/browse/GEOS2-9167
        [OperationContract]
        [FaultContract(typeof(ServiceException))] 
        bool UpdateOffer_V2660(LinkedOffers offer, List<Emailattachment> POAttachementsList);

        /// [pramod.misal]][05-08-2025][GEOS2-9167]https://helpdesk.emdep.com/browse/GEOS2-9049
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntryByPORequest> GetAllPORequestChangeLog_V2660(long idPORequest, string idOffer);
        
        //[Rahul.gadhave][GEOS2-9191][Date:11-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ObservableCollection<LookupValue> GetLookupValues_V2660();

        //[rdixit][GEOS2-9137][12.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddChangeLogByPORequest_V2660(ObservableCollection<LogEntryByPORequest> logEntries);

        //[pooja.jadhav][GEOS2-9179][13-08-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetAllCustomers_V2660(int IdCustomer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Emailattachment> OTM_GetEmailAttachementByIdEmail_V2660(Int64 IdEmail);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> OTM_GetPoRequestLinkedPO_V2660(string OfferCode);

        //[Rahul.Gadhave][GEOS2-9041][Date:02-09-2025] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORequestDetails> GetPORequestDetails_V2670(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string InsertFileOnPath_V6(PORegisteredDetails po);

        //[pramod.misal][04-09-2025][GEOS2-9041] https://helpdesk.emdep.com/browse/GEOS2-9041
        [OperationContract]
        [FaultContract(typeof(ServiceException))]      
        bool UpdateOffer_V2670(Int64 IdEmail,LinkedOffers offer, List<Emailattachment> POAttachementsList, Int64 IdCustomerGroup, Int64 IdCustomerPlant);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POType> OTM_GetAllPOTypeStatus_V2670();

        //[rdixit][04.09.2025][GEOS2-9416] To get timezone as per service connected plant region
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetTimezoneByServiceUrl_V2670(string serviceUrl);

        //[pooja.jadhav][04-09-2025][GEOS2-9322] OTM - Limit the Sender list in Edit PO
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORegisteredDetails> OTM_GetPOSender_V2670();

        /// [Rahul.Gadhave][09.09.2025][GEOS2-9281]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POEmployeeInfo> GetJob_DescriptionsList_V2670();

        /// [Rahul.Gadhave][09.09.2025][GEOS2-9281]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<POEmployeeInfo> GetAddedJob_DescriptionsList_V2670();

        //[Rahul.Gadhave][GEOS2-9437][10 - 09 - 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ChectCustomerpurchaseOrderExist_V2670(PORegisteredDetails po);

        /// [Rahul.Gadhave][09.09.2025][GEOS2-9281]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateConfirmationJDSetting_V2670(List<POEmployeeInfo> jobs);

        //[Rahul.Gadhave][GEOS2-9150][Date:15-09-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> OTM_GetLinkedofferByIdPlantAndGroup_V2670(string SelectedIdPlant, string SelectedIdGroup, GeosAppSetting geosAppSetting);

        //[pramod.misal][24.09.2025][GEOS2-9576]https://helpdesk.emdep.com/browse/GEOS2-9576
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 InsertOffer_V2670(Quotation q);

        //[rdixit][GEOS2-9624][03.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Email> GetEmailCreatedIn_V2670();

        //[Rahul.Gadhave][GEOS2-9517][Date:10-06-2025] https://helpdesk.emdep.com/browse/GEOS2-9517
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ShippingAddress> OTM_GetShippingAddressForShowAll_V2680(int IdCustomerPlant);
        //[Rahul.Gadhave][GEOS2-9517][Date:07-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9517
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ShippingAddress> OTM_GetShippingAddress_V2680(int IdSite);

        //[pramod.misal][GEOS2-9324][01 - 10 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9324
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetPeoples();

        //[pramod.misal][GEOS2-9324][01 - 10 - 2025] https://helpdesk.emdep.com/browse/GEOS2-9324
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetPeopleDetailsbyEmpCode_V2680(string EmployeeCodes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        People GetContactsByIdPermission_V2680(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission, int idperson);

        //[pramod.misal][GEOS2-9601][29-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetPODetails_V2680(string PoCode);

        //[pramod.misal][GEOS2-9601][29-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<int> GetIdOffersByCustomerPurchaseOrder_V2680(int idCustomerPurchaseOrder);

        //[pramod.misal][GEOS2-9601][29-10-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void OTM_InsertPORequestLinkedOffer_V2680(Int64 IdPORequest, List<int> IdOfferList);

        //[Rahul.Gadhave][GEOS-7040][Date:28-02-2025][https://helpdesk.emdep.com/browse/GEOS2-7040]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LinkedOffers> GetOTM_GetLOffersByIdcutomerIdplantAmount_V2680(Int32 IdCustomer, int IdPlant, string amount);

        //[ashish.malkhede][GEOS2-9207] https://helpdesk.emdep.com/browse/GEOS2-9207
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PORequestDetails GetPODetailsbyAttachment_V2680(int IdAttachment);

        //[rahul.gadhave][GEOS2-9878] https://helpdesk.emdep.com/browse/GEOS2-9878
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PORequestDetails GetPODetailsbyIdEmail_V2690(Int64 IdEmail);

        //[rahul.gadhave][GEOS2-9878] https://helpdesk.emdep.com/browse/GEOS2-9878
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PORequestDetails GetPODetailsbyEmail_V2690(List<string> allEmails);

        //[pramod.misal][20.11.2025][PredefinedGeometryStock2DModel-9429] https://helpdesk.emdep.com/browse/GEOS2-9429
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PORequestDetails> GetPORequestDetails_V2690(DateTime FromDate, DateTime ToDate, Int32 Idcurrency, long plantId);

        //[Rahul.Gadhave][GEOS2-9880][Date:26-11-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PdfResultDto GenerateEmailPdf_V2690(Int64 IdEmail, string Emailbody, string Year, string CustomerGroup, string Name, string Code, string html);
    }
}
