using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class Customers : ModelBase, IDisposable
    {
        #region Fields
        UInt32 idCustomer;
        UInt32 idSite;
        uint idCountry;
        UInt32 idRegion;
        string customerName;
        string siteName;
        string countryName;
        string regionName;

        bool isChecked;
        #endregion

        #region Constructor
        public Customers()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdCustomer
        {
            get
            {
                return idCustomer;
            }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }
        [DataMember]
        public UInt32 IdSite
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

        [DataMember]
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }
        [DataMember]
        public string SiteName
        {
            get
            {
                return siteName;
            }
            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }

        [DataMember]
        public uint IdCountry
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

        [DataMember]
        public UInt32 IdRegion
        {
            get
            {
                return idRegion;
            }

            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

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

        [DataMember]
        public string RegionName
        {
            get
            {
                return regionName;
            }

            set
            {
                regionName = value;
                OnPropertyChanged("RegionName");
            }
        }

        [DataMember]
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
