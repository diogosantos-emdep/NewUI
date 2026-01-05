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
        List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetPurchaseOrderPdf(string AttachPDF);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder(Int64 idWarehousePurchaseOrder);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> GetAllWorkflowStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowTransition> GetAllWorkflowTransitions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateWorkflowStatusInPO(uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ArticleSupplier> GetArticleSuppliersByWarehouse(Warehouses warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteArticleSupplier(Int64 idArticleSupplier, int IdUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SendSupplierPurchaseOrderRequestMail(WarehousePurchaseOrder warehousePurchaseOrder, string EmailFrom);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ArticleSupplier GetArticleSupplierByIdArticleSupplier(Warehouses warehouse, UInt64 IdArticleSupplier);
    }
}
