using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class Site : ModelBase, IDisposable
    {

        #region Fields
        UInt32 idSite;
        string name;
        UInt64 idArticle;
        float articleCostValue;
        UInt64 idBasePriceList;
        DateTime currencyConversionDate;

        UInt64 idCustomerPriceList;
        string iso;
        string groupName;
        string regionName;
        UInt32 idGroup;
        UInt32? idRegion;
        byte idCountry;
        string countryName;
        DateTime? currencyConversionDate_New;
        string reference;
        byte[] countryIconbytes;
        Customer customer; // [nsatpute][21-01-2025][GEOS2-5725]
        Int32 idStatus;
        string status;
        #endregion

        #region Constructor
        public Site()
        {

        }
        #endregion

        #region Properties


        [DataMember]
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public Int32 IdStatus
        {
            get
            {
                return idStatus;
            }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
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
        [DataMember]
        public ulong IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }
        [DataMember]
        public float ArticleCostValue
        {
            get
            {
                return articleCostValue;
            }

            set
            {
                articleCostValue = value;
                OnPropertyChanged("ArticleCostValue");
            }
        }

        [DataMember]
        public ulong IdBasePriceList
        {
            get
            {
                return idBasePriceList;
            }

            set
            {
                idBasePriceList = value;
                OnPropertyChanged("IdBasePriceList");
            }
        }
        [DataMember]
        public DateTime CurrencyConversionDate
        {
            get
            {
                return currencyConversionDate;
            }

            set
            {
                currencyConversionDate = value;
                OnPropertyChanged("CurrencyConversionDate");
            }
        }

        [DataMember]
        public ulong IdCustomerPriceList
        {
            get
            {
                return idCustomerPriceList;
            }

            set
            {
                idCustomerPriceList = value;
                OnPropertyChanged("IdCustomerPriceList");
            }
        }

        [DataMember]
        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
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
        public uint IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        [DataMember]
        public uint? IdRegion
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
        public byte IdCountry
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
        public DateTime? CurrencyConversionDate_New
        {
            get
            {
                return currencyConversionDate_New;
            }

            set
            {
                currencyConversionDate_New = value;
                OnPropertyChanged("CurrencyConversionDate_New");
            }
        }
        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [DataMember]
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
                OnPropertyChanged("CountryIconBytes");
            }
        }
        [NotMapped]
        [DataMember]
        public string Iso
        {
            get { return iso; }
            set
            {
                iso = value;
                OnPropertyChanged("Iso");
            }
        }
		// [nsatpute][21-01-2025][GEOS2-5725]
        [NotMapped]
        [DataMember]
        public Customer Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
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
