using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSelectedUsersOfferLeadSourceCompanyWise_V2110 instead.")]
        List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferLeadSourceCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOfferLeadSourceCompanyWiseDateWise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetOfferLeadSourceCompanyWise_V2110 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetCurrencies_V2490 instead.")]
        IList<Currency> GetCurrencies();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetCurrencyByExchangeRate_V2600 instead.")]
        IList<Currency> GetCurrencyByExchangeRate();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetOfferMaxValue_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2680. Use UpdateContact_V2680 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetOfferQuantitySalesStatusByMonthCompanyWise_V2110 instead.")]
        List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise(byte idCurrency, Int64 accountingYear, string idUser, Company companyDetails);

        [OperationContract(Name = "GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWiseDatewise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise_V2110 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetCompanyGroup_V2420 instead.")]
        List<Customer> GetCompanyGroup(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanyPlantByCustomerId(Int32 idCustomer, Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferOption> GetAllOfferOptions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetSalesOwnerBySiteId_V2390 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2550. Use GetCountries_V2550 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSelectedUsersOfferLostReasonsDetailsCompanyWise_V2110 instead.")]
        List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise(byte idCurrency, Int32 idActiveUser, Int32 idZone, Int64 accountingYear, Company companyDetails, Int32 idUserPermission);

        [OperationContract(Name = "GetOfferLostReasonsDetailsCompanyWiseDatewise")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetOfferLostReasonsDetailsCompanyWise_V2110 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2350. Use IsExistCarProject_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2110. Use AddSaleUser_V2110 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2110. Use UpdateSaleUser_V2110 instead.")]
        bool UpdateSaleUser(SalesUser salesUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use UpdateOrderForParticularColumn_V2090 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetAllCustomer_V2630 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetSalesUserByPlant_V2440 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2350. Use IsExistTagName_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetActivitiesForActivityReminder_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetOffersEngineeringAnalysis_V2110 instead.")]
        List<Offer> GetOffersEngineeringAnalysis(byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetOfferMaxValueById_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetActivitiesLinkedToAccount_V2500 instead.")]
        List<Activity> GetActivitiesLinkedToAccount(string idOwner, Int32 idPermission, string idPlant, Int32 idSite, Int64 idOffer, Int32 idEmdepSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAccountBySelectedPlant(Int32 idActiveUser, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetReportOffersPerSalesUser(byte idCurrency, Int32 idUser, Int32 idZone, Int64 accountingYear, Company company, string Interval);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetActivitiesGoingToDueInInverval_V2350 instead.")]
        List<Activity> GetActivitiesGoingToDueInInverval(string Interval);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetAllSalesUsersForReport_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2140. Use UpdateCurrencyConversionListDaily_V2140 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSelectedOffersEngAnalysisDateWise_V2110 instead.")]
        List<Offer> GetSelectedOffersEngAnalysisDateWise(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetOffersEngineeringAnalysisByPermission_V2110 instead.")]
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


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetAllCpTypes_V2530 instead.")]

        List<CpType> GetAllCpTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetCpTypesByTemplate_V2530 instead.")]

        List<CpType> GetCpTypesByTemplate(byte idTemplate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetDetectionByCpTypeAndTemplate_V2530 instead.")]

        List<Detection> GetDetectionByCpTypeAndTemplate(byte idTemplate, byte idCPType);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivityPerformanceTest(Int32 idActiveUser, string idOwner, Int32 idPermission, string idPlant, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetCompanyPlantByUserId_V2420 instead.")]
        List<Company> GetCompanyPlantByUserId(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSelectedUserCompanyPlantByIdUser_V2420 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetSelectedUsersTargetForecastDate_V2301 instead.")]
        IList<Offer> GetSelectedUsersTargetForecastDate(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetTargetForecastByPlantDate_V2301 instead.")]
        IList<Offer> GetTargetForecastByPlantDate(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetTargetForecastNewCurrencyConversion_V2301 instead.")]
        IList<Offer> GetTargetForecastNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSalesTargetBySiteDate(SalesTargetBySite salesTargetBySite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetLatestCurrencyConversion_V2180 instead.")]
        List<DailyCurrencyConversion> GetLatestCurrencyConversion();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PlantBusinessUnitSalesQuota GetTotalPlantQuotaSelectedPlantWiseAndYearWithExchangeDate(string assignedPlant, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SalesUserQuota GetTotalSalesQuotaBySelectedUserIdAndYearWithExchangeRate(string userIds, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSitesTargetNewCurrencyConversion_V2420 instead.")]
        List<Company> GetSitesTargetNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSitesTargetByPlantNewCurrencyConversion(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSelectedUsersSitesTargetNewCurrencyConversion_V2420 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetActivitiesRecurrence_V2380 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetActivitiesByIdOffer_V2500 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetContactsByIdPermission_V2300 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetOffersByIdCustomerToLinkedWithActivities_V2150 instead.")]
        List<Offer> GetOffersByIdCustomerToLinkedWithActivities_V2035(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCustomer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetOffersPipeline_V2110 instead.")]
        IList<Offer> GetOffersPipeline_V2035(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSalesStatusWithTarget_V2110 instead.")]
        List<OfferDetail> GetSalesStatusWithTarget_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetDailyEvents_V2110 instead.")]
        List<TempAppointment> GetDailyEvents_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSalesStatusByMonthAllPermission_V2110 instead.")]
        List<OfferDetail> GetSalesStatusByMonthAllPermission_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetDashboard2Details_V2110 instead.")]
        IList<Customer> GetDashboard2Details_V2035(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSalesStatus_V2110 instead.")]
        IList<Offer> GetSalesStatus_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSalesStatusByMonthWithTarget_V2110 instead.")]
        List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_V2035(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetTop5CustomersDashboardDetails_V2110 instead.")]
        CustomerTargetDetail GetTop5CustomersDashboardDetails_V2035(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetAllSalesTeamUserWonDetail_V2110 instead.")]
        List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2035(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetBusinessUnitStatusWithTarget_V2110 instead.")]
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
        [ObsoleteAttribute("This method will be removed after this version V2560. Use GetSalesAndCommericalUsers_V2560 instead.")]
        List<User> GetSalesAndCommericalUsers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferContact> GetContactsOfCustomerGroupByOfferId(Int32 idCustomer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use UpdateOrderByIdOffer_V2090 instead.")]
        bool UpdateOrderByIdOffer_V2037(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use IsExistCustomer_V2350 instead.")]
        bool IsExistCustomer_V2040(string customerName, byte idCustomerType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserSiteGeosServiceProvider> GetAllCompaniesWithServiceProvider(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetNextNumberOfOfferFromCounters_V2350 instead.")]
        Int64 GetNextNumberOfOfferFromCounters_V2040(byte? idOfferType, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use MakeOfferCode_V2350 instead.")]
        string MakeOfferCode_V2040(byte? idOfferType, Int32 idCustomer, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetAllCompaniesDetails_V2490 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2090. Use AddOfferWithIdSourceOffer_V2090 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2090. Use UpdateOfferForParticularColumn_V2090 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2090. Use AddOffer_V2090 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetOfferDetailsById_V2090 instead.")]
        Offer GetOfferDetailsById_V2042(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use UpdateOffer_V2090 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2043. Use GetSalesActivityRegister_V2051 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister_V2043(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetActionPlanItem_V2080 instead.")]
        ActionPlanItem GetActionPlanItem_V2043(Int64 IdActionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use AddActionPlanItem_V2080 instead.")]
        ActionPlanItem AddActionPlanItem_V2043(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use UpdateActionPlanItem_V2080 instead.")]
        bool UpdateActionPlanItem_V2043(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use AddCompany_V2340 instead.")]
        Company AddCompany_V2043(Company company, Company emdepSite = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActionPlanItem> GetAppointmentActivities(ActivityParams objActivityParams);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use UpdateCompany_V2340 instead.")]
        Company UpdateCompany_V2043(Company company);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetAllSalesUserQuotaPeopleDetails_V2110 instead.")]
        List<SalesUser> GetAllSalesUserQuotaPeopleDetails_V2045(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use UpdateActionPlanItemForGrid_V2380 instead.")]
        bool UpdateActionPlanItemForGrid(ActionPlanItem actionPlanItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetSalesActivityRegister_V2080 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister_V2051(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use AddActionPlanItem_V2120 instead.")]
        ActionPlanItem AddActionPlanItem_V2080(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use UpdateActionPlanItem_V2120 instead.")]
        bool UpdateActionPlanItem_V2080(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetSalesActivityRegister_V2120 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister_V2080(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetActionPlanItem_V2120 instead.")]
        ActionPlanItem GetActionPlanItem_V2080(Int64 IdActionPlanItem);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetActionPlanItem_V2170 instead.")]
        Offer AddOfferWithIdSourceOffer_V2090(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetAllSalesUserQuotaPeopleDetails_V2301 instead.")]
        List<SalesUser> GetAllSalesUserQuotaPeopleDetails_V2110(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddSaleUser_V2110(SalesUser salesUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSaleUser_V2110(SalesUser salesUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSalesStatus_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferLeadSourceCompanyWise_V2110(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_V2110(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerTargetDetail GetTop5CustomersDashboardDetails_V2110(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetAllSalesTeamUserWonDetail_V2360 instead.")]
        List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2110(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_V2110(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedOffersEngAnalysisDateWise_V2110(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersEngineeringAnalysis_V2110(byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetOffersPipeline_V2110(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise_V2110(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersEngineeringAnalysisByPermission_V2110(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferDetail> GetSalesStatusWithTarget_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferDetail> GetSalesStatusByMonthAllPermission_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TempAppointment> GetDailyEvents_V2110(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Customer> GetDashboard2Details_V2110(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferQuantitySalesStatusByMonthCompanyWise_V2110(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferQuantitySalesStatusByMonthCompanyWise_V2110(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use AddActionPlanItem_V2380 instead.")]
        ActionPlanItem AddActionPlanItem_V2120(ActionPlanItem actionPlanItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetSalesActivityRegister_V2480 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister_V2120(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetActionPlanItem_V2480 instead.")]
        ActionPlanItem GetActionPlanItem_V2120(Int64 IdActionPlanItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use UpdateActionPlanItem_V2380 instead.")]
        bool UpdateActionPlanItem_V2120(ActionPlanItem actionPlanItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use UpdateOfferForParticularColumn_V2120 instead.")]
        bool UpdateOfferForParticularColumn_V2090(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use UpdateOrderForParticularColumn_V2120 instead.")]
        bool UpdateOrderForParticularColumn_V2090(Offer offer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetOfferDetailsById_V2120 instead.")]
        Offer GetOfferDetailsById_V2090(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use UpdateOrderForParticularColumn_V2200 instead.")]
        bool UpdateOrderForParticularColumn_V2120(Offer offer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use UpdateOfferForParticularColumn_V2130 instead.")]
        bool UpdateOfferForParticularColumn_V2120(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Detection> GetAllDetectionForCreateColumn();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetailsWithServiceProvider();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedOfferDetails(UInt64 idOffer);

        [OperationContract(Name = "GetPlantSalesQuotaWithYearByIdUserPermissionwise1")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130 instead.")]
        List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionDatewise(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130 instead.")]
        List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2130(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetAllCompaniesDetails_V2490 instead.")]
        List<Company> GetAllCompaniesDetails_V2130(Int32 idUser);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetModulesReportDetails_V2130 instead.")]
        List<Offer> GetModulesReportDetails(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2220. Use GetModulesReportDetails_V2220 instead.")]
        List<Offer> GetModulesReportDetails_V2130(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetArticlesReportDetails_V2130 instead.")]
        List<Offer> GetArticlesReportDetails_V2037(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2220. Use GetArticlesReportDetails_V2220 instead.")]
        List<Offer> GetArticlesReportDetails_V2130(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use UpdateOfferForParticularColumn_V2200 instead.")]
        bool UpdateOfferForParticularColumn_V2130(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use UpdateCurrencyConversionListDaily_V2190 instead.")]
        bool UpdateCurrencyConversionListDaily_V2140(List<DailyCurrencyConversion> dailyCurrencyConversionList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use AddOffer_V2170 instead.")]
        Offer AddOffer_V2140(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2270. Use UpdateOffer_V2270 instead.")]
        Offer UpdateOffer_V2140(Offer offer, Int32 idSite, Int32 idUser);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetEngAnalysisByOfferCode_V2140 instead.")]
        EngineeringAnalysis GetEngAnalysisByOfferCode(Offer offer);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetOfferDetailsById_V2200 instead.")]
        Offer GetOfferDetailsById_V2120(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use CreateFolderOffer_V2140 instead.")]
        bool CreateFolderOffer(Offer offer, bool? isNewFolderForOffer = null);





        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use CreateFolderOffer_V2360 instead.")]
        bool CreateFolderOffer_V2140(Offer offer, bool? isNewFolderForOffer = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version. Use GetEngAnalysisAllRevisionsByOfferCode_V2680 instead.")]
        List<EngineeringAnalysis> GetEngAnalysisAllRevisionsByOfferCode(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetOffersByIdCustomerToLinkedWithActivities_V2490 instead.")]
        List<Offer> GetOffersByIdCustomerToLinkedWithActivities_V2150(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCustomer);



        //GEOS2-3144 IESD-24539 CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<QuotationMatrixTemplate> GetAllQuotationMatrixTemplates_V2160();


        //GEOS2-3145 IESD-24539 CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        QuotationMatrixTemplate AddQuotationMatrix_V2160(QuotationMatrixTemplate quotationMatrixTemplate);

        //GEOS2-3145 IESD-24539 CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateQuotationMatrix_V2160(QuotationMatrixTemplate quotationMatrixTemplate);

        //[GEOS2-3144] [GEOS2-3145] [GEOS2-3185] [YJoshi]
        //[CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM - Add MartixList And load the grid details in Configuration section]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetAllCustomersDetails_V2160();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer AddOffer_V2170(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use AddOfferWithIdSourceOffer_V2490 instead.")]
        Offer AddOfferWithIdSourceOffer_V2170(Offer offer, Int32 idSite, Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<DailyCurrencyConversion> GetLatestCurrencyConversion_V2190();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCurrencyConversionListDaily_V2190(List<DailyCurrencyConversion> dailyCurrencyConversionList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOrderForParticularColumn_V2200(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use UpdateOfferForParticularColumn_V2490 instead.")]
        bool UpdateOfferForParticularColumn_V2200(Offer offer, Int32 idSite, Int32 idUser);

        //GEOS2-1900
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetArticlesReportDetails_V2340 instead.")]
        List<Offer> GetArticlesReportDetails_V2220(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);


        //GEOS2-1900
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetModulesReportDetails_V2350 instead.")]
        List<Offer> GetModulesReportDetails_V2220(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null);

        //GEOS2-3609
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer UpdateOffer_V2270(Offer offer, Int32 idSite, Int32 idUser);

        //GEOS2-243
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CarOEM> GetAllCarOEM();


        //GEOS2-244
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertCarOEM(byte[] ImageBytes, string Name);

        //GEOS2-245
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCarOEM(byte[] CarByteImage, int CarId, string PrevCarOEMName, string CarOEMName);

        //[rdixit][29.06.2022][GEOS2-3514]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2360 instead.")]
        IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);


        //[rdixit][29.06.2022][GEOS2-3515]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2360 instead.")]
        List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);

        //[rdixit][29.06.2022][GEOS2-3515]	
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2360 instead.")]
        List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        //[rdixit][29.06.2022][GEOS2-3515]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetDailyEvents_IgnoreOfferCloseDate_V2420 instead.")]
        List<TempAppointment> GetDailyEvents_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser);

        //[rdixit][30.06.2022][GEOS2-3517]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetDashboard2Details_IgnoreOfferCloseDate_V2320 instead.")]
        IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);


        //[rdixit][30.06.2022][GEOS2-3518]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate_V2360 instead.")]
        List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false);

        //[rdixit][30.06.2022][GEOS2-3518]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate_V2360 instead.")]
        CustomerTargetDetail GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false);

        //[rdixit][30.06.2022][GEOS2-3518]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate_V2360 instead.")]
        BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false);


        //[rdixit][GEOS2-3519][01.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2420 instead.")]
        List<Offer> GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails);

        //[rdixit][GEOS2-3519][01.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2420 instead.")]
        List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        //[rdixit][Ignore the “To” date in the “DASHBOARD-> DASHBOARD3” section for Opportunities info][01.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSalesStatus_IgnoreOfferCloseDate_V2420 instead.")]
        IList<Offer> GetSalesStatus_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission);

        //[rdixit][Ignore the “To” date in the “DASHBOARD-> DASHBOARD3” section for Opportunities info][01.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420 instead.")]
        List<Offer> GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        //[rdixit][Ignore the “To” date in the “DASHBOARD-> DASHBOARD3” section for Opportunities info][01.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSelectedUsersOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420 instead.")]
        List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise_IgnoreOfferCloseDate(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0);

        //[rdixit][Ignore the “To” date in the “DASHBOARD-> DASHBOARD3” section for Opportunities info][01.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSelectedUsersOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420 instead.")]
        List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0);

        //[rdixit][Ignore the “To” date in the “DASHBOARD-> DASHBOARD3” section for Opportunities info][01.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420 instead.")]
        List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use UpdateOrderByIdOffer_V2290 instead.")]
        bool UpdateOrderByIdOffer_V2090(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use AddOffer_V2290 instead.")]
        Offer AddOffer_V2090(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use UpdateOffer_V2290 instead.")]
        Offer UpdateOffer_V2090(Offer offer, Int32 idSite, Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use GetOfferDetailsById_V2290 instead.")]
        Offer GetOfferDetailsById_V2200(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite);

        //[rdixit][GEOS2-2911][04.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use UpdateOrderByIdOffer_V2520 instead.")]
        bool UpdateOrderByIdOffer_V2290(Offer offer);

        //[rdixit][GEOS2-2909][05.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use AddOffer_V2490 instead.")]
        Offer AddOffer_V2290(Offer offer, Int32 idSite, Int32 idUser);


        //[001][kshinde][GEOS2-2910][05.07.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use UpdateOffer_V2460 instead.")]
        Offer UpdateOffer_V2290(Offer offer, Int32 idSite, Int32 idUser);

        //[001][kshinde][GEOS2-2910][05.07.2022]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetOfferDetailsById_V2200 instead.")]
        Offer GetOfferDetailsById_V2290(Int64 idOffer, Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, ActiveSite activeSite);

        //[GEOS2-2312][12.08.2022][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetContactsByIdPermission_V2380 instead.")]
        List<People> GetContactsByIdPermission_V2300(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetSelectedUsersTargetForecastDate_V2380 instead.")]
        IList<Offer> GetSelectedUsersTargetForecastDate_V2301(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetTargetForecastByPlantDate_V2380 instead.")]
        IList<Offer> GetTargetForecastByPlantDate_V2301(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission, string idSite);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use GetTargetForecastNewCurrencyConversion_V238 instead.")]
        IList<Offer> GetTargetForecastNewCurrencyConversion_V2301(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUser> GetAllSalesUserQuotaPeopleDetails_V2301(byte idCurrency, Int32 accountingFromYear, Int32 accountingToYear);

        //shubham[skadam] GEOS2-3359 top customer showing wrong in CRM dashboard report 10 OCT 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetDashboard2Details_IgnoreOfferCloseDate_V2360 instead.")]
        IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2320(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);


        //[GEOS2-3994][rdixit][17.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use UpdateCompany_V2390 instead.")]
        Company UpdateCompany_V2340(Company company);

        //[GEOS2-3994][rdixit][17.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use AddCompany_V2390 instead.")]
        Company AddCompany_V2340(Company company, Company emdepSite = null);

        // shubham[skadam]GEOS2-4052 Add a new column “PO Date” in Articles Report 30 11 2022
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetArticlesReportDetails_V2620 instead.")]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetArticlesReportDetails_V2340(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);

        //[rdixit][GEOS2-3871][09.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetCurrentSiteTimeZone(string SiteName);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistCarProject_V2350(string Name);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Double GetOfferMaxValue_V2350();

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistTagName_V2350(string Name);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistCustomer_V2350(string customerName, byte idCustomerType);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetNextNumberOfOfferFromCounters_V2350(byte? idOfferType, Int32 idUser);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Double GetOfferMaxValueById_V2350(Int16 idMaxValue);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use MakeOfferCode_V2620 instead.")]
        string MakeOfferCode_V2350(byte? idOfferType, Int32 idCustomer, Int32 idUser);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesForActivityReminder_V2350();

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesGoingToDueInInverval_V2350(string Interval);

        //[GEOS2-4120][rdixit][10.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUser> GetAllSalesUsersForReport_V2350();

        //[GEOS2-4128][rdixit][11.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetModulesReportDetails_V2360 instead.")]
        List<Offer> GetModulesReportDetails_V2350(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany, byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, Int32? idCustomer = null);

        //[plahange][7/2/2023] GEOS2-4146
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantBusinessUnitSalesQuota> GetPlantSalesQuotaWithYearByIdUserPermissionDatewise_V2360(Int32 idUser, byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission);

        //[rdixit][GEOS2-2358][09.02.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreateFolderOffer_V2360(Offer offer, bool? isNewFolderForOffer = null);

        //[rdixit][GEOS2-4127][10.02.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ModuleFamily> GetModuleFamilies();

        //[rdixit][GEOS2-4127][10.02.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetModulesReportDetails_V2530 instead.")]
        List<Offer> GetModulesReportDetails_V2360(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany,
            byte? idTemplate, byte? idCPType, string idDetections, byte idCurrency, byte? idfamily, byte? idsubFamily, Int32? idCustomer = null);

        //[rdixit][01.03.2023][GEOS2-4219]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2420 instead.")]
        List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2360(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);

        //[rdixit][01.03.2023][GEOS2-4219]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2420 instead.")]
        List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2360(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        //[rdixit][GEOS2-4222][01.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2420 instead.")]
        IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2360(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        //[rdixit][GEOS2-4224][01.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CustomerTargetDetail GetTop5CustomersDashboardDetails_IgnoreOfferCloseDate_V2360(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, string assignedPlant, bool isTarget = false);

        //[rdixit][GEOS2-4224][01.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        BusinessUnitTargetDetail GetBusinessUnitStatusWithTarget_IgnoreOfferCloseDate_V2360(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser, Company company, bool isTarget = false);

        //[rdixit][GEOS2-4224][01.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferMonthDetail> GetSalesStatusByMonthWithTarget_IgnoreOfferCloseDate_V2360(byte idCurrency, string assignedPlant, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser, bool isSiteTarget = false);

        //[rdixit][GEOS2-4224][01.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetAllSalesTeamUserWonDetail_V2460 instead.")]
        List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2360(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser);

        //[rdixit][GEOS2-4223][03.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetDashboard2Details_IgnoreOfferCloseDate_V2420 instead.")]
        IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2360(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);

        //[rdixit][GEOS2-4324][11.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetSelectedUserCustomersBySalesOwnerId_V2390 instead.")]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2380(string idUser);

        //[pmisal][GEOS2-4323][10.04.2023]   
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetSelectedUserCustomersByPlant_V2390 instead.")]
        List<Company> GetSelectedUserCustomersByPlant_V2380(string idSite);

        //[rdixit][GEOS2-4324][11.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetCustomersBySalesOwnerId_V2390 instead.")]
        List<Company> GetCustomersBySalesOwnerId_V2380(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        //[rdixit][GEOS2-4324][17.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetAllCountriesDetails();

        //[GEOS2-4327][rdixit][18.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetActivitiesRecurrence_V2390 instead.")]
        List<ActivitiesRecurrence> GetActivitiesRecurrence_V2380(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission);

        //[GEOS2-4327][rdixit][26.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetContactsByIdPermission_V2390 instead.")]
        List<People> GetContactsByIdPermission_V2380(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);
        //[GEOS2-4327][rdixit][26.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSelectedUsersTargetForecastDate_V2380(byte idCurrency, string idUser, Int64 accountingFromYear, Int64 accountingToYear);
        //[GEOS2-4327][rdixit][26.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetTargetForecastByPlantDate_V2380(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idUserPermission, string idSite);
        //[GEOS2-4327][rdixit][26.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetTargetForecastNewCurrencyConversion_V2380(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission);
        //[rdixit][GEOS2-4372][27.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlanItemForGrid_V2380(ActionPlanItem actionPlanItem);
        //[rdixit][GEOS2-4372][27.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use UpdateActionPlanItem_V2480 instead.")]
        bool UpdateActionPlanItem_V2380(ActionPlanItem actionPlanItem);
        //[rdixit][GEOS2-4372][27.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use AddActionPlanItem_V2480 instead.")]
        ActionPlanItem AddActionPlanItem_V2380(ActionPlanItem actionPlanItem);

        #region V2390
        //[GEOS2-4278][rdixit][04.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use GetContactsByIdPermission_V2680 instead.")]
        List<People> GetContactsByIdPermission_V2390(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);
        //[GEOS2-4278][rdixit][04.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetSalesOwnerBySiteId_V2450 instead.")]
        List<People> GetSalesOwnerBySiteId_V2390(Int32 idSite);
        //[GEOS2-4278][rdixit][04.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany_V2390(Company company);

        //[GEOS2-4279][rdixit][05.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSelectedUserCustomersBySalesOwnerId_V2420 instead.")]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2390(string idUser);

        //[GEOS2-4279][rdixit][05.05.2023]  
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomersByPlant_V2390(string idSite);

        //[GEOS2-4279][rdixit][05.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetCustomersBySalesOwnerId_V2420 instead.")]
        List<Company> GetCustomersBySalesOwnerId_V2390(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        //[GEOS2-4279][rdixit][05.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company AddCompany_V2390(Company company, Company emdepSite = null);

        //[GEOS2-4283][rdixit][10.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetActivitiesRecurrence_V2420 instead.")]
        List<ActivitiesRecurrence> GetActivitiesRecurrence_V2390(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission);

        //[rdixit][GEOS2-4274][22.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Regions> GetRegionsByGroupAndCountryAndSites(int IdGroup, string CountryNames, string SiteNames);

        //[rdixit][GEOS2-4274][22.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountriesByGroupAndRegionAndSites(int IdGroup, string RegionNames, string SiteNames);

        //[rdixit][GEOS2-4274][22.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use GetCustomerWithSites_V2410 instead.")]
        List<SitesWithCustomer> GetCustomerWithSites();

        //[rdixit][GEOS2-4274][22.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use ActionLauncherAdd_V2500 instead.")]
        ActionPlanItem ActionLauncherAdd(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList);

        //[rdixit][GEOS2-4274][22.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SitesWithCustomer> GetSitesBySalesOwner(int idSalesOwner);

        //[rdixit][GEOS2-4274][22.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetActivePeoplesBySiteList(string idSiteList);

        //TO get only IsStillActive and IsEnabled Users by rdixit 31.05.2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllActivePeoples_V2390();
        #endregion

        //[rdixit][GEOS2-4652][11.07.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SitesWithCustomer> GetCustomerWithSites_V2410();

        //[rdixit][GEOS2-4655][18.07.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllActivePeoplesWithSites();

        //[rdixit][GEOS2-4655][18.07.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SitesWithCustomer> GetWithSiteswithSalesUsers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetActivePeoplesBySiteList_V2410(string idSiteList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCustomersBySalesOwnerId_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2420(string idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2490 instead.")]
        IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2420(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActivitiesRecurrence> GetActivitiesRecurrence_V2420(string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, string idSite, Int32 idPermission);

        //[pramod.misal][GEOS2-4682][07-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetCompanyGroup_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        //[pramod.misal][GEOS2-4682][08-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCompanyPlantByIdUser_V2420(string idUser, bool isIncludeDefault = false);

        //[pramod.misal][GEOS2-4682][08-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanyPlantByUserId_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomersByPlant_V2420(string idSite);

        //[GEOS2-4682][08-08-2023][wrdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetSelectedUserCompanyGroup_V2420(string idUser, bool isIncludeDefault = false);

        //AnnualSalesPerformanceViewModel
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2490 instead.")]
        List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2490 instead.")]
        List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TempAppointment> GetDailyEvents_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCurrentUser);

        //DashboardSaleViewModel
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetDashboard2Details_IgnoreOfferCloseDate_V2490 instead.")]
        IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2420(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSitesTargetNewCurrencyConversion_V2420(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingFromYear, DateTime accountingToYear, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUsersSitesTargetNewCurrencyConversion_V2420(byte idCurrency, Int64 accountingFromYear, Int64 accountingToYear, string idUser);

        //DahsBoardPerformanceViewModel
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetSalesStatus_IgnoreOfferCloseDate_V2490 instead.")]
        IList<Offer> GetSalesStatus_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2490 instead.")]
        List<Offer> GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2490 instead.")]
        List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedUsersOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idCurrentUser = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetSelectedUsersOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2420(byte idCurrency, DateTime accountingYearFrom, DateTime accountingYearTo, string idUser, Company companyDetails, Int32 idCurrentUser = 0);

        //DashboardEngineeringAnalysisViewModel
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2490 instead.")]
        List<Offer> GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2420(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails);

        //Order
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2490 instead.")]
        List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2420(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany_V2430(Company company);


        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetCompanyDetailsById_V2580 instead.")]
        Company GetCompanyDetailsById_V2340(Int32 idSite);


        //[Sudhir.Jangra][GEOS2-4663][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use AddCompany_V2580 instead.")]
        Company AddCompany_V2430(Company company, Company emdepSite = null);

        //[sudhir.jangra][GEOS2-4664]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetSelectedUserCustomersBySalesOwnerId_V2450 instead.")]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2430(string idUser);

        //[Sudhir.jangra][GEOS2-4664]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetSelectedUserCustomersByPlant_V2450 instead.")]
        List<Company> GetSelectedUserCustomersByPlant_V2430(string idSite);

        //[Sudhir.jangra][GEOS2-4664]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetCustomersBySalesOwnerId_V2450 instead.")]
        List<Company> GetCustomersBySalesOwnerId_V2430(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        //[pramod.misal][GEOS2-4688][27.09.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserManagerDtl> GetSalesUserByPlant_V2440(string idPlants);

        //[rajashri.telvekar][GEOS2-4689]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllAttendesList_V2440(string idSite);


        //[pramod.misal][GEOS2-4690]][29-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<User> GetSalesAndCommericalUsers_V2440();

        //[19.10.2023][GEOS2-4903][rdixit] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetCompanyGroup_V2450(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        //[rdixit][GEOS2-4903][27.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetSelectedUserCustomersBySalesOwnerId_V2630 instead.")]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2450(string idUser);

        //[rdixit][GEOS2-4903][27.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetSalesOwnerBySiteId_V2450(Int32 idSite);

        //[rdixit][GEOS2-4903][27.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetSelectedUserCustomersByPlant_V2630 instead.")]
        List<Company> GetSelectedUserCustomersByPlant_V2450(string idSite);

        //[rdixit][GEOS2-4903][27.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetCustomersBySalesOwnerId_V2630 instead.")]
        List<Company> GetCustomersBySalesOwnerId_V2450(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        //rajashri [GEOS2-5014][6-12-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetAllSalesTeamUserWonDetail_V2630 instead.")]
        List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2460(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser);

        //Shubham[skadam] GEOS2-5041 Error when trying to close an analysis from CRM analysis window 13 12 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer UpdateOffer_V2460(Offer offer, Int32 idSite, Int32 idUser);


        //[cpatil][11-01-2024][GEOS2-5064]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use UpdateOffer_V2490 instead.")]
        Offer UpdateOffer_V2480(Offer offer, Int32 idSite, Int32 idUser);

        //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use UpdateActionPlanItem_V2500 instead.")]
        bool UpdateActionPlanItem_V2480(ActionPlanItem actionPlanItem);

        //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetActionPlanItem_V2500 instead.")]
        ActionPlanItem GetActionPlanItem_V2480(Int64 IdActionPlanItem);

        //chitra.girigosavi[15/01/2024][GEOS2-3802][ACTIONS_REVIEW] Edit ALLOW Multiselection in Action Tags:
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610. Use GetSalesActivityRegister_V2610 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister_V2480(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);

        //[rgadhave][GEOS2-3801][17.01.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use AddActionPlanItem_V2500 instead.")]
        ActionPlanItem AddActionPlanItem_V2480(ActionPlanItem actionPlanItem);

        //[Sudhir.Jangra][GEOS2-5170]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        int GetSourcePositionForLookupValue_V2480(int IdLookupKey);

        //[Sudhir.Jangra][GEOS2-5170]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        LookupValue AddNewSourceInLookupValue_V2480(LookupValue lookupValue);

        //[pramod.misal][GEOS2-5261][13.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Currency> GetCurrencies_V2490();

        //[pramod.misal][GEOS2-5347][16.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersByIdCustomerToLinkedWithActivities_V2490(byte idCurrency, Int32 idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company compdetails, Int32 idUserPermission, Int32 idCustomer);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetAllCompaniesDetails_V2500 instead.")]
        List<Company> GetAllCompaniesDetails_V2490(Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCurrentPlantId_V2490(Int32 idUser);

        //[cpatil][GEOS2-5348][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOfferForParticularColumn_V2490(Offer offer, Int32 idSite, Int32 idUser);


        //[cpatil][GEOS2-5348][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use AddOffer_V2600 instead.")]
        Offer AddOffer_V2490(Offer offer, Int32 idSite, Int32 idUser);


        //[cpatil][GEOS2-5348][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use UpdateOffer_V2500 instead.")]
        Offer UpdateOffer_V2490(Offer offer, Int32 idSite, Int32 idUser);

        //[cpatil][GEOS2-5348][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use AddOfferWithIdSourceOffer_V2500 instead.")]
        Offer AddOfferWithIdSourceOffer_V2490(Offer offer, Int32 idSite, Int32 idUser);

        //[cpatil][GEOS2-5348][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetSelectedOffersEngAnalysisDateWise_IgnoreCloseDate_V2490(string idUser, byte idCurrency, Int32 idCurrentUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails);

        //[cpatil][GEOS2-5348][29.02.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2680 instead.")]
        List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2490(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);



        #region GEOS2-5310 Sudhir.Jangra
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetOfferPipelineDatewise_IgnoreOfferCloseDate_V2490(byte idCurrency, string userids, Int32 idCurrentUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferDetail> GetSalesStatusWithTarget_IgnoreOfferCloseDate_V2490(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferDetail> GetSalesStatusByMonthAllPermission_IgnoreOfferCloseDate_V2490(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission, Int32 idCurrentUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Offer> GetSalesStatus_IgnoreOfferCloseDate_V2490(byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Company companydetails, Int32 idCurrentUser, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOfferLeadSourceCompanyWise_IgnoreOfferCloseDate_V2490(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LostReasonsByOffer> GetOfferLostReasonsDetailsCompanyWise_IgnoreOfferCloseDate_V2490(byte idCurrency, Int32 idActiveUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Customer> GetDashboard2Details_IgnoreOfferCloseDate_V2490(byte idCurrency, Int32 idCurrentUser, string idsUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 customerLimit, Company companyDetails, Int32 idUserPermission);

        //chitra.girigosavi GEOS2-3799 [ACTIONS_REVIEW] ADD “Linked Items” section in “ACTIONS”:
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActionPlanItem AddActionPlanItem_V2500(ActionPlanItem actionPlanItem);

        //chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateActionPlanItem_V2500(ActionPlanItem actionPlanItem);

        //chitra.girigosavi [14/02/2024] Geos2-3800 [ACTIONS_REVIEW] EDIT “Linked Items” section in “ACTIONS”
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetActionPlanItem_V2520 instead.")]
        ActionPlanItem GetActionPlanItem_V2500(Int64 IdActionPlanItem);
        #endregion

        //[GEOS2-5445][rdixit][14.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use AddOfferWithIdSourceOffer_V2600 instead.")]
        Offer AddOfferWithIdSourceOffer_V2500(Offer offer, Int32 idSite, Int32 idUser);

        //chitra.girigosavi[28/02/2024][GEOS2-3803][ACTIONS_REVIEW] Display “ACTIONS” in the “Opportunities
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetActivitiesByIdOffer_V2520 instead.")]
        List<Activity> GetActivitiesByIdOffer_V2500(Int64 idOffer, Int32 idEmdepSite);

        //chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetActivitiesLinkedToAccount_V2520 instead.")]
        List<Activity> GetActivitiesLinkedToAccount_V2500(string idOwner, Int32 idPermission, string idPlant, Int32 idSite, Int64 idOffer, Int32 idEmdepSite);

        //chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use UpdateOffer_V2520 instead.")]
        Offer UpdateOffer_V2500(Offer offer, Int32 idSite, Int32 idUser);

        //[GEOS2-5490][rdixit][15.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use ActionLauncherAdd_V2630 instead.")]
        ActionPlanItem ActionLauncherAdd_V2500(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList);

        //[GEOS2-5556][27.03.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompaniesDetails_V2500(Int32 idUser);

        //chitra.girigosavi[20/03/2024][GEOS2-3804][ACTIONS_REVIEW] Display “ACTIONS” in the “Orders"
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOrderByIdOffer_V2520(Offer offer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActionPlanItem GetActionPlanItem_V2520(Int64 IdActionPlanItem);

        //chitra.girigosavi[28/02/2024][GEOS2-3803][ACTIONS_REVIEW] Display “ACTIONS” in the “Opportunities
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdOffer_V2520(Int64 idOffer, Int32 idEmdepSite);

        //chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesLinkedToAccount_V2520(string idOwner, Int32 idPermission, string idPlant, Int32 idSite, Int64 idOffer, Int32 idEmdepSite);

        //chitra.girigosavi[28/02/2024][GEOS2-3805][ACTIONS_REVIEW] Assign “ACTIONS” in the “Opportunities”
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use UpdateOffer_V2600 instead.")]
        Offer UpdateOffer_V2520(Offer offer, Int32 idSite, Int32 idUser);


        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]    
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CpType> GetAllCpTypes_V2530();

        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]  
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CpType> GetCpTypesByTemplate_V2530(byte idTemplate);


        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Detection> GetDetectionByCpTypeAndTemplate_V2530(byte idTemplate, long idCPType);

        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetModulesReportDetails_V2530(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany,
            byte? idTemplate, long? idCPType, string idDetections, byte idCurrency, byte? idfamily, byte? idsubFamily, Int32? idCustomer = null);
        // [nsatpute][23-06-2024] GEOS2-5701
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2690. Use InsertImportedAccounts_V2690 instead.")]
        bool InsertImportedAccounts(List<Company> lstCompanies, Company emdepSite);

        // [nsatpute][23-06-2024] GEOS2-5701
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllActivePeoples_V2530();

        // [nsatpute][27-06-2024] GEOS2-5702
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Emdep.Geos.Data.Common.PLM.Group> GetAllActiveCompanyGroups();

        // [nsatpute][27-06-2024] GEOS2-5702
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanyListByIdGroup(int idGroup);

        // [nsatpute][27-06-2024] GEOS2-5702
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertImportedContacts(List<People> lstContacts);

        //chitra.girigosavi 24/07/2024 GEOS2-5645 Different styles applied in combobox selection
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2570. Use GetCountries_V2570 instead.")]
        List<Country> GetCountries_V2550();

        //[ashish.malkhede][30.07.2024] [GEOS2-5897] https://helpdesk.emdep.com/browse/GEOS2-5897
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetModulesReportDetails_V2550(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, Int32? idCompany,
        byte? idTemplate, long? idCPType, string idDetections, byte idCurrency, byte? idfamily, byte? idsubFamily, Int32? idCustomer = null);
        //[Rahul.Gadhave][GEOS2-6084][Date:04/09/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<User> GetSalesAndCommericalUsers_V2560();
        //[Rahul.Gadhave][GEOS2-6084][Date:12/09/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetUserById_V2560(int userId);

        //chitra.girigosavi GEOS2-6498 When add/Edit customer taking to much time to load form
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountries_V2570();

        // [pramod.misal][25-10-2024][GEOS2-6461]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Currency> GetCurrencies_V2580();

        //[GEOS2-6446][05.11.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlantsByIdSourceList_V2580(string idSourceList, string idRegionList, string idCountryList);

        //[pramod.misal][GEOS2-6462][15.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Emdep.Geos.Data.Common.Crm.ShippingAddress> GetShippingAddressByIdPlant(IList<Company> CompanyList);

        //[RGadhave][12.11.2024][GEOS2-6462]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Data.Common.OTM.CustomerDetail> GetIncotermsList_V2580();

        //[RGadhave][12.11.2024][GEOS2-6462]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Data.Common.OTM.CustomerDetail> GetPaymentTermsList_V2580();

        //[RGadhave][12.11.2024][GEOS2-6462]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use AddCompany_V2630 instead.")]
        Company AddCompany_V2580(Company company, int SelectedIncotermId, string SelectedPaymentTermsName, ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddressList, Company emdepSite = null);

        //[RGadhave][12.11.2024][GEOS2-6462]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Emdep.Geos.Data.Common.Crm.ShippingAddress> GetContentcountryimgurl_V2580(Int64 idcountry);

        //[pramod.misal][GEOS2-6462][18-11-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetCompanyDetailsById_V2630 instead.")]
        Company GetCompanyDetailsById_V2580(Int32 idSite);

        //[pooja.jadhav][22-11-2024][GEOS2-6462]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use UpdateCompany_V2630 instead.")]
        Company UpdateCompany_V2580(Company company);

        //[rdixit][GEOS2-6602][26.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetActionPlanItemResponsible_V2590(string idSiteList);
        // [Rahul.Gadhave][GEOS2-6011][Date:10-01-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<Currency> GetCurrencyByExchangeRate_V2600();
        //[Rahul.Gadhave][GEOS2-6817 - Q quotation generated without Date] [Date:14-01-2025] 
        [OperationContract]
        [ObsoleteAttribute("This method will be removed after version V2580. Use UpdateCompany_V2630 instead.")]
        [FaultContract(typeof(ServiceException))]
        Offer UpdateOffer_V2600(Offer offer, Int32 idSite, Int32 idUser);

        //[Rahul.Gadhave][GEOS2-6817 - Q quotation generated without Date] [Date:14-01-2025] 
        [OperationContract]

        [FaultContract(typeof(ServiceException))]
        Offer AddOffer_V2600(Offer offer, Int32 idSite, Int32 idUser);
        //[Rahul.Gadhave][GEOS2-6817 - Q quotation generated without Date] [Date:14-01-2025] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer AddOfferWithIdSourceOffer_V2600(Offer offer, Int32 idSite, Int32 idUser);


        //[cpatil][GEOS2-6664][05 - 02 - 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetSalesActivityRegister_V2650 instead.")]
        List<ActionPlanItem> GetSalesActivityRegister_V2610(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);


        //[GEOS2-6605][cpatil][05.03.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Tuple<string, Int64> MakeOfferCode_V2620(byte? idOfferType, Int32 idCustomer, Int32 idUser);

        //[nsatpute][19-03-2025][GEOS2-6991]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetArticlesReportDetails_V2620(Int32 idActiveUser, Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles);

        //[pooja.jadhav][GEOS2-6809][20-03-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreateFolderOffer_V2620(Offer offer, EmdepSite site, bool? isNewFolderForOffer = null);

        //chitra.girigosavi[GEOS2-7207][25/03/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetAllCustomer_V2630();
        //[Rahul.Gadhave][GEOS2-7650][Date:04-01-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SalesUserWon> GetAllSalesTeamUserWonDetail_V2630(Company company, byte idCurrency, string idUser, DateTime accountingYearFrom, DateTime accountingYearTo, Int32 idCurrentUser);

        //chitra.girigosavi GEOS2-6695 04/04/2025
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlantsByIdSourceList_V2630(string idSourceList, string idRegionList, string idCountryList, string SalesOwnerForPlant, string IdStatus);

        //chitra.girigosavi GEOS2-7242 02/04/2025
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany_V2630(Company company);

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetSelectedUserCustomersBySalesOwnerId_V2670 instead.")]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2630(string idUser);

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetSelectedUserCustomersByPlant_V2670 instead.")]
        List<Company> GetSelectedUserCustomersByPlant_V2630(string idSite);

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetCustomersBySalesOwnerId_V2670 instead.")]
        List<Company> GetCustomersBySalesOwnerId_V2630(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyDetailsById_V2630(Int32 idSite);

        //chitra.girigosavi[GEOS2-7242][10/04/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company AddCompany_V2630(Company company, int SelectedIncotermId, string SelectedPaymentTermsName, ObservableCollection<Emdep.Geos.Data.Common.Crm.ShippingAddress> shippingAddressList, Company emdepSite = null);

        //chitra.girigosavi[GEOS2-7242][14/04/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use ActionLauncherAdd_V2640 instead.")]
        ActionPlanItem ActionLauncherAdd_V2630(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList);

        //chitra.girigosavi[GEOS2-8129][19/05/2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ActionPlanItem ActionLauncherAdd_V2640(ActionPlanItem actionPlanItem, List<SitesWithCustomer> customerSiteList);

        //[nsatpute][19.06.2025][GEOS2-7032]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActionPlanItem> GetSalesActivityRegister_V2650(string idsSelectedUser, string idsSelectedPlant, Int32 idUser, Int32 idPermission, DateTime closedDate);

        //[pramod.misal][GEOS2-8169][17.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SitesWithCustomer> GetWithSiteswithSalesUsers_V2660();

        //[pramod.misal][GEOS2-8169][17.07.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Site> GetPlantsByIdSourceList_V2660(string idSourceList, string idRegionList, string idCountryList, string SalesOwnerForPlant, string IdStatus);
         
        // [pallavi.kale][04-09-2025] [GEOS2-8949]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetSelectedUserCustomersBySalesOwnerId_V2680 instead.")]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2670(string idUser);
       
        // [pallavi.kale][04-09-2025] [GEOS2-8949]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetCustomersBySalesOwnerId_V2680 instead.")]
        List<Company> GetCustomersBySalesOwnerId_V2670(Int32 idUser, Int32 idZone, Int32 idUserPermission);

        // [pallavi.kale][04-09-2025] [GEOS2-8949]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetSelectedUserCustomersByPlant_V2680 instead.")]
        List<Company> GetSelectedUserCustomersByPlant_V2670(string idSite);

        //[Rahul.Gadhave][Date:08/10/2025][https://helpdesk.emdep.com/browse/GEOS2-8440]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2680(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);
       
        //[pallavi.kale][GEOS2-9792][09.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomersByPlant_V2680(string idSite);

        //[pallavi.kale][GEOS2-9792][09.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCustomersBySalesOwnerId_V2680(string idUser);

        //[pallavi.kale][GEOS2-9792][09.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCustomersBySalesOwnerId_V2680(Int32 idUser, Int32 idZone, Int32 idUserPermission);
       
        //[pallavi.kale][GEOS2-8955][09.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateContact_V2680(People people);

        //[pallavi.kale][GEOS2-8955][09.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsByIdPermission_V2680(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ObservableCollection<User> GetAssigneeUserList_V2680();
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EngineeringAnalysis> GetEngAnalysisAllRevisionsByOfferCode_V2680(Offer offer);
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EngineeringAnalysisType CRM_GetAssignedToFromOtitems_V2680(Int64 IdRevisionItem);
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        User GetAssigneeUserIdUserwise_V2680(Int64 AssignedToUser);
        //[rahul.gadhave][16-10-2025][GEOS2-8438]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Offer UpdateOffer_V2680(Offer offer, Int32 idSite, Int32 idUser);


        //[pramod.misal][19-11-2025][GEOS2-8444] https://helpdesk.emdep.com/browse/GEOS2-8444
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersEngineeringAnalysisByPermission_IgnoreCloseDate_V2690(byte idCurrency, Int32 idUser, Int32 idZone, DateTime accountingYearFrom, DateTime accountingYearTo, Company companyDetails, Int32 idUserPermission);

        //[nsatpute][03.12.2025][GEOS2-10361]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertImportedAccounts_V2690(List<Company> lstCompanies, Company emdepSite);
    }
}
