using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Crm
{
    [DataContract]
    public class CustomerAccountImportField : ModelBase, IDisposable
    {
        #region Fileds
        private string group;
        private string plant;
        private string country;
        private string city;
        private string zipCode;
        private string address;
        private string state;
        private string registeredName;
        private string registrationNumber;
        private string businessField;
        private string businessType;
        private string website;
        private string salesOwner;
        private string source;

        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        [NotMapped]
        [DataMember]
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        [NotMapped]
        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [NotMapped]
        [DataMember]
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }

        [NotMapped]
        [DataMember]
        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                zipCode = value;
                OnPropertyChanged("ZipCode");
            }
        }

        [NotMapped]
        [DataMember]
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        [NotMapped]
        [DataMember]
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        [NotMapped]
        [DataMember]
        public string RegisteredName
        {
            get { return registeredName; }
            set
            {
                registeredName = value;
                OnPropertyChanged("RegisteredName");
            }
        }

        [NotMapped]
        [DataMember]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                registrationNumber = value;
                OnPropertyChanged("RegistrationNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public string BusinessField
        {
            get { return businessField; }
            set
            {
                businessField = value;
                OnPropertyChanged("BusinessField");
            }
        }

        [NotMapped]
        [DataMember]
        public string BusinessType
        {
            get { return businessType; }
            set
            {
                businessType = value;
                OnPropertyChanged("BusinessType");
            }
        }

        [NotMapped]
        [DataMember]
        public string Website
        {
            get { return website; }
            set
            {
                website = value;
                OnPropertyChanged("Website");
            }
        }

        [NotMapped]
        [DataMember]
        public string SalesOwner
        {
            get { return salesOwner; }
            set
            {
                salesOwner = value;
                OnPropertyChanged("SalesOwner");
            }
        }

        [NotMapped]
        [DataMember]
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
