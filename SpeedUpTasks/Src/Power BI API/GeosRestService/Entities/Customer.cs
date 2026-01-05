using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class Customer
    {
        #region Fields
       
        private int _Id=0;
        [DataMember(Order=1)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Group=string.Empty;
        [DataMember(Order = 2)]
        public string Group
        {
            get { return _Group; }
            set { _Group = value; }
        }
        private string _Plant=string.Empty;
        [DataMember(Order = 3)]
        public string Plant
        {
            get { return _Plant; }
            set { _Plant = value; }
        }
        private string _RegisteredName = string.Empty;
        [DataMember(Order = 4)]
        public string RegisteredName
        {
            get { return _RegisteredName; }
            set { _RegisteredName = value; }
        }

        private string _RegistrationNumber = string.Empty;
        [DataMember(Order = 5)]
        public string RegistrationNumber
        {
            get { return _RegistrationNumber; }
            set { _RegistrationNumber = value; }
        }
        private string _Address = string.Empty;
        [DataMember(Order = 6)]
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        private string _City = string.Empty;
        [DataMember(Order = 7)]
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        private string _Zipcode = string.Empty;
        [DataMember(Order = 8)]
        public string Zipcode
        {
            get { return _Zipcode; }
            set { _Zipcode = value; }
        }

        private string _State = string.Empty;
        [DataMember(Order = 9)]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        private string _Country = string.Empty;
        [DataMember(Order = 10)]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        private string _Region = string.Empty;
        [DataMember(Order = 11)]
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }
        private string _Email = string.Empty;
        [DataMember(Order = 12)]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _Fax = string.Empty;
        [DataMember(Order = 13)]
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        private string _Website = string.Empty;
        [DataMember(Order = 14)]
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }

        private string _Phone = string.Empty;
        [DataMember(Order = 15)]
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        private string _BusinessField = string.Empty;
        [DataMember(Order = 16)]
        public string BusinessField
        {
            get { return _BusinessField; }
            set { _BusinessField = value; }
        }
        private string _BusinessType = string.Empty;
        [DataMember(Order = 17)]
        public string BusinessType
        {
            get { return _BusinessType; }
            set { _BusinessType = value; }
        }
        private string _BusinessProduct = string.Empty;
        [DataMember(Order = 18)]
        public string BusinessProduct
        {
            get { return _BusinessProduct; }
            set { _BusinessProduct = value; }
        }
        private string _Size = string.Empty;
        [DataMember(Order = 19)]
        public string Size
        {
            get { return _Size; }
            set { _Size = value; }
        }
        private string _NumberOfEmployees = string.Empty;
        [DataMember(Order = 20)]
        public string NumberOfEmployees
        {
            get { return _NumberOfEmployees; }
            set { _NumberOfEmployees = value; }
        }
        private string _NumberOfCuttingMachines = string.Empty;
        [DataMember(Order = 21)]
        public string NumberOfCuttingMachines
        {
            get { return _NumberOfCuttingMachines; }
            set { _NumberOfCuttingMachines = value; }
        }
        private string _NumberOfLines = string.Empty;
        [DataMember(Order = 22)]
        public string NumberOfLines
        {
            get { return _NumberOfLines; }
            set { _NumberOfLines = value; }
        }

        private string _CreationDate=string.Empty;
        [DataMember(Order = 23)]
        public string CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate= value; }
        }

        private double _Age = 0;
        [DataMember(Order = 24)]
        public double Age
        {
            get { return _Age; }
            set { _Age = value; }
        }
        private string _SalesOwner = string.Empty;
        [DataMember(Order = 25)]
        public string SalesOwner
        {
            get { return _SalesOwner; }
            set { _SalesOwner = value; }
        }

        #endregion
    }
}
