using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class SiteAPI
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
        string databaseIP;
        #endregion


        #region Properties

        [DataMember]
        [IgnoreDataMember]
        public UInt32 IdSite
        {
            get
            {
                return idSite;
            }
            set
            {
                idSite = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        [DataMember]
        [IgnoreDataMember]
        public ulong IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
            }
        }
        [DataMember]
        [IgnoreDataMember]
        public float ArticleCostValue
        {
            get
            {
                return articleCostValue;
            }

            set
            {
                articleCostValue = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public ulong IdBasePriceList
        {
            get
            {
                return idBasePriceList;
            }

            set
            {
                idBasePriceList = value;
            }
        }
        [DataMember]
        [IgnoreDataMember]
        public DateTime CurrencyConversionDate
        {
            get
            {
                return currencyConversionDate;
            }

            set
            {
                currencyConversionDate = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public ulong IdCustomerPriceList
        {
            get
            {
                return idCustomerPriceList;
            }

            set
            {
                idCustomerPriceList = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public string RegionName
        {
            get
            {
                return regionName;
            }

            set
            {
                regionName = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public uint IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
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
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public byte IdCountry
        {
            get
            {
                return idCountry;
            }

            set
            {
                idCountry = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value;
            }
        }
        [DataMember]
        [IgnoreDataMember]
        public DateTime? CurrencyConversionDate_New
        {
            get
            {
                return currencyConversionDate_New;
            }

            set
            {
                currencyConversionDate_New = value;
            }
        }
        [DataMember]
        [IgnoreDataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
            }
        }

        [DataMember]
        [IgnoreDataMember]
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
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
            }

        }


        [DataMember]
        [IgnoreDataMember]
        public string DatabaseIP
        {
            get
            {
                return databaseIP;
            }
            set
            {
                databaseIP = value;
            }
        }
        #endregion
    }
}
