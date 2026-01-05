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
    public class Activity
    {
        #region Fields
        string _CarprojectString = string.Empty;
        string _Subject = string.Empty;
        string _ActivityType = string.Empty;
        string _ActivityTagsString = string.Empty;
        string _Description = string.Empty;
        string _ActivityAttendeesString = string.Empty;
        string _Location = string.Empty;
        string _DueDate = string.Empty;
        string _Status = string.Empty;
        string _CloseDate = string.Empty;
        string _SalesOwner = string.Empty;
        string _LinkedAccountGroup = string.Empty;
        string _LinkedAccountPlant = string.Empty;
        string _ContactString = string.Empty;
        string _OfferCodeString = string.Empty;
        string _CompetitorString = string.Empty;
        #endregion

        #region Properties
        [DataMember(Order = 1)]
        public Int64 IdActivity { get; set; }
        [DataMember(Order = 2)]
        public byte IsInternal { get; set; }
        [DataMember(Order = 4)]
        public string Subject
        {
            get
            {
                return _Subject;
            }
            set
            {
                _Subject = value;
            }
        }
        [DataMember(Order = 3)]
        public string ActivityType
        {
            get
            {
                return _ActivityType;
            }
            set
            {
                _ActivityType = value;
            }
        }
        [DataMember(Order = 5)]
        public string ActivityTagsString
        {
            get
            {
                return _ActivityTagsString;
            }
            set
            {
                _ActivityTagsString = value;
            }
        }
        [DataMember(Order = 6)]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        [DataMember(Order = 7)]
        public string ActivityAttendeesString
        {
            get
            {
                return _ActivityAttendeesString;
            }
            set
            {
                _ActivityAttendeesString = value;
            }
        }
        [DataMember(Order = 8)]
        public string Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
            }
        }
        [DataMember(Order = 9)]
        public string DueDate
        {
            get
            {
                return _DueDate;
            }
            set
            {
                _DueDate = value;
            }
        }
        [DataMember(Order = 10)]
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }
        [DataMember(Order = 11)]
        public string CloseDate
        {
            get
            {
                return _CloseDate;
            }
            set
            {
                _CloseDate = value;
            }
        }
        [DataMember(Order = 12)]
        public string SalesOwner
        {
            get
            {
                return _SalesOwner;
            }
            set
            {
                _SalesOwner = value;
            }
        }
        [DataMember(Order = 13)]
        public string LinkedAccountGroup
        {
            get
            {
                return _LinkedAccountGroup;
            }
            set
            {
                _LinkedAccountGroup = value;
            }
        }
        [DataMember(Order = 14)]
        public string LinkedAccountPlant
        {
            get
            {
                return _LinkedAccountPlant;
            }
            set
            {
                _LinkedAccountPlant = value;
            }
        }
        [DataMember(Order = 15)]
        public string CarprojectString
        {
            get
            {
                return _CarprojectString;
            }
            set
            {
                _CarprojectString = value;
            }
        }

        [DataMember(Order = 16)]
        public string ContactString
        {
            get
            {
                return _ContactString;
            }
            set
            {
                _ContactString = value;
            }
        }

        [DataMember(Order = 17)]
        public string OfferCodeString
        {
            get
            {
                return _OfferCodeString;
            }
            set
            {
                _OfferCodeString = value;
            }
        }

        [DataMember(Order = 18)]
        public string CompetitorString
        {
            get
            {
                return _CompetitorString;
            }
            set
            {
                _CompetitorString = value;
            }
        }
        #endregion
    }
}
