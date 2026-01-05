using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
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
        public bool UpdateWorkflowStatusInPO(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList)
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


        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2110(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2110(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2110(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2110(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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


        public List<Article> GetPendingArticles(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingArticles(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<User> GetPermissionUsers()
        {
            try
            {
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPermissionUsers(connectionWorkbenchString);
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


        public List<KeyValuePair<string, string>> GetBodyAndUpdateReminderDateInPO(Warehouses warehouse, List<WarehousePurchaseOrder> warehousePurchaseOrderList, Dictionary<string, List<long>> supplierEmailId, int IdModifiedBy)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetBodyAndUpdateReminderDateInPO(warehouse, MainServerConnectionString, warehousePurchaseOrderList, supplierEmailId, Properties.Settings.Default.EmailTemplate, IdModifiedBy);
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
                else if (mgr.IsConnectionStringNameExist("CrmContext") == false)
                {
                    exp.ErrorMessage = "CrmContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<KeyValuePair<string, string>> GetBodyAndUpdateReminderDateInPO_V2650(Warehouses warehouse, List<WarehousePurchaseOrder> warehousePurchaseOrderList, Dictionary<string, List<long>> supplierEmailId, int IdModifiedBy)
        {
            try
            {
                string MainServerConnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetBodyAndUpdateReminderDateInPO_V2650(warehouse, MainServerConnectionString, warehousePurchaseOrderList, supplierEmailId, Properties.Settings.Default.EmailTemplate, IdModifiedBy);
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
                else if (mgr.IsConnectionStringNameExist("CrmContext") == false)
                {
                    exp.ErrorMessage = "CrmContext - connection string name not found.";
                    exp.ErrorCode = "000091";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public bool UpdateWorkflowStatusInPO_V2110(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInPO_V2110(connectionString, IdWarehousePurchaseOrder, IdWorkflowStatus, IdUser, LogEntriesByWarehousePOList, IdAssignee, IdApprover);
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

        //GEOS_3434


        //GEOS_3434



        public Contacts AddContact(Contacts contacts)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddContact(connectionString, contacts);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public Contacts GetContacts(int idContact)
        {
            Contacts Contact = new Contacts();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Contact = mgr.GetContacts(connectionString, idContact);
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
            return Contact;
        }

        public List<Contacts> GetAllContacts()
        {
            List<Contacts> allContactList = new List<Contacts>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                allContactList = mgr.GetAllContacts(connectionString);
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
            return allContactList;
        }

        public List<Contacts> GetContactsByIdArticleSupplier(int idArticleSupplie)
        {
            List<Contacts> contactsList = new List<Contacts>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                contactsList = mgr.GetContactsByIdArticleSupplier(connectionString, idArticleSupplie);
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

            return contactsList;
        }
        //PRM
        public void ArticleSupplierContacts_Insert(int idContact, Int64 idArticleSupplier)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                mgr.ArticleSupplierContacts_Insert(connectionString, idContact, idArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2240(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier_V2240(warehouse, IdArticleSupplier);
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
        /// Get Article Suppliers By Warehouse
        /// </summary>
        /// <param name="warehouse">The warehouse for connection string</param>
        /// <returns>List of Article Suppliers By Warehouse</returns>
        public List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2250(Warehouses warehouse)
        {
            try
            {
                return mgr.GetArticleSuppliersByWarehouse_V2250(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2250(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier_V2250(warehouse, IdArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2301(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier_V2301(warehouse, IdArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LogEntriesByArticleSuppliers> GetLogEntriesByArticleSuppliers(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetLogEntriesByArticleSuppliers(warehouse, IdArticleSupplier);
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
        /// This method is used to Delete Contacts
        /// </summary>
        /// <param name="idContact">Get idContact.</param>
        public bool DeleteContacts(int idContact)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.DeleteContacts(connectionString, idContact);
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
        public void ArticleSupplierContacts_Insert_V2250(int idContact, Int64 idArticleSupplier, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                mgr.ArticleSupplierContacts_Insert_V2250(connectionString, idContact, idArticleSupplier, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public Contacts AddContact_V2250(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddContact_V2250(connectionString, contacts, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public bool SetIsMainContact(int idContact, Int64 IdArticleSupplier)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.SetIsMainContact(connectionString, idContact, IdArticleSupplier);
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



        public void AddCommentsOrLogEntriesByArticleSuppliers(List<LogEntriesByArticleSuppliers> AddLogEntries)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                mgr.AddCommentsOrLogEntriesByArticleSuppliers(connectionString, AddLogEntries);
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

        public Contacts GetContacts_V2250(int idContact)
        {
            Contacts Contact = new Contacts();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Contact = mgr.GetContacts_V2250(connectionString, idContact);
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
            return Contact;
        }

        public List<Contacts> GetAllContacts_V2250()
        {
            List<Contacts> allContactList = new List<Contacts>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                allContactList = mgr.GetAllContacts_V2250(connectionString);
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
            return allContactList;
        }

        public bool SetIsMainContact_V2300(int idContact, Int64 IdArticleSupplier)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.SetIsMainContact(connectionString, idContact, IdArticleSupplier);
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

        public bool DeleteContacts_V2300(int idContact)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteContacts(connectionString, idContact);
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

        public void AddCommentsOrLogEntriesByArticleSuppliers_V2300(List<LogEntriesByArticleSuppliers> AddLogEntries)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.AddCommentsOrLogEntriesByArticleSuppliers(connectionString, AddLogEntries);
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

        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2300(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2300(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        public Contacts AddContact_V2300(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddContact_V2250(connectionString, contacts, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public void ArticleSupplierContacts_Insert_V2300(int idContact, long idArticleSupplier)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.ArticleSupplierContacts_Insert(connectionString, idContact, idArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Country> GetCountries_V2301()
        {
            List<Country> countries = null;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetCountries_V2301(connectionString);
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

        public List<Group> GetGroups_V2301()
        {
            List<Group> group = null;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                group = mgr.GetGroups_V2301(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return group;
        }
        public List<Categorys> GetCategorys_V2301()
        {
            List<Categorys> categ = null;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                categ = mgr.GetCategorys_V2301(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return categ;
        }

        public List<PaymentTerm> GetPayments_V2301()
        {
            List<PaymentTerm> pay = null;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                pay = mgr.GetPayments_V2301(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return pay;
        }

        public bool UpdateArticleSupplier(ArticleSupplier articleSupplier)
        {
            bool isUpdated = false;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateArticleSupplier(connectionString, articleSupplier);
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

        public List<WorkflowStatus> GetWorkFlowStatus()
        {
            List<WorkflowStatus> workFlowStatusLst = null;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                workFlowStatusLst = mgr.GetWorkFlowStatus(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return workFlowStatusLst;
        }


        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2301(Warehouses warehouse)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2301(warehouse, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public WarehousePurchaseOrder GetPendingPODetailsForGridUpdate(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPODetailsForGridUpdate(connectionString, idWarehousePurchaseOrder);
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


        public bool IsUpdateWarehousePurchaseOrderWithStatus(WarehousePurchaseOrder warehousePurchaseOrder)
        {
            bool isUpdated = false;
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                isUpdated = mgr.IsUpdateWarehousePurchaseOrderWithStatus(connectionString, warehousePurchaseOrder);
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
            return isUpdated;
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowStatus> GetAllWorkflowStatus_V2301()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowStatus_V2301(connectionString);
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
        public List<WorkflowTransition> GetAllWorkflowTransitions_V2301()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowTransitions_V2301(connectionString);
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

        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2310(Warehouses warehouse)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2310(warehouse, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4027][rdixit][18.11.2022]
        public ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2340(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier_V2340(warehouse, IdArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4027][rdixit][18.11.2022]
        public List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2340(Warehouses warehouse)
        {
            try
            {
                return mgr.GetArticleSuppliersByWarehouse_V2340(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-4027][rdixit][18.11.2022]
        public bool UpdateArticleSupplier_V2340(ArticleSupplier articleSupplier)
        {
            bool isUpdated = false;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateArticleSupplier_V2340(connectionString, articleSupplier);
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
        //[GEOS2-3441][sshegaonkar][24.01.2023]
        public ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2360(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier_V2360(warehouse, IdArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-3442][cpatil][20.03.2023]
        public ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2370(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier_V2370(warehouse, IdArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3442][cpatil][20.03.2023]
        public bool UpdateArticleSupplier_V2370(ArticleSupplier articleSupplier)
        {
            bool isUpdated = false;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateArticleSupplier_V2370(connectionString, articleSupplier);
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

        //[GEOS2-4310][rdixit][05.04.2023]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2380(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2380(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //[GEOS2-4309][rdixit][10.04.2023]
        public List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2380(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked)
        {
            List<WarehousePurchaseOrder> warehousePurchaseOrder = new List<WarehousePurchaseOrder>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.SendPOMailForSelectedPO_V2380(connectionString, ListPurchaseOrder_Checked, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate);
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

        //[rdixit][GEOS2-4313][17.04.2023]
        public bool UpdateWorkflowStatusInPO_V2380(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInPO_V2380(connectionString, IdWarehousePurchaseOrder, IdWorkflowStatus, IdUser, LogEntriesByWarehousePOList, IdAssignee, IdApprover);
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

        //[rdixit][GEOS2-4313][17.04.2023]
        public bool UpdateAssigneeAndApproverInPO_V2380(WarehousePurchaseOrder warehousePurchaseOrder)
        {
            bool isUpdated = false;
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                isUpdated = mgr.UpdateAssigneeAndApproverInPO_V2380(connectionString, warehousePurchaseOrder);
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
            return isUpdated;
        }

        //[Sudhir.Jangra][GEOS2-4401][02/05/2023]
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2390(long idWarehouse,string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2390(idWarehouse,connectionstring, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4402][03/05/2023]
        public List<Article> GetPendingArticles_V2390(long idWarehouse,string connectionString)
        {
            try
            {
                return mgr.GetPendingArticles_V2390(idWarehouse,connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4403][03/05/2023]
        public List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2390(long idWarehouse,string connectionString)
        {
            try
            {
                return mgr.GetArticleSuppliersByWarehouse_V2390(idWarehouse,connectionString, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4407][04/05/2023]
        public List<GeosAppSetting> GetSystemSettings_V2390()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSystemSettings_V2390(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4407][09/05/2023]
        public bool UpdateSystemSettings_V2390(string defaultValue)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                
                return mgr.UpdateSystemSettings_V2390(defaultValue, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException(); exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("MainServerWorkbenchContext") == false)
                {
                    exp.ErrorMessage = "MainServerWorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000101";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4407][19/05/2023]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2390(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2390(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //[Sudhir.jangra][GEOS2-4407]
        public List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2390(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked)
        {
            List<WarehousePurchaseOrder> warehousePurchaseOrder = new List<WarehousePurchaseOrder>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.SendPOMailForSelectedPO_V2390(connectionString, ListPurchaseOrder_Checked, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate);
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

        //[Sudhir.Jangra][GEOS2-4487][31/05/2023]
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2400(long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2400(idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[pramod.misal][GEOS2-4431][16/06/2023]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2400(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2400(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //Shubham[skadam]  GEOS2-4404 (View Supplier) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        //Shubham[skadam]  GEOS2-4405 (Grid) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        public bool UpdatePurchasingOrderStatus(Int64 idWarehousePurchaseOrder, Int64 IdUser)
        {
            bool IsUpdate = false;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                IsUpdate = mgr.UpdatePurchasingOrderStatus(idWarehousePurchaseOrder, IdUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return IsUpdate;
        }
        //Shubham[skadam]  GEOS2-4404 (View Supplier) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        //Shubham[skadam]  GEOS2-4405 (Grid) Set the PO status automatically to “Sent” when sending successfully the Email 21 06 2023
        public Int32 GetPurchasingOrderStatus(Int64 idWarehousePurchaseOrder)
        {
            Int32 IdWorkflowStatus = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                IdWorkflowStatus = mgr.GetPurchasingOrderStatus(idWarehousePurchaseOrder, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return IdWorkflowStatus;
        }

        public POEmailNotification GetPurchasingOrderNotificationDetails(Int64 idWarehousePurchaseOrder, Int64 IdUser)
        {
            POEmailNotification POEmailNotification = new POEmailNotification();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                POEmailNotification = mgr.GetPurchasingOrderNotificationDetails(idWarehousePurchaseOrder, IdUser, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return POEmailNotification;
        }
        public List<WorkflowTransition> GetAllWorkflowTransitions_V2400()
        {
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllWorkflowTransitions_V2400(connectionString);
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

        public bool PurchasingOrderNotificationSend(POEmailNotification POEmailNotification)
        {
            try
            {
                string emailTemplate = System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, "SupplierPurchaseOrderStatusChangeMailFormat.html"));
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.PurchasingOrderNotificationSend(POEmailNotification, emailTemplate,"SRM-noreply@emdep.com",Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort);
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

        //[rdixit][28.06.2023]
        public bool SendEmailForPO(string emailto, string sub, string emailbody, Dictionary<string, byte[]> attachments, string EmailFrom, List<string> ccAddress)
        {
            try
            {
                return mgr.SendEmailForPO(emailto, sub, emailbody, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, attachments, EmailFrom, ccAddress);
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


        //[pramod.misal][GEOS2-4448][30-06-2023]
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2410(long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2410(idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

          //[pramod.misal][GEOS2-4449][04-06-2023]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2410(Int64 idWarehousePurchaseOrder)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2410(connectionString, idWarehousePurchaseOrder, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //[GEOS2-4453][21.07.2023][rdixit]
        public WarehousePurchaseOrder GetOfferExportChart(int idPo, int idWarehouse)
        {
            WarehousePurchaseOrder IsCreated;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                IsCreated = mgr.GetOfferExportChart(idPo, idWarehouse, connectionString, Properties.Settings.Default.ExportPurchaseOrderTemplatePath);
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

            return IsCreated;
        }
        //[GEOS2-4453][21.07.2023][rdixit]
        public bool ExportPurchaseOrderReport(WarehousePurchaseOrder po, byte[] pdffile, byte[] excelfile)
        {
            try
            {
                string updatedFilePath = Properties.Settings.Default.PurchaseOrdersPath;
                if (!updatedFilePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    updatedFilePath += Path.DirectorySeparatorChar;
                }
                updatedFilePath = updatedFilePath + po.CreatedIn.Year + @"\" + po.Code + @"\01 Purchase Order\";            
                if (!System.IO.Directory.Exists(updatedFilePath))
                {
                    System.IO.Directory.CreateDirectory(updatedFilePath);
                }
                //Excel File
                using (FileStream ExcelFileStream = new FileStream(Path.Combine(updatedFilePath, "PO_" + po.Code + ".xlsx"), FileMode.Create))
                {
                    ExcelFileStream.Write(excelfile, 0, excelfile.Length);
                }
                //Pdf File
                using (FileStream pdfFileStream = new FileStream(Path.Combine(updatedFilePath, "PO_" + po.Code + ".pdf"), FileMode.Create))
                {
                    pdfFileStream.Write(pdffile, 0, pdffile.Length);
                }

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();       
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return true;
        }


        //[pramod.misal][GEOS2-4451][21/06/2023]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2420(Int64 idWarehousePurchaseOrder,string idShippingAddress)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2420(connectionString, idWarehousePurchaseOrder, idShippingAddress, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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
        //Shubham[skadam]  GEOS2-4713 Missing Delivery Dates 31 07 2023 
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2420(long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2420(idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-4450][21/06/2023]
        public bool UpdateAssigneeAndApproverInPO_V2420(WarehousePurchaseOrder warehousePurchaseOrder)
        {
            bool isUpdated = false;
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                isUpdated = mgr.UpdateAssigneeAndApproverInPO_V2420(connectionString, warehousePurchaseOrder);
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
            return isUpdated;
        }

        //[pramod.misal][GEOS2-4755][16/08/2023]
        public bool UpdateWorkflowStatusInPO_V2420(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInPO_V2420(connectionString, IdWarehousePurchaseOrder, IdWorkflowStatus, IdUser, LogEntriesByWarehousePOList, IdAssignee, IdApprover);
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

        //[pramod.misal][GEOS2-4674][22-08-2023]
        public Contacts AddContact_V2430(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddContact_V2430(connectionString, contacts, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //[pramod.misal][GEOS2-4675][22-08-2023]
        public Contacts GetContacts_V2430(int idContact)
        {
            Contacts Contact = new Contacts();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Contact = mgr.GetContacts_V2430(connectionString, idContact);
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
            return Contact;
        }


        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public bool DisableContact(People people)
        {
            bool isDeleted = false;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isDeleted = mgr.DisableContact(people, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public List<People> GetContactsByIdPermission_V2390(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2390(connectionString, idActiveUser, idUser, idSite, idPermission);
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

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public List<Country> GetAllCountriesDetails()
        {
            List<Country> Countries = new List<Country>();

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Countries = mgr.GetAllCountriesDetails(connectionString, Properties.Settings.Default.CountryFilePath);

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

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public List<Company> GetAllCustomerCompanies()
        {
            List<Company> companies = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetAllCustomerCompanies(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public People GetContactsByIdPerson(Int32 idPerson)
        {
            People peopleContact = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peopleContact = mgr.GetContactsByIdPerson(idPerson, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peopleContact;
        }

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public List<Offer> GetOffersByIdSiteToLinkedWithActivities(byte idCurrency, Int32 idUser, Int64 accountingYear, Company company, Int32 idUserPermission, Int32 idSite)
        {
            List<Offer> offers = null;

            try
            {
                SRMManager mgr = new SRMManager();
                offers = mgr.GetOffersByIdSiteToLinkedWithActivities(idCurrency, idUser, accountingYear, company, idUserPermission, idSite);
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
        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public List<Activity> GetActivitiesByIdSite(Int32 idSite)
        {
            List<Activity> activities = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdSite(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public List<LogEntryBySite> GetAllLogEntriesByIdSite(Int64 idSite)
        {
            List<LogEntryBySite> logEntries = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntries = mgr.GetAllLogEntriesByIdSite(idSite, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntries;
        }

        //[sudhir.Jangra][GEOS2-4676][28/08/2023]
        public List<Country> GetCountries()
        {
            List<Country> countries = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                countries = mgr.GetCountries(connectionString);
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

        //[sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<Customer> GetSelectedUserCompanyGroup_V2420(string idUser, bool isIncludeDefault = false)
        {
            List<Customer> customers = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetSelectedUserCompanyGroup_V2420(connectionString, idUser, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<Customer> GetCompanyGroup_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Customer> customers = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                customers = mgr.GetCompanyGroup_V2420(connectionString, idUser, idZone, idUserPermission, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return customers;
        }


        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<Company> GetSelectedUserCompanyPlantByIdUser_V2420(string idUser, bool isIncludeDefault = false)
        {
            List<Company> companies = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetSelectedUserCompanyPlantByIdUser_V2420(connectionString, idUser, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }
        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<Company> GetCompanyPlantByUserId_V2420(Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Company> companies = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                companies = mgr.GetCompanyPlantByUserId_V2420(connectionString, idUser, idZone, idUserPermission, isIncludeDefault);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<People> GetSalesOwnerBySiteId_V2390(Int32 idSite)
        {
            List<People> peoples = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSalesOwnerBySiteId_V2390(connectionString, idSite);
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

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<People> GetSiteContacts(Int64 idSite)
        {
            List<People> peoples = null;
            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetSiteContacts(connectionString, idSite);
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

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public Company UpdateCompany_V2430(Company company)
        {
            Company addedCompany = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string mainconnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                string crmconnectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                addedCompany = mgr.UpdateCompany_V2430(company, mainconnectionString, crmconnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedCompany;
        }

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<People> GetAllActivePeoples_V2390()
        {
            List<People> ActivePeoples = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                ActivePeoples = mgr.GetAllActivePeoples_V2390(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ActivePeoples;
        }

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public Activity GetActivityByIdActivity_V2035(Int64 idActivity)
        {
            Activity Activity = new Activity();

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Activity = mgr.GetActivityByIdActivity_V2035(idActivity, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Activity;
        }

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public Company GetCompanyDetailsById_V2340(Int32 idSite)
        {
            Company company = new Company();

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                company = mgr.GetCompanyDetailsById_V2340(connectionString, idSite);
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

        //[Sudhir.jangra][GEOS2-4676][29/08/2023]
        public List<Competitor> GetCompetitors()
        {
            List<Competitor> competitors = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                competitors = mgr.GetCompetitors(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return competitors;
        }

        //[sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<Activity> GetActivitiesByIdContact(Int32 idPerson)
        {
            List<Activity> activities = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetActivitiesByIdContact(idPerson, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public List<LogEntriesByContact> GetLogEntriesByContact(Int32 idContact, byte idLogEntryType)
        {
            List<LogEntriesByContact> logEntriesByContact = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                logEntriesByContact = mgr.GetLogEntriesByContact(idContact, idLogEntryType, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntriesByContact;
        }

        //[sudhir.jangra][GEOS2-4676][29/08/2023]
        public bool UpdateContact(People people)
        {
            bool isUpdated = false;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateContact(connectionString, people);
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

        //[sudhir.jangra][GEOS2-4676][29/08/2023]
        public People AddContact_V2033(People people)
        {
            People addedPeople = new People();

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                addedPeople = mgr.AddContact_V2033(connectionString, people);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedPeople;
        }

        //[Sudhir.Jangra][GEOS2-4676][29/08/2023]
        public People AddSRMContact_V2430(People people)
        {
            People addedPeople = new People();

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                addedPeople = mgr.AddSRMContact_V2430(connectionString, people);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return addedPeople;
        }

        //[Sudhir.Jangra][GEOS2-4676]
        public List<People> GetContactsByIdPermission_V2430(Int32 idActiveUser, string idUser, string idSite, Int32 idPermission)
        {
            List<People> peoples = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                peoples = mgr.GetContactsByIdPermission_V2430(connectionString, idActiveUser, idUser, idSite, idPermission);
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
        //[sudhir.jangra][GEOS2-4676][29/08/2023]
        public bool UpdateContact_V2430(People people)
        {
            bool isUpdated = false;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.UpdateContact_V2430(connectionString, people);
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

        // [rdixit][GEOS2-4822][13-09-2023]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2430(Int64 idWarehousePurchaseOrder, string idShippingAddress)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2430(connectionString, idWarehousePurchaseOrder, idShippingAddress, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //[Sudhir.Jangra][GEOS2-4676]
        public List<ArticleSupplier> GetArticleSuppliersForSRMContact_V2430(Warehouses warehouse)
        {
            try
            {
                return mgr.GetArticleSuppliersForSRMContact_V2430(warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4676]
        public List<Contacts> GetContactsAndSupplier_V2430(Warehouses warehouse)
        {
            try
            {
                return mgr.GetContactsAndSupplier_V2430(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4676][28/08/2023]
        public bool DeleteContactDetails_V2340(Contacts contact)
        {
            bool isDeleted = false;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isDeleted = mgr.DeleteContactDetails_V2340(contact, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;
        }

        //[Sudhir.Jangra][GEOS2-4676]
        public bool ArticleSupplierContacts_Update_V2430(Int32 IdArticleSupplierContact, long IdArticleSupplier)
        {
            bool isUpdated = false;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                isUpdated = mgr.ArticleSupplierContacts_Update_V2430(connectionString, IdArticleSupplierContact, IdArticleSupplier);
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

        //[Sudhir.Jangra][GEOS2-4740][26/09/2023]
        public List<ArticleSupplier> GetArticleSuppliersByWarehouse_V2440(long idWarehouse, string connectionString)
        {
            try
            {
                return mgr.GetArticleSuppliersByWarehouse_V2440(idWarehouse, connectionString, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-4738][27/09/2023]
        public ArticleSupplier GetSRMEmdepCodeForAddSupplier()
        {
            try
            {
               
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetSRMEmdepCodeForAddSupplier(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4738][27/09/2023]
        public Contacts AddContact_V2440(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddContact_V2440(connectionString, contacts, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //[Sudhir.Jangra][GEOS2-4738]
        public bool AddArticleSupplier_V2440(ArticleSupplier articleSupplier,List<Contacts> contactsList,List<Warehouses> warehouses)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddArticleSupplier_V2440(connectionString, articleSupplier, contactsList, warehouses);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-4738]
        public ArticleSupplier GetArticleSupplierByIdArticleSupplier_V2440(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            try
            {
                return mgr.GetArticleSupplierByIdArticleSupplier_V2440(warehouse, IdArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4589][09-10-2023]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2440(Int64 idWarehousePurchaseOrder, string idShippingAddress)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2440(connectionString, idWarehousePurchaseOrder, idShippingAddress, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public List<LogEntriesByArticleSuppliers> GetArticleSuppliersContactsComments_V2450(int idContact)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetArticleSuppliersContactsComments_V2450(connectionString, idContact);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public List<LogEntriesByArticleSuppliers> GetArticleSuppliersContactsChangelog_V2450(int idContact)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.GetArticleSuppliersContactsChangelog_V2450(connectionString, idContact);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[chitra.girigosavi][GEOS2-4692][17.10.2023]
        public Contacts AddContact_V2450(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddContact_V2450(connectionString, contacts, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [rdixit][19.10.2023][GEOS2-4961]
        public bool AddArticleSupplier_V2450(ArticleSupplier articleSupplier, List<Contacts> contactsList, List<Warehouses> warehouses)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddArticleSupplier_V2450(connectionString, articleSupplier, contactsList, warehouses);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil]  GEOS2-4902
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2450(long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2450(idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //Shubham[skadam] GEOS2-5026 Log not added properly in purchase order 08 11 2023
        public bool IsUpdateWarehousePurchaseOrderWithStatus_V2450(WarehousePurchaseOrder warehousePurchaseOrder)
        {
            bool isUpdated = false;
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                isUpdated = mgr.IsUpdateWarehousePurchaseOrderWithStatus_V2450(connectionString, warehousePurchaseOrder);
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
            return isUpdated;
        }

        //Shubham[skadam] GEOS2-4965 General discount row is not added in the PO 24 11 2023
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2460(Int64 idWarehousePurchaseOrder, string idShippingAddress)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2460(connectionString, idWarehousePurchaseOrder, idShippingAddress, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //[pramod.misal][GEOS2-4450][21/06/2023]
        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
        public bool UpdateAssigneeAndApproverInPO_V2480(WarehousePurchaseOrder warehousePurchaseOrder)
        {
            bool isUpdated = false;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;

                isUpdated = mgr.UpdateAssigneeAndApproverInPO_V2480(connectionString, warehousePurchaseOrder);
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
            return isUpdated;
        }

        //[chitra.girigosavi][GEOS2-4692][17.10.2023]
        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
        public Contacts AddContact_V2480(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddContact_V2480(connectionString, contacts, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5136][22.01.2024]
        public void ArticleSupplierContacts_Insert_V2480(int idContact, long idArticleSupplier)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.ArticleSupplierContacts_Insert_V2480(connectionString, idContact, idArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5136][23.01.2024]
        public bool SetIsMainContact_V2480(int idContact, Int64 IdArticleSupplier)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.SetIsMainContact_V2480(connectionString, idContact, IdArticleSupplier);
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
        //rajashriGEOS2-5375
        public List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2500(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked)
        {
            List<WarehousePurchaseOrder> warehousePurchaseOrder = new List<WarehousePurchaseOrder>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.SendPOMailForSelectedPO_V2500(connectionString, ListPurchaseOrder_Checked, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate);
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
        public List<WarehousePurchaseOrder> GetWarehouseHolidays(DateTime? deliverydate,long idWarehouse, long IdWarehousePurchaseOrder)
        {
            List<WarehousePurchaseOrder> warehousePurchaseOrder = new List<WarehousePurchaseOrder>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetWarehouseHolidays(connectionString, deliverydate, idWarehouse, IdWarehousePurchaseOrder);
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
        //rajashriGEOS2-5376
        public List<Warehouses> GetCustomMessageforHoliday_V2500()
        {
            List<Warehouses> warehousePurchaseOrder = new List<Warehouses>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetCustomMessageforHoliday_V2500(connectionString);
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
        public bool UpdateCustomMessageForPORequestMail_V2500(List<Warehouses> warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateCustomMessageForPORequestMail_V2500(connectionString,warehouse);
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
        public List<Warehouses> GetCustomMessageforHoliday_PORequestMail_V2500(long? idWarehouse)
        {
            List<Warehouses> warehousePurchaseOrder = new List<Warehouses>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetCustomMessageforHoliday_PORequestMail_V2500(connectionString, idWarehouse);
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
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2500(Int64 idWarehousePurchaseOrder, string idShippingAddress)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2500(connectionString, idWarehousePurchaseOrder, idShippingAddress, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        #region
        public bool UpdatePoflag(WarehousePurchaseOrder selectedPurchaseOrder)
        {
            bool IsUpdate = false;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                IsUpdate = mgr.UpdatePoflag(connectionString, selectedPurchaseOrder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return IsUpdate;
        }
        #endregion


        //[Sudhir.Jangra][GEOS2-5491]
        public bool DeleteArticleSupplierOrder_V2510(UInt32 idArticleSupplierPOReceiver)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteArticleSupplierOrder_V2510(connectionString, idArticleSupplierPOReceiver);
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

        //[Sudhir.Jangra][GEOS2-5491]
        public bool AddDeleteArticleSupplierOrder_V2510(List<Contacts> contactList, long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeleteArticleSupplierOrder_V2510(connectionString, contactList, idWarehouse);
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

        //[Sudhir.Jangra][GEOS2-5491]
        public List<Contacts> GetArticleSuppliersOrders_V2510(Int32 idArticleSupplier, long idWarehouse)
        {
            List<Contacts> contactsList = new List<Contacts>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                contactsList = mgr.GetArticleSuppliersOrders_V2510(connectionString, idArticleSupplier, idWarehouse);
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

            return contactsList;
        }


        //[cpatil]  GEOS2-5618
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2510(long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2510(idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri [GEOS2-5461]
        public List<WarehousePurchaseOrder> GetCompletedPurchaseOrdersByWarehouse_V2520(DateTime startDate, DateTime endDate, long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompletedPurchaseOrdersByWarehouse_V2520(startDate, endDate,idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region //chitra.girigosavi ON [2/04/2024] GEOS2-5406 Changes in Supplier PO Management(2/3)
        public WarehousePurchaseOrder GetOfferExportChart_V2520(int idPo, int idWarehouse)
        {
            WarehousePurchaseOrder IsCreated;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                IsCreated = mgr.GetOfferExportChart_V2520(idPo, idWarehouse, connectionString, Properties.Settings.Default.ExportPurchaseOrderTemplatePath);
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

            return IsCreated;
        }
        #endregion
        //rajashri 5762
        public List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2520(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked)
        {
            List<WarehousePurchaseOrder> warehousePurchaseOrder = new List<WarehousePurchaseOrder>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.SendPOMailForSelectedPO_V2520(connectionString, ListPurchaseOrder_Checked, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate);
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
        public List<WarehousePurchaseOrder> GetWarehouseHolidays_V2520(DateTime? deliverydate, long idWarehouse, long IdWarehousePurchaseOrder)
        {
            List<WarehousePurchaseOrder> warehousePurchaseOrder = new List<WarehousePurchaseOrder>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetWarehouseHolidays_V2520(connectionString, deliverydate, idWarehouse, IdWarehousePurchaseOrder);
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
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2520(Int64 idWarehousePurchaseOrder, string idShippingAddress)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2520(connectionString, idWarehousePurchaseOrder, idShippingAddress, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath);
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

        //[rushikesh.gaikwad]
        public bool AddDeleteArticleSupplierOrder_V2520(List<Contacts> contactList, long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddDeleteArticleSupplierOrder_V2520(connectionString, contactList, idWarehouse);
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

        //[rushikesh.gaikwad]
        public List<Contacts> GetArticleSuppliersOrders_V2520(Int32 idArticleSupplier, long idWarehouse)
        {
            List<Contacts> contactsList = new List<Contacts>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                contactsList = mgr.GetArticleSuppliersOrders_V2520(connectionString, idArticleSupplier, idWarehouse);
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

            return contactsList;
        }


        //[Sudhir.Jangra][GEOS2-5827]
        public bool AddArticleSupplier_V2530(ArticleSupplier articleSupplier, List<Contacts> contactsList, List<Warehouses> warehouses)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddArticleSupplier_V2530(connectionString, articleSupplier, contactsList, warehouses);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-4755][16/08/2023]
        public bool UpdateWorkflowStatusInPO_V2530(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList, UInt32 IdAssignee, UInt32 IdApprover, string comment)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.UpdateWorkflowStatusInPO_V2530(connectionString, IdWarehousePurchaseOrder, IdWorkflowStatus, IdUser, LogEntriesByWarehousePOList, IdAssignee, IdApprover, comment);
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
        // [nsatpute][12-06-2024] GEOS2-5463
        public List<WarehouseCategory> GetWarehouseCategories()
        {
            List<WarehouseCategory> articleCategories = null;

            try
            {
                SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                articleCategories = mgr.GetWarehouseCategories(connectionString);
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
            return articleCategories;
        }
		// [nsatpute][12-06-2024] GEOS2-5463
        public List<Catalogue> GetCatalogue(string connectionString, string reference, string articleSupplierIds, string idCategories, string conditionalOperator, string stockValue, long idWarehouse, Currency currency)
        {
            List<Catalogue> cataloguelist = new List<Catalogue>();
            try
            {
                cataloguelist = mgr.GetCatalogue(connectionString, Properties.Settings.Default.ArticleVisualAidsPath, reference, articleSupplierIds, idCategories, conditionalOperator, stockValue, idWarehouse, currency);
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

            return cataloguelist;
        }
		// [nsatpute][12-06-2024] GEOS2-5463
        public List<ArticleSupplier> GetArticleSuppliersByWarehouseForCatalogue(long idWarehouse, string connectionString)
        {
            try
            {
                return mgr.GetArticleSuppliersByWarehouse_V2440(idWarehouse, connectionString, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [Rahul.Gadhave][17-06-2024] [GEOS2-2530]
        public bool DeleteContacts_V2530(int idContact)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteContacts_V2530(connectionString, idContact);
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


        //[Sudhir.Jangra][GEOS2-5634]
        public List<Ots> GetPendingReviewByWarehouse_V2540(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingReviewByWarehouse_V2540(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5634]
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

        //[Sudhir.Jangra][GEOS2-5634]
        public Ots GetPendingReviewByIdOt_V2540(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPendingReviewByIdOt_V2540(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5634]
        public List<OTWorkingTime> GetOTWorkingTimeDetails_V2540(Int64 idOT, Warehouses warehouse)
        {
            try
            {
                return mgr.GetOTWorkingTimeDetails_V2540(idOT, warehouse, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5634]
        public List<WorkflowTransition> GetAllWorkflowTransitions_V2540()
        {
            List<WorkflowTransition> lstWorkflowStatus = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                lstWorkflowStatus = mgr.GetAllWorkflowTransitions_V2540(connectionString);
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


        //[Sudhir.Jangra][GEOS2-5634]
        public List<WorkflowStatus> GetAllWorkflowStatus_V2540()
        {
            List<WorkflowStatus> lstWorkflowStatus = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                lstWorkflowStatus = mgr.GetAllWorkflowStatus_V2540(connectionString);
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


        //[Sudhir.Jangra][GEOS2-5635]
        public Article GetArticleDetails_V2540(Int32 idArticle)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleDetails_V2540(idArticle, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5635]
        public WarehouseDeliveryNote GetWarehouseDeliveryNoteById_V2540(Int64 idWarehouseDeliveryNote)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetWarehouseDeliveryNoteById_V2540(connectionString, Properties.Settings.Default.ArticleVisualAidsPath, Properties.Settings.Default.PurchaseOrdersPath, idWarehouseDeliveryNote);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-5635]
        public bool UpdateWorkLog_V2540(Company company, OTWorkingTime otWorkingTime)
        {
            try
            {
                return mgr.UpdateWorkLog_V2540(company, otWorkingTime);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-5635]
        public bool UpdateEditWorkOrderOTItems_V2540(List<OtItem> Ots, List<SRMOtItemsComment> changeLog)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateEditWorkOrderOTItems_V2540(connectionString, mainServerConnectionString, Ots, changeLog);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }




        //[Sudhir.Jangra][GEOS2-5635]
        public List<OTAssignedUser> GetOTAssignedUsers_V2540(Company company, Int64 idOT)
        {
            try
            {
                return mgr.GetOTAssignedUsers_V2540(company, idOT, Properties.Settings.Default.UserProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-5636]
        public List<Ots> GetPendingReviewListForArticle_V2540(Int64 idOt, Int32 idArticle, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPendingReviewListForArticle_V2540(idOt, idArticle, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5636]
        public bool UpdateEditWorkOrderOTItemsForArticles_V2540(Ots Ots, List<SRMOtItemsComment> changeLog)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string mainServerConnectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.UpdateEditWorkOrderOTItemsForArticles_V2540(connectionString, mainServerConnectionString, Ots, changeLog);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6049]
        public Ots GetPendingReviewByIdOt_V2550(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPendingReviewByIdOt_V2550(idOt, warehouse);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-6049]
        public List<Ots> GetPendingReviewByWarehouse_V2550(Warehouses warehouse)
        {
            try
            {
                return mgr.GetPendingReviewByWarehouse_V2550(warehouse, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5636]
        public Article GetArticleDetails_V2550(Int32 idArticle,Int64 IdSite)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetArticleDetails_V2550(idArticle, IdSite, Properties.Settings.Default.ArticleVisualAidsPath, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][02.09.2024][GEOS2-6383]
        public bool PurchasingOrderNotificationSend_V2560(POEmailNotification POEmailNotification)
        {
            try
            {
                string emailTemplate = System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, "GEOSAPISupplierPurchaseOrderStatusChangeMailFormat.html"));
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.PurchasingOrderNotificationSend_V2560(POEmailNotification, emailTemplate, "SRM-noreply@emdep.com", Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.EmailTemplate);
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
        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        public List<ArticleSupplier> GetArticleSupplierContactsByIdContact_V2560(Warehouses warehouse, Int32 IdContact)
        {
            try
            {
                return mgr.GetArticleSupplierContactsByIdContact_V2560(warehouse, IdContact);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        public Contacts AddContact_V2560(Contacts contacts, List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.AddContact_V2560(connectionString, contacts, ArticleSuppliersChangeLogList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5136][22.01.2024]
        public void ArticleSupplierContacts_Insert_V2560(int idContact, long idArticleSupplier)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                mgr.ArticleSupplierContacts_Insert_V2560(connectionString, idContact, idArticleSupplier);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][13-01-2025][GEOS2-6443]
        public Ots GetPendingReviewByIdOt_V2600(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string currenciesImagesPath = Properties.Settings.Default.CurrenciesImages;
                return mgr.GetPendingReviewByIdOt_V2600(idOt, warehouse, currenciesImagesPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][21-01-2025][GEOS2-5725]
        public List<Article> GetAllArticles()
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

        // [nsatpute][23-01-2025][GEOS2-5725]
        public List<ArticleSupplier> GetAllSuppliersForPurchasingReport()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllSuppliersForPurchasingReport(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][23-01-2025][GEOS2-5725]
        public List<ArticlePurchasing> GetPurchasingReport(DateTime fromDate, DateTime toDate, long idArticleSupplier, int idArticle, long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPurchasingReport(fromDate, toDate, idArticleSupplier, idArticle , idWarehouse, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][26-02-2025][GEOS2-7034]
        public Ots GetPendingReviewByIdOt_V2620(Int64 idOt, Warehouses warehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                string currenciesImagesPath = Properties.Settings.Default.CurrenciesImages;
                return mgr.GetPendingReviewByIdOt_V2620(idOt, warehouse, currenciesImagesPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
         // [pallavi.kale][04-03-2025][GEOS2-7012]
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2620(long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2620(idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath,Properties.Settings.Default.CurrenciesImages, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [pallavi.kale][04-03-2025][GEOS2-7012]
        //Shubham[skadam] GEOS2-8262 PO reminder Mail send to whom 29 05 2025
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse_V2650(long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPendingPurchaseOrdersByWarehouse_V2650(idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, Properties.Settings.Default.CurrenciesImages, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [pallavi.kale][04-03-2025][GEOS2-7013]
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder_V2620(Int64 idWarehousePurchaseOrder, string idShippingAddress)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.GetPendingPOByIdWarehousePurchaseOrder_V2620(connectionString, idWarehousePurchaseOrder, idShippingAddress, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate, Properties.Settings.Default.CountryFilePath, Properties.Settings.Default.CurrenciesImages);
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
        
        // [pallavi.kale][04-03-2025][GEOS2-7012]
        public List<WarehousePurchaseOrder> GetCompletedPurchaseOrdersByWarehouse_V2620(DateTime startDate, DateTime endDate, long idWarehouse, string connectionstring)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompletedPurchaseOrdersByWarehouse_V2620(startDate, endDate, idWarehouse, connectionstring, Properties.Settings.Default.CountryFilePath, Properties.Settings.Default.CurrenciesImages,workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-7979][rdixit][16.07.2025][All connected tasks]
        public List<PreOrder> GetAllPreOrder_V2660(Warehouses warehouse, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllPreOrder_V2660(connectionString,warehouse, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-7979][rdixit][16.07.2025][All connected tasks]
        public int GetPOIdByCode(Warehouses warehouse, string poCode)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPOIdByCode(connectionString,warehouse, poCode);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.gadhave][GEOS2-7243][Date:17-07-2025]
        public List<WarehousePurchaseOrder> SendPOMailForSelectedPO_V2660(List<WarehousePurchaseOrder> ListPurchaseOrder_Checked)
        {
            List<WarehousePurchaseOrder> warehousePurchaseOrder = new List<WarehousePurchaseOrder>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                warehousePurchaseOrder = mgr.SendPOMailForSelectedPO_V2660(connectionString, ListPurchaseOrder_Checked, Properties.Settings.Default.PurchaseOrdersPath, Properties.Settings.Default.EmailTemplate);
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

        //[rdixit][GEOS2-8252][16.10.2025]
        public List<string> GetExistingPreOrderCodes_V2680(string code)
        {
            List<string> preOderCode = new List<string>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                preOderCode = mgr.GetExistingPreOrderCodes_V2680(connectionString, code);
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
            return preOderCode;
        }

        //[rdixit][GEOS2-8252][16.10.2025]
        public List<Warehouses> GetAllWarehousesByUserPermissionInSRM_V2680(Int32 idActiveUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllWarehousesByUserPermissionInSRM_V2680(connectionString, idActiveUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-8252][16.10.2025]
        public PreOrder AddPreOrder_V2680(PreOrder preOrder)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.AddPreOrder_V2680(connectionString, preOrder);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-8252][16.10.2025]
        public List<PreOrder> GetAllPreOrder_V2680(Warehouses warehouse, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetAllPreOrder_V2680(connectionString, warehouse, fromDate, toDate, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        //[pallavi.kale][GEOS2-9558][17.10.2025]
        public List<ArticlePurchasing> GetPurchasingReport_V2680(DateTime fromDate, DateTime toDate, long idArticleSupplier, int idArticle, long idWarehouse)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                return mgr.GetPurchasingReport_V2680(fromDate, toDate, idArticleSupplier, idArticle, idWarehouse, connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pallavi.kale][GEOS2-9806][24.11.2025]
        public bool DeleteArticleSupplier_V2690(Int64 idArticleSupplier, int IdUser)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerContext"].ConnectionString;
                return mgr.DeleteArticleSupplier_V2690(connectionString, idArticleSupplier, IdUser);
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
    }
}
