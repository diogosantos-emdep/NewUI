using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.SAM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2090(Warehouses warehouse, Int64 idWarehouseLocation)
        {
            List<InventoryMaterial> inventoryMaterials = null;

            try
            {

                inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseLocation_V2090(warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
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

        /// <summary>
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
                return mgr.GetReadyForShippingOTItems(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort);
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

        public double GetSleepedArticleStockAmountInWarehouse(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear, Int64 aritclesleepDays)
        {
            try
            {

                return mgr.GetSleepedArticleStockAmountInWarehouse(warehouse, idCurrency, accountingFromYear, accountingToYear, aritclesleepDays);
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
                return mgr.GetArticleImageInBytes(ImagePath, Properties.Settings.Default.ArticleVisualAidsPath);
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
                return mgr.GetArticleDetailsByReference_V2044(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate);
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


        public bool InsertIntoArticleStockForInternalUse(InventoryMaterial inventoryMaterial)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockForInternalUse(connectionString, inventoryMaterial);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetNotPacking(Warehouses warehouse)
        {
            try
            {
                return mgr.GetNotPacking(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2080(Warehouses warehouse, string idOts)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2080(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateArticleDetails_V2080(Article article)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2080(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2080(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2080(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Int64 GetArticleLockedStockByWarehouse(int idArticle, int idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleLockedStockByWarehouse(connectionString, idArticle, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Warehouses> GetAllWarehouses()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehouses(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool IsInUSENoWarehouseLocation(Int64 idWarehouseLocation, Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsInUSENoWarehouseLocation(connectionString, idWarehouseLocation, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseLocation> GetAllWarehouseLocationsByIdWarehouse_V2080(Int64 idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehouseLocationsByIdWarehouse_V2080(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LocationRefill> GetRefillToListByFullName_V2080(Warehouses warehouse, string fullName)
        {
            try
            {
                return mgr.GetRefillToListByFullName_V2080(warehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LocationRefill> GetRefillToListByFullNameSortByStock_V2080(Warehouses warehouse, string fullName)
        {
            try
            {
                return mgr.GetRefillToListByFullNameSortByStock_V2080(warehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsExistWarehouseLocationFullName_V2080(string fullName, long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsExistWarehouseLocationFullName_V2080(fullName, idWarehouse, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TransferMaterials> GetMaterialDetailsByLocationName_V2080(string locatioName, Warehouses warehouse)
        {
            try
            {

                return mgr.GetMaterialDetailsByLocationName_V2080(locatioName, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LocationRefill> GetRefillToListSortByStock_V2080(Warehouses warehouse)
        {
            try
            {
                return mgr.GetRefillToListSortByStock_V2080(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LocationRefill> GetRefillToList_V2080(Warehouses warehouse)
        {
            try
            {
                return mgr.GetRefillToList_V2080(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Int64 GetMaxPosition_V2080(Int64 idParent, Int64 idWarehouse, string fullName)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMaxPosition_V2080(idParent, idWarehouse, fullName, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Warehouses> GetSelectedWarehousesArticleStock(Int32 idArticle, List<Warehouses> Lstwarehouses)
        {
            try
            {

                return mgr.GetSelectedWarehousesArticleStock(idArticle, Lstwarehouses);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Warehouses> GetAllWarehouses_V2080(Int32 idUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                // return mgr.GetAllWarehouses_V2080(connectionString, idUser);
                return mgr.GetAllWarehousesByUserPermission_V2490(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseInventoryAudit> GetAllWarehouseInventoryAudits_V2090(Warehouses warehouse)
        {
            try
            {
                return mgr.GetAllWarehouseInventoryAudits_V2090(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public LocationRefill GetArticleWarehouseLocation_V2090(Warehouses warehouse, Int32 idArticle, Int64 position)
        {
            try
            {
                return mgr.GetArticleWarehouseLocation_V2090(warehouse, idArticle, position);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddUpdateWarehouseInventoryAuditItems_V2090(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems)
        {
            try
            {
                return mgr.AddUpdateWarehouseInventoryAuditItems_V2090(warehouse, warehouseInventoryAuditItems);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseInventoryAuditItem> GetWarehouseInventoryAuditItemsByInventoryAudit_V2090(Warehouses warehouse, WarehouseInventoryAudit warehouseInventoryAudit, WarehouseLocation warehouseLocation)
        {
            try
            {
                return mgr.GetWarehouseInventoryAuditItemsByInventoryAudit_V2090(warehouse, warehouseInventoryAudit, warehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices(Warehouses warehouse, Currency currency)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_WithPrices(warehouse, currency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetPackingWorkOrdersByWarehouse_V2130(Warehouses warehouse)
        {
            List<Ots> ots = null;

            try
            {

                ots = mgr.GetPackingWorkOrdersByWarehouse_V2130(warehouse);
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


        #region GEOS2-2952 Picking Screen incorrectly asks to pick the assembled components inside the assembly item whenever the assembly item is inside a Kit Item in the work order

        public List<OtItem> GetRemainingOtItemsByIdOt_V2130(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2130(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2130(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2130(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        #endregion




        #region GEOS2-2370 Add Articles Report Screen in WMS like CRM report.
        public List<Offer> GetArticlesReportDetails_V2130(Int32 idActiveUser,
            Company company, DateTime fromDate, DateTime toDate, byte idCurrency, string idArticles,
            long IdWarehouse)
        {
            List<Offer> offers = null;

            try
            {
                WarehouseManager mgr = new WarehouseManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                offers = mgr.GetArticlesReportDetails_V2130(connectionString, idActiveUser,
                    company, fromDate, toDate, idCurrency, idArticles, IdWarehouse);
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
        #endregion


        #region GEOS2-3032 Error when picking is started for a work order which has an assembly Type Item when item's current stock=0 & remaining quantity>0 and anyone child item's current stock>0 & remaining quantity>0

        public List<OtItem> GetRemainingOtItemsByIdOt_V2140(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2140(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2140(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2140(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        #endregion



        #region  GEOS2-2250 Orders Preparation

        public List<Ots> GetWorkOrdersPreparationReport_V2140(Warehouses warehouse,
            DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetWorkOrdersPreparationReport_V2140(warehouse,
                    fromDate, toDate);
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


        public WarehouseAvailability GetAllWarehouseAvailabilityByIdCompany_V2140(Warehouses warehouse)
        {
            try
            {
                // string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWarehouseAvailabilityByIdCompany_V2140(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Boolean UpdateWarehouseAvailability_V2140(WarehouseAvailability warehouseAvailability)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWarehouseAvailability_V2140(connectionString, warehouseAvailability);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2150(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2150(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2150(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2150(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public PendingStorageArticles GetArticleDetailByIdWarehouseDeliveryNoteItem_V2150(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            PendingStorageArticles pendingStorageArticles = null;

            try
            {

                pendingStorageArticles = mgr.GetArticleDetailByIdWarehouseDeliveryNoteItem_V2150(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
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



        #region  GEOS2-3104 Orders Preparation screen loading is taking 5 min

        public List<Ots> GetWorkOrdersPreparationReport_V2150(Warehouses warehouse,
            DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetWorkOrdersPreparationReport_V2150(warehouse,
                    fromDate, toDate);
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


        //[GEOS-3098]
        public List<TransferMaterials> GetMaterialDetailsByLocationName_V2150(string locatioName, Warehouses warehouse)
        {
            try
            {

                return mgr.GetMaterialDetailsByLocationName_V2150(locatioName, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3098
        /// <summary>
        /// Method for insert Transfer details.
        /// </summary>
        /// <param name="transferMaterials"></param>
        /// <returns>Is added or not</returns>
        public bool InsertIntoArticleStockForTransferMaterial_V2150(TransferMaterials transferMaterials)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockForTransferMaterial_V2150(connectionString, transferMaterials);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool InsertIntoArticleStockForLocateMaterial_V2150(PendingStorageArticles pendingStorageArticles)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockForLocateMaterial_V2150(connectionString, pendingStorageArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LocationRefill> GetRefillToListByFullNameSortByStock_V2150(Warehouses warehouse, string fullName)
        {
            try
            {
                return mgr.GetRefillToListByFullNameSortByStock_V2150(warehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetRemainingOtItemsByIdOt_V2150(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                // string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2150(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2150(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2150(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        public List<LocationRefill> GetRefillToListSortByStock_V2150(Warehouses warehouse)
        {
            try
            {
                return mgr.GetRefillToListSortByStock_V2150(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<TransferMaterials> GetRefillMaterialDetails_V2160(string fromLocationName, string toLocationName, Warehouses warehouse, LocationRefill toLocationRefill = null)
        {
            try
            {

                return mgr.GetRefillMaterialDetails_V2160(warehouse, fromLocationName, toLocationName, Properties.Settings.Default.ArticleVisualAidsPath, toLocationRefill);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LocationRefill> GetRefillToListByFullNameSortByStock_V2160(Warehouses warehouse, string fullName)
        {
            try
            {
                return mgr.GetRefillToListByFullNameSortByStock_V2160(warehouse, fullName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2160(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2160(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<PickingMaterials> GetArticleStockDetailForRefund_V2160(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockDetailForRefund_V2160(warehouse, idOT, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GESO2-1957]
        public bool IsIdWarehouseLocationPresent(Warehouses warehouse, string IdArticles, PickingMaterials pickingMaterials)
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsIdWarehouseLocationPresent(warehouse, IdArticles, pickingMaterials);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GESO2-1957]
        public List<ArticleWarehouseLocations> GetAllPossibleLocationOfArticle(string IdArticles, Warehouses warehouse)
        {
            try
            {

                return mgr.GetAllPossibleLocationOfArticle(IdArticles, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }






        //[GEOS2-1957]
        /// <summary>
        /// Method for insert negative quantity of  article stock, after scan for piking material. 
        /// </summary>
        /// <param name="pickingMaterials"></param>
        /// <returns></returns>
        public bool InsertIntoArticlesStockForRefund(PickingMaterials pickingMaterials)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticlesStockForRefund(connectionString, pickingMaterials);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-1957]
        public List<PickingMaterials> GetArticleStockDetailForRefund_V2170(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockDetailForRefund_V2170(warehouse, idOT, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-1957]
        /// <summary>
        /// Method for insert negative quantity of  article stock, after scan for piking material. 
        /// </summary>
        /// <param name="pickingMaterials"></param>
        /// <returns></returns>
        public bool InsertIntoArticleStockForRefund_V2170(PickingMaterials pickingMaterials)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockForRefund_V2170(connectionString, pickingMaterials);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2170(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2170(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetWarehouseArticlesStockByWarehouse_V2180(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_V2180(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2180(Warehouses warehouse, Currency currency)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_WithPrices_V2180(warehouse, currency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<InventoryMaterial> InsertInventoryMaterialIntoArticleStock_V2190(List<InventoryMaterial> inventoryMaterials, string mainComments)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertInventoryMaterialIntoArticleStock_V2190(connectionString, inventoryMaterials, mainComments);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PlanningSimulator> GetPlanningSimulator_V2190(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPlanningSimulator_V2190(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Ots GetWorkOrderByIdOt_V2200(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetWorkOrderByIdOt_V2200(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2200(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2200(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetPackingWorkOrdersByWarehouse_V2200(Warehouses warehouse)
        {
            List<Ots> ots = null;

            try
            {

                ots = mgr.GetPackingWorkOrdersByWarehouse_V2200(warehouse, Properties.Settings.Default.CountryFilePath);
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



        public WarehouseInventoryAudit GetAllItemsForSelectedWarehouseInventoryAudit_V2200(Warehouses warehouse, Int64 idWarehouseInventoryAudit)
        {
            try
            {
                return mgr.GetAllItemsForSelectedWarehouseInventoryAudit_V2200(warehouse, idWarehouseInventoryAudit);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WOItem> GetRevisionItemPackingWorkOrders_V2210(Warehouses warehouse, Int32 idCompany, Int64? ProducerIdCountryGroup)
        {
            try
            {
                return mgr.GetRevisionItemPackingWorkOrders_V2210(warehouse, idCompany, Properties.Settings.Default.ArticleVisualAidsPath, ProducerIdCountryGroup);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3405
        public List<OriginOfContent> GetOriginOfContentList(Warehouses warehouse)
        {
            try
            {
                return mgr.GetOriginOfContentList(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //GEOS2-3405
        public PackingBox AddPackingBox_V2210(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.AddPackingBox_V2210(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //GEOS2-3405
        public bool UpdatePackingBox_V2210(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.UpdatePackingBox_V2210(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //GEOS2-3405
        public List<PackingCompany> GetCompanyPackingWorkOrders_V2210(Warehouses warehouse, string siteIds)
        {
            try
            {
                return mgr.GetCompanyPackingWorkOrders_V2210(warehouse, siteIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<BoxPrint> GetWorkorderByIdPackingBox_V2210(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetWorkorderByIdPackingBox_V2210(warehouse, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2210(Warehouses warehouse, Int64 idWarehouseLocation)
        {
            List<InventoryMaterial> inventoryMaterials = null;

            try
            {

                inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseLocation_V2210(warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
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


        public List<InventoryMaterial> GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2210(Warehouses warehouse, Int64 idWarehouseDeliveryNoteItem)
        {
            List<InventoryMaterial> inventoryMaterials = null;

            try
            {
                inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseDeliveryNoteItem_V2210(warehouse, idWarehouseDeliveryNoteItem, Properties.Settings.Default.ArticleVisualAidsPath);
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


        public bool IsExistArticleWarehouseLocation(Int32 idArticle, Int64 idWarehouseLocation, Warehouses warehouse)
        {
            try
            {
                return mgr.IsExistArticleWarehouseLocation(idArticle, idWarehouseLocation, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2210(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2210(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateArticleDetails_V2220(Article article)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2220(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_V2220(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseLocationsByIdWarehouse_V2220(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Warehouses> GetAllWarehousesByUserPermission_V2220(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermission_V2220(connectionString, idActiveUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Manufacturer> GetAllActiveManufacturers()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllActiveManufacturers(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public WarehouseLocation AddWarehouseLocation_V2220(WarehouseLocation warehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWarehouseLocation_V2220(connectionString, warehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public WarehouseLocation UpdateWarehouseLocation_V2220(WarehouseLocation warehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWarehouseLocation_V2220(connectionString, warehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Article GetArticleDetailsByReference_V2230(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2230(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-34556
        public PackingBox AddPackingBox_V2230(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.AddPackingBox_V2230(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //GEOS2-3556
        public bool UpdatePackingBox_V2230(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.UpdatePackingBox_V2230(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3408
        public List<PackingCompany> GetCompanyPackingWorkOrders_V2230(Warehouses warehouse, string siteIds)
        {
            try
            {
                return mgr.GetCompanyPackingWorkOrders_V2230(warehouse, siteIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-2988
        public List<Warehouses> GetSelectedWarehousesArticleStock_V2230(Int32 idArticle, List<Warehouses> Lstwarehouses)
        {
            try
            {
                return mgr.GetSelectedWarehousesArticleStock_V2230(idArticle, Lstwarehouses);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3384
        public PackingBox AddPackingBox_V2240(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.AddPackingBox_V2240(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3384
        public bool UpdatePackingBox_V2240(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.UpdatePackingBox_V2240(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3384
        public List<PackingCompany> GetCompanyPackingWorkOrders_V2240(Warehouses warehouse, string siteIds)
        {
            try
            {
                return mgr.GetCompanyPackingWorkOrders_V2240(warehouse, siteIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetNotPacking_V2250(Warehouses warehouse)
        {
            try
            {
                return mgr.GetNotPacking_V2250(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<StockBySupplier> GetStockBySupplier(Warehouses warehouse)
        {
            try
            {
                return mgr.GetStockBySupplier(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3722
        public PackingBox AddPackingBox_V2260(Warehouses warehouse, PackingBox packingBox)
        {
            try
            {
                return mgr.AddPackingBox_V2260(warehouse, packingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3717
        public List<Ots> GetNotPacking_V2260(Warehouses warehouse)
        {
            try
            {
                return mgr.GetNotPacking_V2260(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3551
        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2290(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2290(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2300(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2300(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetWorkOrdersPreparationReport_V2300(Warehouses warehouse, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetWorkOrdersPreparationReport_V2300(warehouse,
                    fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2300(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2300(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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


        public List<Article> GetPendingArticles_V2300(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingArticles_V2300(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // shubham[skadam] GEOS2-3762 No reporta correctamente Check Availability (01IFRM06N13G1L)  09 Aug 2022
        public List<Warehouses> GetSelectedWarehousesArticleStock_V2300(Int32 idArticle, List<Warehouses> Lstwarehouses)
        {
            try
            {
                return mgr.GetSelectedWarehousesArticleStock_V2300(idArticle, Lstwarehouses);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // shubham[skadam] GEOS2-3548 In Pending Articles Change the way how the PO Expected delivery date is calculated when we have PO items with Expected Delivery Dates info  12 Aug 2022 
        public List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2300(Warehouses warehouse)
        {
            try
            {

                return mgr.GetPurchaseOrdersPendingReceptionByWarehouse_V2300(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3831][12.08.2022]
        public DashboardInventory GetTotalItemsToRefill_V2300(Warehouses warehouse)
        {
            try
            {
                return mgr.GetTotalItemsToRefill_V2300(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        // shubham[skadam] GEOS2-3907 Error when trying to send the Email Ready for Shipment  06 Sep 2022
        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2301(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2301(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][14.09.2022][GEOS2-3902]
        public List<Ots> GetWorkOrdersPreparationReport_V2301(Warehouses warehouse, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetWorkOrdersPreparationReport_V2301(warehouse,
                    fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // shubham[skadam]  GEOS2-3918 EURO and NO EURO ots have been mixed in a box  15 Sep 2022
        public bool WMS_ISExistsEuropeanAndNonEuropeanCountry(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.WMS_ISExistsEuropeanAndNonEuropeanCountry(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2320(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2320(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Ots> GetPackingWorkOrdersByWarehouse_V2320(Warehouses warehouse)
        {
            List<Ots> ots = null;

            try
            {

                ots = mgr.GetPackingWorkOrdersByWarehouse_V2320(warehouse, Properties.Settings.Default.CountryFilePath);
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

        public List<WorkflowStatus> GetAllWorkflowStatus_V2320()
        {
            List<WorkflowStatus> lstWorkflowStatus = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                lstWorkflowStatus = mgr.GetAllWorkflowStatus_V2320(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lstWorkflowStatus;
        }

        public List<WorkflowTransition> GetAllWorkflowTransitions_V2320()
        {
            List<WorkflowTransition> lstWorkflowStatus = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                lstWorkflowStatus = mgr.GetAllWorkflowTransitions_V2320(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lstWorkflowStatus;
        }

        public bool UpdateWorkflowStatusInOT_V2320(Int64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            bool isupdated = false;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isupdated = mgr.UpdateWorkflowStatusInOT_V2320(connectionString, IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
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

        public Ots GetWorkOrderByIdOt_V2320(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetWorkOrderByIdOt_V2320(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<OtItem> GetPendingMaterialArticles_V2330(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2330(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.jangra][GEOS2-3959][07/11/2022]
        public List<ArticleRefillDetail> GetArticleRefillDetails(DateTime fromDate, DateTime toDate, Warehouses warehouse)
        {
            try
            {
                return mgr.GetArticleRefillDetails(fromDate, toDate, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool IsDeleteInventoryAudit(long IdWarehouseInventoryAudit)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsDeleteInventoryAudit(connectionString, IdWarehouseInventoryAudit);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[sshegaonkae][GEOS2-4041][06/12/2022]
        public List<CountryEURO> WMS_ISExistsEuropeanAndNonEuropeanCountry_V2340(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.WMS_ISExistsEuropeanAndNonEuropeanCountry_V2340(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public List<InventoryAuditLocation> GetAllLocationsForSelectedWarehouseInventoryAudit(Int64 idWarehouse, Int64 idWarehouseInventoryAudit)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllLocationsForSelectedWarehouseInventoryAudit(idWarehouse, idWarehouseInventoryAudit, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public List<InventoryAuditArticle> GetAllArticleForSelectedWarehouseInventoryAudit(Int64 idWarehouse, Int64 idWarehouseInventoryAudit)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllArticleForSelectedWarehouseInventoryAudit(idWarehouse, idWarehouseInventoryAudit, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public List<InventoryAuditArticle> GetAllArticlesByWarehouseLocation(Int64 idWarehouse, Int64 IdWarehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllArticlesByWarehouseLocation(idWarehouse, IdWarehouseLocation, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public List<InventoryAuditLocation> GetAllWarehouseLocationsByArticle(Int64 idWarehouse, Int64 idArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehouseLocationsByArticle(idWarehouse, idArticle, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public List<ArticleCategory> GetWMSArticlesWithCategoryForReference()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWMSArticlesWithCategoryForReference(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public bool UpdateWarehouseInventoryAudit_V2340(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit)
        {
            try
            {
                return mgr.UpdateWarehouseInventoryAudit_V2340(warehouse, WarehouseInventoryAudit);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public bool AddUpdateWarehouseInventoryAuditItems_V2340(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems)
        {
            try
            {
                return mgr.AddUpdateWarehouseInventoryAuditItems_V2340(warehouse, warehouseInventoryAuditItems);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public string GetEmployeeCodeByIdUser(Int32 IdUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeCodeByIdUser(IdUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateArticleDetails_V2340(Article article, Warehouses warehouse)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2340(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public List<WarehouseLocation> GetInuseWarehouseLocationsByIdWarehouse(Warehouses warehouse)
        {
            try
            {
                return mgr.GetInuseWarehouseLocationsByIdWarehouse(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool ExpiredArticleWarningsCleanUp_V2340(DateTime StartDate)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.ExpiredArticleWarningsCleanUp_V2340(mainConnectionString, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public bool IsArticleExistForSelectedWarehouseInventoryAudit(Int32 IdArticle, Int64 idWarehouseInventoryAudit)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsArticleExistForSelectedWarehouseInventoryAudit(IdArticle, idWarehouseInventoryAudit, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][09.12.2022][GEOS2-3962]
        public bool IsLocationExistForSelectedWarehouseInventoryAudit(long IdWarehouse, string WarehouseLocation, Int64 idWarehouseInventoryAudit)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsLocationExistForSelectedWarehouseInventoryAudit(IdWarehouse, WarehouseLocation, idWarehouseInventoryAudit, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][09.12.2022][GEOS2-3962]
        public List<WarehouseInventoryAudit> GetAllWarehouseInventoryAudits_V2340(Warehouses warehouse)
        {
            try
            {
                return mgr.GetAllWarehouseInventoryAudits_V2340(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-3532
        public Article GetArticleDetailsByReference_V2360(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2360(reference, Properties.Settings.Default.ArticleVisualAidsPath, warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateArticleDetails_V2360(Article article, Warehouses warehouse)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2360(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString, warehouse);
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

        //[pjadhav][02-01-2023][GEOS2-3530] 
        public List<ProductInspectionArticles> GetArticlesProductInspection(Warehouses warehouse)
        {
            try
            {

                return mgr.GetArticlesProductInspection(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][10.12.2022][GEOS2-3532]
        public List<PendingStorageArticles> GetArticlesPendingStorage_V2350(Warehouses warehouse)
        {
            try
            {

                return mgr.GetArticlesPendingStorage_V2350(warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WarehouseDeliveryNote GetLabelPrintDetails_V2350(Int32 IdArticle, Warehouses warehouse, Int64 IdWarehouseDeliveryNoteItem)
        {
            try
            {

                return mgr.GetLabelPrintDetails_V2350(IdArticle, warehouse, IdWarehouseDeliveryNoteItem);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-3531][17/01/2023]
        public List<EditProductInspectionArticles> GetArticlesProductInspection_V2350(Warehouses warehouse, long IdArticleWarehouseInspection)
        {
            try
            {
                return mgr.GetArticlesProductInspection_V2350(warehouse, IdArticleWarehouseInspection, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-3531][20/01/2023]
        public List<ProductInspectionImageInfo> GetProductInspectionImageInBytes(Warehouses warehouse, Int32 IdArticle)
        {
            List<ProductInspectionImageInfo> ProductInspectionImageInfoList = new List<ProductInspectionImageInfo>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                //ProductInspectionImageInfoList = mgr.GetProductInspectionImageByImagePath(warehouse, IdArticle, Properties.Settings.Default.ProductInspectionImage);
                ProductInspectionImageInfoList = mgr.GetProductInspectionImageByImagePath(warehouse, IdArticle, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ProductInspectionImageInfoList;
        }

        //[rdixit][30.01.2023][GEOS2-3605]
        public List<Offer> GetPendingWorkOrdersByWarehouse_V2360(Warehouses warehouse)
        {
            List<Offer> WorkOrdersList = new List<Offer>();
            try
            {
                WorkOrdersList = mgr.GetPendingWorkOrdersByWarehouse_V2360(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return WorkOrdersList;
        }

        //[rdixit][06.02.2023][GEOS2-3605]
        public List<OtItem> GetRemainingOtItemsByIdOt_V2360(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2360(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //[rdixit][06.02.2023][GEOS2-3605]
        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2360(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2360(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //[rdixit][07.02.2023][GEOS2-4132]
        public ArticleWarehouseLocations AddArticleWarehouseLocationByFullName_V2360(ArticleWarehouseLocations articleWarehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddArticleWarehouseLocationByFullName_V2360(connectionString, articleWarehouseLocation);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //Shubham[skadam] GEOS2-1512 Changes in article attachments to allow download them  07 02 2023
        public Article GetArticleDetailsByReference_V2360_New(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2360_New(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][07.02.2023][GEOS2-4134]
        public List<Article> GetWarehouseArticlesStockByWarehouse_V2360(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_V2360(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][07.02.2023][GEOS2-4134]
        public List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2360(Warehouses warehouse, Currency currency)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_WithPrices_V2360(warehouse, currency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][08.02.2023][GEOS2-4133]
        public List<WarehouseLocation> GetWarehouseLocationToPlaceArticle_V2360(Warehouses warehouse, string reference)
        {
            try
            {
                return mgr.GetWarehouseLocationToPlaceArticle_V2360(warehouse, reference);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][08.02.2023][GEOS2-4133]
        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2360(string IdArticles, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesWarehouseLocation_V2360(IdArticles, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][08.02.2023][GEOS2-3605]
        public bool UpdateArticleDetails_V2360_New(Article article, Warehouses warehouse)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2360_New(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4132][08.02.2023]
        public List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_Transfer(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseLocationsByIdWarehouse_Transfer(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-3531
        //Shubham[skadam] GEOS2-3531 [QUALITY_INSPECTION] Add Product Inspection Screen 13 02 2023
        public List<EditProductInspectionArticles> GetArticlesProductInspection_V2360(Warehouses warehouse, long IdArticleWarehouseInspection)
        {
            try
            {
                return mgr.GetArticlesProductInspection_V2360(warehouse, IdArticleWarehouseInspection, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.ArticleImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsInsertedProductInspection_V2360(Warehouses warehouse, string Comments, long IdArticleWarehouseInspection, List<EditProductInspectionArticles> ProductInspectionArticles, Int64 QuantityInspected)
        {
            try
            {
                return mgr.IsInsertedProductInspection_V2360(Comments, IdArticleWarehouseInspection, ProductInspectionArticles, warehouse.Company.ConnectPlantConstr, QuantityInspected);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][16-03-2023][GEOS2-4209] 
        public List<ProductInspectionArticles> GetAllArticlesProductInspection_V2370(Warehouses warehouse)
        {
            try
            {

                return mgr.GetAllArticlesProductInspection_V2370(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][16-03-2023][GEOS2-4148] 
        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2370(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2370(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
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

        //[rdixit][17-03-2023][GEOS2-4209] 
        public string GetArticle_Comment(Warehouses warehouse, int idArticle)
        {
            try
            {
                return mgr.GetArticle_Comment(warehouse, idArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //Shubham[skadam] GEOS2-3965 Add a NEW option in the Inventory Audit Edit/View screen in order to print a report 24 03 2023 
        public List<AuditedArticle> GetAllAuditedArticleForInventoryAudit(Int64 idWarehouseInventoryAudit)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllAuditedArticleForInventoryAudit(idWarehouseInventoryAudit, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil] [GEOS2-4251][28-03-2023] 
        public bool InsertIntoArticleStockForLocateMaterialForTransit(PendingStorageArticles pendingStorageArticles)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStockForLocateMaterialForTransit(connectionString, pendingStorageArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][18.05.2023][GEOS2-4411]
        public List<OtItem> GetRemainingOtItemsByIdOt_V2390(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2390(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //[rdixit][18.05.2023][GEOS2-4411]
        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2390(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2390(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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


        //[pramod.misal][GEOS2-4229][15/05/2023]
        public List<WarehouseQuota> GetWarehouseQuota_V2390(byte idCurrency)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseQuota_V2390(workbenchConnectionString, idCurrency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4496][25.05.2023]
        public Article GetArticleDetailsByReference_V2390_New(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2390_New(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4271][244/05/2023]

        public List<WarehouseWorklogReport> GetOTWorkLogTimesByPeriodAndSite_V2390(DateTime FromDate, DateTime ToDate, long IdSite, Warehouses warehouse)
        {
            try
            {
                return mgr.GetOTWorklogTimesByPeriodSite_V2390(FromDate, ToDate, IdSite, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4271][25/05/2023]
        public List<WarehouseWorkLogUser> GetWorkLogUserListByPeriodAndSite_V2390(DateTime FromDate, DateTime ToDate, long IdSite, Warehouses warehouse)
        {
            try
            {
                return mgr.GetWorkLogUserListByPeriodAndSite_V2390(FromDate, ToDate, IdSite, Properties.Settings.Default.UserProfileImage, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4271][25/05/2023]
        public List<WarehouseWorklogReport> GetWorkLogOTWithHoursAndUserByPeriodAndSite_V2390(DateTime FromDate, DateTime ToDate, long IdSite, Warehouses warehouse)
        {
            try
            {
                return mgr.GetWorkLogOTWithHoursAndUserByPeriodAndSite_V2390(FromDate, ToDate, IdSite, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<WarehouseInventoryWeek> GetInventoryWeek_V2390(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetInventoryWeek_V2390(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<ArticleMaterialType> GetArticleMaterialTypeStockAmountInWarehouse_V2390(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetArticleMaterialTypeStockAmountInWarehouse_V2390(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2390(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2390(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        //[pramod.misal][GEOS2-4229][26/05/2023]
        public bool UpdateWarehouseQuota_V2400(WarehouseQuota selectedAmount)
        {
            bool isUpdatedPlantquota = false;

            try
            {
                WarehouseManager mgr = new WarehouseManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdatedPlantquota = mgr.UpdateWarehouseQuota_V2400(selectedAmount, connectionString);
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

        //[Sudhir.jangra][GEOS2-4489][30/05/2023]
        public List<Warehouses> GetAllWarehousesByUserPermissionInSRM_V2400(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermissionInSRM_V2400(connectionString, idActiveUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4488][30/05/2023]
        public List<Warehouses> GetAllWarehousesByUserPermission_V2400(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermission_V2400(connectionString, idActiveUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-4422&GEOS2-4421
        //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
        //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
        public List<WOItem> GetRevisionItemPackingWorkOrders_V2400(Warehouses warehouse, Int32 idCompany, Int64? ProducerIdCountryGroup)
        {
            try
            {
                return mgr.GetRevisionItemPackingWorkOrders_V2400(warehouse, idCompany, Properties.Settings.Default.ArticleVisualAidsPath, ProducerIdCountryGroup);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<WOItem> GetPackedItemByIdPackingBox_V2400(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetPackedItemByIdPackingBox_V2400(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<PackingCompany> GetCompanyPackingWorkOrders_V2400(Warehouses warehouse, string siteIds)
        {
            try
            {
                return mgr.GetCompanyPackingWorkOrders_V2400(warehouse, siteIds);
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

        //[Sudhir.Jangra][GEOS2-4435][04/08/2023]
        public Article GetArticleDetailsByReference_V2420(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2420(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4435][04/08/2023]
        public bool UpdateArticleDetails_V2420(Article article, Warehouses warehouse)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2420(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-4227
        //Shubham[skadam] GEOS2-4227 New Inventory Dashboard 08 08 2023 
        public double GetArticleStockAmountInWarehouse_V2420(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear)
        {
            try
            {

                return mgr.GetArticleStockAmountInWarehouse_V2420(warehouse, idCurrency, accountingFromYear, accountingToYear);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        public double GetSleepedArticleStockAmountInWarehouse_V2420(Warehouses warehouse, byte idCurrency, DateTime accountingFromYear, DateTime accountingToYear, Int64 aritclesleepDays)
        {
            try
            {

                return mgr.GetSleepedArticleStockAmountInWarehouse_V2420(warehouse, idCurrency, accountingFromYear, accountingToYear, aritclesleepDays);
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

        //[Sudhir.Jangra][GEOS2-4539][10/08/2023]
        public Ots GetWorkOrderByIdOt_V2420(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetWorkOrderByIdOt_V2420(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4539][10/08/2023]
        public List<ItemOTStatusType> GetOtItemStatusList()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetOtItemStatusList(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4540][17/08/2023]
        public List<OtItem> GetRemainingOtItemsByIdOt_V2420(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2420(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //[Sudhir.Jangra][GEOS2-4540][17/08/2023]
        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2420(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2420(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //[Sudhir.Jangra][GEOS2-4541][21/08/2023]
        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2420(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2420(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4539][24/08/2023]
        public bool UpdateEditWorkOrderOTItems(List<OtItem> OtItems)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateEditWorkOrderOTItems(connectionString, OtItems);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //Shubham[skadam] GEOS2-4732 Too many errors of lock wait timeout exceeded in EWHQ (3/3) 28 08 2023
        public Article GetArticleDetailsByReference_V2430(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2430(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleVisualAidsPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][04.09.2023][GEOS2-4557]
        public List<Offer> GetPendingWorkOrdersByWarehouse_V2430(Warehouses warehouse)
        {
            List<Offer> WorkOrdersList = new List<Offer>();
            try
            {
                WorkOrdersList = mgr.GetPendingWorkOrdersByWarehouse_V2430(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return WorkOrdersList;
        }

        //[cpatil][04.09.2023][GEOS2-4414]
        public List<WarehouseBulkArticle> GetWarehouseBulkArticle(Warehouses warehouse)
        {
            List<WarehouseBulkArticle> warehouseBulkArticleList = new List<WarehouseBulkArticle>();
            try
            {
                warehouseBulkArticleList = mgr.GetWarehouseBulkArticle(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return warehouseBulkArticleList;
        }

        //[cpatil] [GEOS2-4414][04-09-2023]
        public WarehouseBulkArticle AddUpdateWarehouseBulkArticle(WarehouseBulkArticle warehouseBulkArticle)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdateWarehouseBulkArticle(mainConnectionString, warehouseBulkArticle);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4414]
        public List<BulkPicking> GetAllArticles()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetAllArticles(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4414]
        public WarehouseBulkArticle GetAllArticlesByIdBulkPicking(long IdWarehouseBulkPicking)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetAllArticlesByIdBulkPicking(connectionString, IdWarehouseBulkPicking);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][GEOS2-4416][06-09-2023]
        public List<OtItem> GetPendingMaterialArticles_V2430(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2430(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][GEOS2-4417][08/09/2023]
        public Ots GetWorkOrderByIdOt_V2430(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetWorkOrderByIdOt_V2430(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][GEOS2-4417][13/09/2023]
        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2430(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2430(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][21/09/2023]
        public List<OtItem> GetRemainingOtItemsByIdOt_V2430(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2430(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //[cpatil][21/09/2023]
        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2430(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2430(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        public bool UpdateOtItemFinishStatus(Int64 idOt, Int64 idWarehouse, OtItem parentOtItem, int keyId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
            string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
            try
            {
                return mgr.UpdateOtItemFinishStatus(connectionString, mainconnectionString, idOt, idWarehouse, parentOtItem, keyId);
            }
            catch (Exception ex)
            {
                return false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        //[Sudhir.Jangra][GEOS2-4544]
        public List<OtItem> GetRemainingOtItemsByIdOt_V2440(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2440(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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


        //[Sudhir.Jangra][GEOS2-4544]
        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2440(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2440(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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


        //[Sudhir.jangra][GEOS2-4543]
        /// <summary>
        /// This method is used to get warehouse delivery note details.
        /// </summary>
        /// <param name="idWarehouseDeliveryNote">The id warehouse delivery note.</param>
        /// <returns>The Warehouse Delivery Note.</returns>
        public WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2440(Int64 idWarehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseDeliveryNoteById_V2440(connectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.PurchaseOrdersPath, idWarehouseDeliveryNote);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4543]
        public bool IsUpdatedLockPropertyForDeliveryNote(Int64 IdWarehouseDeliveryNoteItem, bool IsLocked)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsUpdatedLockPropertyForDeliveryNote(connectionString, IdWarehouseDeliveryNoteItem, IsLocked);
            }
            catch (Exception ex)
            {
                return false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4860][25.10.2023]
        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2450(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2450(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
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
        /// This method is to read mail template details
        /// </summary>
        /// <param name="templateName">Get template name</param>
        /// <returns>Mail template</returns>
        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
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
        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        public bool IsEmailSendAfterPickingInventory(string title, string description, string mailTo, string mailFrom, byte[] AttachmentData)
        {
            try
            {
                return mgr.IsEmailSendAfterPickingInventory(title, description, mailTo, mailFrom, AttachmentData, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][GEOS2-4948][28/10/2023]
        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2450(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2450(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][GEOS2-4930][28/10/2023]
        public List<OtItem> GetRemainingOtItemsByIdOt_V2450(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2450(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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


        //[cpatil][GEOS2-4930][28/10/2023]
        public List<OtItem> GetRemainingOtItemsByIdOtDisbaledFIFO_V2450(Int64 idOt, Warehouses warehouse)
        {
            List<OtItem> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2450(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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


        //[Sudhir.Jangra][GEOS2-4859]
        public List<ArticlesStock> GetFinancialIncomeOutcome_V2450(Warehouses warehouse, byte idCurrency, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetFinancialIncomeOutcome_V2450(warehouse, idCurrency, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rajashri][GEOS2-4849][28.10.2023]
        public bool UpdateWorkflowStatusInOT_V2450(Int64 IdOT, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByOT> LogEntriesByOTList)
        {
            bool isupdated = false;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isupdated = mgr.UpdateWorkflowStatusInOT_V2450(connectionString, IdOT, IdWorkflowStatus, IdUser, LogEntriesByOTList);
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
        //[rdixit][GEOS2-4948][31.10.2023]
        public Ots GetWorkOrderByIdOt_V2450(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkOrderByIdOt_V2450(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-4437 Ask for location inventory after do the picking of certain references (3/3) 27 10 2023
        public bool IsEmailSendAfterPickingInventory_V2450(string title, string emailTemplate, string mailTo, string mailFrom, byte[] AttachmentData)
        {
            try
            {
                return mgr.IsEmailSendAfterPickingInventory_V2450(title, emailTemplate, mailTo, mailFrom, AttachmentData, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4859]
        public List<ArticlesStock> GetFinancialIncomeOutcome_V2460(Warehouses warehouse, byte idCurrency, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetFinancialIncomeOutcome_V2460(warehouse, idCurrency, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri GEOS2-4966 28-11-2023
        public Article GetArticleDetailsByReference_V2460(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2460(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Ots GetWorkOrderByIdOt_V2460(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkOrderByIdOt_V2460(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateOtItemsStatusToFinish(List<OtItem> ItemsToFinishStatus)
        {
            bool isupdated = false;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isupdated = mgr.UpdateOtItemsStatusToFinish(connectionString, ItemsToFinishStatus);
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


        //[pramod.misal][GEOS2-5093][15-12-2023]
        public List<Ots> GetNotPacking_V2470(Warehouses warehouse)
        {
            try
            {
                return mgr.GetNotPacking_V2470(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5094][20-12-2023]
        public bool UpdateDeliveryDateofTodoOts_V2470(Dictionary<Int64, DateTime?> UpdatedTodoOts)
        {
            bool isupdated = false;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                isupdated = mgr.UpdateDeliveryDateofTodoOts_V2470(connectionString, UpdatedTodoOts);
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

        //[pramod.misal][GEOS2-5069][29-12-2023]
        public WarehouseInventoryAudit GetAllItemsForSelectedWarehouseInventoryAudit_V2470(Warehouses warehouse, Int64 idWarehouseInventoryAudit)
        {
            try
            {
                return mgr.GetAllItemsForSelectedWarehouseInventoryAudit_V2470(warehouse, idWarehouseInventoryAudit);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-4543]
        /// <summary>
        /// This method is used to get warehouse delivery note details.
        /// </summary>
        /// <param name="idWarehouseDeliveryNote">The id warehouse delivery note.</param>
        /// <returns>The Warehouse Delivery Note.</returns>
        //Shubham[skadam] GEOS2-5226 NO SE PUEDE DESBLOQUEAR UN ALBARÁN DE LA REFERENCIA 02MOTTRONIC 12 01 2024
        public WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2480(Int64 idWarehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseDeliveryNoteById_V2480(connectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.PurchaseOrdersPath, idWarehouseDeliveryNote);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ExpiredArticleWarningsCleanUp> ExpiredArticleWarningsCleanUp_V2480(DateTime StartDate)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.ExpiredArticleWarningsCleanUp_V2480(mainConnectionString, LocalGeosContext, StartDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool CleanUpExpiredArticleWarningsByIdOfferAndIdWarehouse(ExpiredArticleWarningsCleanUp ExpiredArticleWarningsCleanUp)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string LocalGeosContext = ConfigurationManager.ConnectionStrings["LocalGeosContext"].ConnectionString;
                return mgr.CleanUpExpiredArticleWarningsByIdOfferAndIdWarehouse(mainConnectionString, LocalGeosContext, ExpiredArticleWarningsCleanUp);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //rajashri GEOS2-4966 28-11-2023
        //Shubham[skadam] GEOS2-5016 Changes in Type of Article 20 02 2024
        public Article GetArticleDetailsByReference_V2490(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2490(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4435][04/08/2023]
        //Shubham[skadam] GEOS2-5016 Changes in Type of Article 20 02 2024
        public bool UpdateArticleDetails_V2490(Article article, Warehouses warehouse)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2490(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][GEOS2-5299][27/02/2024]
        public List<Warehouses> GetAllWarehousesByUserPermission_V2490(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermission_V2490(connectionString, idActiveUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][GEOS2-5299][27/02/2024]
        public List<Warehouses> GetAllWarehousesByUserPermissionInSRM_V2490(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermissionInSRM_V2490(connectionString, idActiveUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5421][rdixit][12.03.2024]
        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2500(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2500(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.jangra][GEOS2-5373]
        public List<Ots> GetWorkOrdersPreparationReport_V2500(Warehouses warehouse, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetWorkOrdersPreparationReport_V2500(warehouse,
                    fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][GEOS2-4930][28/10/2023]
        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
        public List<WMSItemScan> GetRemainingOtItemsByIdOt_V2500(Int64 idOt, Warehouses warehouse)
        {
            List<WMSItemScan> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2500(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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
        public bool InsertIntoArticleStock_V2500(WMSPickingMaterials pickingMaterials)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertIntoArticleStock_V2500(connectionString, pickingMaterials);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][GEOS2-4930][28/10/2023]
        //Shubham[skadam] GEOS2-5405 WMS scan action is too slow 20 03 2024
        public List<WMSItemScan> GetRemainingOtItemsByIdOtDisbaledFIFO_V2500(Int64 idOt, Warehouses warehouse)
        {
            List<WMSItemScan> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2500(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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

        //rajashri GEOS2-4966 28-11-2023
        //Shubham[skadam] GEOS2-5016 Changes in Type of Article 20 02 2024
        //Shubham[skadam]GEOS2-5344 Sleep day difference 04 04 2024
        public Article GetArticleDetailsByReference_V2500(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2500(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri GEOS2-5455 12-04-2024
        public Ots GetWorkOrderByIdOt_V2510(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkOrderByIdOt_V2510(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5486]
        public List<StockBySupplier> GetStockBySupplier_V2510(Warehouses warehouse, Currency currency)
        {
            try
            {
                return mgr.GetStockBySupplier_V2510(warehouse, currency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5456]
        public bool IsLockPropertyForDeliveryNoteUpdated_V2510(List<WarehouseDeliveryNoteItem> deliveryNotes)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.IsLockPropertyForDeliveryNoteUpdated_V2510(connectionString, deliveryNotes);
            }
            catch (Exception ex)
            {
                return false;
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri GEOS2-5433
        public Article GetArticleDetailsByReference_V2510(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2510(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri GEOS2-5433
        public bool UpdateArticleDetails_V2510(Article article, Warehouses warehouse)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2510(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5457]
        public WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2510(Int64 idWarehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseDeliveryNoteById_V2510(connectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.PurchaseOrdersPath, idWarehouseDeliveryNote);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri GEOS2-5434
        public List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2510(Warehouses warehouse, Currency currency)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_WithPrices_V2510(warehouse, currency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<Article> GetWarehouseArticlesStockByWarehouse_V2510(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_V2510(warehouse);
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
        public Tuple<int, Int64> GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(int idArticle, int idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleStockByWarehouseAndGetArticleLockedStockByWarehouse(connectionString, idArticle, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri GEOS2-5487
        public List<Tuple<string, string, string>> GetReadyForShippingOTItems_V2520(Warehouses warehouse, string idOts, Int64 idWarehouseForSiteDL)
        {
            try
            {
                return mgr.GetReadyForShippingOTItems_V2520(warehouse, idOts, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.CommandTimeout, idWarehouseForSiteDL);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[rushikesh.gaikwad] [GEOS2-5488] [16.05.2024]
        public List<WarehouseLocation> GetWarehouseLocationsByIdWarehouse_V2520(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseLocationsByIdWarehouse_V2520(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rushikesh.gaikwad] [GEOS2-5488] [16.05.2024]
        public List<InventoryAuditArticle> GetAllArticlesByWarehouseLocation_V2520(Int64 idWarehouse, Int64 IdWarehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllArticlesByWarehouseLocation_V2520(idWarehouse, IdWarehouseLocation, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5524][17.05.2024]
        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2520(string IdArticles, Warehouses warehouse, Int64 qty)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesWarehouseLocation_V2520(IdArticles, warehouse, qty);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][GEOS2-5513][17.05.2024]
        public string GetMandatoryStageNameOpenIfExist(Int64 partNumber, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetMandatoryStageNameOpenIfExist(partNumber, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<InventoryMaterial> GetInventoryArticleByIdWarehouseLocation_V2520(Warehouses warehouse, Int64 idWarehouseLocation)
        {
            List<InventoryMaterial> inventoryMaterials = null;

            try
            {

                inventoryMaterials = mgr.GetInventoryArticleByIdWarehouseLocation_V2520(warehouse, idWarehouseLocation, Properties.Settings.Default.ArticleVisualAidsPath);
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


        //[Sudhir.Jangra][GEOS2-5644]
        public bool DeleteWorkLog_V2530(Company company, Int64 idOTWorkingTime)
        {
            try
            {
                return mgr.DeleteWorkLog_V2530(company, idOTWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5644]
        public bool UpdateWorkLog(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                return mgr.UpdateWorkLog(company, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5644]
        public List<OTAssignedUser> GetOTAssignedUsers(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetOTAssignedUsers(company, idOT, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        #region GEOS2-4422&GEOS2-4421&GEOS2-5784
        //Shubham[skadam]  GEOS2-4421 Do not mix different carriage methods in a box (1/2) 19 06 2023
        //Shubham[skadam]  GEOS2-4422 Do not mix different carriage methods in a box (2/2) 19 06 2023
        //Shubham[skadam] GEOS2-5784 Expedition bug improvement  20 06 2024
        public List<WOItem> GetRevisionItemPackingWorkOrders_V2530(Warehouses warehouse, Int32 idCompany, Int64? ProducerIdCountryGroup)
        {
            try
            {
                return mgr.GetRevisionItemPackingWorkOrders_V2530(warehouse, idCompany, Properties.Settings.Default.ArticleVisualAidsPath, ProducerIdCountryGroup);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<WOItem> GetPackedItemByIdPackingBox_V2530(Warehouses warehouse, Int64 idPackingBox)
        {
            try
            {
                return mgr.GetPackedItemByIdPackingBox_V2530(warehouse, Properties.Settings.Default.ArticleVisualAidsPath, idPackingBox);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<PackingCompany> GetCompanyPackingWorkOrders_V2530(Warehouses warehouse, string siteIds, string workOrder)
        {
            try
            {
                return mgr.GetCompanyPackingWorkOrders_V2530(warehouse, siteIds, workOrder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<WOItem> GetPackingWorkOrders_V2530(Warehouses warehouse, string workOrder)
        {
            try
            {
                return mgr.GetPackingWorkOrders_V2530(warehouse, workOrder, Properties.Settings.Default.ArticleVisualAidsPath);
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

        // Rahul[rgadhave] GEOS2-5865 Not show PO which having type offer.(1732)[21/06/2024]
        public List<WarehousePurchaseOrder> GetPurchaseOrdersPendingReceptionByWarehouse_V2530(Warehouses warehouse)
        {
            try
            {

                return mgr.GetPurchaseOrdersPendingReceptionByWarehouse_V2530(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateArticleDetails_V2530(Article article, Warehouses warehouse)
        {
            try
            {
                string mainConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateArticleDetails_V2530(mainConnectionString, article, Properties.Settings.Default.ArticleVisualAidsPath, connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5904][02.07.2024]
        public List<Ots> GetWorkOrdersPreparationReport_V2540(Warehouses warehouse, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return mgr.GetWorkOrdersPreparationReport_V2540(warehouse,
                    fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public List<Article> GetWarehouseArticlesStockByWarehouse_WithPrices_V2540(Warehouses warehouse, Currency currency)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_WithPrices_V2540(warehouse, currency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public List<WMSItemScan> GetRemainingOtItemsByIdOt_V2540(Int64 idOt, Warehouses warehouse)
        {
            List<WMSItemScan> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOt_V2540(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public Ots GetWorkOrderByIdOt_V2540(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkOrderByIdOt_V2540(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public List<Article> GetWarehouseArticlesStockByWarehouse_V2540(Warehouses warehouse)
        {
            try
            {
                return mgr.GetWarehouseArticlesStockByWarehouse_V2540(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public List<Ots> GetPendingMaterialWorkOrdersByWarehouse_V2540(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialWorkOrdersByWarehouse_V2540(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public List<WMSItemScan> GetRemainingOtItemsByIdOtDisbaledFIFO_V2540(Int64 idOt, Warehouses warehouse)
        {
            List<WMSItemScan> serialNumbers = null;

            try
            {
                serialNumbers = mgr.GetRemainingOtItemsByIdOtDisbaledFIFO_V2540(idOt, warehouse, Properties.Settings.Default.ArticleVisualAidsPath);
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
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public List<ArticleType> GetArticleTypes_V2540(Warehouses warehouse)
        {
            try
            {
                return mgr.GetArticleTypes_V2540(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rahul.gadhave] [GEOS2-5676] [16-07-2024]
        public List<Ots> GetPackingWorkOrdersByWarehouse_V2540(Warehouses warehouse)
        {
            List<Ots> ots = null;

            try
            {

                ots = mgr.GetPackingWorkOrdersByWarehouse_V2540(warehouse, Properties.Settings.Default.CountryFilePath);
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

        //[cpatil][GEOS2-4416][06-09-2023]
        public List<OtItem> GetPendingMaterialArticles_V2550(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingMaterialArticles_V2550(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.jangra][GEOS2-6050]

        public Ots GetWorkOrderByIdOt_V2550(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWorkOrderByIdOt_V2550(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6050]
        public List<OTWorkingTime> GetOTWorkingTimeDetails_V2550(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                return mgr.GetOTWorkingTimeDetails_V2550(idOT, warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool AddOTWorkingTime_V2550(Warehouses warehouse, OTWorkingTime otWorkingTime)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();

                return mgr.AddOTWorkingTime_V2550(warehouse, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public bool AddCancelledOTWorkingTime_V2550(Warehouses warehouse, OTWorkingTime otWorkingTime)
        {
            try
            {
                WarehouseManager mgr = new WarehouseManager();

                return mgr.AddCancelledOTWorkingTime_V2550(warehouse, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //[GEOS2-5906][rdixit][13.11.2024]
        public List<ProductInspectionReworkCauses> GetReworkCausesbyArticleCategory(Int64 IdArticleCategory)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetReworkCausesbyArticleCategory(IdArticleCategory, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5906][rdixit][13.11.2024]
        public List<ProductInspectionReworkCauses> GetAllReworkCauses()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllReworkCauses(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5906][rdixit][13.11.2024]
        public bool IsDeletedReworkCauseForArticleCategory_V2580(ProductInspectionReworkCauses rework)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.IsDeletedReworkCauseForArticleCategory_V2580(rework, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5906][rdixit][13.11.2024]
        public bool AddUpdateReworkListByArticleCategory_V2580(List<ProductInspectionReworkCauses> reworkListByArticleCategory)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddUpdateReworkListByArticleCategory_V2580(reworkListByArticleCategory, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5906][rdixit][13.11.2024]
        public ProductInspectionReworkCauses AddEditNewReworkCause_V2580(ProductInspectionReworkCauses newReworkCause)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddEditNewReworkCause_V2580(newReworkCause, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][08.02.2023][GEOS2-4133]
        //Shubham[skadam] GEOS2-5992 Improvements in the filling criteria developed in Ticket IESD-96777 22 11 2024.
        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2580(string IdArticles, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesWarehouseLocation_V2580(IdArticles, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5524][17.05.2024]
        //Shubham[skadam] GEOS2-5992 Improvements in the filling criteria developed in Ticket IESD-96777 22 11 2024.
        public List<ArticleWarehouseLocations> GetArticlesWarehouseLocation_V2580_New(string IdArticles, Warehouses warehouse, Int64 qty)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticlesWarehouseLocation_V2580_New(IdArticles, warehouse, qty);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][12-12-2024][GEOS2-6382] 
        public bool UpdateWarehouseInventoryAudit_V2590(Warehouses warehouse, WarehouseInventoryAudit WarehouseInventoryAudit)
        {
            try
            {
                return mgr.UpdateWarehouseInventoryAudit_V2590(warehouse, WarehouseInventoryAudit);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][12-12-2024][GEOS2-6382] 
        public List<InventoryAuditArticle> GetAllArticlesByWarehouseLocations(Int64 idWarehouse, string IdWarehouseLocation)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllArticlesByWarehouseLocations(idWarehouse, IdWarehouseLocation, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][16-12-2024][GEOS2-6382] 
        public bool AddUpdateWarehouseInventoryAuditItems_V2590(Warehouses warehouse, List<WarehouseInventoryAuditItem> warehouseInventoryAuditItems)
        {
            try
            {
                return mgr.AddUpdateWarehouseInventoryAuditItems_V2590(warehouse, warehouseInventoryAuditItems);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][01-04-2025][GEOS2-7015]
        public LocalWarehouseStock GetLocalWarehouseStockReport(long idWarehouse, bool isRegional)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetLocalWarehouseStockReport(connectionString, idWarehouse, isRegional);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][01-04-2025][GEOS2-7015]
        public Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails(long idCompany)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetWarehouseStockReportEmails(connectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region 
        public List<SendNotificationMail> GetPikingNotification_V2630(Warehouses warehouse)
        {
            try
            {
                //   string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                return mgr.GetPikingNotification_V2630(warehouse);


            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool SendNotificationMail(string warehouse, string mailTo, string itemReference, string expectedQty, string actualQty, string picker)
        {

            try
            {
                //   string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                HRMMail mailServer = new HRMMail();
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                mailServer.MailFrom = Properties.Settings.Default.MailFrom;
                mailServer.MailTo = mailTo;
                return mgr.SendNotificationMail(warehouse, mailServer, itemReference, expectedQty, actualQty, picker);


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


        //[Sudhir.Jangra][GEOS2-6490]
        public Article GetArticleDetailsByReference_V2630(string reference, Warehouses warehouse, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return mgr.GetArticleDetailsByReference_V2630(reference, Properties.Settings.Default.ArticleVisualAidsPath,
                warehouse, Properties.Settings.Default.CountryFilePath, fromDate, toDate, Properties.Settings.Default.ArticleAttachmentDocPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][01-04-2025][GEOS2-7015]
        public List<Warehouses> GetWarehousesForLocalStockReport()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehousesForLocalStockReport(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][15-04-2025][GEOS2-7016]
        public LocalWarehouseChartData GetWarehouseStockForGlobalReport(long idWarehouse, bool isRegional)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseStockForGlobalReport(connectionString, idWarehouse, isRegional);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][15-04-2025][GEOS2-7016]
        public List<Warehouses> GetWarehousesForGlobalStockReport()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehousesForGlobalStockReport(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][17-04-2025][GEOS2-7016]
        public Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetGlobalWarehouseStockReportEmails(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region[rani dhamankar][16-04-2025][GEOS2-7021]
        public ItemPickerDetails GetPickerNameBYDeliveryNoteItem_V2630(long IdWarehouseDeliveryNoteItem, Warehouses warehouse)
        {
            ItemPickerDetails PickerDetails = null;

            try
            {
                PickerDetails = mgr.GetPickerNameBYDeliveryNoteItem_V2630(IdWarehouseDeliveryNoteItem, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return PickerDetails;
        }
        #endregion

        #region[pallavi.jadhav][9 5 2025][GEOS2-6823]
        public WarehouseLocation AddWarehouseLocation_V2640(WarehouseLocation warehouseLocation, List<RangeItems> RangeOfLocationResult)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddWarehouseLocation_V2640(connectionString, warehouseLocation, RangeOfLocationResult);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public WarehouseLocation UpdateWarehouseLocation_V2640(WarehouseLocation warehouseLocation , List<RangeItems> RangeOfLocationResult)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateWarehouseLocation_V2640(connectionString, warehouseLocation, RangeOfLocationResult);
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

        //[nsatpute][11.07.2025][GEOS2-8837] 
        public Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails_V2660(long idCompany)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetWarehouseStockReportEmails_V2660(connectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][11.07.2025][GEOS2-8837] 
        public Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails_V2660()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetGlobalWarehouseStockReportEmails_V2660(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][29.08.2025][GEOS2-6505]
		public List<Article> GetArticlesStockByWarehouse_V2670(Warehouses warehouse, Currency currency)
        {
            try
            {
                return mgr.GetArticlesStockByWarehouse_V2670(warehouse, currency);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

		//[nsatpute][29.08.2025][GEOS2-6505]
		public void UpdateArticleMinMaxStock_V2670(Warehouses warehouse, List<Article> modifiedArticles)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                mgr.UpdateArticleMinMaxStock_V2670(connectionString, warehouse, modifiedArticles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][05.09.2025][GEOS2-9210]
        public LocalWarehouseStock GetLocalWarehouseStockReport_V2670(long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetLocalWarehouseStockReport_V2670(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][05.09.2025][GEOS2-9210]
        public LocalWarehouseChartData GetWarehouseStockForGlobalReport_V2670(long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseStockForGlobalReport_V2670(connectionString, idWarehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][12.09.2025][GEOS2-9181]
        public Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails_V2670()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetGlobalWarehouseStockReportEmails_V2670(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][12.09.2025][GEOS2-9181]
        public Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails_V2670(long idCompany)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetWarehouseStockReportEmails_V2670(connectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        // [cpatil][15.09.2025][GEOS2-8200]
        public List<RegionCountryWarehouse> GetRegionCountrySiteWarehouseData(Int32 idUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetRegionCountrySiteWarehouseData(connectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		
		// [nsatpute][18.09.2025][GEOS2-9210]
        public bool InsertLocalWarehouseStock(long idWarehouse, int monthNumber)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.InsertLocalWarehouseStock(connectionString, idWarehouse, monthNumber);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][22.09.2025][GEOS2-8793]
        public List<long> GetWarehouseScheduleYears()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseScheduleYears(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][22.09.2025][GEOS2-8795]
        public bool SaveWarehouseScheduleEvent(ScheduleEvent scheduleEvent)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.SaveWarehouseScheduleEvent(connectionString, scheduleEvent);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][25.09.2025][GEOS2-8797]
        public List<ScheduleEvent> GetWarehouseScheduleEventsByIdWarehouse(long idWarehouse, long year)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseScheduleEventsByIdWarehouse(connectionString, idWarehouse, year);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][16.10.2025][GEOS2-8799]
        public bool UpdateWarehouseScheduleEvent(ScheduleEvent scheduleEvent)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWarehouseScheduleEvent(connectionString, scheduleEvent);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][16.10.2025][GEOS2-8799]
        public bool DeleteWarehouseScheduleEvent(ScheduleEvent scheduleEvent)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.DeleteWarehouseScheduleEvent(connectionString, scheduleEvent);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
    
        //[nsatpute][04.11.2025][GEOS2-8805]
        public bool CreatePreordersForWarehouse(Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.CreatePreordersForWarehouse(connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][04.11.2025][GEOS2-8805]
        public List<Warehouses> GetAllWarehouseForScheduleEvents()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehouseForScheduleEvents(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][07-11-2025][GEOS2-10097]
        public Dictionary<string, List<string>> WMS_GetWarehouseStockReportEmails_V2680(Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetWarehouseStockReportEmails_V2680(connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][07-11-2025][GEOS2-10097]
        public Dictionary<string, List<string>> WMS_GetGlobalWarehouseStockReportEmails_V2680(Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.WMS_GetGlobalWarehouseStockReportEmails_V2680(connectionString, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][11-11-2025][GEOS2-8805]
        public List<string> GetJobFailureNotificationEmails(string key)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetJobFailureNotificationEmails(connectionString, key);
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
        public List<WarehousePurchaseOrder> GetPurchaseOrdersByRegionalWarehouse_V2680(long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPurchaseOrdersByRegionalWarehouse_V2680(idWarehouse, connectionString, Properties.Settings.Default.PurchaseOrdersPath);
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

        //[rdixit][GEOS2-9141][02.08.2025]
        public int GetNextNumberOfOfferFromCounters(string siteCode, int type)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetNextNumberOfOfferFromCounters(siteCode, type, connectionString);
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
        public Int64 InsertOffer_V2680(Quotation quotation, CustomerPurchaseOrder custPurOrder)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.InsertOffer_V2680(quotation, custPurOrder, connectionString, workbenchConnectionString, Properties.Settings.Default.WorkingOrdersPath, Properties.Settings.Default.CommericalPath);
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
        public bool CheckIfOfferExists(CustomerPurchaseOrder custPurOrder)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.CheckIfOfferExists(custPurOrder, connectionString);
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
        public void UpdateNextOfferNumberToCounters(int idOfferType)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.UpdateNextOfferNumberToCounters(connectionString, idOfferType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][18.11.2025][GEOS2-9364]

        public List<TransportFrequency> GetSiteTransportFrequencies()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
               return mgr.GetSiteTransportFrequencies(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][19.11.2025][GEOS2-9371]
        public void SaveSiteTransportFrequencies(List<TransportFrequency> transportFrequencies)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.SaveSiteTransportFrequencies(connectionString, transportFrequencies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][21.11.2025][GEOS2-9367]
        public List<LogEntriesByTransportFrequency> GetLogEntriesByTransportFrequency()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
               return mgr.GetLogEntriesByTransportFrequency(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][21.11.2025][GEOS2-9367]
        public void SaveLogEntrieByTransportFrequency(List<LogEntriesByTransportFrequency> logEntries)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.SaveLogEntrieByTransportFrequency(connectionString, logEntries);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool CheckCustomerPurchasOrderExists(string POCode)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.CheckCustomerPurchasOrderExists(connectionString, POCode);
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
