using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class BusinessUnitDetail
    {
        #region  Fields
        Int32 idLookupValue;
        string value;
        List<SalesStatusTypeDetail> salesStatusTypeDetail;
        double amount;
        byte idCurrency;
        double? percentage;
        object tag;
        Int64? idImage;
        string htmlColor;
        #endregion

        #region Properties
        [DataMember]
        public int IdLookupValue
        {
            get
            {
                return idLookupValue;
            }

            set
            {
                idLookupValue = value;
            }
        }

        [DataMember]
        public string Value
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
        public List<SalesStatusTypeDetail> SalesStatusTypeDetail
        {
            get
            {
                return salesStatusTypeDetail;
            }

            set
            {
                salesStatusTypeDetail = value;
            }
        }

        [DataMember]
        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        [DataMember]
        public byte IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
            }
        }

        [DataMember]
        public double? Percentage
        {
            get
            {
                return percentage;
            }

            set
            {
                percentage = value;
            }
        }
       

        [DataMember]
        public object Tag
        {
            get
            {
                return tag;
            }

            set
            {
                 tag = value;
            }
        }

        [DataMember]
        public long? IdImage
        {
            get
            {
                return idImage;
            }

            set
            {
                idImage = value;
            }
        }

        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
            }
        }
        #endregion
    }
}
