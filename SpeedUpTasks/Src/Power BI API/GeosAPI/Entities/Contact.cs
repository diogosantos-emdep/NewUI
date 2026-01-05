using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class Contact
    {
        private int _Id = 0;
        [DataMember(Order = 1)]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _FirstName = string.Empty;
        [DataMember(Order = 2)]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName = string.Empty;
        [DataMember(Order = 3)]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        private string _Department = string.Empty;
        [DataMember(Order = 4)]
        public string Department
        {
            get { return _Department; }
            set { _Department = value; }
        }

        private string _JobTitle = string.Empty;
        [DataMember(Order = 5)]
        public string JobTitle
        {
            get { return _JobTitle; }
            set { _JobTitle = value; }
        }

        private string _Group = string.Empty;
        [DataMember(Order = 6)]
        public string Group
        {
            get { return _Group; }
            set { _Group = value; }
        }

        private string _Plant = string.Empty;
        [DataMember(Order = 7)]
        public string Plant
        {
            get { return _Plant; }
            set { _Plant = value; }
        }

        private string _Country = string.Empty;
        [DataMember(Order = 8)]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Region = string.Empty;
        [DataMember(Order = 9)]
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }

        private string _Email = string.Empty;
        [DataMember(Order = 10)]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Phone = string.Empty;
        [DataMember(Order = 11)]
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _EmdepAffinity = string.Empty;
        [DataMember(Order = 12)]
        public string EmdepAffinity
        {
            get { return _EmdepAffinity; }
            set { _EmdepAffinity = value; }
        }

        private string _InfluenceLevel = string.Empty;
        [DataMember(Order = 13)]
        public string InfluenceLevel
        {
            get { return _InfluenceLevel; }
            set { _InfluenceLevel = value; }
        }

        private string _CompetitorAffinity = string.Empty;
        [DataMember(Order = 14)]
        public string CompetitorAffinity
        {
            get { return _CompetitorAffinity; }
            set { _CompetitorAffinity = value; }
        }

        private string _ProductInvolved = string.Empty;
        [DataMember(Order = 15)]
        public string ProductInvolved
        {
            get { return _ProductInvolved; }
            set { _ProductInvolved = value; }
        }
    }
}
