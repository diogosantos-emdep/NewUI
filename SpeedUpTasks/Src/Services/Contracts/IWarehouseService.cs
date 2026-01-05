using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWarehouseService" in both code and config file together.
    [ServiceContract]
    public interface IWarehouseService
    {
        [OperationContract]
        List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReception();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetPurchaseOrdersPendingReceptionByWarehouse_V2035 instead.")]
        List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse(string warehouseIds, Warehouses warehouse);

        [OperationContract]
        WarehousePurchaseOrder GetPurchaseOrderPendingReceptionByIdWarehousePurchaseOrder(Int64 idWarehousePurchaseOrder, Warehouses warehouse = null);

        [OperationContract]
        List<Template> GetTemplatesByIdTemplateType(Int64 idTemplateType);

        [OperationContract]
        List<Ots> GetPendingMaterialWorkOrders();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetPendingMaterialWorkOrdersByWarehouse_V2032 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse(string warehouseIds, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Quotation> GetAllQuotationsOfTypeSites(Int32 idActiveUser, byte idCurrency, DateTime? accountingYearFrom, DateTime? accountingYearTo, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt(Int64 idOt, Int32 idWarehouse, Warehouses warehouse = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetPendingArticles_V2033 instead.")]
        List<Article> GetPendingArticles(Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetAllWarehousesByUserPermission_V2034 instead.")]
        List<Warehouses> GetAllWarehousesByUserPermission(Int32 idActiveUser);

        [OperationContract]
        byte[] GetDeliveryNotePdf(WarehouseDeliveryNote warehouseDeliveryNote);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetWarehouseArticlesStockByWarehouse_V2036 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse(string warehouseIds, Warehouses warehouse);

        [OperationContract]
        WarehouseDeliveryNote GenerateDeliveryNote(WarehousePurchaseOrder warehousePurchaseOrder);

        [OperationContract]
        WarehouseDeliveryNote AddWarehouseDeliveryNote(WarehouseDeliveryNote warehouseDeliveryNote);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetAllCountries();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SerialNumber> GetAllSerialNumbers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetRemainingOtItemsByIdOt(Int64 idOt, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStock(PickingMaterials pickingMaterials);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetArticlesPendingStorage_V2034 instead.")]
        List<PendingStorageArticles> GetArticlesPendingStorage(Int64 IdWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetArticlesWarehouseLocation_V2032 instead.")]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocation(string IdArticles, long IdWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStockForLocateMaterial(PendingStorageArticles pendingStorageArticles);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetMaterialDetailsByLocationName_V2034 instead.")]
        List<TransferMaterials> GetMaterialDetailsByLocationName(string locatioName, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStockForTransferMaterial(TransferMaterials transferMaterials);

        [OperationContract]
        WarehouseDeliveryNote GetWarehouseDeliveryNoteById(Int64 idWarehouseDeliveryNote);

        [OperationContract]
        bool UpdateWarehouseDeliveryNoteItem(WarehouseDeliveryNoteItem warehouseDeliveryNoteItem);

        [OperationContract]
        bool SaveWarehouseDeliveryNotePdf(WarehouseDeliveryNote warehouseDeliveryNote);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int32 GetArticleStockByWarehouse(int idArticle, int idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetAllWarehouseLocationById_V2034 instead.")]
        List<WarehouseLocation> GetAllWarehouseLocationById(long idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        MyWarehouse GetWarehouseStockDetailsByArticleAndWarehouse(Int32 idArticle, Int64 idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetZoneByIdArticle_V2034 instead.")]
        string GetZoneByIdArticle(Int64 idWarehouseDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetWarehousePODetailsByCode(string code);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCategory> GetArticleCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Article GetArticleDetailsByReference(string reference, Int64 idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetAllWarehouseLocationsByIdWarehouse(Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> AddArticleWarehouseLocation(List<ArticleWarehouseLocations> articleWarehouseLocations);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetAllArticlesWithWarehouseLocations_V2034 instead.")]
        List<Article> GetAllArticlesWithWarehouseLocations(string idWarehouses, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetAllWarehouseLocations();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetWarehouseLocationsByIdWarehouse_V2034 instead.")]
        List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse(string warehouseIds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use UpdateArticleDetails_V2041 instead.")]
        bool UpdateArticleDetails(Article article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdateItemStatusAndStage_V2035 instead.")]
        bool UpdateItemStatusAndStage(Int64 idOtItem, byte idItemOtStatus, Int32 idOperator);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseLocation AddWarehouseLocation(WarehouseLocation warehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseLocation UpdateWarehouseLocation(WarehouseLocation warehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistWarehouseLocationName(string name, Int64 parent, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateRevisionItemComments(Int64 idRevisionItem, Int64 idDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetWarehouseLocationBySelectedWarehouse_V2034 instead.")]
        List<WarehouseLocation> GetWarehouseLocationBySelectedWarehouse(Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetIsLeafWarehouseLocations_V2034 instead.")]
        List<WarehouseLocation> GetIsLeafWarehouseLocations(string warehouseIds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSerialNumber(SerialNumber serialNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetRefillToList_V2034 instead.")]
        List<LocationRefill> GetRefillToList(string idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetArticleWarehouseLocation_V2034 instead.")]
        LocationRefill GetArticleWarehouseLocation(Int32 idArticle, Int64 position);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetArticlesWarehouseLocationTransfer_V2033 instead.")]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocationTransfer(string IdArticles, long IdWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetRefillMaterialDetails_V2034 instead.")]
        List<TransferMaterials> GetRefillMaterialDetails(string fromLocationName, string toLocationName, Int64 idWarehouse, LocationRefill toLocationRefill = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleWarehouseLocations AddArticleWarehouseLocationByFullName(ArticleWarehouseLocations articleWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistWarehouseLocationFullName(string fullName, long idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int16 GetWorkorderStatus(Int64 idOT);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetMaxPosition(Int64 idParent, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetWarehouseLocationToPlaceArticle_V2034 instead.")]
        List<WarehouseLocation> GetWarehouseLocationToPlaceArticle(Int64 idWarehouse, string reference);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseLocation GetWarehouseLocationByFullName(Int64 idWarehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetIdOtByBarcode(string barcode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetAllArticlesByWLFullName(string warehouseIds, Warehouses warehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetRefillToListByFullName_V2034 instead.")]
        List<LocationRefill> GetRefillToListByFullName(string idWarehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SupplierComplaint> GetSupplierComplaints(string idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        SupplierComplaint GetSupplierComplaintDetails(Int64 idSupplierComplaint, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SupplierComplaintItem> GetRemainingSCItemsByIdSC(Int64 idSupplierComplaint, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStockSC(PickingMaterialsSC pickingMaterialsSc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetWarehouseLocationsByIdArticles(string IdArticles, Int64 IdWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetPickingItemsForSupplierComplaintItemArticlesAndLocation_V2034 instead.")]
        List<PickingMaterialsSC> GetPickingItemsForSupplierComplaintItemArticlesAndLocation(string IdArticles, Int64 IdWarehouse, Int64 IdWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleWarehouseLocations GetArticleStockByWarehouseLocation(Int32 IdArticle, Int64 IdWarehouseLocation, Int64 IdWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetAVGStockByIdArticle_V2034 instead.")]
        ArticleWarehouseLocations GetAVGStockByIdArticle(Int32 IdArticle, Int64 IdWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetLabelPrintDetails_V2032 instead.")]
        WarehouseDeliveryNote GetLabelPrintDetails(Int32 IdArticle, Int64 IdWarehouse, Int64 IdWarehouseDeliveryNote);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2031. Use GetArticleStockDetailForRefund_V2031 instead.")]
        List<PickingMaterials> GetArticleStockDetailForRefund(Int64 idOT, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PickingMaterials GetMadeInPartNumberDetail(Int64 idOTItem, Int64 idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdateRevisionItemComments_V2035 instead.")]
        bool UpdateRevisionItemComments_Sprint59(Int64 idRevisionItem, Int64 idDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetRefundIdOtByBarcode(string barcode);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetStockForScanItem(Int64 idArticle, Int64 idWarehouse, Int64 idWarehouseDeliveryNoteItem, Int64 idWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use FillPickingMaterialByIdWarehouseLocation_V2034 instead.")]
        List<PickingMaterials> FillPickingMaterialByIdWarehouseLocation(Int64 idOT, Warehouses warehouse, Int64 idWarehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetArticleDetailsByReference_V2033 instead.")]
        Article GetArticleDetailsByReference_V2030(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetWarehouseDeliveryNoteCode_V2032 instead.")]
        List<ArticlesStock> GetWarehouseDeliveryNoteCode(Int64 idOTItem, Warehouses warehouse, Int64? idWarehouseProductComponent);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetPendingMaterialArticles_V2033 instead.")]
        List<OtItem> GetPendingMaterialArticles(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetRemainingOtItemsByIdOt_V2034 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2030(Int64 idOt, Int64 idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetPackingWorkOrdersByWarehouse_V2035 instead.")]
        List<Ots> GetPackingWorkOrdersByWarehouse(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
       // [ObsoleteAttribute("This method will be removed after version V2038. Use GetInventoryArticleByIdWarehouseLocation_V2038 instead.")]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2038(Warehouses warehouse, Int64 idWarehouseLocation, WarehouseInventoryAudit warehouseInventoryAudit);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetArticleDetailByIdWarehouseDeliveryNoteItem_V2034 instead.")]
        PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetArticleStockDetailForRefund_V2034 instead.")]
        List<PickingMaterials> GetArticleStockDetailForRefund_V2031(Int64 idOT, Int64 idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use InsertInventoryMaterialIntoArticleStock_V2034 instead.")]
        List<InventoryMaterial> InsertInventoryMaterialIntoArticleStock(List<InventoryMaterial> inventoryMaterials);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetMinimumStockByLocationFullName(Warehouses warehouses, string fullName, Int64 idArticle);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> GetArticleWarehouseLocationByIdArticle(Warehouses warehouse, Int64 idArticle);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetWarehouseDeliveryNoteCode_V2060 instead.")]
        List<ArticlesStock> GetWarehouseDeliveryNoteCode_V2032(Int64 idOTItem, Warehouses warehouse, Int64? idWarehouseProductComponent);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2032(string IdArticles, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetMadeInByIdWareHouseDeliveryNoteItem(Int64 idWareHouseDeliveryNoteItem, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<string> GetReadMeEntriesTitle(Int64 idQuotation, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2034 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddObservation(Observation observation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseDeliveryNoteItem(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> AddArticleWarehouseLocationIFNotExist(List<ArticleWarehouseLocations> articleWarehouseLocations);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetPendingMaterialWorkOrdersByWarehouse_V2035 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2032(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Notification AddNotification(Notification notification);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TimeSpan GetOTTotalWorkingTime(Int64 idOT, byte idStage, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetLabelPrintDetails_V2034 instead.")]
        WarehouseDeliveryNote GetLabelPrintDetails_V2032(Int32 IdArticle, Int64 IdWarehouse, Int64 IdWarehouseDeliveryNote);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetArticleDetailsByReference_V2034 instead.")]
        Article GetArticleDetailsByReference_V2033(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateGeosAppSetting(Int32 idAppSetting, bool isON, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocationTransfer_V2033(string IdArticles, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsOUTSupplierComplaintItem(Int64 IdSupplierComplaint, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateSupplierComplaint(Int64 idSupplierComplaint, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetPlanningSimulator_V2034 instead.")]
        List<PlanningSimulator> GetPlanningSimulator(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetPendingArticles_V2034 instead.")]
        List<Article> GetPendingArticles_V2033(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetPendingMaterialArticles_V2034 instead.")]
        List<OtItem> GetPendingMaterialArticles_V2033(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetOrderInProcessing_V2036 instead.")]
        List<OrderProcessing> GetOrderInProcessing(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetPendingWorkOrders_V2036 instead.")]
        List<OrderPreparation> GetPendingWorkOrders(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<MyWarehouse> GetWarehouseArticleDetails(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetTotalItemsToLocate_V2037 instead.")]
        DashboardInventory GetTotalItemsToLocate(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetTotalItemsToRefill_V2037 instead.")]
        DashboardInventory GetTotalItemsToRefill(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TransferMaterials> GetMaterialDetailsByLocationName_V2034(string locatioName, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetRefillToListByFullNameSortByStock_V2034 instead.")]
        List<LocationRefill> GetRefillToListByFullNameSortByStock(Int64 idWarehouse, string fullName);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetRefillToListSortByStock_V2034 instead.")]
        List<LocationRefill> GetRefillToListSortByStock(Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseDeliveryNote GetLabelPrintDetails_V2034(Int32 IdArticle, Warehouses Warehouses, Int64 IdWarehouseDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetWarehouseLocationBySelectedWarehouse_V2034(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetAllArticlesWithWarehouseLocations_V2041 instead.")]
        List<Article> GetAllArticlesWithWarehouseLocations_V2034(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_V2034(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetRemainingOtItemsByIdOt_V2041 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2034(Int64 idOt, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2035 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2034(Int64 idOt, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PendingStorageArticles> GetArticlesPendingStorage_V2034(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetAllWarehouseLocationById_V2034(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetZoneByIdArticle_V2034(Int64 idArticle, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetIsLeafWarehouseLocations_V2034(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToList_V2034(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        LocationRefill GetArticleWarehouseLocation_V2034(Warehouses warehouse, Int32 idArticle, Int64 position);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TransferMaterials> GetRefillMaterialDetails_V2034(string fromLocationName, string toLocationName, Warehouses warehouse, LocationRefill toLocationRefill = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetWarehouseLocationToPlaceArticle_V2034(Warehouses warehouse, string reference);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToListByFullName_V2034(Warehouses warehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PickingMaterialsSC> GetPickingItemsForSupplierComplaintItemArticlesAndLocation_V2034(string IdArticles, Warehouses warehouse, Int64 IdWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleWarehouseLocations GetAVGStockByIdArticle_V2034(Int32 IdArticle, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetArticleStockDetailForRefund_V2036 instead.")]
        List<PickingMaterials> GetArticleStockDetailForRefund_V2034(Int64 idOT, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PickingMaterials> FillPickingMaterialByIdWarehouseLocation_V2034(Int64 idOT, Warehouses warehouse, Int64 idWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetArticleDetailsByReference_V2036 instead.")]
        Article GetArticleDetailsByReference_V2034(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetArticleDetailByIdWarehouseDeliveryNoteItem_V2041 instead.")]
        PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2034(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2034(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToListSortByStock_V2034(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToListByFullNameSortByStock_V2034(Warehouses warehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PackingBoxType> GetPackingBoxType(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PackingBox AddPackingBox(Warehouses warehouse, PackingBox packingBox);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetRevisionItemPackingWorkOrders_V2039 instead.")]
        List<WOItem> GetRevisionItemPackingWorkOrders(Warehouses warehouse, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetCompanyPackingWorkOrders_V2035 instead.")]
        List<PackingCompany> GetCompanyPackingWorkOrders(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePackingBox(Warehouses warehouse, PackingBox packingBox);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehousesByUserPermission_V2034(Int32 idActiveUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetOTWorkingTimeDetails_V2036 instead.")]
        List<OTWorkingTime> GetOTWorkingTimeDetails(Int64 idOT, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetPlanningSimulator_V2038 instead.")]
        List<PlanningSimulator> GetPlanningSimulator_V2034(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetPendingMaterialArticles_V2038 instead.")]
        List<OtItem> GetPendingMaterialArticles_V2034(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> InsertInventoryMaterialIntoArticleStock_V2034(List<InventoryMaterial> inventoryMaterials);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetPendingArticles_V2038 instead.")]
        List<Article> GetPendingArticles_V2034(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdatePackingBoxInPartnumber_V2035 instead.")]
        bool UpdatePackingBoxInPartnumber(Warehouses warehouse, Int64 idPackingBox, Int64 idPartNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetWorkorderByIdPackingBox_V2036 instead.")]
        List<BoxPrint> GetWorkorderByIdPackingBox(Warehouses warehouse, Int64 idPackingBox);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdateOTItemStatus_V2035 instead.")]
        bool UpdateOTItemStatus(Warehouses warehouse, Int64 idotitem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdateUnPackingBoxInPartnumber_V2035 instead.")]
        bool UpdateUnPackingBoxInPartnumber(Warehouses warehouse, Int64 idPackingBox, Int64 idPartNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdateOTItemStatusToFinished_V2035 instead.")]
        bool UpdateOTItemStatusToFinished(Warehouses warehouse, Int64 idotitem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetWorkOrderByIdOt_V2038 instead.")]
        Ots GetWorkOrderByIdOt_V2035(Int64 idOt, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2041 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2035(Int64 idOt, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetPurchaseOrdersPendingReceptionByWarehouse_V2044 instead.")]
        List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2035(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetPendingMaterialWorkOrdersByWarehouse_V2036 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2035(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPackingWorkOrdersByWarehouse_V2035(Warehouses warehouse);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetPackedItemByIdPackingBox_V2039 instead.")]
        List<WOItem> GetPackedItemByIdPackingBox(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<MyWarehouse> GetWarehouseArticleDetails_V2035(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTItemStatus_V2035(Warehouses warehouse, Int64 idotitem, Int32 idOperator);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTItemStatusToFinished_V2035(Warehouses warehouse, Int64 idotitem, Int32 idOperator);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddCancelledOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateItemStatusAndStage_V2035(Warehouses warehouse, Int64 idOtItem, byte idItemOtStatus, Int32 idOperator);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateRevisionItemComments_V2035(Warehouses warehouse, Int64 idRevisionItem, Int64 idDeliveryNoteItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePackingBoxInPartnumber_V2035(Warehouses warehouse, Int64 idPackingBox, Int64 idotitem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateUnPackingBoxInPartnumber_V2035(Warehouses warehouse, Int64 idPackingBox, Int64 idotitem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PackingCompany> GetCompanyPackingWorkOrders_V2035(Warehouses warehouse, string siteIds);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BoxPrint> GetWorkorderByIdPackingBox_V2036(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetArticleDetailsByReference_V2037 instead.")]
        Article GetArticleDetailsByReference_V2036(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleType> GetArticleTypes(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateGeosAppSettingById(GeosAppSetting geosAppSetting);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTWorkingTime> GetOTWorkingTimeDetails_V2036(Int64 idOT, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateItemStatusAndStageForRefund(Warehouses warehouse, Int64 idOtItem, byte idItemOtStatus, Int32 idOperator);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateRevisionItemCommentsForRefund(Warehouses warehouse, Int64 idRevisionItem, Int64 idDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetPendingMaterialWorkOrdersByWarehouse_V2044 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2036(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetWarehouseArticlesStockByWarehouse_V2038 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse_V2036(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PickingMaterials> GetArticleStockDetailForRefund_V2036(Int64 idOT, Warehouses warehouse);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //Ots GetWorkOrderByIdOt_V2036(Int64 idOt, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetOrderInProcessing_V2036 instead.")]
        List<OrderProcessing> GetOrderInProcessing_V2036(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetPendingWorkOrders_V2037 instead.")]
        List<OrderPreparation> GetPendingWorkOrders_V2036(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetCountOfRefillWithNoStock(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetSelectedArticleImageInBytes(List<Article> articles);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool RemovePackingBox(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DashboardInventory GetTotalItemsToRefill_V2037(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DashboardInventory GetTotalItemsToLocate_V2037(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetArticleDetailsByReference_V2038 instead.")]
        Article GetArticleDetailsByReference_V2037(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateIsClosedInPackingBox(Warehouses warehouse, Int64 idPackingBox, sbyte isClosed);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetArticlesByLocation(Warehouses warehouse, Int64 idFromWarehouseLocation, Int64 idToWarehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OrderPreparation> GetPendingWorkOrders_V2037(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryAudit> GetAllWarehouseInventoryAudits(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseInventoryAudit AddWarehouseInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWarehouseInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryAudit> GetOpenWarehouseInventoryAudits(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdateWarehouseInventoryAuditItems(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetWarehouseArticlesStockByWarehouse_V2039 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse_V2038(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetArticleDetailsByReference_V2041 instead.")]
        Article GetArticleDetailsByReference_V2038(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetPendingMaterialArticles_V2040 instead.")]
        List<OtItem> GetPendingMaterialArticles_V2038(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlanningSimulator> GetPlanningSimulator_V2038(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryAuditItem> GetWarehouseInventoryAuditItemsByInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit warehouseInventoryAudit, WarehouseLocation warehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt_V2038(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetReadyForShippingOTItems_V2040 instead.")]
        List<string> GetReadyForShippingOTItems(Warehouses warehouse, string idOts);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetPendingArticles_V2038(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<Int32, string>> GetCustomersWithOneOrderBox();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<Int32, string>> GetEMDEPSites();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetWarehouseArticlesStockByWarehouse_V2039(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetRevisionItemPackingWorkOrders_V2041 instead.")]
        List<WOItem> GetRevisionItemPackingWorkOrders_V2039(Warehouses warehouse, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetPackedItemByIdPackingBox_V2041 instead.")]
        List<WOItem> GetPackedItemByIdPackingBox_V2039(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetPendingMaterialArticles_V2044 instead.")]
        List<OtItem> GetPendingMaterialArticles_V2040(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetNotPackingButOTDeliveryDateInCurrentWeek(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetReadyForShippingOTItems_V2041 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2040(Warehouses warehouse, string idOts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2042. Use GetReadyForShippingOTItems_V2042 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2041(Warehouses warehouse, string idOts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2042(Warehouses warehouse, string idOts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetAllArticlesWithWarehouseLocations_V2041(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetArticleDetailsByReference_V2044 instead.")]
        Article GetArticleDetailsByReference_V2041(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use UpdateArticleDetails_V2051 instead.")]
        bool UpdateArticleDetails_V2041(Article article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Stage> GetStagesByWarehouseStageIds();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use GetRevisionItemPackingWorkOrders_V2051 instead.")]
        List<WOItem> GetRevisionItemPackingWorkOrders_V2041(Warehouses warehouse, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use GetPackedItemByIdPackingBox_V2051 instead.")]
        List<WOItem> GetPackedItemByIdPackingBox_V2041(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use GetRemainingOtItemsByIdOt_V2051 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2041(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2051 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2041(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use GetArticleDetailByIdWarehouseDeliveryNoteItem_V2051 instead.")]
        PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2041(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetWONOfferAmount(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetAbosleteArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetSleepedArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear, Int64 aritclesleepDays);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OfferDetail> GetSalesByMonth(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseCustomer> GetSalesByCustomer(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryWeek> GetInventoryWeek(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetArticleImageInBytes(string ImagePath);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetPendingMaterialArticles_V2044(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use GetArticleDetailsByReference_V2051 instead.")]
        Article GetArticleDetailsByReference_V2044(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2044(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2044(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WMSOrder> GetOrders(Warehouses warehouse, DateTime accountingYearFrom, DateTime accountingYearTo);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OrderProcessing> GetOrderInProcessing_V2051(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Article GetArticleDetailsByReference_V2051(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateArticleDetails_V2051(Article article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInternalUsePickArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInternalUseRefundArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetRemainingOtItemsByIdOt_V2051(Int64 idOt, Warehouses warehouse);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2051(Int64 idOt, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2051(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WOItem> GetPackedItemByIdPackingBox_V2051(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WOItem> GetRevisionItemPackingWorkOrders_V2051(Warehouses warehouse, Int32 idCompany);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlesStock> GetWarehouseDeliveryNoteCode_V2060(Int64 idOTItem, Warehouses warehouse, Int64? idWarehouseProductComponent);

    }
}
