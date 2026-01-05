using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common.WMS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetArticlesPendingStorage_V2034 instead.")]
        List<PendingStorageArticles> GetArticlesPendingStorage(Int64 IdWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetArticlesWarehouseLocation_V2032 instead.")]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocation(string IdArticles, long IdWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetMaterialDetailsByLocationName_V2034 instead.")]
        List<TransferMaterials> GetMaterialDetailsByLocationName(string locatioName, Int64 idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetWarehouseDeliveryNoteById_V2440 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetAllWarehouseLocationsByIdWarehouse_V2080 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2220. Use AddWarehouseLocation_V2220 instead.")]
        WarehouseLocation AddWarehouseLocation(WarehouseLocation warehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2220. Use UpdateWarehouseLocation_V2220 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2360. Use AddArticleWarehouseLocationByFullName_V2360 instead.")]
        ArticleWarehouseLocations AddArticleWarehouseLocationByFullName(ArticleWarehouseLocations articleWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use IsExistWarehouseLocationFullName_V2080 instead.")]
        bool IsExistWarehouseLocationFullName(string fullName, long idWarehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int16 GetWorkorderStatus(Int64 idOT);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetMaxPosition_V2080 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetInventoryArticleByIdWarehouseLocation_V2090 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetArticlesWarehouseLocation_V2360 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2550. Use AddOTWorkingTime_V2550 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetMaterialDetailsByLocationName_V2080 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetLabelPrintDetails_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2220. Use GetWarehouseLocationsByIdWarehouse_V2220 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2350. Use GetArticlesPendingStorage_V2350 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetRefillToList_V2080 instead.")]
        List<LocationRefill> GetRefillToList_V2034(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetArticleWarehouseLocation_V2090 instead.")]
        LocationRefill GetArticleWarehouseLocation_V2034(Warehouses warehouse, Int32 idArticle, Int64 position);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetWarehouseLocationToPlaceArticle_V2360 instead.")]
        List<WarehouseLocation> GetWarehouseLocationToPlaceArticle_V2034(Warehouses warehouse, string reference);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetRefillToListByFullName_V2080 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2210. Use GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2210 instead.")]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2034(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetRefillToListSortByStock_V2080 instead.")]
        List<LocationRefill> GetRefillToListSortByStock_V2034(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetRefillToListByFullNameSortByStock_V2080 instead.")]
        List<LocationRefill> GetRefillToListByFullNameSortByStock_V2034(Warehouses warehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PackingBoxType> GetPackingBoxType(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2210. Use AddPackingBox_V2210 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2210. Use UpdatePackingBox_V2210 instead.")]
        bool UpdatePackingBox(Warehouses warehouse, PackingBox packingBox);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2220. Use GetAllWarehousesByUserPermission_V2220 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2190. Use InsertInventoryMaterialIntoArticleStock_V2190 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2550. Use AddCancelledOTWorkingTime_V2550 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2210. Use GetCompanyPackingWorkOrders_V2210 instead.")]
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
        [ObsoleteAttribute("This method will be removed. Use GetArticleTypes_V2540 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2550. Use GetOTWorkingTimeDetails_V2550 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetArticleStockDetailForRefund_V2160 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetTotalItemsToRefill_V2300 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetAllWarehouseInventoryAudits_V2090 instead.")]
        List<WarehouseInventoryAudit> GetAllWarehouseInventoryAudits(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseInventoryAudit AddWarehouseInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use UpdateWarehouseInventoryAudit_V2090 instead.")]
        bool UpdateWarehouseInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryAudit> GetOpenWarehouseInventoryAudits(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use AddUpdateWarehouseInventoryAuditItems_V2090 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2190. Use GetPlanningSimulator_V2190 instead.")]
        List<PlanningSimulator> GetPlanningSimulator_V2038(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetWarehouseInventoryAuditItemsByInventoryAudit_V2090 instead.")]
        List<WarehouseInventoryAuditItem> GetWarehouseInventoryAuditItemsByInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit warehouseInventoryAudit, WarehouseLocation warehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetWorkOrderByIdOt_V2200 instead.")]
        Ots GetWorkOrderByIdOt_V2038(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetReadyForShippingOTItems_V2040 instead.")]
        List<string> GetReadyForShippingOTItems(Warehouses warehouse, string idOts);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetPendingArticles_V2300 instead.")]
        List<Article> GetPendingArticles_V2038(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<Int32, string>> GetCustomersWithOneOrderBox();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<Int32, string>> GetEMDEPSites();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetWarehouseArticlesStockByWarehouse_V2180 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetReadyForShippingOTItems_V2080 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetArticleStockAmountInWarehouse_V2420 instead.")]
        double GetArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetAbosleteArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetSleepedArticleStockAmountInWarehouse_V2420 instead.")]
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
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetPendingMaterialArticles_V2330 instead.")]
        List<OtItem> GetPendingMaterialArticles_V2044(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2051. Use GetArticleDetailsByReference_V2051 instead.")]
        Article GetArticleDetailsByReference_V2044(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetPendingMaterialWorkOrdersByWarehouse_V2200 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2044(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetPurchaseOrdersPendingReceptionByWarehouse_V2300 instead.")]
        List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2044(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WMSOrder> GetOrders(Warehouses warehouse, DateTime accountingYearFrom, DateTime accountingYearTo);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OrderProcessing> GetOrderInProcessing_V2051(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetArticleDetailsByReference_V2080 instead.")]
        Article GetArticleDetailsByReference_V2051(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use UpdateArticleDetails_V2080 instead.")]
        bool UpdateArticleDetails_V2051(Article article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInternalUsePickArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInternalUseRefundArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetRemainingOtItemsByIdOt_V2130 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2051(Int64 idOt, Warehouses warehouse);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2130 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2051(Int64 idOt, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetArticleDetailByIdWarehouseDeliveryNoteItem_V2150 instead.")]
        PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2051(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use GetPackedItemByIdPackingBox_V2400 instead.")]
        List<WOItem> GetPackedItemByIdPackingBox_V2051(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2210. Use GetRevisionItemPackingWorkOrders_V2210 instead.")]
        List<WOItem> GetRevisionItemPackingWorkOrders_V2051(Warehouses warehouse, Int32 idCompany);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticlesStock> GetWarehouseDeliveryNoteCode_V2060(Int64 idOTItem, Warehouses warehouse, Int64? idWarehouseProductComponent);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStockForInternalUse(InventoryMaterial inventoryMaterial);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetNotPacking_V2250 instead.")]
        List<Ots> GetNotPacking(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetReadyForShippingOTItems_V2150 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2080(Warehouses warehouse, string idOts);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetArticleDetailsByReference_V2150 instead.")]
        Article GetArticleDetailsByReference_V2080(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2220. Use UpdateArticleDetails_V2220 instead.")]
        bool UpdateArticleDetails_V2080(Article article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetArticleLockedStockByWarehouse(int idArticle, int idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2080. Use GetAllWarehouses_V2080 instead.")]
        List<Warehouses> GetAllWarehouses();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsInUSENoWarehouseLocation(Int64 idWarehouseLocation, Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetAllWarehouseLocationsByIdWarehouse_V2080(Int64 idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToListByFullName_V2080(Warehouses warehouse, string fullName);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistWarehouseLocationFullName_V2080(string fullName, long idWarehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetMaterialDetailsByLocationName_V2150 instead.")]
        List<TransferMaterials> GetMaterialDetailsByLocationName_V2080(string locatioName, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToList_V2080(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 GetMaxPosition_V2080(Int64 idParent, Int64 idWarehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehouses_V2080(Int32 idUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetAllWarehouseInventoryAudits_V2340 instead.")]
        List<WarehouseInventoryAudit> GetAllWarehouseInventoryAudits_V2090(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        LocationRefill GetArticleWarehouseLocation_V2090(Warehouses warehouse, Int32 idArticle, Int64 position);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetInventoryArticleByIdWarehouseLocation_V2200 instead.")]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2090(Warehouses warehouse, Int64 idWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetInventoryArticleByIdWarehouseLocation_V2340 instead.")]
        bool AddUpdateWarehouseInventoryAuditItems_V2090(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryAuditItem> GetWarehouseInventoryAuditItemsByInventoryAudit_V2090(Warehouses warehouse, WarehouseInventoryAudit warehouseInventoryAudit, WarehouseLocation warehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetWarehouseArticlesStockByWarehouse_WithPrices_V2180 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices(Warehouses warehouse, Currency currency);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2130. Use GetPackingWorkOrdersByWarehouse_V2130 instead.")]
        List<Ots> GetPackingWorkOrdersByWarehouse_V2035(Warehouses warehouse);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetPackingWorkOrdersByWarehouse_V2200 instead.")]
        List<Ots> GetPackingWorkOrdersByWarehouse_V2130(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetRemainingOtItemsByIdOt_V2140 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2130(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2140 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2130(Int64 idOt, Warehouses warehouse);




        #region GEOS2-2370 Add Articles Report Screen in WMS like CRM report.
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetArticlesReportDetails_V2130(Int32 idActiveUser, Company company,
            DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles,
            long idWarehouse);
        #endregion



        #region GEOS2-3032 Error when picking is started for a work order which has an assembly Type Item when item's current stock=0 & remaining quantity>0 and anyone child item's current stock>0 & remaining quantity>0

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetRemainingOtItemsByIdOt_V2150 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2140(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2150 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2140(Int64 idOt, Warehouses warehouse);

        #endregion


        #region  GEOS2-2250 Orders Preparation

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetWorkOrdersPreparationReport_V2150 instead.")]
        List<Ots> GetWorkOrdersPreparationReport_V2140(Warehouses warehouse,
            DateTime fromDate, DateTime toDate);



        #endregion

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseAvailability GetAllWarehouseAvailabilityByIdCompany_V2140(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Boolean UpdateWarehouseAvailability_V2140(WarehouseAvailability warehouseAvailability);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetArticleDetailsByReference_V2160 instead.")]
        Article GetArticleDetailsByReference_V2150(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetReadyForShippingOTItems_V2170 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2150(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2150(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);




        #region  GEOS2-3104 Orders Preparation screen loading is taking 5 min

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetWorkOrdersPreparationReport_V2300 instead.")]
        List<Ots> GetWorkOrdersPreparationReport_V2150(Warehouses warehouse,
            DateTime fromDate, DateTime toDate);

        #endregion


        //GEOS2-3098]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TransferMaterials> GetMaterialDetailsByLocationName_V2150(string locatioName, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use InsertIntoArticleStockForTransferMaterial_V2150 instead.")]
        bool InsertIntoArticleStockForTransferMaterial(TransferMaterials transferMaterials);


        //GEOS2-3098
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStockForTransferMaterial_V2150(TransferMaterials transferMaterials);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use InsertIntoArticleStockForLocateMaterial_V2150 instead.")]

        bool InsertIntoArticleStockForLocateMaterial(PendingStorageArticles pendingStorageArticles);





        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        bool InsertIntoArticleStockForLocateMaterial_V2150(PendingStorageArticles pendingStorageArticles);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetRefillToListByFullNameSortByStock_V2150 instead.")]
        List<LocationRefill> GetRefillToListByFullNameSortByStock_V2080(Warehouses warehouse, string fullName);


     
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetRefillToListByFullNameSortByStock_V2360 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2150(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2300 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2150(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetRefillToListSortByStock_V2150 instead.")]
        List<LocationRefill> GetRefillToListSortByStock_V2080(Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToListSortByStock_V2150(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetArticleWarehouseLocation_V2160 instead.")]
        List<TransferMaterials> GetRefillMaterialDetails_V2034(string fromLocationName, string toLocationName, Warehouses warehouse, LocationRefill toLocationRefill = null);


        //GEOS2-3097
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TransferMaterials> GetRefillMaterialDetails_V2160(string fromLocationName, string toLocationName, Warehouses warehouse, LocationRefill toLocationRefill = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetRefillToListByFullNameSortByStock_V2160 instead.")]
        List<LocationRefill> GetRefillToListByFullNameSortByStock_V2150(Warehouses warehouse, string fullName);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LocationRefill> GetRefillToListByFullNameSortByStock_V2160(Warehouses warehouse, string fullName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use GetArticleDetailsByReference_V2230 instead.")]
        Article GetArticleDetailsByReference_V2160(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        
        //[GESO2-1957]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsIdWarehouseLocationPresent(Warehouses warehouse, string IdArticles, PickingMaterials pickingMaterials);


        //[GESO2-1957]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> GetAllPossibleLocationOfArticle(string IdArticles, Warehouses warehouse);


        //[GESO2-1957]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticlesStockForRefund(PickingMaterials pickingMaterials);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetArticleStockDetailForRefund_V2170 instead.")]
        List<PickingMaterials> GetArticleStockDetailForRefund_V2160(Int64 idOT, Warehouses warehouse);





        //[GEOS2-1957]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PickingMaterials> GetArticleStockDetailForRefund_V2170(Int64 idOT, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use InsertIntoArticleStockForRefund_V2170 instead.")]
        bool InsertIntoArticleStock(PickingMaterials pickingMaterials);



        //[GEOS2-1957]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStockForRefund_V2170(PickingMaterials pickingMaterials);
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetWarehouseArticlesStockByWarehouse_V2360 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse_V2180(Warehouses warehouse);
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetWarehouseArticlesStockByWarehouse_WithPrices_V2360 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2180(Warehouses warehouse, Currency currency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> InsertInventoryMaterialIntoArticleStock_V2190(List<InventoryMaterial> inventoryMaterials, string mainComments);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlanningSimulator> GetPlanningSimulator_V2190(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetWorkOrderByIdOt_V2320 instead.")]
        Ots GetWorkOrderByIdOt_V2200(Int64 idOt, Warehouses warehouse);

       
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetPackingWorkOrdersByWarehouse_V2320 instead.")]
        List<Ots> GetPackingWorkOrdersByWarehouse_V2200(Warehouses warehouse);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetAllItemsForSelectedWarehouseInventoryAudit_V2470 instead.")]
        WarehouseInventoryAudit GetAllItemsForSelectedWarehouseInventoryAudit_V2200(Warehouses warehouse, Int64 idWarehouseInventoryAudit);

           [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use GetRevisionItemPackingWorkOrders_V2400 instead.")]
        List<WOItem> GetRevisionItemPackingWorkOrders_V2210(Warehouses warehouse, Int32 idCompany,Int64? ProducerIdCountryGroup);

        //GEOS2-3405
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OriginOfContent> GetOriginOfContentList(Warehouses warehouse);
        //GEOS2-3405
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use AddPackingBox_V2230 instead.")]
        PackingBox AddPackingBox_V2210(Warehouses warehouse, PackingBox packingBox);
        //GEOS2-3405
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use UpdatePackingBox_V2230 instead.")]
        bool UpdatePackingBox_V2210(Warehouses warehouse, PackingBox packingBox);

        //GEOS2-3405
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use GetCompanyPackingWorkOrders_V2230 instead.")]
        List<PackingCompany> GetCompanyPackingWorkOrders_V2210(Warehouses warehouse, string siteIds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BoxPrint> GetWorkorderByIdPackingBox_V2210(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetInventoryArticleByIdWarehouseLocation_V2520 instead.")]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2210(Warehouses warehouse, Int64 idWarehouseLocation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2210(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistArticleWarehouseLocation(Int32 idArticle, Int64 idWarehouseLocation, Warehouses warehouse);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2210. Use GetPendingMaterialWorkOrdersByWarehouse_V2210 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2200(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetPendingMaterialWorkOrdersByWarehouse_V2320 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2210(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use UpdateArticleDetails_V2340 instead.")]
        bool UpdateArticleDetails_V2220(Article article);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetWarehouseLocationsByIdWarehouse_V2520 instead.")]
        List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_V2220(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehousesByUserPermission_V2220(Int32 idActiveUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Manufacturer> GetAllActiveManufacturers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseLocation AddWarehouseLocation_V2220(WarehouseLocation warehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseLocation UpdateWarehouseLocation_V2220(WarehouseLocation warehouseLocation);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetArticleDetailsByReference_V2360 instead.")]
        Article GetArticleDetailsByReference_V2230(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);
        
        //GEOS2-3556
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use AddPackingBox_V2240 instead.")]
        PackingBox AddPackingBox_V2230(Warehouses warehouse, PackingBox packingBox);
        
        //GEOS2-3556
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use UpdatePackingBox_V2240 instead.")]
        bool UpdatePackingBox_V2230(Warehouses warehouse, PackingBox packingBox);

        //GEOS2-3408
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetCompanyPackingWorkOrders_V2240 instead.")]
        List<PackingCompany> GetCompanyPackingWorkOrders_V2230(Warehouses warehouse, string siteIds);
        
        //GEOS2-2988
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use GetSelectedWarehousesArticleStock_V2230 instead.")]
        List<Warehouses> GetSelectedWarehousesArticleStock(Int32 idArticle, List<Warehouses> Lstwarehouses);

        //GEOS2-2988
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetSelectedWarehousesArticleStock_V2300 instead.")]
        List<Warehouses> GetSelectedWarehousesArticleStock_V2230(Int32 idArticle, List<Warehouses> Lstwarehouses);
        
        //GEOS2-3384
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use AddPackingBox_V2260 instead.")]
        PackingBox AddPackingBox_V2240(Warehouses warehouse, PackingBox packingBox);

        //GEOS2-3384
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdatePackingBox_V2240(Warehouses warehouse, PackingBox packingBox);

        //GEOS2-3384
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use AddPackingBox_V2400 instead.")]
        List<PackingCompany> GetCompanyPackingWorkOrders_V2240(Warehouses warehouse, string siteIds);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use GetNotPacking_V2260 instead.")]
        List<Ots> GetNotPacking_V2250(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetStockBySupplier_V2510 instead.")]
        List<StockBySupplier> GetStockBySupplier(Warehouses warehouse);


        //GEOS2-3722
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        PackingBox AddPackingBox_V2260(Warehouses warehouse, PackingBox packingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetNotPacking_V2470 instead.")]
        List<Ots> GetNotPacking_V2260(Warehouses warehouse);

     
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2290. Use GetReadyForShippingOTItems_V2290 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2170(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);


        //GEOS2-3551
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetReadyForShippingOTItems_V2300 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2290(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetReadyForShippingOTItems_V2301 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2300(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetWorkOrdersPreparationReport_V2301 instead.")]
        List<Ots> GetWorkOrdersPreparationReport_V2300(Warehouses warehouse, DateTime fromDate, DateTime toDate);

        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2360 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2300(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetPendingArticles_V2300(Warehouses warehouse);

        // shubham[skadam] GEOS2-3762 No reporta correctamente Check Availability (01IFRM06N13G1L)  09 Aug 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetSelectedWarehousesArticleStock_V2300(Int32 idArticle, List<Warehouses> Lstwarehouses);

        // shubham[skadam] GEOS2-3548 In Pending Articles Change the way how the PO Expected delivery date is calculated when we have PO items with Expected Delivery Dates info  12 Aug 2022 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetPurchaseOrdersPendingReceptionByWarehouse_V2530 instead.")]
        List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2300(Warehouses warehouse);

        //[rdixit][GEOS2-3831][12.08.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DashboardInventory GetTotalItemsToRefill_V2300(Warehouses warehouse);

        // shubham[skadam] GEOS2-3907 Error when trying to send the Email Ready for Shipment  06 Sep 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use GetReadyForShippingOTItems_V2370 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2301(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        //[rdixit][14.09.2022][GEOS2-3902]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetWorkOrdersPreparationReport_V2500 instead.")]
        List<Ots> GetWorkOrdersPreparationReport_V2301(Warehouses warehouse, DateTime fromDate, DateTime toDate);

        // shubham[skadam]  GEOS2-3918 EURO and NO EURO ots have been mixed in a box  15 Sep 2022
        [OperationContract] 
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use WMS_ISExistsEuropeanAndNonEuropeanCountry_V2340 instead.")]
        bool WMS_ISExistsEuropeanAndNonEuropeanCountry(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2320(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetPackingWorkOrdersByWarehouse_V2540 instead.")]
        List<Ots> GetPackingWorkOrdersByWarehouse_V2320(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetAllWorkflowStatus_V2320();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowTransition> GetAllWorkflowTransitions_V2320();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInOT_V2320(Int64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt_V2320(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetPendingMaterialArticles_V2430 instead.")]
        List<OtItem> GetPendingMaterialArticles_V2330(Warehouses warehouse);

        //[Sudhir.jangra][GEOS2-3959][07/11/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleRefillDetail> GetArticleRefillDetails(DateTime fromDate, DateTime toDate, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeleteInventoryAudit(long IdWarehouseInventoryAudit);

        //[sshegaonkar][GEOS2-4041][06/12/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CountryEURO> WMS_ISExistsEuropeanAndNonEuropeanCountry_V2340(Warehouses warehouse, Int64 idPackingBox);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryAuditLocation> GetAllLocationsForSelectedWarehouseInventoryAudit(Int64 idWarehouse, Int64 idWarehouseInventoryAudit);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryAuditArticle> GetAllArticleForSelectedWarehouseInventoryAudit(Int64 idWarehouse, Int64 idWarehouseInventoryAudit);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryAuditArticle> GetAllArticlesByWarehouseLocation(Int64 idWarehouse, Int64 IdWarehouseLocation);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryAuditLocation> GetAllWarehouseLocationsByArticle(Int64 idWarehouse, Int64 idArticle);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleCategory> GetWMSArticlesWithCategoryForReference();
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWarehouseInventoryAudit_V2340(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use AddUpdateWarehouseInventoryAuditItems_V2600 instead.")]
        bool AddUpdateWarehouseInventoryAuditItems_V2340(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetEmployeeCodeByIdUser(Int32 IdUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use UpdateArticleDetails_V2360 instead.")]
        bool UpdateArticleDetails_V2340(Article article,Warehouses warehouse);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetInuseWarehouseLocationsByIdWarehouse(Warehouses warehouse);

        //GEOS2-3606 [Gulab lakade] [Window Service] [19 12 2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use ExpiredArticleWarningsCleanUp_V2480 instead.")]
        bool ExpiredArticleWarningsCleanUp_V2340(DateTime StartDate);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsArticleExistForSelectedWarehouseInventoryAudit(Int32 IdArticle, Int64 idWarehouseInventoryAudit);
        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsLocationExistForSelectedWarehouseInventoryAudit(long IdWarehouse, string WarehouseLocation, Int64 idWarehouseInventoryAudit);

        //[rdixit][09.12.2022][GEOS2-3962]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryAudit> GetAllWarehouseInventoryAudits_V2340(Warehouses warehouse);

        //Shubham[skadam] GEOS2-3532 [QUALITY_INSPECTION] Ignore the Items in TRANSIT without “Product Inspection” OK 29 12 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetArticleDetailsByReference_V2360_New instead.")]
        Article GetArticleDetailsByReference_V2360(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //Shubham[skadam] GEOS2-3532 [QUALITY_INSPECTION] Ignore the Items in TRANSIT without “Product Inspection” OK 29 12 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360 New. Use UpdateArticleDetails_V2360_New instead.")]
        bool UpdateArticleDetails_V2360(Article article, Warehouses warehouse);

        //[pjadhav][02-01-2023][GEOS2-3530] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductInspectionArticles> GetArticlesProductInspection(Warehouses warehouse);

        //[rdixit][10.12.2022][GEOS2-3532]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PendingStorageArticles> GetArticlesPendingStorage_V2350(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseDeliveryNote GetLabelPrintDetails_V2350(Int32 IdArticle, Warehouses Warehouses, Int64 IdWarehouseDeliveryNoteItem);

        //[Sudhir.Jangra][GEOS2-3531][17/01/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetArticlesProductInspection_V2360 instead.")]
        List<EditProductInspectionArticles> GetArticlesProductInspection_V2350(Warehouses warehouse, long IdArticleWarehouseInspection);

        //[Sudhir.Jangra][GEOS2-3531][20/01/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductInspectionImageInfo> GetProductInspectionImageInBytes(Warehouses warehouse, Int32 IdArticle);

        //[rdixit][30.01.2023][GEOS2-3605]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetPendingWorkOrdersByWarehouse_V2360(Warehouses warehouse);

        //[rdixit][06.02.2023][GEOS2-3605]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetRemainingOtItemsByIdOt_V2390 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2360(Int64 idOt, Warehouses warehouse);

        //[rdixit][06.02.2023][GEOS2-3605]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2390 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2360(Int64 idOt, Warehouses warehouse);

        //[rdixit][07.02.2023][GEOS2-4132]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleWarehouseLocations AddArticleWarehouseLocationByFullName_V2360(ArticleWarehouseLocations articleWarehouseLocation);
        //Shubham[skadam] GEOS2-1512 Changes in article attachments to allow download them  07 02 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetArticleDetailsByReference_V2390_New instead.")]
        Article GetArticleDetailsByReference_V2360_New(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //[rdixit][07.02.2023][GEOS2-4134]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetWarehouseArticlesStockByWarehouse_V2360(Warehouses warehouse);

        //[rdixit][07.02.2023][GEOS2-4134]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2360(Warehouses warehouse, Currency currency);

        //[rdixit][08.02.2023][GEOS2-4133]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetWarehouseLocationToPlaceArticle_V2360(Warehouses warehouse, string reference);

        //[rdixit][08.02.2023][GEOS2-4133]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetArticlesWarehouseLocation_V2520 instead.")]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2360(string IdArticles, Warehouses warehouse);

        //[rdixit][08.02.2023][GEOS2-3605]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateArticleDetails_V2360_New(Article article, Warehouses warehouse);

        //[rdixit][GEOS2-4132][08.02.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]      
        List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_Transfer(Warehouses warehouse);

        //Shubham[skadam] GEOS2-3531 [QUALITY_INSPECTION] Add Product Inspection Screen 13 02 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EditProductInspectionArticles> GetArticlesProductInspection_V2360(Warehouses warehouse, long IdArticleWarehouseInspection);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
          bool IsInsertedProductInspection_V2360(Warehouses warehouse, string Comments, long IdArticleWarehouseInspection, List<EditProductInspectionArticles> ProductInspectionArticles, Int64 QuantityInspected);

        //[cpatil][16-03-2023][GEOS2-4209] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductInspectionArticles> GetAllArticlesProductInspection_V2370(Warehouses warehouse);


        //[cpatil][16-03-2023][GEOS2-4148] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2370(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        //[rdixit][17-03-2023][GEOS2-4209] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetArticle_Comment(Warehouses warehouse, int idArticle);

        //Shubham[skadam] GEOS2-3965 Add a NEW option in the Inventory Audit Edit/View screen in order to print a report 24 03 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AuditedArticle> GetAllAuditedArticleForInventoryAudit(Int64 idWarehouseInventoryAudit);

        //[cpatil] [GEOS2-4251][28-03-2023] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStockForLocateMaterialForTransit(PendingStorageArticles pendingStorageArticles);

        //[rdixit][18.05.2023][GEOS2-4411]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetRemainingOtItemsByIdOt_V2390(Int64 idOt, Warehouses warehouse);

        //[rdixit][18.05.2023][GEOS2-4411]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2390(Int64 idOt, Warehouses warehouse);

        //[pramod.misal][GEOS2-4228][15/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseQuota> GetWarehouseQuota_V2390(byte idCurrency);

        //[rdixit][GEOS2-4496][25.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Article GetArticleDetailsByReference_V2390_New(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);


        //[Sudhir.Jangra][GEOS2-4271][24/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseWorklogReport> GetOTWorkLogTimesByPeriodAndSite_V2390(DateTime FromDate, DateTime ToDate, long IdSite, Warehouses warehouse);


        //[Sudhir.Jangra][GEOS2-4271][25/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseWorkLogUser> GetWorkLogUserListByPeriodAndSite_V2390(DateTime FromDate, DateTime ToDate, long IdSite, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4271][25/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseWorklogReport> GetWorkLogOTWithHoursAndUserByPeriodAndSite_V2390(DateTime FromDate, DateTime ToDate, long IdSite, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseInventoryWeek> GetInventoryWeek_V2390(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleMaterialType> GetArticleMaterialTypeStockAmountInWarehouse_V2390(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetReadyForShippingOTItems_V2450 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2390(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        //[pramod.misal][GEOS2-4229][15/05/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWarehouseQuota_V2400(WarehouseQuota selectedAmount);


        //[Sudhir.Jangra][GEOS2-4489][30/05/2023]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehousesByUserPermissionInSRM_V2400(Int32 idActiveUser);

        //[Sudhir.Jangra][GEOS2-4488][30/05/2023]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehousesByUserPermission_V2400(Int32 idActiveUser);

        #region GEOS2-4422&GEOS2-4421
        //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
        //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetRevisionItemPackingWorkOrders_V2530 instead.")]
        List<WOItem> GetRevisionItemPackingWorkOrders_V2400(Warehouses warehouse, Int32 idCompany, Int64? ProducerIdCountryGroup);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetPackedItemByIdPackingBox_V2530 instead.")]
        List<WOItem> GetPackedItemByIdPackingBox_V2400(Warehouses warehouse, Int64 idPackingBox);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetCompanyPackingWorkOrders_V2530 instead.")]
        List<PackingCompany> GetCompanyPackingWorkOrders_V2400(Warehouses warehouse, string siteIds);
        #endregion

        //[Sudhir.Jangra][GEOS2-4435][04/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetArticleDetailsByReference_V2430 instead.")]
        Article GetArticleDetailsByReference_V2420(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //[Sudhir.Jangra][GEOS2-4435][04/08/2023]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use UpdateArticleDetails_V2490 instead.")]
        bool UpdateArticleDetails_V2420(Article article, Warehouses warehouse);

        //Shubham[skadam] GEOS2-4227 New Inventory Dashboard 08 08 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetArticleStockAmountInWarehouse_V2420(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        double GetSleepedArticleStockAmountInWarehouse_V2420(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear, Int64 aritclesleepDays);

        //[Sudhir.Jangra][GEOS2-4539][10/08/2023]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt_V2420(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4539][10/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ItemOTStatusType> GetOtItemStatusList();

        //[Sudhir.Jangra][GEOS2-4540][17/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetRemainingOtItemsByIdOt_V2430 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2420(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4540][17/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2430 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2420(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4541][21/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetPendingMaterialWorkOrdersByWarehouse_V2430 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2420(Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4539][24/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEditWorkOrderOTItems(List<OtItem> otItems);
        //Shubham[skadam] GEOS2-4732 Too many errors of lock wait timeout exceeded in EWHQ (3/3) 28 08 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetArticleDetailsByReference_V2460 instead.")]
        Article GetArticleDetailsByReference_V2430(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //[cpatil] [GEOS2-4557][04-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Offer> GetPendingWorkOrdersByWarehouse_V2430(Warehouses warehouse);

        //[cpatil] [GEOS2-4414][04-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseBulkArticle> GetWarehouseBulkArticle(Warehouses warehouse);

        //[cpatil] [GEOS2-4414][04-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseBulkArticle AddUpdateWarehouseBulkArticle(WarehouseBulkArticle warehouseBulkArticle);

        //[Sudhir.Jangra][GEOS2-4414]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<BulkPicking> GetAllArticles();
        //[Sudhir.Jangra][GEOS2-4414]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseBulkArticle GetAllArticlesByIdBulkPicking(long IdWarehouseBulkPicking);

        //[cpatil][GEOS2-4416][06-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2550. Use GetPendingMaterialArticles_V2550 instead.")]
        List<OtItem> GetPendingMaterialArticles_V2430(Warehouses warehouse);


        //[cpatil][GEOS2-4417][08/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetWorkOrderByIdOt_V2450 instead.")]
        Ots GetWorkOrderByIdOt_V2430(Int64 idOt, Warehouses warehouse);


        //[cpatil][GEOS2-4417][13/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetPendingMaterialWorkOrdersByWarehouse_V2450 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2430(Warehouses warehouse);


        //[cpatil][21/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetRemainingOtItemsByIdOt_V2440 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2430(Int64 idOt, Warehouses warehouse);

        //[cpatil][21/09/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2430 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2430(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOtItemFinishStatus(Int64 idOt, Int64 idWarehouse, OtItem parentOtItem, int keyId);

        //[Sudhir.jangra][GEOS2-4544]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetRemainingOtItemsByIdOt_V2450 instead.")]
        List<OtItem> GetRemainingOtItemsByIdOt_V2440(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-4544]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
     
        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2440(Int64 idOt, Warehouses warehouse);


        //[Sudhir.jangra][GEOS2-4543]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetWarehouseDeliveryNoteById_V2480 instead.")]
        WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2440(Int64 idWarehouseDeliveryNote);

        //[Sudhir.Jangra][GEOS2-4543]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsUpdatedLockPropertyForDeliveryNote(Int64 IdWarehouseDeliveryNoteItem, bool IsLocked);

        //[rdixit][GEOS2-4860][25.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetReadyForShippingOTItems_V2500 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2450(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string ReadMailTemplate(string templateName);
        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use IsEmailSendAfterPickingInventory_V2450 instead.")]
        bool IsEmailSendAfterPickingInventory(string title, string description, string mailTo, string mailFrom, byte[] AttachmentData);


        //[cpatil][GEOS2-4948][28/10/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetPendingMaterialWorkOrdersByWarehouse_V2540 instead.")]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2450(Warehouses warehouse);


        //[cpatil][GEOS2-4930][28/10/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetRemainingOtItemsByIdOt_V2450(Int64 idOt, Warehouses warehouse);

        //[cpatil][GEOS2-4930][28/10/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2450(Int64 idOt, Warehouses warehouse);


        //[Sudhir.Jangra][GEOS2-4859]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2450. Use GetFinancialIncomeOutcome_V2460 instead.")]
        List<ArticlesStock> GetFinancialIncomeOutcome_V2450(Warehouses idWarehouse,byte idCurrency, DateTime fromDate, DateTime toDate);

        //[rajashri][GEOS2-4849][28.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInOT_V2450(Int64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList);

        //[rdixit][GEOS2-4948][31.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetWorkOrderByIdOt_V2460 instead.")]
        Ots GetWorkOrderByIdOt_V2450(Int64 idOt, Warehouses warehouse);

        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsEmailSendAfterPickingInventory_V2450(string title, string emailTemplate, string mailTo, string mailFrom, byte[] AttachmentData);

        //[Sudhir.Jangra][GEOS2-4859]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        
        List<ArticlesStock> GetFinancialIncomeOutcome_V2460(Warehouses idWarehouse, byte idCurrency, DateTime fromDate, DateTime toDate);

        //[rajashri][GEOS2-4966]  28-11-2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2490. Use GetArticleDetailsByReference_V2490 instead.")]
        Article GetArticleDetailsByReference_V2460(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //[rdixit][GEOS2-4948][31.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetWorkOrderByIdOt_V2510 instead.")]
        Ots GetWorkOrderByIdOt_V2460(Int64 idOt, Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateOtItemsStatusToFinish(List<OtItem> ItemsToFinishStatus);

        //[pramod.misal][GEOS2-5093][15-12-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]      
        List<Ots> GetNotPacking_V2470(Warehouses warehouse);

        //[pramod.misal][GEOS2-5094][19-12-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateDeliveryDateofTodoOts_V2470(Dictionary<Int64, DateTime?> UpdatedTodoOts);

        //[pramod.misal][GEOS2-5069][29-12-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseInventoryAudit GetAllItemsForSelectedWarehouseInventoryAudit_V2470(Warehouses warehouse, Int64 idWarehouseInventoryAudit);

        //Shubham[skadam] GEOS2-5226 NO SE PUEDE DESBLOQUEAR UN ALBARÁN DE LA REFERENCIA 02MOTTRONIC 12 01 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetWarehouseDeliveryNoteById_V2510 instead.")]
        WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2480(Int64 idWarehouseDeliveryNote);

        //GEOS2-3606 [Gulab lakade] [Window Service] [19 12 2022]
        //Shubham[skadam] GEOS2-5225 Automatic date not updated properly 01 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ExpiredArticleWarningsCleanUp> ExpiredArticleWarningsCleanUp_V2480(DateTime StartDate);

        //Shubham[skadam] GEOS2-5225 Automatic date not updated properly 01 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CleanUpExpiredArticleWarningsByIdOfferAndIdWarehouse(ExpiredArticleWarningsCleanUp ExpiredArticleWarningsCleanUp);

        //[rajashri][GEOS2-4966]  28-11-2023
        //Shubham[skadam] GEOS2-5016 Changes in Type of Article 20 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetArticleDetailsByReference_V2500 instead.")]
        Article GetArticleDetailsByReference_V2490(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);
        //[Sudhir.Jangra][GEOS2-4435][04/08/2023]
        //Shubham[skadam] GEOS2-5016 Changes in Type of Article 20 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateArticleDetails_V2490(Article article, Warehouses warehouse);



        //[cpatil][GEOS2-5299][27/02/2024]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehousesByUserPermission_V2490(Int32 idActiveUser);


        //[cpatil][GEOS2-5299][27/02/2024]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehousesByUserPermissionInSRM_V2490(Int32 idActiveUser);

        //[GEOS2-5421][rdixit][12.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetReadyForShippingOTItems_V2520 instead.")]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2500(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        //[Sudhir.Jangra][GEOS2-5373]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetWorkOrdersPreparationReport_V2500(Warehouses warehouse, DateTime fromDate, DateTime toDate);

        //[cpatil][GEOS2-4930][28/10/2023]
        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetRemainingOtItemsByIdOt_V2540 instead.")]
        List<WMSItemScan> GetRemainingOtItemsByIdOt_V2500(Int64 idOt, Warehouses warehouse);

        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertIntoArticleStock_V2500(WMSPickingMaterials pickingMaterials);

        //[cpatil][GEOS2-4930][28/10/2023]
        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetRemainingOtItemsByIdOtDisbaledFIFO_V2540 instead.")]
        List<WMSItemScan> GetRemainingOtItemsByIdOtDisbaledFIFO_V2500(Int64 idOt, Warehouses warehouse);

        //[rajashri][GEOS2-4966]  28-11-2023
        //Shubham[skadam] GEOS2-5016 Changes in Type of Article 20 02 2024
        //Shubham[skadam]GEOS2-5344 Sleep day difference 04 04 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Article GetArticleDetailsByReference_V2500(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //rajashri GEOS2-5455 12-04-2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetWorkOrderByIdOt_V2540 instead.")]
        Ots GetWorkOrderByIdOt_V2510(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-5486]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<StockBySupplier> GetStockBySupplier_V2510(Warehouses warehouse, Currency currency);

        //[Sudhir.Jangra][GEOS2-5456]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsLockPropertyForDeliveryNoteUpdated_V2510(List<WarehouseDeliveryNoteItem> deliveryNotes);

        //rajashri GEOS2-5433
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetArticleDetailsByReference_V2630 instead.")]
        Article GetArticleDetailsByReference_V2510(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //rajashri GEOS2-5433
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use UpdateArticleDetails_V2530 instead.")]
        bool UpdateArticleDetails_V2510(Article article, Warehouses warehouse);


        //[Sudhir.jangra][GEOS2-5457]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2510(Int64 idWarehouseDeliveryNote);

        //rajashri GEOS2-5434
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetWarehouseArticlesStockByWarehouse_WithPrices_V2540 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2510(Warehouses warehouse, Currency currency);

        //rajashri GEOS2-5434
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetWarehouseArticlesStockByWarehouse_V2540 instead.")]
        List<Article> GetWarehouseArticlesStockByWarehouse_V2510(Warehouses warehouse);

        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 06 05 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Tuple<int, Int64> GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(int idArticle, int idWarehouse);

        //rajashri GEOS2-5487
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2520(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL);

        //[rushikesh.gaikwad] [GEOS2-5488] [16.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_V2520(Warehouses warehouse);

        //[rushikesh.gaikwad] [GEOS2-5488] [16.05.2024]
        
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryAuditArticle> GetAllArticlesByWarehouseLocation_V2520(Int64 idWarehouse, Int64 IdWarehouseLocation);

        //[pramod.misal][GEOS2-5524][17.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetArticlesWarehouseLocation_V2580 instead.")]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2520(string IdArticles, Warehouses warehouse, Int64 qty);

        //[cpatil][GEOS2-5513][17.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetMandatoryStageNameOpenIfExist(Int64 partNumber, Warehouses warehouse);

        //[cpatil][GEOS2-5548][23.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2520(Warehouses warehouse, Int64 idWarehouseLocation);

        //[Sudhir.Jangra][GEOS2-5644]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteWorkLog_V2530(Company company, Int64 idOTWorkingTime);

        //[Sudhir.Jangra][GEOS2-5644]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkLog(Company company, OTWorkingTime otWorkingTime);

        //[Sudhir.Jangra][GEOS2-5644]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTAssignedUser> GetOTAssignedUsers(Company company, Int64 idOT);

        #region GEOS2-4422&GEOS2-4421&GEOS2-5784
        //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
        //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
        //Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WOItem> GetRevisionItemPackingWorkOrders_V2530(Warehouses warehouse, Int32 idCompany, Int64? ProducerIdCountryGroup);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WOItem> GetPackedItemByIdPackingBox_V2530(Warehouses warehouse, Int64 idPackingBox);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PackingCompany> GetCompanyPackingWorkOrders_V2530(Warehouses warehouse, string siteIds, string workOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WOItem> GetPackingWorkOrders_V2530(Warehouses warehouse, string workOrder);
        #endregion

        // Rahul[rgadhave] GEOS2-5865 Not show PO which having type offer.(1732)[21/06/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2530(Warehouses warehouse);

        //rajashri GEOS2-5433
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateArticleDetails_V2530(Article article, Warehouses warehouse);

        //[pramod.misal][GEOS2-5904][02.07.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetWorkOrdersPreparationReport_V2540(Warehouses warehouse, DateTime fromDate, DateTime toDate);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2540(Warehouses warehouse, Currency currency);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WMSItemScan> GetRemainingOtItemsByIdOt_V2540(Int64 idOt, Warehouses warehouse);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2550. Use GetWorkOrderByIdOt_V2550 instead.")]
        Ots GetWorkOrderByIdOt_V2540(Int64 idOt, Warehouses warehouse);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetWarehouseArticlesStockByWarehouse_V2540(Warehouses warehouse);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2540(Warehouses warehouse);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WMSItemScan> GetRemainingOtItemsByIdOtDisbaledFIFO_V2540(Int64 idOt, Warehouses warehouse);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleType> GetArticleTypes_V2540(Warehouses warehouse);
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Ots> GetPackingWorkOrdersByWarehouse_V2540(Warehouses warehouse);

        //[cpatil][GEOS2-4416][06-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OtItem> GetPendingMaterialArticles_V2550(Warehouses warehouse);


        //[Sudhir.jangra][GEOS2-6050]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Ots GetWorkOrderByIdOt_V2550(Int64 idOt, Warehouses warehouse);

        //[Sudhir.Jangra][GEOS2-6050]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OTWorkingTime> GetOTWorkingTimeDetails_V2550(Int64 idOT, Warehouses warehouse);

        //[cpatil][GEOS2-5864]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddOTWorkingTime_V2550(Warehouses warehouse, OTWorkingTime otWorkingTime);

        //[cpatil][GEOS2-5864]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddCancelledOTWorkingTime_V2550(Warehouses warehouse, OTWorkingTime otWorkingTime);

        //[GEOS2-5906][rdixit][13.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductInspectionReworkCauses> GetReworkCausesbyArticleCategory(Int64 IdArticleCategory);

        //[GEOS2-5906][rdixit][13.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProductInspectionReworkCauses> GetAllReworkCauses();

        //[GEOS2-5906][rdixit][13.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeletedReworkCauseForArticleCategory_V2580(ProductInspectionReworkCauses rework);

        //[GEOS2-5906][rdixit][13.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdateReworkListByArticleCategory_V2580(List<ProductInspectionReworkCauses> reworkListByArticleCategory);

        //[GEOS2-5906][rdixit][13.11.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProductInspectionReworkCauses AddEditNewReworkCause_V2580(ProductInspectionReworkCauses newReworkCause);

        //[rdixit][08.02.2023][GEOS2-4133]
        //Shubham[skadam] GEOS2-5992 Improvements in the filling criteria developed in Ticket IESD-96777 22 11 2024.
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2580(string IdArticles, Warehouses warehouse);

        //[pramod.misal][GEOS2-5524][17.05.2024]
        //Shubham[skadam] GEOS2-5992 Improvements in the filling criteria developed in Ticket IESD-96777 22 11 2024.
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2580_New(string IdArticles, Warehouses warehouse, Int64 qty);

        //[nsatpute][12-12-2024][GEOS2-6382] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWarehouseInventoryAudit_V2590(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit);

        //[nsatpute][12-12-2024][GEOS2-6382] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<InventoryAuditArticle> GetAllArticlesByWarehouseLocations(Int64 idWarehouse, string IdWarehouseLocation);

        //[nsatpute][16-12-2024][GEOS2-6382] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdateWarehouseInventoryAuditItems_V2590(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems);

        //[nsatpute][01-04-2025][GEOS2-7015]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetLocalWarehouseStockReport_V2670 instead.")]
        LocalWarehouseStock GetLocalWarehouseStockReport(long idWarehouse, bool isRegional);

        //[nsatpute][01-04-2025][GEOS2-7015]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use WMS_GetWarehouseStockReportEmails_V2660 instead.")]
        Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails(long idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<SendNotificationMail> GetPikingNotification_V2630(Warehouses warehouse);


        // [pallavi jadhav][GEOS2-7022][10 - 04 - 2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SendNotificationMail(string warehouse, string mailTo, string itemReference, string expectedQty, string actualQty, string picker);

        //[Sudhir.Jangra][GEOS2-6490]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Article GetArticleDetailsByReference_V2630(string reference, Warehouses idWarehouse, DateTime? fromDate = null, DateTime? toDate = null);

        //[nsatpute][01-04-2025][GEOS2-7015]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetWarehousesForLocalStockReport();

        //[nsatpute][15-04-2025][GEOS2-7016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetWarehouseStockForGlobalReport_V2670 instead.")]
        LocalWarehouseChartData GetWarehouseStockForGlobalReport(long idWarehouse, bool isRegional);

        //[nsatpute][15-04-2025][GEOS2-7016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetWarehousesForGlobalStockReport();

        //[nsatpute][17-04-2025][GEOS2-7016]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2660. Use WMS_GetGlobalWarehouseStockReportEmails instead.")]
        Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails();

        #region[rani dhamankar][16-04-2025][GEOS2-7021]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ItemPickerDetails GetPickerNameBYDeliveryNoteItem_V2630(long IdWarehouseDeliveryNoteItem, Warehouses warehouse);
        #endregion

        #region [pallavi.jadhav][9 5 2025][GEOS2-6823]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseLocation AddWarehouseLocation_V2640(WarehouseLocation warehouseLocation,List<RangeItems> RangeOfLocationResult);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehouseLocation UpdateWarehouseLocation_V2640(WarehouseLocation warehouseLocation, List<RangeItems> RangeOfLocationResult);

        #endregion

        //[nsatpute][11.07.2025][GEOS2-8837] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use WMS_GetWarehouseStockReportEmails_V2670 instead.")]
        Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails_V2660(long idCompany);

        //[nsatpute][11.07.2025][GEOS2-8837] 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use WMS_GetGlobalWarehouseStockReportEmails_V2670 instead.")]
        Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails_V2660();

        //[nsatpute][29.08.2025][GEOS2-6505]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Article> GetArticlesStockByWarehouse_V2670(Warehouses warehouse, Currency currency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateArticleMinMaxStock_V2670(Warehouses warehouse, List<Article> ModifiedArticles);

		//[nsatpute][05.09.2025][GEOS2-9210]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        LocalWarehouseStock GetLocalWarehouseStockReport_V2670(long idWarehouse);

        //[nsatpute][05.09.2025][GEOS2-9210]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        LocalWarehouseChartData GetWarehouseStockForGlobalReport_V2670(long idWarehouse);

        // [nsatpute][12.09.2025][GEOS2-9181]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails_V2670();

        // [nsatpute][12.09.2025][GEOS2-9181]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use WMS_GetWarehouseStockReportEmails_V2680 instead.")]
        Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails_V2670(long idCompany);

        // [cpatil][15.09.2025][GEOS2-8200]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<RegionCountryWarehouse> GetRegionCountrySiteWarehouseData(Int32 idUser);

		// [nsatpute][18.09.2025][GEOS2-9210]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertLocalWarehouseStock(long idWarehouse,int monthNumber);

        //[nsatpute][22.09.2025][GEOS2-8793]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<long> GetWarehouseScheduleYears();

        //[nsatpute][22.09.2025][GEOS2-8795]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SaveWarehouseScheduleEvent(ScheduleEvent scheduleEvent);

        //[nsatpute][25.09.2025][GEOS2-8797]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ScheduleEvent> GetWarehouseScheduleEventsByIdWarehouse(long idWarehouse, long year);
		//[nsatpute][16.10.2025][GEOS2-8799]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWarehouseScheduleEvent(ScheduleEvent scheduleEvent);
		//[nsatpute][16.10.2025][GEOS2-8799]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteWarehouseScheduleEvent(ScheduleEvent scheduleEvent);

        //[nsatpute][04.11.2025][GEOS2-8805]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CreatePreordersForWarehouse(Warehouses warehouse);

        //[nsatpute][04.11.2025][GEOS2-8805]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Warehouses> GetAllWarehouseForScheduleEvents();

        //[nsatpute][07-11-2025][GEOS2-10097]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails_V2680(Warehouses warehouse);  
        
        //[nsatpute][07-11-2025][GEOS2-10097]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails_V2680(Warehouses warehouse);

        //[nsatpute][11-11-2025][GEOS2-8805]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<string> GetJobFailureNotificationEmails(string key);

        //[rdixit][GEOS2-9593][13.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WarehousePurchaseOrder> GetPurchaseOrdersByRegionalWarehouse_V2680(long idWarehouse);

        //[rdixit][GEOS2-9593][13.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GeosAppSetting GetGeosAppSettings(Int16 IdAppSetting);

        //[rdixit][GEOS2-9593][13.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetNextNumberOfOfferFromCounters(string siteCode, int type);

        //[rdixit][GEOS2-9593][13.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 InsertOffer_V2680(Quotation quotation, CustomerPurchaseOrder custPurOrder);

        //[rdixit][GEOS2-9593][13.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckIfOfferExists(CustomerPurchaseOrder custPurOrder);

        //[rdixit][GEOS2-9593][13.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateNextOfferNumberToCounters(int idOfferType);

        //[rdixit][GEOS2-9593][13.11.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TransportFrequency> GetSiteTransportFrequencies();

        //[nsatpute][19.11.2025][GEOS2-9371]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveSiteTransportFrequencies(List<TransportFrequency> transportFrequencies);

        //[nsatpute][21.11.2025][GEOS2-9367]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByTransportFrequency> GetLogEntriesByTransportFrequency();

        //[nsatpute][21.11.2025][GEOS2-9367]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveLogEntrieByTransportFrequency(List<LogEntriesByTransportFrequency> logEntries);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckCustomerPurchasOrderExists(string POCode);
    }
}
