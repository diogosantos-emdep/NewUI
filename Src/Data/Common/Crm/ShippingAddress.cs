using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Crm
{

    public class ShippingAddress : ModelBase, IDisposable
    {
        #region  Fields
        Int64 idShippingAddress;
        string name;
        string address;
        string zipCode;
        string city;
        string region;
        string countryName;
        Country country;
        Int64 idCountry;
        string remark;
        string comments;
        Int32 idSite;
        bool isDefault;
        string zipcityregion;
        private bool isInUse;
        private bool isPrimaryAddress;
        bool isDisabled;

        #endregion

        #region Constructor
        public ShippingAddress()
        {

        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdShippingAddress
        {
            get
            {
                return idShippingAddress;
            }

            set
            {
                idShippingAddress = value;
                OnPropertyChanged("IdShippingAddress");
            }
        }

        [NotMapped]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [NotMapped]
        [DataMember]
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
                OnPropertyChanged("CountryName");
            }
        }

        [NotMapped]
        [DataMember]
        public Country Country
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
        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }

        [NotMapped]
        [DataMember]
        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        [NotMapped]
        [DataMember]
        public string ZipCode
        {
            get
            {
                return zipCode;
            }

            set
            {
                zipCode = value;
                OnPropertyChanged("ZipCode");
            }
        }

        [NotMapped]
        [DataMember]
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }

        [NotMapped]
        [DataMember]
        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdCountry
        {
            get
            {
                return idCountry;
            }

            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        [NotMapped]
        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
                isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        [NotMapped]
        [DataMember]
        public string Zipcityregion
        {
            get { return zipcityregion; }
            set
            {
                zipcityregion = value;
                OnPropertyChanged("Zipcityregion");
            }
        }
        [NotMapped]
        [DataMember]
        public bool IsInUse
        {
            get
            {
                return isInUse;
            }
            set
            {
                isInUse = value;
                OnPropertyChanged(nameof(IsInUse));
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsPrimaryAddress
        {
            get
            {
                return isPrimaryAddress;
            }
            set
            {
                isPrimaryAddress = value;
                OnPropertyChanged(nameof(IsPrimaryAddress));
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsDisabled
        {
            get
            {
                return isDisabled;
            }

            set
            {
                isDisabled = value;
                OnPropertyChanged("IsDisabled");
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }


        #endregion

    }
}
