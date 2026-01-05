using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class WarehouseCustomer
    {
        #region  Fields
        Int32 idCustomer;
        string alias;
        double amount;
        string saleStatusHTMLColor;
        #endregion
        #region Properties
        [DataMember]
        public Int32 IdCustomer
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
        public string Alias
        {
            get
            {
                return alias;
            }

            set
            {
                alias = value;
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
