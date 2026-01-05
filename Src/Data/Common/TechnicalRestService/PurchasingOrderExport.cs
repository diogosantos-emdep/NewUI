using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class PurchasingOrderExport
    {
        [DataMember(Order = 1)]
        public string IdPO { get; set; }

        [DataMember(Order = 2)]
        public string ParameterPlantOwner { get; set; }
        [DataMember(Order = 3)]
        public string Lang { get; set; }

        [DataMember(Order = 4)]
        public string ParameterMainConn { get; set; }

        [DataMember(Order = 5)]
        public string ParameterLoginContext { get; set; }

        [DataMember(Order = 6)]
        public string ParameterPlantwiseconnectionstring { get; set; }

        [DataMember(Order = 7)]
        public string login { get; set; }

        [DataMember(Order = 8)]
        public string ParameterExportPurchaseOrderTemplatePath { get; set; }
        [DataMember(Order = 9)]
        public string ParameterPurchasingOrdersFilePath { get; set; }
        [DataMember(Order = 10)]
        public byte[] ParameterImageBytes { get; set; }

        [Display(Order = 11)]
        public HttpContent File { get; set; }

        [DataMember(Order = 10)]
        public CreateExpenseReportsSign ParameterCreateExpenseReportsSign { get; set; }
    }

    //[Rdixit][APIGEOS-484][29-04-2022]
    [DataContract]
    public class PurchaseOrderExport
    {
        #region Fields
        public string _POCode = string.Empty;
        public string _PODeliveryDate = string.Empty;
        public string _PODate = string.Empty;
        public string _PoIncoterms = string.Empty;
        public string _PlantRegisteredName = string.Empty;
        public string _PlantCity = string.Empty;
        public string _PlantPhone = string.Empty;
        public string _PlantStreet = string.Empty;
        public string _PlantZipCode = string.Empty;
        public string _PlantState = string.Empty;
        public string _PlantCountry = string.Empty;
        public string _SupplierName = string.Empty;
        public string _SupplierStreet = string.Empty;
        public string _SupplierState = string.Empty;
        public string _SupplierCity = string.Empty;
        public string _SupplierZipCode = string.Empty;
        public string _SupplierContactEmail = string.Empty;
        public string _SupplierPhone = string.Empty;
        public string _SupplierContactName = string.Empty;
        public string _SupplierCountry = string.Empty;
        public string _DeliveryAddressWarehouseName = string.Empty;
        public string _DeliveryAddressStreet = string.Empty;
        public string _DeliveryAddressZipCode = string.Empty;
        public string _DeliveryAddressPhone = string.Empty;
        public string _DeliveryAddressCity = string.Empty;
        public string _DeliveryAddressState = string.Empty;
        public string _DeliveryAddressCountry = string.Empty;
        public string _SUPEntGroup = string.Empty;
        public string _Remarks = string.Empty;
        public string _PurchasingContactName = string.Empty;
        public string _PurchasingContactEmail = string.Empty;
        public string _Year = string.Empty;
        public List<ArticleItems> _ArticleItemsLst = null;
        public string _PlantRegistrationNumber = string.Empty; //[001][kshinde][APIGEOS-563][14/07/2022]
        public string _PaymentTerms = string.Empty;            //[001][kshinde][APIGEOS-564][15/07/2022]
        public string _SupplierNotes = string.Empty;           //[001][kshinde][APIGEOS-565][18/07/2022]
        public string _SupplierContactPhone = string.Empty;    //[001][kshinde][APIGEOS-569][20/07/2022]
        public string _PoCurrency = string.Empty;              //[001][kshinde][APIGEOS-575][22/07/2022]
        public string _Observations = string.Empty;            //chitra[cgirigosavi][APIGEOS-788][17/05/2023]
        #endregion
    }

    //[Rdixit][APIGEOS-484][29-04-2022]
    [DataContract]
    public class ArticleItems
    {
        #region Fields
        Int32 _ArticleID = 0;
        string _ArticleName = string.Empty;
        string _Description = string.Empty;
        double _UnitPrice = 0;
        Int64 _Qty = 0;
        double _discount = 0;
        double _totalPrice = 0;
        Int32 _PackingSize = 0;
        #endregion

        #region Properties
        [DataMember(Order = 1)]
        public string ArticleName { get { return _ArticleName; } set { _ArticleName = value; } }

        [DataMember(Order = 2)]
        public string Description { get { return _Description; } set { _Description = value; } }

        [DataMember(Order = 3)]
        public double UnitPrice { get { return _UnitPrice; } set { _UnitPrice = value; } }

        [DataMember(Order = 4)]
        public Int64 Qty { get { return _Qty; } set { _Qty = value; } }

        [DataMember(Order = 6)]
        public double Discount { get { return _discount; } set { _discount = value; } }

        [DataMember(Order = 5)]
        public double TotalPrice { get { return _totalPrice; } set { _totalPrice = value; } }

        [IgnoreDataMember] //[rdixit][APIGEOS-562][19.07.2022]
        public Int32 ArticleID { get { return _ArticleID; } set { _ArticleID = value; } }
        [DataMember(Order = 7)] //chitra.girigosavi APIGEOS-1194 30/07/2024 MEJORAS EN GEOS2 SRM
        public Int32 PackingSize { get { return _PackingSize; } set { _PackingSize = value; } }
        #endregion
    }

    public class CreateExpenseReportsSign
    {
        private string _Signature = string.Empty;
        [DataMember(Order = 1)]
        public string Signature
        {
            get { return _Signature; }
            set { _Signature = value; }
        }

        private string _DateTime = string.Empty;
        [DataMember(Order = 2)]
        public string DateTime
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        private string _FullName = string.Empty;
        [DataMember(Order = 3)]
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
    }


}
