using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.OptimizedClass;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Emdep.Geos.Services.Contracts
{
    [ServiceContract]
    public interface ICrmService
    {

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSelectedUsersTargetForecast(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddLogEntryBySite(LogEntryBySite logEntryBySite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntryBySite> GetAllLogEntriesByIdSite(Int64 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyDetailsById(Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSelectedOffersPipeline(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0);


        [OperationContract(Name = "GetSelectedOffersPipelineDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetOffersPipeline_V2035 instead.")]
        IList<Offer> GetSelectedOffersPipeline(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetAllCountries();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreateFolderOffer(Offer offer, bool? isNewFolderForOffer = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmdepSite GetEmdepSiteById(Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ModelAppointment> GetSelectedUsersOffersModelAppointment(byte idCurrency, string idUser, Int64 accountingYear, Company compdetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetSelectedUsersOffersModelAppointmentDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<ModelAppointment> GetSelectedUsersOffersModelAppointment(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<SalesTargetBySite> GetSelectedUsersSalesStatusTargetDashboard(byte idCurrency, string idUser, Int64 accountingYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetOffersPipeline(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOffersPipelineDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetOffersPipeline_V2035 instead.")]
        IList<Offer> GetOffersPipeline(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise(byte idCurrency, string idUser, Int64 accountingYear, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetSelectedUsersOfferLeadSourceCompanyWiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferLeadSourceCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOfferLeadSourceCompanyWiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferLeadSourceCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Company> GetCompanies();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use AddOffer_V2033 instead.")]
        Offer AddOffer(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer GetOfferDetailsById(Int64 idOffer, Int32 idUser, byte idCurrency, Int64 accountingYear, Company company);

        [OperationContract(Name = "GetOfferDetailsByIdDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetOfferDetailsById_V2033 instead.")]
        Offer GetOfferDetailsById(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company compdetails, Int32 idUserPermission);

        [OperationContract(Name = "Method2")]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, string idUser, Int64 accountingYear, Company compdetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "SelectedUsersOffersWithoutPurchaseOrderDateWise")]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetSelectedUsersOffersWithoutPurchaseOrderReturnListDatatable(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetSelectedUsersOrdersDatatable(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetSelectedUsersOrdersDatatableDateWise")]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetSelectedUsersOrdersDatatable(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetOrdersDatatable(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOrdersDatatableDateWise")]
        [FaultContract(typeof(ServiceException))]
        OffersOptionsList GetOrdersDatatable(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetTopOffers(byte idCurrency, Int32 idUser, Int32 idZone, Int32 offerLimit, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<SalesTargetBySite> GetSalesStatusTargetDashboard(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSalesStatus(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetSalesStatusDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetSalesStatus_V2035 instead.")]
        IList<Offer> GetSalesStatus(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSelectedUserSalesStatus(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetSelectedUserSalesStatusDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetSelectedUserSalesStatus_V2035 instead.")]
        IList<Offer> GetSelectedUserSalesStatus(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersSalesStatusByMonth(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetSelectedUsersSalesStatusByMonthDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersSalesStatusByMonth(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2031. Use UpdateOffer_V2031 instead.")]
        bool UpdateOffer(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        [ObsoleteAttribute("This method will be removed after version V2033. Use UpdateOfferForParticularColumn_V2033 instead.")]
        bool UpdateOfferForParticularColumn(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Currency> GetCurrencies();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Currency> GetCurrencyByExchangeRate();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Double GetOfferMaxValue();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<CurrencyConversion> GetCurrencyConversions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<People> GetPeopleContacts(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        //  [ObsoleteAttribute("This method will be removed after version V2033. Use UpdateContact_V2033 instead.")]
        bool UpdateContact(People people);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use AddContact_V2033 instead.")]
        People AddContact(People people);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Company> GetSiteInfo(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<OfferType> GetOfferType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use MakeOfferCode_V2040 instead.")]
        string MakeOfferCode(byte? idOfferType, Int32 idCustomer, string connectPlantstr, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSalesStatusByWeek(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Template> GetTemplates();

        [OperationContract]
        List<Template> GetTemplatesByIdTemplateType(Int64 idTemplateType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetNextNumberOfOfferFromCounters_V2040 instead.")]
        Int64 GetNextNumberOfOfferFromCounters(byte? idOfferType, string connectPlantstr, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetNextNumberOfSuppliesFromGCM(byte? idOfferType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsBySalesOwnerId(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCustomersBySalesOwnerId(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCurrencyConversion(List<CurrencyConversion> currencyConversions);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetAllLogEntriesByIdOffer_V2040 instead.")]
        List<LogEntryByOffer> GetAllLogEntriesByIdOffer(Int64 idOffer, string offerConnectstr);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetAllCommentsByIdOffer_V2040 instead.")]
        List<LogEntryByOffer> GetAllCommentsByIdOffer(Int64 idOffer, string offerConnectstr);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSalesStatusByMonth(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetSalesStatusByMonthDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSalesStatusByMonth(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomersBySalesOwnerId(string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetSelectedUserContactsBySalesOwnerId(string idUser);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSelectedUsersTopOffers(byte idCurrency, Int32 offerLimit, Int64 accountingYear, string idUser, Company companyDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Customer> GetDashboardDetailsByCustomerCompanywise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetDashboardDetailsByCustomerCompanywiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetDashboard2Details_V2035 instead.")]
        IList<Customer> GetDashboardDetailsByCustomerCompanywise(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Customer> GetSelectedUsersDashboardDetailsByCustomerCompanywise(byte idCurrency, Int64 accountingYear, Int32 customerLimit, string idUser, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetSelectedUsersDashboardDetailsByCustomerCompanywiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetDashboard2Details_V2035 instead.")]
        IList<Customer> GetSelectedUsersDashboardDetailsByCustomerCompanywise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, string idUser, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer GetDashboardDetailsCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetOffersPipelineLogEntryWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSelectedOffersPipelineLogentryWise(byte idCurrency, string idUser, Int64 accountingYear, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer GetSelectedUsersDashboardDetailsCompanyWise(byte idCurrency, Int64 accountingYear, string idUser, Company companyDetails);

        [OperationContract(Name = "GetOfferQuantitySalesStatusByMonthCompanyWiseDatewise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int64 accountingYear, string idUser, Company companyDetails);

        [OperationContract(Name = "GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWiseDatewise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUsersSitesTarget(byte idCurrency, Int64 accountingFromYear, Int64 accountingToYear, string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<SalesStatusType> GetAllSalesStatusType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersTargetByCustomer(byte idCurrency, string idUser, Int64 accountingYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetTargetByCustomer(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ModelAppointment> GetOffersModelAppointment(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company compdetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOffersModelAppointmentDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<ModelAppointment> GetOffersModelAppointment(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetTargetForecast(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferLostReason> GetOfferLostReason();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetLostReasonsByOffer_V2040 instead.")]
        LostReasonsByOffer GetLostReasonsByOffer(Int64 idOffer, string offerConnectStr);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetCompanyGroup(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanyPlantByCustomerId(Int32 idCustomer, Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferOption> GetAllOfferOptions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetSalesOwnerBySiteId(Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues(byte key);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOfferStatus(Int64 idOffer, Int64 idStatus, Int32 idUser, DateTime statusDate, Int64 idSalesStatusType, bool isCodeUpdate, LostReasonsByOffer lostReasonsByOffer, string connectPlantconstr, Offer offer = null, Int32 idSite = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<GeosStatus> GetGeosOfferStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CarOEM> GetCarOEM();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosProject> GetProjectByCustomerId(Int64 idCustomer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetOfferContact_V2040 instead.")]
        List<OfferContact> GetOfferContact(Int64 idOffer, string offerPlantConnectStr);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountries();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use AddCompany_V2043 instead.")]
        Company AddCompany(Company company, Company emdepSite = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use UpdateCompany_V2043 instead.")]
        Company UpdateCompany(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Competitor> GetCompetitors();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use AddLogEntryByOffer_V2040 instead.")]
        bool AddLogEntryByOffer(LogEntryByOffer logEntryByOffer, string offerPlantConnectStr);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSalesTargetBySite(SalesTargetBySite salesTargetBySite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSitesTarget(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise(byte idCurrency, Int64 accountingYear, string idUser, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetSelectedUsersOfferLostReasonsDetailsCompanyDatewise")]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise(byte idCurrency, Int32 idActiveUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOfferLostReasonsDetailsCompanyWiseDatewise")]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<People> GetContactsOfSiteByOfferId(Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsSetPrimaryOfferContact(OfferContact offerContact);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCurrentPlantId(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetAllCompaniesDetails_V2040 instead.")]
        List<Company> GetAllCompaniesDetails(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetPeoples();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use IsExistCustomer_V2040 instead.")]
        bool IsExistCustomer(string customerName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Company> GetEmdepSitesCompanies();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCompanyPlantByCustomerId(Int32 idCustomer, string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetSelectedUserCompanyGroup(string idUser, bool isIncludeDefault = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Customer AddCustomer(Customer customer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetSiteContacts(Int64 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSalesTargetByPlantQuotas(Int64 accountingFromYear, Int64 accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantBusinessUnitSalesQuotaByUser(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePlantBusinessUnitSalesQuota(PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantBusinessUnitSalesQuotaBySelectedUsers(string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedUserWise(string assignedPlant, byte idCurrency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SalesUser GetTotalSalesQuotaBySelectedUserId(string userIds, byte idCurrency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CarProject AddCarProject(CarProject carProject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CarProject> GetCarProject(Int32 idCustomer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistCarProject(string Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetAllShipmentsByOfferId_V2040 instead.")]
        List<Shipment> GetAllShipmentsByOfferId(Company company, Int64 idOffer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetAllPackingBoxesByShipmentId_V2040 instead.")]
        List<PackingBox> GetAllPackingBoxesByShipmentId(Company company, Int64 idShipment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUserQuota> GetAllSalesUserDetails(byte idCurrency, Int32 accountingYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetWonValueByIdSaleUser(Company company, byte idCurrency, Int32 idUser, Int64 accountingYear);

        [OperationContract(Name = "GetWonValueByIdSaleUserDateWise")]
        [FaultContract(typeof(ServiceException))]
        double GetWonValueByIdSaleUser(Company company, byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CarProject> GetTopCarProjectOffers(byte idCurrency, string idUser, Int64 accountingYear, Int32 projectLimit, Int32 idUserPermission, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetTopCarProjectOffersDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<CarProject> GetTopCarProjectOffers(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 projectLimit, Int32 idUserPermission, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUser> GetAllSalesUserPeopleDetails(byte idCurrency, Int32 accountingYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddSaleUser(SalesUser salesUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUser(Int32 idUser, byte idCurrency, Int32 accountingYear);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYear(string userIds, byte idCurrency, Int32 accountingYear);

        [OperationContract(Name = "GetTotalSalesQuotaBySelectedUserIdAndYearDateWise")]
        [FaultContract(typeof(ServiceException))]
        SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYear(string userIds, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedPlantWiseAndYear(string assignedPlant, byte idCurrency, Int32 accountingYear, Int32 idCurrentUser = 0);

        [OperationContract(Name = "GetTotalPlantQuotaSelectedPlantWiseAndYearDateWise")]
        [FaultContract(typeof(ServiceException))]
        PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedPlantWiseAndYear(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePlantBusinessUnitSalesQuotaWithYear(PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetBusinessUnitStatusWise(byte idCurrency, Int32 idUser, Int64 accountingYear, Int32 idPermission, Company company);

        [OperationContract(Name = "GetBusinessUnitStatusWiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetBusinessUnitStatusWise(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idPermission, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetPlantQuotaAmountBUWise(string assignedPlant, byte idCurrency, Int64 accountingYear);

        [OperationContract(Name = "GetPlantQuotaAmountBUWiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetPlantQuotaAmountBUWise(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompanies(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmdepSite> GetAllEmdepSites(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SalesUser GetUserAllSalesQuota(Int32 idUser, byte idCurrency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddSaleUserQuota(List<SalesUserQuota> salesUserQuotas);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUserQuota> GetAllSalesUserDetailsPlantWise(byte idCurrency, Int32 accountingYear, string assignedPlants);

        [OperationContract(Name = "GetAllSalesUserDetailsPlantWiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<SalesUserQuota> GetAllSalesUserDetailsPlantWise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string assignedPlants);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYearPlantWise(string userIds, byte idCurrency, Int32 accountingYear, string assignedPlants);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSaleUser(SalesUser salesUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOrderForParticularColumn(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CarProject AddCarProjectWithCreatedBy(CarProject carProject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use AddOfferWithIdSourceOffer_V2033 instead.")]
        Offer AddOfferWithIdSourceOffer(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CustomerPurchaseOrder> GetOfferPurchaseOrders(Company company, byte idCurrency, Int64 idOffer, Int64 accountingYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUser> GetAllSalesUserPeopleDetailsWithPlantAndPermission(byte idCurrency, Int32 accountingYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        People GetPeopleDetailByIdPerson(Int32 idPerson);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Activity AddActivity(Activity activity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddActivityAttendees(Int64 idActivity, List<ActivityAttendees> activityAttendees);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddLogEntriesByActivity(Int64 idActivity, List<LogEntriesByActivity> logEntriesByActivity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActivity(Activity activity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteActivityAttendees(ActivityAttendees activityAttendees);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivities(Int32 idOwner);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByActivity> GetlogEntriesByActivity(Int64 idActivity, byte idLogEntryType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetActivityByIdActivity_V2035 instead.")]
        Activity GetActivityByIdActivity(Int64 idActivity);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCustomerCompanies();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCommentsByActivity(List<LogEntriesByActivity> logEntriesByActivity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllActivePeoples();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetAllCustomer();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdPermission(string idOwner, Int32 idPermission, string idPlant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Activity GetPlannedAndActualActivity(Int32 idOwner);

        [OperationContract(Name = "GetPlannedAndActualActivityDateWise")]
        [FaultContract(typeof(ServiceException))]
        Activity GetPlannedAndActualActivity(Int32 idOwner, DateTime accountingYearFrom, DateTime accountingYearTo);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<ActivityAttachment> UploadActivityAttachment(List<ActivityAttachment> activityAttachments);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActivityAttachment DownloadActivityAttachment(ActivityAttachment activityAttachment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserManagerDtl> GetSalesUserByPlant(string idPlants);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivityReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idbusinessUnit, string idSite, byte idCurrency, Company company);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsByLinkedItemAccount(string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllAttendesList(string idSite);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedSalesOwnerSites(string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomerPlant(string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCustomerPlant(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteActivity(Activity activity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteMultipleActivities(List<Activity> activities, LogEntriesByActivity deleteLogEntry);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomersByPlant(string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetSelectedUserContactsByPlant(string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetContactsByIdPermission_V2033 instead.")]
        List<People> GetContactsByIdPermission(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdSite(Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdContact(Int32 idContact);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tag> GetAllTags();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistTagName(string Name);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Tag AddTag(Tag tag);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Notification AddCommonNotification(Notification notification);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdOffer(Int64 idOffer, Int32 idEmdepSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer GetOfferByIdOfferAndEmdepSite(Int64 idOffer, string offerConnectStr);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        People GetContactsByIdPerson(Int32 idPerson);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetBusinessUnitsWithoutRestrictedBU(Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesForActivityReminder();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SendActivityReminderMail(ActivityMail activityMail);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActivityReminder(Activity activity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersEngineeringAnalysis(byte idCurrency, Int32 idCurrentUser, Int32 idZone, Int64 accountingYear, Company compdetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOffersEngineeringAnalysisDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersEngineeringAnalysis(byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Double GetOfferMaxValueById(Int16 idMaxValue);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetAllActivityReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idSite, string idContacts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetAllOfferReportDetails_V2035 instead.")]
        List<Offer> GetAllOfferReportDetails(string idSalesOwner, DateTime fromDate, DateTime toDate, string idbusinessUnit, string idSite, byte idCurrency, Company company, string idContacts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsPurchaseOrderDoneByIdOffer(Int64 idOffer, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DisableContact(People people);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DisableAccount(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSitesTargetByPlant(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, string idSite);


        [OperationContract(Name = "GetSitesTargetByPlantDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSitesTargetByPlant(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetTargetForecastByPlant(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingFromYear, Int64 accountingToYear, Int32 idUserPermission, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetTargetByCustomerByPlant(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetSharedActivitiesByIdPermission(Int32 idActiveUser, string idOwner, Int32 idPermission, string idPlant);

        [OperationContract(Name = "GetSharedActivitiesByIdPermissionDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetSharedActivitiesByIdPermission(Int32 idActiveUser, string idOwner, Int32 idPermission, string idPlant, DateTime accountingYearFrom, DateTime accountingYearTo);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOrderByIdOffer(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesLinkedToAccount(string idOwner, Int32 idPermission, string idPlant, Int32 idSite, Int64 idOffer, Int32 idEmdepSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAccountBySelectedPlant(Int32 idActiveUser, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetReportOffersPerSalesUser(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company company, string Interval);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesGoingToDueInInverval(string Interval);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUser> GetAllSalesUsersForReport();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersByIdSiteToLinkedWithActivities(byte idCurrency, Int32 idUser, Int64 accountingYear, Company company, Int32 idUserPermission, Int32 idSite);

        [OperationContract(Name = "GetOffersByIdSiteToLinkedWithActivitiesDateWise")]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersByIdSiteToLinkedWithActivities(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company company, Int32 idUserPermission, Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActivityTemplateTrigger> GetActivityTemplateTriggers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByContact> GetLogEntriesByContact(Int32 idContact, byte idLogEntryType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllSalesUserByIdSalesTeam(Int32 idSalesTeam);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Competitor> GetAllCompetitor();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllSalesUser();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        People GetIdSalesTeamByIdSalesUser(Int32 idSalesUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DailyCurrencyConversion> GetAllDailyCurrencyConversionsByDate(DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DailyCurrencyConversion AddCurrencyConversion(DailyCurrencyConversion dailyCurrencyConversion);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DailyCurrencyConversion UpdateCurrencyConversionDaily(DailyCurrencyConversion dailyCurrencyConversion);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCurrencyConversionListDaily(List<DailyCurrencyConversion> dailyCurrencyConversionList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateReadOnlyActivityLogs(Activity activity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUserManager(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetPlannedAppointmentAndAppointment(Int32 idOwner, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetPlannedAppiontmentByUserId(Int32 idActiveUser, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetSelectedOffersEngAnalysisDateWise_V2034 instead.")]
        List<Offer> GetSelectedOffersEngAnalysisDateWise(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetOffersEngineeringAnalysisByPermission_V2034 instead.")]
        List<Offer> GetOffersEngineeringAnalysisByPermission(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseCategory> GetWarehouseCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleBySupplier> GetArticlesBySupplier(string idCompanies = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetArticlesReportDetails_V2035 instead.")]
        List<Offer> GetArticlesReportDetails(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetOffersByIdCustomerToLinkedWithActivities_V2035 instead.")]
        List<Offer> GetOffersByIdCustomerToLinkedWithActivities(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCustomer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupvaluesWithoutRestrictedBU(Int32 idUser, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionwise(Int32 idUser, byte idCurrency, Int32 accountingYear, Int32 idUserPermission);

        [OperationContract(Name = "GetPlantSalesQuotaWithYearByIdUserPermissionwise1")]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionDatewise(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CpType> GetAllCpTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CpType> GetCpTypesByTemplate(byte idTemplate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Detection> GetDetectionByCpTypeAndTemplate(byte idTemplate, byte idCPType);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetModulesReportDetails(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivityPerformanceTest(Int32 idActiveUser, string idOwner, Int32 idPermission, string idPlant, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanyPlantByUserId(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCompanyPlantByIdUser(string idUser, bool isIncludeDefault = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SalesUser GetUserAllSalesQuotaByDate(Int32 idUser, byte idCurrency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUser> GetAllSalesUserPeopleDetailsWithPlantAndPermissionByDate(byte idCurrency, Int32 accountingYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DateTime> GetAllCurrencyConversionDates();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DailyCurrencyConversion GetCurrencyRateByDateAndId(DailyCurrencyConversion dailyCurrencyConversion);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddSaleUserQuotaWithDate(List<SalesUserQuota> salesUserQuotas);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePlantBusinessUnitSalesQuotaWithYearAndDate(PlantBusinessUnitSalesQuota plantBusinessUnitSalesQuota);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionwiseByDate(Int32 idUser, byte idCurrency, Int32 accountingYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSelectedUsersTargetForecastDate(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetTargetForecastByPlantDate(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetTargetForecastNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSalesTargetBySiteDate(SalesTargetBySite salesTargetBySite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DailyCurrencyConversion> GetLatestCurrencyConversion();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(string userIds, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSitesTargetNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSitesTargetByPlantNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUsersSitesTargetNewCurrencyConversion(byte idCurrency, Int64 accountingFromYear, Int64 accountingToYear, string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesTargetBySite> GetSalesTargetBySiteDetailByIdSite(Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUserQuota> GetAllSalesUserDetailsNewCurrencyConversion(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string assignedPlants);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetPlantQuotaAmountBUWiseNewCurrencyConversion(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CarProjectDetail> GetTopCarProjectOffersOptimization(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 projectLimit, Int32 idUserPermission, Company companydetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetDailyEvents_V2035 instead.")]
        List<TempAppointment> GetDailyEvents(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetSalesStatusWithTarget_V2035 instead.")]
        List<OfferDetail> GetSalesStatusWithTarget(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetSalesStatusByMonthAllPermission_V2035 instead.")]
        List<OfferDetail> GetSalesStatusByMonthAllPermission(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<ProductCategory> GetAllProductCategory();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DailyEventActivity> GetDailyEventActivities(string idOwner, Int32 idPermission, string idPlant);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<ProductCategory> GetAllProductCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductCategory> GetAllCategory();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductCategoryOfferOption> GetAllCategoryOfferOption();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64? GetIdProductCategoryByIdOffer(Int64 idOffer, string connectPlant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetAllSalesUserQuotaPeopleDetails_V2045 instead.")]
        List<SalesUser> GetAllSalesUserQuotaPeopleDetailsByDatewise(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetPlannedAppiontmentByUserIdIsInternal(Int32 idActiveUser, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActivitiesRecurrence> GetActivitiesRecurrence(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActivitiesRecurrence AddActivitiesRecurrence(ActivitiesRecurrence activitiesRecurrence);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActivitiesRecurrence UpdateActivitiesRecurrence(ActivitiesRecurrence activitiesRecurrence);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreateAutomaticPlannedActivity(DateTime startDate, DateTime endDate, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetTop5CustomersDashboardDetails_V2035 instead.")]
        CustomerTargetDetail GetTop5CustomersDashboardDetails(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUserQuota> GetAllSalesTeamUserDetail(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string assignedPlants);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetAllSalesTeamUserWonDetail_V2035 instead.")]
        List<SalesUserWon> GetAllSalesTeamUserWonDetail(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetSalesStatusByMonthWithTarget_V2035 instead.")]
        List<OfferMonthDetail> GetSalesStatusByMonthWithTarget(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetBusinessUnitStatusWithTarget_V2035 instead.")]
        BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BusinessUnitDetail> GetBusinessUnitsDetails(Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesStatusTypeDetail> GetSalesStatusTypeDetail();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantQuotaDetailsById(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePlantQuota(List<PlantBusinessUnitSalesQuota> plantBusinessUnitSalesQuotas);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PlannedVisitDetail GetPlannedVisitDetail(DateTime accountingYearFrom, DateTime accountingYearTo, string idOwner = null, Int32 idPermission = 0, string idPlant = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreateAutomaticPlannedActivitySprint60(DateTime startDate, DateTime endDate, Int32 idUser, List<ActivitiesRecurrence> LstActivitiesRecurrence);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetOfferAmountByCurrencyConversion(Int64 idOffer, double offerAmount, byte formIdCurrency, byte toIdCurrency, DateTime? deliveryDate, DateTime? sendIn, DateTime? RFQReception, DateTime? CreatedIn, DateTime? ReceivedIn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllAttendesList_V2030(string idSite);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsByLinkedItemAccount_V2030(string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Activity AddActivity_V2031(Activity activity);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActivity_V2031(Activity activity);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActivitiesRecurrence> GetActivitiesRecurrence_V2031(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActivitiesRecurrence UpdateActivitiesRecurrence_V2031(ActivitiesRecurrence activitiesRecurrence);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use CreateAutomaticPlannedActivity_V2035 instead.")]
        bool CreateAutomaticPlannedActivity_V2031(DateTime startDate, DateTime endDate, Int32 idUser, List<ActivitiesRecurrence> LstActivitiesRecurrence);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdOffer_V2031(Int64 idOffer, Int32 idEmdepSite);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use UpdateOffer_V2033 instead.")]
        bool UpdateOffer_V2031(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use UpdateOrderByIdOffer_V2037 instead.")]
        bool UpdateOrderByIdOffer_V2031(Offer offer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        People AddContact_V2033(People people);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsByIdPermission_V2033(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use AddOffer_V2034 instead.")]
        Offer AddOffer_V2033(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use AddOfferWithIdSourceOffer_V2040 instead.")]
        Offer AddOfferWithIdSourceOffer_V2033(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use UpdateOffer_V2034 instead.")]
        bool UpdateOffer_V2033(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use UpdateOfferForParticularColumn_V2040 instead.")]
        bool UpdateOfferForParticularColumn_V2033(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetOfferDetailsById_V2035 instead.")]
        Offer GetOfferDetailsById_V2033(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use AddOffer_V2037 instead.")]
        Offer AddOffer_V2034(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use UpdateOffer_V2037 instead.")]
        Offer UpdateOffer_V2034(Offer offer, Int32 idSite, Int32 idUser);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedOffersEngAnalysisDateWise_V2034(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersEngineeringAnalysisByPermission_V2034(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreateAutomaticPlannedActivity_V2035(DateTime startDate, DateTime endDate, Int32 idUser, List<ActivitiesRecurrence> LstActivitiesRecurrence);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Activity GetActivityByIdActivity_V2035(Int64 idActivity);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersByIdCustomerToLinkedWithActivities_V2035(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCustomer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetOffersPipeline_V2035(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferDetail> GetSalesStatusWithTarget_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TempAppointment> GetDailyEvents_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferDetail> GetSalesStatusByMonthAllPermission_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Customer> GetDashboard2Details_V2035(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSalesStatus_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerTargetDetail GetTop5CustomersDashboardDetails_V2035(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2035(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_V2035(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetArticlesReportDetails_V2037 instead.")]
        List<Offer> GetArticlesReportDetails_V2035(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetAllOfferReportDetails_V2035(string idSalesOwner, DateTime fromDate, DateTime toDate, string idbusinessUnit, string idSite, byte idCurrency, Company company, string idContacts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetOfferDetailsById_V2037 instead.")]
        Offer GetOfferDetailsById_V2035(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEngAnalysisArticles_V2041 instead.")]
        List<EngineeringAnalysisType> GetEngAnalysisArticles();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use AddOffer_V2040 instead.")]
        Offer AddOffer_V2037(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use UpdateOffer_V2040 instead.")]
        Offer UpdateOffer_V2037(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetOfferDetailsById_V2040 instead.")]
        Offer GetOfferDetailsById_V2037(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Company company);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<User> GetSalesAndCommericalUsers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferContact> GetContactsOfCustomerGroupByOfferId(Int32 idCustomer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetArticlesReportDetails_V2037(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOrderByIdOffer_V2037(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistCustomer_V2040(string customerName, byte idCustomerType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserSiteGeosServiceProvider> GetAllCompaniesWithServiceProvider(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetNextNumberOfOfferFromCounters_V2040(byte? idOfferType, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string MakeOfferCode_V2040(byte? idOfferType, Int32 idCustomer, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetails_V2040(Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2042. Use GetOfferDetailsById_V2042 instead.")]
        Offer GetOfferDetailsById_V2040(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        LostReasonsByOffer GetLostReasonsByOffer_V2040(Int64 idOffer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use AddOffer_V2041 instead.")]
        Offer AddOffer_V2040(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use UpdateOffer_V2041 instead.")]
        Offer UpdateOffer_V2040(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer AddOfferWithIdSourceOffer_V2040(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddLogEntryByOffer_V2040(LogEntryByOffer logEntryByOffer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntryByOffer> GetAllCommentsByIdOffer_V2040(Int64 idOffer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntryByOffer> GetAllLogEntriesByIdOffer_V2040(Int64 idOffer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferContact> GetOfferContact_V2040(Int64 idOffer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOfferForParticularColumn_V2040(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Shipment> GetAllShipmentsByOfferId_V2040(Int64 idOffer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PackingBox> GetAllPackingBoxesByShipmentId_V2040(Int64 idShipment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActionPlan GetActionPlan(Int64 IdActionPlan);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use GetSalesActivityRegister_V2043 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use GetActionPlanItem_V2043 instead.")]
        ActionPlanItem GetActionPlanItem(Int64 IdActionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use AddActionPlanItem_V2043 instead.")]
        ActionPlanItem AddActionPlanItem(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use UpdateActionPlanItem_V2043 instead.")]
        bool UpdateActionPlanItem(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteActionPlanItem(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByActionItem> AddUpdateDeleteLogEntriesByActionItem(Int64 idActionPlanItem, List<LogEntriesByActionItem> logEntriesByActionItems);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetActionPlanItemResponsible(Int32 idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateParticluarColumnActionPlanItem(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetActionsCountOfStatus(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime accountingYearFrom, DateTime accountingYearTo);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimelineGrid> GetOfferAndOrderDetailsByOfferCode(string offerCode, TimelineParams timelineParams);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use AddOffer_V2045 instead.")]
        Offer AddOffer_V2041(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer AddOffer_V2045(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2042. Use UpdateOffer_V2042 instead.")]
        Offer UpdateOffer_V2041(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EngineeringAnalysisType> GetEngAnalysisArticles_V2041();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SaveCopyOfEMail(Offer offer, FileDetail emailFileDetail);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActivityGrid> GetActivitiesDetail(ActivityParams objActivityParams);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TimelineGrid> GetOpportunities(TimelineParams timelineParams);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PeopleDetails GetGroupPlantByMailId(string mailId);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer GetOfferDetailsById_V2042(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer UpdateOffer_V2042(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateAttachActivity(Activity activity);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EngineeringAnalysisArticleDetail> GetEngineeringAnalysisArticleDetail(string offerCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateAttachOffer(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EngineeringAnalysis GetEngAnalysisByOfferCode(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2043. Use GetSalesActivityRegister_V2051 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister_V2043(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActionPlanItem GetActionPlanItem_V2043(Int64 IdActionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActionPlanItem AddActionPlanItem_V2043(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlanItem_V2043(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company AddCompany_V2043(Company company, Company emdepSite = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActionPlanItem> GetAppointmentActivities(ActivityParams objActivityParams);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
       Company UpdateCompany_V2043(Company company);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUser> GetAllSalesUserQuotaPeopleDetails_V2045(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlanItemForGrid(ActionPlanItem actionPlanItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActionPlanItem> GetSalesActivityRegister_V2051(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);

    }
}
