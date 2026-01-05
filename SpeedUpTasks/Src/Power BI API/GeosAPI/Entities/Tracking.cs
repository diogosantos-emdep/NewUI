using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class Tracking
    {
        private string _Id = string.Empty;
        [DataMember(Order = 1)]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _SerialNumber = string.Empty;
        [DataMember(Order = 2)]
        public string SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }

        private string _WorkOrder = string.Empty;
        [DataMember(Order = 3)]
        public string WorkOrder
        {
            get { return _WorkOrder; }
            set { _WorkOrder = value; }
        }


        private string _OfferCode = string.Empty;
        [DataMember(Order = 4)]
        public string OfferCode
        {
            get { return _OfferCode; }
            set { _OfferCode = value; }
        }

        private string _OfferTitle = string.Empty;
        [DataMember(Order = 5)]
        public string OfferTitle
        {
            get { return _OfferTitle; }
            set { _OfferTitle = value; }
        }

        private string _OfferType = string.Empty;
        [DataMember(Order = 6)]
        public string OfferType
        {
            get { return _OfferType; }
            set { _OfferType = value; }
        }

        private string _Group = string.Empty;
        [DataMember(Order = 7)]
        public string Group
        {
            get { return _Group; }
            set { _Group = value; }
        }

        private string _Plant = string.Empty;
        [DataMember(Order = 8)]
        public string Plant
        {
            get { return _Plant; }
            set { _Plant = value; }
        }

        private string _Country = string.Empty;
        [DataMember(Order = 9)]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }


        private string _Region = string.Empty;
        [DataMember(Order = 10)]
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }

        private string _Project = string.Empty;
        [DataMember(Order = 11)]
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

        private string _Priority = string.Empty;
        [DataMember(Order = 12)]
        public string Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        private string _CustomerSpecifications = string.Empty;
        [DataMember(Order = 13)]
        public string CustomerSpecifications
        {
            get { return _CustomerSpecifications; }
            set { _CustomerSpecifications = value; }
        }

        private string _GoAheadDate = string.Empty;
        [DataMember(Order = 14)]
        public string GoAheadDate
        {
            get { return _GoAheadDate; }
            set { _GoAheadDate = value; }
        }

        private string _PODate = string.Empty;
        [DataMember(Order = 15)]
        public string PODate
        {
            get { return _PODate; }
            set { _PODate = value; }
        }

        private string _ExpectedDeliveryDate = string.Empty;
        [DataMember(Order = 16)]
        public string ExpectedDeliveryDate
        {
            get { return _ExpectedDeliveryDate; }
            set { _ExpectedDeliveryDate = value; }
        }

        private string _OfferOwner = string.Empty;
        [DataMember(Order = 17)]
        public string OfferOwner
        {
            get { return _OfferOwner; }
            set { _OfferOwner = value; }
        }

        private string _OfferStatus = string.Empty;
        [DataMember(Order = 18)]
        public string OfferStatus
        {
            get { return _OfferStatus; }
            set { _OfferStatus = value; }
        }

        private string _Item = string.Empty;
        [DataMember(Order = 19)]
        public string Item
        {
            get { return _Item; }
            set { _Item = value; }
        }

        private string _Template = string.Empty;
        [DataMember(Order = 20)]
        public string Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        private string _Type = string.Empty;
        [DataMember(Order = 21)]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private string _Cavities = string.Empty;
        [DataMember(Order = 22)]
        public string Cavities
        {
            get { return _Cavities; }
            set { _Cavities = value; }
        }

        private string _ConnectorFamily = string.Empty;
        [DataMember(Order = 23)]
        public string ConnectorFamily
        {
            get { return _ConnectorFamily; }
            set { _ConnectorFamily = value; }
        }

        private string _ConnectorGender = string.Empty;
        [DataMember(Order = 24)]
        public string ConnectorGender
        {
            get { return _ConnectorGender; }
            set { _ConnectorGender = value; }
        }

        private string _IdDrawing = string.Empty;
        [DataMember(Order = 25)]
        public string IdDrawing
        {
            get { return _IdDrawing; }
            set { _IdDrawing = value; }
        }

        private string _ItemStatus = string.Empty;
        [DataMember(Order = 26)]
        public string ItemStatus
        {
            get { return _ItemStatus; }
            set { _ItemStatus = value; }
        }

        private string _Stage = string.Empty;
        [DataMember(Order = 27)]
        public string Stage
        {
            get { return _Stage; }
            set { _Stage = value; }
        }

        private string _OpenDate = string.Empty;
        [DataMember(Order = 28)]
        public string OpenDate
        {
            get { return _OpenDate; }
            set { _OpenDate = value; }
        }

        private string _StartDate = string.Empty;
        [DataMember(Order = 29)]
        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private string _CloseDate = string.Empty;
        [DataMember(Order = 30)]
        public string CloseDate
        {
            get { return _CloseDate; }
            set { _CloseDate = value; }
        }

        private string _OperatorName = string.Empty;
        [DataMember(Order = 31)]
        public string OperatorName
        {
            get { return _OperatorName; }
            set { _OperatorName = value; }
        }

        private string _WorkingTime = string.Empty;
        [DataMember(Order = 32)]
        public string WorkingTime
        {
            get { return _WorkingTime; }
            set { _WorkingTime = value; }
        }

        private string _Result = string.Empty;
        [DataMember(Order = 33)]
        public string Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        private string _FailureCodes = string.Empty;
        [DataMember(Order = 34)]
        public string FailureCodes
        {
            get { return _FailureCodes; }
            set { _FailureCodes = value; }
        }

        private string _EmdepSite = string.Empty;
        [DataMember(Order = 35)]
        public string EmdepSite
        {
            get { return _EmdepSite; }
            set { _EmdepSite = value; }
        }

        private byte _IdStage;
        //[DataMember(Order = 2)]
        public byte IdStage
        {
            get { return _IdStage; }
            set { _IdStage = value; }
        }


        private Int32 _IdPerson;
        //[DataMember(Order = 2)]
        public Int32 IdPerson
        {
            get { return _IdPerson; }
            set { _IdPerson = value; }
        }

        private Int64 _IdRevisionItem;
        //[DataMember(Order = 2)]
        public Int64 IdRevisionItem
        {
            get { return _IdRevisionItem; }
            set { _IdRevisionItem = value; }
        }
        

    }
}
