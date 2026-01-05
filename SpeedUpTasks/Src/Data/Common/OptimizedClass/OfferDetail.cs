using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class OfferDetail
    {
        #region Fields
        Int64 idOffer;
        string code;
        string description;
        string siteName;
        string countryName;
        Int32 connectPlantId;
        Int32 idCustomer;
        string customerName;
        double value;
        string currName;
        string currSymbol;
        Int64 idCarProject;
        DateTime? offerExpectedDate;
        Int32 idCarOEM;
        string carOEMName;
        Int64 idStatus;
        string geosStatusName;
        string saleStatusName;
        Int64? saleStatusIdImage;
        Int64 numberOfOffers;
        Int32 currentMonth;
        Int32 currentYear;
        string saleStatusHTMLColor;
        #endregion

        #region Properties
        [DataMember]
        public long IdOffer
        {
            get
            {
                return idOffer;
            }

            set
            {
                idOffer = value;
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
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
            }
        }

        [DataMember]
        public int ConnectPlantId
        {
            get
            {
                return connectPlantId;
            }

            set
            {
                connectPlantId = value;
            }
        }

        [DataMember]
        public int IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
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
            }
        }

        [DataMember]
        public double Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        [DataMember]
        public string CurrName
        {
            get
            {
                return currName;
            }

            set
            {
                currName = value;
            }
        }

        [DataMember]
        public string CurrSymbol
        {
            get
            {
                return currSymbol;
            }

            set
            {
                currSymbol = value;
            }
        }

        [DataMember]
        public long IdCarProject
        {
            get
            {
                return idCarProject;
            }

            set
            {
                idCarProject = value;
            }
        }

        [DataMember]
        public DateTime? OfferExpectedDate
        {
            get
            {
                return offerExpectedDate;
            }

            set
            {
                offerExpectedDate = value;
            }
        }

        [DataMember]
        public int IdCarOEM
        {
            get
            {
                return idCarOEM;
            }

            set
            {
                idCarOEM = value;
            }
        }

        [DataMember]
        public string CarOEMName
        {
            get
            {
                return carOEMName;
            }

            set
            {
                carOEMName = value;
            }
        }

        [DataMember]
        public long IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
            }
        }

        [DataMember]
        public string GeosStatusName
        {
            get
            {
                return geosStatusName;
            }

            set
            {
                geosStatusName = value;
            }
        }

        [DataMember]
        public string SaleStatusName
        {
            get
            {
                return saleStatusName;
            }

            set
            {
                saleStatusName = value;
            }
        }

        [DataMember]
        public long? SaleStatusIdImage
        {
            get
            {
                return saleStatusIdImage;
            }

            set
            {
                saleStatusIdImage = value;
            }
        }

        [DataMember]
        public long NumberOfOffers
        {
            get
            {
                return numberOfOffers;
            }

            set
            {
                numberOfOffers = value;
            }
        }

        [DataMember]
        public Int32 CurrentMonth
        {
            get
            {
                return currentMonth;
            }

            set
            {
                currentMonth = value;
            }
        }

        [DataMember]
        public Int32 CurrentYear
        {
            get
            {
                return currentYear;
            }

            set
            {
                currentYear = value;
            }
        }

        [DataMember]
        public string SaleStatusHTMLColor
        {
            get
            {
                return saleStatusHTMLColor;
            }

            set
            {
                saleStatusHTMLColor = value;
            }
        }
        #endregion
    }
}
