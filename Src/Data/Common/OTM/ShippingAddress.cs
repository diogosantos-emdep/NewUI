using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    [DataContract]
    public class ShippingAddress : ModelBase
    {
        #region  Fields
        Int64 idShippingAddress;
        string name;
        string address;
        string fullAddress;
        string isocode;
        string zipCode;
        string city;
        string countryIconUrl;
        string countriesName;
        string region;
        Country country;
        Int64 idCountry;
        string zipcityregion;
        bool isDefault;
        string remark;
        Int32 idSite;
        private bool isInUse;
        bool isDisabled;
        int? idCustomerPlant;
        #endregion

        #region Properties

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
        public string CountriesName
        {
            get
            {
                return countriesName;
            }

            set
            {
                countriesName = value;
                OnPropertyChanged("CountriesName");
            }
        }



        [NotMapped]
        [DataMember]
        public string CountryIconUrl
        {
            get
            {
                return countryIconUrl;
            }

            set
            {
                countryIconUrl = value;
                OnPropertyChanged("CountryIconUrl");
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
        public string IsoCode
        {
            get
            {
                return isocode;
            }

            set
            {
                isocode = value;
                OnPropertyChanged("IsoCode");
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
        public string FullAddress
        {
            get
            {
                return fullAddress;
            }

            set
            {
                fullAddress = value;
                OnPropertyChanged("FullAddress");
            }
        }
        [NotMapped]
        [DataMember]
        public int? IdCustomerPlant
        {
            get { return idCustomerPlant; }
            set
            {
                idCustomerPlant = value;
                OnPropertyChanged("IdCustomerPlant");
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
