using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WarehouseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WarehouseService.svc or WarehouseService.svc.cs at the Solution Explorer and start debugging.

    /// <summary>
    /// This Service is used for all warehouse service functions.
    /// </summary>
    public class WarehouseService : IWarehouseService
    {
        WarehouseManager mgr = new WarehouseManager();

        /// <summary>
        /// [WAREHOUSE-M026-06] Add pending po section.
        /// </summary>
        /// <returns>The list of wpending purchase orders</returns>
        public List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReception()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPurchaseOrdersPendingReception(connectionString);
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
        /// Load PO pending reception by warehouse.
        /// </summary>
        /// <param name="warehouseIds">The warehouse ids.</param>
        /// <param name="warehouse">The Warehouse which contains connection string.</param>
        /// <returns>The list of PO.</returns>
        public List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse(string warehouseIds, Warehouses warehouse)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPurchaseOrdersPendingReceptionByWarehouse(warehouseIds, warehouse);
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
        /// PO information by IdWarehousePurchaseOrder.
        /// </summary>
        /// <param name="idWarehousePurchaseOrder">The idWarehousePurchaseOrder.</param>
        /// <param name="warehouse">The warehouse which contains connection string of plant.</param>
        /// <returns></returns>
        public WarehousePurchaseOrder GetPurchaseOrderPendingReceptionByIdWarehousePurchaseOrder(Int64 idWarehousePurchaseOrder, Warehouses warehouse = null)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                if (warehouse != null)
                {
                    connectionString = warehouse.Company.ConnectPlantConstr;
                }

                return mgr.GetPurchaseOrderPendingReceptionByIdWarehousePurchaseOrder(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath);
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
        /// [WAREHOUSE-M027-02] Add Pending Work Orders Section.
        /// Get templates list by idTemplateType
        /// </summary>
        /// <param name="idTemplateType"></param>
        /// <returns>The list of templates.</returns>
        public List<Template> GetTemplatesByIdTemplateType(Int64 idTemplateType)
        {
            try
            {
                //WarehouseManager mgr = new WarehouseManager();
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
        /// [WAREHOUSE-M027-02] Add Pending Work Orders Section
        /// Get pending material work orders.
        /// </summary>
        /// <returns>The of pending material work orders.</returns>
        public List<Ots> GetPendingMaterialWorkOrders()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPendingMaterialWorkOrders(connectionString);
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
        /// [WAREHOUSE-M028-07] Load info by warehouse - Load WO by warehouse.
        /// </summary>
        /// <param name="warehouseIds">The warehouse ids.</param>
        /// <param name="warehouse">The Warehouse with connection string.</param>
        /// <returns>The list of work orders.</returns>
        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse(string warehouseIds, Warehouses warehouse)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPendingMaterialWorkOrdersByWarehouse(warehouseIds, warehouse);
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
        /// We will display all quotations of type "SITES" where the offer is "FORECASTED".
        /// We wil take the date from the "CloseDate" of the offer, the same date we are taking for CRM offers.
        /// </summary>
        /// <param name="idActiveUser"></param>
        /// <param name="idCurrency"></param>
        /// <param name="accountingYearFrom"></param>
        /// <param name="accountingYearTo"></param>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        public List<Quotation> GetAllQuotationsOfTypeSites(Int32 idActiveUser, byte idCurrency, DateTime? accountingYearFrom, DateTime? accountingYearTo, Warehouses warehouse)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllQuotationsOfTypeSites(idActiveUser, idCurrency, accountingYearFrom, accountingYearTo, warehouse);
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
        /// [WAREHOUSE-M027-04] Add the Work Order section information
        /// </summary>
        /// <param name="idOt">The idot.</param>
        /// <param name="idWarehouse">The connected plant id.</param>
        /// <param name="warehouse">The warehouse contains connection string.</param>
        /// <returns>The OT</returns>
        public Ots GetWorkOrderByIdOt(Int64 idOt, Int32 idWarehouse, Warehouses warehouse = null)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                //This code to make service compatible with previous version.
                if (warehouse == null)
                {
                    warehouse = new Warehouses();
                    warehouse.IdWarehouse = idWarehouse;
                    warehouse.Company = new Company();
                    warehouse.Company.ConnectPlantConstr = connectionString;
                }

                return mgr.GetWorkOrderByIdOt(idOt, warehouse);
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
        /// Get all pending articles. - [WAREHOUSE-M028-05] Add Pending articles section
        /// </summary>
        /// <returns>The list of articles.</returns>
        public List<Article> GetPendingArticles(Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPendingArticles(connectionString, idWarehouse);
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
        /// Load warehouses by user permission.
        /// </summary>
        /// <param name="idActiveUser">The active user of workbench.</param>
        /// <returns>The list of warehouses.</returns>
        public List<Warehouses> GetAllWarehousesByUserPermission(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermission(connectionString, idActiveUser);
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
        /// This method is used to get delivery note pdf in bytes to show in pdf viewer.
        /// </summary>
        /// <param name="warehouseDeliveryNote">The Warehouse delivery note.</param>
        /// <returns></returns>
        public byte[] GetDeliveryNotePdf(WarehouseDeliveryNote warehouseDeliveryNote)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetDeliveryNotePdf(warehouseDeliveryNote);
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
        /// This method is used to load all warehouse articles stock by warehouse.
        /// </summary>
        /// <param name="warehouseIds">The warehouse ids</param>
        /// <param name="warehouse">The warehouse object with connection string.</param>
        /// <returns></returns>
        public List<Article> GetWarehouseArticlesStockByWarehouse(string warehouseIds, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                if (warehouse == null)
                {
                    warehouse = new Warehouses();
                    warehouse.Company = new Company();
                    warehouse.Company.ConnectPlantConstr = connectionString;
                }

                return mgr.GetWarehouseArticlesStockByWarehouse(warehouseIds, warehouse);
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
        /// [WMS-M029-09] Add Incoming Material section.
        /// Generate Delivery Note - Get all pending items and its Qty and remaining MaxQty.
        /// </summary>
        /// <param name="warehousePurchaseOrder">The WarehousePurchaseOrder</param>
        /// <returns>The WarehouseDeliveryNote.</returns>
        public WarehouseDeliveryNote GenerateDeliveryNote(WarehousePurchaseOrder warehousePurchaseOrder)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GenerateDeliveryNote(connectionString, Properties.Settings.Default.ArticleVisualAidsPath, warehousePurchaseOrder);
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
        /// This method is used to Add Warehouse Delivery Note.
        /// </summary>
        /// <param name="warehouseDeliveryNote"></param>
        /// <returns>The WarehouseDeliveryNote.</returns>
        public WarehouseDeliveryNote AddWarehouseDeliveryNote(WarehouseDeliveryNote warehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddWarehouseDeliveryNote(connectionString, Properties.Settings.Default.PurchaseOrdersPath, warehouseDeliveryNote);
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
        /// All countries list.
        /// </summary>
        /// <returns>The list of countries.</returns>
        public List<Country> GetAllCountries()
        {
            List<Country> countries = null;

            try
            {
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
        /// This method is used to get all serial numbers to avoid Serial Numbers.
        /// </summary>
        /// <returns>The list serial numbers.</returns>
        public List<SerialNumber> GetAllSerialNumbers()
        {
            List<SerialNumber> serialNumbers = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetAllSerialNumbers(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }

        /// <summary>
        /// This method is to get workorder details related to idot
        /// </summary>
        /// <param name="idOt">Get idot</param>
        /// <param name="Warehouse">Get warehouse details</param>
        /// <returns>OTs details</returns>
        public Ots GetWorkOrderByIdOt(Int64 idOt, Warehouses Warehouse)
        {
            Ots ots = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ots = mgr.GetWorkOrderByIdOt(idOt, Warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ots;
        }

        /// <summary>
        /// Thgis method is to get remaining otitmes related to idot
        /// </summary>
        /// <param name="idOt">Get idot</param>
        /// <param name="idWarehouse">Get idwarehouse</param>
        /// <returns>List of otitem details</returns>
        public List<OtItem> GetRemainingOtItemsByIdOt(Int64 idOt, Int64 idWarehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt(connectionString, idOt, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }

        /// <summary>
        /// Method for insert negative quantity of  article stock, after scan for piking material. 
        /// </summary>
        /// <param name="pickingMaterials"></param>
        /// <returns></returns>
        public bool InsertIntoArticleStock(PickingMaterials pickingMaterials)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStock(connectionString, pickingMaterials);
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
        /// This method is to get list of article in pending storage
        /// </summary>
        /// <param name="idWarehouse">Get idwarehouse</param>
        /// <returns>List of article in pending storage</returns>
        public List<PendingStorageArticles> GetArticlesPendingStorage(Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesPendingStorage(connectionString, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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
        /// This method is to get list of articles warehouse location details
        /// </summary>
        /// <param name="IdArticles">Get id articles</param>
        /// <param name="IdWarehouse">Get id warehouse</param>
        /// <returns>List of articles warehouse location details</returns>
        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocation(string IdArticles, long IdWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesWarehouseLocation(connectionString, IdArticles, IdWarehouse);
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
        /// Method for insert negative quantity of  article stock, after scan for piking material. 
        /// </summary>
        /// <param name="pendingStorageArticles"></param>
        /// <returns></returns>
        public bool InsertIntoArticleStockForLocateMaterial(PendingStorageArticles pendingStorageArticles)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockForLocateMaterial(connectionString, pendingStorageArticles);
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
        /// Method for get material details by location.
        /// </summary>
        /// <param name="locatioName"></param>
        /// <param name="idWarehouse"></param>
        /// <returns>List of transfer materials details</returns>
        public List<TransferMaterials> GetMaterialDetailsByLocationName(string locatioName, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMaterialDetailsByLocationName(connectionString, locatioName, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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
        /// Method for insert Transfer details.
        /// </summary>
        /// <param name="transferMaterials"></param>
        /// <returns>Is added or not</returns>
        public bool InsertIntoArticleStockForTransferMaterial(TransferMaterials transferMaterials)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockForTransferMaterial(connectionString, transferMaterials);
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
        /// This method is used to get warehouse delivery note details.
        /// </summary>
        /// <param name="idWarehouseDeliveryNote">The id warehouse delivery note.</param>
        /// <returns>The Warehouse Delivery Note.</returns>
        public WarehouseDeliveryNote GetWarehouseDeliveryNoteById(Int64 idWarehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseDeliveryNoteById(connectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.PurchaseOrdersPath, idWarehouseDeliveryNote);
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
        /// This method is to update warehouse delivery note item details
        /// </summary>
        /// <param name="warehouseDeliveryNoteItem">Get warehouse delivery not items details to update</param>
        /// <returns>Is updated or not</returns>
        public bool UpdateWarehouseDeliveryNoteItem(WarehouseDeliveryNoteItem warehouseDeliveryNoteItem)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.UpdateWarehouseDeliveryNoteItem(connectionString, warehouseDeliveryNoteItem);
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
        /// This method is to save warehouse delivery note pdf
        /// </summary>
        /// <param name="warehouseDeliveryNote">Get warehouse delivery note details</param>
        /// <returns>Is saved or not</returns>
        public bool SaveWarehouseDeliveryNotePdf(WarehouseDeliveryNote warehouseDeliveryNote)
        {
            try
            {
                return mgr.SaveWarehouseDeliveryNote(Properties.Settings.Default.PurchaseOrdersPath, warehouseDeliveryNote);
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
        /// This method is to get article stock details related to warehouse
        /// </summary>
        /// <param name="idArticle">Get id article</param>
        /// <param name="idWarehouse">Get id warehouse</param>
        /// <returns>Article stock</returns>
        public Int32 GetArticleStockByWarehouse(int idArticle, int idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockByWarehouse(connectionString, idArticle, idWarehouse);
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
        /// This method is to get List of warehouse location related to idwarehouse
        /// </summary>
        /// <param name="idWarehouse">Get id warehouse</param>
        /// <returns>List of warehouse location related to idwarehouse</returns>
        public List<WarehouseLocation> GetAllWarehouseLocationById(long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehouseLocationById(idWarehouse, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public MyWarehouse GetWarehouseStockDetailsByArticleAndWarehouse(Int32 idArticle, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseStockDetailsByArticleAndWarehouse(connectionString, idArticle, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public string GetZoneByIdArticle(Int64 idArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetZoneByIdArticle(connectionString, idArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WarehousePurchaseOrder GetWarehousePODetailsByCode(string code)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehousePODetailsByCode(connectionString, code);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticleCategory> GetArticleCategories()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                CrmManager crmManager = new CrmManager();
                return crmManager.GetArticleCategories(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference(string reference, Int64 idWarehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleDetailsByReference(connectionString, reference, Properties.Settings.Default.ArticleVisualAidsPath, idWarehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseLocation> GetAllWarehouseLocationsByIdWarehouse(Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehouseLocationsByIdWarehouse(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticleWarehouseLocations> AddArticleWarehouseLocation(List<ArticleWarehouseLocations> articleWarehouseLocations)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddArticleWarehouseLocation(connectionString, articleWarehouseLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetAllArticlesWithWarehouseLocations(string idWarehouses, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllArticlesWithWarehouseLocations(connectionString, idWarehouses, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseLocation> GetAllWarehouseLocations()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehouseLocations(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse(string warehouseIds)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseLocationsByIdWarehouse(connectionString, warehouseIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateArticleDetails(Article article)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateItemStatusAndStage(Int64 idOtItem, byte idItemOtStatus, Int32 idOperator)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateItemStatusAndStage(connectionString, idOtItem, idItemOtStatus, idOperator);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WarehouseLocation AddWarehouseLocation(WarehouseLocation warehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWarehouseLocation(connectionString, warehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public WarehouseLocation UpdateWarehouseLocation(WarehouseLocation warehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWarehouseLocation(connectionString, warehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsExistWarehouseLocationName(string name, Int64 parent, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsExistWarehouseLocationName(connectionString, name, parent, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateRevisionItemComments(Int64 idRevisionItem, Int64 idDeliveryNoteItem)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateRevisionItemComments(connectionString, idRevisionItem, idDeliveryNoteItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WarehouseLocation> GetWarehouseLocationBySelectedWarehouse(Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseLocationBySelectedWarehouse(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseLocation> GetIsLeafWarehouseLocations(string warehouseIds)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetIsLeafWarehouseLocations(connectionString, warehouseIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateSerialNumber(SerialNumber serialNumber)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateSerialNumber(connectionString, serialNumber);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LocationRefill> GetRefillToList(string idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRefillToList(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public LocationRefill GetArticleWarehouseLocation(Int32 idArticle, Int64 position)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleWarehouseLocation(connectionString, idArticle, position);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocationTransfer(string IdArticles, long IdWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesWarehouseLocationTransfer(connectionString, IdArticles, IdWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<TransferMaterials> GetRefillMaterialDetails(string fromLocationName, string toLocationName, Int64 idWarehouse, LocationRefill toLocationRefill = null)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRefillMaterialDetails(connectionString, fromLocationName, toLocationName, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath, toLocationRefill);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public ArticleWarehouseLocations AddArticleWarehouseLocationByFullName(ArticleWarehouseLocations articleWarehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddArticleWarehouseLocationByFullName(connectionString, articleWarehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool IsExistWarehouseLocationFullName(string fullName, long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsExistWarehouseLocationFullName(fullName, idWarehouse, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Int16 GetWorkorderStatus(Int64 idOT)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkorderStatus(idOT, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Int64 GetMaxPosition(Int64 idParent, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMaxPosition(idParent, idWarehouse, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WarehouseLocation> GetWarehouseLocationToPlaceArticle(Int64 idWarehouse, string reference)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseLocationToPlaceArticle(connectionString, idWarehouse, reference);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public WarehouseLocation GetWarehouseLocationByFullName(Int64 idWarehouse, string fullName)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseLocationByFullName(connectionString, idWarehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Int64 GetIdOtByBarcode(string barcode)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetIdOtByBarcode(connectionString, barcode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Article> GetAllArticlesByWLFullName(string warehouseIds, Warehouses warehouse, string fullName)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllArticlesByWLFullName(connectionString, warehouseIds, warehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LocationRefill> GetRefillToListByFullName(string idWarehouse, string fullName)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRefillToListByFullName(connectionString, idWarehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<SupplierComplaint> GetSupplierComplaints(string idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetSupplierComplaints(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public SupplierComplaint GetSupplierComplaintDetails(Int64 idSupplierComplaint, Warehouses warehouse)
        {
            try
            {
                return mgr.GetSupplierComplaintDetails(idSupplierComplaint, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<SupplierComplaintItem> GetRemainingSCItemsByIdSC(Int64 idSupplierComplaint, Int64 idWarehouse)
        {
            List<SupplierComplaintItem> supplierComplaintItems = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                supplierComplaintItems = mgr.GetRemainingSCItemsByIdSC(connectionString, idSupplierComplaint, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return supplierComplaintItems;
        }

        public bool InsertIntoArticleStockSC(PickingMaterialsSC pickingMaterialsSc)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockSC(connectionString, pickingMaterialsSc);
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
        /// 
        /// </summary>
        /// <param name="IdArticles"></param>
        /// <param name="IdWarehouse"></param>
        /// <returns></returns>
        public List<WarehouseLocation> GetWarehouseLocationsByIdArticles(string IdArticles, Int64 IdWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseLocationsByIdArticles(connectionString, IdArticles, IdWarehouse);
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
        /// 
        /// </summary>
        /// <param name="IdArticles"></param>
        /// <param name="IdWarehouse"></param>
        /// <param name="IdWarehouseLocation"></param>
        /// <returns></returns>
        public List<PickingMaterialsSC> GetPickingItemsForSupplierComplaintItemArticlesAndLocation(string IdArticles, Int64 IdWarehouse, Int64 IdWarehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPickingItemsForSupplierComplaintItemArticlesAndLocation(connectionString, IdArticles, IdWarehouse, IdWarehouseLocation);
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
        /// This method is used to get warehouse location stock of an article.
        /// </summary>
        /// <param name="IdArticle">The idArticle.</param>
        /// <param name="IdWarehouseLocation">The id warehouse location.</param>
        /// <param name="IdWarehouse">The idwarehouse</param>
        /// <returns>The Article warehouse location</returns>
        public ArticleWarehouseLocations GetArticleStockByWarehouseLocation(Int32 IdArticle, Int64 IdWarehouseLocation, Int64 IdWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockByWarehouseLocation(connectionString, IdArticle, IdWarehouseLocation, IdWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public ArticleWarehouseLocations GetAVGStockByIdArticle(Int32 IdArticle, Int64 IdWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAVGStockByIdArticle(connectionString, IdArticle, IdWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WarehouseDeliveryNote GetLabelPrintDetails(Int32 IdArticle, Int64 IdWarehouse, Int64 IdWarehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetLabelPrintDetails(connectionString, IdArticle, IdWarehouse, IdWarehouseDeliveryNote);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PickingMaterials> GetArticleStockDetailForRefund(Int64 idOT, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockDetailForRefund(connectionString, idOT, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        public PickingMaterials GetMadeInPartNumberDetail(Int64 idOTItem, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMadeInPartNumberDetail(connectionString, idOTItem, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateRevisionItemComments_Sprint59(Int64 idRevisionItem, Int64 idDeliveryNoteItem)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateRevisionItemComments_Sprint59(connectionString, idRevisionItem, idDeliveryNoteItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Int64 GetRefundIdOtByBarcode(string barcode)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRefundIdOtByBarcode(connectionString, barcode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        public Int64 GetStockForScanItem(Int64 idArticle, Int64 idWarehouse, Int64 idWarehouseDeliveryNoteItem, Int64 idWarehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetStockForScanItem(connectionString, idArticle, idWarehouse, idWarehouseDeliveryNoteItem, idWarehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PickingMaterials> FillPickingMaterialByIdWarehouseLocation(Int64 idOT, Warehouses warehouse, Int64 idWarehouseLocation)
        {
            try
            {

                return mgr.GetRevisionItemDetails(idOT, warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        public Article GetArticleDetailsByReference_V2030(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {

                return mgr.GetArticleDetailsByReference_V2030(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        public List<ArticlesStock> GetWarehouseDeliveryNoteCode(Int64 idOTItem, Warehouses warehouse, Int64? idWarehouseProductComponent)
        {
            try
            {
                return mgr.GetWarehouseDeliveryNoteCode(idOTItem, warehouse, idWarehouseProductComponent);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        public List<OtItem> GetPendingMaterialArticles(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        public List<OtItem> GetRemainingOtItemsByIdOt_V2030(Int64 idOt, Int64 idWarehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2030(connectionString, idOt, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }
        
        public List<Ots> GetPackingWorkOrdersByWarehouse(Warehouses warehouse)
        {
            List<Ots> ots = null;

            try
            {

                ots = mgr.GetPackingWorkOrdersByWarehouse(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ots;
        }

        public List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation)
        {
            List<InventoryMaterial> inventoryMaterials = null;

            try
            {

                inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseLocation(warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return inventoryMaterials;
        }

        ///// <summary>
        ///// [000][skhade][29-11-2019][GEOS2-1817] Add New Inventory Audits section
        ///// </summary>
        ///// <param name="warehouse">Selected Warehouse</param>
        ///// <param name="idWarehouseLocation">The idWarehouseLocation</param>
        ///// <param name="warehouseInventoryAudit">The warehouseInventoryAudit</param>
        ///// <returns>The List of InventoryMaterial.</returns>
        //public List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2038(Warehouses warehouse, Int64 idWarehouseLocation, WarehouseInventoryAudit warehouseInventoryAudit)
        //{
        //    List<InventoryMaterial> inventoryMaterials = null;

        //    try
        //    {
        //        inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseLocation_V2038(warehouse, idWarehouseLocation, warehouseInventoryAudit, Properties.Settings.Default.ArticleVisualAidsPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }

        //    return inventoryMaterials;
        //}


        public PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            PendingStorageArticles pendingStorageArticles = null;

            try
            {

                pendingStorageArticles = mgr.GetArticleDetailByIdWarehouseDeliveryNoteItem(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return pendingStorageArticles;
        }


        public List<PickingMaterials> GetArticleStockDetailForRefund_V2031(Int64 idOT, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockDetailForRefund_V2031(connectionString, idOT, idWarehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<InventoryMaterial> InsertInventoryMaterialIntoArticleStock(List<InventoryMaterial> inventoryMaterials)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertInventoryMaterialIntoArticleStock(connectionString, inventoryMaterials);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Int64 GetMinimumStockByLocationFullName(Warehouses warehouse, string fullName, Int64 idArticle)
        {

            try
            {
                return mgr.GetMinimumStockByLocationFullName(warehouse, fullName, idArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticleWarehouseLocations> GetArticleWarehouseLocationByIdArticle(Warehouses warehouse, Int64 idArticle)
        {

            try
            {
                return mgr.GetArticleWarehouseLocationByIdArticle(warehouse, idArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ArticlesStock> GetWarehouseDeliveryNoteCode_V2032(Int64 idOTItem, Warehouses warehouse, Int64? idWarehouseProductComponent)
        {
            try
            {
                return mgr.GetWarehouseDeliveryNoteCode_V2032(idOTItem, warehouse, idWarehouseProductComponent);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2032(string IdArticles, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesWarehouseLocation_V2032(IdArticles, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public string GetMadeInByIdWareHouseDeliveryNoteItem(Int64 idWareHouseDeliveryNoteItem, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMadeInByIdWareHouseDeliveryNoteItem(idWareHouseDeliveryNoteItem, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<string> GetReadMeEntriesTitle(Int64 idQuotation, Warehouses warehouse)
        {
            try
            {
                return mgr.GetReadMeEntriesTitle(idQuotation, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }


        public bool AddObservation(Observation observation)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddObservation(connectionString, observation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public List<InventoryMaterial> GetInventoryArticleByIdWarehouseDeliveryNoteItem(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            List<InventoryMaterial> inventoryMaterials = null;

            try
            {

                inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseDeliveryNoteItem(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return inventoryMaterials;
        }


        public List<ArticleWarehouseLocations> AddArticleWarehouseLocationIFNotExist(List<ArticleWarehouseLocations> articleWarehouseLocations)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddArticleWarehouseLocationIFNotExist(connectionString, articleWarehouseLocations);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2032(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2032(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Notification AddNotification(Notification notification)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                notification = mgr.AddNotification(notification, connectionString, connectionWorkbenchString);
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


        public bool AddOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();

                return mgr.AddOTWorkingTime(warehouse, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public TimeSpan GetOTTotalWorkingTime(Int64 idOT, byte idStage, Warehouses warehouse)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();

                return mgr.GetOTTotalWorkingTime(idOT, idStage, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public WarehouseDeliveryNote GetLabelPrintDetails_V2032(Int32 IdArticle, Int64 IdWarehouse, Int64 IdWarehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetLabelPrintDetails_V2032(connectionString, IdArticle, IdWarehouse, IdWarehouseDeliveryNote);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2033(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2033(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateGeosAppSetting(Int32 idAppSetting, bool isON, Int64 idWarehouse)
        {
            try
            {
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.UpdateGeosAppSetting(mainServerConnectionString, idAppSetting, isON, idWarehouse, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocationTransfer_V2033(string IdArticles, Warehouses warehouse)
        {
            try
            {

                return mgr.GetArticlesWarehouseLocationTransfer_V2033(IdArticles, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsOUTSupplierComplaintItem(Int64 IdSupplierComplaint, Warehouses warehouse)
        {
            try
            {

                return mgr.IsOUTSupplierComplaintItem(IdSupplierComplaint, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateSupplierComplaint(Int64 idSupplierComplaint, Warehouses warehouse)
        {
            try
            {

                return mgr.UpdateSupplierComplaint(idSupplierComplaint, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PlanningSimulator> GetPlanningSimulator(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPlanningSimulator(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetPendingArticles_V2033(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingArticles_V2033(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetPendingMaterialArticles_V2033(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2033(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OrderProcessing> GetOrderInProcessing(Warehouses warehouse)
        {
            try
            {
                return mgr.GetOrderInProcessing(warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OrderPreparation> GetPendingWorkOrders(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingWorkOrders(warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<MyWarehouse> GetWarehouseArticleDetails(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticleDetails(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public DashboardInventory GetTotalItemsToLocate(Warehouses warehouse)
        {
            try
            {
                return mgr.GetTotalItemsToLocate(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public DashboardInventory GetTotalItemsToRefill(Warehouses warehouse)
        {
            try
            {
                return mgr.GetTotalItemsToRefill(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }









        public List<TransferMaterials> GetMaterialDetailsByLocationName_V2034(string locatioName, Warehouses warehouse)
        {
            try
            {

                return mgr.GetMaterialDetailsByLocationName_V2034(locatioName, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LocationRefill> GetRefillToListByFullNameSortByStock(Int64 idWarehouse, string fullName)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRefillToListByFullNameSortByStock(connectionString, idWarehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LocationRefill> GetRefillToListSortByStock(Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRefillToListSortByStock(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WarehouseDeliveryNote GetLabelPrintDetails_V2034(Int32 IdArticle, Warehouses warehouse, Int64 IdWarehouseDeliveryNoteItem)
        {
            try
            {

                return mgr.GetLabelPrintDetails_V2034(IdArticle, warehouse, IdWarehouseDeliveryNoteItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WarehouseLocation> GetWarehouseLocationBySelectedWarehouse_V2034(Warehouses warehouse)
        {
            try
            {

                return mgr.GetWarehouseLocationBySelectedWarehouse_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetAllArticlesWithWarehouseLocations_V2034(Warehouses warehouse)
        {
            try
            {
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllArticlesWithWarehouseLocations_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_V2034(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseLocationsByIdWarehouse_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetRemainingOtItemsByIdOt_V2034(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2034(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }


        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2034(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2034(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }

        public List<PendingStorageArticles> GetArticlesPendingStorage_V2034(Warehouses warehouse)
        {
            try
            {

                return mgr.GetArticlesPendingStorage_V2034(warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WarehouseLocation> GetAllWarehouseLocationById_V2034(Warehouses warehouse)
        {
            try
            {

                return mgr.GetAllWarehouseLocationById_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public string GetZoneByIdArticle_V2034(Int64 idArticle, Warehouses warehouse)
        {
            try
            {

                return mgr.GetZoneByIdArticle_V2034(warehouse, idArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WarehouseLocation> GetIsLeafWarehouseLocations_V2034(Warehouses warehouse)
        {
            try
            {
                return mgr.GetIsLeafWarehouseLocations_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LocationRefill> GetRefillToList_V2034(Warehouses warehouse)
        {
            try
            {
                return mgr.GetRefillToList_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public LocationRefill GetArticleWarehouseLocation_V2034(Warehouses warehouse, Int32 idArticle, Int64 position)
        {
            try
            {
                return mgr.GetArticleWarehouseLocation_V2034(warehouse, idArticle, position);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<TransferMaterials> GetRefillMaterialDetails_V2034(string fromLocationName, string toLocationName, Warehouses warehouse, LocationRefill toLocationRefill = null)
        {
            try
            {

                return mgr.GetRefillMaterialDetails_V2034(warehouse, fromLocationName, toLocationName, Properties.Settings.Default.ArticleVisualAidsPath, toLocationRefill);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WarehouseLocation> GetWarehouseLocationToPlaceArticle_V2034(Warehouses warehouse, string reference)
        {
            try
            {
                return mgr.GetWarehouseLocationToPlaceArticle_V2034(warehouse, reference);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LocationRefill> GetRefillToListByFullName_V2034(Warehouses warehouse, string fullName)
        {
            try
            {
                return mgr.GetRefillToListByFullName_V2034(warehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PickingMaterialsSC> GetPickingItemsForSupplierComplaintItemArticlesAndLocation_V2034(string IdArticles, Warehouses warehouse, Int64 IdWarehouseLocation)
        {
            try
            {
                return mgr.GetPickingItemsForSupplierComplaintItemArticlesAndLocation_V2034(warehouse, IdArticles, IdWarehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ArticleWarehouseLocations GetAVGStockByIdArticle_V2034(Int32 IdArticle, Warehouses warehouse)
        {
            try
            {
                return mgr.GetAVGStockByIdArticle_V2034(IdArticle, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PickingMaterials> GetArticleStockDetailForRefund_V2034(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockDetailForRefund_V2034(warehouse, idOT, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PickingMaterials> FillPickingMaterialByIdWarehouseLocation_V2034(Int64 idOT, Warehouses warehouse, Int64 idWarehouseLocation)
        {
            try
            {

                return mgr.GetRevisionItemDetails_V2034(idOT, warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2034(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2034(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2034(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            PendingStorageArticles pendingStorageArticles = null;

            try
            {

                pendingStorageArticles = mgr.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2034(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return pendingStorageArticles;
        }


        public List<InventoryMaterial> GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2034(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            List<InventoryMaterial> inventoryMaterials = null;

            try
            {

                inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2034(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return inventoryMaterials;
        }


        public List<LocationRefill> GetRefillToListSortByStock_V2034(Warehouses warehouse)
        {
            try
            {
                return mgr.GetRefillToListSortByStock_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LocationRefill> GetRefillToListByFullNameSortByStock_V2034(Warehouses warehouse, string fullName)
        {
            try
            {
                return mgr.GetRefillToListByFullNameSortByStock_V2034(warehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PackingBoxType> GetPackingBoxType(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPackingBoxType(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public PackingBox AddPackingBox(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.AddPackingBox(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WOItem> GetRevisionItemPackingWorkOrders(Warehouses warehouse, Int32 idCompany)
        {
            try
            {
                return mgr.GetRevisionItemPackingWorkOrders(warehouse, idCompany, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PackingCompany> GetCompanyPackingWorkOrders(Warehouses warehouse)
        {
            try
            {
                return mgr.GetCompanyPackingWorkOrders(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdatePackingBox(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.UpdatePackingBox(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Warehouses> GetAllWarehousesByUserPermission_V2034(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermission_V2034(connectionString, idActiveUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OTWorkingTime> GetOTWorkingTimeDetails(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                return mgr.GetOTWorkingTimeDetails(idOT, warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PlanningSimulator> GetPlanningSimulator_V2034(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPlanningSimulator_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OtItem> GetPendingMaterialArticles_V2034(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<InventoryMaterial> InsertInventoryMaterialIntoArticleStock_V2034(List<InventoryMaterial> inventoryMaterials)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertInventoryMaterialIntoArticleStock_V2034(connectionString, inventoryMaterials);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetPendingArticles_V2034(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingArticles_V2034(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdatePackingBoxInPartnumber(Warehouses warehouse, Int64 idPackingBox, Int64 idPartNumber)
        {
            try
            {
                return mgr.UpdatePackingBoxInPartnumber(warehouse, idPackingBox, idPartNumber);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<BoxPrint> GetWorkorderByIdPackingBox(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetWorkorderByIdPackingBox(warehouse, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateOTItemStatus(Warehouses warehouse, Int64 idotitem)
        {
            try
            {
                return mgr.UpdateOTItemStatus(warehouse, idotitem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateUnPackingBoxInPartnumber(Warehouses warehouse, Int64 idPackingBox, Int64 idPartNumber)
        {
            try
            {
                return mgr.UpdateUnPackingBoxInPartnumber(warehouse, idPackingBox, idPartNumber);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTItemStatusToFinished(Warehouses warehouse, Int64 idotitem)
        {
            try
            {
                return mgr.UpdateOTItemStatusToFinished(warehouse, idotitem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // <summary>
        /// [WAREHOUSE-M027-04] Add the Work Order section information
        /// </summary>
        /// <param name="idOt">The idot.</param>
        /// <param name="idWarehouse">The connected plant id.</param>
        /// <param name="warehouse">The warehouse contains connection string.</param>
        /// <returns>The OT</returns>
        public Ots GetWorkOrderByIdOt_V2035(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetWorkOrderByIdOt_V2035(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2035(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2035(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }


        /// <summary>
        /// Load PO pending reception by warehouse.
        /// </summary>
        /// <param name="warehouse">The Warehouse which contains connection string.</param>
        /// <returns>The list of PO.</returns>
        public List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2035(Warehouses warehouse)
        {
            try
            {

                return mgr.GetPurchaseOrdersPendingReceptionByWarehouse_V2035(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2035(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2035(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetPackingWorkOrdersByWarehouse_V2035(Warehouses warehouse)
        {
            List<Ots> ots = null;

            try
            {

                ots = mgr.GetPackingWorkOrdersByWarehouse_V2035(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ots;
        }


        public List<WOItem> GetPackedItemByIdPackingBox(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetPackedItemByIdPackingBox(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<MyWarehouse> GetWarehouseArticleDetails_V2035(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticleDetails_V2035(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTItemStatus_V2035(Warehouses warehouse, Int64 idotitem, Int32 idOperator)
        {
            try
            {
                return mgr.UpdateOTItemStatus_V2035(warehouse, idotitem, idOperator);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateOTItemStatusToFinished_V2035(Warehouses warehouse, Int64 idotitem, Int32 idOperator)
        {
            try
            {
                return mgr.UpdateOTItemStatusToFinished_V2035(warehouse, idotitem, idOperator);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }




        public bool AddCancelledOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();

                return mgr.AddCancelledOTWorkingTime(warehouse, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool UpdateItemStatusAndStage_V2035(Warehouses warehouse, Int64 idOtItem, byte idItemOtStatus, Int32 idOperator)
        {
            try
            {

                return mgr.UpdateItemStatusAndStage_V2035(warehouse, idOtItem, idItemOtStatus, idOperator);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateRevisionItemComments_V2035(Warehouses warehouse, Int64 idRevisionItem, Int64 idDeliveryNoteItem)
        {
            try
            {

                return mgr.UpdateRevisionItemComments_V2035(warehouse, idRevisionItem, idDeliveryNoteItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdatePackingBoxInPartnumber_V2035(Warehouses warehouse, Int64 idPackingBox, Int64 idotitem)
        {
            try
            {
                return mgr.UpdatePackingBoxInPartnumber_V2035(warehouse, idPackingBox, idotitem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateUnPackingBoxInPartnumber_V2035(Warehouses warehouse, Int64 idPackingBox, Int64 idotitem)
        {
            try
            {
                return mgr.UpdateUnPackingBoxInPartnumber_V2035(warehouse, idPackingBox, idotitem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PackingCompany> GetCompanyPackingWorkOrders_V2035(Warehouses warehouse, string siteIds)
        {
            try
            {
                return mgr.GetCompanyPackingWorkOrders_V2035(warehouse, siteIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<BoxPrint> GetWorkorderByIdPackingBox_V2036(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetWorkorderByIdPackingBox_V2036(warehouse, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Article GetArticleDetailsByReference_V2036(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2036(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ArticleType> GetArticleTypes(Warehouses warehouse)
        {
            try
            {
                return mgr.GetArticleTypes(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();

                return mgr.UpdateOTWorkingTime(warehouse, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool DeleteOTWorkingTime(Warehouses warehouse, OTWorkingTime otWorkingTime)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();

                return mgr.DeleteOTWorkingTime(warehouse, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public bool UpdateGeosAppSettingById(GeosAppSetting geosAppSetting)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateGeosAppSettingById(mainConnectionString, geosAppSetting);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //public Ots GetWorkOrderByIdOt_V2036(Int64 idOt, Warehouses warehouse)
        //{
        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

        //        return mgr.GetWorkOrderByIdOt_V2036(idOt, warehouse);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}


        public List<OTWorkingTime> GetOTWorkingTimeDetails_V2036(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                return mgr.GetOTWorkingTimeDetails_V2036(idOT, warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateItemStatusAndStageForRefund(Warehouses warehouse, Int64 idOtItem, byte idItemOtStatus, Int32 idOperator)
        {
            try
            {

                return mgr.UpdateItemStatusAndStageForRefund(warehouse, idOtItem, idItemOtStatus, idOperator);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateRevisionItemCommentsForRefund(Warehouses warehouse, Int64 idRevisionItem, Int64 idDeliveryNoteItem)
        {
            try
            {

                return mgr.UpdateRevisionItemCommentsForRefund(warehouse, idRevisionItem, idDeliveryNoteItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2036(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2036(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Article> GetWarehouseArticlesStockByWarehouse_V2036(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_V2036(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PickingMaterials> GetArticleStockDetailForRefund_V2036(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockDetailForRefund_V2036(warehouse, idOT, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OrderProcessing> GetOrderInProcessing_V2036(Warehouses warehouse)
        {
            try
            {
                return mgr.GetOrderInProcessing_V2036(warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OrderProcessing> GetOrderInProcessing_V2051(Warehouses warehouse)
        {
            try
            {
                return mgr.GetOrderInProcessing_V2051(warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OrderPreparation> GetPendingWorkOrders_V2036(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingWorkOrders_V2036(warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Int64 GetCountOfRefillWithNoStock(Warehouses warehouse)
        {
            try
            {
                return mgr.GetCountOfRefillWithNoStock(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Article> GetSelectedArticleImageInBytes(List<Article> articles)
        {
            try
            {
                return mgr.GetSelectedArticleImageInBytes(articles, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool RemovePackingBox(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.RemovePackingBox(warehouse, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public DashboardInventory GetTotalItemsToLocate_V2037(Warehouses warehouse)
        {
            try
            {
                return mgr.GetTotalItemsToLocate_V2037(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public DashboardInventory GetTotalItemsToRefill_V2037(Warehouses warehouse)
        {
            try
            {
                return mgr.GetTotalItemsToRefill_V2037(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Article GetArticleDetailsByReference_V2037(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2037(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateIsClosedInPackingBox(Warehouses warehouse, Int64 idPackingBox, sbyte isClosed)
        {
            try
            {
                return mgr.UpdateIsClosedInPackingBox(warehouse, idPackingBox, isClosed);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Article> GetArticlesByLocation(Warehouses warehouse, Int64 idFromWarehouseLocation, Int64 idToWarehouseLocation)
        {
            try
            {
                return mgr.GetArticlesByLocation(warehouse, idFromWarehouseLocation, idToWarehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OrderPreparation> GetPendingWorkOrders_V2037(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingWorkOrders_V2037(warehouse, Properties.Settings.Default.UserProfileImage);
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
        /// [000][skhade][28-11-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="warehouse">The selected warehouse.</param>
        /// <returns>The list of WarehouseInventoryAudits</returns>
        public List<WarehouseInventoryAudit> GetAllWarehouseInventoryAudits(Warehouses warehouse)
        {
            try
            {
                return mgr.GetAllWarehouseInventoryAudits(warehouse);
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
        /// [000][skhade][28-11-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="warehouse">The selected warehouse.</param>
        /// <param name="WarehouseInventoryAudit"></param>
        /// <returns>The WarehouseInventoryAudit with auto increment IdWarehouseInventoryAudit</returns>
        public WarehouseInventoryAudit AddWarehouseInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit)
        {
            try
            {
                return mgr.AddWarehouseInventoryAudit(warehouse, WarehouseInventoryAudit);
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
        /// [000][skhade][28-11-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="warehouse">The selected warehouse.</param>
        /// <param name="WarehouseInventoryAudit">The WarehouseInventoryAudit to update.</param>
        /// <returns>True if data is updated else false.</returns>
        public bool UpdateWarehouseInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit)
        {
            try
            {
                return mgr.UpdateWarehouseInventoryAudit(warehouse, WarehouseInventoryAudit);
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
        /// [000][skhade][28-11-2019][GEOS2-1817] Add New Inventory Audits section
        /// </summary>
        /// <param name="warehouse">The selected warehouse.</param>
        /// <returns>The list of WarehouseInventoryAudits</returns>
        public List<WarehouseInventoryAudit> GetOpenWarehouseInventoryAudits(Warehouses warehouse)
        {
            try
            {
                return mgr.GetOpenWarehouseInventoryAudits(warehouse);
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
        /// [000][skhade][28-11-2019][GEOS2-1817] Add New Inventory Audits section - Save inventoried results in this table.
        /// </summary>
        /// <param name="warehouse">The Selected Warehouse.</param>
        /// <param name="warehouseInventoryAuditItems">The list of warehouse inventory audit items</param>
        /// <returns></returns>
        public bool AddUpdateWarehouseInventoryAuditItems(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems)
        {
            try
            {
                return mgr.AddUpdateWarehouseInventoryAuditItems(warehouse, warehouseInventoryAuditItems);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetWarehouseArticlesStockByWarehouse_V2038(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_V2038(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Article GetArticleDetailsByReference_V2038(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2038(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetPendingMaterialArticles_V2038(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2038(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PlanningSimulator> GetPlanningSimulator_V2038(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPlanningSimulator_V2038(warehouse);
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
        /// [000][skhade][28-11-2019][GEOS2-1817] Add New Inventory Audits section - Save inventoried results in this table.
        /// </summary>
        /// <param name="warehouse">The Selected Warehouse.</param>
        /// <param name="warehouseInventoryAudit">The warehouse inventory audit</param>
        /// <returns></returns>
        public List<WarehouseInventoryAuditItem> GetWarehouseInventoryAuditItemsByInventoryAudit(Warehouses warehouse, WarehouseInventoryAudit warehouseInventoryAudit, WarehouseLocation warehouseLocation)
        {
            try
            {
                return mgr.GetWarehouseInventoryAuditItemsByInventoryAudit(warehouse, warehouseInventoryAudit, warehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Ots GetWorkOrderByIdOt_V2038(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetWorkOrderByIdOt_V2038(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<string> GetReadyForShippingOTItems(Warehouses warehouse, string idOts)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems(warehouse, idOts,Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Article> GetPendingArticles_V2038(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingArticles_V2038(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Tuple<Int32, string>> GetCustomersWithOneOrderBox()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetCustomersWithOneOrderBox(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Tuple<Int32, string>> GetEMDEPSites()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetEMDEPSites(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Article> GetWarehouseArticlesStockByWarehouse_V2039(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_V2039(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WOItem> GetRevisionItemPackingWorkOrders_V2039(Warehouses warehouse, Int32 idCompany)
        {
            try
            {
                return mgr.GetRevisionItemPackingWorkOrders_V2039(warehouse, idCompany, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WOItem> GetPackedItemByIdPackingBox_V2039(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetPackedItemByIdPackingBox_V2039(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetPendingMaterialArticles_V2040(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2040(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetNotPackingButOTDeliveryDateInCurrentWeek(Warehouses warehouse)
        {
            try
            {
                return mgr.GetNotPackingButOTDeliveryDateInCurrentWeek(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2040(Warehouses warehouse, string idOts)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2040(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2041(Warehouses warehouse, string idOts)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2041(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2042(Warehouses warehouse, string idOts)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2042(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetAllArticlesWithWarehouseLocations_V2041(Warehouses warehouse)
        {
            try
            {
               return mgr.GetAllArticlesWithWarehouseLocations_V2041(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2041(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2041(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateArticleDetails_V2041(Article article)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2041(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateArticleDetails_V2051(Article article)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2051(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Stage> GetStagesByWarehouseStageIds()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetStagesByWarehouseStageIds(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WOItem> GetRevisionItemPackingWorkOrders_V2041(Warehouses warehouse, Int32 idCompany)
        {
            try
            {
                return mgr.GetRevisionItemPackingWorkOrders_V2041(warehouse, idCompany, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<WOItem> GetPackedItemByIdPackingBox_V2041(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetPackedItemByIdPackingBox_V2041(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetRemainingOtItemsByIdOt_V2041(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2041(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }




        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2041(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2041(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }


        public PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2041(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            PendingStorageArticles pendingStorageArticles = null;

            try
            {

                pendingStorageArticles = mgr.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2041(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return pendingStorageArticles;
        }

        public double GetWONOfferAmount(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetWONOfferAmount(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public double GetArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetArticleStockAmountInWarehouse(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public double GetAbosleteArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetAbosleteArticleStockAmountInWarehouse(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public double GetSleepedArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency,  DateTime accountingFromYear, DateTime accountingToYear, Int64 aritclesleepDays)
        {
            try
            {

                return mgr.GetSleepedArticleStockAmountInWarehouse(warehouse, idCurrency,  accountingFromYear, accountingToYear, aritclesleepDays);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<OfferDetail> GetSalesByMonth(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetSalesByMonth(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<WarehouseCustomer> GetSalesByCustomer(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetSalesByCustomer(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<WarehouseInventoryWeek> GetInventoryWeek(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetInventoryWeek(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public byte[] GetArticleImageInBytes(string ImagePath)
        {
            try
            {
                return mgr.GetArticleImageInBytes(  ImagePath, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OtItem> GetPendingMaterialArticles_V2044(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2044(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2044(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2044(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath,fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2051(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2051(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2044(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2044(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2044(Warehouses warehouse)
        {
            try
            {

                return mgr.GetPurchaseOrdersPendingReceptionByWarehouse_V2044(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WMSOrder> GetOrders(Warehouses warehouse, DateTime accountingYearFrom, DateTime accountingYearTo)
        {
            try
            {

                return mgr.GetOrders(warehouse, accountingYearFrom, accountingYearTo, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

      

        public List<InventoryMaterial> GetInternalUsePickArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation)
        {
            try
            {

                return mgr.GetInternalUsePickArticleByIdWarehouseLocation(warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<InventoryMaterial> GetInternalUseRefundArticleByIdWarehouseLocation(Warehouses warehouse, Int64 idWarehouseLocation)
        {
            try
            {

                return mgr.GetInternalUseRefundArticleByIdWarehouseLocation(warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<OtItem> GetRemainingOtItemsByIdOt_V2051(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2051(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }



        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2051(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2051(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return serialNumbers;
        }


        public PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2051(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            PendingStorageArticles pendingStorageArticles = null;

            try
            {

                pendingStorageArticles = mgr.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2051(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return pendingStorageArticles;
        }

        public List<WOItem> GetPackedItemByIdPackingBox_V2051(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetPackedItemByIdPackingBox_V2051(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WOItem> GetRevisionItemPackingWorkOrders_V2051(Warehouses warehouse, Int32 idCompany)
        {
            try
            {
                return mgr.GetRevisionItemPackingWorkOrders_V2051(warehouse, idCompany, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ArticlesStock> GetWarehouseDeliveryNoteCode_V2060(Int64 idOTItem, Warehouses warehouse, Int64? idWarehouseProductComponent)
        {
            try
            {
                return mgr.GetWarehouseDeliveryNoteCode_V2060(idOTItem, warehouse, idWarehouseProductComponent);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
    }
}
