using Emdep.Geos.Data.Common.TechnicalRestService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITechnicalRestService" in both code and config file together.
    [ServiceContract]
    public interface ITechnicalRestService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetData?value={value}", ResponseFormat = WebMessageFormat.Json)]
        string GetData(int value);

        [OperationContract]
        [WebGet(UriTemplate = "/TechnicalService/Export?IdTechnicalAssistanceReport={IdTechnicalAssistanceReport}&PlantOwner={PlantOwner}&Lang={Lang}", ResponseFormat = WebMessageFormat.Json)]
        string ExportTechnicalAssistanceReport(string IdTechnicalAssistanceReport, string PlantOwner, string Lang);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/TechnicalService/Export", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string ExportTechnicalAssistanceReportNew(InputData input);


        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/SalesOpportunities/Create", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string SalesOpportunitiesCreate(CreateOpportunityModel request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SalesOpportunities/Create", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<OpportunityCreateModel> SalesOpportunitiesCreate(CreateOpportunityModel request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SalesOpportunities/Export", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool SalesOpportunitiesExport(CreateOpportunityModel request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SalesOpportunities/Send", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SendEmail SalesOpportunitiesSend(SendMailOffer request);

        //Shubham[skadam] APIGEOS-1353 Use Workbench services in PURCHASING->ORDERS->Export  11 02 2025 
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/PurchasingOrders/Export", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool PurchasingOrdersExport(PurchasingOrderExport request);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/PurchasingOrders/Sign", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //bool PurchasingOrdersSign(PurchasingOrderExport request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/PurchasingOrders/Sign", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> PurchasingOrdersSign(Stream requestStream);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/PurchasingOrders/Sign", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //bool PurchasingOrdersSignAsync(Stream request);
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/TechnicalServiceOrders/Sign", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> TechnicalServiceSign(Stream requestStream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/TechnicalServiceOrders/Send", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SendEmail TechnicalServiceSend(SendMailOffer request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/TechnicalServiceOrders/Export", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string TechnicalAssistanceReportExport(TechnicalServiceExport request);

        //Shubham[skadam] APIGEOS-1392 Use plant service for Accounting->ExpenseReports->Export  28 04 2025
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ExpenseReports/Export", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string ExpenseReportsExport(EmployeeExpenseReportExport request);
        //Shubham[skadam] APIGEOS-1394 Use plant service for Accounting->ExpenseReports->Send  28 04 2025
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ExpenseReports/Send", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SendEmail ExpenseReportsSend(EmployeeExpenseReportExport request);
        //Shubham[skadam] APIGEOS-1393 Use plant service for Accounting->ExpenseReports->Sign  28 04 2025
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ExpenseReports/Sign", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Task<string> ExpenseReportsSign(Stream request);

        //Shubham[skadam] APIGEOS-1394 Use plant service for Accounting->ExpenseReports->Send  07 05 2025
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ExpenseReports/FileBytes", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        byte[] ExpenseReportsFileBytes(EmployeeExpenseReportExport request);


        //[pallavi.jadhav][03 09 2025][APIGEOS-1628]
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Production/Timetracking/List", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ProductionTimetracking TimetrackingList(TimetrackingList request);
    }
}
//[DataContract]
//public class InputData
//{
//    [DataMember]
//    public string IdTechnicalAssistanceReport { get; set; }

//    [DataMember]
//    public string PlantOwner { get; set; }

//    [DataMember]
//    public string Lang { get; set; }

//    [DataMember]
//    public string TechnicalAssistanceReportTemplatePath { get; set; }

//    [DataMember]
//    public string TechnicalAssistanceReportPath { get; set; }

//    [DataMember]
//    public string WorkingOrdersPath { get; set; }
//}


