using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class ServiceRequest
    {
        private string _Id = string.Empty;
        [DataMember(Order = 1)]
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Code = string.Empty;
        [DataMember(Order = 2)]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Title = string.Empty;
        [DataMember(Order = 3)]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }


        private string _Type = string.Empty;
        [DataMember(Order = 4)]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private string _ProductCategory = string.Empty;
        [DataMember(Order = 5)]
        public string ProductCategory
        {
            get { return _ProductCategory; }
            set { _ProductCategory = value; }
        }

        private string _ProductName = string.Empty;
        [DataMember(Order = 6)]
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
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

        private string _Priority = string.Empty;
        [DataMember(Order = 10)]
        public string Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        private string _CreatedBy = string.Empty;
        [DataMember(Order = 11)]
        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        private string _CreationDate = string.Empty;
        [DataMember(Order = 12)]
        public string CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        private string _ExpectedEndDate = string.Empty;
        [DataMember(Order = 13)]
        public string ExpectedEndDate
        {
            get { return _ExpectedEndDate; }
            set { _ExpectedEndDate = value; }
        }

        private string _EndDate = string.Empty;
        [DataMember(Order = 14)]
        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        private string _Solution = string.Empty;
        [DataMember(Order = 15)]
        public string Solution
        {
            get { return _Solution; }
            set { _Solution = value; }
        }

        private string _Owner = string.Empty;
        [DataMember(Order = 16)]
        public string Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        private string _Status = string.Empty;
        [DataMember(Order = 17)]
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private string _ProgressInPercentage = string.Empty;
        [DataMember(Order = 18)]
        public string ProgressInPercentage
        {
            get { return _ProgressInPercentage; }
            set { _ProgressInPercentage = value; }
        }

        private string _LatestPostDate = string.Empty;
        [DataMember(Order = 19)]
        public string LatestPostDate
        {
            get { return _LatestPostDate; }
            set { _LatestPostDate = value; }
        }


    }
}
