using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Crm
{
	// [nsatpute] [GEOS2-5702][28-06-2024] Add new import accounts/contacts option (2/2)
    public class CustomerContactImportField : ModelBase, IDisposable
    {
        #region Fileds
        private string group;
        private string plant;
        private string country;
        private string firstName;
        private string lastName;
        private string gender;
        private string phone;
        private string department;
        private string jobTitle;
        private string email;
        private string productInvolved;
        private string influenceLevel;
        private string emdepAffinity;
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
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [NotMapped]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [NotMapped]
        [DataMember]
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        [NotMapped]
        [DataMember]
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }

        [NotMapped]
        [DataMember]
        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged("Department");
            }
        }

        [NotMapped]
        [DataMember]
        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                jobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }

        [NotMapped]
        [DataMember]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        [NotMapped]
        [DataMember]
        public string ProductInvolved
        {
            get { return productInvolved; }
            set
            {
                productInvolved = value;
                OnPropertyChanged("ProductInvolved");
            }
        }

        [NotMapped]
        [DataMember]
        public string InfluenceLevel
        {
            get { return influenceLevel; }
            set
            {
                influenceLevel = value;
                OnPropertyChanged("InfluenceLevel");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmdepAffinity
        {
            get { return emdepAffinity; }
            set
            {
                emdepAffinity = value;
                OnPropertyChanged("EmdepAffinity");
            }
        }
        
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
