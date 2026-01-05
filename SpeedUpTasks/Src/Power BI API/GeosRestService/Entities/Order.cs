using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class Order
    {
        #region Fields

        private string _ShipmentDate = string.Empty;
        private string _Code = string.Empty;
        private string _Description = string.Empty;
        private string _Project = string.Empty;
        private string _Group = string.Empty;
        private string _Plant = string.Empty;
        private string _Country = string.Empty;
        private string _Region = string.Empty;
        private string _BusinessUnit = string.Empty;
        private string _Source = string.Empty;
        private string _PoDate = string.Empty;
        private string _OfferCurrency = string.Empty;
        private string _InvoiceCurrency = string.Empty;
        private string _DeliveryDate = string.Empty;
        private string _Status = string.Empty;
        private string _Owner = string.Empty;
        private string _Category = string.Empty;
        private string _ProductSubCategory = string.Empty;
        private string _OEM = string.Empty;
        private string _QuoteSentDate = string.Empty;
        private string _EmdepSite = string.Empty;
        private string _PO = string.Empty;
        private string _RfqReceptionDate = string.Empty;
        #endregion
        #region Properties

        [DataMember(Order = 1)]
        public Int64 Id { get; set; }
        [DataMember(Order = 2)]
        public string Code { get { return _Code; } set { _Code = value; } }
        [DataMember(Order = 4)]
        public string Description { get { return _Description; } set { _Description = value; } }
        [DataMember(Order = 3)]
        public string Project { get { return _Project; } set { _Project = value; } }
        [DataMember(Order = 5)]
        public string Group { get { return _Group; } set { _Group = value; } }
        [DataMember(Order = 6)]
        public string Plant { get { return _Plant; } set { _Plant = value; } }
        [DataMember(Order = 7)]
        public string Country { get { return _Country; } set { _Country = value; } }
        [DataMember(Order = 8)]
        public string Region { get { return _Region; } set { _Region = value; } }
        [DataMember(Order = 9)]
        public string BusinessUnit { get { return _BusinessUnit; } set { _BusinessUnit = value; } }
        [DataMember(Order = 10)]
        public string Source { get { return _Source; } set { _Source = value; } }
        [DataMember(Order = 11)]
        public string PoDate { get { return _PoDate; } set { _PoDate = value; } }
        [DataMember(Order = 12)]
        public double Amount { get; set; }

        [DataMember(Order = 13)]
        public string Currency { get { return _OfferCurrency; } set { _OfferCurrency = value; } }
        [DataMember(Order = 14)]
        public double InvoiceAmount { get; set; }

        [DataMember(Order = 15)]
        public string InvoiceCurrency { get { return _InvoiceCurrency; } set { _InvoiceCurrency = value; } }

        [DataMember(Order = 16)]
        public string DeliveryDate { get { return _DeliveryDate; } set { _DeliveryDate = value; } }

        [DataMember(Order = 17)]
        public string Status { get { return _Status; } set { _Status = value; } }

        [DataMember(Order = 18)]
        public string SalesOwner { get { return _Owner; } set { _Owner = value; } }

        [DataMember(Order = 19)]
        public string Category1 { get { return _Category; } set { _Category = value; } }

        [DataMember(Order = 20)]
        public string Category2 { get { return _ProductSubCategory; } set { _ProductSubCategory = value; } }

        [DataMember(Order = 21)]
        public string OEM
        {
            get { return _OEM; }
            set { _OEM = value; }
        }

        [DataMember(Order = 22)]
        public string QuoteSentDate
        {
            get { return _QuoteSentDate; }
            set { _QuoteSentDate = value; }
        }


        [DataMember(Order = 23)]
        public string PO
        {
            get { return _PO; }
            set { _PO = value; }
        }

        [DataMember(Order = 24)]
        public string RfqReceptionDate
        {
            get { return _RfqReceptionDate; }
            set { _RfqReceptionDate = value; }
        }

        [DataMember(Order = 26)]
        public string EmdepSite
        {
            get { return _EmdepSite; }
            set { _EmdepSite = value; }
        }
        [DataMember(Order = 25)]
        public string ShipmentDate
        {
            get { return _ShipmentDate; }
            set { _ShipmentDate = value; }
        }

        [DataMember(Order=26)]
        public List<Product> Products{get;set;}

        #endregion
    }

    public class Product
    {
        private string _Name=string.Empty;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Qty=string.Empty;

        public string Qty
        {
            get { return _Qty; }
            set { _Qty = value; }
        }


    }
}
