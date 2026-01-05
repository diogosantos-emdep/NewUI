using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class Offer
    {
        private string _ID=string.Empty;
        [DataMember(Order = 1)]            
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _Code = string.Empty;
        [DataMember(Order = 2)]
        public string Code
        {
            get { return _Code ; }
            set { _Code = value; }
        }
        private string _Project = string.Empty;
        [DataMember(Order = 3)]
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

        private string _Description = string.Empty;
        [DataMember(Order = 4)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _Group = string.Empty;
        [DataMember(Order = 5)]
        public string Group
        {
            get { return _Group; }
            set { _Group = value; }
        }

        private string _Plant = string.Empty;
        [DataMember(Order = 6)]
        public string Plant
        {
            get { return _Plant; }
            set { _Plant = value; }
        }

        private string _Country = string.Empty;
        [DataMember(Order = 7)]
        public string Contry
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Region = string.Empty;
        [DataMember(Order = 8)]
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }

        private string _BusinessUnit = string.Empty;
        [DataMember(Order = 9)]
        public string BusinessUnit
        {
            get { return _BusinessUnit; }
            set { _BusinessUnit = value; }
        }

        private string _Source = string.Empty;
        [DataMember(Order = 10)]
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private string _DeliveryDate = string.Empty;
        [DataMember(Order = 11)]
        public string DeliveryDate
        {
            get { return _DeliveryDate; }
            set { _DeliveryDate = value; }
        }

        private string _Status = string.Empty;
        [DataMember(Order = 12)]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private string _SalesOwner = string.Empty;
        [DataMember(Order = 13)]
        public string SalesOwner
        {
            get { return _SalesOwner; }
            set { _SalesOwner = value; }
        }
        
        private string _Category1 = string.Empty;
        [DataMember(Order = 14)]
        public string Category1
        {
            get { return _Category1; }
            set { _Category1 = value; }
        }
        
        private string _Category2 = string.Empty;
        [DataMember(Order = 15)]
        public string Category2
        {
            get { return _Category2; }
            set { _Category2 = value; }
        }

        private string _OEM = string.Empty;
        [DataMember(Order = 16)]
        public string OEM
        {
            get { return _OEM; }
            set { _OEM = value; }
        }

        private string _CreationDate = string.Empty;
        [DataMember(Order = 17)]
        public string CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        private string _ShipmentDate = string.Empty;
        [DataMember(Order = 18)]
        public string ShipmentDate
        {
            get { return _ShipmentDate; }
            set { _ShipmentDate = value; }
        }

        private string _EMDEPSite = string.Empty;
        [DataMember(Order = 19)]
        public string EMDEPSite
        {
            get { return _EMDEPSite; }
            set { _EMDEPSite = value; }
        }
        [DataMember(Order = 20)]
        public List<Product> Products { get; set; }

        public string IdProductSubCategory { get; set; }
        public string IdSalesOwner { get; set; }
    }
}
