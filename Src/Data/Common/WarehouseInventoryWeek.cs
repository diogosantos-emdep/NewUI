using Emdep.Geos.Data.Common.WMS;
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
    public class WarehouseInventoryWeek
    {
        #region  Fields
        Int64 stockWeek;
        double amount;
        List<ArticleMaterialType> articleMaterialType;
        #endregion
        #region Properties
        [DataMember]
        public Int64 StockWeek
        {
            get
            {
                return stockWeek;
            }

            set
            {
                stockWeek = value;
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
        public List<ArticleMaterialType> ArticleMaterialType
        {
            get
            {
                return articleMaterialType;
            }

            set
            {
                articleMaterialType = value;
            }
        }
        #endregion
    }
}
