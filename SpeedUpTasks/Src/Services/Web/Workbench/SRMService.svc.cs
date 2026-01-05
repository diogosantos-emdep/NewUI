using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SRMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SRMService.svc or SRMService.svc.cs at the Solution Explorer and start debugging.
    public class SRMService : ISRMService
    {
        SRMManager mgr = new SRMManager();

        /// <summary>
        /// Get pending PO by warehouse.
        /// </summary>
        /// <param name="warehouse">The Warehouse which contains connection string.</param>
        /// <returns>The list of pending PO.</returns>
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingPurchaseOrdersByWarehouse(warehouse);
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
        /// Get purchase order pdf
        /// </summary>
        /// <param name="AttachPDF">attach pdf path.</param>
        /// <returns>Get purchase order pdf.</returns>
        public byte[] GetPurchaseOrderPdf(string AttachPDF)
        {
            try
            {
                return mgr.GetPurchaseOrderPdf(Properties.Settings.Default.PurchaseOrdersPath, AttachPDF);
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
        /// Get Pending PO By IdWarehousePurchaseOrder
        /// </summary>
        /// <param name="idWarehousePurchaseOrder">get warehouse PO id.</param>
        /// <param name="purchaseOrdersPath">get PO path.</param>
        /// <returns>Get Pending PO By IdWarehousePurchaseOrder</returns>
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("CrmContext") == false)
                {
                    exp.ErrorMessage = "CrmContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return warehousePurchaseOrder;
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowStatus> GetAllWorkflowStatus()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowStatus(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowTransition> GetAllWorkflowTransitions()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowTransitions(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to Update Workflow Status In PO
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdWarehousePurchaseOrder">Get IdWarehousePurchaseOrder.</param>
        /// <param name="IdWorkflowStatus">Get IdWorkflowStatus.</param>
        /// <param name="LogEntriesByWarehousePOList">Get Log entry or comment list details.</param>
        public bool UpdateWorkflowStatusInPO(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus,int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInPO(connectionString, IdWarehousePurchaseOrder, IdWorkflowStatus, IdUser, LogEntriesByWarehousePOList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerContext") == false)
                {
                    exp.ErrorMessage = "MainServerContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// Get Article Suppliers By Warehouse
        /// </summary>
        /// <param name="warehouse">The warehouse for connection string</param>
        /// <returns>List of Article Suppliers By Warehouse</returns>
        public List<ArticleSupplier> GetArticleSuppliersByWarehouse(Warehouses warehouse)
        {
            try
            {
                return mgr.GetArticleSuppliersByWarehouse(warehouse);
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
        /// This method is used to Delete Article Supplier
        /// </summary>
        /// <param name="idArticleSupplier">Get idArticleSupplier.</param>
        public bool DeleteArticleSupplier(Int64 idArticleSupplier, int IdUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.DeleteArticleSupplier(connectionString, idArticleSupplier, IdUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("CrmContext") == false)
                {
                    exp.ErrorMessage = "CrmContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool SendSupplierPurchaseOrderRequestMail(WarehousePurchaseOrder warehousePurchaseOrder, string EmailFrom)
        {
            try
            {
                return mgr.SendSupplierPurchaseOrderRequestMail(warehousePurchaseOrder, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.PurchaseOrdersPath, EmailFrom);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ArticleSupplier GetArticleSupplierByIdArticleSupplier(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier(warehouse, IdArticleSupplier);
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
