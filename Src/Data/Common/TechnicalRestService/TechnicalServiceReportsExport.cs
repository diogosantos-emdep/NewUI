using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emdep.Geos.Data.Common
{
    public class TechnicalServiceReportsExport
    {
        //public string IdRevisionItem = string.Empty;
        public string _IdTechnicalAssistanceReport = string.Empty;
        public string _Code = string.Empty;
        public string _IdOTItem = string.Empty;
        public string _OrderCode = string.Empty;
        public string _ReportCode = string.Empty;
        public string _ReportDate = string.Empty;
        public string _TechnicianFullName = string.Empty;
        public string _RequesterFullName = string.Empty;
        public string _ContactFullName = string.Empty;
        public string _ContactPhone = string.Empty;

        public string _ContactEmail = string.Empty;
        public string _CustomerGroup = string.Empty;
        public string _CustomerPlant = string.Empty;
        public string _CustomerCountry = string.Empty;
        public string _Remarks = string.Empty;
        public string _ServiceType1 = string.Empty;
        public string _ServiceType2 = string.Empty;
        public string _ServiceType3 = string.Empty;
        public string _ServiceType4 = string.Empty;
        public string _ServiceType5 = string.Empty;
        public string _ServiceType6 = string.Empty;
        public string _ServiceType7 = string.Empty;
        public List<ExportWorkLogs> _WorklogItemsLst = null;
        public List<ExportParts> _PartslogItemsLst = null;
        public List<ExportAttachments> _PhotoslogItemsLst = null; //chitra.girigosavi[19/08/2024][APIGEOS-1195]
        public string Attachment1 = null;
        public string Attachment2 = null;
        public string Attachment3 = null;
        public string Attachment4 = null;
        public string _Year = string.Empty;

        //chitra.girigosavi APIGEOS-1102 Add a new service TechnicalService->Orders->Reports->Send
        public string _OfferTitle = string.Empty;
        public string Category1 = string.Empty;
        public string Category2 = string.Empty;
        public string OfferCode = string.Empty;
        public string CarProjectName = string.Empty;
        public string caroems_Name = string.Empty;
        public string _IdProductSubCategory = string.Empty;
        public string _RFQ = string.Empty;// soumitra.kulkarni[23/01/2025][APIGEOS-1335]
    }
}