using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISRMService" in both code and config file together.
    [ServiceContract]
    public interface ISRMService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetPendingPurchaseOrdersByWarehouse_V2110 instead.")]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetPurchaseOrderPdf(string AttachPDF);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetPendingPOByIdWarehousePurchaseOrder_V2110 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder(Int64 idWarehousePurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetAllWorkflowStatus_V2301 instead.")]
        List<WorkflowStatus> GetAllWorkflowStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetAllWorkflowTransitions_V2301 instead.")]
        List<WorkflowTransition> GetAllWorkflowTransitions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use UpdateWorkflowStatusInPO_V2110 instead.")]
        bool UpdateWorkflowStatusInPO(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version _V2250. Use GetArticleSuppliersByWarehouse_V2250 instead.")]
        List<ArticleSupplier> GetArticleSuppliersByWarehouse(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        //[ObsoleteAttribute("This method will be removed after version V2690. Use DeleteArticleSupplier_V2690 instead.")]
        bool DeleteArticleSupplier(Int64 idArticleSupplier, int IdUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SendSupplierPurchaseOrderRequestMail(WarehousePurchaseOrder warehousePurchaseOrder, string EmailFrom);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetPendingPurchaseOrdersByWarehouse_V2301 instead.")]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2110(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetPendingPOByIdWarehousePurchaseOrder_V2300 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2110(Int64 idWarehousePurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetPendingArticles(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<User> GetPermissionUsers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2380. Use UpdateWorkflowStatusInPO_V2380 instead.")]
        bool UpdateWorkflowStatusInPO_V2110(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetBodyAndUpdateReminderDateInPO_V2650 instead.")]
        List<KeyValuePair<string, string>> GetBodyAndUpdateReminderDateInPO(Warehouses warehouse, List<WarehousePurchaseOrder> warehousePurchaseOrderList, Dictionary<string, List<long>> supplierEmailId, int IdModifiedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<KeyValuePair<string, string>> GetBodyAndUpdateReminderDateInPO_V2650(Warehouses warehouse, List<WarehousePurchaseOrder> warehousePurchaseOrderList, Dictionary<string, List<long>> supplierEmailId, int IdModifiedBy);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetArticleSupplierByIdArticleSupplier_V2240 instead.")]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier(Warehouses warehouse, UInt64 IdArticleSupplier);

        //GEOS_3434

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Contacts AddContact(Contacts contacts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Contacts GetContacts(int idContact);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Contacts> GetAllContacts();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Contacts> GetContactsByIdArticleSupplier(int idArticleSupplie);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use ArticleSupplierContacts_Insert_V2300 instead.")]
        void ArticleSupplierContacts_Insert(int idContact, Int64 idArticleSupplier);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetArticleSuppliersByWarehouse_V2340 instead.")]
        List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2250(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetArticleSupplierByIdArticleSupplier_V2250 instead.")]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2240(Warehouses warehouse, UInt64 IdArticleSupplier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetArticleSupplierByIdArticleSupplier_V2340 instead.")]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2301(Warehouses warehouse, UInt64 IdArticleSupplier);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2250(Warehouses warehouse, UInt64 IdArticleSupplier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByArticleSuppliers> GetLogEntriesByArticleSuppliers(Warehouses warehouse, UInt64 IdArticleSupplier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use DeleteContacts_V2300 instead.")]
        bool DeleteContacts(int idContact);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ArticleSupplierContacts_Insert_V2250(int idContact, Int64 idArticleSupplier, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use AddContact_V2300 instead.")]
        Contacts AddContact_V2250(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use SetIsMainContact_V2300 instead.")]
        bool SetIsMainContact(int idContact, Int64 idArticleSupplier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use AddCommentsOrLogEntriesByArticleSuppliers_V2300 instead.")]
        void AddCommentsOrLogEntriesByArticleSuppliers(List<LogEntriesByArticleSuppliers> AddLogEntries);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetContacts_V2430 instead.")]
        Contacts GetContacts_V2250(int idContact);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Contacts> GetAllContacts_V2250();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SetIsMainContact_V2300(int idContact, Int64 idArticleSupplier);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use DeleteContacts_V2530 instead.")]
        bool DeleteContacts_V2300(int idContact);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddCommentsOrLogEntriesByArticleSuppliers_V2300(List<LogEntriesByArticleSuppliers> AddLogEntries);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetPendingPOByIdWarehousePurchaseOrder_V2380 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2300(Int64 idWarehousePurchaseOrder);
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use AddContact_V2430 instead.")]
        Contacts AddContact_V2300(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use ArticleSupplierContacts_Insert_V2480 instead.")]
        void ArticleSupplierContacts_Insert_V2300(int idContact, Int64 idArticleSupplier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountries_V2301();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Group> GetGroups_V2301();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Categorys> GetCategorys_V2301();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PaymentTerm> GetPayments_V2301();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use UpdateArticleSupplier_V2340 instead.")]
        bool UpdateArticleSupplier(ArticleSupplier articleSupplier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetWorkFlowStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2310. Use GetPendingPurchaseOrdersByWarehouse_V2310 instead.")]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2301(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPODetailsForGridUpdate(Int64 idWarehousePurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use IsUpdateWarehousePurchaseOrderWithStatus_V2450 instead.")]
        bool IsUpdateWarehousePurchaseOrderWithStatus(WarehousePurchaseOrder warehousePurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetAllWorkflowStatus_V2301();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use GetAllWorkflowTransitions_V2400 instead.")]
        List<WorkflowTransition> GetAllWorkflowTransitions_V2301();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2310(Warehouses warehouse);
        
        //[GEOS2-4027][rdixit][18.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetArticleSupplierByIdArticleSupplier_V2360 instead.")]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2340(Warehouses warehouse, UInt64 IdArticleSupplier);
        //[GEOS2-4027][rdixit][18.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2340(Warehouses warehouse);

        //[GEOS2-4027][rdixit][18.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use UpdateArticleSupplier_V2370 instead.")]
        bool UpdateArticleSupplier_V2340(ArticleSupplier articleSupplier);


        //[GEOS2-3441][sshegaonkar][24.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2360(Warehouses warehouse, UInt64 IdArticleSupplier);


        //[GEOS2-3442][cpatil][20.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetArticleSupplierByIdArticleSupplier_V2440 instead.")]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2370(Warehouses warehouse, UInt64 IdArticleSupplier);


        //[GEOS2-3442][cpatil][20.03.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateArticleSupplier_V2370(ArticleSupplier articleSupplier);

        //[GEOS2-4310][rdixit][05.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2380(Int64 idWarehousePurchaseOrder);

        //[GEOS2-4309][rdixit][10.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2380(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked);

        //[rdixit][GEOS2-4313][17.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInPO_V2380(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover);

        //[rdixit][GEOS2-4313][17.04.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateAssigneeAndApproverInPO_V2380(WarehousePurchaseOrder warehousePurchaseOrder);

        //[Sudhir.jangra][GEOS2-4401][02/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2390(long idWarehouse,string connectionstring);

        //[Sudhir.Jangra][GEOS2-4402][03/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetPendingArticles_V2390(long idWarehouse,string connectionString);

        //[Sudhir.Jangra][GEOS2-4403][03/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetArticleSuppliersByWarehouse_V2440 instead.")]
        List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2390(long idWarehouse,string connectionString);

        //[Sudhir.Jangra][GEOS2-4407][04/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GeosAppSetting> GetSystemSettings_V2390();


        //[Sudhir.Jangra][GEOS2-4407][09/05/2023]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSystemSettings_V2390(string DefaultValue);

        //[Sudhir.Jangra][GEOS2-4407][19/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use GetArticleSupplierByIdArticleSupplier_V2400 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2390(Int64 idWarehousePurchaseOrder);

        //[Sudhir.Jangra][GEOS2-4407]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use SendPOMailForSelectedPO_V2500 instead.")]
        List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2390(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked);

        //[Sudhir.Jangra][GEOS2-4487][31/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2400(long idWarehouse, string connectionstring);

        //[pramod.misal][GEOS2-4431][16/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2400(Int64 idWarehousePurchaseOrder);
        //Shubham[skadam]  GEOS2-4404 (View Supplier) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        //Shubham[skadam]  GEOS2-4405 (Grid) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePurchasingOrderStatus(Int64 idWarehousePurchaseOrder, Int64 IdUser);
        //Shubham[skadam]  GEOS2-4404 (View Supplier) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        //Shubham[skadam]  GEOS2-4405 (Grid) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int32 GetPurchasingOrderStatus(Int64 idWarehousePurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        POEmailNotification GetPurchasingOrderNotificationDetails(Int64 idWarehousePurchaseOrder,Int64 IdUser);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowTransition> GetAllWorkflowTransitions_V2400();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use PurchasingOrderNotificationSend_V2560 instead.")]        
        bool PurchasingOrderNotificationSend(POEmailNotification POEmailNotification);

        //[rdixit][28.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SendEmailForPO(string emailto, string sub, string emailbody, Dictionary<string, byte[]> attachments, string EmailFrom, List<string> ccAddress);


        //[pramod.misal][GEOS2-4448][03/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetPendingPurchaseOrdersByWarehouse_V2420 instead.")]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2410(long idWarehouse, string connectionstring);

        //[pramod.misal][GEOS2-4449][04/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2410(Int64 idWarehousePurchaseOrder);

        //[GEOS2-4453][21.07.2023][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetOfferExportChart_V2510 instead.")]
        WarehousePurchaseOrder GetOfferExportChart(int idPo, int idWarehouse);

        //[GEOS2-4453][21.07.2023][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ExportPurchaseOrderReport(WarehousePurchaseOrder po, byte[] pdffile, byte[] excelfile);

        //[pramod.misal][GEOS2-4451][21/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetPendingPOByIdWarehousePurchaseOrder_V2430 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2420(Int64 idWarehousePurchaseOrder,string idShippingAddress);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetPendingPurchaseOrdersByWarehouse_V2450 instead.")]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2420(long idWarehouse, string connectionstring);

        //[pramod.misal][GEOS2-4450][1/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use UpdateAssigneeAndApproverInPO_V2480 instead.")]
        bool UpdateAssigneeAndApproverInPO_V2420(WarehousePurchaseOrder warehousePurchaseOrder);

        //[pramod.misal][GEOS2-4755][16.08.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use UpdateWorkflowStatusInPO_V2530 instead.")]
        bool UpdateWorkflowStatusInPO_V2420(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover);

        //[pramod.misal][GEOS2-4674][22-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Contacts AddContact_V2430(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);


        //[pramod.misal][GEOS2-4675][22-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Contacts GetContacts_V2430(int idContact);

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DisableContact(People people);

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsByIdPermission_V2390(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetAllCountriesDetails();

        //[Sudhir.jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCustomerCompanies();

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        People GetContactsByIdPerson(Int32 idPerson);

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetOffersByIdSiteToLinkedWithActivities(byte idCurrency, Int32 idUser, Int64 accountingYear, Company company, Int32 idUserPermission, Int32 idSite);

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdSite(Int32 idSite);

        //[Sudhir.jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntryBySite> GetAllLogEntriesByIdSite(Int64 idSite);

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetCountries();

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetSelectedUserCompanyGroup_V2420(string idUser, bool isIncludeDefault = false);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Customer> GetCompanyGroup_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        //[Sudhir.jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetSelectedUserCompanyPlantByIdUser_V2420(string idUser, bool isIncludeDefault = false);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanyPlantByUserId_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetSalesOwnerBySiteId_V2390(Int32 idSite);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetSiteContacts(Int64 idSite);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany_V2430(Company company);

        //[sudhir.jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetAllActivePeoples_V2390();

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Activity GetActivityByIdActivity_V2035(Int64 idActivity);

        //[Sudhir.jangra][GEOS2-4676][29/08/2023]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyDetailsById_V2340(Int32 idSite);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Competitor> GetCompetitors();

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Activity> GetActivitiesByIdContact(Int32 idContact);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByContact> GetLogEntriesByContact(Int32 idContact, byte idLogEntryType);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateContact(People people);

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use AddContact_V2440 instead.")]
        People AddContact_V2033(People people);
        //[Sudhir.jangra][GEOS2-4676]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        People AddSRMContact_V2430(People people);

        //[Sudhir.jangra][GEOS2-4676]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<People> GetContactsByIdPermission_V2430(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission);

        //[Sudhir.jangra][GEOS2-4676][29/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateContact_V2430(People people);

        // [rdixit][GEOS2-4822][13-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetPendingPOByIdWarehousePurchaseOrder_V2440 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2430(Int64 idWarehousePurchaseOrder, string idShippingAddress);

        //[Sudhir.jangra][GEOS2-4676]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleSupplier> GetArticleSuppliersForSRMContact_V2430(Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4676]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Contacts> GetContactsAndSupplier_V2430(Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteContactDetails_V2340(Contacts contact);

        //[Sudhir.Jangra][GEOS2-4676]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ArticleSupplierContacts_Update_V2430(Int32 IdArticleSupplierContact, long IdArticleSupplier);

        //[Sudhir.Jangra][GEOS2-4740][26/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        
        List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2440(long idWarehouse, string connectionString);


        //[Sudhir.jangra][GEOS2-4738][27/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleSupplier GetSRMEmdepCodeForAddSupplier();

        //[Sudhir.jangra][GEOS2-4738][27/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use AddContact_V2450 instead.")]
        Contacts AddContact_V2440(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);

        //[Sudhir.Jangra][GEOS2-4738]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use AddArticleSupplier_V2450 instead.")]
        bool AddArticleSupplier_V2440(ArticleSupplier articleSupplier,List<Contacts> contactsList,List<Warehouses> warehouses);

        //[Sudhir.jangra][GEOS2-4738]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
       
        ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2440(Warehouses warehouse, UInt64 IdArticleSupplier);

        //[rdixit][GEOS2-4589][09-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetPendingPOByIdWarehousePurchaseOrder_V2460 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2440(Int64 idWarehousePurchaseOrder, string idShippingAddress);

        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByArticleSuppliers> GetArticleSuppliersContactsComments_V2450(int idContact);
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByArticleSuppliers> GetArticleSuppliersContactsChangelog_V2450(int idContact);
        //[chitra.girigosavi][GEOS2-4692][17.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use AddContact_V2480 instead.")]
        Contacts AddContact_V2450(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);

        // [rdixit][19.10.2023][GEOS2-4961]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use AddContact_V2480 instead.")]
        bool AddArticleSupplier_V2450(ArticleSupplier articleSupplier, List<Contacts> contactsList, List<Warehouses> warehouses);

        // [cpatil][30.10.2023][GEOS2-4902]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetPendingPurchaseOrdersByWarehouse_V2510 instead.")]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2450(long idWarehouse, string connectionstring);

        //Shubham[skadam] GEOS2-5026 Log not added properly in purchase order 08 11 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdateWarehousePurchaseOrderWithStatus_V2450(WarehousePurchaseOrder warehousePurchaseOrder);
        //Shubham[skadam] GEOS2-4965 General discount row is not added in the PO 24 11 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetPendingPOByIdWarehousePurchaseOrder_V2500 instead.")]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2460(Int64 idWarehousePurchaseOrder, string idShippingAddress);

        //[pramod.misal][GEOS2-4450][1/08/2023]
        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateAssigneeAndApproverInPO_V2480(WarehousePurchaseOrder warehousePurchaseOrder);

        //[chitra.girigosavi][GEOS2-4692][17.10.2023]
        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use AddContact_V2560 instead.")]
        Contacts AddContact_V2480(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);

        //[pramod.misal][GEOS2-5136][22.01.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use ArticleSupplierContacts_Insert_V2560 instead.")]
        void ArticleSupplierContacts_Insert_V2480(int idContact, Int64 idArticleSupplier);

        //[pramod.misal][GEOS2-5136][23.01.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SetIsMainContact_V2480(int idContact, Int64 idArticleSupplier);
        //rajashri GEOS2-5375
        #region
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2500(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetWarehouseHolidays(DateTime? deliverydate,long idWarehouse,long IdWarehousePurchaseOrder);
        #endregion
        //rajashri GEOS2-5376
        #region
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetCustomMessageforHoliday_V2500();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCustomMessageForPORequestMail_V2500(List<Warehouses> warehouse);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetCustomMessageforHoliday_PORequestMail_V2500(long? idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2500(Int64 idWarehousePurchaseOrder, string idShippingAddress);

        #endregion

        //chitra.girigosavi ON [3/04/2024] GEOS2-5528 After Execute the PO in SRM need flag to 1
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePoflag(WarehousePurchaseOrder selectedPurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteArticleSupplierOrder_V2510(UInt32 idArticleSupplierPOReceiver);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use AddDeleteArticleSupplierOrder_V2520 instead.")]
        bool AddDeleteArticleSupplierOrder_V2510(List<Contacts> contactList, long idWarehouse);

        //[Sudhir.Jangra][GEOS2-5491]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetArticleSuppliersOrders_V2520 instead.")]
        List<Contacts> GetArticleSuppliersOrders_V2510(Int32 idArticleSupplier,long idWarehouse);


        // [cpatil][30.04.2024][GEOS2-5618]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2510(long idWarehouse, string connectionstring);

        //rajashri [GEOS2-5461]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetCompletedPurchaseOrdersByWarehouse_V2520(DateTime startDate, DateTime endDate, long idWarehouse, string connectionstring);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetOfferExportChart_V2520(int idPo, int idWarehouse);

        //rajashri 5762
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use SendPOMailForSelectedPO_V2660 instead.")]
        List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2520(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetWarehouseHolidays_V2520(DateTime? deliverydate, long idWarehouse, long IdWarehousePurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2520(Int64 idWarehousePurchaseOrder, string idShippingAddress);

        // [rushikesh.gaikwad]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]   
        bool AddDeleteArticleSupplierOrder_V2520(List<Contacts> contactList, long idWarehouse);

        // [rushikesh.gaikwad]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Contacts> GetArticleSuppliersOrders_V2520(Int32 idArticleSupplier, long idWarehouse);

        //[Sudhir.Jangra][GEOS2-5827]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddArticleSupplier_V2530(ArticleSupplier articleSupplier, List<Contacts> contactsList, List<Warehouses> warehouses);

        //[pramod.misal][GEOS2-5495][16.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInPO_V2530(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover, string comment);

        // [nsatpute][12-06-2024] GEOS2-5463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseCategory> GetWarehouseCategories();
		
		// [nsatpute][12-06-2024] GEOS2-5463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Catalogue> GetCatalogue(string connectionString, string reference, string articleSupplierIds, string idCategories, string conditionalOperator, string stockValue, long idWarehouse, Currency currency);

        // [nsatpute][12-06-2024] GEOS2-5463
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleSupplier> GetArticleSuppliersByWarehouseForCatalogue(long idWarehouse, string connectPlantConstr);

        // [Rahul.Gadhave][17-06-2024] [GEOS2-2530]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteContacts_V2530(int idContact);

        //[Sudhir.Jangra][GEOS2-5634]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2550. Use GetPendingReviewByWarehouse_V2550 instead.")]
        List<Ots> GetPendingReviewByWarehouse_V2540(Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-5634]
        [OperationContract]
        List<Template> GetTemplatesByIdTemplateType(Int64 idTemplateType);

        //[Sudhir.Jangra][GEOs2-5634]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2550. Use GetPendingReviewByIdOt_V2550 instead.")]
        Ots GetPendingReviewByIdOt_V2540(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-5634]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTWorkingTime> GetOTWorkingTimeDetails_V2540(Int64 idOT, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-5634]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowTransition> GetAllWorkflowTransitions_V2540();

        //[Sudhir.Jangra][GEOS2-5634]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetAllWorkflowStatus_V2540();

        //[Sudhir.Jangra][GEOS2-5635]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2550. Use GetArticleDetails_V2550 instead.")]
        Article GetArticleDetails_V2540(Int32 idArticle);

        //[Sudhir.Jangra][GEOS2-5635]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2540(Int64 idWarehouseDeliveryNote);

        //[Sudhir.Jangra][GEOS2-5635]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkLog_V2540(Company company, OTWorkingTime otWorkingTime);

        //[Sudhir.Jangra][GEOS2-5635]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEditWorkOrderOTItems_V2540(List<OtItem> ots, List<SRMOtItemsComment> changeLog);


        //[Sudhir.Jangra][GEOS2-5635]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAssignedUser> GetOTAssignedUsers_V2540(Company company, Int64 idOT);

        //[Sudhir.Jangra][GEOS2-5636]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<Ots> GetPendingReviewListForArticle_V2540(Int64 idOt,Int32 idArticle,Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-5635]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEditWorkOrderOTItemsForArticles_V2540(Ots ots, List<SRMOtItemsComment> changeLog);

        //[Sudhir.Jangra][GEOS2-6049]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use GetPendingReviewByIdOt_V2660 instead.")]
        Ots GetPendingReviewByIdOt_V2550(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-6049]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingReviewByWarehouse_V2550(Warehouses warehouse);

        //[Sudhir.jangra][GEOS2-5636]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Article GetArticleDetails_V2550(Int32 idArticle,Int64 idSite);

        //[rdixit][02.09.2024][GEOS2-6383]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool PurchasingOrderNotificationSend_V2560(POEmailNotification POEmailNotification);

        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleSupplier> GetArticleSupplierContactsByIdContact_V2560(Warehouses warehouse, Int32 IdContact);


        //[chitra.girigosavi][GEOS2-4692][17.10.2023]
        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Contacts AddContact_V2560(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList);

        //[pramod.misal][GEOS2-5136][22.01.2024]
        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ArticleSupplierContacts_Insert_V2560(int idContact, Int64 idArticleSupplier);

        // [nsatpute][13-01-2025][GEOS2-6443]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetPendingReviewByIdOt_V2620 instead.")]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetPendingReviewByIdOt_V2600(Int64 idOt, Warehouses warehouse);

        // [nsatpute][21-01-2025][GEOS2-5725]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetAllArticles();

        // [nsatpute][23-01-2025][GEOS2-5725]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleSupplier> GetAllSuppliersForPurchasingReport();

        // [nsatpute][23-01-2025][GEOS2-5725]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlePurchasing> GetPurchasingReport(DateTime fromDate, DateTime toDate, long idArticleSupplier, int idArticle, long IdWarehouse);
	
		//[nsatpute][26-02-2025][GEOS2-7034]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetPendingReviewByIdOt_V2620(Int64 idOt, Warehouses warehouse);

        // [pallavi.kale][04-03-2025][GEOS2-7012]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetPendingPurchaseOrdersByWarehouse_V2650 instead.")]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2620(long idWarehouse, string connectionstring);

        // [pallavi.kale][04-03-2025][GEOS2-7012]
        //Shubham[skadam] GEOS2-8262 PO reminder Mail send to whom 29 05 2025
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2650(long idWarehouse, string connectionstring);

        // [pallavi.kale][04-03-2025][GEOS2-7013]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2620(Int64 idWarehousePurchaseOrder, string idShippingAddress);
        
        // [pallavi.kale][04-03-2025][GEOS2-7012]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetCompletedPurchaseOrdersByWarehouse_V2620(DateTime startDate, DateTime endDate, long idWarehouse, string connectionstring);

        //[GEOS2-7979][rdixit][16.07.2025][All connected tasks]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PreOrder> GetAllPreOrder_V2660(Warehouses warehouse, DateTime fromDate, DateTime toDate);

        //[GEOS2-7979][rdixit][16.07.2025][All connected tasks]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetPOIdByCode(Warehouses warehouse, string poCode);

        //[Rahul.gadhave][GEOS2-7243][Date:17-07-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2660(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked);

        //[rdixit][GEOS2-8252][16.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<string> GetExistingPreOrderCodes_V2680(string code);

        //[rdixit][GEOS2-8252][16.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehousesByUserPermissionInSRM_V2680(Int32 idActiveUser);

        //[rdixit][GEOS2-8252][16.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PreOrder AddPreOrder_V2680(PreOrder preOrder);

        //[rdixit][GEOS2-8252][16.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PreOrder> GetAllPreOrder_V2680(Warehouses warehouse, DateTime fromDate, DateTime toDate);
        
        //[pallavi.kale][GEOS2-9558][17.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlePurchasing> GetPurchasingReport_V2680(DateTime fromDate, DateTime toDate, long idArticleSupplier, int idArticle, long IdWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteArticleSupplier_V2690(Int64 idArticleSupplier, int IdUser);
    }
}
