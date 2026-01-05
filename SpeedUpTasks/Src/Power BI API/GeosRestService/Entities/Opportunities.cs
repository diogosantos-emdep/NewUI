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
    public class Opportunities
    {
        #region Fields
        //int _TargetAmount = 0;
        //string _TargetCurrency = string.Empty;
        string _IdSalesOwner = string.Empty;
        string _IdProductCategory = string.Empty;
        string _Code=string.Empty;
        string _Description = string.Empty;
        string _Project = string.Empty;
        string _Group = string.Empty;
        string _Plant = string.Empty;
        string _Country = string.Empty;
        string _Region = string.Empty;
        string _BusinessUnit = string.Empty;
        string _Source = string.Empty;
        string _CloseDate = string.Empty;
        string _Currency = string.Empty;
        string _Rfq = string.Empty;
        string _RFQReception = string.Empty;
        string _Status = string.Empty;
        string _SalesOwner = string.Empty;
        string _Category1 = string.Empty;
        string _Category2 = string.Empty;
        string _OEM = string.Empty;
        string _QuoteSentDate = string.Empty;
        string _EmdepSite = string.Empty;
        string _LostReason = string.Empty;
        string _LostDescription = string.Empty;
        string _Competitor = string.Empty;
        #endregion
        #region Properties
        //[DataMember(Order = 27)]
        //public string TargetCurrency
        //{
        //    get { return _TargetCurrency; }
        //    set { _TargetCurrency = value; }
        //}
        //[DataMember(Order = 26)]
        //public int TargetAmount
        //{
        //    get { return _TargetAmount; }
        //    set { _TargetAmount = value; }
        //}
        //[DataMember(Order = 27)]
        public string IdSalesOwner
        {
            get { return _IdSalesOwner; }
            set { _IdSalesOwner = value; }
        }
        //[DataMember(Order = 26)]
        public string IdProductCategory
        {
            get { return _IdProductCategory; }
            set { _IdProductCategory = value; }
        }
        [DataMember(Order = 25)]
        public string Competitor
        {
            get { return _Competitor; }
            set { _Competitor = value; }
        }
        [DataMember(Order = 24)]
        public string LostDescription
        {
            get { return _LostDescription; }
            set { _LostDescription = value; }
        }
        [DataMember(Order = 23)]
        public string LostReason
        {
            get { return _LostReason; }
            set { _LostReason = value; }
        }
        [DataMember(Order = 26)]
        public string EmdepSite
        {
            get { return _EmdepSite; }
            set { _EmdepSite = value; }
        }
        [DataMember(Order = 22)]
        public string QuoteSentDate
        {
            get { return _QuoteSentDate; }
            set { _QuoteSentDate = value; }
        }
        [DataMember(Order = 21)]
        public string OEM
        {
            get { return _OEM; }
            set { _OEM = value; }
        }
        [DataMember(Order = 1)]
        public Int64 Id { get; set; }
        [DataMember(Order = 2)]
        public string Code
        {
            get {return _Code; } set {_Code=value; }
        }
        [DataMember(Order = 4)]
        public string Description
        {
            get { return _Description; } set { _Description = value; }
        }
        [DataMember(Order = 3)]
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }
        [DataMember(Order = 5)]
        public string Group
        {
            get { return _Group; }
            set { _Group = value; }
        }
        [DataMember(Order = 6)]
        public string Plant
        {
            get { return _Plant; }
            set { _Plant = value; }
        }
        [DataMember(Order = 7)]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        [DataMember(Order = 8)]
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }
        [DataMember(Order = 9)]
        public string BusinessUnit
        {
            get { return _BusinessUnit; }
            set { _BusinessUnit = value; }
        }
        [DataMember(Order = 10)]
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }
        [DataMember(Order = 11)]
        public string CloseDate
        {
            get { return _CloseDate; }
            set { _CloseDate = value; }
        }
        [DataMember(Order = 12)]
        public Int32 ConfidentialLevel { get; set; }
        [DataMember(Order = 13)]
        public double Amount { get; set; }
        [DataMember(Order = 14)]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }

        [DataMember(Order = 15)]
        public string Rfq
        {
            get { return _Rfq; }
            set { _Rfq = value; }
        }

        [DataMember(Order = 16)]
        public string RFQReception
        {
            get { return _RFQReception; }
            set { _RFQReception = value; }
        }

        [DataMember(Order = 17)]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        [DataMember(Order = 18)]
        public string SalesOwner
        {
            get { return _SalesOwner; }
            set { _SalesOwner = value; }
        }

        [DataMember(Order = 19)]
        public string Category1
        {
            get { return _Category1; }
            set { _Category1 = value; }
        }

        [DataMember(Order = 20)]
        public string Category2
        {
            get { return _Category2; }
            set { _Category2 = value; }
        }
        [DataMember(Order = 26)]
        public List<Product> Products { get; set; }
        #endregion
    }
}
