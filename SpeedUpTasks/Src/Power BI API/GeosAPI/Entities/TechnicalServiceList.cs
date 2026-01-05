using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class TechnicalServiceList
    {

        private Int64 _IdOT;
      
        public Int64 IdOT
        {
            get { return _IdOT; }
            set { _IdOT = value; }
        }

        private string _Code = string.Empty;
        [DataMember(Order = 1)]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
              

        private string _Project = string.Empty;
        [DataMember(Order = 2)]
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

        private string _Description = string.Empty;
        [DataMember(Order = 3)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _Group = string.Empty;
        [DataMember(Order = 4)]
        public string Group
        {
            get { return _Group; }
            set { _Group = value; }
        }

        private string _Plant = string.Empty;
        [DataMember(Order = 5)]
        public string Plant
        {
            get { return _Plant; }
            set { _Plant = value; }
        }

        private string _Country = string.Empty;
        [DataMember(Order = 6)]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Region = string.Empty;
        [DataMember(Order = 7)]
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }

        private string _BusinessUnit = string.Empty;
        [DataMember(Order = 8)]
        public string BusinessUnit
        {
            get { return _BusinessUnit; }
            set { _BusinessUnit = value; }
        }

        private string _PODate = string.Empty;
        [DataMember(Order = 9)]
        public string PODate
        {
            get { return _PODate; }
            set { _PODate = value; }
        }

        private string _POCode = string.Empty;
        [DataMember(Order = 10)]
        public string POCode
        {
            get { return _POCode; }
            set { _POCode = value; }
        }


        private string _CreationDate = string.Empty;
        [DataMember(Order = 11)]
        public string CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        private string _DeliveryDate = string.Empty;
        [DataMember(Order = 12)]
        public string DeliveryDate
        {
            get { return _DeliveryDate; }
            set { _DeliveryDate = value; }
        }

        private string _EndDate = string.Empty;
        [DataMember(Order = 13)]
        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }


        private string _EmdepSite = string.Empty;
        [DataMember(Order = 14)]
        public string EmdepSite
        {
            get { return _EmdepSite; }
            set { _EmdepSite = value; }
        }

        private string _Services = string.Empty;
        [DataMember(Order = 15)]
        public string Services
        {
            get { return _Services; }
            set { _Services = value; }
        }


    }
}
