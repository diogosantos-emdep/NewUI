using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class SalesStatusTypeDetail: ModelBase,IDisposable
    {
        #region  Fields
        Int64 idSalesStatusType;
        string name;
        double totalAmount;
        byte idCurrency;
        string currencyName;
        Int64? idImage;
        string htmlColor;
        #endregion
        #region Properties
        [DataMember]
        public Int64 IdSalesStatusType
        {
            get
            {
                return idSalesStatusType;
            }

            set
            {
                idSalesStatusType = value;
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
            }
        }

        [DataMember]
        public double TotalAmount
        {
            get
            {
                return totalAmount;
            }

            set
            {
                totalAmount = value;
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
        public string CurrencyName
        {
            get
            {
                return currencyName;
            }

            set
            {
                currencyName = value;
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
